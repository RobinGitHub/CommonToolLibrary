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
            rtbSend.SelectionIndent = 1;
            rtbSend.SelectionRightIndent = 1;

            rtbReceive.SelectionIndent = 1;
            rtbReceive.SelectionRightIndent = 1;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {

            //rtbReceive.SelectionStart = rtbReceive.TextLength;
            //rtbReceive.SelectionIndent = 1;
            //rtbReceive.SelectionColor = Color.Red;
            //rtbReceive.SelectionAlignment = HorizontalAlignment.Left;
            //rtbReceive.SelectionFont = new Font(this.Font.FontFamily, 14);
            ////rtbReceive.SelectedText = "标题";
            //rtbReceive.AppendText("标题");
            //rtbReceive.AppendText(Environment.NewLine);

            //rtbReceive.SelectionIndent = 20;
            //rtbReceive.SelectionFont = new Font(this.Font.FontFamily, 9);
            //rtbReceive.AppendText("This text contains a hanging indent. The first sentence of the paragraph is spaced normally.This text contains a hanging indent. The first sentence of the paragraph is spaced normally.");
            ////rtbReceive.SelectedText = "This text contains a hanging indent. The first sentence of the paragraph is spaced normally.This text contains a hanging indent. The first sentence of the paragraph is spaced normally.";

            //rtbSend.Text = rtbReceive.Rtf;
            //rtbReceive.AppendText(Environment.NewLine);

            rtbReceive.InsertImageUseGifBox(txtFilePath.Text);
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
