using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
// 添加额外命名空间
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace QQClient
{
    public partial class ChatForm : Form
    {
        /// <summary>
        /// 用户名
        /// </summary>
        string selfUserName;
        /// <summary>
        /// 对方用户名
        /// </summary>
        string peerUserName;
        /// <summary>
        /// 对方地址
        /// </summary>
        IPEndPoint peerUserEP;
        
        UdpClient sendUdpClient;
        /// <summary>
        /// 服务器地址
        /// </summary>
        IPEndPoint serverEP;


        public ChatForm()
        {
            InitializeComponent();
        }


        public ChatForm(string selfUserName, string peerUserName, IPEndPoint peerUserEP, IPEndPoint serverEP)
            : this()
        {
            this.selfUserName = selfUserName;
            this.peerUserName = peerUserName;
            this.peerUserEP = peerUserEP;
            this.serverEP = serverEP;
        }


        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sendUdpClient != null)
                sendUdpClient.Close();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            // 匿名发送
            sendUdpClient = new UdpClient(0);
            // 启动发送线程
            Thread sendThread = new Thread(SendMessage);
            sendThread.Start(string.Format("talk,{0},{1},{2}", DateTime.Now.ToLongTimeString(), selfUserName, txbSend.Text));
            ShowTalkInfo(selfUserName, DateTime.Now.ToLongTimeString(), txbSend.Text);
            txbSend.Text = "";
            txbSend.Focus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="obj"></param>
        private void SendMessage(object obj)
        {
            string message = obj as string;
            byte[] sendbytes = Encoding.Unicode.GetBytes(message);
            sendUdpClient.Send(sendbytes, sendbytes.Length, peerUserEP);
            sendUdpClient.Send(sendbytes, sendbytes.Length, serverEP);
            sendUdpClient.Close();
        }
        /// <summary>
        /// 显示对话内容
        /// </summary>
        /// <param name="peerName"></param>
        /// <param name="time"></param>
        /// <param name="content"></param>
        public void ShowTalkInfo(string peerName, string time, string content)
        {
            richtxbTalkinfo.AppendText(peerName + "    " + time + Environment.NewLine + content);
            richtxbTalkinfo.AppendText(Environment.NewLine);
            richtxbTalkinfo.ScrollToCaret();
        }

    }
}
