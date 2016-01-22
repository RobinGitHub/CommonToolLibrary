using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Emoji处理
{
    public class NewRichTextBox : RichTextBox
    {
        const string DLL_RICHEDIT = "msftedit.dll";
        const string WC_RICHEDITW = "RICHEDIT50W";
        private IntPtr moduleHandle;
        private bool attemptedLoad;

        [DllImport("Kernel32.dll")]
        internal static extern IntPtr LoadLibrary(string libname);


        public NewRichTextBox()
        {
            // This is where we store the riched library.
            moduleHandle = IntPtr.Zero;
            attemptedLoad = false;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                AttemptToLoadNewRichEdit();
                if (moduleHandle != IntPtr.Zero)
                {
                    cp.ClassName = WC_RICHEDITW;
                }
                return cp;
            }
        }

        void AttemptToLoadNewRichEdit()
        {
            // Check for library
            if (false == attemptedLoad)
            {
                attemptedLoad = true;
                string strFile = Path.Combine(Environment.SystemDirectory, DLL_RICHEDIT);
                moduleHandle = LoadLibrary(strFile);
            }
        }
    }
}
