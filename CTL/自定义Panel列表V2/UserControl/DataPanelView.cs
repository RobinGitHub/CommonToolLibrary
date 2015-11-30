using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

/* 把Item 改成Row
 * 关键点是：要保证ItemList中行索引顺序
 */

namespace 自定义Panel列表V2
{
    /// <summary>
    /// 自定义Panel列表
    /// 
    /// </summary>
    public partial class DataPanelView : UserControl
    {
        #region API
        [DllImport("user32.dll")]

        private static extern IntPtr GetForegroundWindow();

        #endregion

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
        /// <summary>
        /// 双击内容项
        /// </summary>
        public event EventHandler ItemDoubleClick;
        /// <summary>
        /// 内容项大小改变
        /// </summary>
        public event EventHandler ItemHeightChanged;
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

        #region 组的位置
        /// <summary>
        /// 组的位置
        /// 上 = true 下= false
        /// 默认是下
        /// </summary>
        private bool groupRowIsTop = false;
        /// <summary>
        /// 组的位置
        /// 上 = true 下= false
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("其他"), Description("组的位置 上 = true 下= false")]
        public bool GroupRowIsTop
        {
            get { return groupRowIsTop; }
            set { groupRowIsTop = value; }
        }
        #endregion

        #region 在分组行最后是否显示组统计
        /// <summary>
        /// 在分组行最后是否显示组统计
        /// </summary>
        private bool isShowGroupTotal = true;
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("其他"), Description("在分组行最后是否显示组统计")]
        public bool IsShowGroupTotal
        {
            get { return isShowGroupTotal; }
            set { isShowGroupTotal = value; }
        }
        #endregion

        #region 分组排序方式
        /// <summary>
        /// 分组排序方式
        /// 默认正序
        /// </summary>
        private bool ascending = true;
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(true), Category("其他"), Description("分组排序方式")]
        public bool Ascending
        {
            get { return ascending; }
            set { ascending = value; }
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

        #region 分组统计要显示的内容 位置
        /// <summary>
        /// 分组统计要显示的内容 位置
        /// </summary>
        private ContentAlignment groupTitleTextAlign = ContentAlignment.MiddleCenter;
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(typeof(ContentAlignment), "MiddleCenter"), Category("其他"), Description("分组统计要显示的内容 位置")]
        public ContentAlignment GroupTitleTextAlign
        {
            get { return groupTitleTextAlign; }
            set { groupTitleTextAlign = value; }
        } 
        #endregion

        #region 分组统计要显示的内容 左边距
        /// <summary>
        /// 分组统计要显示的内容 左边距
        /// </summary>
        private int groupTitleLeft = 0;
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(0), Category("其他"), Description("分组统计要显示的内容 左边距")]
        public int GroupTitleLeft
        {
            get { return groupTitleLeft; }
            set { groupTitleLeft = value; }
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
            ContentLengthChange();
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
            this.pnlContent.Focus();
        }
        #endregion

        #region 滚动条滚动事件
        void myVScrollBar1_Scroll(object sender, EventArgs e)
        {
            ////解决快速移动闪屏问题
            //// https://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=ZH-CN&k=k(System.Windows.Forms.Control.Update);k(TargetFrameworkMoniker-.NETFramework,Version%3Dv3.5);k(DevLang-csharp)&rd=true
            Thread t = new Thread(() =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    isActiveMouseEvent = false;
                    ScrollItem(this.myVScrollBar1.IsMoveUp);
                    //解决快速移动闪屏问题
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

                    //找到最后一次点击的控件
                    DataPanelViewRow tmpItem = null;
                    foreach (DataPanelViewRow pnlItem in itemList)
                    {
                        if (pnlItem.IsFocus)
                        {
                            tmpItem = pnlItem;
                            break;
                        }
                    }
                    if (tmpItem != null)
                    {
                        //清除所有选中项，选中Focus=true的上一个
                        int tmpRowIndex = tmpItem.RowIndex - 1;
                        bool isHasContentRow = false;
                        while (tmpRowIndex >= 0)
                        {
                            if (itemList[tmpRowIndex].RowType == DataPanelRowType.ContentRow)
                            {
                                isHasContentRow = true;
                                break;
                            }
                            tmpRowIndex--;
                        }
                        if (!isHasContentRow)
                        {
                            this.myVScrollBar1.Value = 0;
                            this.pnlContent.VScrollValue = this.myVScrollBar1.Value;
                            ScrollItem(true);
                            return true;
                        }

                        int searchIndex = tmpItem.RowIndex - 1;
                        KeyValuePair<DataPanelViewRowControl, int> find = controlList.FirstOrDefault(t => t.Key.DataPanelRow.RowIndex == searchIndex);
                        while (find.Key != null && find.Key.DataPanelRow.RowType != DataPanelRowType.ContentRow)
                        {
                            searchIndex--;
                            find = controlList.FirstOrDefault(t => t.Key.DataPanelRow.RowIndex == searchIndex);
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
                    //判断最后一行是否是 加载更多
                    //判断最后一行是否是统计行
                    //判断第一点&第二点
                    //判断当前行后面没有内容行

                    //找到最后一次点击的控件
                    tmpItem = null;
                    foreach (DataPanelViewRow pnlItem in itemList)
                    {
                        if (pnlItem.IsFocus)
                        {
                            tmpItem = pnlItem;
                            break;
                        }
                    }
                    if (tmpItem != null)
                    {
                        int tmpRowIndex = tmpItem.RowIndex + 1;
                        bool isHasContentRow = false;
                        while (tmpRowIndex < itemList.Count)
                        {
                            if (itemList[tmpRowIndex].RowType == DataPanelRowType.ContentRow)
                            {
                                isHasContentRow = true;
                                break;
                            }
                            tmpRowIndex++;
                        }
                        if (!isHasContentRow)
                        {
                            this.myVScrollBar1.Value = displayRectangleHeight - this.pnlContent.Height;
                            this.pnlContent.VScrollValue = this.myVScrollBar1.Value;
                            ScrollItem(false);
                            return true;
                        }

                        int searchIndex = tmpItem.RowIndex + 1;
                        KeyValuePair<DataPanelViewRowControl, int> find = controlList.FirstOrDefault(t => t.Key.DataPanelRow.RowIndex == searchIndex);
                        while (find.Key != null && find.Key.DataPanelRow.RowType != DataPanelRowType.ContentRow)
                        {
                            searchIndex++;
                            find = controlList.FirstOrDefault(t => t.Key.DataPanelRow.RowIndex == searchIndex);
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

        #region 添加数据

        #region 数据源 该方法不适用分组 DataSource
        /// <summary>
        /// 数据源 该方法不适用分组
        /// </summary>
        public void DataSource<T>(DataTable dt)
            where T : DataPanelViewRow, new()
        {
            this.Clear();
            if (dt == null)
                return;
            if (isGroup)
            {
                throw new Exception("该方法不适用分组！");
            }
            int rowIndex = 0;
            foreach (DataRow row in dt.Rows)
            {
                DataPanelViewRow item = new T()
                {
                    DataRow = row,
                    IsSelected = false,
                    RowIndex = rowIndex
                };
                InsertItem(rowIndex, item);
                rowIndex++;
            }
            this.UpdateScrollbar();
            if (isShowMore)
            {
                DataPanelViewRow loadMoreItem = itemList.First(t => t.RowType == DataPanelRowType.LoadMoreRow);
                loadMoreItem.RowIndex = itemList.Count - 1;

                KeyValuePair<DataPanelViewRowControl, int> pnlChild = controlList.FirstOrDefault(t => t.Key.DataPanelRow.RowIndex == loadMoreItem.RowIndex);
                if (pnlChild.Key != null)
                {
                    controlList[pnlChild.Key] = loadMoreItem.RowIndex;
                }
                //调整位置
                ScrollItem(null);
            }

            if (controlList.Count > 0)
            {
                DataPanelViewRowControl dpvrc = controlList.First(t => t.Key.DataPanelRow.RowIndex == 0).Key;
                item_MouseClick(dpvrc, null);
            }
        }
        #endregion

        #region 数据源 支持分组 DataSource
        /// <summary>
        /// 数据源 支持分组
        /// </summary>
        /// <param name="dpvrList"></param>
        public void DataSource<T>(List<T> dpvrList)
            where T : DataPanelViewRow, new()
        {
            this.Clear();
            if (dpvrList == null || dpvrList.Count == 0)
                return;
            if (isGroup)
            {
                string lastValue = "";
                //行数
                int rowCount = 0;
                int rowIndex = 0;
                List<T> tmpList = null;
                if (ascending)
                    tmpList = dpvrList.OrderBy(t => t.GroupValue).ThenBy(t => t.GroupValueIndex).ToList();
                else
                    tmpList = dpvrList.OrderByDescending(t => t.GroupValue).ThenByDescending(t => t.GroupValueIndex).ToList();

                #region 统计行在上方
                if (groupRowIsTop)
                {//统计行在上方
                    var groupList = dpvrList.GroupBy(t => t.GroupValue).Select(g => new { g.Key, Count = g.Count() });

                    foreach (var item in tmpList)
                    {
                        if (lastValue != item.GroupValue)
                        {
                            DataPanelViewGroupRow groupItem = new DataPanelViewGroupRow()
                            {
                                IsSelected = false,
                                RowIndex = rowIndex,
                                RowType = DataPanelRowType.GroupRow,
                                GroupDispalyText = item.GroupDispalyText,
                                GroupValue = item.GroupValue
                            };
                            groupItem.RowCount = groupList.First(t => t.Key == item.GroupValue).Count;
                            InsertItem(rowIndex, groupItem);
                            rowIndex++;
                        }

                        item.RowType = DataPanelRowType.ContentRow;
                        item.IsSelected = false;
                        item.RowIndex = rowIndex;
                        InsertItem(rowIndex, item);

                        rowIndex++;
                        lastValue = item.GroupValue;
                    }
                }
                #endregion

                #region 统计行在下方
                else
                {
                    foreach (var item in tmpList)
                    {
                        if (!string.IsNullOrEmpty(lastValue) && lastValue != item.GroupValue)
                        {
                            DataPanelViewGroupRow groupItem = new DataPanelViewGroupRow()
                            {
                                IsSelected = false,
                                RowIndex = rowIndex,
                                RowType = DataPanelRowType.GroupRow,
                                GroupDispalyText = item.GroupDispalyText,
                                GroupValue = item.GroupValue,
                                RowCount = rowCount
                            };
                            InsertItem(rowIndex, groupItem);
                            rowCount = 0;
                            rowIndex++;
                        }

                        item.RowType = DataPanelRowType.ContentRow;
                        item.IsSelected = false;
                        item.RowIndex = dpvrList.IndexOf(item);
                        InsertItem(rowIndex, item);

                        //最后一行
                        if (tmpList.IndexOf(item) == tmpList.Count - 1)
                        {
                            rowIndex++;
                            DataPanelViewGroupRow groupItem = new DataPanelViewGroupRow()
                            {
                                IsSelected = false,
                                RowIndex = rowIndex,
                                RowType = DataPanelRowType.GroupRow,
                                GroupDispalyText = item.GroupDispalyText,
                                GroupValue = item.GroupValue,
                                RowCount = rowCount + 1
                            };
                            InsertItem(rowIndex, groupItem);
                            rowCount = 0;
                        }

                        rowCount++;
                        rowIndex++;
                        lastValue = item.GroupValue;
                    }
                }
                #endregion
            }
            else
            {
                foreach (var item in dpvrList)
                {
                    item.RowType = DataPanelRowType.ContentRow;
                    item.IsSelected = false;
                    item.RowIndex = dpvrList.IndexOf(item);
                    InsertItem(item.RowIndex, item);
                }
            }
            this.UpdateScrollbar();
            if (isShowMore)
            {
                DataPanelViewRow loadMoreItem = itemList.First(t => t.RowType == DataPanelRowType.LoadMoreRow);
                loadMoreItem.RowIndex = itemList.Count - 1;

                KeyValuePair<DataPanelViewRowControl, int> pnlChild = controlList.FirstOrDefault(t => t.Key.DataPanelRow.RowIndex == loadMoreItem.RowIndex);
                if (pnlChild.Key != null)
                {
                    controlList[pnlChild.Key] = loadMoreItem.RowIndex;
                }
                //调整位置
                ScrollItem(null);
            }

            if (controlList.Count > 0)
            {
                DataPanelViewRowControl dpvrc = controlList.First(t => t.Key.DataPanelRow.RowIndex == 0).Key;
                //有可能第一行不是内容行
                int tmpRowIndex = 0;
                while (dpvrc.DataPanelRow.RowType != DataPanelRowType.ContentRow)
                {
                    tmpRowIndex++;
                    if (tmpRowIndex > controlList.Count)
                    {
                        break;
                    }
                    dpvrc = controlList.First(t => t.Key.DataPanelRow.RowIndex == tmpRowIndex).Key;
                }
                item_MouseClick(dpvrc, null);
            }
        }
        #endregion

        #region 添加行数据，会自动合并到组 Add
        /// <summary>
        /// 添加行数据，会自动合并到组
        /// </summary>
        public void Add(DataPanelViewRow item)
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
            if (SetItemTemplate == null)
                throw new Exception("必须启用 SetItemTemplate 事件");
            item.RowType = DataPanelRowType.ContentRow;
            item.IsSelected = true;
            //新增的时候会有闪烁
            ClearSelectedItem();
            if (isGroup)
            {
                AddByGroup(item);
            }
            #region 不分组
            else
            {
                item.RowIndex = itemList.Count;
                //添加到最后
                int rowIndex = itemList.Count;
                if (isShowMore)
                {
                    var find = itemList.First(t => t.RowType == DataPanelRowType.LoadMoreRow);
                    if (find != null)
                    {
                        rowIndex = find.RowIndex;
                        find.RowIndex += 1;

                        KeyValuePair<DataPanelViewRowControl, int> pnlChild = controlList.FirstOrDefault(t => t.Key.DataPanelRow.RowIndex == find.RowIndex);
                        if (pnlChild.Key != null)
                        {
                            controlList[pnlChild.Key] = find.RowIndex;
                        }

                        item.RowIndex = rowIndex;
                    }
                }

                InsertItem(rowIndex, item);
            }
            #endregion
            RefreshContent();
            this.FirstDisplayedScrollingRowIndex = item.RowIndex;
        }
        #endregion

        #region 批量追加数据 该方法不适用分组 Add<T>
        /// <summary>
        /// 批量追加数据 该方法不适用分组
        /// </summary>
        /// <typeparam name="T">必须是继承DataPanelRow的</typeparam>
        /// <param name="dt">数据源</param>
        public void Add<T>(DataTable dt)
            where T : DataPanelViewRow, new()
        {
            if (dt == null)
                throw new Exception("没有数据！");
            if ((isShowMore && itemList.Count == 1) || (!isShowMore && itemList.Count == 0))
            {
                this.DataSource<T>(dt);
                return;
            }
            if (SetItemTemplate == null)
                throw new Exception("必须启用 SetItemTemplate 事件");

            if (isGroup)
            {
                throw new Exception("该方法不适用分组！");
            }
            else
            {
                int rowIndex = itemList.Count;

                if (isShowMore)
                {
                    var find = itemList.First(t => t.RowType == DataPanelRowType.LoadMoreRow);
                    if (find != null)
                    {
                        rowIndex = find.RowIndex;
                        find.RowIndex += dt.Rows.Count;

                        KeyValuePair<DataPanelViewRowControl, int> pnlChild = controlList.FirstOrDefault(t => t.Key.DataPanelRow.RowIndex == find.RowIndex);
                        if (pnlChild.Key != null)
                        {
                            controlList[pnlChild.Key] = find.RowIndex;
                        }
                    }
                }

                foreach (DataRow row in dt.Rows)
                {
                    DataPanelViewRow item = new T()
                    {
                        DataRow = row,
                        IsSelected = false,
                        RowIndex = rowIndex,
                        RowType = DataPanelRowType.ContentRow
                    };
                    InsertItem(rowIndex, item);
                    rowIndex++;
                }
            }
            RefreshContent();
        }
        #endregion

        #region 批量追加数据 支持分组
        /// <summary>
        /// 批量追加数据 支持分组 Add<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dpvrList"></param>
        public void Add<T>(List<T> dpvrList)
            where T : DataPanelViewRow, new()
        {
            if (dpvrList == null || dpvrList.Count == 0)
                throw new Exception("没有数据！");

            if ((isShowMore && itemList.Count == 1) || (!isShowMore && itemList.Count == 0))
            {
                this.DataSource<T>(dpvrList);
                return;
            }
            if (SetItemTemplate == null)
                throw new Exception("必须启用 SetItemTemplate 事件");

            if (isGroup)
            {
                //重新统计排序信息
                foreach (var item in dpvrList)
                {
                    item.IsSelected = false;
                    item.RowType = DataPanelRowType.ContentRow;
                    AddByGroup(item);
                }
            }
            #region 不分组
            else
            {
                int rowIndex = itemList.Count;

                if (isShowMore)
                {
                    var find = itemList.First(t => t.RowType == DataPanelRowType.LoadMoreRow);
                    if (find != null)
                    {
                        rowIndex = find.RowIndex;
                        find.RowIndex += itemList.Count;

                        KeyValuePair<DataPanelViewRowControl, int> pnlChild = controlList.FirstOrDefault(t => t.Key.DataPanelRow.RowIndex == find.RowIndex);
                        if (pnlChild.Key != null)
                        {
                            controlList[pnlChild.Key] = find.RowIndex;
                        }
                    }
                }

                foreach (var item in dpvrList)
                {
                    item.IsSelected = false;
                    item.RowIndex = rowIndex;
                    item.RowType = DataPanelRowType.ContentRow;
                    InsertItem(rowIndex, item);
                    rowIndex++;
                }
            }
            #endregion
            RefreshContent();
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
            if (item.DataRow == null)
                throw new Exception("没有数据！");
            if (SetItemTemplate == null)
                throw new Exception("必须启用 SetItemTemplate 事件");
            //新增的时候会有闪烁
            ClearSelectedItem();
            if (isGroup)
            {
                Add(item);
            }
            else
            {
                item.RowType = DataPanelRowType.ContentRow;
                item.RowIndex = rowIndex;

                for (int i = rowIndex; i < itemList.Count; i++)
                {
                    itemList[i].RowIndex += 1;
                }

                List<DataPanelViewRowControl> childList = controlList.Select(t => t.Key).ToList();
                for (int i = 0; i < controlList.Count; i++)
                {
                    DataPanelViewRowControl childItem = childList[i];
                    controlList[childItem] = childItem.DataPanelRow.RowIndex;
                }

                InsertItem(rowIndex, item);
                RefreshContent();
                this.FirstDisplayedScrollingRowIndex = item.RowIndex;
            }
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
            /* 判断删除的数据是否为 绑定的控件
            * 找Focus=true的上一个或下一个为选中项
            * 更新统计行内容
            */
            if (rowIndexList.Count == 0)
                return;

            #region 处理数据行
            DataPanelViewRow focusItem = null;
            foreach (DataPanelViewRow item in itemList)
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
                DataPanelViewRow delItem = null;
                foreach (DataPanelViewRow pnlItem in itemList)
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
                foreach (DataPanelViewRow item in itemList)
                {
                    if (item.RowIndex >= searchIndex)
                    {
                        if (item.RowType != DataPanelRowType.ContentRow)
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
                        DataPanelViewRow item = itemList[i];
                        if (item.RowIndex <= searchIndex)
                        {
                            if (item.RowType != DataPanelRowType.ContentRow)
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
                int rowIndex = 0;

                var groupList = itemList.Where(t => t.RowType == DataPanelRowType.ContentRow).GroupBy(t => t.GroupValue).Select(g => new { g.Key, Count = g.Count() });

                List<DataPanelViewRow> delList = new List<DataPanelViewRow>();

                for (int i = 0; i < itemList.Count; i++)
                {
                    DataPanelViewRow item = itemList[i];
                    if (item.RowType == DataPanelRowType.GroupRow)
                    {
                        var find = groupList.FirstOrDefault(t => t.Key == item.GroupValue);
                        if (find == null)
                        {
                            rowIndex -= 1;
                            delList.Add(item);
                        }
                        else
                        {
                            DataPanelViewGroupRow groupItem = item as DataPanelViewGroupRow;
                            groupItem.RowCount = find.Count;
                        }
                    }
                    item.RowIndex = rowIndex;
                    rowIndex++;
                }
                //删除统计行
                foreach (DataPanelViewRow item in delList)
                {
                    displayRectangleHeight -= item.Height;
                    itemList.Remove(item);
                }
            }
            else
            {
                int startIndex = rowIndexList[0];
                //更新索引
                foreach (DataPanelViewRow item in itemList)
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
                List<DataPanelViewRowControl> childList = controlList.Select(t => t.Key).ToList();
                for (int i = 0; i < controlList.Count; i++)
                {
                    DataPanelViewRowControl childItem = childList[i];
                    if (!IsChangeControlType(i, ref childItem))
                    {
                        controlList[childList[i]] = i;
                        GetNewInfo(childList[i], i);
                        childList[i].RefreshData();
                    }
                }
            }
            else
            {
                List<DataPanelViewRowControl> delChild = new List<DataPanelViewRowControl>();
                List<DataPanelViewRowControl> childList = controlList.Select(t => t.Key).ToList();

                for (int i = 0; i < controlList.Count; i++)
                {
                    if (i < itemList.Count)
                    {
                        DataPanelViewRowControl childItem = childList[i];
                        if (!IsChangeControlType(i, ref childItem))
                        {
                            controlList[childList[i]] = i;
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

        #region 返回选中的数据 SelectedRows
        /// <summary>
        /// 返回选中的数据
        /// </summary>
        /// <returns></returns>
        public List<DataPanelViewRow> SelectedRows()
        {
            List<DataPanelViewRow> items = new List<DataPanelViewRow>();

            foreach (DataPanelViewRow item in itemList)
            {
                if (item.IsSelected)
                {
                    items.Add(item);
                }
            }
            return items;
        }
        #endregion

        #region 内容修改后更新 Refresh
        /// <summary>
        /// 内容修改后更新
        /// </summary>
        /// <param name="rowIndex"></param>
        public void Refresh(int rowIndex)
        {
            KeyValuePair<DataPanelViewRowControl, int> find = controlList.FirstOrDefault(t => t.Value == rowIndex);
            if (find.Key != null)
            {
                DataPanelViewRow item = itemList.First(t => t.RowIndex == rowIndex);
                find.Key.DataPanelRow = item;
                find.Key.IsSelected = item.IsSelected;
                find.Key.RefreshData();
                find.Key.SetControlHeight();

                bool isSizeChange = false;
                if (item.Height != find.Key.Height)
                    isSizeChange = true;

                displayRectangleHeight += find.Key.Height - item.Height;

                item.Height = find.Key.Height;
                item_MouseClick(find.Key, null);

                if (isSizeChange)
                    ContentLengthChange();
            }
        }
        #endregion

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
                        dpvrc.RefreshData();
                        dpvrc.Height = dpvRow.Height;
                        AddControl(dpvrc);
                    }
                }
                else
                {
                    DataPanelViewRowControl dpvrc = SetItemTemplate(dpvRow);
                    dpvrc.RefreshData();
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
                DataPanelViewRowControl dpvrc = new DataPanelViewGroupRowControl(dpvgr.GroupDispalyText, dpvgr.RowCount, isShowGroupTotal, groupTitleTextAlign, groupTitleLeft);
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
            /* 先判断组是否存在
             *   存：查找在组中的位置，然后插入内容行，更新索引
             *   否：查询组应该在的位置，然后插入内容行&组，更新索引
             * 
             */
            List<DataPanelViewRow> groupList = itemList.Where(t => t.RowType == DataPanelRowType.GroupRow).ToList();
            int groupRowIndex = -1;
            //记录当前是第几个组
            int groupRowNum = 0;
            foreach (DataPanelViewGroupRow row in groupList)
            {
                if (row.GroupValue == item.GroupValue)
                {
                    groupRowIndex = row.RowIndex;
                    break;
                }
                groupRowNum++;
            }
            int rowIndex = 0;
            //是否更新统计信息
            bool isUpdateTotal = true;
            if (groupRowIndex == -1)
            {
                List<DataPanelViewRow> tmpGroupList = new List<DataPanelViewRow>();
                tmpGroupList.AddRange(groupList);
                tmpGroupList.Add(item);

                if (ascending)
                    tmpGroupList = tmpGroupList.OrderBy(t => t.GroupValue).ThenBy(t => t.GroupValueIndex).ToList();
                else
                    tmpGroupList = tmpGroupList.OrderByDescending(t => t.GroupValue).ThenByDescending(t => t.GroupValueIndex).ToList();


                int tmpGroupRowNum = tmpGroupList.IndexOf(item);
                if (tmpGroupRowNum > 0)
                {
                    if (groupRowIsTop)
                    {
                        if (tmpGroupRowNum >= groupList.Count)
                        {
                            if (isShowMore)
                            rowIndex = itemList.Count - 1;
                            else
                                rowIndex = itemList.Count;
                        }
                        else
                            rowIndex = groupList[tmpGroupRowNum].RowIndex;
                    }
                    else
                    {
                        rowIndex = groupList[tmpGroupRowNum - 1].RowIndex + 1;
                    }
                }
                isUpdateTotal = false;
            }
            else
            {
                int startIndex = 0;
                int endIndex = 0;
                if (groupRowIsTop)
                {
                    startIndex = groupList[groupRowNum].RowIndex + 1;
                    if (groupRowNum == groupList.Count - 1)
                    {
                        endIndex = itemList.Count;
                        if (isShowMore)
                            endIndex -= 1;
                    }
                    else
                        endIndex = groupList[groupRowNum + 1].RowIndex;
                }
                else
                {
                    if (groupRowNum > 0)
                    {
                        startIndex = groupList[groupRowNum - 1].RowIndex + 1;
                    }
                    endIndex = groupRowIndex;
                }


                bool isFind = false;
                for (int i = startIndex; i < endIndex; i++)
                {
                    if (item.GroupValueIndex < itemList[i].GroupValueIndex)
                    {
                        rowIndex = i;
                        isFind = true;
                        break;
                    }
                }
                if (!isFind)
                    rowIndex = endIndex;
            }

            #region 更新索引&统计
            for (int i = rowIndex; i < itemList.Count; i++)
            {
                if (groupRowIndex == -1)
                    itemList[i].RowIndex += 2;
                else
                    itemList[i].RowIndex += 1;
                if (!groupRowIsTop)
                    {
                    if (itemList[i].RowType == DataPanelRowType.GroupRow && isUpdateTotal)
                        {//找到最近的一个统计行
                        DataPanelViewGroupRow groupItem = itemList[i] as DataPanelViewGroupRow;
                            groupItem.RowCount += 1;
                            isUpdateTotal = false;
                        }
                    }
                }

            if (groupRowIsTop)
            {//往上找
                int pervRowIndex = rowIndex;
                if (rowIndex == itemList.Count)
                    pervRowIndex = itemList.Count - 1;
                while (isUpdateTotal && pervRowIndex >= 0)
                {
                    pervRowIndex--;
                    if (pervRowIndex >= 0 && itemList[pervRowIndex].RowType == DataPanelRowType.GroupRow)
                    {//找到最近的一个统计行
                        DataPanelViewGroupRow groupItem = itemList[pervRowIndex] as DataPanelViewGroupRow;
                        groupItem.RowCount += 1;
                        isUpdateTotal = false;
                        break;
                    }
                }
            }

            #endregion
            //注意这里类的引用关系， 控件不需要在更改索引
            List<DataPanelViewRowControl> childList = controlList.Select(t => t.Key).ToList();
            for (int i = 0; i < controlList.Count; i++)
            {
                DataPanelViewRowControl childItem = childList[i];
                if (childItem.DataPanelRow.RowIndex >= rowIndex)
                {
                    controlList[childItem] = childItem.DataPanelRow.RowIndex;
                }
            }

            if (groupRowIndex == -1)
            {
                if (groupRowIsTop)
                {
                    //增加统计行
                    DataPanelViewGroupRow groupItem = new DataPanelViewGroupRow()
                    {
                        IsSelected = false,
                        RowIndex = rowIndex,
                        RowType = DataPanelRowType.GroupRow,
                        GroupDispalyText = item.GroupDispalyText,
                        GroupValue = item.GroupValue,
                        RowCount = 1
                    };
                    InsertItem(groupItem.RowIndex, groupItem);


                    item.RowIndex = rowIndex + 1;
                    InsertItem(item.RowIndex, item);
                }
                else
                {
                    item.RowIndex = rowIndex;
                    InsertItem(rowIndex, item);
                    //增加统计行
                    DataPanelViewGroupRow groupItem = new DataPanelViewGroupRow()
                    {
                        IsSelected = false,
                        RowIndex = rowIndex + 1,
                        RowType = DataPanelRowType.GroupRow,
                        GroupDispalyText = item.GroupDispalyText,
                        GroupValue = item.GroupValue,
                        RowCount = 1
                    };
                    InsertItem(groupItem.RowIndex, groupItem);
                }
            }
            else
            {
                item.RowIndex = rowIndex;
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
            item.DefaultColor = defaultColor;
            item.MouseEnterColor = mouseEnterColor;
            item.SelectedColor = selectedColor;
            item.Width = this.pnlContent.Width;
            item.Anchor = (AnchorStyles)(AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
            if (isSetTop)
            {
                if (controlList.Count == 0 || item.DataPanelRow.RowIndex == 0)
                {
                    item.Top = 0;
                }
                else
                {
                    DataPanelViewRowControl lastItem = controlList.First(t => t.Value == item.DataPanelRow.RowIndex - 1).Key;
                    item.Top = lastItem.Top + lastItem.Height;
                }
            }

            controlList.Add(item, item.DataPanelRow.RowIndex);
            this.pnlContent.Controls.Add(item);

            if (item.DataPanelRow.RowType == DataPanelRowType.ContentRow)
            {
                item.MouseClick += item_MouseClick;
                item.MouseEnter += Item_MouseEnter;
                item.MouseLeave += Item_MouseLeave;
                item.MouseMove += Item_MouseMove;
                item.DoubleClick += item_DoubleClick;
                item.SizeChanged += item_SizeChanged;
            }
        }

        void item_SizeChanged(object sender, EventArgs e)
        {
            if (ItemHeightChanged != null)
            {
                DataPanelViewRowControl control = sender as DataPanelViewRowControl;
                DataPanelViewRow dpvr = itemList.First(t => t.RowIndex == control.DataPanelRow.RowIndex);
                if (dpvr.Height != control.Height)
                {
                    control.SetControlHeight();
                    displayRectangleHeight += control.Height - dpvr.Height;
                    dpvr.Height = control.Height;
                    ContentLengthChange();
                    ItemHeightChanged(sender, e);
                }
            }
        }

        void item_DoubleClick(object sender, EventArgs e)
        {
            if (ItemDoubleClick != null)
                ItemDoubleClick(sender, e);
        }
        private void Item_MouseMove(object sender, MouseEventArgs e)
        {
            isActiveMouseEvent = true;
        }

        private void Item_MouseLeave(object sender, EventArgs e)
        {
            DataPanelViewRowControl dpvrc = sender as DataPanelViewRowControl;
            if (!dpvrc.IsSelected)
                dpvrc.BackColor = defaultColor;
        }

        private void Item_MouseEnter(object sender, EventArgs e)
        {
            DataPanelViewRowControl dpvrc = sender as DataPanelViewRowControl;
            if (dpvrc.TopLevelControl.Handle == GetForegroundWindow())
            {
                dpvrc.Parent.Focus();
            }
            if (isActiveMouseEvent && !dpvrc.IsSelected)
                dpvrc.BackColor = mouseEnterColor;
        }

        void item_MouseClick(object sender, MouseEventArgs e)
        {
            DataPanelViewRowControl dpvrc = sender as DataPanelViewRowControl;
            DataPanelViewRow tmpItem = itemList.First(t => t.RowIndex == dpvrc.DataPanelRow.RowIndex);
            bool isFocus = tmpItem.IsFocus;
            dpvrc.Parent.Focus();//只有点击的才是Focus,Focus永远只有一个

            #region Control + 鼠标
            if (multiSelect && (Control.ModifierKeys & Keys.Control) == Keys.Control)// CTRL is pressed    
            {
                if (dpvrc.DataPanelRow.RowIndex < itemList.Count && dpvrc.DataPanelRow.RowIndex >= 0)
                {
                    foreach (DataPanelViewRow item in itemList)
                    {
                        if (item.RowIndex != dpvrc.DataPanelRow.RowIndex && item.IsFocus)
                        {
                            item.IsFocus = false;
                            break;
                        }
                    }
                    DataPanelViewRow pnlItem = itemList.First(t => t.RowIndex == dpvrc.DataPanelRow.RowIndex);
                    pnlItem.IsFocus = true;
                    pnlItem.IsSelected = !pnlItem.IsSelected;
                    dpvrc.IsSelected = pnlItem.IsSelected;
                }
            }
            #endregion
            #region Shift + 鼠标
            else if (multiSelect && (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
            {//范围是从Focus=true 到这个为止
                DataPanelViewRow focusItem = null;
                foreach (DataPanelViewRow item in itemList)
                {
                    if (item.IsFocus)
                    {
                        focusItem = item;
                        break;
                    }
                }
                if (focusItem == null)
                {
                    if (dpvrc.DataPanelRow.RowIndex < itemList.Count && dpvrc.DataPanelRow.RowIndex >= 0)
                    {
                        foreach (DataPanelViewRow item in itemList)
                        {
                            if (item.RowIndex != dpvrc.DataPanelRow.RowIndex && item.IsFocus)
                            {
                                item.IsFocus = false;
                                break;
                            }
                        }

                        DataPanelViewRow pnlItem = itemList.First(t => t.RowIndex == dpvrc.DataPanelRow.RowIndex);
                        pnlItem.IsFocus = true;
                        pnlItem.IsSelected = dpvrc.IsSelected;
                    }
                }
                else
                {
                    int startIndex = 0;
                    int endIndex = 0;
                    if (focusItem.RowIndex > dpvrc.DataPanelRow.RowIndex)
                    {//向下选择
                        startIndex = dpvrc.DataPanelRow.RowIndex;
                        endIndex = focusItem.RowIndex;
                    }
                    else
                    {//向上选择
                        startIndex = focusItem.RowIndex;
                        endIndex = dpvrc.DataPanelRow.RowIndex;
                    }
                    foreach (DataPanelViewRowControl item in controlList.Keys)
                    {
                        if (item.DataPanelRow.RowIndex != dpvrc.DataPanelRow.RowIndex && item.IsSelected)
                        {
                            item.IsSelected = false;
                        }
                    }
                    foreach (DataPanelViewRow item in itemList)
                    {
                        if (item.RowIndex != dpvrc.DataPanelRow.RowIndex && item.IsSelected)
                        {
                            item.IsSelected = false;
                        }
                    }

                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        DataPanelViewRow pnlItem = itemList.First(t => t.RowIndex == i);
                        if (pnlItem.RowType == DataPanelRowType.ContentRow)
                        {
                            pnlItem.IsSelected = true;

                            KeyValuePair<DataPanelViewRowControl, int> find = controlList.FirstOrDefault(t => t.Key.DataPanelRow.RowIndex == i);
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
                ClearSelectedItem(dpvrc.DataPanelRow.RowIndex);
                if (dpvrc.Top < 0)
                {
                    this.myVScrollBar1.Value = GetItemHeightByRowIndex(tmpItem.RowIndex);
                    this.pnlContent.VScrollValue = this.myVScrollBar1.Value;
                    ScrollItem(true);
                }
                else if (dpvrc.Top + dpvrc.Height > this.Height)
                {
                    this.myVScrollBar1.Value += dpvrc.Top + dpvrc.Height - this.Height;//加上隐藏的部分
                    this.pnlContent.VScrollValue = this.myVScrollBar1.Value;
                    ScrollItem(false);
                }
            }
            #endregion
            tmpItem = itemList.First(t => t.RowIndex == dpvrc.DataPanelRow.RowIndex);
            if (SelectionChanged != null && isFocus != tmpItem.IsFocus)
            {
                SelectionChanged(tmpItem);
            }
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
            if (rowIndex >= 0 && rowIndex <= itemList.Count - 1)
            {
                for (int i = 0; i < itemList.Count; i++)
                {
                    if (itemList[i].RowIndex == rowIndex)
                    {
                        DataPanelViewRow item = itemList[i];
                        childItem.DataPanelRow = item;
                        childItem.IsSelected = item.IsSelected;
                        childItem.Height = item.Height;
                        break;
                    }
                }
            }
        }
        #endregion

        #region 内容变化后需调用此方法 UpdateScrollbar
        /// <summary>
        /// 内容变化后需调用此方法
        /// </summary>
        private void UpdateScrollbar()
        {
            this.pnlContent.DisplayRectangleHeight = displayRectangleHeight;
            this.myVScrollBar1.UpdateScrollbar();
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
                GetNewInfo(item.Key, item.Key.DataPanelRow.RowIndex);
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
                DataPanelViewRowControl childItem = controlList.First(t => t.Value == indexArr[i]).Key;
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
                        GetNewInfo(childItem, tmpStart);
                        childItem.RefreshData();
                    }
                }
                else
                {
                    IsChangeControlType(childItem.DataPanelRow.RowIndex, ref childItem);
                }
                childItem.Top = GetItemHeightByRowIndex(childItem.DataPanelRow.RowIndex) - this.myVScrollBar1.Value;
            }
        }
        #endregion

        #region 是否需要替换控件类型 IsChangeControlType
        /// <summary>
        /// 是否需要替换控件类型
        /// 如果替换的这个是统计行，与原本的不一致，则要删除当前控件，删除controlList中的对象，重新添加
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="childItem"></param>
        /// <returns></returns>
        private bool IsChangeControlType(int rowIndex, ref DataPanelViewRowControl childItem)
        {
            DataPanelViewRow item = itemList.First(t => t.RowIndex == rowIndex);
            if (childItem.DataPanelRow.RowType != item.RowType)
            {
                this.pnlContent.Controls.Remove(childItem);
                this.controlList.Remove(childItem);
                if (item.RowType == DataPanelRowType.ContentRow)
                {
                    DataPanelViewRowControl newItem = this.SetItemTemplate(item);
                    newItem.RefreshData();
                    newItem.Height = item.Height;
                    newItem.IsSelected = item.IsSelected;
                    this.AddControl(newItem, false);
                    childItem = newItem;
                }
                else if (item.RowType == DataPanelRowType.GroupRow)
                {
                    DataPanelViewGroupRow groupItem = item as DataPanelViewGroupRow;
                    DataPanelViewRowControl newItem = new DataPanelViewGroupRowControl(groupItem.GroupDispalyText, groupItem.RowCount, isShowGroupTotal, groupTitleTextAlign, groupTitleLeft);
                    newItem.DataPanelRow = groupItem;
                    newItem.RefreshData();
                    this.AddControl(newItem, false);
                    childItem = newItem;
                }
                else if (item.RowType == DataPanelRowType.LoadMoreRow)
                {
                    DataPanelViewLoadMoreRowControl newItem = new DataPanelViewLoadMoreRowControl();
                    newItem.LoadMore += childItem_LoadMore;
                    newItem.DataPanelRow = item;
                    this.AddControl(newItem, false);
                    childItem = newItem;
                }
                return true;
            }
            return false;
        }
        #endregion

        #region 内容或界面大小 发生变化 ContentLengthChange
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

        #region 清除选中项 ClearSelectedItem
        /// <summary>
        /// 清除选中项
        /// </summary>
        /// <param name="rowIndex"></param>
        private void ClearSelectedItem(int rowIndex)
        {
            if (multiSelect)
            {//多选
                foreach (DataPanelViewRowControl item in controlList.Keys)
                {
                    if (item.DataPanelRow.RowIndex != rowIndex && item.IsSelected)
                    {
                        item.IsSelected = false;
                    }
                }
                foreach (DataPanelViewRow item in itemList)
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
                foreach (DataPanelViewRowControl item in controlList.Keys)
                {
                    if (item.DataPanelRow.RowIndex != rowIndex && item.IsSelected)
                    {
                        item.IsSelected = false;
                        break;
                    }
                }

                foreach (DataPanelViewRow item in itemList)
                {
                    if (item.RowIndex != rowIndex && item.IsSelected)
                    {
                        item.IsSelected = false;
                        item.IsFocus = false;
                        break;
                    }
                }
            }

            KeyValuePair<DataPanelViewRowControl, int> find = controlList.FirstOrDefault(t => t.Value == rowIndex);
            if (find.Key != null)
            {
                find.Key.IsSelected = true;
            }
            if (rowIndex < itemList.Count && rowIndex >= 0)
            {
                DataPanelViewRow item = itemList.First(t => t.RowIndex == rowIndex);
                item.IsSelected = true;
                item.IsFocus = true;
            }
        }

        private void ClearSelectedItem()
        {
            foreach (DataPanelViewRowControl item in controlList.Keys)
            {
                if (item.IsSelected)
                {
                    item.IsSelected = false;
                    break;
                }
            }

            foreach (DataPanelViewRow item in itemList)
            {
                if (item.IsSelected)
                {
                    item.IsSelected = false;
                    item.IsFocus = false;
                    break;
                }
            }
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

        #region 根据行索引计算对应的高度 GetItemHeightByRowIndex
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
            int rlt = 0;
            totalHeight = 0;
            if (direction == ArrowDirection.Up)
            {
                List<DataPanelViewRow> descList = itemList.OrderByDescending(t => t.RowIndex).ToList();
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
}
