using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Szef_kuchni.Core;
using Szef_kuchni.MVVM.Model;

namespace Szef_kuchni.MVVM.ViewModel
{
    internal class SearchViewModel : ObservableObject

    {
        private ObservableCollection<Ingredient> _allIngredients;
        private ObservableCollection<Tag> _allTags;
        private ObservableCollection<Recipe> _displayedRecipes;
        private ObservableCollection<Recipe> _allRecipes;
        private ObservableCollection<Recipe> _filteredRecipes;
        private int _columnCount;
        private object _filterText;

        public ObservableCollection<Recipe> DisplayedRecipes
        {
            get => _displayedRecipes;
            set
            {
                _displayedRecipes = value;
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

        public int ColumnCount
        {
            get => _columnCount;
            set
            {
                _columnCount = value;
                OnPropertyChanged();
            }
        }

        private int _numberOfRecipes;
        private readonly Datahelper _dataHelper;
        private int _currentPage;
        private const int RecipesPerPage = 20;

        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }

        public SearchViewModel()
        {
            DisplayedRecipes = new ObservableCollection<Recipe>();
            string dbPath = @"../../recipes.db";
            _dataHelper = new Datahelper(dbPath);

            _currentPage = 0;

            NextPageCommand = new RelayCommand(NextPage, CanGoNextPage);
            PreviousPageCommand = new RelayCommand(PreviousPage, CanGoPreviousPage);

            LoadAllIngredients();
            LoadAllTags();
            LoadAllRecipes();
        }

        private void LoadAllRecipes()
        {
            _allRecipes =  _dataHelper.LoadRecipes();
            _displayedRecipes = _allRecipes;
            _filteredRecipes = _allRecipes;
            _numberOfRecipes = _allRecipes.Count();
            UpdateDisplayedRecipes();
        }

        private void UpdateDisplayedRecipes()
        {
            var skip = _currentPage * RecipesPerPage;
            var recipesToShow = _filteredRecipes.Skip(skip).Take(RecipesPerPage);
            DisplayedRecipes = new ObservableCollection<Recipe>(recipesToShow);
            OnPropertyChanged(nameof(DisplayedRecipes));
        }

        private void NextPage(object parameter)
        {
            _currentPage++;
            UpdateDisplayedRecipes();
        }

        private bool CanGoNextPage(object parameter)
        {
            return (_currentPage + 1) * RecipesPerPage < _numberOfRecipes;
        }

        private void PreviousPage(object parameter)
        {
            _currentPage--;
            UpdateDisplayedRecipes();
        }

        private bool CanGoPreviousPage(object parameter)
        {
            return _currentPage > 0;
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
        private void LoadAllIngredients()
        {
            _allIngredients = _dataHelper.LoadIngredientsForRecipes();
        }

        private void LoadAllTags()
        {
            _allTags = _dataHelper.LoadTagsForRecipes();
        }
        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(_filterText as string))
            {
                _filteredRecipes = new ObservableCollection<Recipe>(_allRecipes);
            }
            else
            {
                var filterText = _filterText as string;
                var filteredByTitle = _allRecipes
                    .Where(recipe => recipe.Title.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0);

                var filteredByIngredients = _allRecipes
                    .Where(recipe => _allIngredients
                        .Any(ingredient => ingredient.Id == recipe.Id && ingredient.Ingredients.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0));

                var filteredByTags = _allRecipes
                    .Where(recipe => _allTags
                        .Any(tag => tag.Id == recipe.Id && tag.Name.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0));

                var combinedFilteredRecipes = filteredByTitle
                    .Union(filteredByIngredients)
                    .Union(filteredByTags);

                _filteredRecipes = new ObservableCollection<Recipe>(combinedFilteredRecipes);
            }

            _currentPage = 0;
            _numberOfRecipes = _filteredRecipes.Count();
            UpdateDisplayedRecipes();
        }

    }
}
