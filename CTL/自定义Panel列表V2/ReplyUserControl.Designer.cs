namespace 自定义Panel列表V2
{
    partial class ReplyUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReplyUserControl));
            this.picUser = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlTitle = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblDateAndUser = new System.Windows.Forms.Label();
            this.btnComment = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlReplyContent = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.txtReplyContent = new System.Windows.Forms.TextBox();
            this.pnlReplyDetail = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pnlRightContent = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.picUser)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlReplyContent.SuspendLayout();
            this.panel3.SuspendLayout();
            this.pnlRightContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // picUser
            // 
            this.picUser.Image = ((System.Drawing.Image)(resources.GetObject("picUser.Image")));
            this.picUser.Location = new System.Drawing.Point(3, 10);
            this.picUser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.picUser.Name = "picUser";
            this.picUser.Size = new System.Drawing.Size(58, 71);
            this.picUser.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picUser.TabIndex = 8;
            this.picUser.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.pnlTitle);
            this.panel1.Controls.Add(this.pnlReplyContent);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(335, 96);
            this.panel1.TabIndex = 9;
            // 
            // pnlTitle
            // 
            this.pnlTitle.AutoSize = true;
            this.pnlTitle.Controls.Add(this.panel2);
            this.pnlTitle.Controls.Add(this.lblTitle);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitle.Location = new System.Drawing.Point(0, 0);
            this.pnlTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(335, 57);
            this.pnlTitle.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblDateAndUser);
            this.panel2.Controls.Add(this.btnComment);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(335, 40);
            this.panel2.TabIndex = 3;
            // 
            // lblDateAndUser
            // 
            this.lblDateAndUser.AutoSize = true;
            this.lblDateAndUser.Location = new System.Drawing.Point(3, 11);
            this.lblDateAndUser.Name = "lblDateAndUser";
            this.lblDateAndUser.Size = new System.Drawing.Size(173, 17);
            this.lblDateAndUser.TabIndex = 0;
            this.lblDateAndUser.Text = "2015-11-17 14:50 超级管理员";
            // 
            // btnComment
            // 
            this.btnComment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnComment.Location = new System.Drawing.Point(280, 4);
            this.btnComment.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnComment.Name = "btnComment";
            this.btnComment.Size = new System.Drawing.Size(49, 33);
            this.btnComment.TabIndex = 1;
            this.btnComment.Text = "评论";
            this.btnComment.UseVisualStyleBackColor = true;
            this.btnComment.Click += new System.EventHandler(this.btnComment_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoEllipsis = true;
            this.lblTitle.AutoSize = true;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblTitle.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTitle.Location = new System.Drawing.Point(0, 40);
            this.lblTitle.MaximumSize = new System.Drawing.Size(700, 71);
            this.lblTitle.MinimumSize = new System.Drawing.Size(334, 17);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(334, 17);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "标题";
            // 
            // pnlReplyContent
            // 
            this.pnlReplyContent.Controls.Add(this.button1);
            this.pnlReplyContent.Controls.Add(this.txtReplyContent);
            this.pnlReplyContent.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlReplyContent.Location = new System.Drawing.Point(0, 57);
            this.pnlReplyContent.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlReplyContent.Name = "pnlReplyContent";
            this.pnlReplyContent.Size = new System.Drawing.Size(335, 39);
            this.pnlReplyContent.TabIndex = 3;
            this.pnlReplyContent.Visible = false;
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(285, 2);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(45, 34);
            this.button1.TabIndex = 1;
            this.button1.Text = "回复";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnReply_Click);
            // 
            // txtReplyContent
            // 
            this.txtReplyContent.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtReplyContent.Location = new System.Drawing.Point(5, 2);
            this.txtReplyContent.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtReplyContent.Multiline = true;
            this.txtReplyContent.Name = "txtReplyContent";
            this.txtReplyContent.Size = new System.Drawing.Size(273, 34);
            this.txtReplyContent.TabIndex = 0;
            this.txtReplyContent.TextChanged += new System.EventHandler(this.txtReplyContent_TextChanged);
            // 
            // pnlReplyDetail
            // 
            this.pnlReplyDetail.AutoSize = true;
            this.pnlReplyDetail.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlReplyDetail.Location = new System.Drawing.Point(0, 131);
            this.pnlReplyDetail.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlReplyDetail.Name = "pnlReplyDetail";
            this.pnlReplyDetail.Size = new System.Drawing.Size(335, 0);
            this.pnlReplyDetail.TabIndex = 10;
            this.pnlReplyDetail.Visible = false;
            // 
            // panel3
            // 
            this.panel3.AutoSize = true;
            this.panel3.Controls.Add(this.picUser);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(64, 131);
            this.panel3.TabIndex = 11;
            // 
            // pnlRightContent
            // 
            this.pnlRightContent.AutoSize = true;
            this.pnlRightContent.Controls.Add(this.pnlReplyDetail);
            this.pnlRightContent.Controls.Add(this.panel1);
            this.pnlRightContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRightContent.Location = new System.Drawing.Point(64, 0);
            this.pnlRightContent.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlRightContent.Name = "pnlRightContent";
            this.pnlRightContent.Size = new System.Drawing.Size(335, 131);
            this.pnlRightContent.TabIndex = 12;
            // 
            // ReplyUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlRightContent);
            this.Controls.Add(this.panel3);
            this.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.Name = "ReplyUserControl";
            this.Size = new System.Drawing.Size(399, 132);
            this.Load += new System.EventHandler(this.ReplyUserControl_Load);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.pnlRightContent, 0);
            ((System.ComponentModel.ISupportInitialize)(this.picUser)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.pnlReplyContent.ResumeLayout(false);
            this.pnlReplyContent.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.pnlRightContent.ResumeLayout(false);
            this.pnlRightContent.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picUser;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnComment;
        private System.Windows.Forms.Label lblDateAndUser;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlReplyContent;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtReplyContent;
        private System.Windows.Forms.Panel pnlReplyDetail;
        private System.Windows.Forms.Panel pnlTitle;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel pnlRightContent;
        private System.Windows.Forms.Panel panel2;
    }
}
