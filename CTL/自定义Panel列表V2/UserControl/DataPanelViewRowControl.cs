using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace 自定义Panel列表V2
{
    public partial class DataPanelViewRowControl : UserControl
    {
        #region API
        [DllImport("user32.dll")]

        private static extern IntPtr GetForegroundWindow();

        #endregion

        #region 私有属性
        /// <summary>
        /// 是否选中
        /// </summary>
        private bool isSelected = false;
        /// <summary>
        /// 默认的背景色
        /// </summary>
        private Color defaultColor = Color.White;
        /// <summary>
        /// 鼠标悬停的背景色
        /// </summary>
        private Color mouseEnterColor = Color.FromArgb(252, 240, 193);
        /// <summary>
        /// 选中时候的背景色
        /// </summary>
        private Color selectedColor = Color.FromArgb(252, 235, 166);

        #endregion

        #region 公布属性

        #region 是否选中
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                if (value)
                    this.BackColor = selectedColor;
                else
                    this.BackColor = defaultColor;
            }
        }
        #endregion
        
        #region 数据源
        /// <summary>
        /// 数据源
        /// </summary>
        public DataPanelViewRow DataPanelRow { get; set; }
        #endregion

        #region 默认的背景色
        /// <summary>
        /// 默认的背景色
        /// </summary>
        public Color DefaultColor
        {
            get { return defaultColor; }
            set { defaultColor = value; }
        }
        #endregion

        #region 鼠标悬停的背景色
        /// <summary>
        /// 鼠标悬停的背景色
        /// </summary>
        public Color MouseEnterColor
        {
            get { return mouseEnterColor; }
            set { mouseEnterColor = value; }
        }
        #endregion

        #region 选中时候的背景色
        /// <summary>
        /// 选中时候的背景色
        /// </summary>
        public Color SelectedColor
        {
            get { return selectedColor; }
            set { selectedColor = value; }
        }
        #endregion

        #region 是否显示底部的分割线
        /// <summary>
        /// 是否显示底部的分割线
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(true), Description("是否显示底部的分割线")]
        public bool IsShowBottomLine
        {
            get { return pnlSplitLine.Visible; }
            set { pnlSplitLine.Visible = value; }
        }
        #endregion
        #endregion

        #region 构造方法
        public DataPanelViewRowControl()
        {
            InitializeComponent();
            base.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.ResizeRedraw, true);
            base.UpdateStyles();
        } 
        #endregion

        #region override
        protected override void OnControlAdded(ControlEventArgs e)
        {
            if (DataPanelRow == null ||
                (DataPanelRow != null && DataPanelRow.RowType == DataPanelRowType.ContentRow))
            {
                Control control = e.Control; // 获取添加的子控件  
                OnControlAdded(control);
            }

            base.OnControlAdded(e);
        }

        void control_ControlAdded(object sender, ControlEventArgs e)
        {
            Control control = e.Control;
            OnControlAdded(control);
        }

        private void OnControlAdded(Control control)
        {
            if (((control.GetType() == typeof(TextBox) && !(control as TextBox).ReadOnly)
                || (control.GetType() == typeof(RichTextBox) && !(control as RichTextBox).ReadOnly)
                )
                && control.Enabled)
            {//控件允许输入
                return;
            }
            control.MouseLeave += this.control_MouseLeave; // 当鼠标离开该子控件时判断是否是离开  
            control.MouseEnter += control_MouseEnter;
            control.MouseMove += Control_MouseMove;
            control.Click += control_Click;
            control.MouseClick += control_MouseClick;
            control.DoubleClick += control_DoubleClick;
            control.ControlAdded += control_ControlAdded;

            foreach (Control item in control.Controls)
            {
                OnControlAdded(item);
            }
        }


        #region 点击事件
        void control_DoubleClick(object sender, EventArgs e)
        {
            base.OnDoubleClick(e);
        }
        void control_MouseClick(object sender, MouseEventArgs e)
        {
            base.OnMouseClick(e);
        }

        void control_Click(object sender, EventArgs e)
        {
            base.OnClick(e);
        }

        void control_MouseEnter(object sender, EventArgs e)
        {
            base.OnMouseEnter(e);
        }
        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }

        /// <summary>  
        /// 子控件鼠标离开时也要做相应的判断  
        /// </summary>  
        /// <param name="sender"></param>  
        /// <param name="e"></param>  
        void control_MouseLeave(Object sender, EventArgs e)
        {
            Control ctl = sender as Control;
            bool isActive = false;
            if (ctl.TopLevelControl.Handle == GetForegroundWindow())
            {
                isActive = true;
            }

            //判断鼠标是否还在本控件的矩形区域内  
            if (!isActive || !this.RectangleToScreen(this.DisplayRectangle).Contains(Control.MousePosition))
            {
                base.OnMouseLeave(e);
            }
        }


        #endregion
        #endregion

        #region 虚方法
        #region 刷新数据
        /// <summary>
        /// 刷新数据
        /// </summary>
        public virtual void RefreshData()
        {
            this.Invalidate(true);
            this.Update();
        }
        #endregion

        #region 设置控件高度
        /// <summary>
        /// 设置控件高度,根据显示的内容
        /// </summary>
        public virtual void SetControlHeight()
        {

        }
        #endregion
        #endregion
    }
}
