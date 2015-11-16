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
    public partial class Form4 : Form
    {
        DataTable dt = null;
        public Form4()
        {
            InitializeComponent();
            this.panelEx1.UpdateChildItem += panelEx1_UpdateChildItem;
            this.panelEx1.SetItemTemplate += panelEx1_SetItemTemplate;
            this.panelEx1.SelectionChanged += panelEx1_SelectionChanged;

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

        MyPanelChild panelEx1_UpdateChildItem(PanelItem item)
        {
            DateTime startTime = DateTime.Now;
            MyPanelChild control = null;
            if (item.PanelChild == null)
            {
                control = panelEx1_SetItemTemplate(item);
                item.PanelChild = control; 
            }
            else
            {
                control = item.PanelChild;
                //control.RefreshData(); 
            }
            richTextBox1.AppendText((DateTime.Now - startTime).TotalMilliseconds + " TTTTTTTTTT \n");
            richTextBox1.ScrollToCaret();
            return control;
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
            pnl.SizeChanged += pnl_SizeChanged;
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

        private void InitializeComponent()
        {
            this.cbxEqualHeight = new System.Windows.Forms.CheckBox();
            this.txtRowCount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnInit = new System.Windows.Forms.Button();
            this.panelEx1 = new 自定义Panel列表.MyPanelList_V3();
            this.SuspendLayout();
            // 
            // cbxEqualHeight
            // 
            this.cbxEqualHeight.AutoSize = true;
            this.cbxEqualHeight.Checked = true;
            this.cbxEqualHeight.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxEqualHeight.Location = new System.Drawing.Point(114, 14);
            this.cbxEqualHeight.Name = "cbxEqualHeight";
            this.cbxEqualHeight.Size = new System.Drawing.Size(48, 16);
            this.cbxEqualHeight.TabIndex = 47;
            this.cbxEqualHeight.Text = "等高";
            this.cbxEqualHeight.UseVisualStyleBackColor = true;
            // 
            // txtRowCount
            // 
            this.txtRowCount.Location = new System.Drawing.Point(40, 12);
            this.txtRowCount.Name = "txtRowCount";
            this.txtRowCount.Size = new System.Drawing.Size(41, 21);
            this.txtRowCount.TabIndex = 46;
            this.txtRowCount.Text = "20";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 45;
            this.label1.Text = "行：";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBox1.Location = new System.Drawing.Point(5, 98);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(157, 230);
            this.richTextBox1.TabIndex = 44;
            this.richTextBox1.Text = "";
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(6, 69);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.TabIndex = 43;
            this.btnDel.Text = "删除";
            this.btnDel.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(88, 69);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 42;
            this.btnUpdate.Text = "修改";
            this.btnUpdate.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(88, 39);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 41;
            this.btnAdd.Text = "增加";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnInit
            // 
            this.btnInit.Location = new System.Drawing.Point(7, 39);
            this.btnInit.Name = "btnInit";
            this.btnInit.Size = new System.Drawing.Size(75, 23);
            this.btnInit.TabIndex = 40;
            this.btnInit.Text = "初始化";
            this.btnInit.UseVisualStyleBackColor = true;
            // 
            // panelEx1
            // 
            this.panelEx1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelEx1.BackColor = System.Drawing.Color.White;
            this.panelEx1.DefaultColor = System.Drawing.Color.White;
            this.panelEx1.FirstDisplayedScrollingRowIndex = 0;
            this.panelEx1.Location = new System.Drawing.Point(186, 0);
            this.panelEx1.MinRowHeight = 60;
            this.panelEx1.MouseEnterColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(240)))), ((int)(((byte)(193)))));
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(235)))), ((int)(((byte)(166)))));
            this.panelEx1.Size = new System.Drawing.Size(304, 335);
            this.panelEx1.TabIndex = 48;
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 335);
            this.Controls.Add(this.panelEx1);
            this.Controls.Add(this.cbxEqualHeight);
            this.Controls.Add(this.txtRowCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnInit);
            this.Name = "Form4";
            this.Text = "Form4";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
