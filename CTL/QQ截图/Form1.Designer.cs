namespace QQ截图
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showMainWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.checkBox_shift = new System.Windows.Forms.CheckBox();
            this.checkBox_alt = new System.Windows.Forms.CheckBox();
            this.checkBox_ctrl = new System.Windows.Forms.CheckBox();
            this.checkBox_CaptureCursor = new System.Windows.Forms.CheckBox();
            this.checkBox_AutoRun = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showMainWindowToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(188, 48);
            // 
            // showMainWindowToolStripMenuItem
            // 
            this.showMainWindowToolStripMenuItem.Name = "showMainWindowToolStripMenuItem";
            this.showMainWindowToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.showMainWindowToolStripMenuItem.Text = "Show MainWindow";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.checkBox_shift);
            this.groupBox1.Controls.Add(this.checkBox_alt);
            this.groupBox1.Controls.Add(this.checkBox_ctrl);
            this.groupBox1.Location = new System.Drawing.Point(12, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(239, 45);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "HotKey";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(146, 17);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(87, 21);
            this.textBox1.TabIndex = 7;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // checkBox_shift
            // 
            this.checkBox_shift.AutoSize = true;
            this.checkBox_shift.Location = new System.Drawing.Point(95, 19);
            this.checkBox_shift.Name = "checkBox_shift";
            this.checkBox_shift.Size = new System.Drawing.Size(54, 16);
            this.checkBox_shift.TabIndex = 6;
            this.checkBox_shift.Text = "shift";
            this.checkBox_shift.UseVisualStyleBackColor = true;
            // 
            // checkBox_alt
            // 
            this.checkBox_alt.AutoSize = true;
            this.checkBox_alt.Location = new System.Drawing.Point(52, 19);
            this.checkBox_alt.Name = "checkBox_alt";
            this.checkBox_alt.Size = new System.Drawing.Size(42, 16);
            this.checkBox_alt.TabIndex = 5;
            this.checkBox_alt.Text = "alt";
            this.checkBox_alt.UseVisualStyleBackColor = true;
            // 
            // checkBox_ctrl
            // 
            this.checkBox_ctrl.AutoSize = true;
            this.checkBox_ctrl.Location = new System.Drawing.Point(6, 19);
            this.checkBox_ctrl.Name = "checkBox_ctrl";
            this.checkBox_ctrl.Size = new System.Drawing.Size(48, 16);
            this.checkBox_ctrl.TabIndex = 4;
            this.checkBox_ctrl.Text = "ctrl";
            this.checkBox_ctrl.UseVisualStyleBackColor = true;
            // 
            // checkBox_CaptureCursor
            // 
            this.checkBox_CaptureCursor.AutoSize = true;
            this.checkBox_CaptureCursor.Location = new System.Drawing.Point(130, 13);
            this.checkBox_CaptureCursor.Name = "checkBox_CaptureCursor";
            this.checkBox_CaptureCursor.Size = new System.Drawing.Size(102, 16);
            this.checkBox_CaptureCursor.TabIndex = 1;
            this.checkBox_CaptureCursor.Text = "CaptureCursor";
            this.checkBox_CaptureCursor.UseVisualStyleBackColor = true;
            // 
            // checkBox_AutoRun
            // 
            this.checkBox_AutoRun.AutoSize = true;
            this.checkBox_AutoRun.Location = new System.Drawing.Point(16, 13);
            this.checkBox_AutoRun.Name = "checkBox_AutoRun";
            this.checkBox_AutoRun.Size = new System.Drawing.Size(120, 16);
            this.checkBox_AutoRun.TabIndex = 0;
            this.checkBox_AutoRun.Text = "AutomaticllayRun";
            this.checkBox_AutoRun.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox_CaptureCursor);
            this.groupBox2.Controls.Add(this.checkBox_AutoRun);
            this.groupBox2.Location = new System.Drawing.Point(12, 62);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(239, 41);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Ohter";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(176, 109);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 21);
            this.button1.TabIndex = 3;
            this.button1.Text = "setting";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 135);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "截屏";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem showMainWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox checkBox_shift;
        private System.Windows.Forms.CheckBox checkBox_alt;
        private System.Windows.Forms.CheckBox checkBox_ctrl;
        private System.Windows.Forms.CheckBox checkBox_CaptureCursor;
        private System.Windows.Forms.CheckBox checkBox_AutoRun;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button1;

    }
}