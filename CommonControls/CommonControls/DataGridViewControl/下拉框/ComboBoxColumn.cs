using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommonControls
{
    public class ComboBoxColumn : DataGridViewColumn
    {
        public ComboBoxColumn()
            : base(new ComboBoxCell())
        {
        }
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (value != null && !value.GetType().IsAssignableFrom(typeof(ComboBoxCell)))
                {
                    string s = "Must be a ComboBoxCell！";
                    throw new InvalidCastException(s);
                }
                base.CellTemplate = value;
            }
        }


    }
}
