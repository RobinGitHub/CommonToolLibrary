//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using System.IO;

//namespace MusicPlayer
//{
//    public partial class Form1 : Form
//    {
//        private WMPLib.IWMPPlaylist initPlaylist;
//        ToolTip tipSource, tipAuthor;

//        public Form1()
//        {
//            InitializeComponent();
//        }

//        private void Form1_Load(object sender, EventArgs e)
//        {
//            this.btnAdd.Text = "Add...";
//            this.btnDel.Text = "Delete";
//            this.btnPlay.Text = "Play";
//            this.btnExit.Text = "Exit";
//            this.Text = "AxWindowMediaPlayer";
//            this.groupBox1.Text = "Currentmedia information";
//            this.setLabels(new string[] { "Nothing" });

//            this.initPlaylist = this.axWMPlayer.playlistCollection.newPlaylist("oriPlaylist");

//            this.tipAuthor = new ToolTip();
//            this.tipSource = new ToolTip();

//            this.btnDel.Enabled = false;
//        }

//        private void setLabels(string[] mediaInfo)
//        {
//            if (mediaInfo.Length != 6)
//            {
//                this.labelAuthor.Text = "Author: ";
//                this.labelDuration.Text = "Duration: ";
//                this.labelSize.Text = "Size: ";
//                this.labelSource.Text = "Source: ";
//                this.labelTitle.Text = "Title: ";
//                this.labelType.Text = "Type: ";
//            }
//            else
//            {
//                this.labelAuthor.Text = "Author: " + mediaInfo[0];
//                this.labelDuration.Text = "Duration: " + mediaInfo[1];
//                this.labelSize.Text = "Size: " + mediaInfo[2];
//                this.labelSource.Text = "Source: " + mediaInfo[3];
//                this.labelTitle.Text = "Title: " + mediaInfo[4];
//                this.labelType.Text = "Type: " + mediaInfo[5];
//            }
//        }

//        private void btnAdd_Click(object sender, EventArgs e)
//        {
//            OpenFileDialog dlg = new OpenFileDialog();
//            dlg.CheckFileExists = true;
//            dlg.Filter = "MP3(*.mp3)|*.mp3|WMA(*.wma)|*.wma|WMV(*.wmv)|*.wmv|All files(*.*)|*.*";
//            dlg.Multiselect = true;

//            if (dlg.ShowDialog() == DialogResult.OK)
//            {
//                string[] files = dlg.FileNames;

//                foreach (string music in files)
//                {
//                    this.listBox_Media.Items.Add(Path.GetFileName(music));
//                    this.initPlaylist.appendItem(this.axWMPlayer.newMedia(music));
//                }
//            }
//        }

//        private void btnDel_Click(object sender, EventArgs e)
//        {
//            if (this.listBox_Media.SelectedIndex != -1)
//            {
//                //WMPLib.IWMPMedia media = this.axWMPlayer.newMedia(this.listBox_Media.Items[this.listBox_Media.SelectedIndex].ToString());
//                //this.axWMPlayer.currentPlaylist.removeItem(this.axWMPlayer.newMedia(this.listBox_Media.Items[this.listBox_Media.SelectedIndex].ToString()));
//                this.initPlaylist.removeItem(this.initPlaylist.Item[this.listBox_Media.SelectedIndex]);
//                this.listBox_Media.Items.RemoveAt(this.listBox_Media.SelectedIndex);

//                if (this.listBox_Media.Items.Count == 0)
//                {
//                    this.axWMPlayer.Ctlcontrols.stop();
//                }
//            }
//        }

//        private void btnPlay_Click(object sender, EventArgs e)
//        {
//            if (this.listBox_Media.Items.Count == 0)
//            {
//                return;
//            }

//            this.axWMPlayer.currentPlaylist = this.initPlaylist;

