﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace 自定义Panel列表V2
{
    /// <summary>
    /// 用于记录每行数据属性
    /// </summary>
    public class DataPanelViewRow
    {
        /// <summary>
        /// 预留字段
        /// </summary>
        public string Key { get; set; }
        public int RowIndex { get; set; }
        public DataRow DataRow { get; set; }
        public bool IsSelected { get; set; }

        public int Height { get; set; }
        /// <summary>
        /// 是否获得焦点
        /// 不管是否多选，有且只能有一个为true
        /// </summary>
        public bool IsFocus { get; set; }

        private DataPanelRowType rowType = DataPanelRowType.ContentRow;
        /// <summary>
        /// 内容行类别
        /// </summary>
        public DataPanelRowType RowType
        {
            get { return rowType; }
            set { rowType = value; }
        }
        /// <summary>
        /// 分组内容
        /// </summary>
        public string GroupValue { get; set; }
        /// <summary>
        /// 组内排序
        /// </summary>
        public int GroupValueIndex { get; set; }
        /// <summary>
        /// 分组统计要显示的内容，不包含统计信息
        /// </summary>
        public string GroupDispalyText { get; set; }
    }
    /// <summary>
    /// 内容行类别
    /// </summary>
    public enum DataPanelRowType
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

    #region 分组对象
    /// <summary>
    /// 分组对象
    /// </summary>
    internal class DataPanelViewGroupRow : DataPanelViewRow
    {
        ///// <summary>
        ///// 分组时间
        ///// </summary>
        //public DateTime GroupDateTime { get; set; }

        /// <summary>
        /// 行数
        /// </summary>
        public int RowCount { get; set; }
    }
    #endregion
}
