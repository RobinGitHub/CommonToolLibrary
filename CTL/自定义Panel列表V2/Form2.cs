using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace 自定义Panel列表V2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            Deserialize(treeView1, "tree.xml");

            this.treeView1.MouseWheel += treeView1_MouseWheel;
        }

        void treeView1_MouseWheel(object sender, MouseEventArgs e)
        {
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
    }
}
