using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Runtime.InteropServices;

namespace 自定义Panel列表V2
{
    //http://blog.csdn.net/tdgx2004/article/details/5864784
    [Designer(typeof(ScrollbarControlDesigner))]
    public partial class MyVScrollBar : UserControl
    {
        #region 属性
        /// <summary>
        /// 轨道颜色
        /// </summary>
        private Color moChannelColor = Color.Transparent;

        private Color moChannelDefaultColor = Color.Transparent;
        /// <summary>
        /// 轨道颜色
        /// </summary>
        protected Color moChannelMouseEnterColor = Color.FromArgb(250, 250, 250);
        /// <summary>
        /// 滑块的颜色
        /// </summary>
        protected Color moThumbColor = Color.FromArgb(193, 193, 193);

        protected int moLargeChange = 10;
        protected int moSmallChange = 1;
        protected int moValue = 0;

        private int nClickPoint;
        /// <summary>
        /// 点击的时间，用来判断是鼠标单击还是 长按
        /// </summary>
        private DateTime nClickDateTime;
        /// <summary>
        /// 滑块到顶部的偏移量
        /// </summary>
        protected int moThumbTop = 0;
        protected bool moAutoSize = false;
        private bool moThumbDown = false;
        private bool moThumbDragging = false;
        /// <summary>
        /// 点击轨道
        /// </summary>
        private bool moTrackDown = false;
        /// <summary>
        /// 鼠标是否按下
        /// </summary>
        private bool moMouseDown = false;

        const int minSize = 15;
        /// <summary>
        /// 滚动条对象
        /// </summary>
        CustomScroll customScrollInfo;

        /// <summary>
        /// 要绑定的控件
        /// </summary>
        Control moControl = null;
        #endregion

        #region 事件
        public new event EventHandler Scroll = null;
        public event EventHandler ValueChanged = null;
        #endregion

        #region 构造函数
        public MyVScrollBar()
        {
            InitializeComponent();

            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            customScrollInfo = new CustomScroll();
            customScrollInfo.TrackHeight = this.Height;
            customScrollInfo.DisplayHeight = customScrollInfo.TrackHeight;
            customScrollInfo.DisplayRectangleHeight = customScrollInfo.TrackHeight;

            this.Width = 8;
            base.MinimumSize = new Size(this.Width, minSize);
        }
        #endregion

        #region 公布属性

        #region 绑定需要滚动条的控件

        #region 绑定需要滚动条的控件
        /// <summary>
        /// 绑定需要滚动条的控件
        /// </summary>
        public Control BindControl
        {
            get { return this.moControl; }
            set
            {
                this.moControl = value;
                if (value != null)
                {
                    if (value.GetType() == typeof(DataGridView))
                    {
                        DataGridView dgv = value as DataGridView;
                        dgv.RowStateChanged += dgv_RowStateChanged;
                        dgv.SizeChanged += dgv_SizeChanged;
                    }
                    else if (value.GetType() == typeof(TreeView))
                    {
                        value.MouseWheel += tv_MouseWheel;
                        value.SizeChanged += tv_SizeChanged;
                        value.Click += tv_Click;
                    }
                    else if (value.GetType() == typeof(RichTextBox))
                    {
                        value.MouseWheel += rtb_MouseWheel;
                        value.SizeChanged += rtb_SizeChanged;
                    }
                }
            }
        }

        #region RichTextBox
        void rtb_SizeChanged(object sender, EventArgs e)
        {
        }

        void rtb_MouseWheel(object sender, MouseEventArgs e)
        {
            MoveThumbMouseWheel(e.Delta > 0);
        } 
        #endregion


        #endregion
        #region TreeView
        void tv_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void tv_SizeChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void tv_MouseWheel(object sender, MouseEventArgs e)
        {
            TreeView tv = sender as TreeView;
            //this.Value = 
        }
        #endregion

        #region Panel 的点击事件

        void dpc_Click(object sender, EventArgs e)
        {
            DataPanelContainer pnl = sender as DataPanelContainer;
            this.Value = pnl.VScrollValue;
        }
        void dpc_MouseWheel(object sender, MouseEventArgs e)
        {
            MoveThumbMouseWheel(e.Delta > 0);
        }
        #endregion

        #region 绑定 DataGridView

        #region DataGridView 的 点击事件
        void dgv_SizeChanged(object sender, EventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            int rowHeight = 23;
            int totalRowHeight = 0;
            foreach (DataGridViewRow item in dgv.Rows)
            {
                totalRowHeight += item.Height;
            }
            bool isVisible = dgv.DisplayedRowCount(false) != dgv.RowCount;
            this.UpdateScrollbar(isVisible, dgv.Height, totalRowHeight, dgv.VerticalScrollingOffset, rowHeight * 3, rowHeight);
        }

