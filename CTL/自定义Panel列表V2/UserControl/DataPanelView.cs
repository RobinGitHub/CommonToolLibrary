using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

/* 把Item 改成Row
 * 
 */

namespace 自定义Panel列表V2
{
    /// <summary>
    /// 自定义Panel列表
    /// </summary>
    public partial class DataPanelView : UserControl
    {
        #region 事件
        /// <summary>
        /// 设置控件内容样式
        /// </summary>
        /// <param name="item">数据</param>
        /// <param name="scrollValue">当前滚动条位置</param>
        /// <returns></returns>
        public delegate DataPanelViewRowControl ItemTemplateDelegate(DataPanelViewRow item);
        public delegate void SelectionChangedDeletegate(DataPanelViewRow item);
        /// <summary>
        /// 设置行模版
        /// </summary>
        public event ItemTemplateDelegate SetItemTemplate;
        /// <summary>
        /// 选中发生变化
        /// </summary>
        public event SelectionChangedDeletegate SelectionChanged;
        /// <summary>
        /// 加载更多
        /// </summary>
        public event EventHandler LoadMore;
        #endregion

        #region 私有属性
        /// <summary>
        /// 内容列表
        /// </summary>
        private List<DataPanelViewRow> itemList = new List<DataPanelViewRow>();
        /// <summary>
        /// 控件列表 控件|行号
        /// </summary>
        private Dictionary<DataPanelViewRowControl, int> controlList = new Dictionary<DataPanelViewRowControl, int>();


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
        /// <summary>
        /// 是否允许多选
        /// </summary>
        private bool multiSelect = false;
        /// <summary>
        /// 内容显示的高度
        /// </summary>
        private int displayRectangleHeight = 0;
        /// <summary>
        /// 控件最多显示个数
        /// </summary>
        private int maxControlCount = 10;
        /// <summary>
        /// 最小行高
        /// 如果内容等高，这个就是行高
        /// </summary>
        private int minRowHeight = 60;
        /// <summary>
        /// 内容是否等高
        /// </summary>
        private bool isEqualHeight = true;
        /// <summary>
        /// 是否触发鼠标事件
        /// </summary>
        private bool isActiveMouseEvent = true;

        #endregion

        #region 公布属性

