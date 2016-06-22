using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace RichTextBox消息处理
{
    public class RichTextBoxEx : ChatRichTextBox //ChatRichTextBox
    {
        private readonly object mustHideCaretLocker = new object();

        private bool mustHideCaret;

        [DefaultValue(false)]
        public bool MustHideCaret
        {
            get
            {
                lock (this.mustHideCaretLocker)
                    return this.mustHideCaret;
            }
            set
            {
                TabStop = false;
                if (value)
                    SetHideCaret();
                else
                    SetShowCaret();
            }
        }

        [DllImport("user32.dll")]
        public static extern int HideCaret(IntPtr hwnd);
        [DllImport("user32.dll", EntryPoint = "ShowCaret")]
        public static extern long ShowCaret(IntPtr hwnd);

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, string lParam);

        public RichTextBoxEx()
        {
            //this.ScrollBars = RichTextBoxScrollBars.None;
            //this.Width = 150;
            //int EM_GETLINECOUNT = 0x00BA;//获取总行数的消息号 
            //int lc = SendMessage(this.Handle, EM_GETLINECOUNT, IntPtr.Zero, "");
            //int sf = this.Font.Height * (lc + 1) + this.Location.Y;
            //this.Height = sf;
            //MessageBox.Show(lc.ToString() + "|" + this.Height.ToString());
        }

        private void SetHideCaret()
        {
            MouseDown += new MouseEventHandler(ReadOnlyRichTextBox_Mouse);
            MouseUp += new MouseEventHandler(ReadOnlyRichTextBox_Mouse);
            Resize += new EventHandler(ReadOnlyRichTextBox_Resize);
            HideCaret(Handle);
            lock (this.mustHideCaretLocker)
                this.mustHideCaret = true;
        }

        private void SetShowCaret()
        {
            try
            {
                MouseDown -= new MouseEventHandler(ReadOnlyRichTextBox_Mouse);
                MouseUp -= new MouseEventHandler(ReadOnlyRichTextBox_Mouse);
                Resize -= new EventHandler(ReadOnlyRichTextBox_Resize);
            }
            catch
            {
            }
            ShowCaret(Handle);
            lock (this.mustHideCaretLocker)
                this.mustHideCaret = false;
        }

        protected override void OnGotFocus(EventArgs e)
        {
            if (MustHideCaret)
                HideCaret(Handle);
        }

        protected override void OnEnter(EventArgs e)
        {
            if (MustHideCaret)
                HideCaret(Handle);
        }

        private void ReadOnlyRichTextBox_Mouse(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            HideCaret(Handle);
        }

        private void ReadOnlyRichTextBox_Resize(object sender, System.EventArgs e)
        {
            HideCaret(Handle);
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