        void dgv_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {//Click 会改变 Row 的状态
            DataGridView dgv = sender as DataGridView;
            if (dgv.VerticalScrollingOffset != this.moValue)
            {
                this.Value = dgv.VerticalScrollingOffset;
            }
        }
        #endregion

        #region 计算 DataGridView 根据偏移量 得出行号
        /// <summary>
        /// 计算 DataGridView 根据偏移量 得出行号
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private int CalcRowIndex(DataGridView dgv, int offset)
        {
            //偏移量 就等于 内容移动的高度
            int totalHeight = 0;
            int rowIndex = 0;
            foreach (DataGridViewRow item in dgv.Rows)
            {
                if (totalHeight - offset >= 0 && totalHeight - offset <= item.Height)
                {
                    rowIndex = item.Index;
                    break;
                }
                totalHeight += item.Height;
            }
            return rowIndex;
        }
        #endregion

        #endregion

        #region 当内容增减时要调用此方法，重新计算滚动条信息
        /// <summary>
        /// 当内容增减时要调用此方法，重新计算滚动条信息
        /// </summary>
        /// <param name="isVisible">控件的滚动条显示隐藏</param>
        /// <param name="displayHeight">显示的高度</param>
        /// <param name="displayRectangleHeight">内容的高度</param>
        /// <param name="offset">控件滚动条的偏移量</param>
        /// <param name="largeChange">控件滚动的最大距离</param>
        /// <param name="smallChange">控件滚动的最小距离</param>
        private void UpdateScrollbar(bool isVisible, int displayHeight, int displayRectangleHeight, int offset, int largeChange, int smallChange)
        {
            if (this.moControl == null)
                return;

            this.Visible = isVisible;

            customScrollInfo.IsVisible = isVisible;
            customScrollInfo.Offset = offset;
            customScrollInfo.TrackHeight = this.Height;
            customScrollInfo.DisplayHeight = displayHeight;
            customScrollInfo.DisplayRectangleHeight = displayRectangleHeight;

            if (largeChange == 0)
                largeChange = 50;
            if (smallChange == 0)
                smallChange = 30;

            customScrollInfo.LargeChange = largeChange;
            customScrollInfo.SmallChange = smallChange;
            customScrollInfo.UpdateThumbHeight();
            if (displayRectangleHeight > 0)
            {
                moLargeChange = this.Height * largeChange / displayRectangleHeight;
                moSmallChange = this.Height * smallChange / displayRectangleHeight;
                if (moSmallChange == 0)
                { //内容太多
                    moSmallChange = 2;
                    moLargeChange = 4;
                }
            }
            //有时候赋值后没有变过来
            if (this.Visible != isVisible)
                this.Visible = isVisible;
            this.Value = offset;
            this.Invalidate();
        }
        /// <summary>
        /// 当内容更新后，要调用此方法
        /// </summary>
        public void UpdateScrollbar()
        {
            if (this.moControl == null)
                return;
            if (this.moControl.GetType() == typeof(DataGridView))
            {
                DataGridView control = this.moControl as DataGridView;
                int rowHeight = 0;
                int totalRowHeight = 0;
                foreach (DataGridViewRow item in control.Rows)
                {
                    if (rowHeight == 0)
                        rowHeight = item.Height;
                    totalRowHeight += item.Height;
                }
                bool isVisible = control.DisplayedRowCount(false) != control.RowCount;
                this.UpdateScrollbar(isVisible, control.Height, totalRowHeight, control.VerticalScrollingOffset, rowHeight * 3, rowHeight);
            }
            else if (this.moControl.GetType() == typeof(DataPanelContainer))
            {
                DataPanelContainer control = this.moControl as DataPanelContainer;
                UpdateScrollbar(control.VScrollVisible, control.Height, control.DisplayRectangleHeight, control.VScrollValue, control.LargeChange, control.SmallChange);
            }
        }
        #endregion

        #endregion

