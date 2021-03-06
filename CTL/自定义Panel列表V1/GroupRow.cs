﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 自定义Panel列表V1
{
    public partial class GroupRow : MyPanelChild
    {
        public GroupRow()
        {
            InitializeComponent();
        }

        public GroupRow(DateTime dateTime, int rowCount, string dateTimeFormart = "yyyy/MM")
            : this()
        {
            SetTitle(dateTime, rowCount, dateTimeFormart);
        }

        private void SetTitle(DateTime dateTime, int rowCount, string dateTimeFormart = "yyyy/MM")
        {
            lblTitle.Text = string.Format("以上为{0}月的数据，共{1}行", dateTime.ToString(dateTimeFormart), rowCount);
        }

        public override void RefreshData()
        {
            if (base.PanelItem != null)
            {
                GroupRowItem model = base.PanelItem as GroupRowItem;
                SetTitle(model.GroupDateTime, model.RowCount);
            }
            base.RefreshData();
        }
    }
}
