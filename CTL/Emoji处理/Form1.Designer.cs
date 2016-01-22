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
            this.newRichTextBox1 = new Emoji处理.NewRichTextBox();
            this.SuspendLayout();
            // 
            // newRichTextBox1
            // 
            this.newRichTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newRichTextBox1.Location = new System.Drawing.Point(0, 0);
            this.newRichTextBox1.Name = "newRichTextBox1";
            this.newRichTextBox1.Size = new System.Drawing.Size(284, 262);
            this.newRichTextBox1.TabIndex = 0;
            this.newRichTextBox1.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.newRichTextBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private NewRichTextBox newRichTextBox1;

    }
}

