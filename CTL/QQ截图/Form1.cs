using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

/* 功能说明：
 * 1.遮罩
 * 2.根据窗体大小，自动设置截图区域，并加粗显示
 * 3.左键点击后确定选区，选区变蓝，可调整
 * 4.在调整时，要增加工具条
 * 5.取色
 * 6.双击，如果直接从对话窗口截屏，双击后显示在内容区域，如果是快捷键截屏，则保存在内存中
 * 7.双击图片可放大查看
 * 
 */


namespace QQ截图
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.btnScreenCut.Click += btnScreenCut_Click;
        }

        void btnScreenCut_Click(object sender, EventArgs e)
        {

        }
    }
}
