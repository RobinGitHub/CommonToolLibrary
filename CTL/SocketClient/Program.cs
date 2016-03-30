using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress address = IPAddress.Parse("127.0.0.1");
            IPEndPoint ep = new IPEndPoint(address, 9000);
            TcpClient client = new TcpClient();
            client.Connect(ep);

            ThreadPool.QueueUserWorkItem(new WaitCallback(TaskMethod), client);
            //Task t2 = Task.Factory.StartNew(TaskMethod);
            //IPAddress address = IPAddress.Parse("127.0.0.1");
            //IPEndPoint ep = new IPEndPoint(address, 9000);
            //TcpClient client = new TcpClient();
            //client.Connect(ep);
            //NetworkStream ns = client.GetStream();
            ////StreamReader sr = new StreamReader(ns);
            ////string rlt = sr.ReadToEnd();
            ////if (rlt != null)
            ////{
            ////    Console.WriteLine(rlt);
            ////}
            //while (true)
            //{
            //    //if (ns.DataAvailable)
            //    //{
            //    //    byte[] bytes = new Byte[1024 * 5];
            //    //    int length = ns.Read(bytes, 0, bytes.Length);
            //    //    string recData = Encoding.UTF8.GetString(bytes, 0, length);
            //    //    Console.WriteLine(recData);
            //    //}
            //}
            ////sr.Close();
            ////sr.Dispose();
            //ns.Close();
            //ns.Dispose();
            //client.Close();
            //Console.ReadKey();

        }

        static void TaskMethod(object state)
        {
            try
            {
                TcpClient client = state as TcpClient;

                //StreamReader sr = new StreamReader(ns);
                //string rlt = sr.ReadToEnd();
                //if (rlt != null)
                //{
                //    Console.WriteLine(rlt);
                //}
                var stream = client.GetStream();
                while (true)
                {
                    try
                    {
                        stream.ReadTimeout = 10000;
                        byte[] bytes = new Byte[1024 * 5];
                        int length = stream.Read(bytes, 0, bytes.Length);
                        string recData = Encoding.UTF8.GetString(bytes, 0, length);
                        Console.WriteLine(recData);
                    }
                    catch (Exception ex1)
                    {

                        throw;
                    }
                }
                //sr.Close();
                //sr.Dispose();
                //ns.Close();
                //ns.Dispose();
                //client.Close();
                //Console.ReadKey();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }
}
