using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace CommonControls
{
    public class DataGridViewEx : DataGridView
    {
        #region 属性

        private bool _DisplayRowNumber = false;
        [DefaultValue(false), Category("Edu345"), DisplayName("1.DisplayRowNumber"), Description("当前控件自动在行标题中产生行号")]
        public bool DisplayRowNumber { get { return _DisplayRowNumber; } set { _DisplayRowNumber = value; } }


        private Color defaultBackColor = Color.White;
        [DefaultValue(false), Category("Edu345"), DisplayName("2.DefaultRowBackColor"), Description("行默认背景色")]
        public Color DefaultRowBackColor
        {
            get { return defaultBackColor; }
            set { defaultBackColor = value; }
        }

        private Color defaultSelectBackColor = Color.FromArgb(249, 255, 247);
        [DefaultValue(false), Category("Edu345"), DisplayName("3.DefaultSelectBackColor"), Description("行默认选中背景色")]
        public Color DefaultRowSelectBackColor
        {
            get { return defaultSelectBackColor; }
            set { defaultSelectBackColor = value; }
        }

        private Color defaultMouseEnterBaseColor = Color.FromArgb(249, 255, 247);
        [DefaultValue(false), Category("Edu345"), DisplayName("4.DefaultMouseEnterBaseColor"), Description("行默认悬停背景色")]
        public Color DefaultRowMouseEnterBaseColor
        {
            get { return defaultMouseEnterBaseColor; }
            set { defaultMouseEnterBaseColor = value; }
        }

        private bool isEditMode = false;
        [DefaultValue(false), Category("Edu345"), DisplayName("5.IsEditMode"), Description("是否是编辑模式")]
        public bool IsEditMode
        {
            get { return isEditMode; }
            set { isEditMode = value; }
        }

        #endregion

        #region 构造函数
        public DataGridViewEx()
            : base()
        {
            Type dgvType = this.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(this, true, null);

            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();

            this.AutoGenerateColumns = false;
            //this.AllowUserToAddRows = false;
            //this.AllowUserToDeleteRows = false;
            this.AllowUserToResizeRows = false;
            this.BackgroundColor = System.Drawing.Color.White;
            this.BorderStyle = BorderStyle.None;
            this.CellBorderStyle = DataGridViewCellBorderStyle.None;
            this.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(138)))), ((int)(((byte)(157)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.ColumnHeadersHeight = 28;
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = defaultSelectBackColor;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            this.DefaultCellStyle = dataGridViewCellStyle2;
            this.EnableHeadersVisualStyles = false;
            this.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(219)))), ((int)(((byte)(220)))));
            this.MultiSelect = false;
            this.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = defaultSelectBackColor;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            this.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.RowHeadersWidth = 55;
            this.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.Padding = new Padding(1);
            dataGridViewCellStyle4.SelectionBackColor = defaultSelectBackColor;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            this.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.RowTemplate.DefaultCellStyle.Padding = new Padding(1);
            this.RowTemplate.DefaultCellStyle.SelectionBackColor = defaultSelectBackColor;
            this.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.RowTemplate.Height = 23;
            this.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            this.ShowCellErrors = false;
            this.ShowRowErrors = false;
            this.Size = new System.Drawing.Size(306, 194);
            this.TabIndex = 1;

            this.RowStateChanged += DataGridViewEx_RowStateChanged;
            this.CellMouseEnter += DataGridViewEx_CellMouseEnter;
            this.CellMouseLeave += DataGridViewEx_CellMouseLeave;
            this.CellMouseClick += DataGridViewEx_CellMouseClick;
            this.RowPostPaint += DataGridViewEx_RowPostPaint;
            this.Scroll += DataGridViewEx_Scroll;
        }


        #endregion

        #region 行进入 & 离开
        void DataGridViewEx_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || isEditMode) return;
            DataGridViewRow row = this.Rows[e.RowIndex];
            row.DefaultCellStyle.BackColor = defaultMouseEnterBaseColor;
        }

        void DataGridViewEx_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || isEditMode) return;
            DataGridViewRow row = this.Rows[e.RowIndex];
            row.DefaultCellStyle.BackColor = defaultBackColor;
            this.InvalidateRow(e.RowIndex);

        }
        void DataGridViewEx_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;

            foreach (DataGridViewRow item in this.Rows)
            {
                if (item.Index != e.RowIndex)
                {
                    item.DefaultCellStyle.BackColor = defaultBackColor;
                    item.Selected = false;
                }
            }
            if (e.ColumnIndex < 0)
            {
                if (this.SelectionMode == DataGridViewSelectionMode.CellSelect)
                    this.Rows[e.RowIndex].Cells[0].Selected = true;
                else
                    this.Rows[e.RowIndex].Selected = true;
            }
        }

        #endregion

        #region 绘制行号
        void DataGridViewEx_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (!isEditMode && RowHeadersVisible && DisplayRowNumber && !e.Row.IsNewRow)
                e.Row.HeaderCell.Value = string.Format("{0}", e.Row.Index + 1);
        }
        #endregion

        #region 绘制边框
        /// <summary>
        /// 绘制边框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DataGridViewEx_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || !this.IsHandleCreated) return;
                DataGridViewRow row = this.Rows[e.RowIndex];
                if (row.IsNewRow) return;
                if (isEditMode)
                {
                    var rowIdx = (e.RowIndex + 1).ToString();
                    var centerFormat = new StringFormat()
                    {
                        // right alignment might actually make more sense for numbers  
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };

                    var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, this.RowHeadersWidth, e.RowBounds.Height);
                    e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
                }
                else
                {

                    Pen pen = new Pen(Color.FromArgb(171, 226, 151), 1);
                    bool isSelected = row.Selected;
                    for (int i = 0; i < this.Columns.Count; i++)
                    {
                        if (row.Cells[i].Selected)
                        {
                            isSelected = true;
                            break;
                        }
                    }

                    Rectangle displayRowBounds = new Rectangle(0, e.RowBounds.Top, this.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) - this.HorizontalScrollingOffset + (RowHeadersVisible ? RowHeadersWidth : 0), e.RowBounds.Height);
                    Rectangle rowBounds = new Rectangle((RowHeadersVisible ? RowHeadersWidth : 0), e.RowBounds.Top, this.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) - this.HorizontalScrollingOffset - 1, e.RowBounds.Height - 1);
                    if (isSelected)
                    {
                        row.DefaultCellStyle.BackColor = defaultMouseEnterBaseColor;
                        e.Graphics.DrawRectangle(pen, rowBounds);
                    }
                    else if (this.RectangleToScreen(displayRowBounds).Contains(Control.MousePosition))
                    {
                        //row.DefaultCellStyle.BackColor = defaultMouseEnterBaseColor;
                        e.Graphics.DrawRectangle(pen, rowBounds);
                    }
                }

            }
            catch
            {
            }
        }
        void DataGridViewEx_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll
                && this.SelectedCells.Count > 0 && this.SelectedCells[0].Visible)
            {//为了去除滚动时的绘制的重影
                this.InvalidateRow(this.SelectedCells[0].RowIndex);
            }
        }
        #endregion

        #region 共有方法
        #region 获取CheckBox选中的行
        /// <summary>
        /// 获取CheckBox选中的行
        /// </summary>
        /// <param name="colIndex">CheckBox 所在的列</param>
        /// <returns></returns>
        public List<DataGridViewRow> GetSelectedRows(int colIndex = 0)
        {
            if (this.Columns.Count == 0 || this.Columns[0].CellType != typeof(DataGridViewCheckBoxColumn))
                return null;
            List<DataGridViewRow> list = new List<DataGridViewRow>();
            foreach (DataGridViewRow item in this.Rows)
            {
                if (ConvertHelper.ObjectToBoolean(item.Cells[colIndex].Value))
                {
                    list.Add(item);
                }
            }
            return list;
        }
        #endregion
        #endregion

    }
}
