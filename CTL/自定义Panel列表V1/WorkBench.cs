using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 自定义Panel列表V1
{
    public partial class WorkBench : MyPanelChild
    {
        public WorkBench()
        {
            InitializeComponent();
        }
        public override void RefreshData()
        {
            if (base.PanelItem != null)
            {
                label5.Text = base.PanelItem.DataRow["Title"].ToString();

            }
        }
    }
}
