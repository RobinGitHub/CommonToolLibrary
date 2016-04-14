using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace RichTextBox消息处理
{
    public partial class Form2 : Form
    {
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, string lParam);

        public Form2()
        {
            InitializeComponent();
            this.Load += Form2_Load;
        }

        void Form2_Load(object sender, EventArgs e)
        {
            Font font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

            RichTextBoxEx richTextBoxEx1 = new RichTextBoxEx();
            richTextBoxEx1.MustHideCaret = true;
            richTextBoxEx1.Font = font;
            richTextBoxEx1.ScrollBars = RichTextBoxScrollBars.None;
            richTextBoxEx1.Text = @"英文全称为 stock keeping unit, 简称SKU，定义为保存库存控制的最小可用单位，例如纺织品中一个SKU通常表示：规格、颜色、款式。 STOCK KEEP UNIT.这是客户拿到商品放到仓库后给商品编号,归类的一种方法. 通常是SKU#是多少多少这样子. 还有的译为存货单元\库存单元\库存单位\货物存储单位\存货保存单位\单元化单位\单品\品种,基于业务还有的是最小零售单位\最小销售单位\最小管理单位\库存盘点单位等；专业物流术语解释为“货格”。\n换言之，有助于理解：\n首先我们应当了解单品的定义，即指包含特定的自然属性与社会属性的商品种类。对一种商品而言，当其品牌、型号、配置、等级、花色、包装容量、单位、生产日期、保质期、用途、价格、产地等属性与其他商品存在不同时，可称为一个单品。在连锁零售门店中有时称单品为一个SKU（中文译为最小存货单位，例如纺织品中一个SKU通常表示规格，颜色，款式）。当然，单品与传统意义上的品种的概念是不同的，用单品这一概念可以区分不同商品的不同属性，从而为商品采购、销售、物流管理、财务管理以及POS系统与MIS系统的开发提供极大的便利。例如：单听销售的可口可乐是一个单品SKU，而整扎销售的可口可乐又是一个单品，这两个单品在库存管理和销售是不一样的。而在传统意义上的品种听装的可口可乐是一个品种，不管其销售模式是什么样的。\n我们不难看出，无论是国外还是国内的定义和解释中，基本上是三个概念：品项、编码、单位。\n这三";
            richTextBoxEx1.Width = 300;
            int EM_GETLINECOUNT = 0x00BA;//获取总行数的消息号 
            int lc = SendMessage(richTextBoxEx1.Handle, EM_GETLINECOUNT, IntPtr.Zero, "");
            int sf = 23 * lc + 5;
            richTextBoxEx1.Height = sf;
            this.Controls.Add(richTextBoxEx1);
        }
    }
}
