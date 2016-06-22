using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using 图标格式转换.Properties;

namespace 图标格式转换
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(Application.StartupPath, "A.PNG");
            ImageHelper.CutForSquare(Resources.Msg_Area, filePath, 30, 1);
            pictureBox2.Image = Image.FromFile(filePath);
        }
    }
}
