using System;
using System.Data.SQLite;
using System.Collections.ObjectModel;
using System.Linq;

internal class Datahelper
{
    private readonly string _connectionString;

    public Datahelper(string dbPath)
    {
        _connectionString = $"Data Source={dbPath}";
    }

    public ObservableCollection<Recipe> LoadRecipes()
    {
        var recipes = new ObservableCollection<Recipe>();

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            string query = "SELECT Id, Title, save_path  FROM recipes";
            using (var command = new SQLiteCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var recipe = new Recipe
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            SavePath = reader.GetString(2)
                        };
                        recipes.Add(recipe);
                    }
                }
            }
        }

        // Losowo wybierz 9 przepisów
        var random = new Random();
        var randomRecipes = new ObservableCollection<Recipe>(recipes.OrderBy(x => random.Next()).Take(9));

        return randomRecipes;
    }
}