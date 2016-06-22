using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace 图标格式转换
{
    public sealed class NotifyHelper
    {
        private static int WM_TASKBARCREATED = RegisterWindowMessage("TaskbarCreated");
        private const int WM_USER = 0x400;
        private const int WM_TRAYMOUSEMESSAGE = 2048;
        private const int WM_MOUSEMOVE = 0x200;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_LBUTTONUP = 0x202;
        private const int WM_LBUTTONDBLCLK = 0x203;
        private const int WM_RBUTTONDOWN = 0x204;
        private const int WM_RBUTTONUP = 0x205;
        private const int WM_RBUTTONDBLCLK = 0x206;
        private const int WM_MBUTTONDOWN = 0x207;
        private const int WM_MBUTTONUP = 0x208;
        private const int WM_MBUTTONDBLCLK = 0x209;
        private const int NIN_BALLOONSHOW = 0x402;
        private const int NIN_BALLOONHIDE = 0x403;
        private const int NIN_BALLOONTIMEOUT = 0x404;
        private const int NIN_BALLOONUSERCLICK = 0x405;

        private const int READ_CONTROL = 0x20000;
        private const int STANDARD_RIGHTS_REQUIRED = 0xF0000;
        private const int STANDARD_RIGHTS_READ = READ_CONTROL;
        private const int STANDARD_RIGHTS_EXECUTE = READ_CONTROL;
        private const int STANDARD_RIGHTS_ALL = 0x1F0000;
        private const int STANDARD_RIGHTS_WRITE = READ_CONTROL;
        private const int SYNCHRONIZE = 0x100000;
        private const int PROCESS_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0xFFF;
        private const int PROCESS_TERMINATE = 0x1;

        private const int PROCESS_VM_OPERATION = 0x8;
        private const int PROCESS_VM_READ = 0x10;
        private const int PROCESS_VM_WRITE = 0x20;
        private const int MEM_RESERVE = 0x2000;
        private const int MEM_COMMIT = 0x1000;
        private const int MEM_RELEASE = 0x8000;
        private const int PAGE_READWRITE = 0x4;

        private const int TB_BUTTONCOUNT = (WM_USER + 24);
        private const int TB_HIDEBUTTON = (WM_USER + 4);
        private const int TB_GETBUTTON = (WM_USER + 23);
        private const int TB_GETBITMAP = (WM_USER + 44);
        private const int TB_DELETEBUTTON = (WM_USER + 22);
        private const int TB_ADDBUTTONS = (WM_USER + 20);
        private const int TB_INSERTBUTTON = (WM_USER + 21);
        private const int TB_ISBUTTONHIDDEN = (WM_USER + 12);
        private const int ILD_NORMAL = 0x0;

        private const int TPM_NONOTIFY = 0x80;

        #region Win32 API 引用

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr PostMessage(HandleRef hwnd, int msg, int wparam, int lparam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool PostMessage(HandleRef hwnd, int msg, IntPtr wparam, IntPtr lparam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern bool SetForegroundWindow(HandleRef hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int RegisterWindowMessage(string msg);

        [DllImport("kernel32", EntryPoint = "OpenProcess")]
        private static extern IntPtr OpenProcess(
            int dwDesiredAccess,
            IntPtr bInheritHandle,
            IntPtr dwProcessId
            );
        [DllImport("kernel32", EntryPoint = "CloseHandle")]
        private static extern int CloseHandle(
            IntPtr hObject
            );
        [DllImport("user32", EntryPoint = "GetWindowThreadProcessId")]
        private static extern IntPtr GetWindowThreadProcessId(
            IntPtr hwnd,
            ref IntPtr lpdwProcessId
            );
        [DllImport("user32", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(
            string lpClassName,
            string lpWindowName
            );
        [DllImport("user32", EntryPoint = "FindWindowEx")]
        private static extern IntPtr FindWindowEx(
            IntPtr hWnd1,
            IntPtr hWnd2,
            string lpsz1,
            string lpsz2
            );
        [DllImport("user32", EntryPoint = "SendMessage")]
        private static extern int SendMessage(
            IntPtr hwnd,
            int wMsg,
            int wParam,
            int lParam
            );
        [DllImport("user32", EntryPoint = "SendMessage")]
        private static extern int SendMessage(
            IntPtr hwnd,
            int wMsg,
            int wParam,
            IntPtr lParam
            );
        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        private static extern int ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            ref IntPtr lpBuffer,
            int nSize,
            int lpNumberOfBytesWritten
            );
        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        private static extern int ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            byte[] lpBuffer,
            int nSize,
            int lpNumberOfBytesWritten
            );
        [DllImport("kernel32", EntryPoint = "WriteProcessMemory")]
        private static extern int WriteProcessMemory(
            IntPtr hProcess,
            ref int lpBaseAddress,
            ref int lpBuffer,
            int nSize,
            ref int lpNumberOfBytesWritten
            );
        [DllImport("kernel32", EntryPoint = "VirtualAllocEx")]
        private static extern IntPtr VirtualAllocEx(
            IntPtr hProcess,
            int lpAddress,
            int dwSize,
            int flAllocationType,
            int flProtect
            );
        [DllImport("kernel32", EntryPoint = "VirtualFreeEx")]
        private static extern int VirtualFreeEx(
            IntPtr hProcess,
            IntPtr lpAddress,
            int dwSize,
            int dwFreeType
            );
        #endregion

        public static IntPtr FindNotifyIcon(string TipTitle)
        {
            if (TipTitle.Length == 0) return IntPtr.Zero;
            IntPtr pid = IntPtr.Zero;
            IntPtr ipHandle = IntPtr.Zero; //圖標句柄
            IntPtr lTextAdr = IntPtr.Zero; //文本內存地址
            IntPtr ipTemp = FindWindow("Shell_TrayWnd", null);
            //找到托盤
            ipTemp = FindWindowEx(ipTemp, IntPtr.Zero, "TrayNotifyWnd", null);
            ipTemp = FindWindowEx(ipTemp, IntPtr.Zero, "SysPager", null);
            IntPtr ipTray = FindWindowEx(ipTemp, IntPtr.Zero, "ToolbarWindow32", null);

            GetWindowThreadProcessId(ipTray, ref pid);
            if (pid.Equals(0)) return ipHandle;

            IntPtr hProcess = OpenProcess(PROCESS_ALL_ACCESS | PROCESS_VM_OPERATION | PROCESS_VM_READ | PROCESS_VM_WRITE, IntPtr.Zero, pid);
            IntPtr lAddress = VirtualAllocEx(hProcess, 0, 4096, MEM_COMMIT, PAGE_READWRITE);

            //得到圖標個數
            int lButton = SendMessage(ipTray, TB_BUTTONCOUNT, 0, 0);
            for (int i = 0; i < lButton; i++)
            {
                SendMessage(ipTray, TB_GETBUTTON, i, lAddress);
                //讀文本地址
                ReadProcessMemory(hProcess, (IntPtr)(lAddress.ToInt32() + 16), ref lTextAdr, 4, 0);
                if (!lTextAdr.Equals(-1))
                {
                    byte[] buff = new byte[1024];
                    //讀文本
                    ReadProcessMemory(hProcess, lTextAdr, buff, 1024, 0);
                    string title = System.Text.ASCIIEncoding.Unicode.GetString(buff);
                    // 從字符0處截斷
                    int nullindex = title.IndexOf("\0");
                    if (nullindex > 0)
                    {
                        title = title.Substring(0, nullindex);
                    }
                    //ReadProcessMemory(hProcess, lAddress, ref ipButtonID, 4, 0);
                    //判斷是不是要找的圖標
                    if (title.Equals(TipTitle))
                    {
                        IntPtr ipHandleAdr = IntPtr.Zero;
                        //讀句柄地址
                        ReadProcessMemory(hProcess, (IntPtr)(lAddress.ToInt32() + 12), ref ipHandleAdr, 4, 0);
                        ReadProcessMemory(hProcess, ipHandleAdr, ref ipHandle, 4, 0);
                        break;
                    }
                }
            }
            VirtualFreeEx(hProcess, lAddress, 4096, MEM_RELEASE);
            CloseHandle(hProcess);
            return ipHandle;
        }

        public static void UpdateNotify()
        {
            IntPtr pid = IntPtr.Zero;
            IntPtr ipHandle = IntPtr.Zero; //圖標句柄
            IntPtr lTextAdr = IntPtr.Zero; //文本內存地址
            IntPtr ipTemp = FindWindow("Shell_TrayWnd", null);
            //找到托盤
            ipTemp = FindWindowEx(ipTemp, IntPtr.Zero, "TrayNotifyWnd", null);
            ipTemp = FindWindowEx(ipTemp, IntPtr.Zero, "SysPager", null);
            IntPtr ipTray = FindWindowEx(ipTemp, IntPtr.Zero, "ToolbarWindow32", null);

            GetWindowThreadProcessId(ipTray, ref pid);
            if (pid.Equals(0)) return;

            IntPtr hProcess = OpenProcess(PROCESS_ALL_ACCESS | PROCESS_VM_OPERATION | PROCESS_VM_READ | PROCESS_VM_WRITE, IntPtr.Zero, pid);
            IntPtr lAddress = VirtualAllocEx(hProcess, 0, 4096, MEM_COMMIT, PAGE_READWRITE);

            //得到圖標個數
            int lButton = SendMessage(ipTray, TB_BUTTONCOUNT, 0, 0);
            for (int i = 0; i < lButton; i++)
            {
                SendMessage(ipTray, TB_GETBUTTON, i, lAddress);
                //讀文本地址
                ReadProcessMemory(hProcess, (IntPtr)(lAddress.ToInt32() + 16), ref lTextAdr, 4, 0);
                if (!lTextAdr.Equals(-1))
                {
                    byte[] buff = new byte[1024];
                    //讀文本
                    ReadProcessMemory(hProcess, lTextAdr, buff, 1024, 0);
                    string title = System.Text.ASCIIEncoding.Unicode.GetString(buff);
                    // 從字符0處截斷
                    int nullindex = title.IndexOf("\0");
                    if (nullindex > 0)
                    {
                        title = title.Substring(0, nullindex);
                    }
                    //判斷是不是要找的圖標
                    IntPtr ipHandleAdr = IntPtr.Zero;
                    //讀句柄地址
                    ReadProcessMemory(hProcess, (IntPtr)(lAddress.ToInt32() + 12), ref ipHandleAdr, 4, 0);
                    ReadProcessMemory(hProcess, ipHandleAdr, ref ipHandle, 4, 0);
                    IntPtr proid = IntPtr.Zero;
                    GetWindowThreadProcessId(ipHandle, ref proid);
                    if (proid == IntPtr.Zero)
                    {
                        SendMessage(ipTray, TB_DELETEBUTTON, i, lAddress);
                    }
                }
            }
        }

    }
}
