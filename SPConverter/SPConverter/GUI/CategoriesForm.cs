using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SPConverter.Model;
using SPConverter.Services.Dictionaries;

namespace SPConverter.GUI
{
    public partial class CategoriesForm : Form, ICategoriesForm
    {
        public CategoriesForm()
        {
            InitializeComponent();
        }

        public TreeNodeCollection NodeCollection => treeView1.Nodes;

        public TreeNode RootNode => NodeCollection[0];

        private void FillTreeView(Category category, TreeNode treeNode)
        {
            for (int i = 0; i < category.Categories.Count; i++)
            {
                var newNode = treeNode.Nodes.Add(category.Categories[i].Name);
                newNode.Tag = category.Categories[i];
                FillTreeView(category.Categories[i], treeNode.Nodes[i]);
            }
        }

        private TreeNode SearchNode(Category searchCategory, TreeNode startNode)
        {
            TreeNode node = null;
            while (startNode != null)
            {
                if (startNode.Tag == searchCategory)
                {
                    node = startNode; //чето нашли, выходим
                    break;
                }
                if (startNode.Nodes.Count != 0) //у узла есть дочерние элементы
                {
                    node = SearchNode(searchCategory, startNode.Nodes[0]);//ищем рекурсивно в дочерних
                    if (node != null)
                        break; //чето нашли
                }
                startNode = startNode.NextNode;
            }
            return node;//вернули результат поиска
        }

        private void ExpandToExactNode(TreeNode node)
        {
            node.Parent.Expand();
            if (node.Parent != RootNode)
            {
                ExpandToExactNode(node.Parent);
            }
        }

        public void LoadTree(Category catalog)
        {
            NodeCollection.Clear();

            var rootNode = new TreeNode(CatalogDictionary.Instance.Catalog.Name)
            {
                Tag = CatalogDictionary.Instance.Catalog
            };
            NodeCollection.Add(rootNode);

            FillTreeView(CatalogDictionary.Instance.Catalog, NodeCollection[0]);
        }

        public void HighlightNode(Category category, Color color)
        {
            var node = SearchNode(category, RootNode);
            if (node != null)
            {
                node.NodeFont = new Font(treeView1.Font, FontStyle.Bold);
                node.ForeColor = color;
            }
        }

        public void ExpandToNode(Category category)
        {
            var node = SearchNode(category, RootNode);
            ExpandToExactNode(node);
        }

        public Category SelectedCategory
        {
            get { return (Category) treeView1.SelectedNode.Tag; }
            set
            {
                var node = SearchNode(value, RootNode);
                treeView1.SelectedNode = node;
            }
        }

        public event Action CategorySelected;

        private void btClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CategorySelected?.Invoke();
            DialogResult = DialogResult.OK;
        }
    }
}
