using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Szef_kuchni.Core;
using Szef_kuchni.MVVM.Model;

namespace Szef_kuchni.MVVM.ViewModel
{
    internal class RecipeDetailsWindowViewModel : ObservableObject
    {
        private readonly Datahelper _dataHelper;
        private ObservableCollection<Step> _steps;
        private ObservableCollection<Ingredient> _ingredients;
        public ICommand GoBackCommand { get; }

        public ObservableCollection<Step> Steps
        {
            get => _steps;
            set
            {
                _steps = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Ingredient> Ingredients
        {
            get => _ingredients;
            set
            {
                _ingredients = value;
                OnPropertyChanged();
            }
        }

        private string _recipeImagePath;
        public string RecipeImagePath
        {
            get => _recipeImagePath;
            set
            {
                _recipeImagePath = value;
                OnPropertyChanged();
            }
        }


        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private int _prepTime;
        public int PrepTime
        {
            get => _prepTime;
            set
            {
                _prepTime = value;
                OnPropertyChanged();
            }
        }

        private string _difficulty;
        public string Difficulty
        {
            get => _difficulty;
            set
            {
                _difficulty = value;
                OnPropertyChanged();
            }
        }

        // konstruktor 
        public RecipeDetailsWindowViewModel(int recipeId)
        {
            _dataHelper = new Datahelper(@"../../recipes.db"); 
            LoadRecipeDetails(recipeId);
            GoBackCommand = new RelayCommand(ExecuteGoBack);
        }

        private void ExecuteGoBack(object obj) // Metoda, która umożliwia powrót na stronę główną
        {
            Application.Current.MainWindow.DataContext = new MainViewModel();
        }

        private void LoadRecipeDetails(int recipeId)
        {
            var recipe = _dataHelper.LoadRecipeById(recipeId);

            if (recipe != null)
            {
                RecipeImagePath = recipe.FullImagePath; 
                Steps = new ObservableCollection<Step>(_dataHelper.LoadSteps(recipeId));
                Ingredients = new ObservableCollection<Ingredient>(_dataHelper.LoadIngredients(recipeId)); 
                Title = recipe.Title;
                PrepTime= recipe.PrepTime;
                Difficulty = recipe.Difficulty;
            }
            else
            {
                MessageBox.Show("Nie znaleziono przepisu."); //sprawdzenie (na wszelki, chociaz w przepisie na razie nie ma nulla)
            }
        }



    }
}
