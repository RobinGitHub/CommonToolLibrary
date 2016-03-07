using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

/*
如果没有播放过，则连续播放，中间有播放过的则跳过，到所有的播完为止
点击控制播放与停止，每次只播放一个
如果当前正在播放，点击其他的，要停止现在播放的，播放点击的
如果点击当前正在播放的，则暂停播放
*/

namespace 播放音频
{
    /// <summary>
    /// 播放语音
    /// </summary>
    public partial class Form2 : Form
    {
        private WMPLib.IWMPPlaylist initPlaylist;

        public Form2()
        {
            InitializeComponent();
            this.Load += Form2_Load;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            axWMPlayer.settings.autoStart = false;
            axWMPlayer.settings.setMode("loop", false);
            this.initPlaylist = this.axWMPlayer.playlistCollection.newPlaylist("oriPlaylist");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            dlg.Filter = "MP3(*.mp3)|*.mp3|WMA(*.wma)|*.wma|WMV(*.wmv)|*.wmv|All files(*.*)|*.*";
            dlg.Multiselect = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string[] files = dlg.FileNames;

                foreach (string music in files)
                {
                    this.initPlaylist.appendItem(this.axWMPlayer.newMedia(music));
                }
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            this.axWMPlayer.currentPlaylist = this.initPlaylist;
            this.axWMPlayer.Ctlcontrols.play();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.axWMPlayer.Ctlcontrols.stop();
        }
    }
}
