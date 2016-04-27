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

        public CategoriesPresenter(ICategoriesForm view, bool forEdit)
        {
            _view = view;

            _view.CategorySelected += OnCategorySelected;

            if (forEdit)
            {
                _view.InitForEdit();
            }
        }

        private void OnCategorySelected()
        {
            
        }
    }
}
