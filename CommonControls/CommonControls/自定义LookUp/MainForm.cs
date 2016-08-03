using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommonControls.自定义LookUp
{
    public partial class MainForm : Form
    {
        List<Storage> list = new List<Storage>();
        public MainForm()
        {
            InitializeComponent();
            this.Load += MainForm_Load;
        }

        void MainForm_Load(object sender, EventArgs e)
        {
            //测试数据
            Storage storage = new Storage();
            storage.StorageNO = "001";
            storage.StorageName = "原料仓";
            storage.Remark = "原料仓";
            list.Add(storage);

            storage = new Storage();
            storage.StorageNO = "002";
            storage.StorageName = "成品仓";
            storage.Remark = "成品仓";
            list.Add(storage);

            storage = new Storage();
            storage.StorageNO = "003";
            storage.StorageName = "半成品仓";
            storage.Remark = "半成品仓";
            list.Add(storage);

            storage = new Storage();
            storage.StorageNO = "004";
            storage.StorageName = "废品仓";
            storage.Remark = "废品仓";
            list.Add(storage);

            this.lookupEditor1.Mapping.Add("编码", "StorageNO");
            this.lookupEditor1.Mapping.Add("仓库名称", "StorageName");
            this.lookupEditor1.Mapping.Add("备注", "Remark");
            this.lookupEditor1.DisplayMember = "StorageName";
            this.lookupEditor1.IsOpen = true;
            this.lookupEditor1.PropertyChanged += new PropertyChangedEventHandler(lookupEditor1_PropertyChanged);
        }
        void lookupEditor1_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string condition = lookupEditor1.Text;
            this.lookupEditor1.DataSource = list.Where(s => s.StorageName.Contains(condition) || s.StorageNO.Contains(condition));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Storage s = lookupEditor1.Target as Storage;
            if (s != null)
                MessageBox.Show(s.StorageName);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //==如果一个窗体有多个lookupEditor,重置方法可以判断控件类型，然后批处理
            lookupEditor1.IsOpen = false;//先关闭
            lookupEditor1.DisplayValue = string.Empty;
            lookupEditor1.Text = string.Empty;
            lookupEditor1.Target = null;
            lookupEditor1.IsOpen = true;// 打开
        }
    }

    public class Storage
    {
        public string StorageNO { set; get; }
        public string StorageName { set; get; }
        public string Remark { set; get; }
    }
}
