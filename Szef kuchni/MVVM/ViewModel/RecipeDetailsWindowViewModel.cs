using System.Collections.ObjectModel;
using System.Windows;
using Szef_kuchni.Core;
using Szef_kuchni.MVVM.Model;

namespace Szef_kuchni.MVVM.ViewModel
{
    internal class RecipeDetailsWindowViewModel : ObservableObject
    {
        private readonly Datahelper _dataHelper;
        private ObservableCollection<Step> _steps;


        public ObservableCollection<Step> Steps
        {
            get => _steps;
            set
            {
                _steps = value;
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

        // konstruktor 
        public RecipeDetailsWindowViewModel(int recipeId)
        {

            _dataHelper = new Datahelper(@"../../recipes.db"); 
            LoadRecipeDetails(recipeId);
        }

        private void LoadRecipeDetails(int recipeId)
        {
            var recipe = _dataHelper.LoadRecipeById(recipeId);

            if (recipe != null)
            {
                RecipeImagePath = recipe.FullImagePath; 
                Steps = new ObservableCollection<Step>(_dataHelper.LoadSteps(recipeId)); 
            }
            else
            {
                MessageBox.Show("Nie znaleziono przepisu."); //sprawdzenie (na wszelki, chociaz w przepisie na razie nie ma nulla)
            }
        }



    }
}
