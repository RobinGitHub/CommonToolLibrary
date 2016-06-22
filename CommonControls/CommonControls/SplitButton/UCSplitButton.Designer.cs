namespace CommonControls
{
    partial class UCSplitButton
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCSplitButton));
            this.pnlRight = new System.Windows.Forms.Panel();
            this.btnArr = new System.Windows.Forms.Button();
            this.pnlLine = new System.Windows.Forms.Panel();
            this.btn = new System.Windows.Forms.Button();
            this.cmsDetails = new CommonControls.ContextMenuStripEx();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlRight.SuspendLayout();
            this.cmsDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.btnArr);
            this.pnlRight.Controls.Add(this.pnlLine);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.Location = new System.Drawing.Point(65, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(25, 21);
            this.pnlRight.TabIndex = 0;
            // 
            // btnArr
            // 
            this.btnArr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnArr.FlatAppearance.BorderSize = 0;
            this.btnArr.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArr.Image = ((System.Drawing.Image)(resources.GetObject("btnArr.Image")));
            this.btnArr.Location = new System.Drawing.Point(1, 0);
            this.btnArr.Name = "btnArr";
            this.btnArr.Size = new System.Drawing.Size(24, 21);
            this.btnArr.TabIndex = 1;
            this.btnArr.UseVisualStyleBackColor = true;
            // 
            // pnlLine
            // 
            this.pnlLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.pnlLine.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLine.Location = new System.Drawing.Point(0, 0);
            this.pnlLine.Name = "pnlLine";
            this.pnlLine.Size = new System.Drawing.Size(1, 21);
            this.pnlLine.TabIndex = 0;
            // 
            // btn
            // 
            this.btn.AutoEllipsis = true;
            this.btn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn.FlatAppearance.BorderSize = 0;
            this.btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn.Location = new System.Drawing.Point(0, 0);
            this.btn.Name = "btn";
            this.btn.Size = new System.Drawing.Size(65, 21);
            this.btn.TabIndex = 1;
            this.btn.Text = "button1";
            this.btn.UseVisualStyleBackColor = true;
            // 
            // cmsDetails
            // 
            this.cmsDetails.AutoSize = false;
            this.cmsDetails.BackColor = System.Drawing.Color.White;
            this.cmsDetails.DropShadowEnabled = false;
            this.cmsDetails.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.cmsDetails.Name = "cmsDetails";
            this.cmsDetails.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.cmsDetails.ShowImageMargin = false;
            this.cmsDetails.Size = new System.Drawing.Size(59, 48);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(93, 22);
            this.toolStripMenuItem1.Text = "111111";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(93, 22);
            this.toolStripMenuItem2.Text = "2";
            // 
            // UCSplitButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.btn);
            this.Controls.Add(this.pnlRight);
            this.Name = "UCSplitButton";
            this.Size = new System.Drawing.Size(90, 21);
            this.pnlRight.ResumeLayout(false);
            this.cmsDetails.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Panel pnlLine;
        private System.Windows.Forms.Button btn;
        private System.Windows.Forms.Button btnArr;
        private ContextMenuStripEx cmsDetails;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
    }
}
