using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Szef_kuchni.Core;

namespace Szef_kuchni.MVVM.ViewModel
{
    internal class SearchViewModel
    {
        public ObservableCollection<Recipe> AllRecipes { get; set; }
        private ObservableCollection<Recipe> _allRecipes;
        private readonly Datahelper _dataHelper;


        public SearchViewModel()
        {
            AllRecipes = new ObservableCollection<Recipe>();
            string dbPath = @"../../recipes.db";
            _dataHelper = new Datahelper(dbPath);
            LoadAllRecipes();
        }

        private void LoadAllRecipes()
        {

            _allRecipes = _dataHelper.LoadRecipes();
            AllRecipes = new ObservableCollection<Recipe>(_allRecipes);
        }

    }
}
