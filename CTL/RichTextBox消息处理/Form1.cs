using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RichTextBox消息处理
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            //rtbReceive.SelectedRtf = rtbSend.Rtf;
        }

        private void btnFilePath_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtFilePath.Text = openFileDialog1.FileName;
                rtbSend.InsertImageUseGifBox(txtFilePath.Text);
            }
        }

        private void btnGetRTF_Click(object sender, EventArgs e)
        {
            rtbReceive.AppendText(rtbSend.Rtf);
        }
    }
}
