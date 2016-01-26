namespace Emoji处理
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
            this.newRichTextBox1 = new System.Windows.Forms.RichTextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.newRichTextBox2 = new Emoji处理.NewRichTextBox();
            this.SuspendLayout();
            // 
            // newRichTextBox1
            // 
            this.newRichTextBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.newRichTextBox1.Location = new System.Drawing.Point(0, 0);
            this.newRichTextBox1.Name = "newRichTextBox1";
            this.newRichTextBox1.Size = new System.Drawing.Size(549, 137);
            this.newRichTextBox1.TabIndex = 0;
            this.newRichTextBox1.Text = "؏؏ᖗ¤̴̶̷̤́‧̫̮¤̴̶̷̤̀)ᖘ؏؏ 小公举";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(5, 370);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(242, 21);
            this.textBox1.TabIndex = 1;
            // 
            // newRichTextBox2
            // 
            this.newRichTextBox2.Location = new System.Drawing.Point(5, 155);
            this.newRichTextBox2.Name = "newRichTextBox2";
            this.newRichTextBox2.Size = new System.Drawing.Size(532, 96);
            this.newRichTextBox2.TabIndex = 2;
            this.newRichTextBox2.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 396);
            this.Controls.Add(this.newRichTextBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.newRichTextBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox newRichTextBox1;
        private System.Windows.Forms.TextBox textBox1;
        private NewRichTextBox newRichTextBox2;


    }
}

