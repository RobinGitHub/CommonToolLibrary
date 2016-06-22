using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommonControls
{
    public partial class UCSplitButton : UserControl
    {
        public UCSplitButton()
        {
            InitializeComponent();
            this.btnArr.Click += btnArr_Click;
        }

        void btnArr_Click(object sender, EventArgs e)
        {
            cmsDetails.Width = this.Width;
            Point location = this.PointToScreen(btn.Location);
            cmsDetails.Show(location.X, location.Y + this.Height);
        }
    }
}
