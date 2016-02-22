namespace ChatRichTextBox用Panel实现
{
    partial class ChatPanelLoadMoreRowControl
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
            this.llblLoadMore = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // llblLoadMore
            // 
            this.llblLoadMore.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(112)))), ((int)(((byte)(235)))));
            this.llblLoadMore.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.llblLoadMore.AutoSize = true;
            this.llblLoadMore.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.llblLoadMore.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(210)))), ((int)(((byte)(241)))));
            this.llblLoadMore.Location = new System.Drawing.Point(69, 0);
            this.llblLoadMore.Name = "llblLoadMore";
            this.llblLoadMore.Size = new System.Drawing.Size(80, 17);
            this.llblLoadMore.TabIndex = 0;
            this.llblLoadMore.TabStop = true;
            this.llblLoadMore.Text = "查看更多消息";
            this.llblLoadMore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ChatPanelLoadMoreRowControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.llblLoadMore);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ChatPanelLoadMoreRowControl";
            this.Size = new System.Drawing.Size(227, 20);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel llblLoadMore;
    }
}
