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
    public partial class Form3 : Form
    {
        DataTable dt = null;
        public Form3()
        {
            InitializeComponent();
            dt = DataSource();
            this.panelEx1.UpdateChildItem += panelEx1_UpdateChildItem;
            this.panelEx1.SetItemTemplate += panelEx1_SetItemTemplate;
            this.panelEx1.SelectionChanged += panelEx1_SelectionChanged;

            this.cbxEqualHeight.CheckedChanged += cbxEqualHeight_CheckedChanged;

            this.btnAdd.Click += btnAdd_Click;
            this.btnUpdate.Click += btnUpdate_Click;
            this.btnDel.Click += btnDel_Click;
            this.btnInit.Click += btnInit_Click;

            this.panelEx1.MinRowHeight = 60;
        }

        int heightOffset = 0;
        private void btnInit_Click(object sender, EventArgs e)
        {
            heightOffset = 0;
            DateTime startDate = DateTime.Now;
            richTextBox1.AppendText(startDate.ToString() + "\n");
            this.panelEx1.DataSource = DataSource();
            richTextBox1.AppendText((DateTime.Now - startDate).TotalMilliseconds.ToString() + "\n");
        }

        int count = 0;
        private void btnAdd_Click(object sender, EventArgs e)
        {
            DataRow row = dt.NewRow();
            row[0] = "新" + count;
            dt.Rows.Add(row);

            PanelItem item = new PanelItem();
            item.DataRow = row;
            item.Height = this.panelEx1.MinRowHeight;
            this.panelEx1.Add(item);
            count++;
        }

        int updCount = 0;
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            List<PanelItem> selectedItems = this.panelEx1.SelectedItems();
            foreach (PanelItem item in selectedItems)
            {
                item.DataRow[0] = "更新" + updCount.ToString();
                this.panelEx1.Refresh(item.RowIndex);
            }
            updCount++;
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            List<PanelItem> selectedItems = this.panelEx1.SelectedItems();
            List<int> indexArr = selectedItems.Select(t => t.RowIndex).ToList();
            this.panelEx1.Remove(indexArr);
        }

        void panelEx1_UpdateChildItem(object sender, EventArgs e)
        {
            WorkBench item = sender as WorkBench;
            item.RefreshData();
        }
        MyPanelChild panelEx1_SetItemTemplate(PanelItem item)
        {
            int height = this.panelEx1.MinRowHeight;
            if (!cbxEqualHeight.Checked)
            {
                height += heightOffset;
                heightOffset++;
            }

            WorkBench pnl = new WorkBench();
            pnl.Height = height;
            pnl.DataRow = item.DataRow;
            pnl.RowIndex = item.RowIndex;
            pnl.RefreshData();
            pnl.SizeChanged +=pnl_SizeChanged;
            return pnl;
        }

        void pnl_SizeChanged(object sender, EventArgs e)
        {
            MyPanelChild pnl = sender as MyPanelChild;
            this.panelEx1.Refresh(pnl.RowIndex);
        }
        void panelEx1_SelectionChanged(PanelItem item)
        {
            richTextBox1.AppendText("SelectionChanged \n");
            richTextBox1.ScrollToCaret();
        }

        void cbxEqualHeight_CheckedChanged(object sender, EventArgs e)
        {
            this.panelEx1.IsEqualHeight = cbxEqualHeight.Checked;
        }

        private DataTable DataSource()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Title");
            dt.Columns.Add("Desc");

            for (int i = 0; i < int.Parse(txtRowCount.Text); i++)
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
