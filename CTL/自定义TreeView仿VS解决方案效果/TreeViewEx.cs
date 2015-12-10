using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using 自定义TreeView仿VS解决方案效果.Properties;

/*实现效果：
 * 1.控制鼠标悬停，点击的颜色
 * 2.是否有分割线
 * 3.是否显示复选框
 * 4.
 */

namespace 自定义TreeView仿VS解决方案效果
{
    public class TreeViewEx : TreeView
    {
        #region 私有属性
        /// <summary>
        /// 鼠标悬停的背景色
        /// </summary>
        private Color mouseEnterColor = Color.FromArgb(252, 240, 193);
        /// <summary>
        /// 选中时候的背景色
        /// </summary>
        private Color selectedColor = Color.FromArgb(252, 235, 166);

        /// <summary>
        /// 是否显示底部的分割线
        /// </summary>
        private bool isShowBottomLine = true;
        /// <summary>
        /// 分割线颜色
        /// </summary>
        private Color bottomLineColor = Color.FromArgb(221, 237, 252);

        /// <summary>
        /// 展开图标
        /// </summary>
        private Image minusImage = Resources.minus;
        /// <summary>
        /// 收起图标
        /// </summary>
        private Image plusImage = Resources.plus;
        #endregion

        #region 公共属性

        #region 鼠标悬停的背景色
        /// <summary>
        /// 鼠标悬停的背景色
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("其他"), Description("鼠标悬停的背景色")]
        public Color MouseEnterColor
        {
            get { return mouseEnterColor; }
            set { mouseEnterColor = value; }
        }
        #endregion

