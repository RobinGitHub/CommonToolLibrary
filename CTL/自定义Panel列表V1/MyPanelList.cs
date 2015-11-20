using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
/* 支持等高&非等高
 * 
 */
namespace 自定义Panel列表V1
{
    public partial class MyPanelList : UserControl
    {
        #region 事件
        /// <summary>
        /// 设置控件内容样式
        /// </summary>
        /// <param name="item">数据</param>
        /// <param name="scrollValue">当前滚动条位置</param>
        /// <returns></returns>
        public delegate MyPanelChild ItemTemplateDelegate(PanelItem item);
        public delegate void SelectionChangedDeletegate(PanelItem item);
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
        private List<PanelItem> itemList = new List<PanelItem>();
        /// <summary>
        /// 控件列表 控件|行号
        /// </summary>
        private Dictionary<MyPanelChild, int> controlList = new Dictionary<MyPanelChild, int>();


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
        public PanelItem this[int rowIndex]
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
                    PanelItem item = itemList.First(t => t.RowIndex == rowIndex);
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
                KeyValuePair<MyPanelChild, int> childItem = controlList.FirstOrDefault(t => t.Key.RowIndex == value);
                if (childItem.Key != null && childItem.Key.PanelItem.RowType == PanelRowType.ContentRow)
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
                PanelItem item = itemList.FirstOrDefault(t => t.RowType == PanelRowType.LoadMoreRow);
                if (!value && item != null)
                {
                    this.Remove(new List<int>() { item.RowIndex });
                }
                else if (value && item == null)
                {
                    PanelItem loadMoreItem = new PanelItem()
                    {
                        IsSelected = false,
                        RowIndex = itemList.Count,
                        RowType = PanelRowType.LoadMoreRow
                    };
                    InsertItem(loadMoreItem.RowIndex, loadMoreItem);
                    RefreshContent();
                    this.FirstDisplayedScrollingRowIndex = loadMoreItem.RowIndex;
                }
            }
        }
        #endregion

        #endregion

        #region 构造函数
        public MyPanelList()
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

        #region 公共方法

        #region 清空 Clear
        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            itemList.Clear();
            controlList.Clear();
            this.pnlContent.Controls.Clear();

            displayRectangleHeight = 0;
            this.pnlContent.VScrollValue = 0;
            myVScrollBar1.Value = 0;
            UpdateScrollbar();

            this.IsShowMore = isShowMore;
        }
        #endregion

        #region 添加行数据 AddItem

        #region 绑定数据 DataSource

        public void DataSource<T>(DataTable dt)
            where T : PanelItem, new()
        {
            this.Clear();
            if (isGroup && !dt.Columns.Contains(groupFieldName))
            {
                throw new Exception("该DataTable 没有包含" + groupFieldName + " 的列名！");
            }
            //行数
            int rowCount = 0;
            //日期值
            string lastDateValue = "";
            int rowIndex = 0;
            foreach (DataRow row in dt.Rows)
            {
                #region 增加统计数据
                if (isGroup)
                {
                    string date = DateTime.Parse(row[groupFieldName].ToString()).ToString("yyyy/MM");
                    if (!string.IsNullOrEmpty(lastDateValue) && lastDateValue != date)
                    {
                        GroupRowItem groupItem = new GroupRowItem()
                        {
                            IsSelected = false,
                            RowIndex = rowIndex,
                            RowType = PanelRowType.GroupRow,
                            GroupDateTime = DateTime.Parse(lastDateValue),
                            RowCount = rowCount
                        };
                        InsertItem(rowIndex, groupItem);
                        rowCount = 0;
                        rowIndex++;
                    }

                    PanelItem item = new T()
                    {
                        DataRow = row,
                        IsSelected = false,
                        RowIndex = rowIndex
                    };
                    InsertItem(rowIndex, item);

                    //最后一行
                    if (dt.Rows.IndexOf(row) == dt.Rows.Count - 1)
                    {
                        rowIndex++;
                        GroupRowItem groupItem = new GroupRowItem()
                        {
                            IsSelected = false,
                            RowIndex = rowIndex,
                            RowType = PanelRowType.GroupRow,
                            GroupDateTime = DateTime.Parse(lastDateValue),
                            RowCount = rowCount + 1
                        };
                        InsertItem(rowIndex, groupItem);
                        rowCount = 0;
                    }
                    rowCount++;
                    lastDateValue = date;
                }
                #endregion
                else
                {
                    PanelItem item = new T()
                    {
                        DataRow = row,
                        IsSelected = false,
                        RowIndex = rowIndex
                    };
                    InsertItem(rowIndex, item);
                }
                rowIndex++;
            }

            if (isShowMore)
            {
                PanelItem loadMoreItem = new PanelItem()
                {
                    IsSelected = false,
                    RowIndex = rowIndex,
                    RowType = PanelRowType.LoadMoreRow
                };
                InsertItem(rowIndex, loadMoreItem);
            }

            if (controlList.Count > 0)
            {
                item_MouseClick(controlList.First().Key, null);
            }
            this.UpdateScrollbar();
        }

        /// <summary>
        /// 绑定数据 DataSource 
        /// </summary>
        //public DataTable DataSource
        //{
        //    set
        //    {
        //        this.Clear();
        //        foreach (DataRow row in value.Rows)
        //        {
        //            PanelItem item = new PanelItem()
        //            {
        //                DataRow = row,
        //                IsSelected = false,
        //                RowIndex = value.Rows.IndexOf(row)
        //            };
        //            this.AddItem(item);
        //        }
        //        if (controlList.Count > 0)
        //        {
        //            item_MouseClick(controlList.First().Key, null);
        //        }
        //        this.UpdateScrollbar();
        //    }
        //}
        #endregion

        /// <summary>
        /// 添加行数据，会自动合并到组
        /// </summary>
        /// <param name="item"></param>
        public void Add(PanelItem item)
        {
            /* 如果有分组，则自动排序后，更改分组内容
             *           如果是一个新的分组，则要创建一个分组合计
             * 如果没有分组，则添加在最后
             * 
             * 如果有加载项，则要保证加载项在最后
             * 
             */
            if (item.DataRow == null)
                throw new Exception("没有数据！");
            if (isGroup && !item.DataRow.Table.Columns.Contains(groupFieldName))
            {
                throw new Exception("该DataTable 没有包含" + groupFieldName + " 的列名！");
            }
            if (SetItemTemplate == null)
                throw new Exception("必须启用 SetItemTemplate 事件");
            item.RowType = PanelRowType.ContentRow;
            item.RowIndex = itemList.Count;
            item.IsSelected = true;

            if (isGroup)
            {
                AddByGroup(item);
            }
            #region 不分组
            else
            {
                //添加到最后
                int rowIndex = itemList.Count;
                if (isShowMore)
                {
                    var find = itemList.First(t => t.RowType == PanelRowType.LoadMoreRow);
                    if (find != null)
                    {
                        rowIndex = find.RowIndex;
                        find.RowIndex += 1;

                        item.RowIndex = rowIndex;
                    }
                }

                InsertItem(rowIndex, item);
            }
            #endregion
            RefreshContent();
            this.FirstDisplayedScrollingRowIndex = item.RowIndex;
        }

        private void InsertItem(int rowIndex, PanelItem item)
        {
            #region ContentRow
            if (item.RowType == PanelRowType.ContentRow)
            {
                if (isEqualHeight)
                {
                    item.Height = this.minRowHeight;
                    displayRectangleHeight += item.Height;
                    itemList.Insert(rowIndex, item);

                    if (controlList.Count < maxControlCount)
                    {
                        MyPanelChild childItem = SetItemTemplate(item);
                        AddControl(childItem);
                    }
                }
                else
                {
                    MyPanelChild childItem = SetItemTemplate(item);

                    Panel pnl = new Panel();
                    pnl.Controls.Add(childItem);

                    item.Height = childItem.Height;
                    displayRectangleHeight += item.Height;
                    itemList.Insert(rowIndex, item);

                    if (controlList.Count < maxControlCount)
                    {
                        AddControl(childItem);
                    }
                    pnl.Dispose();
                    pnl = null;
                }
            }
            #endregion
            #region GroupRow
            else if (item.RowType == PanelRowType.GroupRow)
            {
                GroupRowItem groupItem = item as GroupRowItem;
                MyPanelChild childItem = new GroupRow(groupItem.GroupDateTime, groupItem.RowCount);
                childItem.PanelItem = groupItem;
                childItem.RowIndex = item.RowIndex;
                item.Height = childItem.Height;
                displayRectangleHeight += item.Height;

                itemList.Insert(rowIndex, item);

                if (controlList.Count < maxControlCount)
                {
                    AddControl(childItem);
                }
            }
            #endregion
            #region LoadMoreRow
            else if (item.RowType == PanelRowType.LoadMoreRow)
            {
                LoadMoreRow childItem = new LoadMoreRow();
                childItem.PanelItem = item;
                childItem.RowIndex = item.RowIndex;
                item.Height = childItem.Height;
                displayRectangleHeight += item.Height;
                childItem.LoadMore += childItem_LoadMore;

                itemList.Insert(rowIndex, item);

                if (controlList.Count < maxControlCount)
                {
                    AddControl(childItem);
                }
            }
            #endregion
        }
        /// <summary>
        /// 批量追加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        public void Add<T>(DataTable dt)
            where T : PanelItem, new()
        {
            if (dt == null)
                throw new Exception("没有数据！");
            if (isShowMore && itemList.Count == 1)
            {
                this.DataSource<T>(dt);
                return;
            }
            if (isGroup && !dt.Columns.Contains(groupFieldName))
            {
                throw new Exception("该DataTable 没有包含" + groupFieldName + " 的列名！");
            }
            if (SetItemTemplate == null)
                throw new Exception("必须启用 SetItemTemplate 事件");

            if (isGroup)
            {
                ///重新统计排序信息
                ///
                foreach (DataRow row in dt.Rows)
                {
                    PanelItem item = new T()
                    {
                        DataRow = row,
                        IsSelected = false,
                        RowType = PanelRowType.ContentRow
                    };
                    AddByGroup(item);
                }
            }
            else
            {
                int rowIndex = itemList.Count;

                if (isShowMore)
                {
                    var find = itemList.First(t => t.RowType == PanelRowType.LoadMoreRow);
                    if (find != null)
                    {
                        rowIndex = find.RowIndex;
                        find.RowIndex += dt.Rows.Count;

                        KeyValuePair<MyPanelChild, int> pnlChild = controlList.FirstOrDefault(t => t.Key.RowIndex == rowIndex);
                        if (pnlChild.Key != null)
                        {
                            controlList[pnlChild.Key] = find.RowIndex;
                            pnlChild.Key.RowIndex = find.RowIndex;
                        }
                    }
                }

                foreach (DataRow row in dt.Rows)
                {
                    PanelItem item = new T()
                    {
                        DataRow = row,
                        IsSelected = false,
                        RowIndex = rowIndex,
                        RowType = PanelRowType.ContentRow
                    };
                    InsertItem(rowIndex, item);
                    rowIndex++;
                }
            }
            RefreshContent();
        }

        private void AddByGroup(PanelItem item)
        {
            int rowIndex = itemList.Count;
            PanelItem find = itemList.FirstOrDefault(t => t.RowType == PanelRowType.ContentRow && DateTime.Parse(t.DataRow[groupFieldName].ToString()) > DateTime.Parse(item.DataRow[groupFieldName].ToString()));
            if (find != null)
            {
                rowIndex = find.RowIndex;
                var tmpFind = itemList.FirstOrDefault(t => t.RowIndex == rowIndex - 1);
                if (tmpFind != null && tmpFind.RowType == PanelRowType.GroupRow)
                    rowIndex -= 1;
            }
            else if (find == null)
            {//查询组是否存在
                List<GroupRowItem> groupItemList = itemList.Where(t => t.RowType == PanelRowType.GroupRow).Cast<GroupRowItem>().ToList();
                foreach (GroupRowItem gpItem in groupItemList)
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
                find = itemList.FirstOrDefault(t => t.RowType == PanelRowType.ContentRow && DateTime.Parse(t.DataRow[groupFieldName].ToString()) < DateTime.Parse(item.DataRow[groupFieldName].ToString()));
                if (find != null)
                {
                    rowIndex = find.RowIndex;
                    var tmpFind = itemList.FirstOrDefault(t => t.RowIndex == rowIndex + 1);
                    if (tmpFind != null && tmpFind.RowType == PanelRowType.GroupRow)
                        rowIndex += 1;
                }
            }
            else if (find == null)
            {//查询组是否存在
                List<GroupRowItem> groupItemList = itemList.Where(t => t.RowType == PanelRowType.GroupRow).Cast<GroupRowItem>().ToList();
                foreach (GroupRowItem gpItem in groupItemList)
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
                    find = itemList.First(t => t.RowType == PanelRowType.LoadMoreRow);
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
                GroupRowItem groupItem = new GroupRowItem()
                {
                    IsSelected = false,
                    RowIndex = rowIndex + 1,
                    RowType = PanelRowType.GroupRow,
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
                    if (itemList[i].RowType == PanelRowType.GroupRow && !isUpdateTotal)
                    {//找到最近的一个统计行
                        GroupRowItem groupItem = itemList[i] as GroupRowItem;
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

        /// <summary>
        /// 插入指定的位置
        /// 此方法不适用分组的情况
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="item"></param>
        public void Insert(int rowIndex, PanelItem item)
        {
            if (item.DataRow == null)
                throw new Exception("没有数据！");
            if (isGroup && !item.DataRow.Table.Columns.Contains(groupFieldName))
            {
                throw new Exception("该DataTable 没有包含" + groupFieldName + " 的列名！");
            }
            if (SetItemTemplate == null)
                throw new Exception("必须启用 SetItemTemplate 事件");
            if (isGroup)
            {
                Add(item);
            }
            else
            {
                item.RowType = PanelRowType.ContentRow;
                item.RowIndex = rowIndex;
                for (int i = rowIndex; i < itemList.Count; i++)
                {
                    itemList[i].RowIndex += 1;
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
                RefreshContent();
                this.FirstDisplayedScrollingRowIndex = item.RowIndex;
            }
        }

        #region 添加行数据 --del
        ///// <summary>
        ///// 添加行数据
        ///// </summary>
        ///// <param name="item"></param>
        //private void AddItem(PanelItem item)
        //{
        //    //if (SetItemTemplate == null)
        //    //    throw new Exception("必须启用 SetItemTemplate 事件");
        //    #region ContentRow
        //    if (item.RowType == PanelRowType.ContentRow)
        //    {
        //        if (isEqualHeight)
        //        {
        //            item.Height = this.minRowHeight;
        //            displayRectangleHeight += item.Height;
        //            itemList.Add(item);

        //            if (controlList.Count < maxControlCount)
        //            {
        //                MyPanelChild childItem = SetItemTemplate(item);
        //                AddControl(childItem);
        //            }
        //        }
        //        else
        //        {
        //            MyPanelChild childItem = SetItemTemplate(item);

        //            Panel pnl = new Panel();
        //            pnl.Controls.Add(childItem);

        //            item.Height = childItem.Height;
        //            displayRectangleHeight += item.Height;
        //            itemList.Add(item);

        //            if (controlList.Count < maxControlCount)
        //            {
        //                AddControl(childItem);
        //            }
        //            pnl.Dispose();
        //            pnl = null;
        //        }
        //    }
        //    #endregion
        //    #region GroupRow
        //    else if (item.RowType == PanelRowType.GroupRow)
        //    {
        //        GroupRowItem groupItem = item as GroupRowItem;
        //        MyPanelChild childItem = new GroupRow(groupItem.GroupDateTime, groupItem.RowCount);
        //        childItem.PanelItem = groupItem;
        //        childItem.RowIndex = item.RowIndex;
        //        item.Height = childItem.Height;
        //        displayRectangleHeight += item.Height;

        //        itemList.Add(item);

        //        if (controlList.Count < maxControlCount)
        //        {
        //            AddControl(childItem);
        //        }
        //    }
        //    #endregion
        //    #region LoadMoreRow
        //    else if (item.RowType == PanelRowType.LoadMoreRow)
        //    {
        //        LoadMoreRow childItem = new LoadMoreRow();
        //        childItem.PanelItem = item;
        //        childItem.RowIndex = item.RowIndex;
        //        item.Height = childItem.Height;
        //        displayRectangleHeight += item.Height;
        //        childItem.LoadMore += childItem_LoadMore;

        //        itemList.Add(item);

        //        if (controlList.Count < maxControlCount)
        //        {
        //            AddControl(childItem);
        //        }
        //    }
        //    #endregion
        //} 
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
            this.pnlContent.Focus();
        }
        #endregion

        #endregion

        #region 添加控件 AddControl
        /// <summary>
        /// 添加控件
        /// </summary>
        /// <param name="item"></param>
        private void AddControl(MyPanelChild item, bool isSetTop = true)
        {
            item.DefaultColor = defaultColor;
            item.MouseEnterColor = mouseEnterColor;
            item.SelectedColor = selectedColor;
            item.Width = this.pnlContent.Width;
            item.Anchor = (AnchorStyles)(AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
            if (isSetTop)
            {
                if (controlList.Count == 0 || item.RowIndex == 0)
                {
                    item.Top = 0;
                }
                else
                {
                    MyPanelChild lastItem = controlList.First(t => t.Value == item.RowIndex - 1).Key;
                    item.Top = lastItem.Top + lastItem.Height;
                }
            }

            controlList.Add(item, item.RowIndex);
            this.pnlContent.Controls.Add(item);

            if (item.PanelItem.RowType == PanelRowType.ContentRow)
            {
                item.MouseClick += item_MouseClick;
                item.MouseEnter += Item_MouseEnter;
                item.MouseLeave += Item_MouseLeave;
                item.MouseMove += Item_MouseMove;
            }
        }

        private void Item_MouseMove(object sender, MouseEventArgs e)
        {
            isActiveMouseEvent = true;
        }

        private void Item_MouseLeave(object sender, EventArgs e)
        {
            MyPanelChild pnl = sender as MyPanelChild;
            if (!pnl.IsSelected)
                pnl.BackColor = defaultColor;
        }

        private void Item_MouseEnter(object sender, EventArgs e)
        {
            MyPanelChild pnl = sender as MyPanelChild;
            if (isActiveMouseEvent && !pnl.IsSelected)
                pnl.BackColor = mouseEnterColor;
        }

        void item_MouseClick(object sender, MouseEventArgs e)
        {
            MyPanelChild pnl = sender as MyPanelChild;
            PanelItem tmpItem = itemList.First(t => t.RowIndex == pnl.RowIndex);
            bool isFocus = tmpItem.IsFocus;
            pnl.Parent.Focus();//只有点击的才是Focus,Focus永远只有一个

            #region Control + 鼠标
            if (multiSelect && (Control.ModifierKeys & Keys.Control) == Keys.Control)// CTRL is pressed    
            {
                if (pnl.RowIndex < itemList.Count && pnl.RowIndex >= 0)
                {
                    foreach (PanelItem item in itemList)
                    {
                        if (item.RowIndex != pnl.RowIndex && item.IsFocus)
                        {
                            item.IsFocus = false;
                            break;
                        }
                    }
                    PanelItem pnlItem = itemList.First(t => t.RowIndex == pnl.RowIndex);
                    pnlItem.IsFocus = true;
                    pnlItem.IsSelected = !pnlItem.IsSelected;
                    pnl.IsSelected = pnlItem.IsSelected;
                }
            }
            #endregion
            #region Shift + 鼠标
            else if (multiSelect && (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {//范围是从Focus=true 到这个为止
                PanelItem focusItem = null;
                foreach (PanelItem item in itemList)
                {
                    if (item.IsFocus)
                    {
                        focusItem = item;
                        break;
                    }
                }
                if (focusItem == null)
                {
                    if (pnl.RowIndex < itemList.Count && pnl.RowIndex >= 0)
                    {
                        foreach (PanelItem item in itemList)
                        {
                            if (item.RowIndex != pnl.RowIndex && item.IsFocus)
                            {
                                item.IsFocus = false;
                                break;
                            }
                        }

                        PanelItem pnlItem = itemList.First(t => t.RowIndex == pnl.RowIndex);
                        pnlItem.IsFocus = true;
                        pnlItem.IsSelected = pnl.IsSelected;
                    }
                }
                else
                {
                    int startIndex = 0;
                    int endIndex = 0;
                    if (focusItem.RowIndex > pnl.RowIndex)
                    {
                        startIndex = pnl.RowIndex;
                        endIndex = focusItem.RowIndex;
                    }
                    else
                    {
                        startIndex = focusItem.RowIndex;
                        endIndex = pnl.RowIndex;
                    }
                    foreach (MyPanelChild item in controlList.Keys)
                    {
                        if (item.RowIndex != pnl.RowIndex && item.IsSelected)
                        {
                            item.IsSelected = false;
                        }
                    }
                    foreach (PanelItem item in itemList)
                    {
                        if (item.RowIndex != pnl.RowIndex && item.IsSelected)
                        {
                            item.IsSelected = false;
                        }
                    }

                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        PanelItem pnlItem = itemList.First(t => t.RowIndex == i);
                        if (pnlItem.RowType == PanelRowType.ContentRow)
                        {
                            pnlItem.IsSelected = true;

                            KeyValuePair<MyPanelChild, int> find = controlList.FirstOrDefault(t => t.Key.RowIndex == i);
                            if (find.Key != null)
                            {
                                find.Key.IsSelected = true;
                            }
                        }
                    }
                }
            }
            #endregion
            #region 单选
            else
            {
                ClearSelectedItem(pnl.RowIndex);
                if (pnl.Top < 0)
                {
                    this.myVScrollBar1.Value = GetItemHeightByRowIndex(tmpItem.RowIndex);
                    this.pnlContent.VScrollValue = this.myVScrollBar1.Value;
                    ScrollItem(true);
                }
                else if (pnl.Top + pnl.Height > this.Height)
                {
                    this.myVScrollBar1.Value += pnl.Top + pnl.Height - this.Height;//加上隐藏的部分
                    this.pnlContent.VScrollValue = this.myVScrollBar1.Value;
                    ScrollItem(false);
                }
            }
            #endregion
            tmpItem = itemList.First(t => t.RowIndex == pnl.RowIndex);
            if (SelectionChanged != null && isFocus != tmpItem.IsFocus)
            {
                PanelItem pnlItem = itemList.First(t => t.RowIndex == pnl.RowIndex);
                SelectionChanged(pnlItem);
            }
        }

        #endregion

        #region 删除数据
        public void Remove(List<int> rowIndexList)
        {
            /* 判断删除的数据是否为 绑定的控件
            * 找Focus=true的上一个或下一个为选中项
            */

            #region 处理数据行
            PanelItem focusItem = null;
            foreach (PanelItem item in itemList)
            {
                if (item.IsFocus)
                {
                    focusItem = item;
                    break;
                }
            }
            rowIndexList = rowIndexList.OrderBy(t => t).ToList();
            //删除数据行
            //如果选择任意行进行删除
            foreach (int item in rowIndexList)
            {
                #region 处理数据行
                PanelItem delItem = null;
                foreach (PanelItem pnlItem in itemList)
                {
                    if (pnlItem.RowIndex == item)
                    {
                        delItem = pnlItem;
                        break;
                    }
                }
                if (delItem != null)
                {
                    displayRectangleHeight -= delItem.Height;
                    itemList.Remove(delItem);
                }
                #endregion
            }
            //设置选中项 ,如果删除的没有包含已选中项，则不用设置
            if (focusItem != null && rowIndexList.Contains(focusItem.RowIndex) && itemList.Count > 0)
            {
                bool isFind = false;
                int searchIndex = focusItem.RowIndex + 1;
                foreach (PanelItem item in itemList)
                {
                    if (item.RowIndex >= searchIndex)
                    {
                        if (item.RowType != PanelRowType.ContentRow)
                        {
                            searchIndex++;
                            continue;
                        }
                        item.IsSelected = true;
                        item.IsFocus = true;
                        isFind = true;
                        break;
                    }
                }
                if (!isFind)
                {
                    searchIndex = focusItem.RowIndex - 1;
                    for (int i = itemList.Count - 1; i >= 0; i--)
                    {
                        PanelItem item = itemList[i];
                        if (item.RowIndex <= searchIndex)
                        {
                            if (item.RowType != PanelRowType.ContentRow)
                            {
                                searchIndex--;
                                continue;
                            }
                            item.IsSelected = true;
                            item.IsFocus = true;
                            break;
                        }
                    }
                }
            }


            #region 更新统计内容
            if (isGroup)
            {
                //某个分组删一部分(更新统计信息)或全删(则要删除统计行)
                //行数
                int rowCount = 0;
                int rowIndex = 0;
                List<PanelItem> delList = new List<PanelItem>();

                for (int i = 0; i < itemList.Count; i++)
                {
                    PanelItem item = itemList[i];
                    if (item.RowType == PanelRowType.ContentRow)
                    {
                        rowCount++;
                    }
                    else if (item.RowType == PanelRowType.GroupRow)
                    {
                        GroupRowItem groupItem = item as GroupRowItem;
                        if (rowCount > 0)
                        {
                            groupItem.RowCount = rowCount;
                        }
                        else
                        {
                            rowIndex -= 1;
                            delList.Add(item);
                        }
                        rowCount = 0;
                    }
                    item.RowIndex = rowIndex;
                    rowIndex++;
                }
                //删除统计行
                foreach (PanelItem item in delList)
                {
                    displayRectangleHeight -= item.Height;
                    itemList.Remove(item);
                }
            }
            else
            {
                int startIndex = rowIndexList[0];
                //更新索引
                foreach (PanelItem item in itemList)
                {
                    if (item.RowIndex > rowIndexList[0])
                    {
                        item.RowIndex = startIndex;
                        startIndex++;
                    }
                }
            }
            #endregion

            #endregion

            #region 重新给控件赋值
            //重新给控件赋值
            if (itemList.Count > controlList.Count)
            {
                List<MyPanelChild> childList = controlList.Select(t => t.Key).ToList();
                for (int i = 0; i < controlList.Count; i++)
                {
                    MyPanelChild childItem = childList[i];
                    PanelItem item = itemList.First(t => t.RowIndex == i);
                    if (childItem.PanelItem.RowType != item.RowType)
                    {
                        this.pnlContent.Controls.Remove(childItem);
                        this.controlList.Remove(childItem);
                        if (item.RowType == PanelRowType.ContentRow)
                        {
                            MyPanelChild newItem = this.SetItemTemplate(item);
                            newItem.IsSelected = item.IsSelected;
                            this.AddControl(newItem, true);
                            childItem = newItem;
                        }
                        else if (item.RowType == PanelRowType.GroupRow)
                        {
                            GroupRowItem groupItem = item as GroupRowItem;
                            MyPanelChild newItem = new GroupRow(groupItem.GroupDateTime, groupItem.RowCount);
                            newItem.RowIndex = item.RowIndex;
                            newItem.PanelItem = groupItem;
                            this.AddControl(newItem, true);
                            childItem = newItem;
                        }
                        else if (item.RowType == PanelRowType.LoadMoreRow)
                        {
                            LoadMoreRow newItem = new LoadMoreRow();
                            newItem.LoadMore += childItem_LoadMore;
                            newItem.RowIndex = item.RowIndex;
                            newItem.PanelItem = item;
                            this.AddControl(newItem, true);
                            childItem = newItem;
                        }
                    }
                    else
                    {
                        controlList[childList[i]] = i;
                        GetNewInfo(childList[i], i);
                        childList[i].RefreshData();
                    }
                }
            }
            else
            {
                List<MyPanelChild> delChild = new List<MyPanelChild>();
                List<MyPanelChild> childList = controlList.Select(t => t.Key).ToList();

                for (int i = 0; i < controlList.Count; i++)
                {
                    if (i < itemList.Count)
                    {
                        controlList[childList[i]] = i;

                        MyPanelChild childItem = childList[i];
                        PanelItem item = itemList.First(t => t.RowIndex == i);
                        if (childItem.PanelItem.RowType != item.RowType)
                        {
                            this.pnlContent.Controls.Remove(childItem);
                            this.controlList.Remove(childItem);
                            if (item.RowType == PanelRowType.ContentRow)
                            {
                                MyPanelChild newItem = this.SetItemTemplate(item);
                                newItem.IsSelected = item.IsSelected;
                                this.AddControl(newItem, true);
                                childItem = newItem;
                            }
                            else if (item.RowType == PanelRowType.GroupRow)
                            {
                                GroupRowItem groupItem = item as GroupRowItem;
                                MyPanelChild newItem = new GroupRow(groupItem.GroupDateTime, groupItem.RowCount);
                                newItem.RowIndex = item.RowIndex;
                                newItem.PanelItem = groupItem;
                                this.AddControl(newItem, true);
                                childItem = newItem;
                            }
                            else if (item.RowType == PanelRowType.LoadMoreRow)
                            {
                                LoadMoreRow newItem = new LoadMoreRow();
                                newItem.LoadMore += childItem_LoadMore;
                                newItem.RowIndex = item.RowIndex;
                                newItem.PanelItem = item;
                                this.AddControl(newItem, true);
                                childItem = newItem;
                            }
                        }
                        else
                        {
                            GetNewInfo(childList[i], i);
                            childList[i].RefreshData();
                        }
                    }
                    else
                    {
                        delChild.Add(childList[i]);
                    }
                }
                foreach (var item in delChild)
                {
                    this.controlList.Remove(item);
                    this.pnlContent.Controls.Remove(item);
                }
            }
            #endregion

            UpdateScrollbar();
            ContentLengthChange();
            this.pnlContent.Focus();
        }
        #endregion

        #region 给控件赋值
        /// <summary>
        /// 给控件赋值
        /// </summary>
        /// <param name="childItem"></param>
        /// <param name="rowIndex"></param>
        private void GetNewInfo(MyPanelChild childItem, int rowIndex)
        {
            if (rowIndex >= 0 && rowIndex <= itemList.Count - 1)
            {
                for (int i = 0; i < itemList.Count; i++)
                {
                    if (itemList[i].RowIndex == rowIndex)
                    {
                        PanelItem item = itemList[i];
                        childItem.PanelItem = item;
                        childItem.IsSelected = item.IsSelected;
                        childItem.DataRow = item.DataRow;
                        childItem.Height = item.Height;
                        childItem.RowIndex = rowIndex;
                        break;
                    }
                }
            }
        }
        #endregion

        #region 返回选中的数据 SelectedItems
        /// <summary>
        /// 返回选中的数据
        /// </summary>
        /// <returns></returns>
        public List<PanelItem> SelectedItems()
        {
            List<PanelItem> items = new List<PanelItem>();

            foreach (PanelItem item in itemList)
            {
                if (item.IsSelected)
                {
                    items.Add(item);
                }
            }
            return items;
        }
        #endregion

        #region 内容修改后更新
        /// <summary>
        /// 内容修改后更新
        /// </summary>
        /// <param name="rowIndex"></param>
        public void Refresh(int rowIndex)
        {
            KeyValuePair<MyPanelChild, int> find = controlList.FirstOrDefault(t => t.Value == rowIndex);
            if (find.Key != null)
            {
                PanelItem item = itemList.First(t => t.RowIndex == rowIndex);
                find.Key.DataRow = item.DataRow;
                find.Key.IsSelected = item.IsSelected;

                bool isSizeChange = false;
                if (item.Height != find.Key.Height)
                    isSizeChange = true;

                displayRectangleHeight += find.Key.Height - item.Height;

                item.Height = find.Key.Height;
                find.Key.RefreshData();
                item_MouseClick(find.Key, null);

                if (isSizeChange)
                    ContentLengthChange();
            }
        }
        #endregion

        #region 内容变化后需调用此方法
        /// <summary>
        /// 内容变化后需调用此方法
        /// </summary>
        private void UpdateScrollbar()
        {
            this.pnlContent.DisplayRectangleHeight = displayRectangleHeight;
            this.myVScrollBar1.UpdateScrollbar();
        }
        #endregion

        #region 滚动到最底部 -del
        ///// <summary>
        ///// 滚动到最底部
        ///// </summary>
        //public void ScrollToCaret()
        //{
        //    int value = displayRectangleHeight - this.pnlContent.Height;
        //    if (value < 0)
        //        value = 0;
        //    this.pnlContent.VScrollValue = value;
        //    UpdateScrollbar();
        //    ScrollItem(false);
        //}
        #endregion

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
            ContentLengthChange();
        }
        #endregion

        #region 滚动条滚动事件
        void myVScrollBar1_Scroll(object sender, EventArgs e)
        {
            //解决快速移动闪屏问题
            // https://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=ZH-CN&k=k(System.Windows.Forms.Control.Update);k(TargetFrameworkMoniker-.NETFramework,Version%3Dv3.5);k(DevLang-csharp)&rd=true
            Thread t = new Thread(() =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    isActiveMouseEvent = false;
                    ScrollItem(this.myVScrollBar1.IsMoveUp);
                    ////解决快速移动闪屏问题
                    this.Invalidate(true);
                    this.Update();
                    this.pnlContent.Focus();
                });
            });
            t.Start();
        }
        #endregion

        #region 处理快捷键 ProcessCmdKey
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                #region Up
                case Keys.Up://向上滚动
                    isActiveMouseEvent = false;
                    //清除所有选中项，选中Focus=true的上一个
                    if (itemList[0].IsFocus)
                    {
                        this.myVScrollBar1.Value = 0;
                        this.pnlContent.VScrollValue = this.myVScrollBar1.Value;
                        ScrollItem(true);
                        return true;
                    }

                    //找到最后一次点击的控件
                    PanelItem tmpItem = null;
                    foreach (PanelItem pnlItem in itemList)
                    {
                        if (pnlItem.IsFocus)
                        {
                            tmpItem = pnlItem;
                            break;
                        }
                    }
                    if (tmpItem != null)
                    {
                        int searchIndex = tmpItem.RowIndex - 1;
                        KeyValuePair<MyPanelChild, int> find = controlList.FirstOrDefault(t => t.Key.RowIndex == searchIndex);
                        while (find.Key != null && find.Key.PanelItem.RowType != PanelRowType.ContentRow)
                        {
                            searchIndex--;
                            find = controlList.FirstOrDefault(t => t.Key.RowIndex == searchIndex);
                        }
                        ClearSelectedItem(searchIndex);

                        if (find.Key == null || find.Key.Top < 0 || find.Key.Top > this.Height)
                        {
                            //这里要直接计算Value 的位置
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
                            if (firstRowIndex < tmpItem.RowIndex - 1)//这里判断 在选中项不再控件范围内滚动有问题
                            { //重新计算滚动条的Value
                                int totalHeight = 0;
                                //计算得出从此处开始，向上显示，能显示多少个
                                int displayRowCount = GetDisplayRowCount(tmpItem.RowIndex, ArrowDirection.Up, out totalHeight);
                                //计算 从这个开始，能显示的高度
                                GetDisplayRowCount(tmpItem.RowIndex - displayRowCount, ArrowDirection.Down, out totalHeight);
                                this.myVScrollBar1.Value = GetItemHeightByRowIndex(tmpItem.RowIndex - displayRowCount) + totalHeight - this.Height;
                                this.pnlContent.VScrollValue = this.myVScrollBar1.Value;
                            }
                            else
                            {
                                this.myVScrollBar1.Value = GetItemHeightByRowIndex(tmpItem.RowIndex - 1);
                                this.pnlContent.VScrollValue = this.myVScrollBar1.Value;
                            }
                            ScrollItem(true);
                            if (SelectionChanged != null)
                                SelectionChanged(tmpItem);
                        }
                    }
                    break;
                #endregion

                #region Down
                case Keys.Down://向下滚动
                    isActiveMouseEvent = false;
                    if (itemList[itemList.Count - 1].IsFocus)
                    {
                        this.myVScrollBar1.Value = displayRectangleHeight - this.pnlContent.Height;
                        this.pnlContent.VScrollValue = this.myVScrollBar1.Value;
                        ScrollItem(false);
                        return true;
                    }

                    //找到最后一次点击的控件
                    tmpItem = null;
                    foreach (PanelItem pnlItem in itemList)
                    {
                        if (pnlItem.IsFocus)
                        {
                            tmpItem = pnlItem;
                            break;
                        }
                    }
                    if (tmpItem != null)
                    {
                        int searchIndex = tmpItem.RowIndex + 1;
                        KeyValuePair<MyPanelChild, int> find = controlList.FirstOrDefault(t => t.Key.RowIndex == searchIndex);
                        while (find.Key != null && find.Key.PanelItem.RowType != PanelRowType.ContentRow)
                        {
                            searchIndex++;
                            find = controlList.FirstOrDefault(t => t.Key.RowIndex == searchIndex);
                        }
                        if (searchIndex < itemList.Count)//预防最后一行是统计行或查找更多行
                            ClearSelectedItem(searchIndex);
                        if (find.Key == null || find.Key.Top < 0 || find.Key.Top + tmpItem.Height > this.Height)
                        {
                            int[] indexArr = controlList.Values.OrderBy(t => t).ToArray();
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
                            if (firstRowIndex < tmpItem.RowIndex - 1)//这里判断 在选中项不再控件范围内滚动有问题
                            { //重新计算滚动条的Value
                                int totalHeight = 0;
                                //计算得出从此处开始，向上显示，能显示多少个
                                int displayRowCount = GetDisplayRowCount(tmpItem.RowIndex, ArrowDirection.Up, out totalHeight);
                                //计算 从这个开始，能显示的高度
                                //GetDisplayRowCount(tmpItem.RowIndex - displayRowCount + 1, ArrowDirection.Down, out totalHeight);
                                this.myVScrollBar1.Value = GetItemHeightByRowIndex(tmpItem.RowIndex - displayRowCount + 2) + totalHeight - this.Height;
                                this.pnlContent.VScrollValue = this.myVScrollBar1.Value;
                            }
                            else if (find.Key != null && find.Key.Top + tmpItem.Height > this.Height)
                            {//为了完全显示内容
                                this.myVScrollBar1.Value += find.Key.Top + tmpItem.Height - this.Height;//加上隐藏的部分
                                this.pnlContent.VScrollValue = this.myVScrollBar1.Value;
                            }
                            else
                            {
                                this.myVScrollBar1.Value = GetItemHeightByRowIndex(tmpItem.RowIndex + 1);
                                this.pnlContent.VScrollValue = this.myVScrollBar1.Value;
                            }
                            ScrollItem(false);
                        }
                        if (SelectionChanged != null)
                            SelectionChanged(tmpItem);
                    }
                    break;
                #endregion

                #region Control + A
                case Keys.Control | Keys.A://全选
                    if (multiSelect)
                    {
                        foreach (var item in controlList)
                        {
                            if (!item.Key.IsSelected && item.Key.PanelItem.RowType == PanelRowType.ContentRow)
                                item.Key.IsSelected = true;
                        }
                        foreach (var item in itemList)
                        {
                            if (!item.IsSelected && item.RowType == PanelRowType.ContentRow)
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

        #region 私有方法

        #region 重新绑定控件的内容
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

        #region 滚动条滚动
        /// <summary>
        /// 滚动条滚动
        /// </summary>
        private void ScrollItem(bool? isUp)
        {
            int startIndex = GetRowIndexByScrollValue(myVScrollBar1.Value);
            int endIndex = startIndex + controlList.Count - 1;
            if (controlList.Count == itemList.Count)
            {//当控件内容个数小于最大个数时，不需要调整行索引
                startIndex = 0;
                endIndex = controlList.Count - 1;
            }
            else if (endIndex >= itemList.Count)
            {//当endIndex > itemList.Count 即endIndex超过内容行数
                endIndex = itemList.Count - 1;
                startIndex = endIndex - (controlList.Count - 1);
            }
            int tmpStart = startIndex;

            int[] indexArr = null;
            if (isUp != null && isUp.Value)
            {
                indexArr = controlList.Values.OrderByDescending(t => t).ToArray();
            }
            else
            {
                indexArr = controlList.Values.OrderBy(t => t).ToArray();
            }

            for (int i = 0; i < indexArr.Length; i++)
            {
                MyPanelChild childItem = controlList.First(t => t.Value == indexArr[i]).Key;
                if (controlList[childItem] < startIndex || controlList[childItem] > endIndex)
                {
                    while (controlList.ContainsValue(tmpStart) && tmpStart < endIndex)
                    {
                        tmpStart += 1;
                    }
                    //如果替换的这个是统计行，与原本的不一致，则要删除当前控件，删除controlList中的对象，重新添加
                    if (!IsChangeControlType(tmpStart, ref childItem))
                    {
                        controlList[childItem] = tmpStart;
                        childItem.RowIndex = tmpStart;

                        if (childItem.RowIndex < itemList.Count)
                        {
                            GetNewInfo(childItem, childItem.RowIndex);
                        }
                        childItem.RefreshData();
                    }
                }
                else
                {
                    IsChangeControlType(childItem.RowIndex, ref childItem);
                }
                childItem.Top = GetItemHeightByRowIndex(childItem.RowIndex) - this.myVScrollBar1.Value;
            }
        }
        /// <summary>
        /// 是否需要替换控件类型
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="childItem"></param>
        /// <returns></returns>
        private bool IsChangeControlType(int rowIndex, ref MyPanelChild childItem)
        {
            PanelItem item = itemList.First(t => t.RowIndex == rowIndex);
            if (childItem.PanelItem.RowType != item.RowType)
            {
                this.pnlContent.Controls.Remove(childItem);
                this.controlList.Remove(childItem);
                if (item.RowType == PanelRowType.ContentRow)
                {
                    MyPanelChild newItem = this.SetItemTemplate(item);
                    newItem.IsSelected = item.IsSelected;
                    this.AddControl(newItem, false);
                    childItem = newItem;
                }
                else if (item.RowType == PanelRowType.GroupRow)
                {
                    GroupRowItem groupItem = item as GroupRowItem;
                    MyPanelChild newItem = new GroupRow(groupItem.GroupDateTime, groupItem.RowCount);
                    newItem.RowIndex = item.RowIndex;
                    newItem.PanelItem = groupItem;
                    this.AddControl(newItem, false);
                    childItem = newItem;
                }
                else if (item.RowType == PanelRowType.LoadMoreRow)
                {
                    LoadMoreRow newItem = new LoadMoreRow();
                    newItem.LoadMore += childItem_LoadMore;
                    newItem.RowIndex = item.RowIndex;
                    newItem.PanelItem = item;
                    this.AddControl(newItem, false);
                    childItem = newItem;
                }
                return true;
            }
            return false;
        }

        #endregion

        #region 内容或界面大小 发生变化
        /// <summary>
        /// 内容或界面大小 发生变化
        /// </summary>
        private void ContentLengthChange()
        {
            /* 当滚动条拖到底，放大窗体后
             * 根据显示的最后一条，计算当前第一行应该显示的行号 => 滚动条 的值
             */
            if (controlList.Count > 0)
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
                int[] indexArr = controlList.Values.OrderBy(t => t).ToArray();
                int totalHeight = 0;
                int displayCount = GetDisplayRowCount(itemList[itemList.Count - 1].RowIndex, ArrowDirection.Up, out totalHeight);
                if (firstRowIndex <= (indexArr[indexArr.Count() - 1] - displayCount))
                { //重新计算滚动条的Value
                    int tmpValue = displayRectangleHeight - this.pnlContent.Height;
                    if (tmpValue < 0)
                        tmpValue = 0;
                    this.pnlContent.VScrollValue = tmpValue;
                }
                else
                {//这里是为了判断，当删除数据，删除最后面的数据时
                    int tmpValue = displayRectangleHeight - this.pnlContent.Height;
                    if (tmpValue < 0)
                        tmpValue = 0;
                    if (this.pnlContent.VScrollValue > tmpValue)
                        this.pnlContent.VScrollValue = tmpValue;
                }
                UpdateScrollbar();
                ScrollItem(null);
            }
        }

        #endregion

        #region 清除选中项
        /// <summary>
        /// 清除选中项
        /// </summary>
        /// <param name="rowIndex"></param>
        private void ClearSelectedItem(int rowIndex)
        {
            if (multiSelect)
            {//多选
                foreach (MyPanelChild item in controlList.Keys)
                {
                    if (item.RowIndex != rowIndex && item.IsSelected)
                    {
                        item.IsSelected = false;
                    }
                }
                foreach (PanelItem item in itemList)
                {
                    if (item.RowIndex != rowIndex && item.IsSelected)
                    {
                        item.IsSelected = false;
                    }
                    if (item.RowIndex != rowIndex && item.IsFocus)
                    {
                        item.IsFocus = false;
                    }
                }
            }
            else
            {
                foreach (MyPanelChild item in controlList.Keys)
                {
                    if (item.RowIndex != rowIndex && item.IsSelected)
                    {
                        item.IsSelected = false;
                        break;
                    }
                }

                foreach (PanelItem item in itemList)
                {
                    if (item.RowIndex != rowIndex && item.IsSelected)
                    {
                        item.IsSelected = false;
                        item.IsFocus = false;
                        break;
                    }
                }
            }

            KeyValuePair<MyPanelChild, int> find = controlList.FirstOrDefault(t => t.Value == rowIndex);
            if (find.Key != null)
            {
                find.Key.IsSelected = true;
            }
            if (rowIndex < itemList.Count && rowIndex >= 0)
            {
                PanelItem item = itemList.First(t => t.RowIndex == rowIndex);
                item.IsSelected = true;
                item.IsFocus = true;
            }

        }
        #endregion

        #region 通过滚动条Value 计算对应的 行索引
        /// <summary>
        /// 通过滚动条Value 计算对应的 行索引
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int GetRowIndexByScrollValue(int value)
        {
            int totalHeight = 0;
            int index = 0;
            foreach (var item in itemList)
            {
                totalHeight += item.Height;
                if (totalHeight >= value)
                {
                    index = item.RowIndex;
                    break;
                }
            }
            return index;
        }
        #endregion

        #region 根据行索引计算对应的高度
        /// <summary>
        /// 根据行索引计算对应的高度
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        private int GetItemHeightByRowIndex(int rowIndex)
        {
            int value = 0;
            foreach (var item in itemList)
            {
                if (item.RowIndex < rowIndex)
                {
                    value += item.Height;
                }
                else
                {
                    break;
                }
            }
            return value;
        }
        #endregion

        #region 从当前位置 向上或向下 计算 能显示多少个内容块
        /// <summary>
        /// 从当前位置 向上或向下 计算 能显示多少个内容块
        /// </summary>
        /// <param name="rowIndex">行号</param>
        /// <param name="direction">移动的方向</param>
        /// <param name="totalHeight">从当前位置，超过显示区域的高度</param>
        /// <returns></returns>
        private int GetDisplayRowCount(int rowIndex, ArrowDirection direction, out int totalHeight)
        {
            int rlt = 0;
            totalHeight = 0;
            if (direction == ArrowDirection.Up)
            {
                List<PanelItem> descList = itemList.OrderByDescending(t => t.RowIndex).ToList();
                foreach (var item in descList)
                {
                    if (item.RowIndex <= rowIndex)
                    {
                        totalHeight += item.Height;
                        rlt++;
                    }
                    if (totalHeight >= this.Height)
                    {
                        break;
                    }
                }
            }
            else if (direction == ArrowDirection.Down)
            {
                foreach (var item in itemList)
                {
                    if (item.RowIndex >= rowIndex)
                    {
                        totalHeight += item.Height;
                        rlt++;
                    }
                    if (totalHeight >= this.Height)
                    {
                        break;
                    }
                }
            }
            return rlt;
        }
        #endregion

        #endregion


    }

    #region 分组对象
    /// <summary>
    /// 分组对象
    /// </summary>
    internal class GroupRowItem : PanelItem
    {
        /// <summary>
        /// 分组时间
        /// </summary>
        public DateTime GroupDateTime { get; set; }
        /// <summary>
        /// 行数
        /// </summary>
        public int RowCount { get; set; }
    }
    #endregion

}
