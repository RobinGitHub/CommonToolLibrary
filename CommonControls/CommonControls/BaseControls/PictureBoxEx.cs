using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommonControls
{
    public class PictureBoxEx : PictureBox
    {
        #region 构造函数
        public PictureBoxEx()
        {
            this.MouseEnter += new EventHandler(PictureBoxEx_MouseEnter);
            this.MouseLeave += new EventHandler(PictureBoxEx_MouseLeave);
            this.MouseDown += new MouseEventHandler(PictureBoxEx_MouseDown);
        } 
        #endregion

        #region 定义图片设置


        private Image _DefaultImage = null;
        private Image _MouseEnterImage = null;
        private Image _MouseDownImage = null;
        private Color _TransparentColor = Color.Transparent;

        /// <summary>
        /// 缩略图地址
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Category("Edu345"), Description("缩略图地址")]
        public string ThumbFilePath { get; set; }

        /// <summary>
        /// 源文件地址
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Category("Edu345"), Description("源文件地址")]
        public string FilePath { get; set; }

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
                if (TransparentColor.ToArgb() != Color.Transparent.ToArgb() && Image != null)
                {
                    Bitmap bitMap = new Bitmap(Image);
                    bitMap.MakeTransparent(TransparentColor);
                    this.Image = bitMap;
                }
            }
        }
        void PictureBoxEx_MouseEnter(object sender, EventArgs e)
        {
            //光标进入            
            if (MouseEnterImage != null)
            {
                if (_DefaultImage == null) _DefaultImage = Image;
                if (TransparentColor.ToArgb() != Color.Transparent.ToArgb())
                {
                    Bitmap bitMap = new Bitmap(MouseEnterImage);
                    bitMap.MakeTransparent(TransparentColor);
                    this.Image = bitMap;
                }
                else
                {
                    this.Image = MouseEnterImage;
                }
            }
        }
        void PictureBoxEx_MouseLeave(object sender, EventArgs e)
        {
            //光标离开
            if (_DefaultImage != null && (MouseEnterImage != null || MouseDownImage != null))
            {
                if (TransparentColor.ToArgb() != Color.Transparent.ToArgb())
                {
                    Bitmap bitMap = new Bitmap(_DefaultImage);
                    bitMap.MakeTransparent(TransparentColor);
                    this.Image = bitMap;
                }
                else
                {
                    this.Image = _DefaultImage;
                }
            }
        }

        void PictureBoxEx_MouseDown(object sender, MouseEventArgs e)
        {
            //光标按下
            if (MouseDownImage != null)
            {
                if (TransparentColor.ToArgb() != Color.Transparent.ToArgb())
                {
                    Bitmap bitMap = new Bitmap(MouseDownImage);
                    bitMap.MakeTransparent(TransparentColor);
                    this.Image = bitMap;
                }
                else
                {
                    this.Image = MouseDownImage;
                }
            }
        }

        #endregion

    }
}
