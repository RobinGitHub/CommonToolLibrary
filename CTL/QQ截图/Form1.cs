using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32;

namespace QQ截图
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]//注册全局热键
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]//卸载全局热键
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public Form1()
        {
            InitializeComponent();

            this.MaximizeBox = false;
            this.ShowInTaskbar = false;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;

            textBox1.ReadOnly = true;
            //手动绑定事件 本来里面就一两句 单独一个方法没必要
            this.FormClosing += (s, e) => { e.Cancel = true; this.Hide(); };
            this.notifyIcon1.DoubleClick += (s, e) => this.ShowWindow();
            this.showMainWindowToolStripMenuItem.Click += (s, e) => this.ShowWindow();
            this.exitToolStripMenuItem.Click += (s, e) => this.ExitApp();
        }

        private const int HOTKEY_ID = 1000;
        private const uint WM_HOTKEY = 0x312;
        private const uint MOD_ALT = 0x1;
        private const uint MOD_CONTROL = 0x2;
        private const uint MOD_SHIFT = 0x4;

        private FrmCapture m_frmCapture;
        private void Form1_Load(object sender, EventArgs e)
        {
            if (!this.LoadSetting())
            {  //加载用户设置 如果失败使用默认设置
                if (!RegisterHotKey(this.Handle, HOTKEY_ID, MOD_SHIFT | MOD_ALT, (int)Keys.A))
                {
                    MessageBox.Show("Register HotKey failed!", "ScreenCapture",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;     //如果默认热键设置也失败 那么之后代码不用执行
                }
                checkBox_alt.Checked = checkBox_shift.Checked = true;
                textBox1.Text = "A";
            }
            this.Location = new Point(-500, -500);  //将窗体搞出屏幕外(否则一闪而过)
            this.BeginInvoke(new MethodInvoker(() => this.Visible = false));    //因为直接this.visible = false没用
            notifyIcon1.Visible = true;     //托盘来一个气泡提示
            notifyIcon1.ShowBalloonTip(30, "ScreenCapture", "ScreenCapture has started!", ToolTipIcon.Info);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!checkBox_ctrl.Checked && !checkBox_alt.Checked && !checkBox_shift.Checked)
            {
                MessageBox.Show("Maybe you should select a control key!");
                return;     //至少选择一个控制键(alt ctrl shift)
            }
            if (textBox1.Text == "")
            {
                MessageBox.Show("Maybe you should select a auxiliary key!");
                return;     //必须确定一个辅助键(非alt ctrl shift)
            }
            if (checkBox_AutoRun.Checked)
            {
                if (DialogResult.No == MessageBox.Show(
                    "\"AutomaticllyRun\" will start after the computer start up!\r\n" +
                    "Please keep the path exsit.\r\nContinue?", "question", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    return; //如果选择了开机自起 那么提示保持当前路径的存在
                }
            }
            //如果满足要求开始设置
            uint CtrlKey = 0 | (checkBox_ctrl.Checked ? MOD_CONTROL : 0)
                | (checkBox_alt.Checked ? MOD_ALT : 0)
                | (checkBox_shift.Checked ? MOD_SHIFT : 0);
            uint auxKey = Convert.ToUInt32((Keys)Enum.Parse(typeof(Keys), textBox1.Text));

            if (!UnregisterHotKey(this.Handle, HOTKEY_ID))                  //卸载原来的热键
                MessageBox.Show("The orginal hotkey uninstallation failed!");
            if (!RegisterHotKey(this.Handle, HOTKEY_ID, CtrlKey, auxKey))   //登记新的热键
                MessageBox.Show("The new hotkey failed to install!");
            //将设置存入文件
            FileStream fs = new FileStream("CaptureSetting.cfg", FileMode.Create);
            fs.Write(BitConverter.GetBytes(CtrlKey), 0, 4);                 //保存控制键
            fs.Write(BitConverter.GetBytes(auxKey), 0, 4);                  //保存辅助键
            fs.WriteByte((byte)(checkBox_AutoRun.Checked ? 1 : 0));         //保存是否自起
            fs.WriteByte((byte)(checkBox_CaptureCursor.Checked ? 1 : 0));   //保存是否捕获鼠标
            fs.Close();
            //根据情况是否写入注册表
            RegistryKey regKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run\", true);
            if (checkBox_AutoRun.Checked)
            {
                if (regKey == null)
                    regKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run\");
                regKey.SetValue("ScreenCapture", Application.ExecutablePath);
            }
            else
            {
                if (regKey != null)
                {
                    if (regKey.GetValue("ScreenCapture") != null)
                        regKey.DeleteValue("ScreenCapture");
                }
            }

            regKey.Close();
            MessageBox.Show("Setting Finish!");
        }

        //获取辅助按键
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if ("None" != e.Modifiers.ToString())
            { //禁止输入控制键(非alt ctrl shift...)
                MessageBox.Show("Can not input control keys!");
                return;
            }
            textBox1.Text = e.KeyCode.ToString();   //显示点下的按键
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                this.StartCapture();
            }
            base.WndProc(ref m);
        }

        private void StartCapture()
        {
            if (m_frmCapture == null || m_frmCapture.IsDisposed)
                m_frmCapture = new FrmCapture();
            m_frmCapture.IsCaptureCursor = checkBox_CaptureCursor.Checked;
            m_frmCapture.Show();
        }

        private bool LoadSetting()
        {
            //【注意】是用绝对路径 如果开机启动的画还没有完全进入系统程序就启动 使用相对路径可能无法找到文件
            if (File.Exists(Application.StartupPath + "\\CaptureSetting.cfg"))
            {     //从文件中获取设置
                byte[] byTemp = File.ReadAllBytes(Application.StartupPath + "\\CaptureSetting.cfg");
                if (byTemp.Length == 10)
                {
                    uint ctrlKey = BitConverter.ToUInt32(byTemp, 0);
                    uint auxKey = BitConverter.ToUInt32(byTemp, 4);
                    if (RegisterHotKey(this.Handle, HOTKEY_ID, ctrlKey, auxKey))
                    {
                        textBox1.Text = ((Keys)auxKey).ToString();
                        checkBox_ctrl.Checked = (ctrlKey & MOD_CONTROL) != 0;
                        checkBox_alt.Checked = (ctrlKey & MOD_ALT) != 0;
                        checkBox_shift.Checked = (ctrlKey & MOD_SHIFT) != 0;
                        checkBox_AutoRun.Checked = Convert.ToBoolean(byTemp[8]);
                        checkBox_CaptureCursor.Checked = Convert.ToBoolean(byTemp[9]);
                        return true;
                    }
                }
            }
            return false;
        }

        private void ShowWindow()
        {
            this.Location = new Point(
                (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2,
                (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2);
            this.Visible = true;
            this.Activate();
        }

        private void ExitApp()
        {
            notifyIcon1.Visible = false;
            UnregisterHotKey(this.Handle, HOTKEY_ID);
            Environment.Exit(0);    //Application.Exit()会被 this.Closing拦截
        }
    }
}
