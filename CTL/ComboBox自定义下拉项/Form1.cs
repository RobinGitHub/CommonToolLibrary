using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ComboBox自定义下拉项
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        void Form1_Load(object sender, EventArgs e)
        {
            DataTable dtSource = new DataTable();
            dtSource.TableName = "table1";
            dtSource.Columns.Add("Column1");
            dtSource.Columns.Add("Column2");
            dtSource.Columns.Add("Column3");
            dtSource.Columns.Add("Column4");
            dtSource.Columns.Add("Column5");

            for (int i = 0; i < 10; i++)
            {
                DataRow row = dtSource.NewRow();
                row[0] = string.Format("第{0}行，第1列", i + 1);
                row[1] = string.Format("第{0}行，第1列", i + 1);
                row[2] = string.Format("第{0}行，第1列", i + 1);
                row[3] = string.Format("第{0}行，第1列", i + 1);
                row[4] = string.Format("第{0}行，第1列", i + 1);
                dtSource.Rows.Add(row);
            }

            DataGridView gridView = new DataGridView();
            gridView.AutoGenerateColumns = true;
            gridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridView.CellMouseDoubleClick += gridView_CellMouseDoubleClick;
            gridView.AllowUserToAddRows = false;
            gridView.AllowUserToDeleteRows = false;
            gridView.ReadOnly = true;
            gridView.BorderStyle = BorderStyle.None;
            gridView.Columns.Add("Column1", "Column 1");
            gridView.Columns.Add("Column2", "Column 2");
            gridView.Columns.Add("Column3", "Column 3");
            gridView.Columns.Add("Column4", "Column 4");
            gridView.Columns.Add("Column5", "Column 5");

            gridView.Rows.Add(10);
            for (int i = 0; i < 10; i++)
            {
                gridView.Rows[i].Cells[0].Value = string.Format("第{0}行，第1列", i + 1);
                gridView.Rows[i].Cells[1].Value = string.Format("第{0}行，第2列", i + 1);
                gridView.Rows[i].Cells[2].Value = string.Format("第{0}行，第3列", i + 1);
                gridView.Rows[i].Cells[3].Value = string.Format("第{0}行，第4列", i + 1);
                gridView.Rows[i].Cells[4].Value = string.Format("第{0}行，第5列", i + 1);
            }

            this.customComboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.customComboBox1.DropDownControl = gridView;
        }

        void gridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            DataGridViewRow dgvr = dgv.Rows[e.RowIndex];
            this.customComboBox1.Text = dgvr.Cells[0].Value.ToString();
            this.customComboBox1.HideDropDown();
        }
    }
}
