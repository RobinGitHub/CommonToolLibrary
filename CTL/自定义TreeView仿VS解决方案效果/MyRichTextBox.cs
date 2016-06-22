using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Win32API;

namespace 自定义TreeView仿VS解决方案效果
{
    public class MyRichTextBox : RichTextBox
    {
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(0), Category("其他"), Description("垂直滚动条的值")]
        public int VerticalScrollValue
        {
            get
            {
                return Win32API.Win32API.GetScrollPos(this.Handle, ScrollBarTypes.SB_VERT.GetHashCode());
            }
            set
            {
                Win32API.Win32API.SetScrollPos(this.Handle, ScrollBarTypes.SB_VERT.GetHashCode(), value, true);
                //移动内容
                ///用垂直滚动条的方式没有效果，经过调试，如果直接移动本身的滚动条 wParam 值是随时变化的
                ///而滚动条的值 是通过 HiWord 方法算出来的
                ///而这里需要反推即：通过滚动条的值 算出 wParam 的值
                ///这里为什么要 +5 因为算出来的值与本身滚动条的值相差5，具体原因不明
                int wParam = (value << 16) + 5;
                Win32API.Win32API.SendMessage(this.Handle, WinMsg.WM_VSCROLL.GetHashCode(), (System.IntPtr)wParam, (System.IntPtr)0);
            }
        }
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(true), Category("其他"), Description("垂直滚动条是否显示")]
        public bool VerticalScrollVisible
        {
            set
            {
                Win32API.Win32API.ShowScrollBar(this.Handle, (int)ScrollBarTypes.SB_VERT, value);
            }
            get
            {
                return (Win32API.Win32API.GetWindowLong(this.Handle, (int)SetWindowLongOffsets.GWL_STYLE) & (int)WindowStyles.WS_VSCROLL) != 0;
            }
        }

        private Rectangle displayContentRectangle = new Rectangle();
        public Rectangle DisplayContentRectangle
        {
            get
            {
                return displayContentRectangle;
            }
        }


        protected override void OnContentsResized(ContentsResizedEventArgs e)
        {
            displayContentRectangle = e.NewRectangle;
            base.OnContentsResized(e);
        }

    }
}
