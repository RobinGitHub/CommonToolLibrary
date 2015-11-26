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
using System.IO;

namespace QQClient
{
    public partial class LoginForm : Form
    {
        #region 属性
        UdpClient sendUdpClient;
        UdpClient receiveUdpClient;
        /// <summary>
        /// 客户端地址
        /// </summary>
        IPEndPoint localEP;
        /// <summary>
        /// 服务器地址
        /// </summary>
        IPEndPoint serverEP;
        private TcpClient tcpClient;
        private NetworkStream networkStream;
        private BinaryReader binaryReader;
        /// <summary>
        /// 聊天室
        /// </summary>
        private List<ChatForm> chatFormList = new List<ChatForm>();
        #endregion

        #region 委托
        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="message"></param>
        private delegate void AddMessageEventHandler(string message);
        private delegate void RemoveMessageEventHandler(string userName);
        #endregion

        #region 构造函数
        public LoginForm()
        {
            InitializeComponent();
            IPAddress[] address = Dns.GetHostAddresses(Dns.GetHostName());

            txtServerIP.Text = address[2].ToString();
            txtServerPort.Text = "64135";
            serverEP = new IPEndPoint(IPAddress.Parse(txtServerIP.Text), int.Parse(txtServerPort.Text));
            txtLocalIP.Text = address[2].ToString();

            // 随机指定本地端口
            Random random1 = new Random();
            txtLocalPort.Text = random1.Next(1024, 65500).ToString();
            localEP = new IPEndPoint(IPAddress.Parse(txtLocalIP.Text), int.Parse(txtLocalPort.Text));

            Random random = new Random();
            txtUserName.Text = "User:" + random.Next(100, 900);
            btnLogout.Enabled = false;
        }
        #endregion

        #region 关闭窗体

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //释放资源
            try
            {
                if (btnLogin.Enabled)
                    btnLogout_Click(null, null);
                if (sendUdpClient != null)
                    sendUdpClient.Close();
                if (receiveUdpClient != null)
                    receiveUdpClient.Close();
                if (networkStream != null)
                    networkStream.Close();
                if (binaryReader != null)
                    binaryReader.Close();
            }
            catch (Exception ex)
            {
            }
        } 
        #endregion

        #region 登录
        private void btnLogin_Click(object sender, EventArgs e)
        {
            btnLogout.Enabled = true;
            btnLogin.Enabled = false;
            this.Text = txtUserName.Text;
            // 匿名发送
            sendUdpClient = new UdpClient(0);
            receiveUdpClient = new UdpClient(localEP);
            // 启动发送线程
            Thread sendThread = new Thread(SendMessage);
            sendThread.Start(string.Format("login,{0},{1}", txtUserName.Text, localEP));
            // 启动接收线程
            Thread receiveThread = new Thread(ReceiveMessage);
            receiveThread.Start();
        }
        #endregion

        #region 退出
        private void btnLogout_Click(object sender, EventArgs e)
        {
            // 匿名发送
            sendUdpClient = new UdpClient();
            //启动发送线程
            Thread sendThread = new Thread(SendMessage);
            sendThread.Start(string.Format("logout,{0},{1}", txtUserName.Text, localEP));
            receiveUdpClient.Close();
            dgvUserList.Rows.Clear();
            btnLogin.Enabled = true;
            btnLogout.Enabled = false;
            this.Text = "Client";
        }
        #endregion

