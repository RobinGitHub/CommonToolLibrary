using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommonControls
{
    public class ComboBoxEx : ComboBox
    {
        public ComboBoxEx()
        {
            this.ItemHeight = 21;
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DropDownStyle = ComboBoxStyle.DropDownList;
        }
    }
}
