using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace 获取系统托盘图标位置
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //bool isHide = false;
            //GetIconRec("Haobitou.exe", "深圳", out isHide);

            Cursor.Position = new Point(1170, 862);
        }

        /// <summary>
        /// 获取系统托盘图标位置
        /// </summary>
        /// <param name="exeName">程序名称：QQ.exe</param>
        /// <param name="title">QQ用户名称</param>
        /// <param name="isHide">是否再隐藏</param>
        /// <returns></returns>
        private Rectangle GetIconRec(string exeName, string title, out bool isHide)
        {
            Rectangle rlt = new Rectangle();
            IntPtr pid = IntPtr.Zero;
            IntPtr ipHandle = IntPtr.Zero; //图标句柄 
            IntPtr lTextAdr = IntPtr.Zero; //文本内存地址 

            IntPtr ipTray = TrayToolbarWindow32();


            GetWindowThreadProcessId(ipTray, ref pid);

            IntPtr hProcess = OpenProcess(PROCESS_ALL_ACCESS | PROCESS_VM_OPERATION | PROCESS_VM_READ | PROCESS_VM_WRITE, IntPtr.Zero, pid);
            IntPtr lAddress = VirtualAllocEx(hProcess, 0, 4096, MEM_COMMIT, PAGE_READWRITE);

            //得到图标个数 
            int lButton = SendMessage(ipTray, TB_BUTTONCOUNT, 0, 0);

            RECT rct;
            if (!(GetWindowRect(new HandleRef(null, ipTray), out rct)))
            {
            }

            //图标宽度
            int iconWidth = 25;
            //每行图标显示的个数，考虑多行显示的情况
            int disRowCount = (int)Math.Ceiling((rct.Right - rct.Left) / (decimal)iconWidth);
            //显示行数
            int disLines = (int)Math.Ceiling(lButton / (decimal)disRowCount);

            int iconHeight = (rct.Bottom - rct.Top) / disLines;

            isHide = true;
            for (int i = 0; i < lButton; i++)
            {
                SendMessage(ipTray, TB_GETBUTTON, i, lAddress);

                //读文本地址 
                ReadProcessMemory(hProcess, (IntPtr)(lAddress.ToInt32() + 16), ref lTextAdr, 4, 0);

                if (!lTextAdr.Equals(-1))
                {
                    byte[] buff = new byte[1024];

                    ReadProcessMemory(hProcess, lTextAdr, buff, 1024, 0);//读文本 

                    string tmpExeName = string.Empty;
                    string tmpExeTitle = string.Empty;
                    string tmpTitle = System.Text.ASCIIEncoding.Unicode.GetString(buff).Replace("\0", "");
                    int index = tmpTitle.IndexOf(".exe");
                    int fIndex = tmpTitle.LastIndexOf("\\", index);

                    tmpExeName = tmpTitle.Substring(fIndex + 1, index - fIndex + 3);
                    tmpExeTitle = tmpTitle.Substring(index + 4);
                    if (exeName.ToUpper() == tmpExeName.ToUpper() && tmpExeTitle.Contains(title))
                    {
                        int offsetX = i % disRowCount * iconWidth;
                        rlt = new Rectangle(rct.Left + offsetX, rct.Top, iconWidth, iconHeight);
                        isHide = false;
                        break;
                    }
                }
            }
            if (isHide)
            {
                rlt = new Rectangle(rct.Left - 21, rct.Top, 21, rct.Bottom - rct.Top);
            }
            return rlt;
        }




        //获取托盘指针 
        private static IntPtr TrayToolbarWindow32()
        {
            IntPtr h = IntPtr.Zero;
            IntPtr hTemp = IntPtr.Zero;

            h = FindWindow("Shell_TrayWnd", null); //托盘容器 
            h = FindWindowEx(h, IntPtr.Zero, "TrayNotifyWnd", null);//找到托盘 
            h = FindWindowEx(h, IntPtr.Zero, "SysPager", null);

            hTemp = FindWindowEx(h, IntPtr.Zero, "ToolbarWindow32", null);

            return hTemp;
        }



        [DllImport("user32.dll")]
        static extern bool GetWindowRect(HandleRef handle, out RECT rct);

        [StructLayout(LayoutKind.Sequential)]
        struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public const int WM_USER = 0x400;
        public const int WM_CLOSE = 0x10;
        public const int WM_GETTEXT = 0x000D;
        public const int WM_SETTEXT = 0x000C;

        public const int STANDARD_RIGHTS_REQUIRED = 0xF0000;
        public const int SYNCHRONIZE = 0x100000;
        public const int PROCESS_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0xFFF;
        public const int PROCESS_TERMINATE = 0x1;

        public const int PROCESS_VM_OPERATION = 0x8;
        public const int PROCESS_VM_READ = 0x10;
        public const int PROCESS_VM_WRITE = 0x20;

        public const int MEM_RESERVE = 0x2000;
        public const int MEM_COMMIT = 0x1000;
        public const int MEM_RELEASE = 0x8000;

        public const int PAGE_READWRITE = 0x4;

        public const int TB_BUTTONCOUNT = (WM_USER + 24);
        public const int TB_HIDEBUTTON = (WM_USER + 4);
        public const int TB_GETBUTTON = (WM_USER + 23);
        public const int TB_GETBUTTONTEXT = WM_USER + 75;
        public const int TB_GETBITMAP = (WM_USER + 44);
        public const int TB_DELETEBUTTON = (WM_USER + 22);
        public const int TB_ADDBUTTONS = (WM_USER + 20);
        public const int TB_INSERTBUTTON = (WM_USER + 21);
        public const int TB_ISBUTTONHIDDEN = (WM_USER + 12);
        public const int ILD_NORMAL = 0x0;
        public const int TPM_NONOTIFY = 0x80;

        public const int WS_VISIBLE = 268435456;//窗体可见 
        public const int WS_MINIMIZEBOX = 131072;//有最小化按钮 
        public const int WS_MAXIMIZEBOX = 65536;//有最大化按钮 
        public const int WS_BORDER = 8388608;//窗体有边框 
        public const int GWL_STYLE = (-16);//窗体样式 
        public const int GW_HWNDFIRST = 0;
        public const int GW_HWNDNEXT = 2;
        public const int SW_HIDE = 0;
        public const int SW_SHOW = 5;

        [DllImport("User32.Dll")]
        public static extern void GetClassName(IntPtr hwnd, StringBuilder s, int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow", SetLastError = true)]
        public static extern void SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32.dll", EntryPoint = "GetDlgItem", SetLastError = true)]
        public static extern IntPtr GetDlgItem(int nID, IntPtr phWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int RegisterWindowMessage(string msg);

        [DllImport("kernel32", EntryPoint = "OpenProcess")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, IntPtr bInheritHandle, IntPtr dwProcessId);

        [DllImport("kernel32", EntryPoint = "CloseHandle")]
        public static extern int CloseHandle(IntPtr hObject);

        [DllImport("user32", EntryPoint = "GetWindowThreadProcessId")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hwnd, ref IntPtr lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        [DllImport("user32", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);

        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        public static extern int ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, ref IntPtr lpBuffer, int nSize, int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", EntryPoint = "ReadProcessMemory")]
        public static extern bool ReadProcessMemoryEx(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);

        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        public static extern int ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, int lpNumberOfBytesWritten);

        [DllImport("kernel32", EntryPoint = "WriteProcessMemory")]
        public static extern int WriteProcessMemory(IntPtr hProcess, ref int lpBaseAddress, ref int lpBuffer, int nSize, ref int lpNumberOfBytesWritten);

        [DllImport("kernel32", EntryPoint = "VirtualAllocEx")]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, int lpAddress, int dwSize, int flAllocationType, int flProtect);

        [DllImport("kernel32", EntryPoint = "VirtualFreeEx")]
        public static extern int VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, int dwFreeType);

        [DllImport("User32.dll")]
        public extern static int GetWindow(int hWnd, int wCmd);

        [DllImport("User32.dll")]
        public extern static int GetWindowLongA(int hWnd, int wIndx);

        [DllImport("user32.dll")]
        public static extern bool GetWindowText(int hWnd, StringBuilder title, int maxBufSize);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public extern static int GetWindowTextLength(IntPtr hWnd);

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

    }
}
