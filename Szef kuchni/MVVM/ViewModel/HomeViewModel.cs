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


        public ObservableCollection<Recipe> Recipes { get; set; }
        private ObservableCollection<Recipe> _allRecipes;

        private readonly Datahelper _dataHelper;

        // konstruktor
        public HomeViewModel()
        {
            
            Recipes = new ObservableCollection<Recipe>();

            string dbPath = @"../../recipes.db"; // ścieżka do bazy danych
            _dataHelper = new Datahelper(dbPath);

            LoadRecipes();

        }

        
        private void LoadRecipes()
        {

            _allRecipes = _dataHelper.LoadRecipesTopRated();
            Recipes = new ObservableCollection<Recipe>(_allRecipes);
        }



    }
}
