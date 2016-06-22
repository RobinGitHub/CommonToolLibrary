namespace ComboBox自定义下拉项
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.exComboBox1 = new ExComboBox();
            this.customComboBox1 = new ComboBox自定义下拉项.CustomComboBox();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "123123"});
            this.comboBox1.Location = new System.Drawing.Point(13, 52);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 1;
            // 
            // exComboBox1
            // 
            this.exComboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exComboBox1.FormattingEnabled = true;
            this.exComboBox1.Location = new System.Drawing.Point(13, 104);
            this.exComboBox1.Name = "exComboBox1";
            this.exComboBox1.ReadOnly = true;
            this.exComboBox1.Size = new System.Drawing.Size(121, 20);
            this.exComboBox1.TabIndex = 2;
            // 
            // customComboBox1
            // 
            this.customComboBox1.AllowResizeDropDown = false;
            this.customComboBox1.ControlSize = new System.Drawing.Size(1, 1);
            this.customComboBox1.DropDownControl = null;
            this.customComboBox1.DropDownSizeMode = ComboBox自定义下拉项.CustomComboBox.SizeMode.UseControlSize;
            this.customComboBox1.DropSize = new System.Drawing.Size(121, 106);
            this.customComboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.customComboBox1.FormattingEnabled = true;
            this.customComboBox1.Location = new System.Drawing.Point(12, 12);
            this.customComboBox1.Name = "customComboBox1";
            this.customComboBox1.ReadOnly = true;
            this.customComboBox1.Size = new System.Drawing.Size(173, 20);
            this.customComboBox1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.exComboBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.customComboBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private CustomComboBox customComboBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        private ExComboBox exComboBox1;

    }
}

