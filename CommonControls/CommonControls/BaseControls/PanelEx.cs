using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommonControls
{
    public class PanelEx : Panel
    {
        #region 构造函数
        public PanelEx()
        {
            this.MouseEnter += new EventHandler(PanelEx_MouseEnter);
            this.MouseLeave += new EventHandler(PanelEx_MouseLeave);
            this.MouseDown += new MouseEventHandler(PanelEx_MouseDown);

            _DefaultImage = BackgroundImage;
            if (BorderColor != Color.White)
            {
                this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
                this.Paint += new PaintEventHandler(PanelEx_Paint);
            }
        } 
        #endregion

        #region 重绘边框
        protected Color _BorderColor = Color.White;//边框色
        protected Color _MouseOnColor = Color.Transparent;//鼠标停留色
        protected Color _MouseLeaveColor = Color.Transparent;//鼠标移开色
        protected int _BorderArc = 0;

        [Category("Edu345"), DisplayName("5.BorderColor"), Description("定义要绘制边框的颜色")]
        public Color BorderColor { get { return _BorderColor; } set { _BorderColor = value; } }
        [Category("Edu345"), DisplayName("6.BorderArc"), Description("定义要绘制边框的弧度"), DefaultValue(0)]
        public int BorderArc { get { return _BorderArc; } set { _BorderArc = value; } }

        [Category("Edu345"), DisplayName("MouseOnColor"), Description("鼠标停留的颜色")]
        public Color MouseOnColor { get { return _MouseOnColor; } set { _MouseOnColor = value; } }
        [Category("Edu345"), DisplayName("MouseLeaveColor"), Description("鼠标离开的颜色")]
        public Color MouseLeaveColor { get { return _MouseLeaveColor; } set { _MouseLeaveColor = value; } }

        protected bool _button = false;
        [DefaultValue(false), Browsable(true), DescriptionAttribute("是否启用按钮模式，鼠标悬停变色效果")]
        public bool Button
        {
            set
            {
                _button = value;
            }
            get
            {
                return _button;
            }
        }
        void PanelEx_Paint(object sender, PaintEventArgs e)
        {
            if (BorderArc > 0)
            {
                System.Drawing.Drawing2D.GraphicsPath shape = GetRoundedPath(this, BorderArc);
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.DrawPath(new Pen(BorderColor), shape);
            }
            else
            {
                ControlPaint.DrawBorder(e.Graphics, ClientRectangle, BorderColor, ButtonBorderStyle.Solid);
            }
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            if (Button)
            {
                this.BackColor = MouseOnColor;
            }
            base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            if (Button)
            {
                this.BackColor = MouseLeaveColor;
            }
            base.OnMouseLeave(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.Focus();
            base.OnMouseUp(e);
        }
        #endregion

        #region 定义背景图片设置

        private Image _DefaultImage = null;
        private Image _MouseEnterImage = null;
        private Image _MouseDownImage = null;
        private Color _TransparentColor = Color.Transparent;

        // 
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Category("Edu345"), DisplayName("1.MouseEnterImage"), Description("光标进入图标")]
        public Image MouseEnterImage
        {
            get { return this._MouseEnterImage; }
            set
            {
                _MouseEnterImage = value;
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Category("Edu345"), DisplayName("2.MouseDownImage"), Description("光标按下图标")]
        public Image MouseDownImage
        {
            get { return this._MouseDownImage; }
            set
            {
                _MouseDownImage = value;
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Category("Edu345"), DisplayName("3.TransparentColor"), Description("定义控件透明显示的颜色")]
        public Color TransparentColor
        {
            get { return _TransparentColor; }
            set
            {
                _TransparentColor = value;
                if (TransparentColor.ToArgb() != Color.Transparent.ToArgb() && BackgroundImage != null)
                {
                    Bitmap bitMap = new Bitmap(BackgroundImage);
                    bitMap.MakeTransparent(TransparentColor);
                    this.BackgroundImage = bitMap;
                }
            }
        }

        void PanelEx_MouseEnter(object sender, EventArgs e)
        {
            //光标进入            
            if (MouseEnterImage != null)
            {
                if (TransparentColor.ToArgb() != Color.Transparent.ToArgb())
                {
                    Bitmap bitMap = new Bitmap(MouseEnterImage);
                    bitMap.MakeTransparent(TransparentColor);
                    this.BackgroundImage = bitMap;
                }
                else
                {
                    this.BackgroundImage = MouseEnterImage;
                }
            }
        }
        void PanelEx_MouseLeave(object sender, EventArgs e)
        {
            //光标离开
            if (_DefaultImage != null && MouseEnterImage != null)
            {
                if (TransparentColor.ToArgb() != Color.Transparent.ToArgb())
                {
                    Bitmap bitMap = new Bitmap(_DefaultImage);
                    bitMap.MakeTransparent(TransparentColor);
                    this.BackgroundImage = bitMap;
                }
                else
                {
                    this.BackgroundImage = _DefaultImage;
                }
            }
        }

        void PanelEx_MouseDown(object sender, MouseEventArgs e)
        {
            //光标按下
            if (MouseDownImage != null)
            {
                if (TransparentColor.ToArgb() != Color.Transparent.ToArgb())
                {
                    Bitmap bitMap = new Bitmap(MouseDownImage);
                    bitMap.MakeTransparent(TransparentColor);
                    this.BackgroundImage = bitMap;
                }
                else
                {
                    this.BackgroundImage = MouseDownImage;
                }
            }
        }
        #endregion

        #region 圆弧角的比率
        private GraphicsPath GetRoundedPath(Control control, int arc)
        {
            // int radian = 3; //圆弧角的比率，可以自己改变这个值看具体的效果
            int w = control.Width - 1; //窗体宽
            int h = control.Height - 1; //窗体高

            //对于矩形的窗体，要在一个角上画个弧度至少需要2个点，所以4个角需要至少8个点
            Point p1 = new Point(arc, 0);
            Point p2 = new Point(w - arc, 0);
            Point p3 = new Point(w, arc);
            Point p4 = new Point(w, h - arc);
            Point p5 = new Point(w - arc, h);
            Point p6 = new Point(arc, h);
            Point p7 = new Point(0, h - arc);
            Point p8 = new Point(0, arc);

            System.Drawing.Drawing2D.GraphicsPath shape = new System.Drawing.Drawing2D.GraphicsPath();

            Point[] p = new Point[] { p1, p2, p3, p4, p5, p6, p7, p8 };
            shape.AddPolygon(p);
            return shape;
        } 
        #endregion
    }
}
