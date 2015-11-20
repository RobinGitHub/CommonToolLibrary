using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 自定义Panel列表V1
{
    public partial class Form1 : Form
    {
        DataTable dt = null;
        public Form1()
        {
            InitializeComponent();

            this.panelEx1.SetItemTemplate += panelEx1_SetItemTemplate;
            this.panelEx1.SelectionChanged += panelEx1_SelectionChanged;
            this.panelEx1.LoadMore += panelEx1_LoadMore;

            this.btnAdd.Click += btnAdd_Click;
            this.btnUpdate.Click += btnUpdate_Click;
            this.btnDel.Click += btnDel_Click;
            this.btnInit.Click += btnInit_Click;
            this.btnInsert.Click += btnInsert_Click;
            this.btnAddByDt.Click += btnAddByDt_Click;
            this.btnClear.Click += btnClear_Click;
            this.btnGetSelected.Click += btnGetSelected_Click;
            this.btnLocation.Click += btnLocation_Click;

            this.cbxShowMore.CheckedChanged += cbxShowMore_CheckedChanged;
            this.cbxIsGroup.CheckedChanged += cbxIsGroup_CheckedChanged;
            this.cbxIsEqualHeight.CheckedChanged += cbxIsEqualHeight_CheckedChanged;

            this.panelEx1.MinRowHeight = 60;
            this.panelEx1.IsEqualHeight = false;

            dt = GetDataSource();
        }

        void panelEx1_LoadMore(object sender, EventArgs e)
        {
            btnAddByDt.PerformClick();
        }

        void btnLocation_Click(object sender, EventArgs e)
        {
            this.panelEx1.FirstDisplayedScrollingRowIndex = int.Parse(txtLocation.Text);
        }

        void btnGetSelected_Click(object sender, EventArgs e)
        {
            List<PanelItem> itemList = this.panelEx1.SelectedItems();
            if (itemList.Count == 0)
                return;
            richTextBox1.AppendText("\n===SelectedItems===\n");
            foreach (PanelItem item in itemList)
            {
                richTextBox1.AppendText("索引：" + item.RowIndex + "| 标题：" + item.DataRow["Title"].ToString() + "\n");
            }
            richTextBox1.AppendText("===SelectedItems===\n\n");
        }

        void btnClear_Click(object sender, EventArgs e)
        {
            this.panelEx1.Clear();
        }

        void cbxIsEqualHeight_CheckedChanged(object sender, EventArgs e)
        {
            this.panelEx1.Clear();
            this.panelEx1.IsEqualHeight = cbxIsEqualHeight.Checked;
        }

        void cbxIsGroup_CheckedChanged(object sender, EventArgs e)
        {
            this.panelEx1.Clear();
            this.panelEx1.IsGroup = cbxIsGroup.Checked;
        }

        void cbxShowMore_CheckedChanged(object sender, EventArgs e)
        {
            this.panelEx1.IsShowMore = cbxShowMore.Checked;
        }



        private void btnInit_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            DateTime startTime = DateTime.Now;
            this.panelEx1.DataSource<ReplyModel>(GetDataSource());
            richTextBox1.AppendText("总耗时：" + (DateTime.Now - startTime).TotalMilliseconds + "\n");
            richTextBox1.ScrollToCaret();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DataRow row = dt.NewRow();
            row[0] = dt.Rows.Count;
            row[1] = " 新增,TTTT" + this.panelEx1.Count;
            row[2] = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd HH:mm");
            row[3] = "超级管理员";
            dt.Rows.Add(row);

            ReplyModel item = new ReplyModel();
            item.DataRow = row;
            this.panelEx1.Add(item);
        }
        void btnInsert_Click(object sender, EventArgs e)
        {
            DataRow row = dt.NewRow();
            row[0] = dt.Rows.Count;
            row[1] = " 插入 TTTTTT 生日,TTTT敬请关注" + this.panelEx1.Count;
            row[2] = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd HH:mm");
            row[3] = "超级管理员";
            dt.Rows.Add(row);

            ReplyModel item = new ReplyModel();
            item.DataRow = row;
            this.panelEx1.Insert(int.Parse(txtInsertRowIndex.Text), item);
        }
        void btnAddByDt_Click(object sender, EventArgs e)
        {
            DateTime startTime = DateTime.Now;
            this.panelEx1.Add<ReplyModel>(AddByDt());
            richTextBox1.AppendText("总耗时：" + (DateTime.Now - startTime).TotalMilliseconds + "\n");
            richTextBox1.ScrollToCaret();
        }

        int updCount = 0;
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            List<PanelItem> selectedItems = this.panelEx1.SelectedItems();
            foreach (PanelItem item in selectedItems)
            {
                item.DataRow["Title"] = "更新" + updCount.ToString();
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

        MyPanelChild panelEx1_SetItemTemplate(PanelItem item)
        {
            MyPanelChild control = null;
            DateTime startTime = DateTime.Now;
            if (cbxIsEqualHeight.Checked)
            {
                WorkBench pnl = new WorkBench();
                pnl.PanelItem = item;
                pnl.DataRow = item.DataRow;
                pnl.RowIndex = item.RowIndex;
                pnl.RefreshData();
                control = pnl;
            }
            else
            {
                ReplyModel model = item as ReplyModel;
                model.ReplyData = GetReply(int.Parse(model.DataRow[0].ToString()));

                ReplyUserControl pnl = new ReplyUserControl();
                pnl.PanelItem = model;
                pnl.DataRow = item.DataRow;
                pnl.RowIndex = item.RowIndex;
                pnl.SizeChanged += pnl_SizeChanged;
                control = pnl;
            }
            richTextBox1.AppendText((DateTime.Now - startTime).TotalMilliseconds + "\n");
            richTextBox1.ScrollToCaret();
            return control;
        }

        void pnl_SizeChanged(object sender, EventArgs e)
        {
            MyPanelChild pnl = sender as MyPanelChild;
            this.panelEx1.Refresh(pnl.PanelItem.RowIndex);
        }
        void panelEx1_SelectionChanged(PanelItem item)
        {

        }

        private DataTable GetDataSource()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Title");
            dt.Columns.Add("Date");
            dt.Columns.Add("UserName");

            for (int i = 0; i < int.Parse(txtRowCount.Text); i++)
            {
                DataRow row = dt.NewRow();
                row[0] = i;
                row[1] = "【生日】 2014/11/20 生日,新增生死时速新增生死时速新增生死时速新增生死时速新增生死时速新增生死时速新增生死时速新增生死时速新增生死时速新增生死时速新增生死时速新增生死时速新增生死时速新增生死时速新增生死时速新增生死时速新增生死时速新增生死时速新增生" + i.ToString();
                row[2] = DateTime.Now.AddDays(-i).ToString("yyyy-MM-dd HH:mm");
                row[3] = "超级管理员";
                dt.Rows.Add(row);
            }
            return dt;
        }

        private DataTable AddByDt()
        {
            DataTable newDt = dt.Clone();
            for (int i = 0; i < int.Parse(txtAddRowCount.Text); i++)
            {
                DataRow row = newDt.NewRow();
                row[0] = i;
                row[1] = "新增生死时速新增生死时速新增生死时速新增生死时速新增生死时速新增生死时速新增生死时速新增生死时速新增生死时速新增生死时速新增生死时速新增生死时速新增生死时速新增生死时速新增生死时速新增生死时速新增生死时速新增生死时速" + i.ToString();
                row[2] = DateTime.Now.AddDays(-i).ToString("yyyy-MM-dd HH:mm");
                row[3] = "超级管理员";
                newDt.Rows.Add(row);
            }
            return newDt;
        }

        private DataTable GetReply(int i)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ParentID");
            dt.Columns.Add("Title");
            dt.Columns.Add("Date");
            dt.Columns.Add("UserName");

            for (int j = 0; j < 5; j++)
            {
                DataRow row = dt.NewRow();
                if (j < 3)
                    row[0] = i;
                else
                    row[0] = i;

                if (j % 2 == 0)
                    row[1] = "测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试测试";
                else if (j % 3 == 0)
                    row[1] = "同意同意同意同意同意同意同意同意同意同意同意同意同意同意同意同意同意同意同意同意同意";
                else if (j % 4 == 0)
                    row[1] = "不同意不同意不同意不同意不同意不同意不同意不同意不同意不同意不同意不同意不同意不同意不同意";
                else
                    row[1] = "生死时速生死时速生死时速少时诵诗书少时诵诗书生死时速送555555";

                row[2] = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                row[3] = "花花" + j.ToString();
                dt.Rows.Add(row);
            }

            DataView dv = dt.DefaultView;
            dv.RowFilter = "ParentID=" + i.ToString();
            return dv.ToTable();
        }

    }
}
