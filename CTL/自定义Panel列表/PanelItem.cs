using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace 自定义Panel列表
{
    /// <summary>
    /// 用于记录每行数据属性
    /// </summary>
    public class PanelItem
    {
        public int RowIndex { get; set; }
        public DataRow DataRow { get; set; }
        public bool IsSelected { get; set; }

        public int Height { get; set; }
        /// <summary>
        /// 是否获得焦点
        /// 不管是否多选，有且只能有一个为true
        /// </summary>
        public bool IsFocus { get; set; }
        /// <summary>
        /// 对应的控件
        /// </summary>
        public MyPanelChild PanelChild { get; set; }

    }
}
