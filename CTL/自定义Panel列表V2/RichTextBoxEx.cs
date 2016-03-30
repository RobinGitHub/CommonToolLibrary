using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace 自定义Panel列表V2
{
    public class RichTextBoxEx : RichTextBox
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int ShowScrollBar(IntPtr hWnd, int bar, bool show);

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(true), Category("其他"), Description("垂直滚动条是否显示")]

        //获取滚动条位置
        [DllImport("user32.dll", EntryPoint = "GetScrollPos")]
        public static extern int GetScrollPos(IntPtr hwnd, int nbar);
        [DllImport("user32.dll", EntryPoint = "SetScrollPos")]
        public static extern int SetScrollPos(IntPtr hwnd, int nbar, int nPos, bool bRedraw);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);



        //public bool VerticalScrollVisible
        //{
        //    set
        //    {
        //        ShowScrollBar(this.Handle, (int)ScrollBarTypes.SB_VERT, value);
        //    }
        //    get
        //    {
        //        return (Win32API.GetWindowLong(this.Handle, (int)SetWindowLongOffsets.GWL_STYLE) & (int)WindowStyles.WS_VSCROLL) != 0;
        //    }
        //}

        public int VerticalScrollValue
        {
            get
            {
                return GetScrollPos(this.Handle, 1);
            }
            set
            {
                SetScrollPos(this.Handle, 1, value, true);
                //移动内容
                SendMessage(this.Handle, (int)0x0115, (System.IntPtr)4, (System.IntPtr)0);
            }
        }
    }
}
