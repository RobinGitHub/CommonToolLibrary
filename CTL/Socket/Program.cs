using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Socket
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener lsner = new TcpListener(9000);
            lsner.Start();
            Console.WriteLine("started in port: 9000");
            while (true)
            {
                TcpClient client = lsner.AcceptTcpClient();
                Console.WriteLine("new client received. hashcode: {0}", client.GetHashCode());
                ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessTcpClient), client);
            }
            Console.ReadKey();
        }

        private static void ProcessTcpClient(object state)
        {
            TcpClient client = state as TcpClient;
            if (client == null)
                Console.WriteLine("client is null");

            NetworkStream ns = client.GetStream();
            StreamWriter sw = new StreamWriter(ns);
            sw.WriteLine("Welcome.");
            sw.Flush();
            sw.Close();
            client.Close();
        }
    }
}
