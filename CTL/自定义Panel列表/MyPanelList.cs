using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace 自定义Panel列表
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
        public delegate MyPanelChild ItemTemplateDelegate(PanelItem item, int scrollValue);
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
        /// 更新内容
        /// </summary>
        public event EventHandler UpdateChildItem;
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

        ///// <summary>
        ///// 控件绑定的个数
        ///// </summary>
        //private int controlCount = 10;
        /// <summary>
        /// 控件最多显示个数
        /// </summary>
        private int maxControlCount = 10;
        /// <summary>
        /// 最小行高
        /// </summary>
        private int minRowHeight = 60;
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
        /// 控件的最小行高
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
                maxControlCount = SystemInformation.WorkingArea.Height / minRowHeight + 1;//前后各加一个
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
                if (!myVScrollBar1.Visible)
                    return;
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
                    this.pnlContent.VScrollValue = value * minRowHeight;
                }
                UpdateScrollbar();
                bool isUp = this.pnlContent.VScrollValue < this.myVScrollBar1.Value;
                ScrollItem(isUp);
                KeyValuePair<MyPanelChild, int> childItem = controlList.FirstOrDefault(t => t.Key.RowIndex == value);
                if (childItem.Key != null)
                {
                    item_MouseClick(childItem.Key, null);
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

            this.pnlContent.LargeChange = 30;
            this.pnlContent.SmallChange = 15;

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
        }
        #endregion

        #region 绑定数据 DataSource
        /// <summary>
        /// 绑定数据 DataSource 
        /// </summary>
        public DataTable DataSource
        {
            set
            {
                this.Clear();
                foreach (DataRow row in value.Rows)
                {
                    PanelItem item = new PanelItem()
                    {
                        DataRow = row,
                        IsSelected = false,
                        RowIndex = value.Rows.IndexOf(row)
                    };
                    this.AddItem(item);
                }
                if (controlList.Count > 0)
                {
                    item_MouseClick(controlList.First().Key, null);
                }
                this.UpdateScrollbar();
            }
        }
        #endregion

        #region 添加行数据 AddItem
        /// <summary>
        /// 添加行数据
        /// </summary>
        /// <param name="item"></param>
        private void AddItem(PanelItem item)
        {
            if (SetItemTemplate == null)
                throw new Exception("必须启用 SetItemTemplate 事件");
            //MyPanelChild childItem = SetItemTemplate(item, myVScrollBar1.Value);

            //item.Height = childItem.Height;
            //displayRectangleHeight += item.Height;
            //itemList.Add(item);

            //if (controlList.Count < maxControlCount)
            //{
            //    AddControl(childItem);
            //}

            item.Height = this.minRowHeight;
            displayRectangleHeight += item.Height;
            itemList.Add(item);

            if (controlList.Count < maxControlCount)
            {
                MyPanelChild childItem = SetItemTemplate(item, myVScrollBar1.Value);
                AddControl(childItem);
            }
        }

        public void Add(PanelItem item)
        {
            item.RowIndex = itemList.Count;
            item.IsSelected = true;
            AddItem(item);
            this.UpdateScrollbar();
            this.ScrollToCaret();
            this.Refresh(item.RowIndex);
        }
        #endregion

        #region 添加控件 AddControl
        /// <summary>
        /// 添加控件
        /// </summary>
        /// <param name="item"></param>
        private void AddControl(MyPanelChild item)
        {
            item.DefaultColor = defaultColor;
            item.MouseEnterColor = mouseEnterColor;
            item.SelectedColor = selectedColor;
            item.Width = this.pnlContent.Width;
            item.Anchor = (AnchorStyles)(AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);

            controlList.Add(item, item.RowIndex);
            this.pnlContent.Controls.Add(item);
            item.MouseClick += item_MouseClick;
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
                        pnlItem.IsSelected = true;

                        KeyValuePair<MyPanelChild, int> find = controlList.FirstOrDefault(t => t.Key.RowIndex == i);
                        if (find.Key != null)
                        {
                            find.Key.IsSelected = true;
                        }
                    }
                }
            }
            #endregion
            #region 单选
            else
            {
                ClearSelectedItem(pnl.RowIndex);
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
            //设置选中项
            if (focusItem != null)
            {
                PanelItem find = itemList.FirstOrDefault(t => t.RowIndex == focusItem.RowIndex + 1);
                if (find != null)
                {
                    find.IsSelected = true;
                    find.IsFocus = true;
                }
                else
                {
                    find = itemList.FirstOrDefault(t => t.RowIndex == focusItem.RowIndex - 1);
                    if (find != null)
                    {
                        find.IsSelected = true;
                        find.IsFocus = true;
                    }
                }
            }

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
            #endregion

            #region 重新给控件赋值
            //重新给控件赋值
            if (itemList.Count > controlList.Count)
            {
                List<MyPanelChild> childList = controlList.Select(t => t.Key).ToList();
                for (int i = 0; i < controlList.Count; i++)
                {
                    controlList[childList[i]] = i;
                    GetNewInfo(childList[i], i);
                    UpdateChildItem(childList[i], null);
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
                        GetNewInfo(childList[i], i);
                        UpdateChildItem(childList[i], null);
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

            //重新定位，设置选中项
            //FirstDisplayedScrollingRowIndex = selIndex;
            UpdateScrollbar();
            ContentLengthChange();
        }
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
                UpdateChildItem(find.Key, null);
                item_MouseClick(find.Key, null);
            }
        }
        #endregion

        #region 内容变化后需调用此方法
        /// <summary>
        /// 内容变化后需调用此方法
        /// </summary>
        public void UpdateScrollbar()
        {
            this.pnlContent.DisplayRectangleHeight = displayRectangleHeight;
            this.myVScrollBar1.UpdateScrollbar();
        }
        #endregion

        #region 滚动到最底部
        /// <summary>
        /// 滚动到最底部
        /// </summary>
        public void ScrollToCaret()
        {
            int value = displayRectangleHeight - this.pnlContent.Height;
            if (value < 0)
                value = 0;
            this.pnlContent.VScrollValue = value;
            UpdateScrollbar();
            ScrollItem(false);
        }
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
            //Thread t = new Thread(() =>
            //{
            //    this.Invoke((MethodInvoker)delegate 
            //    {
            //        ScrollItem(this.myVScrollBar1.IsMoveUp);
            //    });                                
            //});
            //t.Start();
            ScrollItem(this.myVScrollBar1.IsMoveUp);
        }
        #endregion

        #region 处理快捷键 ProcessCmdKey
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                #region Up
                case Keys.Up://向上滚动
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
                        ClearSelectedItem(tmpItem.RowIndex - 1);

                        KeyValuePair<MyPanelChild, int> find = controlList.FirstOrDefault(t => t.Key.RowIndex == tmpItem.RowIndex - 1);
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
                            if (firstRowIndex < tmpItem.RowIndex - 1)
                            { //重新计算滚动条的Value
                                int displayRowCount = this.Height / this.minRowHeight;
                                this.myVScrollBar1.Value = (tmpItem.RowIndex - displayRowCount - 1) * tmpItem.Height + ((displayRowCount + 1) * tmpItem.Height - this.Height);
                                this.pnlContent.VScrollValue = this.myVScrollBar1.Value;
                            }
                            else
                            {
                                this.myVScrollBar1.Value = (tmpItem.RowIndex - 1) * tmpItem.Height;
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
                        ClearSelectedItem(tmpItem.RowIndex + 1);
                        KeyValuePair<MyPanelChild, int> find = controlList.FirstOrDefault(t => t.Key.RowIndex == tmpItem.RowIndex + 1);
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
                            if (firstRowIndex < tmpItem.RowIndex - 1)//这里判断 在选中项不再控件范围内滚动有问题？？？？
                            { //重新计算滚动条的Value
                                int displayRowCount = this.Height / this.minRowHeight;
                                this.myVScrollBar1.Value = (tmpItem.RowIndex - displayRowCount + 1) * tmpItem.Height + ((displayRowCount + 1) * tmpItem.Height - this.Height);
                                this.pnlContent.VScrollValue = this.myVScrollBar1.Value;
                            }
                            else if (find.Key != null && find.Key.Top + tmpItem.Height > this.Height)
                            {//为了完全显示内容
                                this.myVScrollBar1.Value += find.Key.Top + tmpItem.Height - this.Height;//加上隐藏的部分
                                this.pnlContent.VScrollValue = this.myVScrollBar1.Value;
                            }
                            else
                            {
                                this.myVScrollBar1.Value = (tmpItem.RowIndex + 1) * tmpItem.Height;
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
                            if (!item.Key.IsSelected)
                                item.Key.IsSelected = true;
                        }
                        foreach (var item in itemList)
                        {
                            if (!item.IsSelected)
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

        #region 滚动条滚动
        /// <summary>
        /// 滚动条滚动
        /// </summary>
        //private void ScrollItem()
        //{
        //    int startIndex = myVScrollBar1.Value / this.minRowHeight;
        //    int endIndex = startIndex + controlList.Count - 1;
        //    if (controlList.Count == itemList.Count)
        //    {//当控件内容个数小于最大个数时，不需要调整行索引
        //        startIndex = 0;
        //        endIndex = controlList.Count - 1;
        //    }
        //    else if (endIndex > itemList.Count)
        //    {//当endIndex > itemList.Count 即endIndex超过内容行数
        //        endIndex = itemList.Count - 1;
        //        startIndex = endIndex - (controlList.Count - 1);
        //    }
        //    else if (startIndex > 0)
        //    {
        //        startIndex -= 1;
        //        endIndex -= 1;
        //    }
        //    int tmpStart = startIndex;

        //    int[] indexArr = controlList.Values.OrderBy(t => t).ToArray();
        //    for (int i = 0; i < indexArr.Length; i++)
        //    {
        //        MyPanelChild childItem = controlList.First(t => t.Value == indexArr[i]).Key;
        //        if (controlList[childItem] < startIndex || controlList[childItem] > endIndex)
        //        {
        //            while (controlList.ContainsValue(tmpStart) && tmpStart < endIndex)
        //            {
        //                tmpStart += 1;
        //            }
        //            controlList[childItem] = tmpStart;
        //            childItem.RowIndex = tmpStart;

        //            if (childItem.RowIndex < itemList.Count)
        //            {
        //                GetNewInfo(childItem, childItem.RowIndex);
        //            }

        //            if (UpdateChildItem != null)
        //                UpdateChildItem(childItem, null);
        //        }
        //        childItem.Top = childItem.RowIndex * childItem.Height - myVScrollBar1.Value;
        //    }
        //}

        private void ScrollItem(bool? isUp)
        {
            int startIndex = myVScrollBar1.Value / this.minRowHeight;
            int endIndex = startIndex + controlList.Count - 1;
            if (controlList.Count == itemList.Count)
            {//当控件内容个数小于最大个数时，不需要调整行索引
                startIndex = 0;
                endIndex = controlList.Count - 1;
            }
            else if (endIndex > itemList.Count)
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
                    controlList[childItem] = tmpStart;
                    childItem.RowIndex = tmpStart;

                    if (childItem.RowIndex < itemList.Count)
                    {
                        GetNewInfo(childItem, childItem.RowIndex);
                    }

                    if (UpdateChildItem != null)
                        UpdateChildItem(childItem, null);
                }
                //每次尽可能显示整行
                int tmpTop = childItem.RowIndex * childItem.Height - myVScrollBar1.Value;
                //int tmpOffset = this.Height - this.Height / this.minRowHeight * this.minRowHeight;
                //if (isUp != null && isUp.Value)
                //{
                //    if (tmpTop < 0)
                //    {
                //        tmpTop = (tmpTop / this.minRowHeight - 1) * this.minRowHeight;
                //    }
                //    else
                //    {
                //        tmpTop = tmpTop / this.minRowHeight * this.minRowHeight;                    
                //    }                   
                //}
                //else if (isUp != null && !isUp.Value)
                //{
                //    if (tmpTop < 0)
                //    {
                //        tmpTop = (tmpTop / this.minRowHeight - 1) * this.minRowHeight;
                //    }
                //    else
                //    {
                //        tmpTop = tmpTop / this.minRowHeight * this.minRowHeight;
                //    }
                //    tmpTop += tmpOffset;
                //}
                childItem.Top = tmpTop;
            }
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
                int displayCount = this.Height / this.minRowHeight;
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

        #endregion

    }
}
