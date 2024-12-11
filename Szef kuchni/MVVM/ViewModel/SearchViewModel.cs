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
        public ObservableCollection<Recipe> Recipes { get; set; }
        private ObservableCollection<Recipe> _allRecipes;

        private readonly Datahelper _dataHelper;

        public SearchViewModel()
        {
            Recipes = new ObservableCollection<Recipe>();

            string dbPath = @"../../recipes.db"; // ścieżka do bazy danych
            _dataHelper = new Datahelper(dbPath);

            LoadRecipes();
        }

        private void LoadRecipes()
        {

            _allRecipes = _dataHelper.LoadRecipes();
            Recipes = new ObservableCollection<Recipe>(_allRecipes);
        }

    }
}
