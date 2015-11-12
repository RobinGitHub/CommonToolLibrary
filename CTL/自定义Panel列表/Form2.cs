using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 自定义Panel列表
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.dataGridView1.DataSource = DataSource();
        }


        private DataTable DataSource()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Title");
            dt.Columns.Add("Desc");

            for (int i = 0; i < 100; i++)
            {
                DataRow row = dt.NewRow();
                row[0] = "测试" + i.ToString();
                row[1] = "描述" + i.ToString();
                dt.Rows.Add(row);
            }
            return dt;
        }
    }
}
