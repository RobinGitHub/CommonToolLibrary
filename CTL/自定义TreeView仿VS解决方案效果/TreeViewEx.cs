using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
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

        public TreeViewEx()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true); // 双缓冲
            this.UpdateStyles();

            this.DrawNode += TreeViewEx_DrawNode;
            this.NodeMouseClick += TreeViewEx_NodeMouseClick;
            this.SizeChanged += TreeViewEx_SizeChanged;

            this.FullRowSelect = true;
            this.HideSelection = false;
            this.HotTracking = true;
        }

        void TreeViewEx_SizeChanged(object sender, EventArgs e)
        {
            this.Refresh();
        }

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

        void TreeViewEx_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            Size txtSize = TextRenderer.MeasureText(e.Node.Text, this.Font);

            PointF center = new PointF(e.Node.Bounds.X, e.Node.Bounds.Y + (this.ItemHeight- txtSize.Height) / 2 );

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
            if (e.Node.Nodes.Count > 0)
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
    }
}
