using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Common
{
    /// <summary>
    /// 常用正则表达式
    /// </summary>
    public static class RegExpHelper
    {
        #region 电话格式是否正确
        public static bool IsTel(string text)
        {
            string pattern = @"^((0\d{2,4}-?\d{7,8})|(1\d{10})|(400-\d{3}-\d{4}))$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(text);
        }
        #endregion

        #region 身份证格式是否正确
        /// <summary>
        /// 身份证格式是否正确
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsIdentity(string text)
        {
            return Regex.IsMatch(text, @"^(^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$)|(^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[Xx])$)$");
        }
        #endregion

        #region 护照格式是否正确
        /// <summary>
        /// 护照格式是否正确
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsPassport(string text)
        {
            return Regex.IsMatch(text, @"(P\d{7})|G\d{8})");
        }
        #endregion

        #region 邮箱是否输入正确
        /// <summary>
        /// 邮箱是否输入正确
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsEmail(string text)
        {
            string pattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(text);
        }
        #endregion

        #region 邮编是否输入正确
        /// <summary>
        /// 邮编是否输入正确
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsPostalcode(string text)
        {
            return Regex.IsMatch(text, @"^\d{6}$");
        }
        #endregion

        #region 获取生日，根据身份证
        /// <summary>
        /// 获取生日，根据身份证
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static DateTime GetBirthdayByIdentity(string identity)
        {
            if (!string.IsNullOrEmpty(identity.Trim()))
            {
                if (!ValidateInputHelper.IsIdentity(identity))
                {
                    throw new Exception("身份证 格式错误！");
                }
                else
                {
                    string birthday = "";
                    //处理18位的身份证号码从号码中得到生日和性别代码
                    if (identity.Length == 18)
                    {
                        birthday = identity.Substring(6, 4) + "-" + identity.Substring(10, 2) + "-" + identity.Substring(12, 2);
                    }
                    //处理15位的身份证号码从号码中得到生日和性别代码
                    if (identity.Length == 15)
                    {
                        birthday = "19" + identity.Substring(6, 2) + "-" + identity.Substring(8, 2) + "-" + identity.Substring(10, 2);
                    }
                    return DateTime.Parse(birthday);
                }
            }
            else
                throw new Exception("身份证 不能为空！");
        }
        #endregion

        #region 获取性别，根据身份证
        /// <summary>
        /// 获取性别，根据身份证
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static bool GetGenderByIdentity(string identity)
        {
            if (!string.IsNullOrEmpty(identity.Trim()))
            {
                if (!ValidateInputHelper.IsIdentity(identity))
                {
                    throw new Exception("身份证 格式错误！");
                }
                else
                {
                    string sex = "";
                    //处理18位的身份证号码从号码中得到生日和性别代码
                    if (identity.Length == 18)
                    {
                        sex = identity.Substring(14, 3);
                    }
                    //处理15位的身份证号码从号码中得到生日和性别代码
                    if (identity.Length == 15)
                    {
                        sex = identity.Substring(12, 3);
                    }
                    return ConvertHelper.ObjectToInt(sex) % 2 == 0;
                }
            }
            else
                throw new Exception("身份证 不能为空！");
        }
        #endregion
    }
}
