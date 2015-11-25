using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace 无边框窗体边框阴影效果与拖动
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
