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
    public partial class MyPanelChild : UserControl
    {
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

        #region 行号
        /// <summary>
        /// 行号
        /// </summary>
        public int RowIndex { get; set; }
        #endregion

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
        public DataRow DataRow { get; set; }
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

        #endregion

        #region 构造函数
        public MyPanelChild()
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
            Control control = e.Control; // 获取添加的子控件  
            control.MouseLeave += this.control_MouseLeave; // 当鼠标离开该子控件时判断是否是离开  
            control.MouseEnter += control_MouseEnter;
            control.Click += control_Click;
            control.MouseClick += control_MouseClick;
            control.DoubleClick += control_DoubleClick;
            base.OnControlAdded(e);
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
        /// <summary>  
        /// 子控件鼠标离开时也要做相应的判断  
        /// </summary>  
        /// <param name="sender"></param>  
        /// <param name="e"></param>  
        void control_MouseLeave(Object sender, EventArgs e)
        {
            //判断鼠标是否还在本控件的矩形区域内  
            if (!this.RectangleToScreen(this.DisplayRectangle).Contains(Control.MousePosition))
            {
                base.OnMouseLeave(e);
            }
        }
        #endregion
        #endregion

        public virtual void RefreshData()
        { 
            
        }
    }
}
