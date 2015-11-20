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
    public partial class DataPanelViewLoadMoreRowControl : DataPanelViewRowControl
    {
        public event EventHandler LoadMore;
        public DataPanelViewLoadMoreRowControl()
        {
            InitializeComponent();
        }

        private void btnLoadMore_Click(object sender, EventArgs e)
        {
            if (LoadMore != null)
                LoadMore(sender, e);
        }
    }
}