        #region 弹出对话框
        private void dgvUserList_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvUserList.Rows.Count == 0)
                return;
            string peerName = dgvUserList.SelectedRows[0].Cells[0].Value.ToString();
            if (peerName == txtUserName.Text)
                return;
            string peerUserEPStr = dgvUserList.SelectedRows[0].Cells[1].Value.ToString();
            string[] peerEPArray = peerUserEPStr.Split(':');
            IPEndPoint peerUserEP = new IPEndPoint(IPAddress.Parse(peerEPArray[0]), int.Parse(peerEPArray[1]));
            ChatForm form = chatFormList.Find(t => t.Text == peerName);
            if (form == null)
            {
                form = new ChatForm(txtUserName.Text, peerName, peerUserEP, serverEP);
                form.Text = peerName;
                chatFormList.Add(form);
            }
            form.Show();
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="obj"></param>
        private void SendMessage(object obj)
        {
            string message = (string)obj;
            byte[] sendbytes = Encoding.Unicode.GetBytes(message);
            sendUdpClient.Send(sendbytes, sendbytes.Length, serverEP);
        }
        /// <summary>
        /// 客户端接受服务器回应消息 
        /// </summary>
        private void ReceiveMessage()
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                try
                {
                    // 关闭receiveUdpClient时会产生异常
                    byte[] receiveBytes = receiveUdpClient.Receive(ref remoteEP);
                    string message = Encoding.Unicode.GetString(receiveBytes, 0, receiveBytes.Length);

                    // 处理消息
                    string[] splitString = message.Split(',');

                    switch (splitString[0])
                    {
                        case "Accept":
                            try
                            {
                                tcpClient = new TcpClient();
                                tcpClient.Connect(remoteEP.Address, int.Parse(splitString[1]));
                                if (tcpClient != null)
                                {
                                    // 表示连接成功
                                    networkStream = tcpClient.GetStream();
                                    binaryReader = new BinaryReader(networkStream);
                                    //获取所有在线人员
                                    Thread getUserListThread = new Thread(GetUserList);
                                    getUserListThread.Start();
                                }
                            }
                            catch
                            {
                                MessageBox.Show("连接失败", "异常");
                            }
                            break;
                        case "login":
                            string userItem = splitString[1] + "," + splitString[2];
                            AddMessage(userItem);
                            break;
                        case "logout":
                            RemoveMessage(splitString[1]);
                            break;
                        case "talk":
                            for (int i = 0; i < chatFormList.Count; i++)
                            {
                                if (chatFormList[i].Text == splitString[2])
                                {
                                    chatFormList[i].ShowTalkInfo(splitString[2], splitString[1], splitString[3]);
                                }
                            }
                            break;
                    }
                }
                catch
                {
                    break;
                }
            }
        }
        /// <summary>
        /// 获取在线用户
        /// </summary>
        private void GetUserList()
        {
            while (true)
            {
                try
                {
                    string userListString = binaryReader.ReadString();
                    if (userListString.EndsWith("end"))
                    {
                        string[] userList = userListString.Split(';');
                        //排除end
                        foreach (var item in userList)
                        {
                            if (item == "end")
                                continue;
                            AddMessage(item);
                        }
                        networkStream.Close();
                        binaryReader.Close();
                        tcpClient.Close();
                    }
                }
                catch (Exception ex)
                {
                    break;
                }
            }
        }
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="message"></param>
        private void AddMessage(string message)
        {
            if (dgvUserList.InvokeRequired)
            {
                AddMessageEventHandler addMessage = AddMessage;
                dgvUserList.Invoke(addMessage, message);
            }
            else
            {
                string[] userInfo = message.Split(',');
                int rowIndex = dgvUserList.Rows.Add();
                dgvUserList.Rows[rowIndex].Cells[0].Value = userInfo[0];
                dgvUserList.Rows[rowIndex].Cells[1].Value = userInfo[1];
            }
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userName"></param>
        private void RemoveMessage(string userName)
        {
            if (dgvUserList.InvokeRequired)
            {
                RemoveMessageEventHandler removeMessage = RemoveMessage;
                dgvUserList.Invoke(removeMessage, userName);
            }
            else
            {
                int delRowIndex = -1;
                foreach (DataGridViewRow item in dgvUserList.Rows)
                {
                    if (item.Cells[0].Value.ToString() == userName)
                    {
                        delRowIndex = item.Index;
                        break;
                    }
                }
                if (delRowIndex > -1)
                    dgvUserList.Rows.RemoveAt(delRowIndex);
            }
        }
        #endregion

    }
}
