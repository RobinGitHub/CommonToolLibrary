﻿using System;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.treeViewMenu.MouseWheel += treeViewMenu_MouseWheel;
        }

        void treeViewMenu_MouseWheel(object sender, MouseEventArgs e)
        {
            richTextBox1.AppendText(this.treeViewMenu.VerticalScrollValue.ToString() + "\n");
            richTextBox1.ScrollToCaret();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            Deserialize(treeViewEx1, "tree.xml");
            Deserialize(treeViewMenu, "tree.xml");
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
                    //tn.Expand();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            treeViewMenu.SelectedNode.Text = "asdf";
            treeViewEx1.SelectedNode.Text = "asdf";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.treeViewMenu.VerticalScrollValue = int.Parse(txtPos.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.AppendText(this.treeViewMenu.VerticalScrollValue.ToString() + "\n");
            richTextBox1.ScrollToCaret();
        }
    }
}
