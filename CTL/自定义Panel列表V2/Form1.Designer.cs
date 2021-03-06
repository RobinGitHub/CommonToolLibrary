﻿namespace 自定义Panel列表V2
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
            this.btnLocation = new System.Windows.Forms.Button();
            this.btnGetSelected = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.cbxIsEqualHeight = new System.Windows.Forms.CheckBox();
            this.cbxIsGroup = new System.Windows.Forms.CheckBox();
            this.cbxShowMore = new System.Windows.Forms.CheckBox();
            this.btnAddByDt = new System.Windows.Forms.Button();
            this.btnInsert = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.txtAddRowCount = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtInsertRowIndex = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRowCount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnInit = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.btnGetTotal = new System.Windows.Forms.Button();
            this.btnInit1 = new System.Windows.Forms.Button();
            this.cbxAsc = new System.Windows.Forms.CheckBox();
            this.cbxGroupIsTop = new System.Windows.Forms.CheckBox();
            this.cbxShowGroupTotal = new System.Windows.Forms.CheckBox();
            this.txtGroupValue = new System.Windows.Forms.TextBox();
            this.btnAddByDt1 = new System.Windows.Forms.Button();
            this.txtGroupDispalyText = new System.Windows.Forms.TextBox();
            this.txtGroupValueIndex = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panelEx1 = new 自定义Panel列表V2.DataPanelView();
            this.SuspendLayout();
            // 
            // btnLocation
            // 
            this.btnLocation.Location = new System.Drawing.Point(94, 290);
            this.btnLocation.Name = "btnLocation";
            this.btnLocation.Size = new System.Drawing.Size(75, 23);
            this.btnLocation.TabIndex = 83;
            this.btnLocation.Text = "定位";
            this.btnLocation.UseVisualStyleBackColor = true;
            // 
            // btnGetSelected
            // 
            this.btnGetSelected.Location = new System.Drawing.Point(14, 150);
            this.btnGetSelected.Name = "btnGetSelected";
            this.btnGetSelected.Size = new System.Drawing.Size(75, 23);
            this.btnGetSelected.TabIndex = 82;
            this.btnGetSelected.Text = "获取选中项";
            this.btnGetSelected.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(13, 122);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(76, 23);
            this.btnClear.TabIndex = 81;
            this.btnClear.Text = "清空";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // cbxIsEqualHeight
            // 
            this.cbxIsEqualHeight.AutoSize = true;
            this.cbxIsEqualHeight.Location = new System.Drawing.Point(12, 34);
            this.cbxIsEqualHeight.Name = "cbxIsEqualHeight";
            this.cbxIsEqualHeight.Size = new System.Drawing.Size(72, 16);
            this.cbxIsEqualHeight.TabIndex = 80;
            this.cbxIsEqualHeight.Text = "是否等高";
            this.cbxIsEqualHeight.UseVisualStyleBackColor = true;
            // 
            // cbxIsGroup
            // 
            this.cbxIsGroup.AutoSize = true;
            this.cbxIsGroup.Location = new System.Drawing.Point(12, 12);
            this.cbxIsGroup.Name = "cbxIsGroup";
            this.cbxIsGroup.Size = new System.Drawing.Size(72, 16);
            this.cbxIsGroup.TabIndex = 79;
            this.cbxIsGroup.Text = "是否分组";
            this.cbxIsGroup.UseVisualStyleBackColor = true;
            // 
            // cbxShowMore
            // 
            this.cbxShowMore.AutoSize = true;
            this.cbxShowMore.Location = new System.Drawing.Point(97, 12);
            this.cbxShowMore.Name = "cbxShowMore";
            this.cbxShowMore.Size = new System.Drawing.Size(72, 16);
            this.cbxShowMore.TabIndex = 78;
            this.cbxShowMore.Text = "显示更多";
            this.cbxShowMore.UseVisualStyleBackColor = true;
            // 
            // btnAddByDt
            // 
            this.btnAddByDt.Location = new System.Drawing.Point(94, 233);
            this.btnAddByDt.Name = "btnAddByDt";
            this.btnAddByDt.Size = new System.Drawing.Size(75, 23);
            this.btnAddByDt.TabIndex = 77;
            this.btnAddByDt.Text = "批量增加";
            this.btnAddByDt.UseVisualStyleBackColor = true;
            // 
            // btnInsert
            // 
            this.btnInsert.Location = new System.Drawing.Point(94, 205);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(75, 23);
            this.btnInsert.TabIndex = 76;
            this.btnInsert.Text = "插入单个";
            this.btnInsert.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBox1.Location = new System.Drawing.Point(13, 319);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(156, 219);
            this.richTextBox1.TabIndex = 75;
            this.richTextBox1.Text = "";
            // 
            // txtLocation
            // 
            this.txtLocation.Location = new System.Drawing.Point(46, 292);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(41, 21);
            this.txtLocation.TabIndex = 74;
            this.txtLocation.Text = "0";
            // 
            // txtAddRowCount
            // 
            this.txtAddRowCount.Location = new System.Drawing.Point(46, 234);
            this.txtAddRowCount.Name = "txtAddRowCount";
            this.txtAddRowCount.Size = new System.Drawing.Size(41, 21);
            this.txtAddRowCount.TabIndex = 73;
            this.txtAddRowCount.Text = "20";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 296);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 70;
            this.label4.Text = "索引：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 238);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 69;
            this.label3.Text = "行：";
            // 
            // txtInsertRowIndex
            // 
            this.txtInsertRowIndex.Location = new System.Drawing.Point(46, 206);
            this.txtInsertRowIndex.Name = "txtInsertRowIndex";
            this.txtInsertRowIndex.Size = new System.Drawing.Size(41, 21);
            this.txtInsertRowIndex.TabIndex = 72;
            this.txtInsertRowIndex.Text = "20";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 210);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 68;
            this.label2.Text = "行：";
            // 
            // txtRowCount
            // 
            this.txtRowCount.Location = new System.Drawing.Point(47, 179);
            this.txtRowCount.Name = "txtRowCount";
            this.txtRowCount.Size = new System.Drawing.Size(41, 21);
            this.txtRowCount.TabIndex = 71;
            this.txtRowCount.Text = "20";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 183);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 67;
            this.label1.Text = "行：";
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(12, 261);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.TabIndex = 66;
            this.btnDel.Text = "删除";
            this.btnDel.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(94, 261);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 65;
            this.btnUpdate.Text = "修改";
            this.btnUpdate.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(94, 150);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 64;
            this.btnAdd.Text = "增加单个";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnInit
            // 
            this.btnInit.Location = new System.Drawing.Point(94, 178);
            this.btnInit.Name = "btnInit";
            this.btnInit.Size = new System.Drawing.Size(75, 23);
            this.btnInit.TabIndex = 63;
            this.btnInit.Text = "绑定数据DataSource";
            this.btnInit.UseVisualStyleBackColor = true;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(12, 56);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(157, 21);
            this.dateTimePicker1.TabIndex = 86;
            // 
            // btnGetTotal
            // 
            this.btnGetTotal.Location = new System.Drawing.Point(94, 122);
            this.btnGetTotal.Name = "btnGetTotal";
            this.btnGetTotal.Size = new System.Drawing.Size(76, 23);
            this.btnGetTotal.TabIndex = 81;
            this.btnGetTotal.Text = "总行数";
            this.btnGetTotal.UseVisualStyleBackColor = true;
            // 
            // btnInit1
            // 
            this.btnInit1.Location = new System.Drawing.Point(175, 179);
            this.btnInit1.Name = "btnInit1";
            this.btnInit1.Size = new System.Drawing.Size(75, 23);
            this.btnInit1.TabIndex = 87;
            this.btnInit1.Text = "绑定数据1";
            this.btnInit1.UseVisualStyleBackColor = true;
            // 
            // cbxAsc
            // 
            this.cbxAsc.AutoSize = true;
            this.cbxAsc.Checked = true;
            this.cbxAsc.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxAsc.Location = new System.Drawing.Point(97, 34);
            this.cbxAsc.Name = "cbxAsc";
            this.cbxAsc.Size = new System.Drawing.Size(48, 16);
            this.cbxAsc.TabIndex = 88;
            this.cbxAsc.Text = "正序";
            this.cbxAsc.UseVisualStyleBackColor = true;
            // 
            // cbxGroupIsTop
            // 
            this.cbxGroupIsTop.AutoSize = true;
            this.cbxGroupIsTop.Location = new System.Drawing.Point(188, 12);
            this.cbxGroupIsTop.Name = "cbxGroupIsTop";
            this.cbxGroupIsTop.Size = new System.Drawing.Size(72, 16);
            this.cbxGroupIsTop.TabIndex = 89;
            this.cbxGroupIsTop.Text = "组在上方";
            this.cbxGroupIsTop.UseVisualStyleBackColor = true;
            // 
            // cbxShowGroupTotal
            // 
            this.cbxShowGroupTotal.AutoSize = true;
            this.cbxShowGroupTotal.Checked = true;
            this.cbxShowGroupTotal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxShowGroupTotal.Location = new System.Drawing.Point(188, 34);
            this.cbxShowGroupTotal.Name = "cbxShowGroupTotal";
            this.cbxShowGroupTotal.Size = new System.Drawing.Size(84, 16);
            this.cbxShowGroupTotal.TabIndex = 90;
            this.cbxShowGroupTotal.Text = "显示组统计";
            this.cbxShowGroupTotal.UseVisualStyleBackColor = true;
            // 
            // txtGroupValue
            // 
            this.txtGroupValue.Location = new System.Drawing.Point(12, 97);
            this.txtGroupValue.Name = "txtGroupValue";
            this.txtGroupValue.Size = new System.Drawing.Size(85, 21);
            this.txtGroupValue.TabIndex = 91;
            // 
            // btnAddByDt1
            // 
            this.btnAddByDt1.Location = new System.Drawing.Point(175, 233);
            this.btnAddByDt1.Name = "btnAddByDt1";
            this.btnAddByDt1.Size = new System.Drawing.Size(75, 23);
            this.btnAddByDt1.TabIndex = 92;
            this.btnAddByDt1.Text = "批量增加1";
            this.btnAddByDt1.UseVisualStyleBackColor = true;
            // 
            // txtGroupDispalyText
            // 
            this.txtGroupDispalyText.Location = new System.Drawing.Point(103, 97);
            this.txtGroupDispalyText.Name = "txtGroupDispalyText";
            this.txtGroupDispalyText.Size = new System.Drawing.Size(66, 21);
            this.txtGroupDispalyText.TabIndex = 93;
            // 
            // txtGroupValueIndex
            // 
            this.txtGroupValueIndex.Location = new System.Drawing.Point(175, 97);
            this.txtGroupValueIndex.Name = "txtGroupValueIndex";
            this.txtGroupValueIndex.Size = new System.Drawing.Size(66, 21);
            this.txtGroupValueIndex.TabIndex = 94;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 95;
            this.label5.Text = "组内容";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(101, 82);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 95;
            this.label6.Text = "组标题";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(173, 82);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 95;
            this.label7.Text = "组内容索引";
            // 
            // panelEx1
            // 
            this.panelEx1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelEx1.BackColor = System.Drawing.Color.White;
            this.panelEx1.DefaultColor = System.Drawing.Color.White;
            this.panelEx1.FirstDisplayedScrollingRowIndex = 0;
            this.panelEx1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panelEx1.GroupTitleLeft = 20;
            this.panelEx1.GroupTitleTextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.panelEx1.IsShowGroupTotal = true;
            this.panelEx1.Location = new System.Drawing.Point(278, 0);
            this.panelEx1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelEx1.MinRowHeight = 60;
            this.panelEx1.MouseEnterColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(240)))), ((int)(((byte)(193)))));
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(235)))), ((int)(((byte)(166)))));
            this.panelEx1.Size = new System.Drawing.Size(466, 538);
            this.panelEx1.TabIndex = 84;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 538);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtGroupValueIndex);
            this.Controls.Add(this.txtGroupDispalyText);
            this.Controls.Add(this.btnAddByDt1);
            this.Controls.Add(this.txtGroupValue);
            this.Controls.Add(this.cbxShowGroupTotal);
            this.Controls.Add(this.cbxGroupIsTop);
            this.Controls.Add(this.cbxAsc);
            this.Controls.Add(this.btnInit1);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.panelEx1);
            this.Controls.Add(this.btnLocation);
            this.Controls.Add(this.btnGetSelected);
            this.Controls.Add(this.btnGetTotal);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.cbxIsEqualHeight);
            this.Controls.Add(this.cbxIsGroup);
            this.Controls.Add(this.cbxShowMore);
            this.Controls.Add(this.btnAddByDt);
            this.Controls.Add(this.btnInsert);
            this.Controls.Add(this.richTextBox1);
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

        private System.Windows.Forms.Button btnLocation;
        private System.Windows.Forms.Button btnGetSelected;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.CheckBox cbxIsEqualHeight;
        private System.Windows.Forms.CheckBox cbxIsGroup;
        private System.Windows.Forms.CheckBox cbxShowMore;
        private System.Windows.Forms.Button btnAddByDt;
        private System.Windows.Forms.Button btnInsert;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.TextBox txtAddRowCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtInsertRowIndex;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRowCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnInit;
        private DataPanelView panelEx1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button btnGetTotal;
        private System.Windows.Forms.Button btnInit1;
        private System.Windows.Forms.CheckBox cbxAsc;
        private System.Windows.Forms.CheckBox cbxGroupIsTop;
        private System.Windows.Forms.CheckBox cbxShowGroupTotal;
        private System.Windows.Forms.TextBox txtGroupValue;
        private System.Windows.Forms.Button btnAddByDt1;
        private System.Windows.Forms.TextBox txtGroupDispalyText;
        private System.Windows.Forms.TextBox txtGroupValueIndex;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
    }
}

