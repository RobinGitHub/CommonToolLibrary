namespace 自定义Panel列表V1
{
    partial class MyPanelChild
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
            this.pnlSplitLine = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlSplitLine
            // 
            this.pnlSplitLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(233)))));
            this.pnlSplitLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSplitLine.Location = new System.Drawing.Point(0, 59);
            this.pnlSplitLine.Name = "pnlSplitLine";
            this.pnlSplitLine.Size = new System.Drawing.Size(300, 1);
            this.pnlSplitLine.TabIndex = 1;
            // 
            // MyPanelChild
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlSplitLine);
            this.Name = "MyPanelChild";
            this.Size = new System.Drawing.Size(300, 60);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSplitLine;
    }
}
