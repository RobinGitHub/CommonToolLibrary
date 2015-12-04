namespace 自定义TreeView仿VS解决方案效果
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.myVScrollBar1 = new 自定义TreeView仿VS解决方案效果.MyVScrollBar();
            this.treeViewEx1 = new 自定义TreeView仿VS解决方案效果.TreeViewEx();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(241, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(77, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(241, 56);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(77, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(324, 14);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(159, 143);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "";
            // 
            // myVScrollBar1
            // 
            this.myVScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.myVScrollBar1.BindControl = null;
            this.myVScrollBar1.ChannelColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
            this.myVScrollBar1.Location = new System.Drawing.Point(204, 0);
            this.myVScrollBar1.MinimumSize = new System.Drawing.Size(8, 15);
            this.myVScrollBar1.Name = "myVScrollBar1";
            this.myVScrollBar1.Size = new System.Drawing.Size(10, 394);
            this.myVScrollBar1.TabIndex = 1;
            this.myVScrollBar1.ThumbColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(131)))), ((int)(((byte)(135)))));
            this.myVScrollBar1.Value = 0;
            // 
            // treeViewEx1
            // 
            this.treeViewEx1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewEx1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeViewEx1.BottomLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(237)))), ((int)(((byte)(252)))));
            this.treeViewEx1.FullRowSelect = true;
            this.treeViewEx1.HideSelection = false;
            this.treeViewEx1.HorizontalScrollValue = 0;
            this.treeViewEx1.HotTracking = true;
            this.treeViewEx1.Location = new System.Drawing.Point(0, 0);
            this.treeViewEx1.MinusImage = ((System.Drawing.Image)(resources.GetObject("treeViewEx1.MinusImage")));
            this.treeViewEx1.MouseEnterColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(240)))), ((int)(((byte)(193)))));
            this.treeViewEx1.Name = "treeViewEx1";
            this.treeViewEx1.PlusImage = ((System.Drawing.Image)(resources.GetObject("treeViewEx1.PlusImage")));
            this.treeViewEx1.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(235)))), ((int)(((byte)(166)))));
            this.treeViewEx1.Size = new System.Drawing.Size(155, 394);
            this.treeViewEx1.TabIndex = 0;
            this.treeViewEx1.VerticalScrollValue = 0;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(241, 94);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(77, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 394);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.myVScrollBar1);
            this.Controls.Add(this.treeViewEx1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);

        }

        #endregion

        private TreeViewEx treeViewEx1;
        private MyVScrollBar myVScrollBar1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button3;
    }
}