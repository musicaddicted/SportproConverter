using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SPConverter.Model;

namespace SPConverter.GUI
{
    public interface ICategoriesForm
    {
        DialogResult ShowDialog();

        void LoadTree(Category catalog);

        void Close();

        void HighlightNode(Category category, Color color);

        void ExpandToNode(Category category);

        Category SelectedCategory { get; set; }
         
        event Action CategorySelected;

    }
}
