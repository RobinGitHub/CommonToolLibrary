namespace 自定义Panel列表V1
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtRowCount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnInit = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btnInsert = new System.Windows.Forms.Button();
            this.panelEx1 = new 自定义Panel列表V1.MyPanelList();
            this.btnAddByDt = new System.Windows.Forms.Button();
            this.cbxShowMore = new System.Windows.Forms.CheckBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnGetSelected = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtInsertRowIndex = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAddRowCount = new System.Windows.Forms.TextBox();
            this.cbxIsGroup = new System.Windows.Forms.CheckBox();
            this.cbxIsEqualHeight = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.btnLocation = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtRowCount
            // 
            this.txtRowCount.Location = new System.Drawing.Point(39, 113);
            this.txtRowCount.Name = "txtRowCount";
            this.txtRowCount.Size = new System.Drawing.Size(41, 21);
            this.txtRowCount.TabIndex = 53;
            this.txtRowCount.Text = "20";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 117);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 52;
            this.label1.Text = "行：";
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(4, 196);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.TabIndex = 51;
            this.btnDel.Text = "删除";
            this.btnDel.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(86, 196);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 50;
            this.btnUpdate.Text = "修改";
            this.btnUpdate.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(86, 84);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 49;
            this.btnAdd.Text = "增加单个";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnInit
            // 
            this.btnInit.Location = new System.Drawing.Point(86, 112);
            this.btnInit.Name = "btnInit";
            this.btnInit.Size = new System.Drawing.Size(75, 23);
            this.btnInit.TabIndex = 48;
            this.btnInit.Text = "绑定数据DataSource";
            this.btnInit.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBox1.Location = new System.Drawing.Point(5, 254);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(156, 172);
            this.richTextBox1.TabIndex = 55;
            this.richTextBox1.Text = "";
            // 
            // btnInsert
            // 
            this.btnInsert.Location = new System.Drawing.Point(86, 140);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(75, 23);
            this.btnInsert.TabIndex = 57;
            this.btnInsert.Text = "插入单个";
            this.btnInsert.UseVisualStyleBackColor = true;
            // 
            // panelEx1
            // 
            this.panelEx1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelEx1.BackColor = System.Drawing.Color.White;
            this.panelEx1.DefaultColor = System.Drawing.Color.White;
            this.panelEx1.FirstDisplayedScrollingRowIndex = 0;
            this.panelEx1.GroupFieldName = "Date";
            this.panelEx1.IsEqualHeight = false;
            this.panelEx1.Location = new System.Drawing.Point(167, 0);
            this.panelEx1.MinRowHeight = 60;
            this.panelEx1.MouseEnterColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(240)))), ((int)(((byte)(193)))));
            this.panelEx1.MultiSelect = true;
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(235)))), ((int)(((byte)(166)))));
            this.panelEx1.Size = new System.Drawing.Size(371, 426);
            this.panelEx1.TabIndex = 54;
            // 
            // btnAddByDt
            // 
            this.btnAddByDt.Location = new System.Drawing.Point(86, 168);
            this.btnAddByDt.Name = "btnAddByDt";
            this.btnAddByDt.Size = new System.Drawing.Size(75, 23);
            this.btnAddByDt.TabIndex = 58;
            this.btnAddByDt.Text = "批量增加";
            this.btnAddByDt.UseVisualStyleBackColor = true;
            // 
            // cbxShowMore
            // 
            this.cbxShowMore.AutoSize = true;
            this.cbxShowMore.Location = new System.Drawing.Point(89, 12);
            this.cbxShowMore.Name = "cbxShowMore";
            this.cbxShowMore.Size = new System.Drawing.Size(72, 16);
            this.cbxShowMore.TabIndex = 59;
            this.cbxShowMore.Text = "显示更多";
            this.cbxShowMore.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(5, 56);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(156, 23);
            this.btnClear.TabIndex = 60;
            this.btnClear.Text = "清空";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // btnGetSelected
            // 
            this.btnGetSelected.Location = new System.Drawing.Point(6, 84);
            this.btnGetSelected.Name = "btnGetSelected";
            this.btnGetSelected.Size = new System.Drawing.Size(75, 23);
            this.btnGetSelected.TabIndex = 61;
            this.btnGetSelected.Text = "获取选中项";
            this.btnGetSelected.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 145);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 52;
            this.label2.Text = "行：";
            // 
            // txtInsertRowIndex
            // 
            this.txtInsertRowIndex.Location = new System.Drawing.Point(38, 141);
            this.txtInsertRowIndex.Name = "txtInsertRowIndex";
            this.txtInsertRowIndex.Size = new System.Drawing.Size(41, 21);
            this.txtInsertRowIndex.TabIndex = 53;
            this.txtInsertRowIndex.Text = "20";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 173);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 52;
            this.label3.Text = "行：";
            // 
            // txtAddRowCount
            // 
            this.txtAddRowCount.Location = new System.Drawing.Point(38, 169);
            this.txtAddRowCount.Name = "txtAddRowCount";
            this.txtAddRowCount.Size = new System.Drawing.Size(41, 21);
            this.txtAddRowCount.TabIndex = 53;
            this.txtAddRowCount.Text = "20";
            // 
            // cbxIsGroup
            // 
            this.cbxIsGroup.AutoSize = true;
            this.cbxIsGroup.Location = new System.Drawing.Point(4, 12);
            this.cbxIsGroup.Name = "cbxIsGroup";
            this.cbxIsGroup.Size = new System.Drawing.Size(72, 16);
            this.cbxIsGroup.TabIndex = 59;
            this.cbxIsGroup.Text = "是否分组";
            this.cbxIsGroup.UseVisualStyleBackColor = true;
            // 
            // cbxIsEqualHeight
            // 
            this.cbxIsEqualHeight.AutoSize = true;
            this.cbxIsEqualHeight.Location = new System.Drawing.Point(4, 34);
            this.cbxIsEqualHeight.Name = "cbxIsEqualHeight";
            this.cbxIsEqualHeight.Size = new System.Drawing.Size(72, 16);
            this.cbxIsEqualHeight.TabIndex = 59;
            this.cbxIsEqualHeight.Text = "是否等高";
            this.cbxIsEqualHeight.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 231);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 52;
            this.label4.Text = "索引：";
            // 
            // txtLocation
            // 
            this.txtLocation.Location = new System.Drawing.Point(38, 227);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(41, 21);
            this.txtLocation.TabIndex = 53;
            this.txtLocation.Text = "0";
            // 
            // btnLocation
            // 
            this.btnLocation.Location = new System.Drawing.Point(86, 225);
            this.btnLocation.Name = "btnLocation";
            this.btnLocation.Size = new System.Drawing.Size(75, 23);
            this.btnLocation.TabIndex = 62;
            this.btnLocation.Text = "定位";
            this.btnLocation.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 426);
            this.Controls.Add(this.btnLocation);
            this.Controls.Add(this.btnGetSelected);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.cbxIsEqualHeight);
            this.Controls.Add(this.cbxIsGroup);
            this.Controls.Add(this.cbxShowMore);
            this.Controls.Add(this.btnAddByDt);
            this.Controls.Add(this.btnInsert);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.panelEx1);
            this.Controls.Add(this.txtLocation);
            this.Controls.Add(this.txtAddRowCount);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtInsertRowIndex);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtRowCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnInit);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtRowCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnInit;
        private MyPanelList panelEx1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.Button btnAddByDt;
        private System.Windows.Forms.CheckBox cbxShowMore;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnGetSelected;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtInsertRowIndex;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAddRowCount;
        private System.Windows.Forms.CheckBox cbxIsGroup;
        private System.Windows.Forms.CheckBox cbxIsEqualHeight;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.Button btnLocation;
    }
}

