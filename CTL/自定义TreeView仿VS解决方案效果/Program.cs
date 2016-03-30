using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace 自定义TreeView仿VS解决方案效果
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
            Application.Run(new RichTextBoxForm());
        }
    }
}
