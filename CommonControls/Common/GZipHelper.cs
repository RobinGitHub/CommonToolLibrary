using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Common
{
    public class GZipHelper
    {
        public static string ConvertImageToBase64(string filePath)
        {
            try
            {
                string imageBase64 = Convert.ToBase64String(System.IO.File.ReadAllBytes(filePath));
                return imageBase64;
            }
            catch { return ""; }
        }

        public static Image ConvertBase64ToImage(string base64String)
        {
            if (base64String == null) return null;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                try
                {
                    Byte[] bytes = Convert.FromBase64String(base64String);
                    memoryStream.Write(bytes, 0, bytes.Length);
                    bytes = null;
                    Bitmap image = new Bitmap(memoryStream, true);
                    memoryStream.Close();
                    bytes = null;
                    return new Bitmap(image);
                }
                catch (Exception e)
                {
                    if (memoryStream != null) memoryStream.Close();
                    throw new Exception(e.Message, e);
                }
            }
        }
        /// <summary>
        /// 压缩字符串
        /// </summary>
        public static byte[] GZipStringToBytes(string str)
        {
            byte[] buffer = UnicodeEncoding.UTF8.GetBytes(str);
            byte[] Zipbuffer = Compress(buffer);
            return Zipbuffer;
        }
        /// <summary>
        /// 解压字符串
        /// </summary>
        public static string GZipBytesToString(byte[] data)
        {
            byte[] buffer = null;
            buffer = Decompress(data);
            return UnicodeEncoding.UTF8.GetString(buffer);
        }
        /// <summary>
        /// 压缩字符串
        /// </summary>
        public static string GZipStringToBase64String(string str)
        {
            byte[] buffer = UnicodeEncoding.UTF8.GetBytes(str);
            byte[] Zipbuffer = Compress(buffer);
            return Convert.ToBase64String(Zipbuffer);
        }
        /// <summary>
        /// 解压字符串
        /// </summary>
        public static string GZipBase64StringToString(string str)
        {
            byte[] buffer = Convert.FromBase64String(str);
            byte[] Zipbuffer = Decompress(buffer);
            return UnicodeEncoding.UTF8.GetString(Zipbuffer);
        }
        /// <summary>
        /// 用.net自带的Gzip对二进制数组进行压缩,压缩比率可能不是太好
        /// </summary>
        /// <param name="data">二进制数组</param>
        /// <returns>压缩后二进制数组</returns>
        public static byte[] Compress(byte[] data)
        {
            MemoryStream ms = new MemoryStream();
            Stream zipStream = null;
            zipStream = new GZipStream(ms, CompressionMode.Compress, true);
            zipStream.Write(data, 0, data.Length);
            zipStream.Close();
            ms.Position = 0;
            byte[] compressed_data = new byte[ms.Length];
            ms.Read(compressed_data, 0, int.Parse(ms.Length.ToString()));
            return compressed_data;
        }

        /// <summary>
        /// 用.net自带的Gzip对数据流进行解压缩
        /// </summary>
        /// <param name="data">二进制数组</param>
        /// <returns>解压缩后的二进制数组</returns>
        public static byte[] Decompress(byte[] data)
        {
            MemoryStream zipMs = new MemoryStream(data);
            int dataBlock = data.Length;
            //
            byte[] decompress_data = null;
            int totalBytesRead = 0;
            Stream zipStream = null;
            zipStream = new GZipStream(zipMs, CompressionMode.Decompress);
            while (true)
            {
                Array.Resize(ref decompress_data, totalBytesRead + dataBlock + 1);
                int bytesRead = zipStream.Read(decompress_data, totalBytesRead, dataBlock);
                if (bytesRead == 0)
                {
                    break;
                }
                totalBytesRead += bytesRead;
            }
            Array.Resize(ref decompress_data, totalBytesRead);
            return decompress_data;
        }

    }
}
