﻿namespace 自定义Panel列表V1
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
            this.lblDateAndUser = new System.Windows.Forms.Label();
            this.btnComment = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlReplyContent = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.pnlReplyDetail = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.picUser)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlReplyContent.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // picUser
            // 
            this.picUser.Image = ((System.Drawing.Image)(resources.GetObject("picUser.Image")));
            this.picUser.Location = new System.Drawing.Point(3, 8);
            this.picUser.Name = "picUser";
            this.picUser.Size = new System.Drawing.Size(50, 50);
            this.picUser.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picUser.TabIndex = 8;
            this.picUser.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.pnlReplyContent);
            this.panel1.Location = new System.Drawing.Point(60, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(279, 86);
            this.panel1.TabIndex = 9;
            // 
            // lblDateAndUser
            // 
            this.lblDateAndUser.AutoSize = true;
            this.lblDateAndUser.Location = new System.Drawing.Point(4, 8);
            this.lblDateAndUser.Name = "lblDateAndUser";
            this.lblDateAndUser.Size = new System.Drawing.Size(167, 12);
            this.lblDateAndUser.TabIndex = 0;
            this.lblDateAndUser.Text = "2015-11-17 14:50 超级管理员";
            // 
            // btnComment
            // 
            this.btnComment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnComment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnComment.Location = new System.Drawing.Point(234, 3);
            this.btnComment.Name = "btnComment";
            this.btnComment.Size = new System.Drawing.Size(42, 23);
            this.btnComment.TabIndex = 1;
            this.btnComment.Text = "评论";
            this.btnComment.UseVisualStyleBackColor = true;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Location = new System.Drawing.Point(0, 30);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(279, 17);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "label1";
            // 
            // pnlReplyContent
            // 
            this.pnlReplyContent.Controls.Add(this.button1);
            this.pnlReplyContent.Controls.Add(this.textBox1);
            this.pnlReplyContent.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlReplyContent.Location = new System.Drawing.Point(0, 47);
            this.pnlReplyContent.Name = "pnlReplyContent";
            this.pnlReplyContent.Size = new System.Drawing.Size(279, 39);
            this.pnlReplyContent.TabIndex = 3;
            this.pnlReplyContent.Visible = false;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(4, 4);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(228, 32);
            this.textBox1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(237, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(39, 32);
            this.button1.TabIndex = 1;
            this.button1.Text = "回复";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // pnlReplyDetail
            // 
            this.pnlReplyDetail.Location = new System.Drawing.Point(60, 86);
            this.pnlReplyDetail.Name = "pnlReplyDetail";
            this.pnlReplyDetail.Size = new System.Drawing.Size(279, 31);
            this.pnlReplyDetail.TabIndex = 10;
            this.pnlReplyDetail.Visible = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnComment);
            this.panel2.Controls.Add(this.lblDateAndUser);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(279, 30);
            this.panel2.TabIndex = 4;
            // 
            // ReplyUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlReplyDetail);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.picUser);
            this.Name = "ReplyUserControl";
            this.Size = new System.Drawing.Size(342, 63);
            this.Load += new System.EventHandler(this.ReplyUserControl_Load);
            this.Controls.SetChildIndex(this.picUser, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.pnlReplyDetail, 0);
            ((System.ComponentModel.ISupportInitialize)(this.picUser)).EndInit();
            this.panel1.ResumeLayout(false);
            this.pnlReplyContent.ResumeLayout(false);
            this.pnlReplyContent.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picUser;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnComment;
        private System.Windows.Forms.Label lblDateAndUser;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlReplyContent;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel pnlReplyDetail;
        private System.Windows.Forms.Panel panel2;
    }
}
