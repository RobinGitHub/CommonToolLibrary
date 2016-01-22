using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Emoji处理
{
    public class EmojiUtil
    {
        public static bool isEmojiCharacter(string sources)
        {
            if (string.IsNullOrEmpty(sources))
            {
                return false;
            }
            else
            {
                string regxpForTag = @"[\\ud83c\\udc00-\\ud83c\\udfff]|[\\ud83d\\udc00-\\ud83d\\udfff]|[\\u2600-\\u27ff]";
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
        public static string ToUnicode(string s)
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
