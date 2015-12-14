using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 自定义TreeView仿VS解决方案效果
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.AppendText("H:" + dataGridViewEx1.HorizontalScrollVisible.ToString() + "\n");
            richTextBox1.AppendText("V:" + dataGridViewEx1.VerticalScrollVisible.ToString() + "\n");
            richTextBox1.ScrollToCaret();
        }
    }
}
