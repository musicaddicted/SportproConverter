using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPConverter.GUI
{
    public class CategoriesPresenter
    {
        private ICategoriesForm _view;

        public CategoriesPresenter(ICategoriesForm view)
        {
            _view = view;

            _view.CategorySelected += OnCategorySelected;
        }

        private void OnCategorySelected()
        {
            
        }
    }
}
