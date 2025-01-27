using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Szef_kuchni.Core;
using Szef_kuchni.MVVM.Model;
using System.Data.SQLite;
using System.Windows;

namespace Szef_kuchni.MVVM.ViewModel
{
    internal class FavouriteViewModel : ObservableObject
    {
        private ObservableCollection<Recipe> _displayedRecipes;
        private ObservableCollection<Recipe> _favouriteRecipes;
        private ObservableCollection<Recipe> _filteredRecipes;
        private ObservableCollection<Recipe> _filteredSortedRecipes;
        private int _columnCount;
        private object _filterText;
        private string _dbFilePath;
        private int _currentPage;
        private const int RecipesPerPage = 20;

        private Datahelper _dataHelper;

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

        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }

        private int _numberOfRecipes;


        public FavouriteViewModel()
        {
            DisplayedRecipes = new ObservableCollection<Recipe>();
            _dbFilePath = @"../../recipes.db";
            _dataHelper = new Datahelper(_dbFilePath);
            _currentPage = 0;

            NextPageCommand = new RelayCommand(NextPage, CanGoNextPage);
            PreviousPageCommand = new RelayCommand(PreviousPage, CanGoPreviousPage);

            LoadFavouriteRecipes();
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

        internal void LoadFavouriteRecipes()
        {
            var favouriteRecipeIds = LoadFavourites(GetFavouritesFilePath());

            if (favouriteRecipeIds.Any())
            {
                var favouriteRecipes = favouriteRecipeIds.Select(id => _dataHelper.LoadRecipeById(id)).Where(recipe => recipe != null).ToList();

                _favouriteRecipes = new ObservableCollection<Recipe>(favouriteRecipes);
                _filteredRecipes = _favouriteRecipes;
                _displayedRecipes = _favouriteRecipes;
                _numberOfRecipes = _favouriteRecipes.Count;
                if (_numberOfRecipes % 20 == 0)
                {
                    BackOnePage();
                }


                UpdateDisplayedRecipes();
            }
            else
            {
                _favouriteRecipes = new ObservableCollection<Recipe>();
                _filteredRecipes = _favouriteRecipes;
                _displayedRecipes = _favouriteRecipes;
                _numberOfRecipes = 0;
                UpdateDisplayedRecipes();
            }
            UpdatePaginationVisibility();
        }

        private List<int> LoadFavourites(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    return JsonConvert.DeserializeObject<List<int>>(json) ?? new List<int>();
                }
                catch (Exception)
                {
                    return new List<int>();
                }
            }
            return new List<int>();
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



        internal void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(_filterText as string))
            {
                var limitedRecipes = _favouriteRecipes;

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
                var limitedRecipes = _favouriteRecipes
                    .Where(recipe => recipe.Title.IndexOf(FilterText as string, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

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

        private string GetFavouritesFilePath()
        {
            string userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appDirectory = Path.Combine(userDirectory, "SzefKuchni");
            if (!Directory.Exists(appDirectory))
            {
                Directory.CreateDirectory(appDirectory);
            }
            return Path.Combine(appDirectory, "Favourites.json");
        }

        private bool _isPaginationVisible;

        public bool IsPaginationVisible
        {
            get => _isPaginationVisible;
            set
            {
                _isPaginationVisible = value;
                OnPropertyChanged(nameof(IsPaginationVisible));
            }
        }

        private void UpdatePaginationVisibility()
        {
            IsPaginationVisible = _numberOfRecipes > 20;
        }

        public void BackOnePage()
        {
            _currentPage--;
            UpdateDisplayedRecipes();
        }

    }
}