        #region 可编辑属性

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("LargeChange")]
        public int LargeChange
        {
            get { return moLargeChange; }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("SmallChange")]
        public int SmallChange
        {
            get { return moSmallChange; }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("Value")]
        public int Value
        {
            get { return moValue; }
            set
            {
                moValue = value;
                moThumbTop = customScrollInfo.GetScrollOffsetY(value);
                lastmoThumbTopValue = moThumbTop;
                if (moThumbTop == 0)
                    moValue = 0;
                Invalidate();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Channel Color")]
        public Color ChannelColor
        {
            get { return moChannelMouseEnterColor; }
            set { moChannelMouseEnterColor = value; }
        }


        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Thumb Color")]
        public Color ThumbColor
        {
            get { return moThumbColor; }
            set { moThumbColor = value; }
        }

        #endregion

        #endregion

        #region override
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            //draw channel
            Brush oBrush = new SolidBrush(moChannelColor);
            e.Graphics.FillRectangle(oBrush, new Rectangle(0, 0, this.Width, this.Height));

            //draw thumb
            Brush oWhiteBrush = new SolidBrush(moThumbColor);
            e.Graphics.FillRectangle(oWhiteBrush, 0, moThumbTop, this.Width, customScrollInfo.ThumbHeight);
        }
        #endregion

        #region API
        [DllImport("user32.dll")]

        private static extern IntPtr GetForegroundWindow();

        #endregion

        #region InitializeComponent
        /// <summary>
        /// 初始化
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CustomScrollbar
            // 
            this.Name = "VScrollBar";
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CustomScrollbar_MouseUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CustomScrollbar_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CustomScrollbar_MouseMove);
            this.MouseEnter += MyVScrollBar_MouseEnter;
            this.MouseLeave += MyVScrollBar_MouseLeave;
            this.SizeChanged += MyVScrollBar_SizeChanged;
            this.ResumeLayout(false);
        }


        void MyVScrollBar_SizeChanged(object sender, EventArgs e)
        {
            UpdateScrollbar(customScrollInfo.IsVisible, customScrollInfo.DisplayHeight, customScrollInfo.DisplayRectangleHeight, customScrollInfo.Offset, customScrollInfo.LargeChange, customScrollInfo.SmallChange);
        }
        #region MouseEnter
        void MyVScrollBar_MouseEnter(object sender, EventArgs e)
        {
            if (this.moControl != null)
            {
                if (moControl.TopLevelControl.Handle == GetForegroundWindow())
                {
                    this.moControl.Focus();
                }
            }
            moChannelColor = moChannelMouseEnterColor;
            this.Refresh();
        }
        void MyVScrollBar_MouseLeave(object sender, EventArgs e)
        {
            moChannelColor = moChannelDefaultColor;
            this.Refresh();
        }
        #endregion
        #endregion

        #region MouseDown
        /// <summary>
        /// MouseDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomScrollbar_MouseDown(object sender, MouseEventArgs e)
        {
            //点击时间
            nClickDateTime = DateTime.Now;
            moMouseDown = true;

            int nTop = moThumbTop;
            Point ptPoint = this.PointToClient(Cursor.Position);
            //滑块
            Rectangle thumbrect = new Rectangle(new Point(0, nTop), new Size(this.Width, customScrollInfo.ThumbHeight));
            //轨道
            Rectangle trackrec = new Rectangle(new Point(0, 0), new Size(this.Width, customScrollInfo.TrackHeight));
            if (thumbrect.Contains(ptPoint))
            {
                //hit the thumb
                nClickPoint = (ptPoint.Y - nTop);
                this.moThumbDown = true;
            }
            else if (trackrec.Contains(ptPoint))
            {
                this.moTrackDown = true;
                //判断当前鼠标点击的位置与 滑块的位置差
                nClickPoint = ptPoint.Y;
                bool isUp = (ptPoint.Y - nTop) > 0 ? false : true;
                MoveThumbByClick(isUp);
            }
        }
        #endregion

        #region MouseUp
        /// <summary>
        /// MouseUp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomScrollbar_MouseUp(object sender, MouseEventArgs e)
        {
            this.moThumbDown = false;
            this.moThumbDragging = false;
            this.moMouseDown = false;
            this.moTrackDown = false;
            if (this.moControl != null)
                this.moControl.Focus();
        }
        #endregion

        #region MouseMove
        /// <summary>
        /// MouseMove
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomScrollbar_MouseMove(object sender, MouseEventArgs e)
        {
            if (moThumbDown == true)
            {
                this.moThumbDragging = true;
            }
            if (this.moThumbDragging)
            {
                MoveThumb(e.Y);
            }
        }
        #endregion

        #region 移动滑块
        /// <summary>
        /// 点击轨道 移动滑块
        /// </summary>
        /// <param name="isUp"></param>
        private void MoveThumbByClick(bool isUp)
        {
            MoveThumb(isUp);
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 100;
            timer.Tick += timer_Tick;
            timer.Tag = isUp;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            System.Windows.Forms.Timer timer = sender as System.Windows.Forms.Timer;
            bool isUp = bool.Parse(timer.Tag.ToString());

            if ((DateTime.Now - nClickDateTime).TotalMilliseconds > 800)
            {
                MoveThumb(isUp);
                //到最高或最低退出
                if (moValue == 0 || !moMouseDown)
                    timer.Stop();
            }
            if (!moMouseDown)
                timer.Stop();
        }
        /// <summary>
        /// 点击轨道 移动滑块
        /// </summary>
        private void MoveThumb(bool isUp)
        {
            if (!moMouseDown)
                return;
            int nRealRange = 0;
            int nPixelRange = customScrollInfo.TrackHeight - customScrollInfo.ThumbHeight;
            if (moTrackDown)
            {
                int nSpot = nClickPoint;//除去上箭头的高度
                if (isUp)
                {
                    nRealRange = moThumbTop - nSpot;//拇指顶端与点击处的距离
                }
                else
                {
                    nRealRange = nSpot - moThumbTop - customScrollInfo.ThumbHeight; // 拇指底端与点击处的距离
                }
            }
            if (nRealRange > 0)
            {
                if (nPixelRange > 0)
                {
                    if (isUp)
                    {
                        if ((moThumbTop - SmallChange) < 0)
                            moThumbTop = 0;
                        else
                            moThumbTop -= SmallChange;
                    }
                    else
                    {
                        if ((moThumbTop + SmallChange) > nPixelRange)
                            moThumbTop = nPixelRange;
                        else
                            moThumbTop += SmallChange;
                    }
                    MoveThumbGetValue();
                }
            }
        }

        private void MoveThumbMouseWheel(bool isUp)
        {
            int nRealRange = 0;
            int nPixelRange = customScrollInfo.TrackHeight - customScrollInfo.ThumbHeight;
            if (isUp)
            {
                nRealRange = moThumbTop;//拇指顶端与点击处的距离
            }
            else
            {
                nRealRange = customScrollInfo.TrackHeight - moThumbTop - customScrollInfo.ThumbHeight; // 拇指底端与点击处的距离
            }
            if (nRealRange > 0)
            {
                if (nPixelRange > 0)
                {
                    if (isUp)
                    {
                        if ((moThumbTop - SmallChange) < 0)
                            moThumbTop = 0;
                        else
                            moThumbTop -= SmallChange;
                    }
                    else
                    {
                        if ((moThumbTop + SmallChange) > nPixelRange)
                            moThumbTop = nPixelRange;
                        else
                            moThumbTop += SmallChange;
                    }
                    MoveThumbGetValue();
                }
            }
        }

        /// <summary>
        /// 鼠标拖动 移动滑块
        /// </summary>
        /// <param name="y"></param>
        private void MoveThumb(int y)
        {
            int nSpot = nClickPoint;
            int nPixelRange = customScrollInfo.TrackHeight - customScrollInfo.ThumbHeight;
            if (moThumbDown && nPixelRange > 0)
            {
                int nNewThumbTop = y - nSpot;

                if (nNewThumbTop < 0)
                {
                    moThumbTop = nNewThumbTop = 0;
                }
                else if (nNewThumbTop > nPixelRange)
                {
                    moThumbTop = nNewThumbTop = nPixelRange;
                }
                else
                {
                    moThumbTop = y - nSpot;
                }

                MoveThumbGetValue();
            }
        }

        #region 计算要移动的值
        /// <summary>
        /// 计算要移动的值
        /// </summary>
        /// <param name="nPixelRange"></param>
        private void MoveThumbGetValue()
        {
            moValue = customScrollInfo.GetValue(moThumbTop);

            #region 计算绑定控件 应该滚动的位置
            if (this.moControl != null)
            {
                if (this.moControl.GetType() == typeof(DataGridView))
                {
                    DataGridView dgv = this.moControl as DataGridView;
                    dgv.FirstDisplayedScrollingRowIndex = CalcRowIndex(dgv, moValue);
                }
                else if (this.moControl.GetType() == typeof(DataPanelContainer))
                {
                    DataPanelContainer pnl = this.moControl as DataPanelContainer;
                    pnl.VScrollValue = moValue;
                }
            }
            #endregion

            if (moThumbTop - lastmoThumbTopValue != 0)
            {
                if (moThumbTop - lastmoThumbTopValue > 0)
                    isMoveUp = false;
                else
                    isMoveUp = true;

                if (ValueChanged != null)
                    ValueChanged(this, new EventArgs());

                if (Scroll != null)
                    Scroll(this, new EventArgs());

            }
            lastmoThumbTopValue = moThumbTop;
            Invalidate();
        }
        #endregion
        #region 移动的方向
        /// <summary>
        /// 上次移动的位置
        /// </summary>
        private int lastmoThumbTopValue = 0;


        /// <summary>
        /// 上一次的值
        /// </summary>
        public int OldValue
        {
            get
            {
                return customScrollInfo.GetValue(lastmoThumbTopValue);
            }
        }
        private bool isMoveUp = false;

        /// <summary>
        /// 移动的方向
        /// </summary>
        public bool IsMoveUp
        {
            get { return isMoveUp; }
        }
        #endregion

        #endregion
    }

    #region class ScrollbarControlDesigner
    internal class ScrollbarControlDesigner : System.Windows.Forms.Design.ControlDesigner
    {
        public override SelectionRules SelectionRules
        {
            get
            {
                SelectionRules selectionRules = base.SelectionRules;
                PropertyDescriptor propDescriptor = TypeDescriptor.GetProperties(this.Component)["AutoSize"];
                if (propDescriptor != null)
                {
                    bool autoSize = (bool)propDescriptor.GetValue(this.Component);
                    if (autoSize)
                    {
                        selectionRules = SelectionRules.Visible | SelectionRules.Moveable | SelectionRules.BottomSizeable | SelectionRules.TopSizeable;
                    }
                    else
                    {
                        selectionRules = SelectionRules.Visible | SelectionRules.AllSizeable | SelectionRules.Moveable;
                    }
                }
                return selectionRules;
            }
        }
    }

    #endregion

    #region 滚动条对象
    /// <summary>
    /// 滚动条信息
    /// </summary>
    internal struct CustomScroll
    {
        /// <summary>
        /// 滑块最小高度
        /// </summary>
        const int thumbMinHeight = 15;
        /// <summary>
        /// 滑块高度
        /// </summary>
        private int thumbHeight;

        /// <summary>
        /// 轨道高度
        /// </summary>
        public int TrackHeight { get; set; }
        /// <summary>
        /// 滑块高度
        /// </summary>
        public int ThumbHeight
        {
            get
            {
                return thumbHeight;
            }
        }

        /// <summary>
        /// 显示区域
        /// </summary>
        public float Rate { get; set; }

        /// <summary>
        /// 内容显示区域高度
        /// </summary>
        public int DisplayHeight { get; set; }
        /// <summary>
        /// 内容的实际范围
        /// </summary>
        public int DisplayRectangleHeight { get; set; }
        /// <summary>
        /// 控件滚动条的显示隐藏
        /// </summary>
        public bool IsVisible { get; set; }
        /// <summary>
        /// 控件滚动条的偏移量
        /// </summary>
        public int Offset { get; set; }

        public int LargeChange { get; set; }

        public int SmallChange { get; set; }


        private bool IsVisibleScroll
        {
            get
            {
                return DisplayRectangleHeight - DisplayHeight > 0;
            }
        }

        /// <summary>
        /// 获取滚动条的位置
        /// </summary>
        /// <returns></returns>
        public int GetScrollOffsetY(int offsetY)
        {
            /* 由 P:(G-B)=O:(H-D) 得 P=O(G-B)/(H-D)  
             * 滑块的偏移(P) = 主体内容的偏移量(O) * (轨道高度(G) - 滑块高度(B)) / (内容的实际高度(H) - 显示区域的高度(D))
             */
            float rlt = 0;
            if (IsVisibleScroll)
                rlt = offsetY * (TrackHeight - ThumbHeight) / (DisplayRectangleHeight - DisplayHeight);
            return (int)rlt;
        }
        /// <summary>
        /// 获取当前显示内容的偏移量
        /// </summary>
        /// <returns></returns>
        public int GetValue(int offsetY)
        {
            /* 由 P:(G-B)=O:(H-D) 得 O=P(H-D)/(G-B)  
             * 主体内容的偏移量(O) = 滑块的偏移(P) * (内容的实际高度(H) - 显示区域的高度(D) / (轨道高度(G) - 滑块高度(B))
             */
            float rlt = 0;
            if (IsVisibleScroll)
                rlt = offsetY * (DisplayRectangleHeight - DisplayHeight) / (TrackHeight - ThumbHeight);
            return (int)rlt;
        }
        /// <summary>
        /// 重新计算滑块高度
        /// </summary>
        public void UpdateThumbHeight()
        {
            /* 由 B:G=D:H 得 B=GD/H 滑块高度(B) = 轨道高度(G) * 显示区域的高度(D) / 内容的实际高度(H)
                */
            float B = thumbMinHeight;
            if (IsVisibleScroll)
                B = TrackHeight * DisplayHeight / DisplayRectangleHeight;
            if (B < thumbMinHeight)
            {
                B = thumbMinHeight;
            }
            thumbHeight = (int)B;
        }
    }
    #endregion
}
