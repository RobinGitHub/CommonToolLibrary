﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommonControls
{
    public class CalendarCell : DataGridViewTextBoxCell
    {
        public CalendarCell()
            : base()
        {
            this.Style.Format = "d";
        }

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);
            //CalendarEditingControl ctl = DataGridView.EditingControl as CalendarEditingControl;
            //if (this.Value == null)
            //{
            //    ctl.Value = (DateTime)this.DefaultNewRowValue;
            //}
            //else
            //{
            //    ctl.Value = (DateTime)this.Value;
            //}
        }
        //设置默认值
        //public override object DefaultNewRowValue
        //{
        //    get
        //    {

        //        return DateTime.Now;
        //    }
        //}

        public override Type EditType
        {
            get
            {

                return typeof(CalendarEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                return typeof(DateTime);
            }
        }

    }
}
