using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace COMDLL注册
{
    public partial class Form1 : Form
    {
        [DllImport("Interop.ImageOleLib")]
        public static extern int DllRegisterServer();//注册时用
        [DllImport("Interop.ImageOleLib")]
        public static extern int DllUnregisterServer();//取消注册

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RegistryKey rkTest = Registry.ClassesRoot.OpenSubKey("CLSID\\{06ADA938-0FB0-4BC0-B19B-0A38AB17F182}\\");
            if (rkTest == null)
            {
                //Dll没有注册，在这里调用DllRegisterServer()吧
                int i = DllRegisterServer();
                if (i >= 0)
                {    //注册成功!
                }
                else
                {    //注册失败
                }
            }
        }
    }
}
