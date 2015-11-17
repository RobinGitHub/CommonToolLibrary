﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using 自定义Panel列表V1;

namespace 自定义Panel列表V1
{
    public partial class ReplyUserControl : MyPanelChild
    {
        public new EventHandler SizeChanged;

        public ReplyUserControl()
        {
            InitializeComponent();
        }

        private void ReplyUserControl_Load(object sender, EventArgs e)
        {
        }

        private void btnComment_Click(object sender, EventArgs e)
        {
            int oldHeight = this.Height;
            this.pnlReplyContent.Visible = !this.pnlReplyContent.Visible;
            ReplyModel model = base.PanelItem as ReplyModel;
            model.IsShowReply = this.pnlReplyContent.Visible;
            if (this.Height == oldHeight && model.ReplyData != null && model.ReplyData.Rows.Count > 0)
            {
                if (this.pnlReplyContent.Visible)
                    this.Height += this.pnlReplyContent.Height;
                else
                    this.Height -= this.pnlReplyContent.Height;
            }
            if (SizeChanged != null)
                SizeChanged(this, null);
        }

        private void btnReply_Click(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(txtReplyContent.Text))
            //{
            //    this.pnlReplyContent.Visible = !this.pnlReplyContent.Visible;

            //    ReplyModel model = base.PanelItem as ReplyModel;
            //    model.IsShowReply = this.pnlReplyContent.Visible;

            //    if (SizeChanged != null)
            //        SizeChanged(this, null);
            //}
            Label lbl = new Label();
            lbl.Text = "测试";
            lbl.BringToFront();
            lbl.Left = 20;
            lbl.Top = 80;

            panel3.Controls.Add(lbl);

        }

        public override void RefreshData()
        {
            if (base.PanelItem != null)
            {
                ReplyModel model = base.PanelItem as ReplyModel;
                this.pnlReplyContent.Visible = model.IsShowReply;

                lblDateAndUser.Text = model.DataRow["Date"].ToString() + " " + model.DataRow["UserName"].ToString();
                lblTitle.Text = model.DataRow["Title"].ToString();

                pnlReplyDetail.Controls.Clear();
                if (model.ReplyData != null && model.ReplyData.Rows.Count > 0)
                {
                    pnlReplyDetail.Visible = true;
                    int lastTop = 5;
                    foreach (DataRow item in model.ReplyData.Rows)
                    {
                        Label label1 = new Label();
                        label1.Top = lastTop;
                        label1.Text = item["Date"].ToString() + " " + item["UserName"].ToString();
                        label1.Width = this.Width;
                        pnlReplyDetail.Controls.Add(label1);

                        Label label2 = new Label();
                        label2.Top = label1.Top + label1.Height;
                        label2.Text = item["Title"].ToString();
                        label2.AutoSize = false;
                        label2.AutoEllipsis = true;
                        label2.Width = this.Width;
                        label2.MaximumSize = new System.Drawing.Size(this.Width, 300);
                        pnlReplyDetail.Controls.Add(label2);

                        Panel pnlSplitLine = new Panel();
                        pnlSplitLine.BackColor = Color.Gray;
                        pnlSplitLine.Height = 1;
                        pnlSplitLine.Top = label2.Top + label2.Height;
                        pnlSplitLine.Width = this.Width;
                        pnlReplyDetail.Controls.Add(pnlSplitLine);

                        lastTop = pnlSplitLine.Top + pnlSplitLine.Height +2;
                    }
                    //this.Height = pnlReplyDetail.Top + lastTop;
                }
                else
                {
                    pnlReplyDetail.Visible = false;
                    //this.Height = lblTitle.Top + lblTitle.Height;
                }
                
            }
        }
    }
}
