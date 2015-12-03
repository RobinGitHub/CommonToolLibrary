using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace 自定义TreeView仿VS解决方案效果
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.myVScrollBar1.BindControl = this.treeViewEx1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Deserialize(treeViewEx1, "tree.xml");
            this.myVScrollBar1.UpdateScrollbar();
        }

        public void Deserialize(TreeView tv, string fn)
        {
            tv.Nodes.Clear();
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load("tree.xml");
            XmlNode node = xdoc.DocumentElement;
            ReadNode(xdoc, node, tv.Nodes);
        }

        protected void ReadNode(XmlDocument xdoc, XmlNode node, TreeNodeCollection nodes)
        {
            foreach (XmlNode xn in node.ChildNodes)
            {
                TreeNode tn = new TreeNode();
                tn.Text = xn.Attributes["Text"].Value;
                tn.Tag = xn.Name;
                nodes.Add(tn);

                ReadNode(xdoc, xn, tn.Nodes);

                if (Convert.ToBoolean(xn.Attributes["IsExpanded"].Value))
                {
                    tn.Expand();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.AppendText(this.treeViewEx1.VerticalScrollVisible.ToString() + "\n");
            richTextBox1.ScrollToCaret();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.AppendText(this.treeViewEx1.HorizontalScrollVisible.ToString() + "\n");
            richTextBox1.ScrollToCaret();
        }
    }
}
