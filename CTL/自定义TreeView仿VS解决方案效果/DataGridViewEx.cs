using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Win32API;

namespace 自定义TreeView仿VS解决方案效果
{
    public class DataGridViewEx : DataGridView
    {

        public bool VerticalScrollVisible
        {
            get
            {
                return (Win32API.Win32API.GetWindowLong(this.Handle, SetWindowLongOffsets.GWL_STYLE.GetHashCode()) & WindowStyles.WS_VSCROLL.GetHashCode()) != 0;
            }
        }
        public bool HorizontalScrollVisible
        {
            get
            {
                SCROLLINFO si = new SCROLLINFO();
                si.cbSize = (uint)Marshal.SizeOf(si);
                si.fMask = (uint)ScrollBarInfoFlags.SIF_ALL;
                Win32API.Win32API.GetScrollInfo(this.Handle, (int)ScrollBarTypes.SB_HORZ, ref si);
                Win32API.Win32API.GetScrollInfo(this.Handle, (int)ScrollBarTypes.SB_VERT, ref si);

                return (Win32API.Win32API.GetWindowLong(this.Handle, SetWindowLongOffsets.GWL_STYLE.GetHashCode()) & WindowStyles.WS_HSCROLL.GetHashCode()) != 0;
            }
        }
    }
}
