using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using 图标格式转换.Properties;

namespace 图标格式转换
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            notifyIcon1.BalloonTipTitle = "AAA";
            notifyIcon1.Text = "asdf";
            this.Load += Form1_Load;
        }

        void Form1_Load(object sender, EventArgs e)
        {

            //IntPtr rlt = NotifyHelper.FindNotifyIcon(notifyIcon1.Text);
            //string rlt1 = rlt.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Icon icon = BitmapToIcon(Resources.a, true);
            FileStream fs = new FileStream("a.ico", FileMode.Create);
            icon.Save(fs);//将Icon保存的指定的输出
            fs.Close();
            //notifyIcon1.Icon = BitmapToIcon(Resources._1, true);

        }

        #region 图片转为图标
        /// <summary>
        /// 图片转为图标
        /// </summary>
        /// <param name="obm"></param>
        /// <param name="preserve"></param>
        /// <returns></returns>
        public Icon BitmapToIcon(Bitmap obm, bool preserve)
        {
            int ICON_W = obm.Width;
            int ICON_H = obm.Height;
            Bitmap bm;
            // if not preserving aspect
            if (!preserve)        // if not preserving aspect
                bm = new Bitmap(obm, ICON_W, ICON_H);  //   rescale from original bitmap

            // if preserving aspect drop excess significance in least significant direction
            else          // if preserving aspect
            {
                Rectangle rc = new Rectangle(0, 0, ICON_W, ICON_H);
                if (obm.Width >= obm.Height)   //   if width least significant
                {          //     rescale with width based on max icon height
                    bm = new Bitmap(obm, (ICON_H * obm.Width) / obm.Height, ICON_H);
                    rc.X = (bm.Width - ICON_W) / 2;  //     chop off excess width significance
                    if (rc.X < 0) rc.X = 0;
                }
                else         //   if height least significant
                {          //     rescale with height based on max icon width
                    bm = new Bitmap(obm, ICON_W, (ICON_W * obm.Height) / obm.Width);
                    rc.Y = (bm.Height - ICON_H) / 2; //     chop off excess height significance
                    if (rc.Y < 0) rc.Y = 0;
                }
                bm = bm.Clone(rc, bm.PixelFormat);  //   bitmap for icon rectangle
            }

            // create icon from bitmap
            Icon icon = Icon.FromHandle(bm.GetHicon()); // create icon from bitmap
            bm.Dispose();        // dispose of bitmap
            return icon;        // return icon
        }
        #endregion

       
    }
}
