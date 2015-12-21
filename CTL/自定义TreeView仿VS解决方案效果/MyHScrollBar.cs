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
using System.Threading;

/* DataGridView要注意事件：
 *  1.列增加
 *  2.列减少
 *  3.列宽的变化
 *  4.列头的宽度变化
 *  5.大小变化
 *  
 * TreeView要注意的事件
 *  1.节点名称的修改
 *  2.节点的展开收缩
 *  3.大小改变
 */

namespace 自定义TreeView仿VS解决方案效果
{
    //http://blog.csdn.net/tdgx2004/article/details/5864784
    [Designer(typeof(ScrollbarControlDesigner))]
    public partial class MyHScrollBar : UserControl
    {
        #region 属性
        /// <summary>
        /// 轨道颜色
        /// </summary>
        protected Color moChannelColor = Color.Empty;
        /// <summary>
        /// 滑块的颜色
        /// </summary>
        protected Color moThumbColor = Color.Empty;

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
        protected int moThumbLeft = 0;
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
        public MyHScrollBar()
        {
            InitializeComponent();

            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            moChannelColor = Color.FromArgb(225, 225, 225);
            moThumbColor = Color.FromArgb(124, 131, 135);

            this.Height = 8;

            customScrollInfo = new CustomScroll();
            customScrollInfo.TrackHeight = this.Height;
            customScrollInfo.DisplayHeight = customScrollInfo.TrackHeight;
            customScrollInfo.DisplayRectangleHeight = customScrollInfo.TrackHeight;

            base.MinimumSize = new Size(minSize, this.Height);
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
                        dgv.SizeChanged += dgv_SizeChanged;
                        dgv.CellStateChanged += dgv_CellStateChanged;
                        dgv.ColumnAdded += dgv_ColumnAdded;
                        dgv.ColumnRemoved += dgv_ColumnRemoved;
                        dgv.ColumnWidthChanged += dgv_ColumnWidthChanged;
                        dgv.RowHeadersWidthChanged += dgv_RowHeadersWidthChanged;
                        dgv.RowHeaderMouseClick += dgv_RowHeaderMouseClick;
                    }
                    else if (value.GetType() == typeof(TreeViewEx))
                    {
                        value.MouseWheel += tv_MouseWheel;
                        value.SizeChanged += tv_SizeChanged;
                        value.Click += tv_Click;

                        TreeViewEx tv = value as TreeViewEx;
                        tv.AfterExpand += tv_AfterExpand;
                        tv.AfterCollapse += tv_AfterCollapse;
                        tv.AfterSelect += tv_AfterSelect;
                    }
                }
            }
        }

        #endregion
        #region TreeView
        void tv_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeViewEx tv = sender as TreeViewEx;
            this.Value = tv.HorizontalScrollValue;
        }
        void tv_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            tv_SizeChanged(sender, e);
        }

        void tv_AfterExpand(object sender, TreeViewEventArgs e)
        {
            tv_SizeChanged(sender, e);
        }
        void tv_Click(object sender, EventArgs e)
        {
            TreeViewEx tv = sender as TreeViewEx;
            this.Value = tv.HorizontalScrollValue;
        }
        void tv_SizeChanged(object sender, EventArgs e)
        {
            //判断当前展开的节点有没有超出显示的宽度
            TreeViewEx tv = sender as TreeViewEx;
            int displayRectangleWidth = tv.Width;
            tv_HScrollBarVisible(tv.Nodes, tv.HorizontalScrollValue, ref displayRectangleWidth);

            int disWeight = tv.Width;
            if (tv.VerticalScrollVisible)
            {
                disWeight -= SystemInformation.VerticalScrollBarWidth;
            }

            UpdateScrollbar(tv.HorizontalScrollVisible, disWeight, displayRectangleWidth, tv.HorizontalScrollValue, tv.ItemHeight * 3, tv.ItemHeight);
        }
        void tv_MouseWheel(object sender, MouseEventArgs e)
        {
            Thread t = new Thread(() =>
            {
                this.Invoke((MethodInvoker)delegate
                {//执行完后才能得到滚动的值，所有这里用异步的方式去解决这个问题
                    //VerticalScrollValue 返回的是移动的行数
                    TreeViewEx tv = sender as TreeViewEx;
                    if (this.Value != tv.HorizontalScrollValue)
                    {
                        this.Value = tv.HorizontalScrollValue;
                        tv.Refresh();
                    }
                });
            });
            t.Start();

        }

        private void tv_HScrollBarVisible(TreeNodeCollection tnc, int scrollValue, ref int maxWidth)
        {
            foreach (TreeNode item in tnc)
            {
                if (item.Bounds.Width + item.Bounds.Left + scrollValue > item.TreeView.Width)
                {
                    if (maxWidth < item.Bounds.Width + item.Bounds.Left + scrollValue)
                    {
                        maxWidth = item.Bounds.Width + item.Bounds.Left + scrollValue;
                        if (item.TreeView.BorderStyle != System.Windows.Forms.BorderStyle.None)
                            maxWidth += 2;
                    }
                }
                if (item.Nodes.Count > 0 && item.IsExpanded)
                {
                    tv_HScrollBarVisible(item.Nodes, scrollValue, ref maxWidth);
                }
            }
        }
        #endregion

        #region 绑定 DataGridView

        #region DataGridView 的 点击事件
        void dgv_SizeChanged(object sender, EventArgs e)
        {
            
            DataGridView dgv = sender as DataGridView;
            int colWidth = 0;
            if (dgv.Columns.Count > 0)
                colWidth = dgv.Columns[0].Width;
            //int totalRowWidth = dgv.RowHeadersWidth;
            //foreach (DataGridViewColumn item in dgv.Columns)
            //{
            //    totalRowWidth += item.Width;
            //}
            //bool isVisible = dgv.Bounds.Height != dgv.DisplayRectangle.Height;

            int totalRowWidth = 0;
            bool isVisible = dgv_HScrollBarVisible(dgv, out totalRowWidth);
            dgv.SizeChanged -= dgv_SizeChanged;
            //if (isVisible)
            //{
            //    dgv.Height = dgv.Parent.Height + SystemInformation.HorizontalScrollBarHeight;
            //}
            //else
            //{
            //    dgv.Height = dgv.Parent.Height;
            //}
            //dgv.Height = dgv.Parent.Height;
            isVisible = dgv_HScrollBarVisible(dgv, out totalRowWidth);
            dgv.SizeChanged += dgv_SizeChanged;

            Thread t = new Thread(() =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    this.UpdateScrollbar(isVisible, dgv.DisplayRectangle.Width, totalRowWidth, dgv.HorizontalScrollingOffset, colWidth * 3, colWidth);
                });
            });
            t.Start();
        }

        private bool dgv_HScrollBarVisible(DataGridView control, out int totalRowWidth)
        {
            totalRowWidth = control.RowHeadersWidth;
            foreach (DataGridViewColumn item in control.Columns)
            {
                if (!item.Visible) continue;
                totalRowWidth += item.Width;
            }
            int displayWidth = control.DisplayRectangle.Width;
            if (control.BorderStyle != System.Windows.Forms.BorderStyle.None)
            {
                displayWidth -= 2;
            }
            return displayWidth < totalRowWidth;
        }

        void dgv_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (dgv.HorizontalScrollingOffset != this.moValue)
            {
                this.Value = dgv.HorizontalScrollingOffset;
            }
        }
        void dgv_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            DataGridView dgv = sender as DataGridView;
            if (dgv.HorizontalScrollingOffset != this.moValue)
            {
                this.Value = dgv.HorizontalScrollingOffset;
            }
        }
        void dgv_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            dgv_SizeChanged(sender, e);
        }

        void dgv_ColumnRemoved(object sender, DataGridViewColumnEventArgs e)
        {
            dgv_SizeChanged(sender, e);
        }

        void dgv_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            dgv_SizeChanged(sender, e);
        }
        void dgv_RowHeadersWidthChanged(object sender, EventArgs e)
        {
            dgv_SizeChanged(sender, e);
        }
        #endregion

        #endregion

        #region 当内容增减时要调用此方法，重新计算滚动条信息
        /// <summary>
        /// 当内容增减时要调用此方法，重新计算滚动条信息
        /// </summary>
        /// <param name="isVisible">控件的滚动条显示隐藏</param>
        /// <param name="displayWidth">显示的高度</param>
        /// <param name="displayRectangleWidth">内容的高度</param>
        /// <param name="offset">控件滚动条的偏移量</param>
        /// <param name="largeChange">控件滚动的最大距离</param>
        /// <param name="smallChange">控件滚动的最小距离</param>
        private void UpdateScrollbar(bool isVisible, int displayWidth, int displayRectangleWidth, int offset, int largeChange, int smallChange)
        {
            if (this.moControl == null)
                return;

            this.Visible = isVisible;
            customScrollInfo.IsVisible = isVisible;
            customScrollInfo.Offset = offset;
            customScrollInfo.TrackHeight = this.Width;
            customScrollInfo.DisplayHeight = displayWidth;
            customScrollInfo.DisplayRectangleHeight = displayRectangleWidth;

            if (largeChange == 0)
                largeChange = 50;
            if (smallChange == 0)
                smallChange = 30;

            customScrollInfo.UpdateThumbHeight();
            if (displayRectangleWidth > 0)
            {
                moLargeChange = this.Height * largeChange / displayRectangleWidth;
                moSmallChange = this.Height * smallChange / displayRectangleWidth;
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
            this.Invalidate(true);
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
                dgv_SizeChanged(this.moControl, null);
            }
            else if (this.moControl.GetType() == typeof(TreeViewEx))
            {
                tv_SizeChanged(this.moControl, null);
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
                moThumbLeft = customScrollInfo.GetScrollOffsetY(value);
                lastmoThumbLeftValue = moThumbLeft;
                if (moThumbLeft == 0)
                    moValue = 0;
                Invalidate();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Skin"), Description("Channel Color")]
        public Color ChannelColor
        {
            get { return moChannelColor; }
            set { moChannelColor = value; }
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
            e.Graphics.FillRectangle(oWhiteBrush, moThumbLeft, 0, customScrollInfo.ThumbHeight, this.Height);
            base.OnPaint(e);
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

            int nLeft = moThumbLeft;
            Point ptPoint = this.PointToClient(Cursor.Position);
            //滑块
            Rectangle thumbrect = new Rectangle(new Point(nLeft, 0), new Size(customScrollInfo.ThumbHeight, this.Height));
            //轨道
            Rectangle trackrec = new Rectangle(new Point(0, 0), new Size(customScrollInfo.TrackHeight, this.Height));
            if (thumbrect.Contains(ptPoint))
            {
                //hit the thumb
                nClickPoint = (ptPoint.X - nLeft);
                this.moThumbDown = true;
            }
            else if (trackrec.Contains(ptPoint))
            {
                this.moTrackDown = true;
                //判断当前鼠标点击的位置与 滑块的位置差
                nClickPoint = ptPoint.X;
                bool isLeft = (ptPoint.X - nLeft) > 0 ? false : true;
                MoveThumbByClick(isLeft);
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
                MoveThumb(e.X);
            }
        }
        #endregion

        #region 移动滑块
        /// <summary>
        /// 点击轨道 移动滑块
        /// </summary>
        /// <param name="isLeft"></param>
        private void MoveThumbByClick(bool isLeft)
        {
            MoveThumb(isLeft);
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 100;
            timer.Tick += timer_Tick;
            timer.Tag = isLeft;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            System.Windows.Forms.Timer timer = sender as System.Windows.Forms.Timer;
            bool isLeft = bool.Parse(timer.Tag.ToString());

            if ((DateTime.Now - nClickDateTime).TotalMilliseconds > 800)
            {
                MoveThumb(isLeft);
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
        private void MoveThumb(bool isLeft)
        {
            if (!moMouseDown)
                return;
            int nRealRange = 0;
            int nPixelRange = customScrollInfo.TrackHeight - customScrollInfo.ThumbHeight;
            if (moTrackDown)
            {
                int nSpot = nClickPoint;//除去上箭头的高度
                if (isLeft)
                {
                    nRealRange = moThumbLeft - nSpot;//拇指顶端与点击处的距离
                }
                else
                {
                    nRealRange = nSpot - moThumbLeft - customScrollInfo.ThumbHeight; // 拇指底端与点击处的距离
                }
            }
            if (nRealRange > 0)
            {
                if (nPixelRange > 0)
                {
                    if (isLeft)
                    {
                        if ((moThumbLeft - SmallChange) < 0)
                            moThumbLeft = 0;
                        else
                            moThumbLeft -= SmallChange;
                    }
                    else
                    {
                        if ((moThumbLeft + SmallChange) > nPixelRange)
                            moThumbLeft = nPixelRange;
                        else
                            moThumbLeft += SmallChange;
                    }
                    MoveThumbGetValue();
                }
            }
        }

        private void MoveThumbMouseWheel(bool isLeft)
        {
            int nRealRange = 0;
            int nPixelRange = customScrollInfo.TrackHeight - customScrollInfo.ThumbHeight;
            if (isLeft)
            {
                nRealRange = moThumbLeft;//拇指顶端与点击处的距离
            }
            else
            {
                nRealRange = customScrollInfo.TrackHeight - moThumbLeft - customScrollInfo.ThumbHeight; // 拇指底端与点击处的距离
            }
            if (nRealRange > 0)
            {
                if (nPixelRange > 0)
                {
                    if (isLeft)
                    {
                        if ((moThumbLeft - SmallChange) < 0)
                            moThumbLeft = 0;
                        else
                            moThumbLeft -= SmallChange;
                    }
                    else
                    {
                        if ((moThumbLeft + SmallChange) > nPixelRange)
                            moThumbLeft = nPixelRange;
                        else
                            moThumbLeft += SmallChange;
                    }
                    MoveThumbGetValue();
                }
            }
        }

        /// <summary>
        /// 鼠标拖动 移动滑块
        /// </summary>
        /// <param name="x"></param>
        private void MoveThumb(int x)
        {
            int nSpot = nClickPoint;
            int nPixelRange = customScrollInfo.TrackHeight - customScrollInfo.ThumbHeight;
            if (moThumbDown && nPixelRange > 0)
            {
                int nNewThumbTop = x - nSpot;

                if (nNewThumbTop < 0)
                {
                    moThumbLeft = nNewThumbTop = 0;
                }
                else if (nNewThumbTop > nPixelRange)
                {
                    moThumbLeft = nNewThumbTop = nPixelRange;
                }
                else
                {
                    moThumbLeft = x - nSpot;
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
            moValue = customScrollInfo.GetValue(moThumbLeft);

            #region 计算绑定控件 应该滚动的位置
            if (this.moControl != null)
            {
                if (this.moControl.GetType() == typeof(DataGridView))
                {
                    Thread t = new Thread(() =>
                    {
                        this.Invoke((MethodInvoker)delegate
                        {//滚动有延迟，所以用线程
                            DataGridView dgv = this.moControl as DataGridView;
                            dgv.HorizontalScrollingOffset = moValue;
                        });
                    });
                    t.Start();
                }
                else if (this.moControl.GetType() == typeof(TreeViewEx))
                {
                    TreeViewEx tv = this.moControl as TreeViewEx;
                    tv.HorizontalScrollValue = moValue;
                }
            }
            #endregion

            if (moThumbLeft - lastmoThumbLeftValue != 0)
            {
                if (moThumbLeft - lastmoThumbLeftValue > 0)
                    isMoveLeft = false;
                else
                    isMoveLeft = true;

                if (ValueChanged != null)
                    ValueChanged(this, new EventArgs());

                if (Scroll != null)
                    Scroll(this, new EventArgs());

            }
            lastmoThumbLeftValue = moThumbLeft;
            Invalidate();
        }
        #endregion

        #region 移动的方向
        /// <summary>
        /// 上次移动的位置
        /// </summary>
        private int lastmoThumbLeftValue = 0;


        /// <summary>
        /// 上一次的值
        /// </summary>
        public int OldValue
        {
            get
            {
                return customScrollInfo.GetValue(lastmoThumbLeftValue);
            }
        }
        private bool isMoveLeft = false;

        /// <summary>
        /// 移动的方向
        /// </summary>
        public bool IsMoveLeft
        {
            get { return isMoveLeft; }
        }
        #endregion

        #endregion



    }
}