//            if (this.listBox_Media.SelectedIndex == -1)
//            {
//                this.axWMPlayer.Ctlcontrols.play();
//            }
//            else
//            {
//                this.axWMPlayer.Ctlcontrols.playItem(this.axWMPlayer.currentPlaylist.Item[this.listBox_Media.SelectedIndex]);
//            }
//        }


//        private void btnExit_Click(object sender, EventArgs e)
//        {
//            this.axWMPlayer.Ctlcontrols.stop();
//            this.axWMPlayer.close();
//            this.axWMPlayer.Dispose();
//            Application.Exit();
//        }

//        //Play the double-clicked media,and play the next
//        private void listBox_Media_MouseDoubleClick(object sender, MouseEventArgs e)
//        {
//            if (this.listBox_Media.SelectedIndex != -1)
//            {
//                Rectangle selectedItemRectangle = this.listBox_Media.GetItemRectangle(this.listBox_Media.SelectedIndex);

//                if (selectedItemRectangle.Contains(e.Location))
//                {
//                    /*this.axWMPlayer.Ctlcontrols.stop();
//                    //Make a new playlsit without medias before the selected one
//                    string playlistName = DateTime.Now.Millisecond.ToString();
//                    WMPLib.IWMPPlaylist newList = this.axWMPlayer.playlistCollection.newPlaylist(playlistName);
//                    for(int i=this.listBox_Media.SelectedIndex;i<this.listBox_Media.Items.Count;i++)
//                    {
//                        newList.appendItem(this.initPlaylist.Item[i]);
//                    }

//                    this.axWMPlayer.currentPlaylist = newList;
//                    this.axWMPlayer.Ctlcontrols.play();*/
//                    this.axWMPlayer.currentPlaylist = this.initPlaylist;
//                    this.axWMPlayer.Ctlcontrols.playItem(this.axWMPlayer.currentPlaylist.Item[this.listBox_Media.SelectedIndex]);
//                }
//            }
//        }

//        private void axWMPlayer_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
//        {
//            if (this.axWMPlayer.playState == WMPLib.WMPPlayState.wmppsStopped)
//            {
//                this.setLabels(new string[] { "Nothing" });

//                this.tipAuthor.SetToolTip(this.labelAuthor, string.Empty);
//                this.tipSource.SetToolTip(this.labelSource, string.Empty);

//                this.Text = "AxWindowMediaPlayer";
//            }
//            if (this.axWMPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
//            {
//                WMPLib.IWMPMedia currentMedia = this.axWMPlayer.currentMedia;
//                string[] info = new string[6];

//                info[0] = currentMedia.getItemInfo("Author");
//                info[1] = currentMedia.getItemInfo("Duration");
//                info[2] = currentMedia.getItemInfo("FileSize");
//                info[3] = currentMedia.getItemInfo("sourceURL");
//                info[4] = currentMedia.getItemInfo("Title");
//                info[5] = currentMedia.getItemInfo("FileType");

//                info[1] = this.getMediaTime(info[1]);
//                info[2] = string.Format("{0:F2}M", Convert.ToDouble(info[2]) / 1024.0 / 1024);
//                this.setLabels(info);

//                this.tipAuthor.SetToolTip(this.labelAuthor, info[0]);
//                this.tipSource.SetToolTip(this.labelSource, info[3]);

//                this.Text = "AxWindowMediaPlayer ----- " + info[4];
//            }
//        }

//        private void listBox_Media_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            if (this.listBox_Media.SelectedIndex != -1)
//            {
//                this.btnDel.Enabled = true;
//            }
//            else
//            {
//                this.btnDel.Enabled = false;
//            }
//        }

//        private string getMediaTime(string timeInSeconds)
//        {
//            double timedouble = Convert.ToDouble(timeInSeconds);
//            int time = Convert.ToInt32(timedouble);

//            if (time < 60)
//            {
//                return time.ToString() + " s";
//            }
//            else
//            {
//                int mi = time / 60;
//                int se = time - mi * 60;
//                return mi.ToString() + " min " + se.ToString() + " s";
//            }

//            //return string.Empty;
//        }
//    }
//}