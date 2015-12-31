using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using 右上角带数字.Properties;

namespace 右上角带数字
{
    internal sealed class MessageNumberGraphics
    {
        #region 属性
        private static Image _imgNumbers = null;
        private static Dictionary<int, Image> _imgNumberDict = null;
        /// <summary>
        /// 是否显示加号
        /// </summary>
        private static bool isShowPlus = false;

        /// <summary>
        /// 图片整体宽度
        /// </summary>
        private static int width = 0;
        #endregion

        #region 公共属性
        /// <summary>
        /// 最大上线，不包含这个
        /// </summary>
        private static int maxNum = 1000;
        /// <summary>
        /// 最大上线，不包含这个
        /// </summary>
        public static int MaxNum
        {
            get { return maxNum; }
            set { maxNum = value; }
        } 
        #endregion

        #region 构造函数
        static MessageNumberGraphics()
        {
            _imgNumbers = Resources.T1;
            _imgNumberDict = new Dictionary<int, Image>();
            for (int i = 0; i < 11; i++)
            {
                Image img = GetSingleNumberImage(i);
                _imgNumberDict.Add(i, img);
            }
        } 
        #endregion

        #region 获取整体图片
        /// <summary>
        /// 获取整体图片
        /// 数字+背景+显示的图片
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static Image GetMessageNumber(int number)
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
                    width = 8 * 2;
                else
                {
                    width = (len - 1) * 7 + 8 * 2;
                }

                Bitmap bmp = new Bitmap(width, 40);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    Image numberImage = GetNumberImage(number);
                    Image backImage = GetNumberBack(len);

                    //在指定的位置使用原始物理大小绘制指定的 Image。
                    g.DrawImage(Resources.T, new Point(0, 10));
                    g.DrawImage(backImage, new Point(0, 0));
                    g.DrawImage(numberImage, new Point(4, 1));
                }
                return bmp;
            }
            return null;
        }

        #endregion

        #region 获取要显示的数字图片
        /// <summary>
        /// 获取要显示的数字图片
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static Image GetNumberImage(int number)
        {
            if (number >= 0)
            {
                int len = number.ToString().Length;
                int numWidth = 7 * len;
                if (isShowPlus)
                {
                    numWidth = 7 * (len + 1 );
                }
                Bitmap bmp = new Bitmap(numWidth, 20);
                
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
                            g.DrawImage(img, new Rectangle(7 * (len - 1 - i), -1, img.Width, img.Height));
                        }
                    }
                    if (isShowPlus)
                    {
                        Image img = _imgNumberDict[10];
                        g.DrawImage(img, new Rectangle(numWidth - 7, -1, img.Width, img.Height));
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
        public static Image GetNumberBack(int len)
        {
            //因背景的半径是 8 而文字的宽度是7
            Bitmap bmp = new Bitmap(width, 20);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                //在指定位置并且按指定大小绘制指定的 Image 的指定部分。

                //四个参数的意思
                //要绘制的 Image
                //它指定所绘制图像的位置和大小。将图像进行缩放以适合该矩形。
                //它指定 image 对象中要绘制的部分。
                //枚举的成员，它指定 srcRect 参数所用的度量单位。
                g.DrawImage(_imgNumbers, new Rectangle(0, 0, 8, 20), new RectangleF(0, 20, 8, 20), GraphicsUnit.Pixel);
                g.DrawImage(_imgNumbers, new Rectangle(8, 0, width - 16, 20), new Rectangle(8, 20, 54, 20), GraphicsUnit.Pixel);
                g.DrawImage(_imgNumbers, new Rectangle(width - 8, 0, 8, 20), new Rectangle(72, 20, 8, 20), GraphicsUnit.Pixel);
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
        public static Image GetSingleNumberImage(int number)
        {
            if (number >= 0 && number < 11)
            {
                Bitmap bmp = new Bitmap(7, 20);
                //从指定的 Image 创建新的 Graphics。
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    //在指定的位置使用原始物理大小绘制指定的 Image。
                    g.DrawImage(_imgNumbers, new Rectangle(0, 0, 7, 20), new Rectangle(number * 7, 0, 7, 20), GraphicsUnit.Pixel);
                }
                return bmp;
            }
            return null;
        }
        #endregion
    }
}
