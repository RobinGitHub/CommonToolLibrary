using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace QQServer
{
    /// <summary>
    /// 用户对象
    /// </summary>
    public class User
    {
        private string userName;
        private IPEndPoint userEP;

        public User(string userName, IPEndPoint userEP)
        {
            this.userName = userName;
            this.userEP = userEP;
        }

        public string GetUserName
        {
            get { return userName; }
        }

        public IPEndPoint GetUserEP
        {
            get { return userEP; }
        }
    }
}
