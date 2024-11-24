using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Szef_kuchni
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Recipe> localRecipes;  // Kolekcja przepisów
        public MainWindow()
        {
            InitializeComponent();
            LoadLocalRecipes();
        }

        private void LoadLocalRecipes()
        {
            // Ładowanie przepisów z lokalnej bazy danych lub pliku JSON
        }

        private void OnSearchButtonClick(object sender, RoutedEventArgs e)
        {
            string searchIngredients = ProductSearchBox.Text;
            string occasion = (OccasionComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            int time = (int)TimeSlider.Value;
            string difficulty = (DifficultyComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            int calories = int.Parse(CaloriesBox.Text);

            // Filtruj przepisy według kryteriów
            var filteredRecipes = localRecipes.Where(recipe =>
                recipe.Ingredients.Any(ingredient => searchIngredients.Contains(ingredient)) &&
                recipe.Occasion == occasion &&
                recipe.PreparationTime <= time &&
                recipe.Difficulty == difficulty &&
                recipe.Calories <= calories).ToList();

            RecipeListView.ItemsSource = filteredRecipes;
        }
    }
}
