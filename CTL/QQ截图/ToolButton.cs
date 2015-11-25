using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

using System.Windows.Forms.Design;
using System.Drawing;
using System.Windows.Forms;
using QQ截图.Properties;


namespace QQ截图
{
    [Designer(typeof(ToolButtonDesigner))]
    public partial class ToolButton : Control
    {
        #region 属性
        private Image btnImage;
        public Image BtnImage
        {
            get { return btnImage; }
            set { btnImage = value; }
        }

        private bool isSingleSelectedBtn;
        public bool IsSingleSelectedBtn
        {
            get { return isSingleSelectedBtn; }
            set
            {
                isSingleSelectedBtn = value;
                if (isSingleSelectedBtn)
                    this.isSelectedBtn = true;
            }
        }

        private bool isSelectedBtn;
        public bool IsSelectedBtn
        {
            get { return isSelectedBtn; }
            set
            {
                isSelectedBtn = value;
                if (!isSelectedBtn)
                    this.isSingleSelectedBtn = false;
            }
        }

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (value == isSelected)
                    return;
                isSelected = value;
                this.Invalidate();
            }
        }

        #endregion

        #region 构造函数
        public ToolButton()
        {
            InitializeComponent();
        }
        #endregion

        #region override
        private bool m_bMouseEnter;
        protected override void OnMouseEnter(EventArgs e)
        {
            m_bMouseEnter = true;
            this.Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            m_bMouseEnter = false;
            this.Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnClick(EventArgs e)
        {
            if (this.isSelectedBtn)
            {
                if (this.isSelected)
                {
                    if (!this.isSingleSelectedBtn)
                    {
                        this.isSelected = false;
                        this.Invalidate();
                    }
                }
                else
                {
                    this.isSelected = true;
                    this.Invalidate();
                    int len = this.Parent.Controls.Count;
                    for (int i = 0; i < len; i++)
                    {
                        if (this.Parent.Controls[i] is ToolButton && this.Parent.Controls[i] != this)
                        {
                            if (((ToolButton)(this.Parent.Controls)[i]).isSelected)
                            {
                                ((ToolButton)(this.Parent.Controls[i])).IsSelected = false;
                            }
                        }
                    }
                }
            }
            this.Focus();
            base.OnClick(e);
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            this.OnClick(e);
            base.OnDoubleClick(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (m_bMouseEnter)
            {
                g.FillRectangle(Brushes.LightBlue, this.ClientRectangle);
                g.DrawRectangle(Pens.DarkCyan, new Rectangle(0, 0, this.Width - 1, this.Height - 1));
            }
            if (this.btnImage == null)
            {
                g.DrawImage(Resources.none, new Rectangle(2, 2, 17, 17));
            }
            else
            {
                g.DrawImage(this.btnImage, new Rectangle(2, 2, 17, 17));
            }
            g.DrawString(this.Text, this.Font, Brushes.Black, 21, (this.Height - this.Font.Height) / 2);
            if (this.isSelected)
            {
                g.DrawRectangle(Pens.DarkCyan, new Rectangle(0, 0, this.Width - 1, this.Height - 1));
            }
            base.OnPaint(e);
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, TextRenderer.MeasureText(this.Text, this.Font).Width + 21, 21, specified);
        }
        #endregion

    }

    public class ToolButtonDesigner : ControlDesigner
    {
        protected override void OnPaintAdornments(System.Windows.Forms.PaintEventArgs pe)
        {
            Pen p = new Pen(Color.SteelBlue, 1);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            pe.Graphics.DrawRectangle(p, 0, 0, pe.ClipRectangle.Width - 1, 20);
            p.Dispose();

            base.OnPaintAdornments(pe);
        }

        public override SelectionRules SelectionRules
        {
            get
            {
                return base.SelectionRules & ~SelectionRules.AllSizeable;
            }
        }
    }
}
