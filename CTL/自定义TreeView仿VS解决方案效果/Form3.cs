using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Win32API;

namespace 自定义TreeView仿VS解决方案效果
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            this.myVScrollBar1.BindControl = this.dataGridView1;
            this.myHScrollBar1.BindControl = this.dataGridView1;

        }

        private DataTable DataSource()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Title");
            dt.Columns.Add("Desc");
            dt.Columns.Add("Desc1");
            dt.Columns.Add("Desc2");
            dt.Columns.Add("Desc3");

            for (int i = 0; i < 10; i++)
            {
                DataRow row = dt.NewRow();
                row[0] = "测试" + i.ToString();
                row[1] = "描述" + i.ToString();
                row[2] = "描述" + i.ToString();
                row[3] = "描述" + i.ToString();
                row[4] = "描述" + i.ToString();
                dt.Rows.Add(row);
            }
            return dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = DataSource();

            Type type = dataGridView1.GetType();
            PropertyInfo pi = type.GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dataGridView1, true, null);
            //this.myHScrollBar1.UpdateScrollbar();
            //this.myVScrollBar1.UpdateScrollbar();

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
