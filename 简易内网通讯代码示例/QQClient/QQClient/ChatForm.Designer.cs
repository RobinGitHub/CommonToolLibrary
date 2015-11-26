namespace QQClient
{
    partial class ChatForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSend = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.txbSend = new System.Windows.Forms.TextBox();
            this.richtxbTalkinfo = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(345, 266);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(51, 23);
            this.btnSend.TabIndex = 11;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(290, 266);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(49, 23);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txbSend
            // 
            this.txbSend.Location = new System.Drawing.Point(9, 185);
            this.txbSend.Margin = new System.Windows.Forms.Padding(0);
            this.txbSend.Multiline = true;
            this.txbSend.Name = "txbSend";
            this.txbSend.Size = new System.Drawing.Size(387, 68);
            this.txbSend.TabIndex = 9;
            // 
            // richtxbTalkinfo
            // 
            this.richtxbTalkinfo.Location = new System.Drawing.Point(9, 9);
            this.richtxbTalkinfo.Margin = new System.Windows.Forms.Padding(0);
            this.richtxbTalkinfo.MinimumSize = new System.Drawing.Size(318, 167);
            this.richtxbTalkinfo.Name = "richtxbTalkinfo";
            this.richtxbTalkinfo.Size = new System.Drawing.Size(387, 167);
            this.richtxbTalkinfo.TabIndex = 8;
            this.richtxbTalkinfo.Text = "";
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 300);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txbSend);
            this.Controls.Add(this.richtxbTalkinfo);
            this.Name = "ChatForm";
            this.Text = "聊天";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txbSend;
        private System.Windows.Forms.RichTextBox richtxbTalkinfo;
    }
}