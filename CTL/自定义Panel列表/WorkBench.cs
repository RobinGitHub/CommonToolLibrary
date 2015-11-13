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
        public new event EventHandler SizeChanged = null;

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

            }
        }
        #endregion

        private void label6_Click(object sender, EventArgs e)
        {
            if (label6.Text == "展开")
            {
                label6.Text = "收起";
                this.Height += 20;
            }
            else
            {
                label6.Text = "展开";
                this.Height -= 20;
            }
            if (this.SizeChanged != null)
                this.SizeChanged(this, null);
        }
    }
}
