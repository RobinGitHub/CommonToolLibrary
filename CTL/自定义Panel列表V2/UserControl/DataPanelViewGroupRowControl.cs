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
    public partial class DataPanelViewGroupRowControl : DataPanelViewRowControl
    {
        /// <summary>
        /// 是否显示统计
        /// </summary>
        private bool isShowGroupTotal = true;
        public DataPanelViewGroupRowControl()
        {
            InitializeComponent();
        }
        public DataPanelViewGroupRowControl(string title, int rowCount, bool isShowGroupTotal, ContentAlignment textAlign, int left)
            : this()
        {
            this.isShowGroupTotal = isShowGroupTotal;
            SetTitle(title, rowCount);
            lblTitle.Left = left;
            lblTitle.TextAlign = textAlign;
        }

        private void SetTitle(string title, int rowCount)
        {
            lblTitle.Text = string.Format("{0}", title);
            if (isShowGroupTotal)
                lblTitle.Text += string.Format("，共{0}行", rowCount);
        }

        public override void RefreshData()
        {
            if (base.DataPanelRow != null)
            {
                DataPanelViewGroupRow model = base.DataPanelRow as DataPanelViewGroupRow;
                SetTitle(model.GroupDispalyText, model.RowCount);
            }
            base.RefreshData();
        }
    }
}