        #region 选中时候的背景色
        /// <summary>
        /// 选中时候的背景色
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("其他"), Description("选中时候的背景色")]
        public Color SelectedColor
        {
            get { return selectedColor; }
            set { selectedColor = value; }
        }
        #endregion

        #region 是否显示底部的分割线
        /// <summary>
        /// 是否显示底部的分割线
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(true), Category("其他"), Description("是否显示底部的分割线")]
        public bool IsShowBottomLine
        {
            get { return isShowBottomLine; }
            set { isShowBottomLine = value; }
        }
        #endregion

        #region 分割线颜色
        /// <summary>
        /// 分割线颜色
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(true), Category("其他"), Description("分割线颜色")]
        public Color BottomLineColor
        {
            get { return bottomLineColor; }
            set { bottomLineColor = value; }
        }
        #endregion

        #region 展开图标
        /// <summary>
        /// 展开图标
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(typeof(Image), ""), Category("其他"), Description("展开图标")]
        public Image MinusImage
        {
            get { return minusImage; }
            set { minusImage = value; }
        }
        #endregion

        #region 收起图标
        /// <summary>
        /// 收起图标
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(typeof(Image), ""), Category("其他"), Description("收起图标")]
        public Image PlusImage
        {
            get { return plusImage; }
            set { plusImage = value; }
        }
        #endregion
        #endregion

        #region 构造函数
        public TreeViewEx()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true); // 双缓冲
            this.UpdateStyles();

            this.DrawNode += TreeViewEx_DrawNode;
            this.NodeMouseClick += TreeViewEx_NodeMouseClick;

            this.FullRowSelect = true;
            this.HideSelection = false;
            this.HotTracking = true;
        }
        #endregion

        #region 控制展开 收缩
        /// <summary>
        /// 控制展开 收缩 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TreeViewEx_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            #region 展开缩小的区域
            //展开缩小的区域
            Rectangle rec = new Rectangle(e.Node.Bounds.X, e.Node.Bounds.Y, minusImage.Width, this.ItemHeight);
            if (rec.Contains(e.Location) && e.Node.Nodes.Count > 0)
            {
                if (e.Node.IsExpanded)
                    e.Node.Collapse(true);
                else
                    e.Node.Expand();
            }
            #endregion
        }
        #endregion

        #region 重绘内容 DrawNode
        /// <summary>
        /// 重绘内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TreeViewEx_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (!e.Node.IsVisible) return;
            Size txtSize = TextRenderer.MeasureText(e.Node.Text, this.Font);

            PointF center = new PointF(e.Node.Bounds.X, e.Node.Bounds.Y + (this.ItemHeight - txtSize.Height) / 2);

            Color foreColor;
            Color backColor;
            if ((e.State & TreeNodeStates.Selected) > 0)
            {
                foreColor = e.Node.ForeColor;
                backColor = selectedColor;
            }
            else if ((e.State & TreeNodeStates.Hot) > 0)
            {
                foreColor = e.Node.ForeColor;  //用该node的forecolor.
                backColor = mouseEnterColor;
                e.Node.ToolTipText = e.Node.Text;
            }
            else
            {
                foreColor = e.Node.ForeColor;
                backColor = Color.White;
            }

            if (foreColor.A == 0)
            {//透明
                foreColor = this.ForeColor;
            }

            if (FullRowSelect)
            {
                e.Graphics.FillRectangle(new SolidBrush(backColor), e.Bounds);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(backColor), new Rectangle(e.Bounds.Location, new Size(this.Width - e.Bounds.X, e.Bounds.Height)));
            }


            //绘制加减号，做了一些硬编码
            if (e.Node.Nodes.Count > 0 && minusImage != null && plusImage != null)
            {
                e.Graphics.DrawImage((e.Node.IsExpanded ? minusImage : plusImage), center);
            }

            //绘制文字
            if (e.Node.NodeFont != null)
                e.Graphics.DrawString(e.Node.Text, e.Node.NodeFont, new SolidBrush(foreColor), new PointF(center.X + 20, center.Y));
            else
                e.Graphics.DrawString(e.Node.Text, this.Font, new SolidBrush(foreColor), new PointF(center.X + 20, center.Y));

            if (isShowBottomLine)
            {
                using (Pen pen = new Pen(bottomLineColor))
                {
                    e.Graphics.DrawLine(pen, new Point(e.Bounds.X, e.Node.Bounds.Y + this.ItemHeight - 1), new Point(e.Bounds.X + e.Bounds.Width, e.Node.Bounds.Y + this.ItemHeight - 1));
                }
            }
            this.Update();
        }
        #endregion

        #region 获取&设置滚动条的值
        //获取滚动条位置
        [DllImport("user32.dll", EntryPoint = "GetScrollPos")]
        public static extern int GetScrollPos(IntPtr hwnd, int nbar);
        //设置滚动条位置
        //TreeView句柄，滚动条方向/水平/垂直/,滚动条位置,是否重新描绘
        [DllImport("user32.dll", EntryPoint = "SetScrollPos")]
        public static extern int SetScrollPos(IntPtr hwnd, int nbar, int nPos, bool bRedraw);

        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static public extern bool GetScrollInfo(System.IntPtr hwnd, int fnBar, ref LPSCROLLINFO lpsi);


        public const int sb_horz = 0;//滚动条水平常量
        public const int sb_vert = 1;//滚动条垂直常量

        private const int SB_LINEUP = 0;
        private const int SB_LINEDOWN = 1;
        private const int SB_PAGEUP = 2;
        private const int SB_PAGEDOWN = 3;
        private const int SB_THUMBPOSITION = 4;
        private const int SB_THUMBTRACK = 5;
        private const int SB_TOP = 6;
        private const int SB_BOTTOM = 7;
        private const int SB_ENDSCROLL = 8;

        private const int WM_HSCROLL = 0x114;
        private const int WM_VSCROLL = 0x115;
        private const int WM_MOUSEWHEEL = 0x020A;
        private const int WM_NCCALCSIZE = 0x0083;
        private const int WM_PAINT = 0x000F;
        private const int WM_SIZE = 0x0005;

        public int VerticalScrollValue
        {
            get
            {
                return GetScrollPos(this.Handle, sb_vert);
            }
            set
            {
                SetScrollPos(this.Handle, sb_vert, value, true);

                int param = getSBFromScrollEventType(ScrollEventType.ThumbTrack);
                if (param == -1)
                    return;
                //移动内容
                SendMessage(this.Handle, (uint)WM_VSCROLL, (System.IntPtr)param, (System.IntPtr)0);
            }
        }

        public int HorizontalScrollValue
        {
            get
            {
                return GetScrollPos(this.Handle, sb_horz);
            }
            set
            {
                SetScrollPos(this.Handle, sb_horz, value, true);

                int param = getSBFromScrollEventType(ScrollEventType.ThumbPosition);
                //if (param == -1)
                //    return;
                //移动内容
                SendMessage(this.Handle, (uint)WM_HSCROLL, (System.IntPtr)param, (System.IntPtr)0);

            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_HSCROLL:
                    SendMessage(this.Handle, (uint)m.Msg, m.WParam, m.LParam);
                    //MessageBox.Show("asdf");
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        //没有效果
        //public bool VerticalScrollVisible
        //{
        //    get
        //    {
        //        LPSCROLLINFO si = new LPSCROLLINFO();
        //        si.cbSize = (uint)Marshal.SizeOf(si);
        //        si.fMask = (uint)ScrollInfoMask.SIF_DISABLEDNOSCROLL;
        //        bool rlt = GetScrollInfo(this.Handle, sb_vert, ref si);
        //        return si.nMax > 0;
        //    }
        //}
        //public bool HorizontalScrollVisible
        //{
        //    get
        //    {
        //        LPSCROLLINFO si = new LPSCROLLINFO();
        //        si.cbSize = (uint)Marshal.SizeOf(si);
        //        si.fMask = (uint)ScrollInfoMask.SIF_DISABLEDNOSCROLL;
        //        bool rlt = GetScrollInfo(this.Handle, sb_horz, ref si);
        //        return si.nMax > 0;
        //    }
        //}



        private int getSBFromScrollEventType(ScrollEventType type)
        {
            int res = -1;
            switch (type)
            {
                case ScrollEventType.SmallDecrement:
                    res = SB_LINEUP;
                    break;
                case ScrollEventType.SmallIncrement:
                    res = SB_LINEDOWN;
                    break;
                case ScrollEventType.LargeDecrement:
                    res = SB_PAGEUP;
                    break;
                case ScrollEventType.LargeIncrement:
                    res = SB_PAGEDOWN;
                    break;
                case ScrollEventType.ThumbTrack:
                    res = SB_THUMBTRACK;
                    break;
                case ScrollEventType.First:
                    res = SB_TOP;
                    break;
                case ScrollEventType.Last:
                    res = SB_BOTTOM;
                    break;
                case ScrollEventType.ThumbPosition:
                    res = SB_THUMBPOSITION;
                    break;
                case ScrollEventType.EndScroll:
                    res = SB_ENDSCROLL;
                    break;
                default:
                    break;
            }
            return res;
        }


        public struct LPSCROLLINFO
        {
            public uint cbSize;
            public uint fMask;
            public int nMin;
            public int nMax;
            public uint nPage;
            public int nPos;
            public int nTrackPos;
        }

        public enum ScrollInfoMask : uint
        {
            SIF_RANGE = 0x1,
            SIF_PAGE = 0x2,
            SIF_POS = 0x4,
            SIF_DISABLEDNOSCROLL = 0x8,
            SIF_TRACKPOS = 0x10,
            SIF_ALL = (SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS),
        }
        #endregion


    }
}
