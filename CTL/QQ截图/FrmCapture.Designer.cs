namespace QQ截图
{
    partial class FrmCapture
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.colorBox1 = new QQ截图.ColorBox();
            this.toolButton3 = new QQ截图.ToolButton();
            this.toolButton2 = new QQ截图.ToolButton();
            this.toolButton1 = new QQ截图.ToolButton();
            this.tBtn_Finish = new QQ截图.ToolButton();
            this.tBtn_Close = new QQ截图.ToolButton();
            this.tBtn_Save = new QQ截图.ToolButton();
            this.tBtn_Cancel = new QQ截图.ToolButton();
            this.tBtn_Text = new QQ截图.ToolButton();
            this.tBtn_Brush = new QQ截图.ToolButton();
            this.tBtn_Arrow = new QQ截图.ToolButton();
            this.tBtn_Ellipse = new QQ截图.ToolButton();
            this.tBtn_Rect = new QQ截图.ToolButton();
            this.imageProcessBox1 = new QQ截图.ImageProcessBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tBtn_Finish);
            this.panel1.Controls.Add(this.tBtn_Close);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.tBtn_Save);
            this.panel1.Controls.Add(this.tBtn_Cancel);
            this.panel1.Controls.Add(this.tBtn_Text);
            this.panel1.Controls.Add(this.tBtn_Brush);
            this.panel1.Controls.Add(this.tBtn_Arrow);
            this.panel1.Controls.Add(this.tBtn_Ellipse);
            this.panel1.Controls.Add(this.tBtn_Rect);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(25, 40);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(284, 25);
            this.panel1.TabIndex = 2;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::QQ截图.Properties.Resources.separator;
            this.pictureBox2.Location = new System.Drawing.Point(199, 5);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(1, 17);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 10;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::QQ截图.Properties.Resources.separator;
            this.pictureBox1.Location = new System.Drawing.Point(138, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1, 17);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.colorBox1);
            this.panel2.Controls.Add(this.toolButton3);
            this.panel2.Controls.Add(this.toolButton2);
            this.panel2.Controls.Add(this.toolButton1);
            this.panel2.Location = new System.Drawing.Point(25, 71);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(250, 32);
            this.panel2.TabIndex = 3;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(25, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 19);
            this.textBox1.TabIndex = 4;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.Resize += new System.EventHandler(this.textBox1_Resize);
            this.textBox1.Validating += new System.ComponentModel.CancelEventHandler(this.textBox1_Validating);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // colorBox1
            // 
            this.colorBox1.Location = new System.Drawing.Point(85, -2);
            this.colorBox1.Name = "colorBox1";
            this.colorBox1.Size = new System.Drawing.Size(165, 35);
            this.colorBox1.TabIndex = 4;
            this.colorBox1.Text = "colorBox1";
            // 
            // toolButton3
            // 
            this.toolButton3.BtnImage = global::QQ截图.Properties.Resources.large;
            this.toolButton3.IsSelected = false;
            this.toolButton3.IsSelectedBtn = true;
            this.toolButton3.IsSingleSelectedBtn = false;
            this.toolButton3.Location = new System.Drawing.Point(57, 6);
            this.toolButton3.Name = "toolButton3";
            this.toolButton3.Size = new System.Drawing.Size(21, 21);
            this.toolButton3.TabIndex = 3;
            // 
            // toolButton2
            // 
            this.toolButton2.BtnImage = global::QQ截图.Properties.Resources.middle;
            this.toolButton2.IsSelected = false;
            this.toolButton2.IsSelectedBtn = true;
            this.toolButton2.IsSingleSelectedBtn = false;
            this.toolButton2.Location = new System.Drawing.Point(30, 6);
            this.toolButton2.Name = "toolButton2";
            this.toolButton2.Size = new System.Drawing.Size(21, 21);
            this.toolButton2.TabIndex = 2;
            // 
            // toolButton1
            // 
            this.toolButton1.BtnImage = global::QQ截图.Properties.Resources.small;
            this.toolButton1.IsSelected = false;
            this.toolButton1.IsSelectedBtn = true;
            this.toolButton1.IsSingleSelectedBtn = false;
            this.toolButton1.Location = new System.Drawing.Point(3, 6);
            this.toolButton1.Name = "toolButton1";
            this.toolButton1.Size = new System.Drawing.Size(21, 21);
            this.toolButton1.TabIndex = 1;
            // 
            // tBtn_Finish
            // 
            this.tBtn_Finish.BtnImage = global::QQ截图.Properties.Resources.ok;
            this.tBtn_Finish.IsSelected = false;
            this.tBtn_Finish.IsSelectedBtn = true;
            this.tBtn_Finish.IsSingleSelectedBtn = false;
            this.tBtn_Finish.Location = new System.Drawing.Point(233, 3);
            this.tBtn_Finish.Name = "tBtn_Finish";
            this.tBtn_Finish.Size = new System.Drawing.Size(50, 21);
            this.tBtn_Finish.TabIndex = 11;
            this.tBtn_Finish.Text = "完成";
            this.tBtn_Finish.Click += new System.EventHandler(this.tBtn_Finish_Click);
            // 
            // tBtn_Close
            // 
            this.tBtn_Close.BtnImage = global::QQ截图.Properties.Resources.close;
            this.tBtn_Close.IsSelected = false;
            this.tBtn_Close.IsSelectedBtn = true;
            this.tBtn_Close.IsSingleSelectedBtn = false;
            this.tBtn_Close.Location = new System.Drawing.Point(206, 3);
            this.tBtn_Close.Name = "tBtn_Close";
            this.tBtn_Close.Size = new System.Drawing.Size(21, 21);
            this.tBtn_Close.TabIndex = 7;
            // 
            // tBtn_Save
            // 
            this.tBtn_Save.BtnImage = global::QQ截图.Properties.Resources.save;
            this.tBtn_Save.IsSelected = false;
            this.tBtn_Save.IsSelectedBtn = true;
            this.tBtn_Save.IsSingleSelectedBtn = false;
            this.tBtn_Save.Location = new System.Drawing.Point(172, 3);
            this.tBtn_Save.Name = "tBtn_Save";
            this.tBtn_Save.Size = new System.Drawing.Size(21, 21);
            this.tBtn_Save.TabIndex = 6;
            this.tBtn_Save.Click += new System.EventHandler(this.tBtn_Save_Click);
            // 
            // tBtn_Cancel
            // 
            this.tBtn_Cancel.BtnImage = global::QQ截图.Properties.Resources.cancel;
            this.tBtn_Cancel.IsSelected = false;
            this.tBtn_Cancel.IsSelectedBtn = true;
            this.tBtn_Cancel.IsSingleSelectedBtn = false;
            this.tBtn_Cancel.Location = new System.Drawing.Point(145, 3);
            this.tBtn_Cancel.Name = "tBtn_Cancel";
            this.tBtn_Cancel.Size = new System.Drawing.Size(21, 21);
            this.tBtn_Cancel.TabIndex = 5;
            this.tBtn_Cancel.Click += new System.EventHandler(this.tBtn_Cancel_Click);
            // 
            // tBtn_Text
            // 
            this.tBtn_Text.BtnImage = global::QQ截图.Properties.Resources.text;
            this.tBtn_Text.IsSelected = false;
            this.tBtn_Text.IsSelectedBtn = true;
            this.tBtn_Text.IsSingleSelectedBtn = false;
            this.tBtn_Text.Location = new System.Drawing.Point(111, 3);
            this.tBtn_Text.Name = "tBtn_Text";
            this.tBtn_Text.Size = new System.Drawing.Size(21, 21);
            this.tBtn_Text.TabIndex = 4;
            // 
            // tBtn_Brush
            // 
            this.tBtn_Brush.BtnImage = global::QQ截图.Properties.Resources.brush;
            this.tBtn_Brush.IsSelected = false;
            this.tBtn_Brush.IsSelectedBtn = true;
            this.tBtn_Brush.IsSingleSelectedBtn = false;
            this.tBtn_Brush.Location = new System.Drawing.Point(84, 3);
            this.tBtn_Brush.Name = "tBtn_Brush";
            this.tBtn_Brush.Size = new System.Drawing.Size(21, 21);
            this.tBtn_Brush.TabIndex = 3;
            // 
            // tBtn_Arrow
            // 
            this.tBtn_Arrow.BtnImage = global::QQ截图.Properties.Resources.arrow;
            this.tBtn_Arrow.IsSelected = false;
            this.tBtn_Arrow.IsSelectedBtn = true;
            this.tBtn_Arrow.IsSingleSelectedBtn = false;
            this.tBtn_Arrow.Location = new System.Drawing.Point(57, 3);
            this.tBtn_Arrow.Name = "tBtn_Arrow";
            this.tBtn_Arrow.Size = new System.Drawing.Size(21, 21);
            this.tBtn_Arrow.TabIndex = 2;
            // 
            // tBtn_Ellipse
            // 
            this.tBtn_Ellipse.BtnImage = global::QQ截图.Properties.Resources.ellips;
            this.tBtn_Ellipse.IsSelected = false;
            this.tBtn_Ellipse.IsSelectedBtn = true;
            this.tBtn_Ellipse.IsSingleSelectedBtn = false;
            this.tBtn_Ellipse.Location = new System.Drawing.Point(30, 3);
            this.tBtn_Ellipse.Name = "tBtn_Ellipse";
            this.tBtn_Ellipse.Size = new System.Drawing.Size(21, 21);
            this.tBtn_Ellipse.TabIndex = 1;
            // 
            // tBtn_Rect
            // 
            this.tBtn_Rect.BtnImage = global::QQ截图.Properties.Resources.rect;
            this.tBtn_Rect.IsSelected = false;
            this.tBtn_Rect.IsSelectedBtn = true;
            this.tBtn_Rect.IsSingleSelectedBtn = false;
            this.tBtn_Rect.Location = new System.Drawing.Point(3, 3);
            this.tBtn_Rect.Name = "tBtn_Rect";
            this.tBtn_Rect.Size = new System.Drawing.Size(21, 21);
            this.tBtn_Rect.TabIndex = 0;
            // 
            // imageProcessBox1
            // 
            this.imageProcessBox1.BackColor = System.Drawing.Color.Black;
            this.imageProcessBox1.BaseImage = null;
            this.imageProcessBox1.CanReset = true;
            this.imageProcessBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.imageProcessBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageProcessBox1.ForeColor = System.Drawing.Color.White;
            this.imageProcessBox1.Location = new System.Drawing.Point(0, 0);
            this.imageProcessBox1.Name = "imageProcessBox1";
            this.imageProcessBox1.Size = new System.Drawing.Size(363, 247);
            this.imageProcessBox1.TabIndex = 0;
            this.imageProcessBox1.Text = "imageProcessBox1";
            this.imageProcessBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.imageProcessBox1_Paint);
            this.imageProcessBox1.DoubleClick += new System.EventHandler(this.imageProcessBox1_DoubleClick);
            this.imageProcessBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.imageProcessBox1_MouseDown);
            this.imageProcessBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.imageProcessBox1_MouseMove);
            this.imageProcessBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.imageProcessBox1_MouseUp);
            // 
            // FrmCapture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 247);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.imageProcessBox1);
            this.Name = "FrmCapture";
            this.Text = "FrmCapture";
            this.Load += new System.EventHandler(this.FrmCapture_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ImageProcessBox imageProcessBox1;
        private System.Windows.Forms.Panel panel1;
        private ToolButton tBtn_Rect;
        private System.Windows.Forms.PictureBox pictureBox1;
        private ToolButton tBtn_Arrow;
        private ToolButton tBtn_Ellipse;
        private ToolButton tBtn_Save;
        private ToolButton tBtn_Cancel;
        private ToolButton tBtn_Text;
        private ToolButton tBtn_Brush;
        private ToolButton tBtn_Finish;
        private ToolButton tBtn_Close;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel panel2;
        private ToolButton toolButton2;
        private ToolButton toolButton1;
        private ToolButton toolButton3;
        private ColorBox colorBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Timer timer1;

    }
}

