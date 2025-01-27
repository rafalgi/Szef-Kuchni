using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Szef_kuchni.Core;

namespace Szef_kuchni.MVVM.ViewModel
{
    internal class SearchViewModel : ObservableObject
    {
        private ObservableCollection<Recipe> _displayedRecipes;
        private ObservableCollection<Recipe> _allRecipes;
        private ObservableCollection<Recipe> _filteredRecipes;
        private ObservableCollection<Recipe> _filteredSortedRecipes;
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

        public ObservableCollection<Recipe> FilteredSortedRecipes
        {
            get => _filteredSortedRecipes;
            set
            {
                if (_filteredSortedRecipes != value)
                {
                    _filteredSortedRecipes = value;
                    OnPropertyChanged(nameof(FilteredSortedRecipes));
                    ApplyFilter();
                }
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

        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(_filterText as string))
            {
                var limitedRecipes = _allRecipes;

                // Element filtrujący przepisy według zakładki "Szukaj"
                if (_filteredSortedRecipes != null && _filteredSortedRecipes.Count != 1307)
                {
                    _filteredRecipes = new ObservableCollection<Recipe>(_filteredSortedRecipes.Where(recipe => limitedRecipes.Any(limited => limited.Id == recipe.Id)).OrderBy(item => _filteredSortedRecipes.IndexOf(item)));
                }
                else
                {
                    _filteredRecipes = new ObservableCollection<Recipe>(limitedRecipes);
                }
            }
            else
            {
                // Element filtrujący przepisy według textboxa
                var limitedRecipes = _allRecipes
                    .Where(recipe => recipe.Title.IndexOf(FilterText as string, StringComparison.OrdinalIgnoreCase) >= 0);

                // Element filtrujący przepisy według zakładki "Szukaj"
                if (_filteredSortedRecipes != null && _filteredSortedRecipes.Count != 1307)
                {
                    _filteredRecipes = new ObservableCollection<Recipe>(_filteredSortedRecipes.Where(recipe => limitedRecipes.Any(limited => limited.Id == recipe.Id)).OrderBy(item => _filteredSortedRecipes.IndexOf(item)));
                }
                else
                {
                    _filteredRecipes = new ObservableCollection<Recipe>(limitedRecipes);
                }
            }
            _currentPage = 0;
            _numberOfRecipes = _filteredRecipes.Count();
            UpdateDisplayedRecipes();
        }

    }
}
