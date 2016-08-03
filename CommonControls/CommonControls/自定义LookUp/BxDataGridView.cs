using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace CommonControls
{
    /// <summary>
    /// 重写列头和行头样式GridView
    /// </summary>
    public partial class BxDataGridView : DataGridView
    {
        /// <summary>
        /// 弹出右键菜单
        /// </summary>
        public delegate void ShowContextMenu(object sender);
        public event ShowContextMenu ShowContextMenuEvent;//右键事件   

        /// <summary>
        /// 构造函数
        /// </summary>
        public BxDataGridView()
        {
            InitializeComponent();
            this.CellMouseClick += new DataGridViewCellMouseEventHandler(DataGridView_CellMouseDown);
        }

        /// <summary>
        /// 右键事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    DataGridView dgv = sender as DataGridView;
                    //若行已是选中状态就不再进行设置
                    if (dgv.Rows[e.RowIndex].Selected == false)
                    {   
                        dgv.ClearSelection();
                        dgv.Rows[e.RowIndex].Selected = true;
                    }
                    //只选中一行时设置活动单元格                    
                    if (dgv.SelectedRows.Count == 1)
                        dgv.CurrentCell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];

                    //进行相应的操作                    
                    if (ShowContextMenuEvent != null)
                        ShowContextMenuEvent(sender);
                }
            }
        }

        int _RowHeadWidth = 41;
        /// <summary>
        /// Column和RowHeader绘制
        /// </summary>
        /// <param name="e"></param>
        private void DrawColumnRow(DataGridViewCellPaintingEventArgs e)
        {
            SolidBrush backColorBrush = new SolidBrush(System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(251)))), ((int)(((byte)(255))))));

            Rectangle border = e.CellBounds;
            border.Width -= 1;
            //填充绘制效果
            e.Graphics.FillRectangle(backColorBrush, border);
            //绘制Column、Row的Text信息
            e.PaintContent(e.CellBounds);
            //绘制边框
            ControlPaint.DrawBorder3D(e.Graphics, e.CellBounds, Border3DStyle.RaisedInner);
        }

        protected override void OnCellPainting(System.Windows.Forms.DataGridViewCellPaintingEventArgs e)
        {
            //如果是Column
            if (e.RowIndex == -1)
            {
                DrawColumnRow(e);
                e.Handled = true; //如果是Rowheader               
            }
            else if (e.ColumnIndex == -1 && e.RowIndex >= 0)
            {
                DrawColumnRow(e);
                _RowHeadWidth = e.CellBounds.Width;
                e.Handled = true;
            }
        }
    }
}