        #region 默认的背景色
        /// <summary>
        /// 默认的背景色
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("其他"), Description("默认的背景色")]
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
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("其他"), Description("鼠标悬停的背景色")]
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
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("其他"), Description("选中时候的背景色")]
        public Color SelectedColor
        {
            get { return selectedColor; }
            set { selectedColor = value; }
        }
        #endregion

        #region 是否允许多选
        /// <summary>
        /// 是否允许多选
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("其他"), Description("是否允许多选")]
        public bool MultiSelect
        {
            get { return multiSelect; }
            set { multiSelect = value; }
        }
        #endregion

        #region 控件的最小行高
        /// <summary>
        /// 最小行高
        /// 如果内容等高，这个就是行高
        /// </summary>
        [Browsable(false)]
        public int MinRowHeight
        {
            get { return minRowHeight; }
            set
            {
                if (minRowHeight <= 0)
                    throw new Exception("行高必须大于0");
                minRowHeight = value;
                maxControlCount = SystemInformation.WorkingArea.Height / minRowHeight + 2;//前后各加一个
            }
        }
        #endregion

        #region 获取或设置当前控件
        /// <summary>
        /// 获取或设置当前控件
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        [Browsable(false)]
        public DataPanelViewRow this[int rowIndex]
        {
            get
            {
                if (rowIndex >= 0 && rowIndex < itemList.Count)
                {
                    return itemList.First(t => t.RowIndex == rowIndex);
                }
                else
                    throw new Exception("索引超出界限");
            }
            set
            {
                if (rowIndex >= 0 && rowIndex < itemList.Count)
                {
                    DataPanelViewRow item = itemList.First(t => t.RowIndex == rowIndex);
                    item = value;
                    Refresh(rowIndex);
                }
                else
                    throw new Exception("索引超出界限");
            }
        }
        #endregion

        #region 内容是否等高
        /// <summary>
        /// 内容是否等高
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(true), Category("其他"), Description("内容是否等高")]
        public bool IsEqualHeight
        {
            get { return isEqualHeight; }
            set { isEqualHeight = value; }
        }
        #endregion

        #region 获取或设置当前滚动条，滚动到第几行
        /// <summary>
        /// 获取或设置当前滚动条，滚动到第几行
        /// </summary>
        [Browsable(false)]
        public int FirstDisplayedScrollingRowIndex
        {
            get
            {
                int rlt = 0;
                if (controlList.Count > 0)
                    rlt = controlList.First().Value;
                return rlt;
            }
            set
            {
                if (value < 0)
                    throw new Exception("FirstDisplayedScrollingRowIndex 必须大于 0");
                if (myVScrollBar1.Visible)
                {
                    //如果最后一行完全显示，推算第一行显示的行号及Top
                    int firstRowIndex = 0;
                    int firstRowTop = 0;
                    for (int i = itemList.Count - 1; i >= 0; i--)
                    {
                        firstRowTop += itemList[i].Height;
                        if (this.pnlContent.Height - firstRowTop < 0)
                        {
                            firstRowIndex = i;
                            break;
                        }
                    }
                    if (firstRowIndex <= value)
                    { //重新计算滚动条的Value
                        int tmpValue = displayRectangleHeight - this.pnlContent.Height;
                        if (tmpValue < 0)
                            tmpValue = 0;
                        this.pnlContent.VScrollValue = tmpValue;
                    }
                    else
                    {

                        this.pnlContent.VScrollValue = GetItemHeightByRowIndex(value);
                    }
                    UpdateScrollbar();
                    bool isUp = this.pnlContent.VScrollValue < this.myVScrollBar1.Value;
                    ScrollItem(isUp);
                }
                KeyValuePair<DataPanelViewRowControl, int> childItem = controlList.FirstOrDefault(t => t.Key.DataPanelRow.RowIndex == value);
                if (childItem.Key != null && childItem.Key.DataPanelRow.RowType == DataPanelRowType.ContentRow)
                {
                    item_MouseClick(childItem.Key, null);
                }
            }
        }
        #endregion

        #region 是否分组
        /// <summary>
        /// 是否分组
        /// </summary>
        private bool isGroup = false;
        /// <summary>
        /// 是否分组
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("其他"), Description("是否分组")]
        public bool IsGroup
        {
            get { return isGroup; }
            set { isGroup = value; }
        }
        #endregion

        #region 分组字段名称
        /// <summary>
        /// 分组字段名称
        /// </summary>
        public string groupFieldName = "";
        /// <summary>
        /// 分组字段名称
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("其他"), Description("分组字段名称, 只有显示分组才会有效")]
        public string GroupFieldName
        {
            get { return groupFieldName; }
            set { groupFieldName = value; }
        }
        #endregion

        #region 总行数
        /// <summary>
        /// 总行数
        /// </summary>
        [Browsable(false)]
        public int Count
        {
            get { return itemList.Count; }
        }
        #endregion

        #region 是否显示更多按钮
        /// <summary>
        /// 是否显示更多按钮
        /// </summary>
        private bool isShowMore = false;
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("其他"), Description("是否显示更多按钮")]
        public bool IsShowMore
        {
            get { return isShowMore; }
            set
            {
                isShowMore = value;
                DataPanelViewRow item = itemList.FirstOrDefault(t => t.RowType == DataPanelRowType.LoadMoreRow);
                if (!value && item != null)
                {
                    this.Remove(new List<int>() { item.RowIndex });
                }
                else if (value && item == null)
                {
                    DataPanelViewRow loadMoreItem = new DataPanelViewRow()
                    {
                        IsSelected = false,
                        RowIndex = itemList.Count,
                        RowType = DataPanelRowType.LoadMoreRow
                    };
                    InsertItem(loadMoreItem.RowIndex, loadMoreItem);
                    RefreshContent();
                }
            }
        }
        #endregion

        #endregion

        #region 构造函数
        public DataPanelView()
        {
            InitializeComponent();

            base.SetStyle(
                   ControlStyles.UserPaint |
                   ControlStyles.OptimizedDoubleBuffer |
                   ControlStyles.AllPaintingInWmPaint |
                   ControlStyles.ResizeRedraw, true);
            base.UpdateStyles();
            this.BackColor = Color.White;

            this.pnlContent.LargeChange = this.minRowHeight * 3;
            //控制滚动条滚动速度
            this.pnlContent.SmallChange = this.minRowHeight * 3;

            this.myVScrollBar1.BindControl = this.pnlContent;
            this.myVScrollBar1.Scroll += myVScrollBar1_Scroll;
            this.SizeChanged += PanelEx_SizeChanged;
        } 
        #endregion

        #region 事件
        #region 大小改变
        /// <summary>
        /// 大小改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PanelEx_SizeChanged(object sender, EventArgs e)
        {
            //ContentLengthChange();
        }
        #endregion

        #region 加载更多
        /// <summary>
        /// 加载更多
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void childItem_LoadMore(object sender, EventArgs e)
        {
            if (this.LoadMore != null)
                LoadMore(sender, e);
        }
        #endregion

        #region 滚动条滚动事件
        void myVScrollBar1_Scroll(object sender, EventArgs e)
        {
            ////解决快速移动闪屏问题
            //// https://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=ZH-CN&k=k(System.Windows.Forms.Control.Update);k(TargetFrameworkMoniker-.NETFramework,Version%3Dv3.5);k(DevLang-csharp)&rd=true
            //Thread t = new Thread(() =>
            //{
            //    this.Invoke((MethodInvoker)delegate
            //    {
            //        isActiveMouseEvent = false;
            //        ScrollItem(this.myVScrollBar1.IsMoveUp);
            //        //解决快速移动闪屏问题
            //        this.Invalidate(true);
            //        this.Update();
            //        this.pnlContent.Focus();
            //    });
            //});
            //t.Start();
        }
        #endregion

        #region 处理快捷键 ProcessCmdKey
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                #region Up
                case Keys.Up://向上滚动
                    
                    break;
                #endregion

                #region Down
                case Keys.Down://向下滚动
                    break;
                #endregion

                #region Control + A
                case Keys.Control | Keys.A://全选
                    if (multiSelect)
                    {
                        foreach (var item in controlList)
                        {
                            if (!item.Key.IsSelected && item.Key.DataPanelRow.RowType == DataPanelRowType.ContentRow)
                                item.Key.IsSelected = true;
                        }
                        foreach (var item in itemList)
                        {
                            if (!item.IsSelected && item.RowType == DataPanelRowType.ContentRow)
                                item.IsSelected = true;
                        }
                    }
                    break;
                #endregion

                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
            return true;
        }
        #endregion
        #endregion

        #region 公共方法

        #region 清空 Clear
        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
        }
        #endregion 

        #region 添加数据

        #region 数据源 DataSource
        /// <summary>
        /// 数据源
        /// </summary>
        public void DataSource<T>(DataTable dt)
            where T : DataPanelViewRow, new()
        {

        } 
        #endregion

        #region 添加行数据，会自动合并到组 Add 
        /// <summary>
        /// 添加行数据，会自动合并到组
        /// </summary>
        public void Add(DataPanelViewRow item)
        {

        } 
        #endregion

        #region 批量追加数据 Add<T>
        /// <summary>
        /// 批量追加数据
        /// </summary>
        /// <typeparam name="T">必须是继承DataPanelRow的</typeparam>
        /// <param name="dt">数据源</param>
        public void Add<T>(DataTable dt)
            where T : DataPanelViewRow, new()
        {

        } 
        #endregion

        #region 插入指定的位置 Insert
        /// <summary>
        /// 插入指定的位置
        /// 如果是分组的情况，则自动插入到组的位置
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="item"></param>
        public void Insert(int rowIndex, DataPanelViewRow item)
        {

        }  
        #endregion

        #endregion

        #region 删除数据 Remove
        /// <summary>
        /// 删除数据 
        /// </summary>
        /// <param name="rowIndexList"></param>
        public void Remove(List<int> rowIndexList)
        {

        } 
        #endregion

        #region 返回选中的数据 SelectedRows
        /// <summary>
        /// 返回选中的数据
        /// </summary>
        /// <returns></returns>
        public List<DataPanelViewRow> SelectedRows()
        {

        } 
        #endregion

        #region 内容修改后更新 Refresh
        /// <summary>
        /// 内容修改后更新
        /// </summary>
        /// <param name="rowIndex"></param>
        public void Refresh(int rowIndex)
        {

        } 
        #endregion

        #region 私有方法

        #region 插入 InsertItem
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="dpvRow"></param>
        private void InsertItem(int rowIndex, DataPanelViewRow dpvRow)
        {
            #region ContentRow
            if (dpvRow.RowType == DataPanelRowType.ContentRow)
            {
                if (isEqualHeight)
                {
                    dpvRow.Height = this.minRowHeight;
                    displayRectangleHeight += dpvRow.Height;
                    itemList.Insert(rowIndex, dpvRow);

                    if (controlList.Count < maxControlCount)
                    {
                        DataPanelViewRowControl dpvrc = SetItemTemplate(dpvRow);
                        dpvrc.Height = dpvRow.Height;
                        AddControl(dpvrc);
                    }
                }
                else
                {
                    DataPanelViewRowControl dpvrc = SetItemTemplate(dpvRow);
                    dpvrc.SetControlHeight();

                    dpvRow.Height = dpvrc.Height;
                    displayRectangleHeight += dpvRow.Height;
                    itemList.Insert(rowIndex, dpvRow);

                    if (controlList.Count < maxControlCount)
                    {
                        AddControl(dpvrc);
                    }
                }
            }
            #endregion
            #region GroupRow
            else if (dpvRow.RowType == DataPanelRowType.GroupRow)
            {
                DataPanelViewGroupRow dpvgr = dpvRow as DataPanelViewGroupRow;
                DataPanelViewRowControl dpvrc = new DataPanelViewGroupRowControl(dpvgr.GroupDateTime, dpvgr.RowCount);
                dpvrc.DataPanelRow = dpvgr;
                dpvRow.Height = dpvrc.Height;
                displayRectangleHeight += dpvRow.Height;

                itemList.Insert(rowIndex, dpvRow);

                if (controlList.Count < maxControlCount)
                {
                    AddControl(dpvrc);
                }
            }
            #endregion
            #region LoadMoreRow
            else if (dpvRow.RowType == DataPanelRowType.LoadMoreRow)
            {
                DataPanelViewLoadMoreRowControl dpvrc = new DataPanelViewLoadMoreRowControl();
                dpvrc.DataPanelRow = dpvRow;
                dpvRow.Height = dpvrc.Height;
                displayRectangleHeight += dpvRow.Height;
                dpvrc.LoadMore += childItem_LoadMore;

                itemList.Insert(rowIndex, dpvRow);

                if (controlList.Count < maxControlCount)
                {
                    AddControl(dpvrc);
                }
            }
            #endregion
        }
        #endregion 

        #region 添加到组中 AddByGroup
        /// <summary>
        /// 添加到组中
        /// </summary>
        /// <param name="item"></param>
        private void AddByGroup(DataPanelViewRow item)
        {
            /* 先按顺序查找，找不到，再查询是否有分组
             * 没有再按倒序查找
             * 
             * 这个逻辑有问题：排序方式的不同，直接决定了位置（是插前面还是后面）
             */
            int rowIndex = itemList.Count;
            DataPanelViewRow find = itemList.FirstOrDefault(t => t.RowType == DataPanelRowType.ContentRow && DateTime.Parse(t.DataRow[groupFieldName].ToString()) > DateTime.Parse(item.DataRow[groupFieldName].ToString()));
            if (find != null)
            {
                rowIndex = find.RowIndex;
                var tmpFind = itemList.FirstOrDefault(t => t.RowIndex == rowIndex - 1);
                if (tmpFind != null && tmpFind.RowType == DataPanelRowType.GroupRow)
                    rowIndex -= 1;
            }
            else if (find == null)
            {//查询组是否存在
                List<DataPanelViewGroupRow> groupItemList = itemList.Where(t => t.RowType == DataPanelRowType.GroupRow).Cast<DataPanelViewGroupRow>().ToList();
                foreach (DataPanelViewGroupRow gpItem in groupItemList)
                {
                    if (gpItem.GroupDateTime.ToString("yyyy/MM") == DateTime.Parse(item.DataRow[groupFieldName].ToString()).ToString("yyyy/MM"))
                    {
                        find = gpItem;
                        rowIndex = find.RowIndex;
                        break;
                    }
                }
            }
            else if (find == null)
            {//倒序查询
                find = itemList.FirstOrDefault(t => t.RowType == DataPanelRowType.ContentRow && DateTime.Parse(t.DataRow[groupFieldName].ToString()) < DateTime.Parse(item.DataRow[groupFieldName].ToString()));
                if (find != null)
                {
                    rowIndex = find.RowIndex;
                    var tmpFind = itemList.FirstOrDefault(t => t.RowIndex == rowIndex + 1);
                    if (tmpFind != null && tmpFind.RowType == DataPanelRowType.GroupRow)
                        rowIndex += 1;
                }
            }
            else if (find == null)
            {//查询组是否存在
                List<DataPanelViewGroupRow> groupItemList = itemList.Where(t => t.RowType == DataPanelRowType.GroupRow).Cast<DataPanelViewGroupRow>().ToList();
                foreach (DataPanelViewGroupRow gpItem in groupItemList)
                {
                    if (gpItem.GroupDateTime.ToString("yyyy/MM") == DateTime.Parse(item.DataRow[groupFieldName].ToString()).ToString("yyyy/MM"))
                    {
                        find = gpItem;
                        rowIndex = find.RowIndex;
                        break;
                    }
                }
            }
            if (find == null)
            {
                if (isShowMore)
                {
                    find = itemList.First(t => t.RowType == DataPanelRowType.LoadMoreRow);
                    if (find != null)
                    {
                        rowIndex = find.RowIndex;
                        find.RowIndex += 2;
                        KeyValuePair<MyPanelChild, int> pnlChild = controlList.FirstOrDefault(t => t.Key.RowIndex == rowIndex);
                        if (pnlChild.Key != null)
                        {
                            controlList[pnlChild.Key] = find.RowIndex;
                            pnlChild.Key.RowIndex = find.RowIndex;
                        }
                    }
                }
                item.RowIndex = rowIndex;

                InsertItem(rowIndex, item);
                //增加统计行
                DataPanelViewGroupRow groupItem = new DataPanelViewGroupRow()
                {
                    IsSelected = false,
                    RowIndex = rowIndex + 1,
                    RowType = DataPanelRowType.GroupRow,
                    GroupDateTime = DateTime.Parse(item.DataRow[groupFieldName].ToString()),
                    RowCount = 1
                };
                InsertItem(groupItem.RowIndex, groupItem);
            }
            else
            {
                //更新所有内容的索引 +1 更新统计+1
                item.RowIndex = rowIndex;

                bool isUpdateTotal = false;
                for (int i = rowIndex; i < itemList.Count; i++)
                {
                    itemList[i].RowIndex += 1;
                    if (itemList[i].RowType == DataPanelRowType.GroupRow && !isUpdateTotal)
                    {//找到最近的一个统计行
                        DataPanelViewGroupRow groupItem = itemList[i] as DataPanelViewGroupRow;
                        groupItem.RowCount += 1;
                        isUpdateTotal = true;
                    }
                }
                List<MyPanelChild> childList = controlList.Select(t => t.Key).ToList();
                for (int i = 0; i < controlList.Count; i++)
                {
                    MyPanelChild childItem = childList[i];
                    if (childItem.RowIndex >= rowIndex)
                    {
                        childItem.RowIndex += 1;
                        controlList[childItem] = childItem.RowIndex;
                    }
                }

                InsertItem(rowIndex, item);
            }
        } 
        #endregion

        #region 添加控件 AddControl
        /// <summary>
        /// 添加控件
        /// </summary>
        /// <param name="item"></param>
        /// <param name="isSetTop"></param>
        private void AddControl(DataPanelViewRowControl item, bool isSetTop = true)
        {

        } 
        #endregion

        #region 给控件赋值 GetNewInfo
        /// <summary>
        /// 给控件赋值
        /// </summary>
        /// <param name="childItem"></param>
        /// <param name="rowIndex"></param>
        private void GetNewInfo(DataPanelViewRowControl childItem, int rowIndex)
        {

        } 
        #endregion

        #region 内容变化后需调用此方法 UpdateScrollbar
        /// <summary>
        /// 内容变化后需调用此方法
        /// </summary>
        private void UpdateScrollbar()
        {
            //this.pnlContent.DisplayRectangleHeight = displayRectangleHeight;
            //this.myVScrollBar1.UpdateScrollbar();
        }
        #endregion

        #region 重新绑定控件的内容 RefreshContent
        /// <summary>
        /// 重新绑定控件的内容
        /// </summary>
        private void RefreshContent()
        {
            this.ContentLengthChange();
            foreach (var item in controlList)
            {
                GetNewInfo(item.Key, item.Key.RowIndex);
                item.Key.RefreshData();
            }
        }
        #endregion

        #region 滚动条滚动 ScrollItem
        /// <summary>
        /// 滚动条滚动
        /// </summary>
        private void ScrollItem(bool? isUp)
        {

        }
        #endregion

        #region 内容或界面大小 发生变化 ContentLengthChange
        /// <summary>
        /// 内容或界面大小 发生变化
        /// </summary>
        private void ContentLengthChange()
        { 
        }
        #endregion

        #region 清除选中项 ClearSelectedItem
        /// <summary>
        /// 清除选中项
        /// </summary>
        /// <param name="rowIndex"></param>
        private void ClearSelectedItem(int rowIndex)
        {
        }
        #endregion

        #region 通过滚动条Value 计算对应的 行索引 GetRowIndexByScrollValue
        /// <summary>
        /// 通过滚动条Value 计算对应的 行索引
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int GetRowIndexByScrollValue(int value)
        { 
        }
        #endregion

        #region 根据行索引计算对应的高度 GetItemHeightByRowIndex
        /// <summary>
        /// 根据行索引计算对应的高度
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        private int GetItemHeightByRowIndex(int rowIndex)
        {
        }
        #endregion

        #region 从当前位置 向上或向下 计算 能显示多少个内容块 GetDisplayRowCount
        /// <summary>
        /// 从当前位置 向上或向下 计算 能显示多少个内容块
        /// </summary>
        /// <param name="rowIndex">行号</param>
        /// <param name="direction">移动的方向</param>
        /// <param name="totalHeight">从当前位置，超过显示区域的高度</param>
        /// <returns></returns>
        private int GetDisplayRowCount(int rowIndex, ArrowDirection direction, out int totalHeight)
        {
        }
        #endregion

        #endregion

        #endregion

    }
}
