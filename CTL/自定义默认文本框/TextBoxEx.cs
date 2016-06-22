using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 自定义默认文本框
{
    public class TextBoxEx : TextBox
    {
        #region 属性
        /// <summary>
        /// 默认文本的样式
        /// </summary>
        Font defautFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

        Font normalFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

        Color normalForeColor = Color.Black;
        /// <summary>
        /// 默认字体颜色
        /// </summary>
        Color defaultColor = Color.FromArgb(215, 214, 214);

        public string defaultText = "";
        [DefaultValue(""), Category("其他"), DisplayName("1.DefaultText"), Description("默认显示文本")]
        public string DefaultText
        {
            get 
            { 
                return defaultText; 
            }
            set
            {
                defaultText = value;
                if (string.IsNullOrEmpty(value))
                {
                    isShowDefaultText = false;
                    this.Font = normalFont;
                    this.ForeColor = normalForeColor;
                }
                else
                {
                    isShowDefaultText = true;
                    base.Text = value;
                    this.Font = defautFont;
                    this.ForeColor = defaultColor;
                }
            }
        }

        /// <summary>
        /// 文本是否为默认文本
        /// </summary>
        [Browsable(false), DefaultValue(false)]
        private bool isShowDefaultText = false;
        protected internal bool IsShowDefaultText
        {
            get
            {
                if (string.IsNullOrEmpty(defaultText))
                    isShowDefaultText = false;
                return isShowDefaultText;
            }
            set
            { 
                isShowDefaultText = value;
            }
        }
        /// <summary>
        /// 这个字段只有在不显示默认内容时有用
        /// </summary>
        [Category("其他"), DefaultValue("")]
        public string TextValue
        {
            get
            {
                if (isShowDefaultText)
                {
                    return "";
                }
                else
                {
                    return base.Text;
                }
            }
            set
            {
                isShowDefaultText = false;
                base.Text = value;
                this.Font = normalFont;
                this.ForeColor = normalForeColor;
            }
        }
        #endregion

        #region 构造函数
        public TextBoxEx()
        {

        }
        #endregion

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            if (isShowDefaultText)
            {
                base.Text = "";
                isShowDefaultText = false;
                this.Font = normalFont;
                this.ForeColor = normalForeColor;
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (string.IsNullOrEmpty(this.Text.Trim()) && !string.IsNullOrEmpty(defaultText))
            {
                isShowDefaultText = true;
                base.Text = defaultText;
                this.Font = defautFont;
                this.ForeColor = defaultColor;
            }
        }







    }
}
