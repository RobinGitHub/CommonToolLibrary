namespace 自定义Panel列表V1
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnComment = new System.Windows.Forms.Button();
            this.lblDateAndUser = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.picUser = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlReplyContent = new System.Windows.Forms.Panel();
            this.btnReply = new System.Windows.Forms.Button();
            this.txtReplyContent = new System.Windows.Forms.TextBox();
            this.pnlReplyDetail = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picUser)).BeginInit();
            this.pnlReplyContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnComment);
            this.panel1.Controls.Add(this.lblDateAndUser);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(57, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(269, 30);
            this.panel1.TabIndex = 1;
            // 
            // btnComment
            // 
            this.btnComment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnComment.Location = new System.Drawing.Point(227, 3);
            this.btnComment.Name = "btnComment";
            this.btnComment.Size = new System.Drawing.Size(39, 23);
            this.btnComment.TabIndex = 1;
            this.btnComment.Text = "评论";
            this.btnComment.UseVisualStyleBackColor = true;
            this.btnComment.Click += new System.EventHandler(this.btnComment_Click);
            // 
            // lblDateAndUser
            // 
            this.lblDateAndUser.AutoEllipsis = true;
            this.lblDateAndUser.AutoSize = true;
            this.lblDateAndUser.Location = new System.Drawing.Point(6, 7);
            this.lblDateAndUser.MaximumSize = new System.Drawing.Size(180, 12);
            this.lblDateAndUser.Name = "lblDateAndUser";
            this.lblDateAndUser.Size = new System.Drawing.Size(167, 12);
            this.lblDateAndUser.TabIndex = 0;
            this.lblDateAndUser.Text = "2015-11-17 09:10 超级管理员";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.picUser);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(57, 119);
            this.panel3.TabIndex = 4;
            // 
            // picUser
            // 
            this.picUser.Image = ((System.Drawing.Image)(resources.GetObject("picUser.Image")));
            this.picUser.Location = new System.Drawing.Point(4, 7);
            this.picUser.Name = "picUser";
            this.picUser.Size = new System.Drawing.Size(50, 50);
            this.picUser.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picUser.TabIndex = 0;
            this.picUser.TabStop = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoEllipsis = true;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Location = new System.Drawing.Point(57, 30);
            this.lblTitle.MaximumSize = new System.Drawing.Size(0, 200);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(269, 27);
            this.lblTitle.TabIndex = 5;
            this.lblTitle.Text = "标题";
            // 
            // pnlReplyContent
            // 
            this.pnlReplyContent.Controls.Add(this.btnReply);
            this.pnlReplyContent.Controls.Add(this.txtReplyContent);
            this.pnlReplyContent.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlReplyContent.Location = new System.Drawing.Point(57, 57);
            this.pnlReplyContent.Name = "pnlReplyContent";
            this.pnlReplyContent.Size = new System.Drawing.Size(269, 50);
            this.pnlReplyContent.TabIndex = 6;
            this.pnlReplyContent.Visible = false;
            // 
            // btnReply
            // 
            this.btnReply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReply.Location = new System.Drawing.Point(227, 4);
            this.btnReply.Name = "btnReply";
            this.btnReply.Size = new System.Drawing.Size(39, 40);
            this.btnReply.TabIndex = 2;
            this.btnReply.Text = "回复";
            this.btnReply.UseVisualStyleBackColor = true;
            this.btnReply.Click += new System.EventHandler(this.btnReply_Click);
            // 
            // txtReplyContent
            // 
            this.txtReplyContent.Location = new System.Drawing.Point(4, 4);
            this.txtReplyContent.Multiline = true;
            this.txtReplyContent.Name = "txtReplyContent";
            this.txtReplyContent.Size = new System.Drawing.Size(222, 40);
            this.txtReplyContent.TabIndex = 0;
            // 
            // pnlReplyDetail
            // 
            this.pnlReplyDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlReplyDetail.Location = new System.Drawing.Point(57, 107);
            this.pnlReplyDetail.Name = "pnlReplyDetail";
            this.pnlReplyDetail.Size = new System.Drawing.Size(269, 12);
            this.pnlReplyDetail.TabIndex = 7;
            this.pnlReplyDetail.Visible = false;
            // 
            // ReplyUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.pnlReplyDetail);
            this.Controls.Add(this.pnlReplyContent);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Name = "ReplyUserControl";
            this.Size = new System.Drawing.Size(326, 120);
            this.Load += new System.EventHandler(this.ReplyUserControl_Load);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.lblTitle, 0);
            this.Controls.SetChildIndex(this.pnlReplyContent, 0);
            this.Controls.SetChildIndex(this.pnlReplyDetail, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picUser)).EndInit();
            this.pnlReplyContent.ResumeLayout(false);
            this.pnlReplyContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnComment;
        private System.Windows.Forms.Label lblDateAndUser;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox picUser;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlReplyContent;
        private System.Windows.Forms.Button btnReply;
        private System.Windows.Forms.TextBox txtReplyContent;
        private System.Windows.Forms.Panel pnlReplyDetail;
    }
}
