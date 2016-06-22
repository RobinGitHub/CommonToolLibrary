namespace RichTextBox消息处理
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.rtbReceive = new RichTextBox消息处理.ChatRichTextBox();
            this.rtbSend = new RichTextBox消息处理.ChatRichTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnGetRTF = new System.Windows.Forms.Button();
            this.btnFilePath = new System.Windows.Forms.Button();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rtbReceive);
            this.panel1.Controls.Add(this.rtbSend);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(585, 376);
            this.panel1.TabIndex = 1;
            // 
            // rtbReceive
            // 
            this.rtbReceive.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbReceive.Location = new System.Drawing.Point(0, 0);
            this.rtbReceive.Name = "rtbReceive";
            this.rtbReceive.Size = new System.Drawing.Size(585, 179);
            this.rtbReceive.TabIndex = 1;
            this.rtbReceive.Text = "";
            // 
            // rtbSend
            // 
            this.rtbSend.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbSend.AutoWordSelection = true;
            this.rtbSend.DetectUrls = false;
            this.rtbSend.EnableAutoDragDrop = true;
            this.rtbSend.HideSelection = false;
            this.rtbSend.Location = new System.Drawing.Point(0, 205);
            this.rtbSend.MaxLength = 500;
            this.rtbSend.Name = "rtbSend";
            this.rtbSend.Size = new System.Drawing.Size(585, 171);
            this.rtbSend.TabIndex = 0;
            this.rtbSend.Text = "";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnGetRTF);
            this.panel2.Controls.Add(this.btnFilePath);
            this.panel2.Controls.Add(this.txtFilePath);
            this.panel2.Controls.Add(this.btnSend);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 376);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(585, 41);
            this.panel2.TabIndex = 1;
            // 
            // btnGetRTF
            // 
            this.btnGetRTF.Location = new System.Drawing.Point(417, 7);
            this.btnGetRTF.Name = "btnGetRTF";
            this.btnGetRTF.Size = new System.Drawing.Size(75, 23);
            this.btnGetRTF.TabIndex = 3;
            this.btnGetRTF.Text = "GetRTF";
            this.btnGetRTF.UseVisualStyleBackColor = true;
            this.btnGetRTF.Click += new System.EventHandler(this.btnGetRTF_Click);
            // 
            // btnFilePath
            // 
            this.btnFilePath.Location = new System.Drawing.Point(199, 5);
            this.btnFilePath.Name = "btnFilePath";
            this.btnFilePath.Size = new System.Drawing.Size(32, 23);
            this.btnFilePath.TabIndex = 2;
            this.btnFilePath.Text = "...";
            this.btnFilePath.UseVisualStyleBackColor = true;
            this.btnFilePath.Click += new System.EventHandler(this.btnFilePath_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(13, 7);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(179, 21);
            this.txtFilePath.TabIndex = 1;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(498, 6);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 23);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 417);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSend;
        private ChatRichTextBox rtbReceive;
        private ChatRichTextBox rtbSend;
        private System.Windows.Forms.Button btnFilePath;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnGetRTF;
    }
}

