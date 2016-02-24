﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;

namespace RichTextBox消息处理
{
    /* 作者：Starts_2000
 * 日期：2009-09-13
 * 网站：http://www.csharpwin.com CS 程序员之窗。
 * 你可以免费使用或修改以下代码，但请保留版权信息。
 * 具体请查看 CS程序员之窗开源协议（http://www.csharpwin.com/csol.html）。
 */
    public class ChatRichTextBox : RichTextBox
    {

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hwnd, Int32 wMsg, Int32 wParam, ref Point pt);
        const int EM_GETSCROLLPOS = 0x0400 + 221;

        private RichEditOle _richEditOle;

        public ChatRichTextBox()
            : base()
        {
        }

        internal RichEditOle RichEditOle
        {
            get
            {
                if (_richEditOle == null)
                {
                    if (base.IsHandleCreated)
                    {
                        _richEditOle = new RichEditOle(this);
                    }
                }

                return _richEditOle;
            }
        }

        public bool InsertImageUseGifBox(string path)
        {
            try
            {
                GifBox gif = new GifBox();
                gif.FilePath = path;
                gif.BackColor = base.BackColor;
                Image img = Image.FromFile(path);
                gif.Image = img;
                RichEditOle.InsertControl(gif);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected override void OnLinkClicked(LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
            base.OnLinkClicked(e);
        }


        //private const int WM_DRAWCLIPBOARD = 0x308;

        //private const int WM_CHANGECBCHAIN = 0x30D;
        //[DllImport("user32")]
        //private static extern IntPtr SetClipboardViewer(IntPtr hwnd);

        //[DllImport("user32")]
        //private static extern IntPtr ChangeClipboardChain(IntPtr hwnd, IntPtr hWndNext);
        //[DllImport("user32")]
        //private static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);  


        //private IntPtr nextClipHwnd;  
        //protected override void WndProc(ref Message m)
        //{
        //    base.WndProc(ref m);
        //}

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                #region Control + V
                case Keys.Control | Keys.V:
                    System.Windows.Forms.IDataObject data = Clipboard.GetDataObject();
                    if (data.GetDataPresent(DataFormats.Text) | data.GetDataPresent(DataFormats.OemText))
                    {
                        string text = this.Text;

                        this.Text = text.Insert(this.SelectionStart, (String)data.GetData(DataFormats.Text));
                    }
                    else if (data.GetDataPresent(DataFormats.Bitmap) | data.GetDataPresent(DataFormats.Dib))
                    {
                        //图片
                        Bitmap photo = (Bitmap)data.GetData(typeof(Bitmap));
                        if (!Directory.Exists(Path.Combine(Application.StartupPath, "tmp")))
                        {
                            Directory.CreateDirectory(Path.Combine(Application.StartupPath, "tmp"));
                        }

                        string imagePath = Path.Combine(Application.StartupPath, string.Format(@"tmp/{0}.png", Guid.NewGuid()));
                        photo.Save(imagePath);
                        InsertImageUseGifBox(imagePath);
                    }
                    else if (data.GetDataPresent(DataFormats.FileDrop))
                    {
                        StringCollection sc = Clipboard.GetFileDropList();
                        List<string> imgExt = new List<string> { ".png", ".jpg", ".gif", ".bmp" };
                        for (int i = 0; i < sc.Count; i++)
                        {
                            if (File.Exists(sc[i]))
                            {
                                FileInfo fileInfo = new FileInfo(sc[i]);
                                if (imgExt.Contains(fileInfo.Extension))
                                {
                                    InsertImageUseGifBox(sc[i]);
                                }
                                else
                                {
                                    InsertImageUseGifBox(sc[i]);
                                }
                            }
                            else
                            {
                                //文件已不存在
                            }
                        }
                    }
                    else if (data.GetDataPresent(DataFormats.Locale))
                    {
                        //object obj = data.GetData(DataFormats.Locale);
                        //if (obj.GetType() == typeof(GifBox))
                        //{ 

                        //}


                    }
                    break;
                #endregion

                #region Control + C
                case Keys.Control | Keys.C:
                    ///对于文本&表情的复制，先将表情转为unicode，然后在粘贴时在转为图片， 对于在发送消息时的粘贴
                    /// 用json数据格式进行传递 {type:,content:} [text:],[pic:],[emoji:]
                    List<GifBox> gifList = RichEditOle.GetGIFInfo();

                    bool isCopyEmoji = false;

                    StringBuilder sb = new StringBuilder();


                    int lastIndex = 0;
                    if (gifList.Count > 0)
                    {
                        sb.Append("[");
                        foreach (GifBox gif in gifList)
                        {
                            //文本
                            string content = this.SelectedText.Substring(lastIndex, gif.Index - lastIndex);
                            if (!string.IsNullOrEmpty(content))
                            {
                                sb.Append("{");
                                sb.Append(string.Format("'type':'text', 'content':'{0}'", content));
                                sb.Append("},");
                            }

                            if (string.IsNullOrEmpty(gif.UnicodeText))
                            {//图片 主要是针对发送消息时的判断
                                sb.Append("{");
                                sb.Append(string.Format(@"'type':'pic', 'content':'{0}'", gif.FilePath));
                                sb.Append("},");
                            }
                            else
                            { //表情
                                sb.Append("{");
                                sb.Append(string.Format(@"'type':'emoji', 'content':'{0}|{1}'", gif.UnicodeText, @gif.FilePath));
                                sb.Append("},");
                            }
                            lastIndex = gif.Index + 1;
                        }
                        if (sb.Length > 1)
                        {
                            sb.Remove(sb.Length - 1, 1);
                        }
                        sb.Append("]");
                    }
                    else
                    {
                        sb.Append("[{");
                        sb.Append(string.Format("'type':'text', 'content':'{0}'", this.SelectedText));
                        sb.Append("}]");
                    }
                    try
                    {
                        JArray ja = JArray.Parse(sb.ToString());
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }

                    //Clipboard.SetText(sb.ToString());
                    break;
                #endregion
                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
            return true;
        }
    }
}
