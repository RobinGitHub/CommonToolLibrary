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

                string rtf = rtbSend.Rtf;

                string key = "wmetafile0";
                int startIndex = rtf.IndexOf(key);
                //rtf = rtf.Insert(startIndex + key.Length, @"{\*\123 N}");
                rtf = rtf.Replace("wmetafile0", "");

                rtf = rtf.Insert(startIndex, @"{\*\123 N wmetafile0}");
                
                rtbReceive.Text = rtf + "\n";
                rtbSend.Rtf = rtf;
                rtbReceive.AppendText(rtbSend.Rtf);

                
            }
        }

        private void btnGetRTF_Click(object sender, EventArgs e)
        {
            rtbReceive.AppendText(rtbSend.Rtf);
        }
        /// 
        /// 添加一个文件资源到RTF数据
        /// 
        /// 文件路径    
        public void AddFile(string p_FileFullPath)
        {
            byte[] _FileBytes = File.ReadAllBytes(p_FileFullPath);
            Image _Image = Image.FromStream(new MemoryStream(_FileBytes));
            string _Guid = BitConverter.ToString(Guid.NewGuid().ToByteArray()).Replace("-", "");
            StringBuilder _RtfText = new StringBuilder(@"{\rtf1\ansi\ansicpg936\deff0\deflang1033\deflangfe2052{\fonttbl{\f0\fnil\fcharset134 \'cb\'ce\'cc\'e5;}}\uc1\pard\lang2052\f0\fs18{\object\objemb{\*\objclass Paint.Picture}");
            int _Width = _Image.Width * 15;
            int _Height = _Image.Height * 15;
            _RtfText.Append(@"\objw" + _Width.ToString() + @"\objh" + _Height.ToString());
            _RtfText.AppendLine(@"{\*\objdata");
            _RtfText.AppendLine(@"010500000200000007000000504272757368000000000000000000" + BitConverter.ToString(BitConverter.GetBytes(_FileBytes.Length + 20)).Replace("-", ""));
            _RtfText.Append("7A676B65" + _Guid); //标记            
            _RtfText.AppendLine(BitConverter.ToString(_FileBytes).Replace("-", ""));
            _RtfText.AppendLine(@"0105000000000000}{\result{\pict\wmetafile0}}}}");
           

        }
    }
}
