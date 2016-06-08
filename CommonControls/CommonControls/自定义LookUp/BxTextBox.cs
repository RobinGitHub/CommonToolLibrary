using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace CommonControls
{
    /// <summary>
    /// 继承TextBox带下划线的输入框
    /// </summary>
    public partial class BxTextBox : System.Windows.Forms.TextBox
    {
        public BxTextBox()
        {
            this.BorderStyle = BorderStyle.None;
        }

        private int WM_PAINT = 0x000F;

        //使TextBox带下划线
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_PAINT)
            {
                Pen pen = new Pen(Brushes.Black, 1.5f);

                using (Graphics g = this.CreateGraphics())
                {
                    g.DrawLine(pen, new Point(0, this.Size.Height - 1), new Point(this.Size.Width, this.Size.Height - 1));
                }
            }
        }  
    }
}
