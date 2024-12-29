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
        private ObservableCollection<Recipe> _allRecipes;
        private ObservableCollection<Recipe> _allRecipes2;
        private int _columnCount;

        public ObservableCollection<Recipe> AllRecipes
        {
            get => _allRecipes;
            set
            {
                _allRecipes = value;
                OnPropertyChanged();
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
        private const int RecipesPerPage = 15;

        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }

        public SearchViewModel()
        {
            AllRecipes = new ObservableCollection<Recipe>();
            string dbPath = @"../../recipes.db";
            _dataHelper = new Datahelper(dbPath);

            _currentPage = 0;

            NextPageCommand = new RelayCommand(NextPage, CanGoNextPage);
            PreviousPageCommand = new RelayCommand(PreviousPage, CanGoPreviousPage);

            LoadAllRecipes();
        }

        private void LoadAllRecipes()
        {
            _allRecipes = _dataHelper.LoadRecipes();
            _allRecipes2 = _allRecipes;
            _numberOfRecipes = _allRecipes.Count();
            UpdateDisplayedRecipes();
        }

        private void UpdateDisplayedRecipes()
        {
            var skip = _currentPage * RecipesPerPage;
            var recipesToShow = _allRecipes2.Skip(skip).Take(RecipesPerPage);
            AllRecipes = new ObservableCollection<Recipe>(recipesToShow);
            OnPropertyChanged(nameof(AllRecipes));
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
