using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 自定义TreeView仿VS解决方案效果
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            this.myVScrollBar1.BindControl = this.dataGridView1;
            this.myHScrollBar1.BindControl = this.dataGridView1;
            this.dataGridView1.ColumnStateChanged += dataGridView1_ColumnStateChanged;
        }

        void dataGridView1_ColumnStateChanged(object sender, DataGridViewColumnStateChangedEventArgs e)
        {
            
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
            this.myHScrollBar1.UpdateScrollbar();
            this.myVScrollBar1.UpdateScrollbar();
        }
    }
}
