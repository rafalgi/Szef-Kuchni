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
        private readonly Datahelper _dataHelper;
        private ObservableCollection<Recipe> AllRecipes;
        private ObservableCollection<Recipe> _topRatedRecipes;
        private int _columnCount;
        private object _filterText;

        public ObservableCollection<Recipe> TopRatedRecipes
        {
            get => _topRatedRecipes;
            set
            {
                _topRatedRecipes = value;
                OnPropertyChanged(nameof(TopRatedRecipes));
            }
        }

        public int ColumnCount
        {
            get => _columnCount;
            set
            {
                _columnCount = value;
                OnPropertyChanged();
            }
        }

        public object FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                OnPropertyChanged();
                ApplyFilter();
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
            AllRecipes = _dataHelper.LoadRecipes();


            var limitedRecipes = AllRecipes
                .OrderByDescending(recipe => recipe.RatingCount) 
                .Take(20); 


            TopRatedRecipes = new ObservableCollection<Recipe>(limitedRecipes);
        }

        public void SetColumnCount(double windowWidth)
        {
            if (windowWidth > 1280)
            {
                ColumnCount = 5;
            }
            else
            {
                ColumnCount = 4;
            }
        }

        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(_filterText as string))
            {
                var limitedRecipes = AllRecipes
                    .OrderByDescending(recipe => recipe.RatingCount)
                    .Take(20);

                TopRatedRecipes = new ObservableCollection<Recipe>(limitedRecipes);
            }
            else
            {
                var filteredRecipes = AllRecipes
                    .Where(recipe => recipe.Title.IndexOf(FilterText as string, StringComparison.OrdinalIgnoreCase) >= 0)
                    .OrderByDescending(recipe => recipe.RatingCount)
                    .Take(20);

                TopRatedRecipes = new ObservableCollection<Recipe>(filteredRecipes);
            }
        }

    }
}
