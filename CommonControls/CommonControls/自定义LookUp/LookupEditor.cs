using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace CommonControls
{
    /// <summary>
    /// 自定义LookupEditor,实现INotifyPropertyChanged接口，当值改变执行数据绑定及查询
    /// </summary>
    public partial class LookupEditor : UserControl, INotifyPropertyChanged
    {
        public delegate bool ValueChangeHandler(object sender);
        public event ValueChangeHandler ChangeEvent;

        public string _DisplayMember { set; get; }
        private string _text;//文本值
        private object _target;

        #region
        PopupControlHost normoalHost;//ToolStripDropDown下拉控件        
        LookupContext lookupContext = new LookupContext();//自定义数据控件       
        private bool isOpen = false;       
        private MessageFilter mf;
        private Point hostPoint;//弹出菜单全局位置       
        private bool _isOpen = true;//开关       
        #endregion

        #region PopupControlHost
        private bool _ChangeRegion = false;
        private Double _Opacity = 1.0F; //透明度
        private bool _CanResize = true;//改变大小        
        private bool _OpenFocused = false;
        #endregion       

        /// <summary>
        /// 显示文本框的值
        /// </summary>
        public string DisplayValue
        {
            get { return this.textBox.Text; }
            set { textBox.Text = value; }
        }

        /// <summary>
        /// 用于保存对象
        /// </summary>
        public object Target
        {
            get { return _target; }
            set
            {
                this._target = value;
                SendPropertyChanged("Target");
            }
        }

        /// <summary>
        /// 为false时不打开PopupControlHost，用于重置控件值不自动打开PopupControlHost
        /// </summary>
        public bool IsOpen { set; get; }

        /// <summary>
        /// 显示值
        /// </summary>
        public string DisplayMember { set; get; }

        /// <summary>
        /// 数据用于设置DataGridView的值
        /// </summary>
        public Dictionary<string, string> Mapping
        {
            get { return lookupContext.Mapping; }
            set { lookupContext.Mapping = value; }
        }

        /// <summary>
        /// 设置数据源
        /// </summary>
        public Object DataSource
        {
            get { return lookupContext.DataSource; }
            set { lookupContext.DataSource = value; }
        }

        /// <summary>
        /// 重写文本属性
        /// </summary>
        public override string Text
        {
            get { return this._text; }
            set
            {
                this._text = value;
                SendPropertyChanged("Text");
            }
        }

        #region 弹出菜单状态
        /// <summary>
        /// 设置焦点是否设置到弹出窗体上
        /// </summary>
        public bool OpenFocused
        {
            get { return _OpenFocused; }
            set { _OpenFocused = value; }
        }

        /// <summary>
        /// 设置能否改变大小
        /// </summary>
        public bool CanResize
        {
            get { return _CanResize; }
            set { _CanResize = value; }
        }

        /// <summary>
        /// 设置透明度
        /// </summary>
        public Double Opacity
        {
            get { return _Opacity; }
            set { _Opacity = value; }
        }

        /// <summary>
        /// 设置区域
        /// </summary>
        public bool ChangeRegion
        {
            get { return _ChangeRegion; }
            set { _ChangeRegion = value; }
        }
        #endregion

        [Description("Gets or Sets the textcolor")]
        [RefreshProperties(RefreshProperties.Repaint)]
        [NotifyParentProperty(true)]
        [DefaultValue(typeof(Color), "WhiteSmoke")]
        [Browsable(true)]
        public Color TextColor
        {
            get { return this.textBox.BackColor; }
            set
            {
                if (!value.Equals(this.textBox.BackColor))
                {
                    this.textBox.BackColor = value;
                }
            }
        }

        public LookupEditor()
        {
            InitializeComponent();

            this.textBox.BackColor = this.BackColor;

            normoalHost = new PopupControlHost(lookupContext);
            lookupContext.PopupControlHost = normoalHost;//添加对PopupControlHost引用,以便关闭弹出式选择框

            this.textBox.TextChanged += new EventHandler(txtValue_TextChanged);
            this.Leave += new EventHandler(this_Leave);
            
            normoalHost.Closed += new ToolStripDropDownClosedEventHandler(normoalHost_Closed);
            this.btnShow.Click += new EventHandler(btnShow_Click);
            lookupContext.CloseHandler += new PopCloseHandler(lookupContext_CloseHandler);//关闭事件
            lookupContext.ClearHandler += new PopCloseHandler(lookupContext_ClearHandler);//置空事件
            lookupContext.AddListen(this.textBox);//注册侦听 
            
            this.textBox.LostFocus += new EventHandler(txtValue_LostFocus);
        }

        private void txtValue_LostFocus(object sender, EventArgs e)
        {
            if (!normoalHost.Focused) 
                normoalHost.Close();
        }

        /// <summary>
        /// 此方法监听鼠标位置，当鼠标不在控件范围内，则自动关闭PopupControlHost
        /// </summary>
        private void CloseHost()
        {
            int X = System.Windows.Forms.Control.MousePosition.X;
            int Y = System.Windows.Forms.Control.MousePosition.Y;
            if ((X < this.hostPoint.X || X > (this.hostPoint.X + normoalHost.Width))
                || (Y < this.hostPoint.Y || Y > (this.hostPoint.Y + normoalHost.Height)))
            {
                normoalHost.Close();
                isOpen = false;
                if (mf != null)
                {
                    Application.RemoveMessageFilter(mf);
                }
            }
        }

        /// <summary>
        /// 关闭事件 
        /// </summary>
        private void normoalHost_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            isOpen = false;
        }

        /// <summary>
        /// 置空事件
        /// </summary>
        private void lookupContext_ClearHandler(object sender)
        {
            bool next = true;
            if (ChangeEvent != null)
                next = !ChangeEvent(null);
            if (next)
            {
                this.normoalHost.Close();
                this.Target = null;
                this.textBox.Text = string.Empty;
            }
        }

        /// <summary>
        /// 关闭事件设置控件的值
        /// </summary>
        private void lookupContext_CloseHandler(object sender)
        {
            LookupContext lookupContext = sender as LookupContext;
            object model = lookupContext.ContextValue;
            Type type = model.GetType();
            PropertyInfo[] pis = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo pi in pis)
            {
                if (pi.Name.Equals(DisplayMember))
                {
                    object PropertyValue = pi.GetValue(model, null);
                    bool next = true;
                    if (ChangeEvent != null)
                        next = !ChangeEvent(model);
                    if (next)
                    {
                        textBox.Text = PropertyValue.ToString();
                        this.Target = model;
                    }
                }
            }
            normoalHost.Close();
        }

        /// <summary>
        /// 单击按钮打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnShow_Click(object sender, EventArgs e)
        {
            ShowPopup("");
            this.textBox.Focus();
        }

        /// <summary>
        /// 离开事件关闭弹出式菜单
        /// </summary>
        private void this_Leave(object sender, EventArgs e)
        {
            object model = this.Target;
            if (model != null)
            {
                Type type = model.GetType();
                PropertyInfo[] pis = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo pi in pis)
                {
                    if (pi.Name.Equals(DisplayMember))
                    {
                        object PropertyValue = pi.GetValue(model, null);
                        textBox.Text = PropertyValue.ToString();
                    }
                }
                if (normoalHost != null)
                {
                    normoalHost.Close();
                }
            }
        }

        /// <summary>
        /// 文本值更改时发生
        /// </summary>
        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            object model = this.Target;
            if (model != null)
            {
                Type type = model.GetType();
                PropertyInfo[] pis = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo pi in pis)
                {
                    if (pi.Name.Equals(DisplayMember))
                    {
                        object PropertyValue = pi.GetValue(model, null);
                        if (!textBox.Text.Equals(PropertyValue.ToString()))
                        {
                            ShowPopup(textBox.Text);
                        }
                    }
                }
            }
            else { ShowPopup(textBox.Text); }
        }

        /// <summary>
        /// 弹出选择框,condition为查询的条件
        /// </summary>
        public void ShowPopup(string condition)
        {
            if (IsOpen)//为false时不打开PopupControlHost
            {
                this.Text = condition;    
                if (!isOpen)
                {
                    normoalHost.ChangeRegion = ChangeRegion;
                    normoalHost.Opacity = Opacity;
                    normoalHost.CanResize = CanResize;
                    normoalHost.OpenFocused = false;
                    normoalHost.AutoClose = false;//设置不自动关闭 
                    mf = new MessageFilter();
                    mf.MouseClickEvent = CloseHost;
                    Application.AddMessageFilter(mf);
                    hostPoint = normoalHost.Show(textBox, true);
                    isOpen = true;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanged(String propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// 只读方法
        /// </summary>
        public bool ReadOnly
        {
            get { return textBox.ReadOnly; }
            set
            {
                if (value)
                    btnShow.Enabled = false;
                else
                    btnShow.Enabled = true;
                textBox.ReadOnly = value;
            }
        }
    }
}
