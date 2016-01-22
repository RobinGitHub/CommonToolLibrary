using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Emoji处理
{
    public partial class Form1 : Form
    {
        string str = "\u263a";
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        void Form1_Load(object sender, EventArgs e)
        {
            //string rlt = convertEmojiToStr(str);
            string rlt = toUnicode("ᖗ");
            newRichTextBox1.Text = "؏؏ᖗ¤̴̶̷̤́‧̫̮¤̴̶̷̤̀)ᖘ؏؏ 小公举";
        }

        public string convertEmojiToStr(string sources)
        {
            try
            {
                if (string.IsNullOrEmpty(sources))
                {
                    return sources;
                }
                else
                {
                    string pattern = @"[^\\u0000-\\uFFFF]";//@"[\ud83c-\ud83d]|[\udc00-\udfff]|[\u2600-\u27ff]";
                    Regex reg = new Regex(pattern, RegexOptions.IgnoreCase);
                    Match match = reg.Match(sources);

                    if (match.Success)
                    {
                        GroupCollection gc = match.Groups;
                        foreach (Group item in gc)
                        {
                            if (isEmojiCharacter(item.Value))
                            {
                                string newText = "[" + toUnicode(item.Value) + "]";
                            }
                        }
                        return "";
                    }
                    else
                        return sources;
                }
            }
            catch (Exception  ex)
            {
                
                throw ex;
            }
            
        }

        /// <summary>
        /// 验证是否为表情字符
        /// </summary>
        /// <param name="sources"></param>
        /// <returns></returns>
        public bool isEmojiCharacter(string sources)
        {
            if (string.IsNullOrEmpty(sources))
            {
                return false;
            }
            else
            {
                string regxpForTag = @"[^\\u0000-\\uFFFF]";//@"[\\ud83c\\udc00-\\ud83c\\udfff]|[\\ud83d\\udc00-\\ud83d\\udfff]|[\\u2600-\\u27ff]";
                Regex reg = new Regex(regxpForTag, RegexOptions.IgnoreCase);
                string s = reg.Replace(sources, "");
                //string s = sources.Replace(@"[\\ud83c\\udc00-\\ud83c\\udfff]|[\\ud83d\\udc00-\\ud83d\\udfff]|[\\u2600-\\u27ff]", "");
                if (string.IsNullOrEmpty(s))
                    return true;
                else
                    return false;
            }
        }

       /// <summary>
        /// 将表情字符转换为uncode值
       /// </summary>
       /// <param name="s"></param>
       /// <returns></returns>
        public string toUnicode(string s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] <= 256)
                {
                    sb.Append("\\\\u00");
                }
                else
                {
                    sb.Append("\\\\u");
                }
                sb.Append(((int)(s[i])).ToString("x"));
            }
            return sb.ToString();
        }


    }
}
