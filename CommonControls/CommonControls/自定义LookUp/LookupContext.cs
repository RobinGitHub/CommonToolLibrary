using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommonControls
{
    /// <summary>
    /// 关闭弹出PopupControlHost的委托
    /// </summary>
    /// <param name="sender"></param>
    public delegate void PopCloseHandler(object sender);

    /// <summary>
    /// 弹出PopupControlHost
    /// </summary>
    public partial class LookupContext : UserControl 
    {       
        public event PopCloseHandler CloseHandler;
        public event PopCloseHandler ClearHandler;
        private object _ContextValue;

        /// <summary>
        /// 添加对PopupControlHost的引用
        /// </summary>
        public PopupControlHost PopupControlHost;

        /// <summary>
        /// 对数据字段的绑定
        /// </summary>
        public Dictionary<string, string> Mapping{set;get;}
        public object ContextValue { get; set; }        

        /// <summary>
        /// 数据源
        /// </summary>
        public Object DataSource
        {
            get { return this.bindingSource.DataSource; }
            set { this.bindingSource.DataSource = value; }
        }

        public LookupContext()
        {
            InitializeComponent();
            Mapping = new Dictionary<string, string>();
        }

        private void LookupContext_Load(object sender, EventArgs e)
        {
            //鼠标双事件
            this.dataGridView.MouseDoubleClick += new MouseEventHandler(dataGridView_MouseDoubleClick);
            //回车事件
            this.dataGridView.KeyDown += new KeyEventHandler(dataGridView_KeyDown);
            
            //初始化DataGridView的列
            foreach(KeyValuePair<string,string> keyValuePair in Mapping)
            {
                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                column.HeaderText = keyValuePair.Key;
                column.DataPropertyName = keyValuePair.Value;
                this.dataGridView.Columns.Add(column);
            }
        }

        /// <summary>
        /// 回车事件
        /// </summary>
        void dataGridView_KeyDown(object sender, KeyEventArgs e)
        { 
            if (e.KeyCode == Keys.Enter)
            {
                dataGridView_MouseDoubleClick(sender, null);
            }
        }

        /// <summary>
        /// 鼠标双击事件
        /// </summary>
        void dataGridView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.ContextValue = bindingSource.Current;
            if (ContextValue != null)
            {
                CloseHandler(this);
            }
        }

        /// <summary>
        /// 添加侦听事件
        /// </summary>
        public void AddListen(TextBox sender)
        {
            sender.KeyUp += new KeyEventHandler(sender_KeyUp);
            sender.KeyPress += new KeyPressEventHandler(sender_KeyPress);
        }

        /// <summary>
        /// 侦听键盘回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sender_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                dataGridView_MouseDoubleClick(sender, null);
            }
        }

        /// <summary>
        /// 键盘上移和下移
        /// </summary>
        private void sender_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            int i = textBox.SelectionStart;
           
            if (e.KeyCode == Keys.Up)
            {
                SelectNextRow(-1);
                return;
            }
            if (e.KeyCode == Keys.Down)
            {
                SelectNextRow(1);
                return;
            }
        }

        /// <summary>
        /// 选择下一行
        /// </summary>
        /// <param name="step"></param>
        private void SelectNextRow(int step)
        {
            int index = 0;
            if (dataGridView.Rows.Count == 1) return;

            if (dataGridView.SelectedRows.Count == 1)
                index = dataGridView.SelectedRows[0].Index + step;
            if (index < 0)
                index = dataGridView.Rows.Count - 1;
            if (index == dataGridView.Rows.Count)
                index = 0;

            if (dataGridView.Rows.Count == 0)
                return;
            dataGridView.Rows[index].Selected = true;
            dataGridView.CurrentCell = dataGridView.Rows[index].Cells[0];
        }

        /// <summary>
        /// //关闭弹出选择框
        /// </summary>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.PopupControlHost.Close();
        }

        /// <summary>
        /// 清空文本框的值
        /// </summary>
        private void btnReset_Click(object sender, EventArgs e)
        {
            if (ClearHandler != null)
                ClearHandler(this);
        }
    }
}
