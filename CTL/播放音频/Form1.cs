using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using 播放音频.Properties;
using System.Diagnostics;
using System.IO;
using System.Media;

///https://ffmpeg.org/
///http://www.cnblogs.com/xiaofengfeng/p/3573025.html
///http://www.cnblogs.com/lidabo/p/3967481.html
///http://wenku.baidu.com/link?url=VD2H1bYnAbshFtqL9BpzJzkkUXPC9X_IkZOXQvmWyt2WGY3KzjPi51leeuYkiVfx0vTMkArYqcxEztANvxtHk74Dj9lCMSF2PD8Y0NvHEEK
///http://www.cnblogs.com/Microblue/archive/2010/09/21/2406704.html
namespace 播放音频
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.axWindowsMediaPlayer1.PlayStateChange += axWindowsMediaPlayer1_PlayStateChange;
            axWindowsMediaPlayer1.settings.setMode("loop", false);
        }

        void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                Thread thread = new Thread(new ThreadStart(PlayThread));
                thread.Start();
            } 
        }
        private void PlayThread()
        {
            axWindowsMediaPlayer1.URL = Path.Combine(Application.StartupPath, "A.mp3");
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            ///如果没有播放过，则连续播放，中间有播放过的则跳过，到所有的播完为止
            ///点击控制播放与停止，每次只播放一个
            ///如果当前正在播放，点击其他的，要停止现在播放的，播放点击的
            ///如果点击当前正在播放的，则暂停播放
            ///

            ///AMR 转 mp3
            string cmd = "ffmpeg -i A.amr A.mp3";
            ExecBatCommand(p =>
            {
                p(@cmd);
                // 这里连续写入的命令将依次在控制台窗口中得到体现
                p("exit 0");
            });

            System.Media.SystemSounds.Beep.Play();

            axWindowsMediaPlayer1.URL = Path.Combine(Application.StartupPath, "A.mp3");
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.URL = Path.Combine(Application.StartupPath, "msg.wav");
            axWindowsMediaPlayer1.Ctlcontrols.play();
            
        }

        Thread playThread = null;
        PictureBox playPicBox = null;
        private void pictureBox_Click(object sender, EventArgs e)
        {
            PictureBox ctl = sender as PictureBox;
            if (playThread == null)
            {
                Thread t = new Thread(new ParameterizedThreadStart(Play));
                object obj = new object[] { ctl, 5 };
                t.Start(obj);
                playThread = t;
            }
            else if (playPicBox == ctl)
            {
                playPicBox.Image = Resources.MsgSoundGray3;
                playPicBox.Tag = "MsgSoundGray3";
                playThread.Abort();
            }
            else
            {
                playPicBox.Image = Resources.MsgSoundGray3;
                playPicBox.Tag = "MsgSoundGray3";
                playThread.Abort();

                Thread t = new Thread(new ParameterizedThreadStart(Play));
                object obj = new object[] { ctl, 5 };
                t.Start(obj);
                playThread = t;
            }
            playPicBox = ctl;
        }

        private void Play(object obj)
        {//http://120.55.193.213/hao1003/Data/30080829000/0c4ec86b090c436d2c29a8cb721b24bd/0c4ec86b090c436d2c29a8cb721b24bd.amr
            PictureBox ctl = (obj as object[])[0] as PictureBox;
            int sec = ((int)(obj as object[])[1]) * 2;
            for (int i = 1; i <= sec; i++)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    if (ctl.Tag.ToString() == "MsgSoundGray1")
                    {
                        ctl.Image = Resources.MsgSoundGray2;
                        ctl.Tag = "MsgSoundGray2";
                    }
                    else if (ctl.Tag.ToString() == "MsgSoundGray2")
                    {
                        ctl.Image = Resources.MsgSoundGray3;
                        ctl.Tag = "MsgSoundGray3";
                    }
                    else if (ctl.Tag.ToString() == "MsgSoundGray3")
                    {
                        ctl.Image = Resources.MsgSoundGray1;
                        ctl.Tag = "MsgSoundGray1";
                    }
                    if (i == sec && ctl.Tag.ToString() != "MsgSoundGray3")
                    {
                        ctl.Image = Resources.MsgSoundGray3;
                        ctl.Tag = "MsgSoundGray3";
                    }
                    System.Media.SystemSounds.Beep.Play();
                });
                Thread.Sleep(500);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (playThread != null)
            {
                playThread.Abort();
                playThread = null;
            }
        }

        /// <summary>
        /// 打开控制台执行拼接完成的批处理命令字符串
        /// </summary>
        /// <param name="inputAction">需要执行的命令委托方法：每次调用 <paramref name="inputAction"/> 中的参数都会执行一次</param>
        private void ExecBatCommand(Action<Action<string>> inputAction)
        {
            Process pro = null;
            StreamWriter sIn = null;
            StreamReader sOut = null;

            try
            {
                pro = new Process();
                pro.StartInfo.FileName = "cmd.exe";
                pro.StartInfo.UseShellExecute = false;
                pro.StartInfo.CreateNoWindow = true;
                pro.StartInfo.RedirectStandardInput = true;
                pro.StartInfo.RedirectStandardOutput = true;
                pro.StartInfo.RedirectStandardError = true;

                pro.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
                pro.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);

                //pro.OutputDataReceived += (sender, e) =>
                //{
                //    this.Invoke((MethodInvoker)delegate
                //    {
                //        richTextBox1.AppendText(e.Data + "\n");
                //    });
                //};
                //pro.ErrorDataReceived += (sender, e) =>
                //{
                //    this.Invoke((MethodInvoker)delegate
                //    {
                //        richTextBox1.AppendText(e.Data + "\n");
                //    });
                //};

                pro.Start();
                sIn = pro.StandardInput;
                sIn.AutoFlush = true;

                pro.BeginOutputReadLine();
                inputAction(value => sIn.WriteLine(value));

                pro.WaitForExit(100);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (pro != null && !pro.HasExited)
                    pro.Kill();

                if (sIn != null)
                    sIn.Close();
                if (sOut != null)
                    sOut.Close();
                if (pro != null)
                    pro.Close();
            }
        }

    }
}
