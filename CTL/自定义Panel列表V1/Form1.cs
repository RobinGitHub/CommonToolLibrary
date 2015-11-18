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

            this.btnAdd.Click += btnAdd_Click;
            this.btnUpdate.Click += btnUpdate_Click;
            this.btnDel.Click += btnDel_Click;
            this.btnInit.Click += btnInit_Click;

            this.panelEx1.MinRowHeight = 60;
            this.panelEx1.IsEqualHeight = false;
        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            this.panelEx1.IsShowMore = true;
            richTextBox1.Clear(); 
            DateTime startTime = DateTime.Now;
            this.panelEx1.DataSource<ReplyModel>(GetDataSource());
            richTextBox1.AppendText("总耗时：" + (DateTime.Now - startTime).TotalMilliseconds + "\n");
            richTextBox1.ScrollToCaret();
        }

        int count = 0;
        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.panelEx1.IsShowMore = false;
            //DataRow row = dt.NewRow();
            //row[0] = "新" + count;
            //dt.Rows.Add(row);

            //PanelItem item = new PanelItem();
            //item.DataRow = row;
            //item.Height = this.panelEx1.MinRowHeight;
            //this.panelEx1.Add(item);
            //count++;
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
        
        MyPanelChild panelEx1_SetItemTemplate(PanelItem item)
        {
            DateTime startTime = DateTime.Now;
            ReplyModel model = item as ReplyModel;
            model.ReplyData = GetReply(int.Parse(model.DataRow[0].ToString()));

            ReplyUserControl pnl = new ReplyUserControl();
            pnl.PanelItem = model;
            pnl.DataRow = item.DataRow;
            pnl.RowIndex = item.RowIndex;
            pnl.RefreshData();
            pnl.SizeChanged += pnl_SizeChanged;
            richTextBox1.AppendText((DateTime.Now - startTime).TotalMilliseconds + "\n");
            richTextBox1.ScrollToCaret();
            return pnl;
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
                row[1] = "【生日】 2014/11/20 生日,敬请关注" + i.ToString();
                row[2] = DateTime.Now.AddDays(i).ToString("yyyy-MM-dd HH:mm");
                row[3] = "超级管理员";
                dt.Rows.Add(row);
            }
            return dt;
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
                    row[0] = 0;
                else
                    row[0] = j;

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
