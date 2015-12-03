namespace 自定义TreeView仿VS解决方案效果
{
    partial class Form1
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.treeViewEx1 = new 自定义TreeView仿VS解决方案效果.TreeViewEx();
            this.treeViewMenu = new 自定义TreeView仿VS解决方案效果.TreeViewEx();
            this.SuspendLayout();
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(257, 12);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 1;
            this.btnLoad.Text = "加载";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(257, 54);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 3;
            this.btnUpdate.Text = "修改";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // treeViewEx1
            // 
            this.treeViewEx1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeViewEx1.BottomLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(237)))), ((int)(((byte)(252)))));
            this.treeViewEx1.FullRowSelect = true;
            this.treeViewEx1.HideSelection = false;
            this.treeViewEx1.HotTracking = true;
            this.treeViewEx1.ItemHeight = 40;
            this.treeViewEx1.Location = new System.Drawing.Point(0, 0);
            this.treeViewEx1.MinusImage = ((System.Drawing.Image)(resources.GetObject("treeViewEx1.MinusImage")));
            this.treeViewEx1.MouseEnterColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(240)))), ((int)(((byte)(193)))));
            this.treeViewEx1.Name = "treeViewEx1";
            this.treeViewEx1.PlusImage = ((System.Drawing.Image)(resources.GetObject("treeViewEx1.PlusImage")));
            this.treeViewEx1.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(235)))), ((int)(((byte)(166)))));
            this.treeViewEx1.ShowNodeToolTips = true;
            this.treeViewEx1.Size = new System.Drawing.Size(231, 336);
            this.treeViewEx1.TabIndex = 2;
            // 
            // treeViewMenu
            // 
            this.treeViewMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewMenu.BottomLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(237)))), ((int)(((byte)(252)))));
            this.treeViewMenu.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
            this.treeViewMenu.FullRowSelect = true;
            this.treeViewMenu.HideSelection = false;
            this.treeViewMenu.HotTracking = true;
            this.treeViewMenu.ItemHeight = 40;
            this.treeViewMenu.Location = new System.Drawing.Point(348, 0);
            this.treeViewMenu.MinusImage = ((System.Drawing.Image)(resources.GetObject("treeViewMenu.MinusImage")));
            this.treeViewMenu.MouseEnterColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(240)))), ((int)(((byte)(193)))));
            this.treeViewMenu.Name = "treeViewMenu";
            this.treeViewMenu.PlusImage = ((System.Drawing.Image)(resources.GetObject("treeViewMenu.PlusImage")));
            this.treeViewMenu.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(235)))), ((int)(((byte)(166)))));
            this.treeViewMenu.ShowLines = false;
            this.treeViewMenu.ShowNodeToolTips = true;
            this.treeViewMenu.ShowPlusMinus = false;
            this.treeViewMenu.ShowRootLines = false;
            this.treeViewMenu.Size = new System.Drawing.Size(253, 336);
            this.treeViewMenu.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 336);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.treeViewEx1);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.treeViewMenu);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private TreeViewEx treeViewMenu;
        private System.Windows.Forms.Button btnLoad;
        private TreeViewEx treeViewEx1;
        private System.Windows.Forms.Button btnUpdate;
    }
}

