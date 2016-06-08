using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CommonControls
{
    /// <summary>
    /// 带下箭头的Button
    /// </summary>
    public class ScrollArrowButton : BxControlPart
    {
        protected override void RenderControl(Graphics g, ButtonState buttonState, CheckState checkState)
        {
            ControlPaint.DrawScrollButton(g, ClientRectangle, sButton, buttonState);
        }
        private ScrollButton sButton = ScrollButton.Up;
        public ScrollButton ButtonType
        {
            get { return sButton; }
            set
            {
                if (sButton != value)
                {
                    sButton = value;
                    Invalidate();
                }
            }
        }
    }
}
