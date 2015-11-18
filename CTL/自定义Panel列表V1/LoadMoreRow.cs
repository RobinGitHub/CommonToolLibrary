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
    /// <summary>
    /// 加载更多
    /// </summary>
    public partial class LoadMoreRow : MyPanelChild
    {
        public event EventHandler LoadMore;
        public LoadMoreRow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载全部，或加载更多
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadMore_Click(object sender, EventArgs e)
        {
            if (LoadMore != null)
                LoadMore(sender, e);
            btnLoadMore.Text = "加载更多";
        }
    }
}
