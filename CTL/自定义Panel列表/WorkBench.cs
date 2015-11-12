using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 自定义Panel列表
{
    public partial class WorkBench : MyPanelChild
    {
        public WorkBench()
        {
            InitializeComponent();
        }

        #region 刷新数据
        /// <summary>
        /// 刷新数据
        /// </summary>
        public void RefreshData()
        {
            if (base.DataRow != null)
            {
                label1.Text = base.DataRow[0].ToString();
                label2.Text = base.DataRow[0].ToString();
                label3.Text = base.DataRow[0].ToString();
                label4.Text = base.DataRow[0].ToString();
                label5.Text = base.DataRow[0].ToString();
                label6.Text = base.DataRow[0].ToString();

            }
        }
        #endregion
    }
}
