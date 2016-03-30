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
using Win32API;

/* DataGridView要注意事件：
 *  1.行增加
 *  2.行减少
 *  3.行高的变化
 *  4.大小变化
 * 
 * TreeView要注意的事件
 *  1.增加节点
 *  2.删除节点
 *  3.节点的展开收缩
 *  4.大小改变
 */


namespace 自定义TreeView仿VS解决方案效果
{
    //http://blog.csdn.net/tdgx2004/article/details/5864784
    [Designer(typeof(ScrollbarControlDesigner))]
    public partial class MyVScrollBar : UserControl
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

            moChannelColor = Color.FromArgb(225, 225, 225);
            moThumbColor = Color.FromArgb(124, 131, 135);

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
                        dgv.RowHeightChanged += dgv_RowHeightChanged;
                        dgv.ColumnHeadersHeightChanged += dgv_ColumnHeadersHeightChanged;
                        dgv.RowHeadersWidthChanged += dgv_RowHeadersWidthChanged;
                        //dgv.SizeChanged += dgv_SizeChanged;
                        dgv.RowsAdded += dgv_RowsAdded;
                        dgv.RowsRemoved += dgv_RowsRemoved;

                        Panel pnl = dgv.Parent as Panel;
                        pnl.SizeChanged += pnl_SizeChanged;
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
                    else if (value.GetType() == typeof(RichTextBox))
                    {
                        value.MouseWheel += rtb_MouseWheel;
                        value.SizeChanged += rtb_SizeChanged;
                        value.Click += rtb_Click;

                        RichTextBox rtb = value as RichTextBox;
                        rtb.ContentsResized += rtb_ContentsResized;
                    }
                }
            }
        }

        void rtb_Click(object sender, EventArgs e)
        {
        }

        void rtb_SizeChanged(object sender, EventArgs e)
        {
        }

        void rtb_MouseWheel(object sender, MouseEventArgs e)
        {
        }
        void rtb_ContentsResized(object sender, ContentsResizedEventArgs e)
        {
        }

        #endregion

        #region TreeView
        void tv_Click(object sender, EventArgs e)
        {
            TreeViewEx tv = sender as TreeViewEx;
            this.Value = tv.VerticalScrollValue * tv.ItemHeight;
        }
        void tv_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeViewEx tv = sender as TreeViewEx;
            this.Value = tv.VerticalScrollValue * tv.ItemHeight;
        }
        void tv_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            tv_SizeChanged(sender, e);
        }

        void tv_AfterExpand(object sender, TreeViewEventArgs e)
        {
            tv_SizeChanged(sender, e);
        }

        void tv_SizeChanged(object sender, EventArgs e)
        {
            TreeViewEx tv = sender as TreeViewEx;
            tv.HorizontalScrollVisible = false;
            int totalHeight = 0;
            NextNode(tv.Nodes, ref totalHeight);

            int disHeight = tv.Height;
            if (tv.HorizontalScrollVisible)//当出现水平滚动条s
                disHeight -= SystemInformation.HorizontalScrollBarHeight;
            //判断当前显示区域（不包含滚动条）是否是行的整数倍，不是则会有空白行
            if (disHeight % tv.ItemHeight != 0)
            {
                totalHeight += disHeight % tv.ItemHeight;
            }

            //tv.SizeChanged -= tv_SizeChanged;
            //if (tv.HorizontalScrollVisible)
            //    tv.Height = tv.Parent.Height + SystemInformation.HorizontalScrollBarHeight;
            //else
            //    tv.Height = tv.Parent.Height;

            //if (tv.VerticalScrollVisible)
            //    tv.Width = tv.Parent.Width + SystemInformation.VerticalScrollBarWidth;
            //else
            //    tv.Width = tv.Parent.Width;
            //tv.SizeChanged += tv_SizeChanged;

            UpdateScrollbar(tv.VerticalScrollVisible, disHeight, totalHeight, tv.VerticalScrollValue * tv.ItemHeight, tv.ItemHeight * 3, tv.ItemHeight);
        }

        private void NextNode(TreeNodeCollection tnc, ref int totalHeight)
        {
            foreach (TreeNode item in tnc)
            {
                totalHeight += item.TreeView.ItemHeight;
                if (item.Nodes.Count > 0 && item.IsExpanded)
                {
                    NextNode(item.Nodes, ref totalHeight);
                }
            }
        }

        void tv_MouseWheel(object sender, MouseEventArgs e)
        {
            Thread t = new Thread(() =>
            {
                this.Invoke((MethodInvoker)delegate
                {//执行完后才能得到滚动的值，所有这里用异步的方式去解决这个问题
                    //VerticalScrollValue 返回的是移动的行数
                    TreeViewEx tv = sender as TreeViewEx;
                    this.Value = tv.VerticalScrollValue * tv.ItemHeight;
                });
            });
            t.Start();
        }
        #endregion

        #region 绑定 DataGridView

        #region DataGridView 的 点击事件
        void dgv_SizeChanged(object sender, EventArgs e)
        {
            
            DataGridView dgv = sender as DataGridView;
            int rowHeight = 23;

            //int totalRowHeight = GetDgvSpaceHeight(dgv) + dgv.ColumnHeadersHeight;
            //foreach (DataGridViewRow item in dgv.Rows)
            //{
            //    totalRowHeight += item.Height;
            //}

            dgv.SizeChanged -= dgv_SizeChanged;

            //如果出现水平滚动条，显示的高度要加上滚动条的高度和间隙       
            int totalRowHeight = 0;
            //int totalRowWidth = 0;
            //bool hScrollVis = dgv_HScrollBarVisible(dgv, out totalRowWidth);//dgv.Bounds.Height != dgv.DisplayRectangle.Height;
            //if (hScrollVis)
            //{
            //    dgv.Height = dgv.Parent.Height + SystemInformation.HorizontalScrollBarHeight;
            //}
            //else
            //{
            //    if (totalRowHeight + SystemInformation.HorizontalScrollBarHeight <= dgv.Parent.Height)
            //        dgv.Height = dgv.Parent.Height;
            //}

            bool isVisible = dgv_VScrollBarVisible(dgv, out totalRowHeight);// dgv.Bounds.Width != dgv.DisplayRectangle.Width;
            //if (isVisible)
            //{
            //    dgv.Width = dgv.Parent.Width + SystemInformation.VerticalScrollBarWidth;
            //}
            //else
            //{
            //    //if (totalRowWidth + SystemInformation.VerticalScrollBarWidth <= dgv.Parent.Width)
            //    dgv.Width = dgv.Parent.Width;
            //}
            dgv.Width = dgv.Parent.Width;
            isVisible = dgv_VScrollBarVisible(dgv, out totalRowHeight);
            dgv.SizeChanged += dgv_SizeChanged;

            Thread t = new Thread(() =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    this.UpdateScrollbar(isVisible, dgv.DisplayRectangle.Height, totalRowHeight, dgv.VerticalScrollingOffset, rowHeight * 3, rowHeight);

                });
            });
            t.Start();
        }

        void pnl_SizeChanged(object sender, EventArgs e)
        {
            Panel pnl = sender as Panel;
            DataGridView dgv = pnl.Controls[0] as DataGridView;

            int rowHeight = 23;
            
            //如果出现水平滚动条，显示的高度要加上滚动条的高度和间隙       
            int totalRowHeight = 0;
            bool isVisible = dgv_VScrollBarVisible(dgv, out totalRowHeight);
            if (isVisible)
            {
                dgv.Width = dgv.Parent.Width + SystemInformation.VerticalScrollBarWidth;
            }
            else
            {
                dgv.Width = dgv.Parent.Width;
            }
            isVisible = dgv_VScrollBarVisible(dgv, out totalRowHeight);

            Thread t = new Thread(() =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    this.UpdateScrollbar(isVisible, dgv.DisplayRectangle.Height, totalRowHeight, dgv.VerticalScrollingOffset, rowHeight * 3, rowHeight);

                });
            });
            t.Start();

        }

        /// <summary>
        /// 计算出间隙的高度
        /// </summary>
        /// <param name="dgv"></param>
        /// <returns></returns>
        private int GetDgvSpaceHeight(DataGridView dgv)
        {
            int totalHeight = dgv.ColumnHeadersHeight;
            int displayHeight = dgv.DisplayRectangle.Height;
            int spaceHeight = 0;
            for (int i = dgv.Rows.Count - 1; i >= 0; i--)
            {
                totalHeight += dgv.Rows[i].Height;
                if (totalHeight > displayHeight)
                {
                    if (i == dgv.Rows.Count - 1)
                        spaceHeight = displayHeight - totalHeight;
                    else
                        spaceHeight = displayHeight - (totalHeight - dgv.Rows[i + 1].Height);
                    break;
                }
            }
            return spaceHeight;
        }

        void dgv_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {//Click 会改变 Row 的状态
            DataGridView dgv = sender as DataGridView;
            if (dgv.VerticalScrollingOffset != this.moValue)
            {
                this.Value = dgv.VerticalScrollingOffset;
            }
        }
        private bool dgv_VScrollBarVisible(DataGridView control, out int totalRowHeight)
        {
            totalRowHeight = control.ColumnHeadersHeight;//GetDgvSpaceHeight(control) + 
            foreach (DataGridViewRow item in control.Rows)
            {
                totalRowHeight += item.Height;
            }
            int displayHeight = control.Height; //control.DisplayRectangle.Height;
            //if (control.BorderStyle != System.Windows.Forms.BorderStyle.None)
            //{
            //    displayHeight -= 2;
            //}
            return displayHeight <= totalRowHeight;
        }
        private bool dgv_HScrollBarVisible(DataGridView control, out int totalRowWidth)
        {
            totalRowWidth = control.RowHeadersWidth;
            foreach (DataGridViewColumn item in control.Columns)
            {
                totalRowWidth += item.Width;
            }
            int displayWidth = control.Width;
            if (control.BorderStyle != System.Windows.Forms.BorderStyle.None)
            {
                displayWidth -= 2;
            }
            return displayWidth < totalRowWidth;
        }

        void dgv_RowHeightChanged(object sender, DataGridViewRowEventArgs e)
        {
            dgv_SizeChanged(sender, e);
        }
        void dgv_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            dgv_SizeChanged(sender, e);
        }

        void dgv_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            dgv_SizeChanged(sender, e);
        }
        void dgv_ColumnHeadersHeightChanged(object sender, EventArgs e)
        {
            dgv_SizeChanged(sender, e);
        }
        void dgv_RowHeadersWidthChanged(object sender, EventArgs e)
        {
            dgv_SizeChanged(sender, e);
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
            e.Graphics.FillRectangle(oWhiteBrush, 0, moThumbTop, this.Width, customScrollInfo.ThumbHeight);
            base.OnPaint(e);
        }
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
                if (moControl.TopLevelControl.Handle == Win32API.Win32API.GetForegroundWindow())
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
                else if (this.moControl.GetType() == typeof(TreeViewEx))
                {
                    TreeViewEx tv = this.moControl as TreeViewEx;
                    tv.VerticalScrollValue = Convert.ToInt32(Math.Ceiling((decimal)moValue / (decimal)tv.ItemHeight));
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
