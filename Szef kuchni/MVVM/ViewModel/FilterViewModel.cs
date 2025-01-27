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
    internal class FilterViewModel : ObservableObject
    {
        private readonly Datahelper _dataHelper;
        private ObservableCollection<Recipe> _filteredSortedRecipes;
        private ObservableCollection<Recipe> _allRecipes;
        public ICommand ResetAllCommand { get; }
        private string _difficultyLower;
        private string _difficultyUpper;
        private int _timeLower;
        private int _timeUpper;

        public ObservableCollection<Recipe> FilteredSortedRecipes
        {
            get => _filteredSortedRecipes;
            set
            {
                _filteredSortedRecipes = value;
                OnPropertyChanged();
            }
        }

        public string DifficultyLower
        {
            get => _difficultyLower;
            set
            {
                _difficultyLower = value;
                OnPropertyChanged();

                DifficultyUpper = DifficultyLower;
            }
        }

        public string DifficultyUpper
        {
            get => _difficultyUpper;
            set
            {
                _difficultyUpper = value;
                OnPropertyChanged();
                FilterRecipes();
            }
        }

        public int TimeLower
        {
            get => _timeLower;
            set
            {
                _timeLower = value;
                OnPropertyChanged();

                TimeUpper = TimeLower;
            }
        }

        public int TimeUpper
        {
            get => _timeUpper;
            set
            {
                _timeUpper = value;
                OnPropertyChanged();
                FilterRecipes();
            }
        }

        public FilterViewModel()
        {
            string dbPath = @"../../recipes.db"; // ścieżka do bazy danych
            _dataHelper = new Datahelper(dbPath);
            _allRecipes = new ObservableCollection<Recipe>(_dataHelper.LoadRecipes());
            ResetAllCommand = new RelayCommand(ResetAll);
        }

        private void FilterRecipes()
        {
            FilteredSortedRecipes = _allRecipes;
            FilterByDifficulty();
            FilterByTime();
        }

        private void FilterByDifficulty()
        {
            if (DifficultyLower != null)
            {
                if (DifficultyLower == "Łatwy" && DifficultyUpper == "Łatwy")
                {
                    FilteredSortedRecipes = new ObservableCollection<Recipe>(FilteredSortedRecipes.Where(recipe => recipe.Difficulty == "Łatwy"));
                }
                if (DifficultyLower == "Średni" && DifficultyUpper == "Średni")
                {
                    FilteredSortedRecipes = new ObservableCollection<Recipe>(FilteredSortedRecipes.Where(recipe => recipe.Difficulty == "Średni"));
                }
                if (DifficultyLower == "Trudny" && DifficultyUpper == "Trudny")
                {
                    FilteredSortedRecipes = new ObservableCollection<Recipe>(FilteredSortedRecipes.Where(recipe => recipe.Difficulty == "Trudny"));
                }
                if (DifficultyLower == "Łatwy" && DifficultyUpper == "Średni")
                {
                    FilteredSortedRecipes = new ObservableCollection<Recipe>(FilteredSortedRecipes.Where(recipe => recipe.Difficulty == "Łatwy" || recipe.Difficulty == "Średni"));
                }
                if (DifficultyLower == "Łatwy" && DifficultyUpper == "Trudny")
                {
                    FilteredSortedRecipes = new ObservableCollection<Recipe>(FilteredSortedRecipes.Where(recipe => recipe.Difficulty == "Łatwy" || recipe.Difficulty == "Średni" || recipe.Difficulty == "Trudny"));
                }
                if (DifficultyLower == "Średni" && DifficultyUpper == "Trudny")
                {
                    FilteredSortedRecipes = new ObservableCollection<Recipe>(FilteredSortedRecipes.Where(recipe => recipe.Difficulty == "Średni" || recipe.Difficulty == "Trudny"));
                }
            }
        }

        private void FilterByTime()
        {
            if ((TimeLower == 0 && TimeUpper > 0) || TimeLower < TimeUpper)
            {
                FilteredSortedRecipes = new ObservableCollection<Recipe>(FilteredSortedRecipes.Where(recipe => recipe.PrepTime >= TimeLower && recipe.PrepTime <= TimeUpper));
            }
            
        }

        private void ResetAll(object parameter)
        {
            DifficultyLower = null;
            DifficultyUpper = null;
            TimeLower = 0;
            TimeUpper = 0;
            FilteredSortedRecipes = _allRecipes;
        }
    }
}
