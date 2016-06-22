using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SQLiteORM.Demo
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

            string CONN = Path.Combine(Application.StartupPath, "test.db3");
            DbConnection.Initialise("Data Source=" + CONN);
            Application.Run(new Form1());
        }
    }
}
