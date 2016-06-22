using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using 右上角带数字.Properties;

/* 消息条数提示
 * 
 */

namespace Haobitou.Win
{
    public partial class MsgCountTip : UserControl
    {
        #region 属性
        /// <summary>
        /// 数字图片
        /// </summary>
        private Image _imgNumbers = null;
        private Dictionary<int, Image> _imgNumberDict = null;
        /// <summary>
        /// 是否显示加号
        /// </summary>
        private bool isShowPlus = false;

        /// <summary>
        /// 图片整体宽度
        /// </summary>
        private int width = 0;
        /// <summary>
        /// 数字图片高度
        /// </summary>
        private int numPicHeight = 10;
        /// <summary>
        /// 单个数字的宽度
        /// </summary>
        private int numSingleWidth = 7;
        /// <summary>
        /// 背景图片高度
        /// </summary>
        private int bgPicHeight = 16;
        /// <summary>
        /// 背景图片半径
        /// </summary>
        private int bgPicRadius = 8;
        #endregion

        #region 公布属性
        /// <summary>
        /// 最大上线，不包含这个
        /// </summary>
        private int maxNum = 100;
        /// <summary>
        /// 最大上线，不包含这个
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), Category("其他"), DefaultValue(100), Description("最大上线(不包含本身)")]
        public int MaxNum
        {
            get { return maxNum; }
            set { maxNum = value; }
        }

        private int msgCount = 0;
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), Category("其他"), DefaultValue(0), Description("消息条数")]
        public int MsgCount
        {
            get { return msgCount; }
            set
            {
                msgCount = value;
                if (value > 0)
                {
                    GetMessageNumber(value);
                }
                else
                {
                    Image = image;
                }
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
                if (msgCount > 0)
                    GetMessageNumber(msgCount);
                else if (value != null)
                {
                    int canvasheight = image.Height + bgPicHeight / 2;
                    int canvasWidth = image.Width;
                    Bitmap bmp = new Bitmap(canvasWidth, canvasheight);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.DrawImage(this.image, new Point(0, bgPicHeight / 2));
                    }
                    picTips.Image = bmp;
                }
                else
                    picTips.Image = null;
            }
        }
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), Category("其他"), DefaultValue(typeof(RightToLeft), "No"), Description("图片显示的方向")]
        public RightToLeft PicRightToLeft
        {
            get { return picTips.RightToLeft; }
            set { picTips.RightToLeft = value; }
        }
        #endregion

        #region 构造函数
        public MsgCountTip()
        {
            InitializeComponent();
            _imgNumbers = Resources.T1;
            _imgNumberDict = new Dictionary<int, Image>();
            for (int i = 0; i < 11; i++)
            {
                Image img = GetSingleNumberImage(i);
                _imgNumberDict.Add(i, img);
            }

            picTips.Click += picTips_Click;
            picTips.MouseEnter += picTips_MouseEnter;
        }
        #endregion

        #region 获取整体图片
        /// <summary>
        /// 获取整体图片
        /// 数字+背景+显示的图片
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private void GetMessageNumber(int number)
        {
            if (number >= 0)
            {
                if (number >= maxNum)
                {
                    number = maxNum - 1;
                    isShowPlus = true;
                }
                else
                    isShowPlus = false;

                int len = number.ToString().Length;
                if (isShowPlus)
                    len += 1;

                //因背景的半径是 8 而文字的宽度是7
                if (len == 1)
                    width = bgPicHeight;
                else
                {
                    width = (len - 1) * numSingleWidth + bgPicRadius * 2;
                }
                int canvasheight = bgPicHeight;//背景的实际高度
                int canvasWidth = width;
                if (this.image != null)
                {
                    canvasheight = this.image.Height + bgPicRadius;
                    canvasWidth = this.image.Width + width;
                }

                Bitmap bmp = new Bitmap(canvasWidth, canvasheight);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    Image numberImage = GetNumberImage(number);
                    Image backImage = GetNumberBack(len);

                    //在指定的位置使用原始物理大小绘制指定的 Image。
                    if (this.image == null)
                    {
                        g.DrawImage(backImage, new Point(0, 0));
                        g.DrawImage(numberImage, new Point(bgPicRadius / 2, 3));
                    }
                    else
                    {
                        g.DrawImage(this.image, new Point(0, bgPicHeight / 2));
                        int x = this.image.Width - bgPicRadius;
                        g.DrawImage(backImage, new Point(x, 0));
                        g.DrawImage(numberImage, new Point(x + bgPicRadius / 2, 3));
                    }
                }
                picTips.Image = bmp;
            }
        }

        #endregion

        #region 获取要显示的数字图片
        /// <summary>
        /// 获取要显示的数字图片
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private Image GetNumberImage(int number)
        {
            if (number >= 0)
            {
                int len = number.ToString().Length;
                int numWidth = numSingleWidth * len;
                if (isShowPlus)
                {
                    numWidth = numSingleWidth * (len + 1);
                }
                Bitmap bmp = new Bitmap(numWidth, bgPicHeight);

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    for (int i = 0; i < len; i++)
                    {
                        int single = number % 10;
                        number = number / 10;
                        Image img = _imgNumberDict[single];
                        if (img != null)
                        {
                            //在指定位置并且按指定大小绘制指定的 Image。
                            g.DrawImage(img, new Rectangle(numSingleWidth * (len - 1 - i), -1, img.Width, img.Height));
                        }
                    }
                    if (isShowPlus)
                    {
                        Image img = _imgNumberDict[10];
                        g.DrawImage(img, new Rectangle(numWidth - numSingleWidth, -1, img.Width, img.Height));
                    }
                }
                return bmp;
            }
            return null;
        }
        #endregion

        #region 获取背景图片
        /// <summary>
        /// 获取背景图片
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        private Image GetNumberBack(int len)
        {
            //因背景的半径是 8 而文字的宽度是7
            Bitmap bmp = new Bitmap(width, bgPicHeight);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                //在指定位置并且按指定大小绘制指定的 Image 的指定部分。

                //四个参数的意思
                //要绘制的 Image
                //它指定所绘制图像的位置和大小。将图像进行缩放以适合该矩形。
                //它指定 image 对象中要绘制的部分。
                //枚举的成员，它指定 srcRect 参数所用的度量单位。
                g.DrawImage(_imgNumbers, new Rectangle(0, 0, bgPicRadius, bgPicHeight), new RectangleF(0, numPicHeight, bgPicRadius, bgPicHeight), GraphicsUnit.Pixel);
                g.DrawImage(_imgNumbers, new Rectangle(bgPicRadius, 0, width - bgPicRadius * 2, bgPicHeight), new Rectangle(bgPicRadius, numPicHeight, _imgNumbers.Width - bgPicRadius * 2, bgPicHeight), GraphicsUnit.Pixel);
                g.DrawImage(_imgNumbers, new Rectangle(width - bgPicRadius, 0, bgPicRadius, bgPicHeight), new Rectangle(_imgNumbers.Width - bgPicRadius, numPicHeight, bgPicRadius, bgPicHeight), GraphicsUnit.Pixel);
            }
            return bmp;
        }
        #endregion

        #region 获取单个数字图片
        /// <summary>
        /// 获取数字图片
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private Image GetSingleNumberImage(int number)
        {
            if (number >= 0 && number < 11)
            {
                Bitmap bmp = new Bitmap(numSingleWidth, numPicHeight);
                //从指定的 Image 创建新的 Graphics。
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    //在指定的位置使用原始物理大小绘制指定的 Image。
                    g.DrawImage(_imgNumbers, new Rectangle(0, 0, numSingleWidth, numPicHeight), new Rectangle(number * numSingleWidth, 0, numSingleWidth, numPicHeight), GraphicsUnit.Pixel);
                }
                return bmp;
            }
            return null;
        }
        #endregion

        #region 向上冒泡事件
        void picTips_MouseEnter(object sender, EventArgs e)
        {
            base.OnMouseEnter(e);
        }

        void picTips_Click(object sender, EventArgs e)
        {
            base.OnClick(e);
        } 
        #endregion


    }
}
