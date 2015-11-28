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
    public partial class CategoriesForm : Form
    {
        public CategoriesForm()
        {
            InitializeComponent();
            LoadTreeView();
        }

        public TreeNodeCollection NodeCollection => treeView1.Nodes;

        public TreeNode RootNode => NodeCollection[0];

        private void LoadTreeView()
        {
            NodeCollection.Clear();

            var rootNode = new TreeNode(CatalogDictionary.Instance.Catalog.Name)
            {
                Tag = CatalogDictionary.Instance.Catalog
            };
            NodeCollection.Add(rootNode);

            FillTreeView(CatalogDictionary.Instance.Catalog, NodeCollection[0]);

            var treeNodeTest = SearchNode(CatalogDictionary.Instance.AllCategoriesList.Find(c => c.Name.ToLower().StartsWith("футбол")),
                rootNode);

            treeNodeTest.ForeColor = Color.Coral;
            treeView1.SelectedNode = treeNodeTest;

            ExpandToExactNode(treeNodeTest);
        }

        private void FillTreeView(Category category, TreeNode treeNode)
        {
            for (int i = 0; i < category.Categories.Count; i++)
            {
                var newNode = treeNode.Nodes.Add(category.Categories[i].Name);
                newNode.Tag = category.Categories[i];
                FillTreeView(category.Categories[i], treeNode.Nodes[i]);
            }
        }

        private TreeNode SearchNode(Category SearchCategory, TreeNode StartNode)
        {
            TreeNode node = null;
            while (StartNode != null)
            {
                if (StartNode.Tag == SearchCategory)
                {
                    node = StartNode; //чето нашли, выходим
                    break;
                };
                if (StartNode.Nodes.Count != 0) //у узла есть дочерние элементы
                {
                    node = SearchNode(SearchCategory, StartNode.Nodes[0]);//ищем рекурсивно в дочерних
                    if (node != null)
                    {
                        break;//чето нашли
                    };
                };
                StartNode = StartNode.NextNode;
            };
            return node;//вернули результат поиска
        }

        private void ExpandToExactNode(TreeNode node)
        {
            node.Parent.Expand();
            while (node.Parent != RootNode)
            {
                ExpandToExactNode(node.Parent);
            }
        }


        private void HighLightNode(string name)
        {
        }


        /// <summary>
        /// Рекурсивно заполняем теги, состоящие из названий категорий
        /// </summary>
        private void FillTagsAndParents(Category category)
        {
            category.Tags.Add(category.Name);
            category.PluginExportString += category.Name + ">";

            if (category.Categories.Count == 0)
                category.PluginExportString = category.PluginExportString.Remove(category.PluginExportString.Length - 1);

            category.Categories.ForEach(children =>
            {
                children.Tags.AddRange(category.Tags);
                children.Tags.ForEach(t => children.PluginExportString += t + ">");

                children.Parent = category;
            });
        }
    }
}
