using iText.Commons.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
        private string _ratingLowerString;
        private string _ratingUpperString;
        private float _ratingLower;
        private float _ratingUpper;
        private int _servingsLower;
        private int _servingsUpper;
        private int _stepsCountLower;
        private int _stepsCountUpper;
        private int _ratingCountLower;
        private int _ratingCountUpper;
        private string _difficultySort;
        private string _timeSort;
        private string _ratingSort;
        private string _servingsSort;
        private string _stepsCountSort;
        private string _ratingCountSort;
        private bool _isResetting;

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

        public string RatingLowerString
        {
            get => _ratingLowerString;
            set
            {
                _ratingLowerString = value;
                OnPropertyChanged();
                if (float.TryParse(RatingLowerString, out float ratingLower))
                {
                    _ratingLower = ratingLower;
                }
                RatingUpperString = RatingLowerString;
            }
        }

        public string RatingUpperString
        {
            get => _ratingUpperString;
            set
            {
                _ratingUpperString = value;
                if (float.TryParse(RatingUpperString, out float ratingUpper))
                {
                    _ratingUpper = ratingUpper;
                }
                OnPropertyChanged();
                FilterRecipes();
            }
        }

        public int ServingsLower
        {
            get => _servingsLower;
            set
            {
                _servingsLower = value;
                OnPropertyChanged();

                ServingsUpper = ServingsLower;
            }
        }

        public int ServingsUpper
        {
            get => _servingsUpper;
            set
            {
                _servingsUpper = value;
                OnPropertyChanged();
                FilterRecipes();
            }
        }

        public int StepsCountLower
        {
            get => _stepsCountLower;
            set
            {
                _stepsCountLower = value;
                OnPropertyChanged();

                StepsCountUpper = StepsCountLower;
            }
        }

        public int StepsCountUpper
        {
            get => _stepsCountUpper;
            set
            {
                _stepsCountUpper = value;
                OnPropertyChanged();
                FilterRecipes();
            }
        }

        public int RatingCountLower
        {
            get => _ratingCountLower;
            set
            {
                _ratingCountLower = value;
                OnPropertyChanged();

                RatingCountUpper = RatingCountLower;
            }
        }

        public int RatingCountUpper
        {
            get => _ratingCountUpper;
            set
            {
                _ratingCountUpper = value;
                OnPropertyChanged();
                FilterRecipes();
            }
        }

        public string DifficultySort
        {
            get => _difficultySort;
            set
            {
                if (_difficultySort == value) return;
                _difficultySort = value;
                OnPropertyChanged();

                if (!_isResetting)
                {
                    _isResetting = true;
                    ResetSortProperties(nameof(DifficultySort));
                    _isResetting = false;
                    FilterRecipes();
                }
            }
        }

        public string TimeSort
        {
            get => _timeSort;
            set
            {
                if (_timeSort == value) return;
                _timeSort = value;
                OnPropertyChanged();

                if (!_isResetting)
                {
                    _isResetting = true;
                    ResetSortProperties(nameof(TimeSort));
                    _isResetting = false;
                    FilterRecipes();
                }
            }
        }

        public string RatingSort
        {
            get => _ratingSort;
            set
            {
                if (_ratingSort == value) return;
                _ratingSort = value;
                OnPropertyChanged();

                if (!_isResetting)
                {
                    _isResetting = true;
                    ResetSortProperties(nameof(RatingSort));
                    _isResetting = false;
                    FilterRecipes();
                }
            }
        }

        public string ServingsSort
        {
            get => _servingsSort;
            set
            {
                if (_servingsSort == value) return;
                _servingsSort = value;
                OnPropertyChanged();

                if (!_isResetting)
                {
                    _isResetting = true;
                    ResetSortProperties(nameof(ServingsSort));
                    _isResetting = false;
                    FilterRecipes();
                }
            }
        }

        public string StepsCountSort
        {
            get => _stepsCountSort;
            set
            {
                if (_stepsCountSort == value) return;
                _stepsCountSort = value;
                OnPropertyChanged();

                if (!_isResetting)
                {
                    _isResetting = true;
                    ResetSortProperties(nameof(StepsCountSort));
                    _isResetting = false;
                    FilterRecipes();
                }
            }
        }

        public string RatingCountSort
        {
            get => _ratingCountSort;
            set
            {
                if (_ratingCountSort == value) return;
                _ratingCountSort = value;
                OnPropertyChanged();

                if (!_isResetting)
                {
                    _isResetting = true;
                    ResetSortProperties(nameof(RatingCountSort));
                    _isResetting = false;
                    FilterRecipes();
                }
            }
        }



        public FilterViewModel()
        {
            string dbPath = @"../../recipes.db"; // ścieżka do bazy danych
            _dataHelper = new Datahelper(dbPath);
            _allRecipes = new ObservableCollection<Recipe>(_dataHelper.LoadRecipes());
            FilteredSortedRecipes = _allRecipes;
            ResetAllCommand = new RelayCommand(ResetAll);
        }

        private void FilterRecipes()
        {
            FilteredSortedRecipes = _allRecipes;
            FilterByDifficulty();
            FilterByTime();
            FilterByRating();
            FilterByServings();
            FilterByStepsCount();
            FilterByRatingCount();

            SortByDifficulty();
            SortByTime();
            SortByRating();
            SortByServings();
            SortBySteps();
            SortByRatingCount();
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
            if ((TimeLower == 0 && TimeUpper > 0) || TimeLower < TimeUpper || (TimeLower != 0 && TimeUpper == TimeLower))
            {
                FilteredSortedRecipes = new ObservableCollection<Recipe>(FilteredSortedRecipes.Where(recipe => recipe.PrepTime >= TimeLower && recipe.PrepTime <= TimeUpper));
            }
            
        }

        private void FilterByRating()
        {
            if ((_ratingLower == 0 && _ratingUpper > 0) || _ratingLower < _ratingUpper || (_ratingLower != 0 && _ratingUpper == _ratingLower))
            {
                FilteredSortedRecipes = new ObservableCollection<Recipe>(FilteredSortedRecipes.Where(recipe => recipe.Rating >= _ratingLower && recipe.Rating <= _ratingUpper));
            }

        }

        private void FilterByStepsCount()
        {
            if ((StepsCountLower == 0 && StepsCountUpper > 0) || StepsCountLower < StepsCountUpper || (StepsCountLower != 0 && StepsCountUpper == StepsCountLower))
            {
                FilteredSortedRecipes = new ObservableCollection<Recipe>(FilteredSortedRecipes.Where(recipe => recipe.Steps >= StepsCountLower && recipe.Steps <= StepsCountUpper));
            }

        }

        private void FilterByRatingCount()
        {
            if ((RatingCountLower == 0 && RatingCountUpper > 0) || RatingCountLower < RatingCountUpper || (RatingCountLower != 0 && RatingCountUpper == RatingCountLower))
            {
                FilteredSortedRecipes = new ObservableCollection<Recipe>(FilteredSortedRecipes.Where(recipe => recipe.RatingCount >= RatingCountLower && recipe.RatingCount <= RatingCountUpper));
            }

        }

        private void FilterByServings()
        {
            if ((ServingsLower == 0 && ServingsUpper > 0) || ServingsLower < ServingsUpper || (ServingsLower != 0 && ServingsUpper == ServingsLower))
            {
                foreach (var recipe in FilteredSortedRecipes)
                {
                    if (recipe.Servings.Contains("-"))
                    {
                        string[] parts = recipe.Servings.Split('-');

                        if (parts.Length == 2 &&
                            double.TryParse(parts[0].Trim(), out double first) &&
                            double.TryParse(parts[1].Trim(), out double second))
                        {
                            double average = (first + second) / 2;
                            recipe.Servings = average.ToString("0.##"); // Formatowanie usuwające niepotrzebne zera
                        }
                    }
                }

                FilteredSortedRecipes = new ObservableCollection<Recipe>(
                    FilteredSortedRecipes.Where(recipe =>
                    float.TryParse(recipe.Servings, NumberStyles.Float, CultureInfo.InvariantCulture, out float servings) && 
                    servings >= ServingsLower && 
                    servings <= ServingsUpper).ToList());

            }
        }

        private void SortByDifficulty()
        {
            if (_difficultySort == null)
            {
                return;
            }

            var difficultyAscending = new List<string> { "Łatwy", "Średni", "Trudny" };
            var difficultyDescending = new List<string> { "Trudny", "Średni", "Łatwy" };
            if (DifficultySort == "Rosnąco")
            {
                FilteredSortedRecipes = new ObservableCollection<Recipe>(FilteredSortedRecipes.OrderBy(recipe => difficultyAscending.IndexOf(recipe.Difficulty)));
            }
            else if (DifficultySort == "Malejąco")
            {
                FilteredSortedRecipes = new ObservableCollection<Recipe>(FilteredSortedRecipes.OrderBy(recipe => difficultyDescending.IndexOf(recipe.Difficulty)));
            }
        }

        private void SortByTime()
        {
            if (_timeSort == null)
            {
                return;
            }

            if (TimeSort == "Rosnąco")
            {
                FilteredSortedRecipes = new ObservableCollection<Recipe>(FilteredSortedRecipes.OrderBy(recipe => recipe.PrepTime));
            }
            else if (TimeSort == "Malejąco")
            {
                FilteredSortedRecipes = new ObservableCollection<Recipe>(FilteredSortedRecipes.OrderByDescending(recipe => recipe.PrepTime));
            }
        }

        private void SortByRating()
        {
            if (_ratingSort == null)
            {
                return;
            }

            if (RatingSort == "Rosnąco")
            {
                FilteredSortedRecipes = new ObservableCollection<Recipe>(FilteredSortedRecipes.OrderBy(recipe => recipe.Rating));
            }
            else if (RatingSort == "Malejąco")
            {
                FilteredSortedRecipes = new ObservableCollection<Recipe>(FilteredSortedRecipes.OrderByDescending(recipe => recipe.Rating));
            }
        }

        private void SortByServings()
        {
            if (_servingsSort == null)
            {
                return;
            }

            foreach (var recipe in FilteredSortedRecipes)
            {
                if (recipe.Servings.Contains("-"))
                {
                    string[] parts = recipe.Servings.Split('-');

                    if (parts.Length == 2 &&
                        double.TryParse(parts[0].Trim(), out double first) &&
                        double.TryParse(parts[1].Trim(), out double second))
                    {
                        double average = (first + second) / 2;
                        recipe.Servings = average.ToString("0.##"); // Formatowanie usuwające niepotrzebne zera
                    }
                }
            }

            if (ServingsSort == "Rosnąco")
            {
                FilteredSortedRecipes = new ObservableCollection<Recipe>(
                    FilteredSortedRecipes.OrderBy(recipe =>
                    double.TryParse(recipe.Servings, NumberStyles.Any, CultureInfo.InvariantCulture, out double servings)
                    ? servings
                    : double.MaxValue));
            }
            else if (ServingsSort == "Malejąco")
            {

                FilteredSortedRecipes = new ObservableCollection<Recipe>(
                    FilteredSortedRecipes.OrderByDescending(recipe =>
                    double.TryParse(recipe.Servings, NumberStyles.Any, CultureInfo.InvariantCulture, out double servings)
                    ? servings
                    : double.MinValue));
            }
        }

        private void SortBySteps()
        {
            if (_stepsCountSort == null)
            {
                return;
            }

            if (StepsCountSort == "Rosnąco")
            {
                FilteredSortedRecipes = new ObservableCollection<Recipe>(FilteredSortedRecipes.OrderBy(recipe => recipe.Steps));
            }
            else if (StepsCountSort == "Malejąco")
            {
                FilteredSortedRecipes = new ObservableCollection<Recipe>(FilteredSortedRecipes.OrderByDescending(recipe => recipe.Steps));
            }
        }

        private void SortByRatingCount()
        {
            if (_ratingCountSort == null)
            {
                return;
            }

            if (RatingCountSort == "Rosnąco")
            {
                FilteredSortedRecipes = new ObservableCollection<Recipe>(FilteredSortedRecipes.OrderBy(recipe => recipe.RatingCount));
            }
            else if (RatingCountSort == "Malejąco")
            {
                FilteredSortedRecipes = new ObservableCollection<Recipe>(FilteredSortedRecipes.OrderByDescending(recipe => recipe.RatingCount));
            }
        }

        private void ResetSortProperties(string currentProperty)
        {
            if (currentProperty != nameof(DifficultySort))
            {
                _difficultySort = null;
                OnPropertyChanged(nameof(DifficultySort));
            }
            if (currentProperty != nameof(TimeSort))
            {
                _timeSort = null;
                OnPropertyChanged(nameof(TimeSort));
            }
            if (currentProperty != nameof(RatingSort))
            {
                _ratingSort = null;
                OnPropertyChanged(nameof(RatingSort));
            }
            if (currentProperty != nameof(ServingsSort))
            {
                _servingsSort = null;
                OnPropertyChanged(nameof(ServingsSort));
            }
            if (currentProperty != nameof(StepsCountSort))
            {
                _stepsCountSort = null;
                OnPropertyChanged(nameof(StepsCountSort));
            }
            if (currentProperty != nameof(RatingCountSort))
            {
                _ratingCountSort = null;
                OnPropertyChanged(nameof(RatingCountSort));
            }
        }

        private void ResetAll(object parameter)
        {
            DifficultyLower = null;
            DifficultyUpper = null;

            TimeLower = 0;
            TimeUpper = 0;

            RatingLowerString = null;
            RatingUpperString = null;
            _ratingLower = 0;
            _ratingUpper = 0;

            _servingsLower = 0; 
            _servingsUpper = 0;

            StepsCountLower = 0;
            StepsCountUpper = 0;

            RatingCountLower = 0;
            RatingCountUpper = 0;

            ResetSortProperties(_ratingLowerString);

            FilteredSortedRecipes = _allRecipes;
        }
    }
}
