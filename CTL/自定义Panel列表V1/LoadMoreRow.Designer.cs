﻿namespace 自定义Panel列表V1
{
    partial class LoadMoreRow
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
            this.btnLoadMore = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLoadMore
            // 
            this.btnLoadMore.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnLoadMore.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLoadMore.Location = new System.Drawing.Point(112, 3);
            this.btnLoadMore.Name = "btnLoadMore";
            this.btnLoadMore.Size = new System.Drawing.Size(75, 23);
            this.btnLoadMore.TabIndex = 2;
            this.btnLoadMore.Text = "加载更多";
            this.btnLoadMore.UseVisualStyleBackColor = true;
            this.btnLoadMore.Click += new System.EventHandler(this.btnLoadMore_Click);
            // 
            // LoadMoreRow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnLoadMore);
            this.Name = "LoadMoreRow";
            this.Size = new System.Drawing.Size(300, 30);
            this.Controls.SetChildIndex(this.btnLoadMore, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLoadMore;
    }
}
