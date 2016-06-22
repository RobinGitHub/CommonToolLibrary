using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Common
{
    public static class ConvertHelper
    {
        /// <summary>
        /// 将字符串string转换为整型int
        /// </summary>
        /// <param name="str">需要转换的字串</param>
        /// <returns>成功返回转换后的结果，失败返回0</returns>
        static public int StringToInt(string str)
        {
            return StringToInt(str, 0);
        }

        /// <summary>
        /// 将字符串string转换为整型int
        /// </summary>
        /// <param name="str">需要转换的字串</param>
        /// <param name="defaultValue"></param>
        /// <returns>成功返回转换后的结果，失败返回0</returns>
        static public int StringToInt(object vValue, int defaultValue)
        {
            if (IsNumeric(vValue))
            {
                string strTmp = vValue.ToString();
                if (strTmp.Contains("."))
                {
                    strTmp = strTmp.Substring(0, strTmp.IndexOf("."));
                }

                if (strTmp.Length > 15)
                {
                    strTmp = strTmp.Substring(0, 15);
                }
                return int.Parse(strTmp);
            }
            else
                return defaultValue;
        }

        /// <summary>
        /// 判断一个值是否为数字
        /// </summary>
        /// <param name="vValue"></param>
        /// <returns></returns>
        public static Boolean IsNumeric(object vValue)
        {
            if (vValue == null)
                return false;

            Regex digitregex = new Regex(@"^[0-9+-]\d*[.]?\d*$");
            if (digitregex.IsMatch(vValue.ToString()))
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 将字符串string转换为无符号整型byte
        /// </summary>
        /// <param name="str">需要转换的字串</param>
        /// <returns>成功返回转换后的结果，失败返回0</returns>
        static public byte StringToByte(string str)
        {
            if (str == null)
                return 0;
            else if (str == "")
                return 0;
            else
            {
                byte i;
                if (byte.TryParse(str, out i))
                    return i;
                else
                    return 0;
            }
        }

        /// <summary>
        /// 将字符串string转换为整型decimal
        /// </summary>
        /// <param name="str">需要转换的字串</param>
        /// <returns>成功返回转换后的结果，失败返回0</returns>
        static public decimal StringToDecimal(string str)
        {
            if (str == null)
                return 0;
            else if (str == "")
                return 0;
            else
            {
                decimal d = 0;
                if (decimal.TryParse(str, out d)) return d;
                return 0;
            }
        }

        public static decimal ObjectToDecimal(object obj)
        {
            return StringToDecimal(ObjectToString(obj));
        }

        /// <summary>
        /// 将字符串string转换为整型double
        /// </summary>
        /// <param name="str">需要转换的字串</param>
        /// <returns>成功返回转换后的结果，失败返回0</returns>
        static public double StringToDouble(string str)
        {
            if (str == null)
                return 0.0;
            else if (str == "")
                return 0.0;
            else
            {
                double d = 0.0;
                if (double.TryParse(str, out d)) return d;
                return 0;
            }
        }

        /// <summary>
        /// 将字符串string转换为布尔bool
        /// </summary>
        /// <param name="str">需要转换的字串，通常情况传True或False</param>
        /// <returns>如果转换成功，返回转换后的值，否则，返回假</returns>
        static public bool StringToBoolean(string str)
        {
            if (str == null)
                return false;
            else if (str == "")
                return false;
            else
            {
                bool b = false;
                if (bool.TryParse(str, out b)) return b;            //如果转换成功，返回转换后的值，否则，返回假
                return false;
            }
        }
        /// <summary>
        /// 将对象转为bool
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool ObjectToBoolean(object obj)
        {
            return StringToBoolean(ObjectToString(obj));
        }

        /// <summary>
        /// 把字符串转换成日期形式
        /// </summary>
        /// <param name="str">需要转换的字符串</param>
        /// <param name="dtValue">传唤失败时返回的日期</param>
        /// <returns>返回转换后的日期</returns>
        static public DateTime StringToDateTime(string str, DateTime dtValue)
        {
            DateTime dt = new DateTime();
            if (DateTime.TryParse(str, out dt))
            {
                return dt;
            }
            else
            {
                return dtValue;
            }
        }
        /// <summary>
        /// 把字符串转换成日期形式
        /// </summary>
        /// <param name="str">需要转换的字符串</param>
        /// <returns>返回转换后的日期,如果失败返回日期类型最小值</returns>
        static public DateTime StringToDateTime(string str)
        {
            return StringToDateTime(str, DateTime.MinValue);
        }
        static public DateTime ObjectToDateTime(object obj)
        {
            return StringToDateTime(obj.ToString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        static public DateTime? ObjToDateTime(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            return StringToDateTime(obj.ToString());
        }

        /// <summary>
        /// 将对象转换为int
        /// </summary>
        /// <param name="obj"></param>
        static public int ObjectToInt(object obj)
        {
            return ObjectToInt(obj, 0);
        }

        /// <summary>
        /// 将对象转换为int
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        static public int ObjectToInt(object obj, int defaultValue)
        {
            if (obj == null)
            {
                return defaultValue;
            }
            return StringToInt(obj.ToString(), defaultValue);
        }

        /// <summary>
        /// 将对象转换为long
        /// </summary>
        /// <param name="obj"></param>
        static public long ObjectToLong(object obj)
        {
            if (obj == null)
            {
                return 0;
            }
            try
            {
                return System.Convert.ToInt64(obj);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 将对象转换为short
        /// </summary>
        /// <param name="obj"></param>
        static public short ObjectToShort(object obj)
        {
            if (obj == null)
            {
                return 0;
            }
            try
            {
                return System.Convert.ToInt16(obj);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 将对象转换为byte
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        static public byte ObjectToByte(object obj)
        {
            if (obj == null)
            {
                return (byte)0;
            }
            try
            {
                return System.Convert.ToByte(obj);
            }
            catch
            {
                return (byte)0;
            }
        }

        /// <summary>
        /// 将对象转换为字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        static public string ObjectToString(object obj)
        {
            return ObjectToString(obj, "");
        }

        /// <summary>
        /// 将对象转换为字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        static public string ObjectToString(object obj, string defaultValue)
        {
            if (obj == null)
            {
                return defaultValue;
            }
            return obj.ToString();
        }

        /// <summary>
        /// 将字符串转换为GUID
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        static public Guid? StringToGuid(string obj)
        {
            Guid? rlt = null;
            if (!string.IsNullOrEmpty(obj))
            {
                try
                {
                    rlt = new Guid(obj);
                }
                catch
                { }
            }
            return rlt;
        }

        /// <summary>
        ///将16进制的字符串转换成字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] HexStringToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary>
        /// 将字节转换成16进制的字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string BytesToHexString(byte[] bytes) // 0xae00cf => "AE00CF "
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }
                hexString = strB.ToString();
            }
            return hexString;
        }

        /// <summary>
        /// 批量转换类型
        /// </summary>
        /// <typeparam name="T1">源数据类型</typeparam>
        /// <typeparam name="T2">转换后类型</typeparam>
        /// <param name="source">源数据集合</param>
        /// <param name="defaultValue">转换默认值</param>
        /// <returns></returns>
        static public IEnumerable<T2> ConvertListType<T1, T2>(IEnumerable<T1> source, T2 defaultValue)
        {
            List<T2> rlt = new List<T2>();
            var list = source.ToArray();
            for (int i = 0; i < list.Length; i++)
            {
                try
                {
                    rlt.Add((T2)Convert.ChangeType(list[i], typeof(T2)));
                }
                catch
                {
                    rlt.Add(defaultValue);
                }
            }
            return rlt;
        }
        /// <summary>
        /// 批量转换类型
        /// </summary>
        /// <typeparam name="T1">源数据类型</typeparam>
        /// <typeparam name="T2">转换后类型</typeparam>
        /// <param name="source">源数据集合</param>
        /// <returns></returns>
        static public IEnumerable<T2> ConvertListType<T1, T2>(IEnumerable<T1> source)
        {
            List<T2> rlt = new List<T2>();
            var list = source.ToArray();
            for (int i = 0; i < list.Length; i++)
            {
                rlt.Add((T2)Convert.ChangeType(list[i], typeof(T2)));
            }
            return rlt;
        }

        #region 图片转为图标
        /// <summary>
        /// 图片转为图标
        /// </summary>
        /// <param name="obm"></param>
        /// <param name="preserve"></param>
        /// <returns></returns>
        public static Icon BitmapToIcon(Bitmap obm, bool preserve)
        {
            try
            {
                int ICON_W = obm.Width;
                int ICON_H = obm.Height;
                Bitmap bm;
                // if not preserving aspect
                if (!preserve)        // if not preserving aspect
                    bm = new Bitmap(obm, ICON_W, ICON_H);  //   rescale from original bitmap

                // if preserving aspect drop excess significance in least significant direction
                else          // if preserving aspect
                {
                    Rectangle rc = new Rectangle(0, 0, ICON_W, ICON_H);
                    if (obm.Width >= obm.Height)   //   if width least significant
                    {          //     rescale with width based on max icon height
                        bm = new Bitmap(obm, (ICON_H * obm.Width) / obm.Height, ICON_H);
                        rc.X = (bm.Width - ICON_W) / 2;  //     chop off excess width significance
                        if (rc.X < 0) rc.X = 0;
                    }
                    else         //   if height least significant
                    {          //     rescale with height based on max icon width
                        bm = new Bitmap(obm, ICON_W, (ICON_W * obm.Height) / obm.Width);
                        rc.Y = (bm.Height - ICON_H) / 2; //     chop off excess height significance
                        if (rc.Y < 0) rc.Y = 0;
                    }
                    bm = bm.Clone(rc, bm.PixelFormat);  //   bitmap for icon rectangle
                }

                // create icon from bitmap
                Icon icon = Icon.FromHandle(bm.GetHicon()); // create icon from bitmap
                bm.Dispose();        // dispose of bitmap
                return icon;        // return icon
            }
            catch (Exception ex)
            {
                LogHelper.Write(ex);
            }
            return null;
        }
        #endregion

        #region 范型集合和DataSet之间的转换

        /// <summary>
        /// 数据集合转换成DataSet
        /// </summary>
        /// <param name="datas">数据集合转换成的Object数组</param>
        /// <returns></returns>
        public static DataSet ToDataSet(object[] datas)
        {
            DataSet result = new DataSet();
            DataTable _DataTable = new DataTable();
            if (datas.Length > 0)
            {
                PropertyInfo[] propertys = datas[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (pi.PropertyType.ToString().IndexOf("EntitySet") >= 0 || pi.PropertyType.ToString().IndexOf("EntityRef") >= 0)
                    {
                        continue;
                    }
                    _DataTable.Columns.Add(pi.Name, GetNotNullableType(pi.PropertyType));
                }

                for (int i = 0; i < datas.Length; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (pi.PropertyType.ToString().IndexOf("EntitySet") >= 0 || pi.PropertyType.ToString().IndexOf("EntityRef") >= 0)
                        {
                            continue;
                        }
                        object obj = pi.GetValue(datas[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    _DataTable.LoadDataRow(array, true);
                }
            }
            result.Tables.Add(_DataTable);
            return result;
        }

        /// <summary>
        /// 数据集合转化成DataTable
        /// Landry add at: 2010-12-08
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(object[] datas)
        {
            return ToDataTable(datas, true);
        }

        public static DataTable ToDataTable(object[] datas, bool isNeedColumnType = true)
        {
            if (datas == null) { return null; }
            DataTable _DataTable = new DataTable();
            if (datas.Length > 0)
            {
                PropertyInfo[] propertys = datas[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (isNeedColumnType)
                    {
                        _DataTable.Columns.Add(pi.Name, GetNotNullableType(pi.PropertyType));
                    }
                    else
                    {
                        _DataTable.Columns.Add(pi.Name);
                    }
                }

                foreach (object obj in datas)
                {
                    DataRow newRow = _DataTable.NewRow();
                    foreach (PropertyInfo field in propertys)
                    {
                        newRow[field.Name] = field.GetValue(obj, null);
                    }
                    _DataTable.Rows.Add(newRow);
                }
            }
            _DataTable.TableName = "Table1";//罗鹏加的，防止WCF返回报错  BY 2012-11-07
            return _DataTable;
        }

        /// <summary>
        /// 范型集合转换成DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(IList<T> datas) where T : class, new()
        {
            if (datas == null) { return null; }
            DataTable _DataTable = new DataTable();
            var pis = typeof(T).GetProperties();
            foreach (var pi in pis)
            {
                //if (pi.GetCustomAttributes(false).Any(t => t is DataMemberAttribute))
                //{
                _DataTable.Columns.Add(pi.Name, GetNotNullableType(pi.PropertyType));
                //}
            }
            foreach (var data in datas)
            {
                ArrayList tempList = new ArrayList();
                foreach (var pi in pis)
                {
                    //if (pi.GetCustomAttributes(false).Any(t => t is DataMemberAttribute))
                    //{
                    tempList.Add(pi.GetValue(data, null));
                    //}
                }
                _DataTable.LoadDataRow(tempList.ToArray(), true);
            }
            _DataTable.TableName = "Table1";//罗鹏加的，防止WCF返回报错  BY 2012-11-07
            return _DataTable;
        }

        /// <summary>
        /// 数据集合转化成泛型 List
        /// Landry add at: 2010-12-08
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static List<T> ObjectToIList<T>(object[] datas) where T : class
        {
            if (datas == null) { return null; }
            Type targetType = typeof(T);
            PropertyInfo[] targetPropertyInfos = targetType.GetProperties();
            FieldInfo[] objFieldInfos = datas[0].GetType().GetFields();
            List<T> resultList = new List<T>();
            foreach (object obj in datas)
            {
                T targetObj = (T)Activator.CreateInstance(typeof(T));
                foreach (FieldInfo field in objFieldInfos)
                {
                    PropertyInfo pi = targetPropertyInfos.SingleOrDefault(p => p.Name == field.Name);
                    if (pi != null)
                    {
                        pi.SetValue(targetObj, field.GetValue(obj), null);
                    }
                }
                resultList.Add(targetObj);
            }
            return resultList;
        }

        /// <summary>
        /// 泛型集合转换DataSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">泛型集合</param>
        /// <returns></returns>
        public static DataSet IListToDataSet<T>(IList<T> list) where T : class
        {
            return IListToDataSet<T>(list, null);
        }


        /// <summary>
        /// 泛型集合转换DataSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p_List">泛型集合</param>
        /// <param name="p_PropertyName">待转换属性名数组</param>
        /// <returns></returns>
        public static DataSet IListToDataSet<T>(IList<T> p_List, params string[] p_PropertyName) where T : class
        {
            List<string> propertyNameList = new List<string>();
            if (p_PropertyName != null)
                propertyNameList.AddRange(p_PropertyName);

            DataSet result = new DataSet();
            DataTable _DataTable = new DataTable();
            if (p_List.Count > 0)
            {
                PropertyInfo[] propertys = p_List[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (pi.PropertyType.ToString().IndexOf("EntitySet") >= 0 || pi.PropertyType.ToString().IndexOf("EntityRef") >= 0)
                    {
                        continue;
                    }
                    if (propertyNameList.Count == 0)
                    {
                        // 没有指定属性的情况下全部属性都要转换
                        _DataTable.Columns.Add(pi.Name, GetNotNullableType(pi.PropertyType));
                    }
                    else
                    {
                        if (propertyNameList.Contains(pi.Name))
                            _DataTable.Columns.Add(pi.Name, GetNotNullableType(pi.PropertyType));
                    }
                }

                for (int i = 0; i < p_List.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (pi.PropertyType.ToString().IndexOf("EntitySet") >= 0 || pi.PropertyType.ToString().IndexOf("EntityRef") >= 0)
                        {
                            continue;
                        }
                        if (propertyNameList.Count == 0)
                        {
                            object obj = pi.GetValue(p_List[i], null);
                            tempList.Add(obj);
                        }
                        else
                        {
                            if (propertyNameList.Contains(pi.Name))
                            {
                                object obj = pi.GetValue(p_List[i], null);
                                tempList.Add(obj);
                            }
                        }
                    }
                    object[] array = tempList.ToArray();
                    _DataTable.LoadDataRow(array, true);
                }
            }
            result.Tables.Add(_DataTable);
            return result;
        }

        /// <summary>
        /// DataSet装换为泛型集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p_DataSet">DataSet</param>
        /// <param name="p_TableIndex">待转换数据表索引</param>
        /// <returns></returns>
        public static IList<T> DataSetToIList<T>(DataSet p_DataSet, int p_TableIndex) where T : class
        {
            if (p_DataSet == null || p_DataSet.Tables.Count < 0)
                return null;
            if (p_TableIndex > p_DataSet.Tables.Count - 1)
                return null;
            if (p_TableIndex < 0)
                p_TableIndex = 0;

            DataTable p_Data = p_DataSet.Tables[p_TableIndex];
            // 返回值初始化
            IList<T> result = new List<T>();
            for (int j = 0; j < p_Data.Rows.Count; j++)
            {
                T _t = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] propertys = _t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    for (int i = 0; i < p_Data.Columns.Count; i++)
                    {
                        // 属性与字段名称一致的进行赋值
                        if (pi.Name.Equals(p_Data.Columns[i].ColumnName))
                        {
                            // 数据库NULL值单独处理
                            if (p_Data.Rows[j][i] != DBNull.Value)
                                pi.SetValue(_t, p_Data.Rows[j][i], null);
                            else
                                pi.SetValue(_t, null, null);
                            break;
                        }
                    }
                }
                result.Add(_t);
            }
            return result;
        }
        /// <summary>
        /// 装换为泛型集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p_Data"></param>
        /// <returns></returns>
        public static IList<T> DataTableToIList<T>(DataTable p_Data) where T : class
        {
            // 返回值初始化
            IList<T> result = new List<T>();
            for (int j = 0; j < p_Data.Rows.Count; j++)
            {
                T _t = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] propertys = _t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    for (int i = 0; i < p_Data.Columns.Count; i++)
                    {
                        // 属性与字段名称一致的进行赋值
                        if (pi.Name.Equals(p_Data.Columns[i].ColumnName))
                        {
                            try
                            {
                                // 数据库NULL值单独处理
                                if (p_Data.Rows[j][i] != DBNull.Value)
                                {
                                    if (pi.PropertyType == typeof(int))
                                        pi.SetValue(_t, ConvertHelper.ObjectToInt(p_Data.Rows[j][i]), null);
                                    else
                                        pi.SetValue(_t, p_Data.Rows[j][i], null);
                                }
                                else
                                    pi.SetValue(_t, null, null);
                                break;
                            }
                            catch (Exception ex)
                            {

                                throw ex;
                            }
                        }
                    }
                }
                result.Add(_t);
            }
            return result;
        }

        /// <summary>
        /// DataSet装换为泛型集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p_DataSet">DataSet</param>
        /// <param name="p_TableName">待转换数据表名称</param>
        /// <returns></returns>
        public static IList<T> DataSetToIList<T>(DataSet p_DataSet, string p_TableName) where T : class
        {
            int _TableIndex = 0;
            if (p_DataSet == null || p_DataSet.Tables.Count < 0)
                return null;
            if (string.IsNullOrEmpty(p_TableName))
                return null;
            for (int i = 0; i < p_DataSet.Tables.Count; i++)
            {
                // 获取Table名称在Tables集合中的索引值
                if (p_DataSet.Tables[i].TableName.Equals(p_TableName))
                {
                    _TableIndex = i;
                    break;
                }
            }
            return DataSetToIList<T>(p_DataSet, _TableIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_type"></param>
        /// <returns></returns>
        public static Type GetNotNullableType(Type p_type)
        {
            if (p_type == typeof(Int16?))
            {
                return typeof(Int16);
            }
            if (p_type == typeof(Int32?))
            {
                return typeof(Int32);
            }
            if (p_type == typeof(Int64?))
            {
                return typeof(Int64);
            }
            if (p_type == typeof(decimal?))
            {
                return typeof(decimal);
            }
            if (p_type == typeof(double?))
            {
                return typeof(double);
            }
            if (p_type == typeof(DateTime?))
            {
                return typeof(DateTime);
            }
            if (p_type == typeof(Boolean?))
            {
                return typeof(Boolean);
            }
            if (p_type == typeof(Guid?))
            {
                return typeof(Guid);
            }
            if (p_type == typeof(byte?))
            {
                return typeof(byte);
            }
            if (p_type == typeof(float?))
            {
                return typeof(float);
            }
            return p_type;
        }
        #endregion

        /// <summary>
        /// 获取匿名类的值
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        public static object GetObjectValue(object Source, string ColumnName)
        {
            object objRlt = null;
            try
            {
                objRlt = Source.GetType().GetProperty(ColumnName).GetValue(Source, null);
            }
            catch
            { }
            return objRlt;
        }

        /// <summary>
        /// 设置匿名类的值
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="ColumnName"></param>
        /// <param name="Value"></param>
        public static void SetObjectValue(object Source, string ColumnName, object Value)
        {
            System.Reflection.PropertyInfo pi = Source.GetType().GetProperty(ColumnName);
            if (pi != null)
            {
                pi.SetValue(Source, Value, null);
            }
        }

        #region 全角转换半角以及半角转换为全角
        /// 转全角的函数(SBC case)
        ///
        ///任意字符串
        ///全角字符串
        ///
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///
        public static string ToSBC(this string input)
        {
            // 半角转全角：
            char[] array = input.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == 32)
                {
                    array[i] = (char)12288;
                    continue;
                }
                if (array[i] < 127)
                {
                    array[i] = (char)(array[i] + 65248);
                }
            }
            return new string(array);
        }

        /**/
        // /
        // / 转半角的函数(DBC case)
        // /
        // /任意字符串
        // /半角字符串
        // /
        // /全角空格为12288，半角空格为32
        // /其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        // /
        public static string ToDBC(this string input)
        {
            char[] array = input.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == 12288)
                {
                    array[i] = (char)32;
                    continue;
                }
                if (array[i] > 65280 && array[i] < 65375)
                {
                    array[i] = (char)(array[i] - 65248);
                }
            }
            return new string(array);
        }
        #endregion
    }
}
