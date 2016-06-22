using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CommonControls
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
            Application.Run(new CommonControls.SplitButton.MainForm());
            //Application.Run(new CommonControls.自定义LookUp.MainForm());
            //Application.Run(new CommonControls.自定义ComboBox.MainForm());
            //Application.Run(new CommonControls.DataGridViewControl.MainForm());
            //Application.Run(new Form1());
        }
    }
}
