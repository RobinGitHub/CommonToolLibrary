namespace CommonControls.SplitButton
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
            this.ucSplitButton1 = new CommonControls.UCSplitButton();
            this.SuspendLayout();
            // 
            // ucSplitButton1
            // 
            this.ucSplitButton1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucSplitButton1.Location = new System.Drawing.Point(13, 13);
            this.ucSplitButton1.Name = "ucSplitButton1";
            this.ucSplitButton1.Size = new System.Drawing.Size(90, 21);
            this.ucSplitButton1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.ucSplitButton1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private UCSplitButton ucSplitButton1;

    }
}