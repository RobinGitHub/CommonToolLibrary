namespace 自定义Panel列表
{
    partial class MyPanelList
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
            this.pnlContent = new 自定义Panel列表.PanelMultiplex();
            this.myVScrollBar1 = new 自定义Panel列表.MyVScrollBar();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.pnlContent.DisplayRectangleHeight = 0;
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.LargeChange = 0;
            this.pnlContent.Location = new System.Drawing.Point(0, 0);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.RowHeight = 0;
            this.pnlContent.Size = new System.Drawing.Size(292, 200);
            this.pnlContent.SmallChange = 0;
            this.pnlContent.TabIndex = 1;
            this.pnlContent.VScrollValue = 0;
            // 
            // myVScrollBar1
            // 
            this.myVScrollBar1.BindControl = null;
            this.myVScrollBar1.ChannelColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.myVScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.myVScrollBar1.Location = new System.Drawing.Point(292, 0);
            this.myVScrollBar1.MinimumSize = new System.Drawing.Size(8, 15);
            this.myVScrollBar1.Name = "myVScrollBar1";
            this.myVScrollBar1.Size = new System.Drawing.Size(8, 200);
            this.myVScrollBar1.TabIndex = 0;
            this.myVScrollBar1.ThumbColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(131)))), ((int)(((byte)(135)))));
            this.myVScrollBar1.Value = 0;
            this.myVScrollBar1.Visible = false;
            // 
            // MyPanelList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.myVScrollBar1);
            this.Name = "MyPanelList";
            this.Size = new System.Drawing.Size(300, 200);
            this.ResumeLayout(false);

        }

        #endregion

        private MyVScrollBar myVScrollBar1;
        private PanelMultiplex pnlContent;
    }
}
