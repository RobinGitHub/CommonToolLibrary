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
        private static Image _imgNumbers = null;
        private static Dictionary<int, Image> _imgNumberDict = null;

        static MessageNumberGraphics()
        {
            _imgNumbers = Resources.MessageNumbers;
            _imgNumberDict = new Dictionary<int, Image>();
            for (int i = 0; i < 10; i++)
            {
                Image img = GetSingleNumberImage(i);
                _imgNumberDict.Add(i, img);
            }
        }

        public static Image GetMessageNumber(int number)
        {
            if (number >= 0)
            {
                int len = number.ToString().Length;
                Bitmap bmp = new Bitmap((len + 1) * 10 + 8, 40);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    Image numberImage = GetNumberImage(number);
                    Image backImage = GetNumberBack(len);

                    //在指定的位置使用原始物理大小绘制指定的 Image。
                    g.DrawImage(Resources.T, new Point(0, 10));
                    g.DrawImage(backImage, new Point(8, 0));
                    g.DrawImage(numberImage, new Point(13, 0));
                }
                return bmp;
            }
            return null;
        }

        private static Image GetNumberBack(int len)
        {
            Bitmap bmp = new Bitmap((len + 1) * 10, 20);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                //在指定位置并且按指定大小绘制指定的 Image 的指定部分。

                //四个参数的意思
                //要绘制的 Image
                //它指定所绘制图像的位置和大小。将图像进行缩放以适合该矩形。
                //它指定 image 对象中要绘制的部分。
                //枚举的成员，它指定 srcRect 参数所用的度量单位。
                g.DrawImage(_imgNumbers, new Rectangle(0, 0, 10, 20), new RectangleF(0, 20, 10, 20), GraphicsUnit.Pixel);
                g.DrawImage(_imgNumbers, new Rectangle(10, 0, (len - 1) * 10, 20), new Rectangle(10, 20, 80, 20), GraphicsUnit.Pixel);
                g.DrawImage(_imgNumbers, new Rectangle(len * 10, 0, 10, 20), new Rectangle(90, 20, 10, 20), GraphicsUnit.Pixel);
            }
            return bmp;
        }

        public static Image GetNumberImage(int number)
        {
            if (number >= 0)
            {
                int len = number.ToString().Length;
                Bitmap bmp = new Bitmap(10 * len, 20);
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
                            g.DrawImage(img, new Rectangle(10 * (len - 1 - i), -1, img.Width, img.Height));
                        }
                    }
                }
                return bmp;
            }
            return null;
        }

        private static Image GetSingleNumberImage(int number)
        {
            if (number >= 0 && number < 10)
            {
                Bitmap bmp = new Bitmap(10, 20);
                //从指定的 Image 创建新的 Graphics。
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    //在指定的位置使用原始物理大小绘制指定的 Image。
                    g.DrawImage(_imgNumbers, new Point(-number * 10, 0));
                }
                return bmp;
            }
            return null;
        }
    }
}
