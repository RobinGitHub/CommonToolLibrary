using ExcelFileHelper.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExcelFileHelper
{
    #region 枚举
    public enum HDRType
    {
        /// <summary>
        /// HDR=Yes，这代表第一行是标题，不做为数据使用
        /// </summary>
        YES,
        /// <summary>
        /// HDR=NO，则表示第一行不是标题，做为数据来使用
        /// </summary>
        NO
    }

    /// <summary>
    /// 判断Excel是导入还是导出
    /// </summary>
    public enum ImportOrExportType
    {
        /// <summary>
        /// 将Excel数据导入程序
        /// </summary>
        Import,
        /// <summary>
        /// 将数据导出到Excel
        /// </summary>
        Export
    }

    public enum ExcelVersion
    {
        /// <summary>
        /// Excel3.0版文档格式
        /// </summary>
        Excel3,
        /// <summary>
        /// Excel4.0版文档格式
        /// </summary>
        Excel4,
        /// <summary>
        /// Excel5.0版文档格式，适用于 Microsoft Excel 5.0 和 7.0 (95) 工作簿
        /// </summary>
        Excel5,
        /// <summary>
        /// Excel8.0版文档格式，适用于Microsoft Excel 8.0 (98-2003) 工作簿
        /// </summary>
        Excel8,
        /// <summary>
        /// Excel12.0版文档格式，适用于Microsoft Excel 12.0 (2007) 工作簿
        /// </summary>
        Excel12
    }


    #endregion    /// <summary>

    /// Excel 帮助
    /// </summary>
    public class ExcelHelper
    {
        private static readonly int m_maxSheelSize = 65000;
        #region 公用静态方法

        /// <summary>
        /// 写数据到Excel。
        /// </summary>
        /// <param name="dtSource">数据源</param>
        /// <param name="filePath">Excel导出路径</param>
        /// <param name="excelVersion">excel版本，为ExcelVersion类型</param>
        /// <param name="pHDR">第一行是否标题，为HDRType类型</param>
        public static void SetExcelData(DataGridView dtSource, string filePath, ExcelVersion excelVersion, HDRType pHDR)
        {
            //数据源为空
            if (dtSource.Rows.Count == 0)
            {
                throw new Exception("无数据可导");
            }
            //保存路径为空
            if (string.IsNullOrEmpty(filePath))
            {
                throw new Exception("未设置Excel保存路径");
            }
            //删除文件
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            string connectionString = string.Format(GetConnectionString(excelVersion, ImportOrExportType.Export),
                filePath, pHDR);

            // 连接Excel 
            using (OleDbConnection Connection = new OleDbConnection(connectionString))
            {
                Connection.Open();

                //导入数据  
                using (OleDbCommand command = new OleDbCommand())
                {
                    command.Connection = Connection;

                    //构建列  格式如：Name VarChar，CreateDate Date
                    string colList = CreateExcelColums(dtSource);

                    //构建插入SQL语句
                    //格式如 "INSERT INTO TABLE [tablename](col1,col2,col3) VALUES(@col1,@col2,@col3)"; 
                    StringBuilder sbColumNames = new StringBuilder();
                    StringBuilder sbColumValues = new StringBuilder();
                    foreach (DataGridViewColumn dc in dtSource.Columns)
                    {
                        sbColumNames.AppendFormat(",{0}", dc.HeaderText);
                        sbColumValues.AppendFormat(",@{0}", dc.Name);
                    }
                    //去掉多余的逗号
                    sbColumNames.Remove(0, 1);
                    sbColumValues.Remove(0, 1);

                    //当数据量超过每页最大数据量时，自动分页
                    int totalRows = dtSource.Rows.Count;//总数据量
                    int pageIndex = 0;

                    //开始插入数据  do...while循环是为了处理分页逻辑
                    do
                    {
                        //计算此轮插入的数据量
                        int insertRows = m_maxSheelSize - 1;
                        //如果总数据量没有达到容量
                        if (totalRows < insertRows)
                        {
                            insertRows = totalRows;
                        }

                        string tableName = dtSource.Name + pageIndex;
                        if (pageIndex == 0)
                        {
                            tableName = "Sheet1";
                        }
                        //创建表框架
                        StringBuilder sbCom = new StringBuilder();
                        sbCom.Append("CREATE TABLE [");
                        sbCom.Append(tableName);
                        sbCom.Append("](");
                        sbCom.Append(colList);
                        sbCom.Append(")");
                        command.CommandText = sbCom.ToString();
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            //如果使用Create语句创建失败则直接创建Excel文件
                            CreateExcelFile(filePath, excelVersion, command.CommandText);
                        }

                        //插入数据
                        sbCom = new StringBuilder();
                        sbCom.AppendFormat("INSERT INTO [{0}]({1}) VALUES({2})",
                                            tableName, sbColumNames.ToString(), sbColumValues.ToString());
                        int startIndex = pageIndex * (m_maxSheelSize - 1);
                        int endIndex = pageIndex * (m_maxSheelSize - 1) + insertRows;
                        for (int i = startIndex; i < endIndex; i++)
                        {
                            DataGridViewRow drData = dtSource.Rows[i];
                            if (!drData.IsNewRow)
                            {
                                OleDbParameterCollection dbParam = command.Parameters;
                                dbParam.Clear();
                                foreach (DataGridViewColumn dc in dtSource.Columns)
                                {
                                    dbParam.Add(new OleDbParameter("@" + dc.Name, OleDbType.VarChar));
                                    dbParam["@" + dc.Name].Value = drData.Cells[dc.Name].Value;
                                }
                                command.CommandText = sbCom.ToString();
                                command.ExecuteNonQuery();
                            }
                        }

                        //计算剩余数据量
                        totalRows = totalRows - insertRows;
                        pageIndex++;

                    } while (totalRows > 0);
                }//end of using OleDbCommand  
            }// end of  using OleDbConnection
        }

        /// <summary>
        /// 写数据到Excel。
        /// </summary>
        /// <param name="dtSource">数据源</param>
        /// <param name="filePath">Excel导出路径</param>
        /// <param name="excelVersion">excel版本，为ExcelVersion类型</param>
        /// <param name="pHDR">第一行是否标题，为HDRType类型</param>
        public static void SetExcelData(DataTable dtSource, string filePath, ExcelVersion excelVersion, HDRType pHDR)
        {
            //数据源为空
            if (dtSource == null)
            {
                throw new Exception("无数据可导");
            }
            //保存路径为空
            if (string.IsNullOrEmpty(filePath))
            {
                throw new Exception("未设置Excel保存路径");
            }
            //删除文件
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            string connectionString = string.Format(GetConnectionString(excelVersion, ImportOrExportType.Export),
                filePath, pHDR);

            // 连接Excel 
            using (OleDbConnection Connection = new OleDbConnection(connectionString))
            {
                Connection.Open();

                //导入数据  
                using (OleDbCommand command = new OleDbCommand())
                {
                    command.Connection = Connection;

                    //构建列  格式如：Name VarChar，CreateDate Date
                    string colList = CreateExcelColums(dtSource);

                    //构建插入SQL语句
                    //格式如 "INSERT INTO TABLE [tablename](col1,col2,col3) VALUES(@col1,@col2,@col3)"; 
                    StringBuilder sbColumNames = new StringBuilder();
                    StringBuilder sbColumValues = new StringBuilder();
                    foreach (DataColumn dc in dtSource.Columns)
                    {
                        sbColumNames.AppendFormat(",{0}", dc.ColumnName);
                        sbColumValues.AppendFormat(",@{0}", dc.ColumnName);
                    }
                    //去掉多余的逗号
                    sbColumNames.Remove(0, 1);
                    sbColumValues.Remove(0, 1);

                    //当数据量超过每页最大数据量时，自动分页
                    int totalRows = dtSource.Rows.Count;//总数据量
                    int pageIndex = 0;

                    //开始插入数据  do...while循环是为了处理分页逻辑
                    do
                    {
                        //计算此轮插入的数据量
                        int insertRows = m_maxSheelSize - 1;
                        //如果总数据量没有达到容量
                        if (totalRows < insertRows)
                        {
                            insertRows = totalRows;
                        }

                        string tableName = dtSource.TableName + pageIndex;
                        if (pageIndex == 0)
                        {
                            tableName = "Sheet1";
                        }
                        //创建表框架
                        StringBuilder sbCom = new StringBuilder();
                        sbCom.Append("CREATE TABLE [");
                        sbCom.Append(tableName);
                        sbCom.Append("](");
                        sbCom.Append(colList);
                        sbCom.Append(")");
                        command.CommandText = sbCom.ToString();
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch
                        {
                            //如果使用Create语句创建失败则直接创建Excel文件
                            CreateExcelFile(filePath, excelVersion, command.CommandText);
                        }

                        //插入数据
                        sbCom = new StringBuilder();
                        sbCom.AppendFormat("INSERT INTO [{0}]({1}) VALUES({2})",
                                            tableName, sbColumNames.ToString(), sbColumValues.ToString());
                        int startIndex = pageIndex * (m_maxSheelSize - 1);
                        int endIndex = pageIndex * (m_maxSheelSize - 1) + insertRows;
                        for (int i = startIndex; i < endIndex; i++)
                        {
                            DataRow drData = dtSource.Rows[i];
                            OleDbParameterCollection dbParam = command.Parameters;
                            dbParam.Clear();
                            foreach (DataColumn dc in dtSource.Columns)
                            {
                                dbParam.Add(new OleDbParameter("@" + dc.ColumnName, GetOleDbTypeByDataColumn(dc)));
                                dbParam["@" + dc.ColumnName].Value = drData[dc.ColumnName];
                            }
                            command.CommandText = sbCom.ToString();
                            command.ExecuteNonQuery();
                        }

                        //计算剩余数据量
                        totalRows = totalRows - insertRows;
                        pageIndex++;

                    } while (totalRows > 0);
                }//end of using OleDbCommand  
            }// end of  using OleDbConnection
        }

        /// <summary>
        /// 从Excel读数据
        /// </summary>
        /// <param name="filePath">excel文档路径</param>
        /// <param name="excelVersion">文档版本</param>
        /// <param name="pHDR">第一行是否标题</param>
        /// <param name="bMerge">
        /// 如果有多页，是否合并数据，合并时必须保证多页的表结构一致
        /// </param>
        /// <returns>DataTable集</returns>
        public static DataTable[] GetExcelData(string filePath, HDRType pHDR, bool bMerge)
        {
            ExcelVersion excelVersion = ExcelVersion.Excel8;
            if (Path.GetExtension(filePath) == ".xlsx")
            {
                excelVersion = ExcelVersion.Excel12;
            }

            List<DataTable> dtResult = new List<DataTable>();
            string connectionString = string.Format(GetConnectionString(excelVersion, ImportOrExportType.Import),
              filePath, pHDR);
            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                con.Open();
                string[] sheels = GetExcelWorkSheets(filePath, excelVersion);
                foreach (string sheelName in sheels)
                {
                    try
                    {
                        DataTable dtExcel = new DataTable();
                        OleDbDataAdapter adapter = new OleDbDataAdapter("Select * from [" + sheelName + "$]", con);

                        adapter.FillSchema(dtExcel, SchemaType.Mapped);
                        adapter.Fill(dtExcel);

                        dtExcel.TableName = sheelName;
                        dtResult.Add(dtExcel);
                    }
                    catch
                    {
                        //容错处理：取不到时，不报错，结果集为空即可。
                    }
                }

                //如果需要合并数据，则合并到第一张表
                if (bMerge)
                {
                    for (int i = 1; i < dtResult.Count; i++)
                    {
                        //如果不为空才合并
                        if (dtResult[0].Columns.Count == dtResult[i].Columns.Count &&
                            dtResult[i].Rows.Count > 0)
                        {
                            dtResult[0].Load(dtResult[i].CreateDataReader());
                        }
                    }
                }
            }
            return dtResult.ToArray();
        }

        #endregion

        #region 私有静态方法


        /// <summary>
        /// 返回指定文件所包含的工作簿列表;如果有WorkSheet，就返回以工作簿名字命名的ArrayList，否则返回空
        /// </summary>
        /// <param name="filePath">要获取的Excel</param>
        /// <param name="excelVersion">文档版本</param>
        /// <returns>如果有WorkSheet，就返回以工作簿名字命名的string[]，否则返回空</returns>
        private static string[] GetExcelWorkSheets(string filePath, ExcelVersion excelVersion)
        {
            List<string> alTables = new List<string>();
            string connectionString = string.Format(GetConnectionString(excelVersion, ImportOrExportType.Import),
              filePath, "Yes");
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                DataTable dt = new DataTable();

                dt = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                if (dt == null)
                {
                    throw new Exception("无法获取指定Excel的架构。");
                }

                foreach (DataRow dr in dt.Rows)
                {
                    string tempName = dr["Table_Name"].ToString();

                    int iDolarIndex = tempName.IndexOf('$');

                    if (iDolarIndex > 0)
                    {
                        tempName = tempName.Substring(0, iDolarIndex);
                    }

                    //修正Excel2003中某些工作薄名称为汉字的表无法正确识别的BUG。
                    if (tempName[0] == '\'')
                    {
                        if (tempName[tempName.Length - 1] == '\'')
                        {
                            tempName = tempName.Substring(1, tempName.Length - 2);
                        }
                        else
                        {
                            tempName = tempName.Substring(1, tempName.Length - 1);
                        }
                    }
                    if (!alTables.Contains(tempName))
                    {
                        alTables.Add(tempName);
                    }

                }
            }

            if (alTables.Count == 0)
            {
                return null;
            }
            return alTables.ToArray();
        }

        /// <summary>
        /// 创建Excel文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="excelVersion">excel版本</param>
        /// <param name="excelVersion">创建sheet的脚本</param>
        private static void CreateExcelFile(string filePath, ExcelVersion excelVersion, string createSql)
        {
            string outputDir = Path.GetDirectoryName(filePath);
            //导出路径不存在则创建
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
            //导出文件不存在则创建，存在则重写

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                if (excelVersion == ExcelVersion.Excel12)
                {
                    //创建2007Excel
                    fs.Write(Resources._2007, 0, Resources._2007.Length);
                }
                else
                {
                    //其他默认创建2003Excel
                    fs.Write(Resources._2003, 0, Resources._2003.Length);
                }

                //插入Sheet表。
                string connectionString = string.Format(GetConnectionString(excelVersion, ImportOrExportType.Export),
                                filePath, "Yes");
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand())
                    {
                        command.CommandText = createSql;
                        command.ExecuteNonQuery();
                    }
                }
            }

        }

        /// <summary>
        /// 构建Excel列脚本。
        /// 格式如：Name VarChar，CreateDate Date
        /// </summary>
        /// <param name="dtSource"></param>
        /// <returns></returns>
        private static string CreateExcelColums(DataTable dtSource)
        {
            //检查列数
            if (dtSource.Columns.Count == 0)
            {
                throw new Exception("数据源列数为0");
            }
            //构建列
            StringBuilder sbColums = new StringBuilder();
            foreach (DataColumn dc in dtSource.Columns)
            {
                sbColums.AppendFormat(",{0} {1}", dc.ColumnName, GetOleDbTypeByDataColumn(dc).ToString());
            }
            //去掉多余的逗号
            sbColums.Remove(0, 1);
            return sbColums.ToString();
        }
        /// <summary>
        /// 构建Excel列脚本。
        /// 格式如：Name VarChar，CreateDate Date
        /// </summary>
        /// <param name="dtSource"></param>
        /// <returns></returns>
        private static string CreateExcelColums(DataGridView dtSource)
        {
            //检查列数
            if (dtSource.Columns.Count == 0)
            {
                throw new Exception("数据源列数为0");
            }
            //构建列
            StringBuilder sbColums = new StringBuilder();
            foreach (DataGridViewColumn dc in dtSource.Columns)
            {
                sbColums.AppendFormat(",{0} {1}", dc.HeaderText, OleDbType.VarChar);
            }
            //去掉多余的逗号
            sbColums.Remove(0, 1);

            return sbColums.ToString();
        }

        /// <summary>
        /// 获取DataColumn对应的Excel列类型
        /// </summary>
        /// <param name="dc">源数据的列</param>
        /// <returns>Excel列类型名称</returns>
        private static OleDbType GetOleDbTypeByDataColumn(DataColumn dc)
        {
            switch (dc.DataType.Name)
            {
                case "String"://字符串
                    return OleDbType.VarChar;
                case "Double"://数字
                    return OleDbType.Double;
                case "Decimal"://数字
                    return OleDbType.Decimal;
                case "DateTime"://时间
                    return OleDbType.Date;
                default:
                    return OleDbType.VarChar;
            }
        }
        /// <summary>
        /// 获得连接
        /// </summary>
        /// <param name="excelVersion"></param>
        /// <returns></returns>
        private static string GetConnectionString(ExcelVersion excelVersion, ImportOrExportType etype)
        {
            if (etype == ImportOrExportType.Import)
            {
                if (excelVersion == ExcelVersion.Excel12)
                {
                    return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 Xml;HDR={1};IMEX=1'";
                }
                else if (excelVersion == ExcelVersion.Excel3)
                {
                    return "Provider=Microsoft.Jet.OleDb.4.0; Data Source={0}; Extended Properties='Excel 3.0;HDR={1};IMEX=1'";
                }
                else if (excelVersion == ExcelVersion.Excel4)
                {
                    return "Provider=Microsoft.Jet.OleDb.4.0; Data Source={0}; Extended Properties='Excel 4.0;HDR={1};IMEX=1'";
                }
                else if (excelVersion == ExcelVersion.Excel5)
                {
                    return "Provider=Microsoft.Jet.OleDb.4.0; Data Source={0}; Extended Properties='Excel 5.0;HDR={1};IMEX=1'";
                }
                else
                {
                    return "Provider=Microsoft.Jet.OleDb.4.0; Data Source={0}; Extended Properties='Excel 8.0;HDR={1};IMEX=1'";
                }
            }
            else
            {
                if (excelVersion == ExcelVersion.Excel12)
                {
                    return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 Xml;HDR={1}'";
                }
                else if (excelVersion == ExcelVersion.Excel3)
                {
                    return "Provider=Microsoft.Jet.OleDb.4.0; Data Source={0}; Extended Properties='Excel 3.0;HDR={1}'";
                }
                else if (excelVersion == ExcelVersion.Excel4)
                {
                    return "Provider=Microsoft.Jet.OleDb.4.0; Data Source={0}; Extended Properties='Excel 4.0;HDR={1};'";
                }
                else if (excelVersion == ExcelVersion.Excel5)
                {
                    return "Provider=Microsoft.Jet.OleDb.4.0; Data Source={0}; Extended Properties='Excel 5.0;HDR={1};'";
                }
                else
                {
                    return "Provider=Microsoft.Jet.OleDb.4.0; Data Source={0}; Extended Properties='Excel 8.0;HDR={1};'";
                }
            }
        }
        #endregion

        #region   运用流导出Excel
        public static void DataToExcel(DataGridView dgv, string FileName)
        {
            FileStream myStream = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Write);
            //StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(-0));

            string columnTitle = "";
            //写入列标题   
            for (int i = 0; i < dgv.ColumnCount; i++)
            {
                if (i > 0)
                {
                    columnTitle += "\t";
                }
                columnTitle += dgv.Columns[i].HeaderText;
            }
            byte[] arr = Encoding.Default.GetBytes(columnTitle + "\n");
            myStream.Write(arr, 0, arr.Length);
            //sw.WriteLine(columnTitle);

            //写入列内容   
            for (int j = 0; j < dgv.Rows.Count; j++)
            {
                string columnValue = "";
                for (int k = 0; k < dgv.Columns.Count; k++)
                {
                    if (k > 0)
                    {
                        columnValue += "\t";
                    }
                    if (dgv.Rows[j].Cells[k].Value == null)
                        columnValue += "";
                    else
                        columnValue += "'" + dgv.Rows[j].Cells[k].Value.ToString();
                }
                byte[] arrValue = Encoding.Default.GetBytes(columnValue + "\n");
                myStream.Write(arrValue, 0, arr.Length);
                //sw.WriteLine(columnValue);
            }
            //sw.Close();
            myStream.Close();
        }
        #endregion
    }

}
