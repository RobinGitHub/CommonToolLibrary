using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace QQServer
{
    public partial class MainForm : Form
    {
        #region 属性
        /// <summary>
        /// 在线用户
        /// </summary>
        List<User> userList = new List<User>();
        /// <summary>
        /// 服务器的地址
        /// </summary>
        IPEndPoint localEP;
        /// <summary>
        /// 监听
        /// </summary>
        TcpListener tcpListener;
        /// <summary>
        /// 信息发送
        /// </summary>
        UdpClient sendUdpClient;
        /// <summary>
        /// 信息接收
        /// </summary>
        UdpClient receiveUdpClient;
        /// <summary>
        /// 信息流
        /// </summary>
        NetworkStream networkStream;
        #endregion

        #region 委托
        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="message"></param>
        private delegate void AddMessageEventHandler(string message);
        #endregion

        #region 构造函数
        public MainForm()
        {
            InitializeComponent();
            //初始化服务器地址
            IPAddress[] address = Dns.GetHostAddresses(Dns.GetHostName());
            txtIP.Text = address[2].ToString();
            txtPort.Text = "64135";
            localEP = new IPEndPoint(address[2], int.Parse(txtPort.Text));
        }
        #endregion


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (tcpListener != null)
                    tcpListener.Stop();
                if (sendUdpClient != null)
                    sendUdpClient.Close();
                if (receiveUdpClient != null)
                    receiveUdpClient.Close();
                if (networkStream != null)
                    networkStream.Close();
            }
            catch (Exception ex)
            {

            }
        }

        #region 启动
        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            receiveUdpClient = new UdpClient(localEP);

            tcpListener = new TcpListener(localEP);
            tcpListener.Start();

            Thread listenThread = new Thread(ListenClientConnect);
            listenThread.Start();

            Thread receiveThread = new Thread(ReceiveMessage);
            receiveThread.Start();

            AddMessage(string.Format("服务器线程{0}启动，监听端口{1}", localEP, localEP.Port));
        }
        #endregion

        #region 停止
        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            tcpListener.Stop();
            receiveUdpClient.Close();
            userList.Clear();
            rtbxMessageCenter.Text = "";
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 接收客户端发来的信息
        /// </summary>
        private void ReceiveMessage()
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                try
                {
                    byte[] receiveBytes = receiveUdpClient.Receive(ref remoteEP);
                    string message = Encoding.Unicode.GetString(receiveBytes, 0, receiveBytes.Length);
                    AddMessage(message);

                    // 处理消息数据
                    // 根据协议的设计部分，从客户端发送来的消息是具有一定格式的
                    // 服务器接收消息后要对消息做处理
                    //login, username localIPEndPint
                    string[] splitString = message.Split(',');
                    switch (splitString[0])
                    {
                        case "login":
                            string[] splitSubstring = splitString[2].Split(':');
                            IPEndPoint clientEP = new IPEndPoint(IPAddress.Parse(splitSubstring[0]), int.Parse(splitSubstring[1]));
                            // 给在线的其他用户发送广播消息
                            // 通知有新用户加入
                            foreach (User item in userList)
                            {
                                SendMessage(item, message);
                            }
                            User user = new User(splitString[1], clientEP);
                            AddMessage(string.Format("用户{0}({1})加入", user.GetUserName, clientEP));
                            //服务器收到后以匿名UDP返回下面的回应：Accept, port(监听端口)
                            string sendMessage = string.Format("Accept,{0}", localEP.Port);
                            userList.Add(user);
                            SendMessage(user, sendMessage);
                            AddMessage(string.Format("广播：[{0}]", message));
                            break;
                        case "logout":
                            int delIndex = -1;
                            foreach (var item in userList)
                            {
                                if (item.GetUserName == splitString[1])
                                {
                                    delIndex = userList.IndexOf(item);
                                    AddMessage(string.Format("用户{0}({1})退出", item.GetUserName, item.GetUserEP));
                                    break;
                                }
                            }
                            userList.RemoveAt(delIndex);

                            foreach (var item in userList)
                            {
                                SendMessage(item, message);
                            }
                            AddMessage(string.Format("广播:[{0}]", message));
                            break;
                        case "talk":
                            AddMessage(string.Format("用户{0} 时间 {1} {2} {3} ", splitString[2], splitString[1], Environment.NewLine, splitString[3]));
                            break;
                    }
                }
                catch (Exception ex)
                {
                    break;
                }
            }
            AddMessage(string.Format("服务线程{0}终止", localEP));
        }

        /// <summary>
        /// 给客户端发送信息
        /// </summary>
        private void SendMessage(User user, string message)
        {
            sendUdpClient = new UdpClient(0);
            byte[] sendBytes = Encoding.Unicode.GetBytes(message);
            sendUdpClient.Send(sendBytes, sendBytes.Length, user.GetUserEP);
            sendUdpClient.Close();
        }

        /// <summary>
        /// 监听客户端连接
        /// </summary>
        private void ListenClientConnect()
        {
            TcpClient client = null;
            while (true)
            {
                try
                {
                    client = tcpListener.AcceptTcpClient();
                    AddMessage(string.Format("接受客户端{0}的TCP请求", client.Client.RemoteEndPoint));
                }
                catch (Exception ex)
                {
                    AddMessage(string.Format("监听线程({0})", localEP));
                    break;
                }
                Thread sendThread = new Thread(SendUserListData);
                sendThread.Start(client);
            }
        }
        /// <summary>
        /// 向客户端发送在线用户列表信息
        /// 服务器通过TCP连接把在线用户列表信息发送给客户端
        /// </summary>
        /// <param name="userClient"></param>
        private void SendUserListData(object userClient)
        {
            TcpClient client = (TcpClient)userClient;
            StringBuilder data = new StringBuilder();
            foreach (var item in userList)
            {
                data.AppendFormat("{0},{1};", item.GetUserName, item.GetUserEP);
            }
            data.AppendFormat("end");
            networkStream = client.GetStream();
            BinaryWriter binaryWriter = new BinaryWriter(networkStream);
            binaryWriter.Write(data.ToString());
            binaryWriter.Flush();
            AddMessage(string.Format("向{0}发送[{1}]", client.Client.RemoteEndPoint, data.ToString()));
            binaryWriter.Close();
            client.Close();
        }

        private void AddMessage(string message)
        {
            // InvokeRequired代表如果调用线程与创建控件的线程不在一个线程上时，则返回true
            // 否则返回false
            if (rtbxMessageCenter.InvokeRequired)
            {
                AddMessageEventHandler addMessage = AddMessage;
                rtbxMessageCenter.Invoke(addMessage, message);
            }
            else
            {
                rtbxMessageCenter.AppendText(message);
                rtbxMessageCenter.AppendText(Environment.NewLine);
                rtbxMessageCenter.ScrollToCaret();
            }
        }

        #endregion

    }
}
