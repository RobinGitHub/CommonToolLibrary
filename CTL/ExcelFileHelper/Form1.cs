using Excel;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace ExcelFileHelper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    System.Data.DataTable dataSrouce = ExcelHelper.GetExcelData(ofd.FileName, HDRType.NO, false)[0];
                    foreach (DataRow item in dataSrouce.Rows)
                    {

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    string filePath = ofd.FileName;
                    FileStream stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read);
                    IExcelDataReader excelReader = null;
                    if (Path.GetExtension(filePath) == ".xls")
                    {
                        //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                        excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (Path.GetExtension(filePath) == ".xlsx")
                    {
                        //...
                        //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                        excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    
                    //...
                    //3. DataSet - The result of each spreadsheet will be created in the result.Tables
                    DataSet result = excelReader.AsDataSet();
                    //...
                    //4. DataSet - Create column names from first row
                    excelReader.IsFirstRowAsColumnNames = true;
                    DataSet result1 = excelReader.AsDataSet();

                    //5. Data Reader methods
                    while (excelReader.Read())
                    {
                        //excelReader.GetInt32(0);
                    }

                    //6. Free resources (IExcelDataReader is IDisposable)
                    excelReader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    NPOIExcelHelper helper = new NPOIExcelHelper(ofd.FileName);
                    dataGridView1.DataSource = helper.ExcelToDataTable("sheet1", true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

    }
}
