using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Common
{
    public class JSONHelper
    {
        /// <summary>
        /// DataTable序列化成JSON
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToJSON(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                JsonSerializer ser = new JsonSerializer();
                jw.WriteStartObject();
                jw.WritePropertyName(dt.TableName);
                jw.WriteStartArray();
                foreach (DataRow dr in dt.Rows)
                {
                    jw.WriteStartObject();

                    foreach (DataColumn dc in dt.Columns)
                    {
                        jw.WritePropertyName(dc.ColumnName);
                        ser.Serialize(jw, dr[dc].ToString());
                    }

                    jw.WriteEndObject();
                }
                jw.WriteEndArray();
                jw.WriteEndObject();

                sw.Close();
                jw.Close();

            }

            return sb.ToString();
        }

        #region Json 字符串 转换为 DataTable数据集合
        /// <summary>
        /// Json 字符串 转换为 DataTable数据集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataTable JSONToDataTable(string json, string tableName = "table1")
        {
            DataTable dataTable = new DataTable();  //实例化
            dataTable.TableName = tableName;
            DataTable result;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count<string>() == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                //dataTable.Columns.Add(current);
                                if (dictionary[current] != null)
                                    dataTable.Columns.Add(current, dictionary[current].GetType());
                                else
                                    dataTable.Columns.Add(current, typeof(string));
                            }
                        }
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            if (dictionary[current] != null)
                                dataRow[current] = dictionary[current];
                        }

                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
                    }
                }
            }
            catch
            {
            }
            result = dataTable;
            return result;
        }
        #endregion

        public static DataTable JSONToDataTable(JArray jsonArr, string tableName = "table1")
        {
            if (jsonArr == null || jsonArr.Count == 0)
                return null;
            DataTable dataTable = new DataTable();  //实例化
            dataTable.TableName = tableName;
            DataTable result;
            try
            {
                for (int i = 0; i < jsonArr[0].Count(); i++)
                {
                    dataTable.Columns.Add(jsonArr[0][i].ToString());
                }
                for (int i = 1; i < jsonArr.Count; i++)
                {
                    DataRow dataRow = dataTable.NewRow();
                    for (int j = 0; j < jsonArr[i].Count(); j++)
                    {
                        dataRow[j] = jsonArr[i][j].ToString();
                    }
                    dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
                }
            }
            catch
            {
            }
            result = dataTable;
            return result;
        }
    }
}
