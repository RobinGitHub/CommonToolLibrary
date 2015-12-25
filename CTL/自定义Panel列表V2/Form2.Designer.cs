namespace 自定义Panel列表V2
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.myVScrollBar1 = new 自定义Panel列表V2.MyVScrollBar();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(397, 396);
            this.treeView1.TabIndex = 0;
            // 
            // myVScrollBar1
            // 
            this.myVScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.myVScrollBar1.BindControl = null;
            this.myVScrollBar1.ChannelColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.myVScrollBar1.Location = new System.Drawing.Point(565, 0);
            this.myVScrollBar1.MinimumSize = new System.Drawing.Size(8, 15);
            this.myVScrollBar1.Name = "myVScrollBar1";
            this.myVScrollBar1.Size = new System.Drawing.Size(8, 396);
            this.myVScrollBar1.TabIndex = 1;
            this.myVScrollBar1.ThumbColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(131)))), ((int)(((byte)(135)))));
            this.myVScrollBar1.Value = 0;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 396);
            this.Controls.Add(this.myVScrollBar1);
            this.Controls.Add(this.treeView1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private MyVScrollBar myVScrollBar1;
    }
}