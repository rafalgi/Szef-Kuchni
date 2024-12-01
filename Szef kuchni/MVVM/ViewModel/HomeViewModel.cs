using System;
using System.Collections.ObjectModel;
using System.Linq;
using Szef_kuchni.Core;

namespace Szef_kuchni.MVVM.ViewModel
{
    internal class HomeViewModel : ObservableObject
    {
        // Kolekcja przepisów
        public ObservableCollection<Recipe> Recipes { get; set; }
        private ObservableCollection<Recipe> _allRecipes;

        // Datahelper do obsługi bazy danych
        private readonly Datahelper _dataHelper;

        // Konstruktor
        public HomeViewModel()
        {
            // Inicjalizacja kolekcji
            Recipes = new ObservableCollection<Recipe>();

            // Utwórz instancję Datahelper
            string dbPath = @"../../recipes.db"; // Ścieżka do bazy danych
            _dataHelper = new Datahelper(dbPath);

            // Załaduj przepisy z bazy danych
            LoadRecipes();
        }

        // Metoda do załadowania przepisów z bazy danych
        private void LoadRecipes()
        {
            _allRecipes = _dataHelper.LoadRecipes();
            Recipes = new ObservableCollection<Recipe>(_allRecipes);
        }

        // Metoda do wyszukiwania przepisów
        public void SearchRecipes(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                Recipes = new ObservableCollection<Recipe>(_allRecipes);
            }
            else
            {
                Recipes = new ObservableCollection<Recipe>(_allRecipes.Where(r => r.Title.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0));
            }
            OnPropertyChanged(nameof(Recipes));
        }
    }
}
