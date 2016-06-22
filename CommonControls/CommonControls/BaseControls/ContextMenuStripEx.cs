using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommonControls
{
    public class ContextMenuStripEx : ContextMenuStrip
    {
        public ContextMenuStripEx()
        {
            this.ShowImageMargin = false;
            this.BackColor = Color.White;
            //this.RenderMode = ToolStripRenderMode.Professional;
            this.DropShadowEnabled = false;
        }

    }
}
