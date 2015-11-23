using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 自定义Panel列表V2
{
    public partial class WorkBench : DataPanelViewRowControl
    {
        public WorkBench()
        {
            InitializeComponent();
        }
        public override void RefreshData()
        {
            if (base.DataPanelRow != null)
            {
                label5.Text = base.DataPanelRow.DataRow["Title"].ToString();

            }
        }
    }
}
