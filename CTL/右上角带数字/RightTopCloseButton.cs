using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using 右上角带数字.Properties;

namespace 右上角带数字
{
    public partial class RightTopCloseButton : UserControl
    {
        #region 事件
        /// <summary>
        /// 关闭
        /// </summary>
        public EventHandler Close; 
        #endregion

        #region 属性
        /// <summary>
        /// 关闭图标
        /// </summary>
        private Image _closeImg = Resources.Cls01;
        /// <summary>
        /// 关闭图标的位置
        /// </summary>
        private Point closePt = new Point();

        /// <summary>
        /// 是否显示关闭图片
        /// </summary>
        private bool isShowClose = false;
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), Category("其他"), DefaultValue(typeof(Image), ""), Description("是否显示关闭图标")]
        public bool IsShowClose
        {
            get { return isShowClose; }
            set
            {
                isShowClose = value;
                this.Image = image;
            }
        }

        private Size imageSize = new Size(64, 64);
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), Category("其他"), DefaultValue(typeof(Size), "64, 64"), Description("显示图片的大小")]
        public Size ImageSize
        {
            get { return imageSize; }
            set
            {
                imageSize = value;
            }
        }


        private Image image = null;
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), Category("其他"), DefaultValue(typeof(Image), ""), Description("要显示的图片")]
        public Image Image
        {
            get { return image; }
            set
            {
                image = value;
                if (value != null)
                {
                    Image oldImg = SetImageSize(value);
                    Bitmap bmp = new Bitmap(this.Width, this.Height);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        Point bgPt = new Point((this.Width - oldImg.Width) / 2, (this.Height - oldImg.Height) / 2);
                        g.DrawImage(oldImg, bgPt);
                        if (isShowClose)
                        {
                            closePt= new Point(oldImg.Width - this._closeImg.Width / 2 + bgPt.X, bgPt.Y - this._closeImg.Height / 2);
                            g.DrawImage(_closeImg, closePt);
                        }
                    }
                    picTips.Image = bmp;
                }
                else
                    picTips.Image = null;
            }
        }

        /// <summary>
        /// 设置原始图片的大小
        /// </summary>
        private Image SetImageSize(Image img)
        {
            Bitmap bmp = new Bitmap(imageSize.Width, imageSize.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawImage(img, new RectangleF(0, 0, imageSize.Width, imageSize.Height));
            }
            return bmp;
        }
        #endregion

        #region 构造函数
        public RightTopCloseButton()
        {
            InitializeComponent();
            this.Load += RightTopCloseButton_Load;
            this.picTips.MouseClick += picTips_MouseClick;
        }

        #endregion

        #region Load
        void RightTopCloseButton_Load(object sender, EventArgs e)
        {
            this.Image = image;
        } 
        #endregion

        #region 点击关闭图标
        void picTips_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left
                && isShowClose
                && this.RectangleToScreen(new Rectangle(closePt, _closeImg.Size)).Contains(Control.MousePosition)
                && this.Close != null)
            {
                Close(this, e);
            }
        } 
        #endregion
    }
}
