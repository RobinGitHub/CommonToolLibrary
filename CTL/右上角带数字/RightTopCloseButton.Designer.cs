namespace 右上角带数字
{
    partial class RightTopCloseButton
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
            this.picTips = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // picTips
            // 
            this.picTips.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picTips.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.picTips.Location = new System.Drawing.Point(0, 0);
            this.picTips.Margin = new System.Windows.Forms.Padding(0);
            this.picTips.MinimumSize = new System.Drawing.Size(16, 16);
            this.picTips.Name = "picTips";
            this.picTips.Size = new System.Drawing.Size(64, 64);
            this.picTips.TabIndex = 2;
            // 
            // RightTopCloseButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.picTips);
            this.Name = "RightTopCloseButton";
            this.Size = new System.Drawing.Size(64, 64);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label picTips;
    }
}
