using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace 无边框窗体边框阴影效果与拖动
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            SetClassLong(this.Handle, GCL_STYLE, GetClassLong(this.Handle, GCL_STYLE) | CS_DropSHADOW); //API函数加载，实现窗体边框阴影效果

        }

        #region API 控制无边框窗口的移动与缩放

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        public const int HWND_BROADCAST = 0xFFFF;
        public const int WM_FONTCHANGE = 0x1D;

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public static int WM_SYSCOMMAND = 0x0112;
        public static int SC_MOVE = 0xF010;
        public static int HTCAPTION = 0x0002;

        /// <summary>
        /// 无边框窗口移动
        /// </summary>
        protected void OnFormMouseMove()
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }


        public struct POINTAPI
        {
            public int x;
            public int y;
        }

        public struct MINMAXINFO
        {
            public POINTAPI ptReserved;
            public POINTAPI ptMaxSize;
            public POINTAPI ptMaxPosition;
            public POINTAPI ptMinTrackSize;
            public POINTAPI ptMaxTrackSize;
        }


        private const long WM_GETMINMAXINFO = 0x24;

        const int HTLEFT = 10;
        const int HTRIGHT = 11;
        const int HTTOP = 12;
        const int HTTOPLEFT = 13;
        const int HTTOPRIGHT = 14;
        const int HTBOTTOM = 15;
        const int HTBOTTOMLEFT = 0x10;
        const int HTBOTTOMRIGHT = 17;
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            switch (m.Msg)
            {
                case 0x0084:
                    base.WndProc(ref m);
                    Point vPoint = new Point((int)m.LParam & 0xFFFF,
                        (int)m.LParam >> 16 & 0xFFFF);
                    vPoint = PointToClient(vPoint);
                    if (vPoint.X <= 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)HTTOPLEFT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)HTBOTTOMLEFT;
                        else m.Result = (IntPtr)HTLEFT;
                    else if (vPoint.X >= ClientSize.Width - 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)HTTOPRIGHT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)HTBOTTOMRIGHT;
                        else m.Result = (IntPtr)HTRIGHT;
                    else if (vPoint.Y <= 5)
                        m.Result = (IntPtr)HTTOP;
                    else if (vPoint.Y >= ClientSize.Height - 5)
                        m.Result = (IntPtr)HTBOTTOM;
                    break;
                case 0x24:
                    MINMAXINFO mmi = (MINMAXINFO)m.GetLParam(typeof(MINMAXINFO));
                    mmi.ptMinTrackSize.x = this.MinimumSize.Width;
                    mmi.ptMinTrackSize.y = this.MinimumSize.Height;
                    if (this.MaximumSize.Width != 0 || this.MaximumSize.Height != 0)
                    {
                        mmi.ptMaxTrackSize.x = this.MaximumSize.Width;
                        mmi.ptMaxTrackSize.y = this.MaximumSize.Height;
                    }
                    mmi.ptMaxPosition.x = 0;
                    mmi.ptMaxPosition.y = 0;
                    System.Runtime.InteropServices.Marshal.StructureToPtr(mmi, m.LParam, true);

                    base.WndProc(ref m);
                    break;

                case 0x0011:
                    m.Result = (IntPtr)1;

                    base.WndProc(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        #endregion

        #region 阴影代码
        //阴影代码
        public const int CS_DropSHADOW = 0x20000;
        public const int GCL_STYLE = (-26);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassLong(IntPtr hwnd, int nIndex); 
        #endregion

        private void pnlTitle_MouseDown(object sender, MouseEventArgs e)
        {
            OnFormMouseMove();
        }



    }

}
