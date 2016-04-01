using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using 自定义TreeView仿VS解决方案效果.Properties;

namespace 自定义TreeView仿VS解决方案效果
{
    public partial class RichTextBoxForm : Form
    {
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, string lParam);
        public RichTextBoxForm()
        {
            InitializeComponent();
            this.richTextBox1.DragDrop += richTextBox1_DragDrop;
            this.richTextBox1.ContentsResized += richTextBox1_ContentsResized;
            this.richTextBox1.DragEnter += richTextBox1_DragEnter;

            RichTextBox rtb = new RichTextBox();
            rtb.Text = "contentcontentcontentcontentcontentcontentcontentcontentcontent321";
            rtb.Width = 200;
            rtb.ScrollBars = RichTextBoxScrollBars.None;
            rtb.Location = new Point(0,0);
            // this.rtb.Anchor = (AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top);这句代码有问题
            //得到RichTextBox高度
            int EM_GETLINECOUNT = 0x00BA;//获取总行数的消息号 
            int lc = SendMessage(rtb.Handle, EM_GETLINECOUNT, IntPtr.Zero, "");
            int sf = rtb.Font.Height * (lc + 1) + rtb.Location.Y;
            rtb.Height = sf;
            rtb.Resize += new EventHandler(richTextBox1_Resize);
            rtb.ContentsResized += richTextBox1_ContentsResized;
            this.Controls.Add(rtb);

        }
        void richTextBox1_Resize(object sender, EventArgs e)
        {
            int EM_GETLINECOUNT = 0x00BA;//获取总行数的消息号 
            int lc = SendMessage(this.richTextBox1.Handle, EM_GETLINECOUNT, IntPtr.Zero, "");
            int sf = this.richTextBox1.Font.Height * (lc + 1) + this.richTextBox1.Location.Y;
            this.richTextBox1.Height = sf;
        }

        void richTextBox1_ContentsResized(object sender, ContentsResizedEventArgs e)
        {
            RichTextBox rtb = sender as RichTextBox;
            rtb.Height = e.NewRectangle.Height + 10;
        }

        void richTextBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Effect == DragDropEffects.Move)
            {
                IDataObject data = e.Data;
                if (!data.GetDataPresent(DataFormats.FileDrop))
                {
                    Cursor = Cursors.NoMove2D;
                    e.Effect = DragDropEffects.None;
                }
            }
        }
        void richTextBox1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Effect == DragDropEffects.Move)
            {
                IDataObject data = e.Data;
                if (data.GetDataPresent(DataFormats.FileDrop))
                {
                    string[] sc = (string[])data.GetData(DataFormats.FileDrop);
                    //判断文件大小
                    for (int i = 0; i < sc.Count(); i++)
                    {
                        if (File.Exists(sc[i]))
                        {
                            //FileInfo fileInfo = new FileInfo(sc[i]);
                            //if (imgExt.Contains(fileInfo.Extension.ToLower()))
                            //{//插入图片
                            //    SetImageSize(sc[i]);
                            //}
                            //else
                            //{//插入文件
                            //    InsertFileUseGifBox(sc[i]);
                            //}
                        }
                        else
                        {
                            //文件已不存在
                            MessageBox.Show(sc[i] + " 文件已不存在！");
                        }
                    }
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
            Cursor = Cursors.IBeam;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetImage(Resources.minus);
            Clipboard.SetImage(Resources.plus);
            Clipboard.SetText("adf");
        }
    }
}


//第一种：
//richTextBox1.ScrollBars = RichTextBoxScrollBars.None;
//richTextBox.ContentsResized += new ContentsResizedEventHandler(richTextBox_ContentsResized);
//  private void richTextBox1_ContentsResized(object sender, ContentsResizedEventArgs e)
//  {
//  richTextBox1.Height = e.NewRectangle.Height+10;
//  }

//第二种：
//1.先调用以下方法：
//          [DllImport("user32.dll", EntryPoint = "SendMessageA")]
//          private static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, string lParam);
//2.设置RichTextBox：
//            this.richTextBox1 = new RichTextBox();           
//            this.richTextBox1.Text = “contentcontentcontentcontentcontentcontentcontentcontentcontent”;
//            this.richTextBox1.Width = this.pPanel.Width-15;
//            this.richTextBox1.ScrollBars = RichTextBoxScrollBars.None;
//            this.richTextBox1.Location = new Point(0, 0 + this.lab1.Height+10);
//           // this.richTextBox1.Anchor = (AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top);这句代码有问题
//            //得到RichTextBox高度
//            int EM_GETLINECOUNT = 0x00BA;//获取总行数的消息号 
//            int lc = SendMessage(this.richTextBox1.Handle, EM_GETLINECOUNT, IntPtr.Zero, "");
//            int sf = this.richTextBox1.Font.Height * (lc + 1) + this.richTextBox1.Location.Y;
//            this.richTextBox1.Height = sf;
//            this.richTextBox1.Resize += new EventHandler(richTextBox1_Resize);
//            this.Controls.Add(this.richTextBox1);
//3.设置RichTextBox的Resize:
//        void richTextBox1_Resize(object sender, EventArgs e)
//        {
//            int EM_GETLINECOUNT = 0x00BA;//获取总行数的消息号 
//            int lc = SendMessage(this.richTextBox1.Handle, EM_GETLINECOUNT, IntPtr.Zero, "");
//            int sf = this.richTextBox1.Font.Height * (lc + 1) + this.richTextBox1.Location.Y;
//            this.richTextBox1.Height = sf;
//        }