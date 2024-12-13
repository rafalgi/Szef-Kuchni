using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Szef_kuchni.Core;
using Szef_kuchni.MVVM.View;

namespace Szef_kuchni.MVVM.ViewModel
{
    internal class HomeViewModel : ObservableObject
    {
        public ObservableCollection<Recipe> TopRatedRecipes { get; set; }
        private ObservableCollection<Recipe> _topRatedRecipes;

        private readonly Datahelper _dataHelper;

        // konstruktor
        public HomeViewModel()
        {
            TopRatedRecipes = new ObservableCollection<Recipe>();

            string dbPath = @"../../recipes.db"; // ścieżka do bazy danych
            _dataHelper = new Datahelper(dbPath);

            LoadTopRatedRecipes();
        }

        
        private void LoadTopRatedRecipes()
        {

            _topRatedRecipes = _dataHelper.LoadRecipesTopRated();
            TopRatedRecipes = new ObservableCollection<Recipe>(_topRatedRecipes);
        }

    }
}
