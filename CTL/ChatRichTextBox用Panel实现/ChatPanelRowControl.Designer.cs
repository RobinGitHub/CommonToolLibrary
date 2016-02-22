namespace ChatRichTextBox用Panel实现
{
    partial class ChatPanelRowControl
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblDate = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pnlContant = new System.Windows.Forms.Panel();
            this.picArrow = new System.Windows.Forms.PictureBox();
            this.picUser = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picArrow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUser)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDate
            // 
            this.lblDate.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.lblDate.Location = new System.Drawing.Point(0, 0);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(360, 19);
            this.lblDate.TabIndex = 0;
            this.lblDate.Text = "2016-2-22";
            this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblUserName
            // 
            this.lblUserName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblUserName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.lblUserName.Location = new System.Drawing.Point(0, 0);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(305, 19);
            this.lblUserName.TabIndex = 2;
            this.lblUserName.Text = "用户名";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.picArrow);
            this.panel1.Controls.Add(this.picUser);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(55, 97);
            this.panel1.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pnlContant);
            this.panel2.Controls.Add(this.lblUserName);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(55, 19);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(305, 97);
            this.panel2.TabIndex = 4;
            // 
            // pnlContant
            // 
            this.pnlContant.BackColor = System.Drawing.Color.Transparent;
            this.pnlContant.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContant.Location = new System.Drawing.Point(0, 19);
            this.pnlContant.Name = "pnlContant";
            this.pnlContant.Size = new System.Drawing.Size(305, 78);
            this.pnlContant.TabIndex = 3;
            // 
            // picArrow
            // 
            this.picArrow.Image = global::ChatRichTextBox用Panel实现.Properties.Resources.MsgLeftArrowBg;
            this.picArrow.Location = new System.Drawing.Point(42, 22);
            this.picArrow.Name = "picArrow";
            this.picArrow.Size = new System.Drawing.Size(13, 21);
            this.picArrow.TabIndex = 2;
            this.picArrow.TabStop = false;
            // 
            // picUser
            // 
            this.picUser.Location = new System.Drawing.Point(3, 15);
            this.picUser.Name = "picUser";
            this.picUser.Size = new System.Drawing.Size(36, 36);
            this.picUser.TabIndex = 1;
            this.picUser.TabStop = false;
            // 
            // ChatPanelRowControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblDate);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximumSize = new System.Drawing.Size(360, 768);
            this.Name = "ChatPanelRowControl";
            this.Size = new System.Drawing.Size(360, 116);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picArrow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUser)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.PictureBox picUser;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel pnlContant;
        private System.Windows.Forms.PictureBox picArrow;
    }
}
