using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommonControls
{
    public class ButtonEx : Button
    {
        #region 属性
        protected override bool ShowFocusCues { get { return false; } }
        #endregion

        #region 构造函数
        public ButtonEx()
        {
            this.MouseEnter += new EventHandler(ButtonEx_MouseEnter);
            this.MouseLeave += new EventHandler(ButtonEx_MouseLeave);
            this.MouseDown += new MouseEventHandler(ButtonEx_MouseDown);

            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        }


        #endregion

        //public override Color BackColor
        //{
        //    get
        //    {
        //        return base.BackColor;
        //    }
        //    set
        //    {
        //        base.BackColor = value;
        //    }
        //}

        #region 定义背景图片设置

        private bool _Selected = false;

        private Image _DefaultImage = null;
        private Image _MouseEnterImage = null;
        private Image _MouseDownImage = null;

        private Color _TransparentColor;
        private Color _DefalutForeColor = Color.Black;
        private Color _MouseEnterForeColor = Color.Black;
        private Color _MouseDownForeColor = Color.Black;

        private Color _DefaultBackColor;
        private Color _MouseEnterBackColor;
        private Color _MouseDownBackColor;

        [Browsable(false)]
        public Image DefaultBackgroundImage
        {
            get
            {
                return _DefaultImage;
            }
        }

        // 
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Category("Edu345"), DisplayName("1.MouseEnterImage"), Description("光标进入图标")]
        public Image MouseEnterImage
        {
            get
            {
                if (_MouseEnterImage == null) _MouseEnterImage = BackgroundImage;
                return this._MouseEnterImage;
            }
            set
            {
                _MouseEnterImage = value;
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Category("Edu345"), DisplayName("2.MouseDownImage"), Description("光标按下图标")]
        public Image MouseDownImage
        {
            get
            {
                if (_MouseDownImage == null) _MouseDownImage = BackgroundImage;
                return this._MouseDownImage;
            }
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
            }
        }

        [Category("Edu345"), DisplayName("4.Selected"), Description("当前按钮是否选定状态")]
        public bool Selected { get { return _Selected; } set { _Selected = value; } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Category("Edu345"), DisplayName("5.DefalutForeColor"), Description("默认前景色")]
        public Color DefalutForeColor
        {
            get { return _DefalutForeColor; }
            set
            {
                _DefalutForeColor = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Category("Edu345"), DisplayName("6.MouseEnterForeColor"), Description("鼠标悬停前景色")]
        public Color MouseEnterForeColor
        {
            get { return _MouseEnterForeColor; }
            set
            {
                _MouseEnterForeColor = value;
            }
        }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Category("Edu345"), DisplayName("7.MouseDownForeColor"), Description("鼠标按下前景色")]
        public Color MouseDownForeColor
        {
            get { return _MouseDownForeColor; }
            set
            {
                _MouseDownForeColor = value;
            }
        }



        private bool isChangeBorderSizeOnMouseEnter = true;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(true), Category("Edu345"), DisplayName("8.IsChangeBorderSizeOnMouseEnter"), Description("光标进是否改变边框")]
        public bool IsChangeBorderSizeOnMouseEnter
        {
            get { return isChangeBorderSizeOnMouseEnter; }
            set { isChangeBorderSizeOnMouseEnter = value; }
        }

        private bool isChangeStyleOnMouseDown = false;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), DefaultValue(false), Category("Edu345"), DisplayName("9.IsChangeStyleOnMouseDown"), Description("点击是否改变样式")]
        public bool IsChangeStyleOnMouseDown
        {
            get { return isChangeStyleOnMouseDown; }
            set { isChangeStyleOnMouseDown = value; }
        }

        [Browsable(true)]
        [DefaultValue(typeof(Color), "")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [NotifyParentProperty(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Category("Edu345"), DisplayName("10.MouseEnterBackColor"), Description("默认背景色")]
        public new Color DefaultBackColor
        {
            get { return _DefaultBackColor; }
            set
            {
                _DefaultBackColor = value;
            }
        }

        [Browsable(true)]
        [DefaultValue(typeof(Color), "")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [NotifyParentProperty(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Category("Edu345"), DisplayName("11.MouseEnterBackColor"), Description("鼠标悬停背景色")]
        public Color MouseEnterBackColor
        {
            get { return _MouseEnterBackColor; }
            set
            {
                _MouseEnterBackColor = value;
            }
        }

        [Browsable(true)]
        [DefaultValue(typeof(Color), "")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [NotifyParentProperty(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Category("Edu345"), DisplayName("12.MouseDownBackColor"), Description("鼠标按下背景色")]
        public Color MouseDownBackColor
        {
            get { return _MouseDownBackColor; }
            set
            {
                _MouseDownBackColor = value;
            }
        }


        void ButtonEx_MouseEnter(object sender, EventArgs e)
        {
            //光标进入 
            if (Selected)
            {
                //按下图标
                this.BackgroundImage = MouseDownImage;
                this.ForeColor = MouseDownForeColor;
                if (!MouseDownBackColor.IsEmpty)
                    this.BackColor = MouseDownBackColor;
            }
            else
            {
                this.BackgroundImage = MouseEnterImage;
                this.BackgroundImageLayout = ImageLayout.Stretch;

                if (!MouseEnterBackColor.IsEmpty)
                    this.BackColor = MouseEnterBackColor;
                this.ForeColor = MouseEnterForeColor;
            }
            if (isChangeBorderSizeOnMouseEnter)
                this.FlatAppearance.BorderSize = 1;
        }

        void ButtonEx_MouseDown(object sender, MouseEventArgs e)
        {
            //清空同组按钮为默认图片
            if (Parent != null && isChangeStyleOnMouseDown)
            {
                foreach (Control c in Parent.Controls)
                {
                    ButtonEx button = c as ButtonEx;
                    if (button != null && button != this)
                    {
                        button.Selected = false;
                        button.BackgroundImage = DefaultBackgroundImage;
                        button.ForeColor = DefaultForeColor;
                        if (!DefaultBackColor.IsEmpty)
                            button.BackColor = DefaultBackColor;
                        if (isChangeBorderSizeOnMouseEnter)
                            button.FlatAppearance.BorderSize = 0;
                    }
                }
            }
            //光标按下
            this.Selected = true;
            this.BackgroundImage = MouseDownImage;
            this.ForeColor = MouseDownForeColor;
        }

        void ButtonEx_MouseLeave(object sender, EventArgs e)
        {
            if (!Selected)
            {
                this.BackgroundImage = _DefaultImage;
                this.ForeColor = _DefalutForeColor;
                if (!_DefaultBackColor.IsEmpty)
                    this.BackColor = _DefaultBackColor;
            }
            if (isChangeBorderSizeOnMouseEnter)
                this.FlatAppearance.BorderSize = 0;
        }
        #endregion

    }
}
