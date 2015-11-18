using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace 自定义Panel列表V1
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

        private PanelRowType rowType = PanelRowType.ContentRow;
        /// <summary>
        /// 内容行类别
        /// </summary>
        public PanelRowType RowType
        {
            get { return rowType; }
            set { rowType = value; }
        }
    }
    /// <summary>
    /// 内容行类别
    /// </summary>
    public enum PanelRowType
    { 
        /// <summary>
        /// 内容行
        /// </summary>
        ContentRow = 0,
        /// <summary>
        /// 分组统计行
        /// </summary>
        GroupRow = 1,
        /// <summary>
        /// 加载更多行
        /// </summary>
        LoadMoreRow = 2,
    }
}
