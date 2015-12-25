using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Win32API;
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

        #region 是否自定义绘制
        /// <summary>
        /// 是否自定义绘制
        /// </summary>
        private bool isCustomDraw = true;
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(true), Category("其他"), Description("是否自定义绘制")]
        public bool IsCustomDraw
        {
            get { return isCustomDraw; }
            set
            {
                isCustomDraw = value;
                if (!value)
                {
                    this.DrawNode += TreeViewEx_DrawNode;
                    this.NodeMouseClick += TreeViewEx_NodeMouseClick;
                }
            }
        }
        #endregion
        
        #endregion

        #region 构造函数
        public TreeViewEx()
        {
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            //SetStyle(ControlStyles.OptimizedDoubleBuffer, true); // 双缓冲
            //this.UpdateStyles();
            //SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.Opaque, true);

            this.FullRowSelect = true;
            this.HideSelection = false;
            this.HotTracking = true;

            HorizontalScrollVisible = false;

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
            if (!e.Node.IsVisible)
            {
                return;
            }
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

            using (BufferedGraphics myBuffer = BufferedGraphicsManager.Current.Allocate(e.Graphics, e.Bounds))
            {
                using (SolidBrush brush = new SolidBrush(backColor))
                {
                    if (FullRowSelect)
                    {
                        myBuffer.Graphics.FillRectangle(brush, e.Bounds);
                    }
                    else
                    {
                        myBuffer.Graphics.FillRectangle(brush, new Rectangle(e.Bounds.Location, new Size(this.Width - e.Bounds.X, e.Bounds.Height)));
                    }
                }

                //    //绘制加减号，做了一些硬编码
                if (e.Node.Nodes.Count > 0 && minusImage != null && plusImage != null)
                {
                    myBuffer.Graphics.DrawImage((e.Node.IsExpanded ? minusImage : plusImage), center);
                }

                //    //绘制文字
                if (e.Node.NodeFont != null)
                    myBuffer.Graphics.DrawString(e.Node.Text, e.Node.NodeFont, new SolidBrush(foreColor), new PointF(center.X + 20, center.Y));
                else
                    myBuffer.Graphics.DrawString(e.Node.Text, this.Font, new SolidBrush(foreColor), new PointF(center.X + 20, center.Y));

                if (isShowBottomLine)
                {
                    using (Pen pen = new Pen(bottomLineColor))
                    {
                        myBuffer.Graphics.DrawLine(pen, new Point(e.Bounds.X, e.Node.Bounds.Y + this.ItemHeight - 1), new Point(e.Bounds.X + e.Bounds.Width, e.Node.Bounds.Y + this.ItemHeight - 1));
                    }
                }
                myBuffer.Render(e.Graphics);
                myBuffer.Dispose();
            }

            //if (FullRowSelect)
            //{
            //    using (SolidBrush brush = new SolidBrush(backColor))
            //    {
            //        e.Graphics.FillRectangle(brush, e.Bounds);
            //    }
            //}
            //else
            //{
            //    using (SolidBrush brush = new SolidBrush(backColor))
            //    {
            //        e.Graphics.FillRectangle(brush, new Rectangle(e.Bounds.Location, new Size(this.Width - e.Bounds.X, e.Bounds.Height)));
            //    }
            //}

            ////绘制加减号，做了一些硬编码
            //if (e.Node.Nodes.Count > 0 && minusImage != null && plusImage != null)
            //{
            //    e.Graphics.DrawImage((e.Node.IsExpanded ? minusImage : plusImage), center);
            //}

            ////绘制文字
            //if (e.Node.NodeFont != null)
            //    e.Graphics.DrawString(e.Node.Text, e.Node.NodeFont, new SolidBrush(foreColor), new PointF(center.X + 20, center.Y));
            //else
            //    e.Graphics.DrawString(e.Node.Text, this.Font, new SolidBrush(foreColor), new PointF(center.X + 20, center.Y));

            //if (isShowBottomLine)
            //{
            //    using (Pen pen = new Pen(bottomLineColor))
            //    {
            //        e.Graphics.DrawLine(pen, new Point(e.Bounds.X, e.Node.Bounds.Y + this.ItemHeight - 1), new Point(e.Bounds.X + e.Bounds.Width, e.Node.Bounds.Y + this.ItemHeight - 1));
            //    }
            //}

            //this.SuspendLayout();
        }
        #endregion

        #region override
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0014) // 禁掉清除背景消息
                return;
            if (m.Msg == (int)WinMsg.WM_HSCROLL)
            {
                this.Invalidate();
            }

            base.WndProc(ref m);
        }
        #endregion

        #region 获取&设置滚动条的值
        public int VerticalScrollValue
        {
            get
            {
                return Win32API.Win32API.GetScrollPos(this.Handle, ScrollBarTypes.SB_VERT.GetHashCode());
            }
            set
            {
                Win32API.Win32API.SetScrollPos(this.Handle, ScrollBarTypes.SB_VERT.GetHashCode(), value, true);
                //移动内容
                Win32API.Win32API.SendMessage(this.Handle, WinMsg.WM_VSCROLL.GetHashCode(), (System.IntPtr)ScrollBarRequests.SB_THUMBPOSITION.GetHashCode(), (System.IntPtr)0);
            }
        }

        public int HorizontalScrollValue
        {
            get
            {
                return Win32API.Win32API.GetScrollPos(this.Handle, ScrollBarTypes.SB_HORZ.GetHashCode());
            }
            set
            {
                //滚动条动，但是内容没有移动
                Win32API.Win32API.SetScrollPos(this.Handle, ScrollBarTypes.SB_HORZ.GetHashCode(), value, true);
                ///用垂直滚动条的方式没有效果，经过调试，如果直接移动本身的滚动条 wParam 值是随时变化的
                ///而滚动条的值 是通过 HiWord 方法算出来的
                ///而这里需要反推即：通过滚动条的值 算出 wParam 的值
                ///这里为什么要 +5 因为算出来的值与本身滚动条的值相差5，具体原因不明
                int wParam = (value << 16) + 5;
                Win32API.Win32API.SendMessage(this.Handle, WinMsg.WM_HSCROLL.GetHashCode(), (System.IntPtr)wParam, (System.IntPtr)0);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(true), Category("其他"), Description("垂直滚动条是否显示")]
        public bool VerticalScrollVisible
        {
            set
            {
                Win32API.Win32API.ShowScrollBar(this.Handle, (int)ScrollBarTypes.SB_VERT, value);
            }
            get
            {
                return (Win32API.Win32API.GetWindowLong(this.Handle, (int)SetWindowLongOffsets.GWL_STYLE) & (int)WindowStyles.WS_VSCROLL) != 0;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(true), Category("其他"), Description("水平滚动条是否显示")]
        public bool HorizontalScrollVisible
        {
            set
            {
                Win32API.Win32API.ShowScrollBar(this.Handle, (int)ScrollBarTypes.SB_HORZ, value);
            }
            get
            {
                return (Win32API.Win32API.GetWindowLong(this.Handle, (int)SetWindowLongOffsets.GWL_STYLE) & (int)WindowStyles.WS_HSCROLL) != 0;
            }
        }

        //public SCROLLINFO VerticalScrollInfo
        //{
        //    get
        //    {
        //        SCROLLINFO si = new SCROLLINFO();
        //        si.cbSize = (uint)Marshal.SizeOf(si);
        //        si.fMask = (uint)ScrollBarInfoFlags.SIF_ALL;
        //        Win32API.Win32API.GetScrollInfo(this.Handle, (int)ScrollBarTypes.SB_VERT, ref si);
        //        return si;
        //    }
        //}

        //public SCROLLINFO HorizontalScrollInfo
        //{
        //    get
        //    {
        //        SCROLLINFO si = new SCROLLINFO();
        //        si.cbSize = (uint)Marshal.SizeOf(si);
        //        si.fMask = (uint)ScrollBarInfoFlags.SIF_ALL;
        //        Win32API.Win32API.GetScrollInfo(this.Handle, (int)ScrollBarTypes.SB_HORZ, ref si);
        //        return si;
        //    }
        //}
        #endregion


    }
}
