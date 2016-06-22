using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Common
{
    public class LogHelper
    {
        public static void Write(Exception e)
        {
            try
            {
                DeleteFile();
                string filePath = GetFilePath;
                if (!Directory.Exists(GetDirectory))
                    Directory.CreateDirectory(GetDirectory);
                StreamWriter log = new StreamWriter(filePath, true);
                log.WriteLine("time:" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss\r\n") + e.Message + "\r\n" + e.StackTrace + "\r\n");
                log.Close();
            }
            catch
            {

            }
        }

        public static void Write(string content)
        {
            try
            {
                DeleteFile();
                string filePath = GetFilePath;
                if (!Directory.Exists(GetDirectory))
                    Directory.CreateDirectory(GetDirectory);
                StreamWriter log = new StreamWriter(filePath, true);
                log.WriteLine("time:" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss\r\n") + content + "\r\n");
                log.Close();
            }
            catch
            {

            }
        }

        public static void Write(UnhandledExceptionEventArgs e)
        {
            try
            {
                DeleteFile();
                string filePath = GetFilePath;
                if (!File.Exists(filePath))
                    File.Create(filePath);
                StreamWriter log = new StreamWriter(filePath, true);
                log.WriteLine("time:" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss\r\n") + e.ExceptionObject.ToString() + "\r\n");
                log.Close();
            }
            catch
            {

            }
        }

        /// <summary>
        /// 获取文件路径
        /// </summary>
        private static string GetFilePath
        {
            get
            {
                return Path.Combine("Log", string.Format("{0}.txt", DateTime.Now.ToString("yyyy-MM-dd")));
            }
        }
        /// <summary>
        /// 文件目录
        /// </summary>
        private static string GetDirectory
        {
            get
            {
                return "Log";
            }
        }

        /// <summary>
        /// 删除一个月之前的数据
        /// </summary>
        private static void DeleteFile()
        {
            if (Directory.Exists(GetDirectory))
            {
                string[] filePathArr = Directory.GetFiles("Log", "*.txt");
                List<string> delFileList = new List<string>();
                foreach (string item in filePathArr)
                {
                    FileInfo fi = new FileInfo(item);
                    if ((DateTime.Now - fi.CreationTime).TotalDays > 30)
                    {
                        delFileList.Add(item);
                    }
                }
                foreach (string item in delFileList)
                {
                    File.Delete(item);
                }
            }
        }

    }
}
