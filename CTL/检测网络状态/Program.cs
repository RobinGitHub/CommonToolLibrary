using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace 检测网络状态
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread t = new Thread(new ThreadStart(Run));
            t.Start();
        }

        static void Run()
        {
            while (true)
            {
                Thread.Sleep(1000);
                if (IsConnectedToInternet())
                    Console.WriteLine("已连接在网上!");
                else
                    Console.WriteLine("未连接在网上!");
            }
        }


        [DllImport("wininet.dll", EntryPoint = "InternetGetConnectedState")]
        public extern static bool InternetGetConnectedState(out int conState, int reder);
        //参数说明 constate 连接说明 ，reder保留值
        public static bool IsConnectedToInternet()
        {
            int Desc = 0;
            return InternetGetConnectedState(out  Desc, 0);
        }

    }
}
