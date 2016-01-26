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


///http://www.cnblogs.com/Microblue/archive/2010/09/21/2406704.html
namespace 播放音频
{

    public partial class Form1 : Form
    {
        public static uint SND_ASYNC = 0x0001;
        public static uint SND_FILENAME = 0x00020000;
        [DllImport("winmm.dll")]
        public static extern uint mciSendString(string lpstrCommand,
        string lpstrReturnString, uint uReturnLength, uint hWndCallback);
        public Form1()
        {
            InitializeComponent();
            System.Media.SystemSounds.Beep.Play();
        }

        public void Play()
        {
            mciSendString(@"close temp_alias", null, 0, 0);
            mciSendString(@"open ""E:/Music/青花瓷.mp3"" alias temp_alias", null, 0, 0);
            mciSendString("play temp_alias repeat", null, 0, 0);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            ///如果没有播放过，则连续播放，中间有播放过的则跳过，到所有的播完为止
            ///点击控制播放与停止，每次只播放一个
            ///如果当前正在播放，点击其他的，要停止现在播放的，播放点击的
            ///如果点击当前正在播放的，则暂停播放
            ///


        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            
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
    }
}
