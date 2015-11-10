using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 自定义Panel列表
{
    public class PanelMultiplex : Panel
    {
        #region 公布的属性

        #region 垂直滚动条是否显示
        /// <summary>
        /// 垂直滚动条是否显示
        /// </summary>
        public bool VScrollVisible
        {
            get
            {
                return DisplayRectangleHeight > this.Height;
            }
        }
        #endregion

        #region 行高
        /// <summary>
        /// 行高
        /// </summary>
        public int RowHeight { get; set; }
        #endregion
        
        #region 内容高度
        /// <summary>
        /// 内容高度
        /// </summary>
        public int DisplayRectangleHeight { get; set; }
        #endregion

        #region 垂直滚动条 位置
        private int vScrollValue = 0;
        /// <summary>
        /// 垂直滚动条 位置
        /// </summary>
        public int VScrollValue
        {
            get
            {
                return vScrollValue;
            }
            set
            {
                vScrollValue = value;
                if (value < 0)
                    vScrollValue = 0;
            }
        }
        #endregion

        #region 控件滚动的最大距离
        /// <summary>
        /// 控件滚动的最大距离
        /// </summary>
        public int LargeChange { get; set; }
        #endregion

        #region 控件滚动的最小距离
        /// <summary>
        /// 控件滚动的最小距离
        /// </summary>
        public int SmallChange { get; set; }
        #endregion
        #endregion

        #region 构造函数
        public PanelMultiplex()
        {
            base.SetStyle(
                   ControlStyles.UserPaint |
                   ControlStyles.OptimizedDoubleBuffer |
                   ControlStyles.AllPaintingInWmPaint |
                   ControlStyles.ResizeRedraw, true);
            base.UpdateStyles();
        } 
        #endregion

    }
}
