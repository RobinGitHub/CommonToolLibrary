namespace CommonControls.自定义LookUp
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.lookupEditor1 = new CommonControls.LookupEditor();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lookupEditor1
            // 
            this.lookupEditor1._DisplayMember = null;
            this.lookupEditor1.CanResize = true;
            this.lookupEditor1.ChangeRegion = false;
            this.lookupEditor1.DataSource = null;
            this.lookupEditor1.DisplayMember = null;
            this.lookupEditor1.DisplayValue = "";
            this.lookupEditor1.IsOpen = false;
            this.lookupEditor1.Location = new System.Drawing.Point(31, 85);
            this.lookupEditor1.Mapping = ((System.Collections.Generic.Dictionary<string, string>)(resources.GetObject("lookupEditor1.Mapping")));
            this.lookupEditor1.Name = "lookupEditor1";
            this.lookupEditor1.Opacity = 1D;
            this.lookupEditor1.OpenFocused = false;
            this.lookupEditor1.ReadOnly = false;
            this.lookupEditor1.Size = new System.Drawing.Size(220, 21);
            this.lookupEditor1.TabIndex = 0;
            this.lookupEditor1.Target = null;
            this.lookupEditor1.TextColor = System.Drawing.SystemColors.Control;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(22, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "获取值";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(176, 13);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "重置";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lookupEditor1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private LookupEditor lookupEditor1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}