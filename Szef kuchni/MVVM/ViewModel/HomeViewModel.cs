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
        private int _columnCount;

        public int ColumnCount
        {
            get => _columnCount;
            set
            {
                _columnCount = value;
                OnPropertyChanged();
            }
        }

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
            _topRatedRecipes = _dataHelper.LoadRecipes();


            var limitedRecipes = _topRatedRecipes
                .OrderByDescending(recipe => recipe.RatingCount) 
                .Take(15); 


            TopRatedRecipes = new ObservableCollection<Recipe>(limitedRecipes);
        }

        public void SetColumnCount(double windowWidth)
        {
            if (windowWidth > 1200)
            {
                ColumnCount = 5;
            }
            else
            {
                ColumnCount = 3;
            }
        }

    }
}
