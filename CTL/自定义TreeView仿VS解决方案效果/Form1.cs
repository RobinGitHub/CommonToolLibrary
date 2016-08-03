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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.treeViewMenu.MouseWheel += treeViewMenu_MouseWheel;
            this.treeViewEx1.ItemDrag += treeViewEx1_ItemDrag;
            this.treeViewEx1.DragEnter += treeViewEx1_DragEnter;
            this.treeViewEx1.DragDrop += treeViewEx1_DragDrop;
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

            treeViewEx1.ExpandAll();
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

        #region 拖拽
        private Point Position = new Point(0, 0);
        void treeViewEx1_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode myNode = null;
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                myNode = (TreeNode)(e.Data.GetData(typeof(TreeNode)));
            }
            else
            {
                MessageBox.Show("error");
            }
            Position.X = e.X;
            Position.Y = e.Y;
            Position = treeViewEx1.PointToClient(Position);
            TreeNode DropNode = this.treeViewEx1.GetNodeAt(Position);
            // 1.目标节点不是空。2.目标节点不是被拖拽接点的字节点。3.目标节点不是被拖拽节点本身
            if (DropNode != null && DropNode.Parent != myNode && DropNode != myNode)
            {
                TreeNode DragNode = myNode;
                // 将被拖拽节点从原来位置删除。
                myNode.Remove();
                // 在目标节点下增加被拖拽节点
                DropNode.Nodes.Add(DragNode);
            }
            // 如果目标节点不存在，即拖拽的位置不存在节点，那么就将被拖拽节点放在根节点之下
            if (DropNode == null)
            {
                TreeNode DragNode = myNode;
                myNode.Remove();
                treeViewEx1.Nodes.Add(DragNode);
            }
        }

        void treeViewEx1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        void treeViewEx1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }
        #endregion
    }
}
