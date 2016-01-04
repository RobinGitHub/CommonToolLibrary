using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

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
                gif.BackColor = base.BackColor;
                gif.Image = Image.FromFile(path);
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

    }
}
