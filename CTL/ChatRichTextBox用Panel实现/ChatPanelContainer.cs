using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

/* 
 * 核心思想：用Panel实现信息展示功能
 * 主要要完成功能：
 * 1.显示方向：左、右
 * 2.显示内容有：文本、表情、语音、附件、图片、定位、连接、视频、邮件
 * 3.行类型有：查看更多消息(可点击)、内容行、内容时间、以上是历史消息行(不可点击&选中)、
 * 
 */


namespace ChatRichTextBox用Panel实现
{
    /// <summary>
    /// 
    /// </summary>
    public class ChatPanelContainer : Panel
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
                if (!VScrollVisible)
                    vScrollValue = 0;
                return vScrollValue;
            }
            set
            {
                vScrollValue = value;
                if (value < 0 || !VScrollVisible)
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
        public ChatPanelContainer()
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
