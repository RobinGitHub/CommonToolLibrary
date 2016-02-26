using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

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
                    this.Paste();
                    //RichEditOle.GetGIFInfo();
                    List<Image> imgList = GetImagesFromRtf(this.Rtf);
                    foreach (var item in imgList)
                    {
                        if (!Directory.Exists(Path.Combine(Application.StartupPath, "tmp")))
                        {
                            Directory.CreateDirectory(Path.Combine(Application.StartupPath, "tmp"));
                        }

                        string imagePath = Path.Combine(Application.StartupPath, string.Format(@"tmp/{0}.png", Guid.NewGuid()));
                        item.Save(imagePath);
                        InsertImageUseGifBox(imagePath);
                    }
                    //System.Windows.Forms.IDataObject data = Clipboard.GetDataObject();
                    //if (data.GetDataPresent(DataFormats.Text) | data.GetDataPresent(DataFormats.OemText))
                    //{
                    //    string text = this.Text;
                    //    //this.Text = text.Insert(this.SelectionStart, (String)data.GetData(DataFormats.Text));
                    //    try
                    //    {
                    //        string content = (String)data.GetData(DataFormats.Text);
                    //        //JArray ja = JArray.Parse(content.Replace(@"\", @"\\"));

                    //        //foreach (var item in ja)
                    //        //{
                    //        //    int insertIndex = this.SelectionStart;
                    //        //    string value = JsonConvert.DeserializeObject(item["content"].ToString()).ToString();
                    //        //    switch (JsonConvert.DeserializeObject(item["type"].ToString()).ToString())
                    //        //    {
                    //        //        case "text":
                    //        //            this.SelectedText = value;
                    //        //            insertIndex += value.Length;
                    //        //            this.SelectionStart = insertIndex;
                    //        //            break;
                    //        //        case "pic":
                    //        //            this.InsertImageUseGifBox(value);
                    //        //            insertIndex += 1;
                    //        //            this.SelectionStart = insertIndex;
                    //        //            break;
                    //        //        case "emoji":
                    //        //            //一共有2项，第一项是Unicode,第二项是图片位置
                    //        //            string[] split = value.Split(new char[] { '|' });
                    //        //            //this.InsertImageUseGifBox(split[1], split[0]);
                    //        //            this.InsertImageUseGifBox(value);
                    //        //            insertIndex += 1;
                    //        //            this.SelectionStart = insertIndex;
                    //        //            break;
                    //        //    }
                    //        //}
                    //    }
                    //    catch (Exception)
                    //    {

                    //        throw;
                    //    }
                    //}
                    //if (data.GetDataPresent(DataFormats.Bitmap) | data.GetDataPresent(DataFormats.Dib))
                    //{
                    //    //图片
                    //    Bitmap photo = (Bitmap)data.GetData(typeof(Bitmap));
                    //    if (!Directory.Exists(Path.Combine(Application.StartupPath, "tmp")))
                    //    {
                    //        Directory.CreateDirectory(Path.Combine(Application.StartupPath, "tmp"));
                    //    }

                    //    string imagePath = Path.Combine(Application.StartupPath, string.Format(@"tmp/{0}.png", Guid.NewGuid()));
                    //    photo.Save(imagePath);
                    //    InsertImageUseGifBox(imagePath);
                    //}
                    //if (data.GetDataPresent(DataFormats.FileDrop))
                    //{
                    //    StringCollection sc = Clipboard.GetFileDropList();
                    //    List<string> imgExt = new List<string> { ".png", ".jpg", ".gif", ".bmp" };
                    //    for (int i = 0; i < sc.Count; i++)
                    //    {
                    //        if (File.Exists(sc[i]))
                    //        {
                    //            FileInfo fileInfo = new FileInfo(sc[i]);
                    //            if (imgExt.Contains(fileInfo.Extension))
                    //            {
                    //                InsertImageUseGifBox(sc[i]);
                    //            }
                    //            else
                    //            {
                    //                InsertImageUseGifBox(sc[i]);
                    //            }
                    //        }
                    //        else
                    //        {
                    //            //文件已不存在
                    //        }
                    //    }
                    //}
                    break;
                #endregion

                #region Control + C
                case Keys.Control | Keys.C:
                    ///对于文本&表情的复制，先将表情转为unicode，然后在粘贴时在转为图片， 对于在发送消息时的粘贴
                    /// 用json数据格式进行传递 {type:,content:} [text:],[pic:],[emoji:]
                    List<GifBox> gifList = RichEditOle.GetGIFInfo();

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
                                sb.Append(string.Format("'type':'text', 'content':'{0}'", JsonConvert.SerializeObject(content).ToString()));
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
                                sb.Append(string.Format(@"'type':'emoji', 'content':'{0}|{1}'", gif.UnicodeText, gif.FilePath));
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
                    Clipboard.SetText(sb.ToString());
                    break;
                #endregion
                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
            return true;
        }

        private List<Image> GetImagesFromRtf(string rtfText)
        {
            List<Image> imageList = new List<Image>();
            int width = 0;
            int height = 0;
            int.TryParse(Regex.Match(rtfText, @"(?<=picw)[\d]+(?=\\pich)").Value, out width);
            if (width != 0)
                width = (int)(width / 26);

            int.TryParse(Regex.Match(rtfText, @"(?<=pich)[\d]+(?=\\picwgoal)").Value, out height);
            if (height != 0)
                height = (int)(height / 26);
            while (true)
            {
                int _Index = rtfText.IndexOf("pichgoal");
                if (_Index == -1) break;
                rtfText = rtfText.Remove(0, _Index + 8);

                _Index = rtfText.IndexOf("\r\n");

                int _Temp = Convert.ToInt32(rtfText.Substring(0, _Index));
                rtfText = rtfText.Remove(0, _Index);

                _Index = rtfText.IndexOf("}");
                string imgByteStr = rtfText.Substring(0, _Index).Replace("\r\n", "");

                rtfText = rtfText.Remove(0, _Index);

                int _Count = imgByteStr.Length / 2;
                byte[] bts = new byte[_Count];
                for (int z = 0; z != _Count; z++)
                {
                    string _TempText = imgByteStr[z * 2].ToString() + imgByteStr[(z * 2) + 1].ToString();
                    bts[z] = Convert.ToByte(_TempText, 16);
                }

                System.IO.MemoryStream ms = new System.IO.MemoryStream(bts);
                System.Drawing.Image img = System.Drawing.Image.FromStream(ms);

                using (var oriBmp = new Bitmap(img, width, height))
                {
                    img = (Image)oriBmp.Clone();
                }
                imageList.Add(img);
            }

            return imageList;
        }
    }
}
