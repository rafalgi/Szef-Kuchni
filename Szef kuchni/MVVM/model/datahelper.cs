using System;
using System.Data.SQLite;
using System.Collections.ObjectModel;
using System.Linq;
using Szef_kuchni.MVVM.Model;

internal class Datahelper
{
    private readonly string _connectionString;

    public Datahelper(string dbPath)
    {
        _connectionString = $"Data Source={dbPath}";
    }

    //public ObservableCollection<Recipe> LoadRecipesTopRated()
    //{
    //    var recipes = new ObservableCollection<Recipe>();

    //    using (var connection = new SQLiteConnection(_connectionString))
    //    {
    //        connection.Open();

    //        string query = "SELECT Id, Title, Servings, Difficulty, prep_time, Description, Steps_num, Rating, rating_count, save_path FROM recipes";
    //        using (var command = new SQLiteCommand(query, connection))
    //        {
    //            using (var reader = command.ExecuteReader())
    //            {
    //                while (reader.Read())
    //                {
    //                    var recipe = new Recipe
    //                    {
    //                        Id = reader.GetInt32(0),
    //                        Title = reader.GetString(1),
    //                        Servings = reader.GetString(2),
    //                        Difficulty = reader.GetString(3),
    //                        PrepTime = reader.GetInt32(4),
    //                        Description = reader.GetString(5),
    //                        Steps = reader.GetInt32(6),
    //                        Rating = reader.IsDBNull(7) ? 0 : reader.GetFloat(7),
    //                        RatingCount = reader.IsDBNull(8) ? 0 : reader.GetInt32(8),
    //                        SavePath = reader.GetString(9)
    //                    };
    //                    recipes.Add(recipe);
    //                }
    //            }
    //        }
    //    }

    //    var topRatedRecipes = new ObservableCollection<Recipe>(
    //       recipes.OrderByDescending(x => x.RatingCount).Take(700)
    //   );

    //    return recipes;
    //}

    public ObservableCollection<Recipe> LoadRecipes()
    {
        var recipes = new ObservableCollection<Recipe>();

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            string query = "SELECT Id, Title, Servings, Difficulty, prep_time, Description, Steps_num, Rating, rating_count, save_path FROM recipes";
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
                            Servings = reader.GetString(2),
                            Difficulty = reader.GetString(3),
                            PrepTime = reader.GetInt32(4),
                            Description = reader.GetString(5),
                            Steps = reader.GetInt32(6),
                            Rating = reader.IsDBNull(7) ? 0 : reader.GetFloat(7),
                            RatingCount = reader.IsDBNull(8) ? 0 : reader.GetInt32(8),
                            SavePath = reader.GetString(9)
                        };
                        recipes.Add(recipe);
                    }
                }
            }
        }
        return recipes;
    }

    public ObservableCollection<Recipe> LoadHistoryRecipes()
    {
        var recipes = new ObservableCollection<Recipe>();

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            string query = @"
                SELECT 
                    r.Id, 
                    r.Title, 
                    r.Servings, 
                    r.Difficulty, 
                    r.prep_time, 
                    r.Description, 
                    r.Steps_num, 
                    r.Rating, 
                    r.rating_count, 
                    r.save_path
                FROM 
                    recipes r
                INNER JOIN 
                    history h
                ON 
                    r.Id = h.recipe_id
                ORDER BY 
                    h.view_date DESC";
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
                            Servings = reader.GetString(2),
                            Difficulty = reader.GetString(3),
                            PrepTime = reader.GetInt32(4),
                            Description = reader.GetString(5),
                            Steps = reader.GetInt32(6),
                            Rating = reader.IsDBNull(7) ? 0 : reader.GetFloat(7),
                            RatingCount = reader.IsDBNull(8) ? 0 : reader.GetInt32(8),
                            SavePath = reader.GetString(9)
                        };
                        recipes.Add(recipe);
                    }
                }
            }
        }
        return recipes;
    }

    public ObservableCollection<Category> LoadCategories()
    {
        var categories = new ObservableCollection<Category>();

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            string query = "SELECT Id, Name FROM categories";
            using (var command = new SQLiteCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var category = new Category
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        };
                        categories.Add(category);
                    }
                }
            }
        }

        return categories;
    }

    public ObservableCollection<Tag> LoadTags()
    {
        var tags = new ObservableCollection<Tag>();

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            string query = "SELECT Id, Name FROM tags";
            using (var command = new SQLiteCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tag = new Tag
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        };
                        tags.Add(tag);
                    }
                }
            }
        }

        return tags;
    }

    public ObservableCollection<Ingredient> LoadIngredients(int recipeId)
    {
        var ingredients = new ObservableCollection<Ingredient>();

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            string query = "SELECT Id, Ingredient FROM ingredients WHERE recipe_id = @RecipeId";
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RecipeId", recipeId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ingredient = new Ingredient
                        {
                            Id = reader.GetInt32(0),
                            Ingredients = reader.GetString(1),
                        };
                        ingredients.Add(ingredient);
                    }
                }
            }
        }

        return ingredients;
    }
    // loadsteps ale z konstruktorem parametrycznym, bo muszę pobrać z konkretnego przepisu
    public ObservableCollection<Step> LoadSteps(int recipeId)
    {
        var steps = new ObservableCollection<Step>();

        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT id, recipe_id, step_number, step_text FROM steps WHERE recipe_id = @RecipeId ORDER BY step_number";
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RecipeId", recipeId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var step = new Step
                        {
                            Id = reader.GetInt32(0),
                            RecipeId = reader.GetInt32(1),
                            StepNumber = reader.GetInt32(2),
                            Description = reader.GetString(3)
                        };
                        steps.Add(step);
                    }
                }
            }
        }


        return steps;
    }

    // również konstruktor parametryczny (chce pobrać zdjecie) 
    public Recipe LoadRecipeById(int recipeId)
    {
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT id, title, servings, difficulty, prep_time, description, steps_num, rating, rating_count, save_path FROM recipes WHERE id = @RecipeId";
            using (var command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RecipeId", recipeId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Recipe
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Servings = reader.GetString(2),
                            Difficulty = reader.GetString(3),
                            PrepTime = reader.GetInt32(4),
                            Description = reader.GetString(5),
                            Steps = reader.GetInt32(6),
                            Rating = reader.GetFloat(7),
                            RatingCount = reader.GetInt32(8),
                            SavePath = reader.GetString(9)
                        };
                    }
                }
            }
        }
        return null;
    }

    public void SaveHistory(int recipeId)
    {
        if (CheckLastHistoryRecipeId() == recipeId)
        {
            return;
        }
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            // Usuwanie wcześniejszych wpisów z tym samym recipeId
            string deleteQuery = "DELETE FROM history WHERE recipe_id = @recipe_id";
            using (var deleteCommand = new SQLiteCommand(deleteQuery, connection))
            {
                deleteCommand.Parameters.AddWithValue("@recipe_id", recipeId);
                deleteCommand.ExecuteNonQuery();
            }

            // Wstawianie nowego wpisu
            string query = @"
                INSERT INTO history (recipe_id, view_date)
                VALUES (@recipe_id, @view_date)";

            using (var command = new SQLiteCommand(query, connection))
            {
                // Ustawianie parametrów
                command.Parameters.AddWithValue("@recipe_id", recipeId);

                // Aktualny czas w formacie Unix
                int unixTimestamp = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                command.Parameters.AddWithValue("@view_date", unixTimestamp);

                // Wykonanie zapytania
                command.ExecuteNonQuery();
            }
        }
    }

    public void EnsureHistoryLimit()
    {
        int history_limit =100;
        using (var connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            // Sprawdzenie liczby rekordów w tabeli history
            string countQuery = "SELECT COUNT(*) FROM history";
            using (var countCommand = new SQLiteCommand(countQuery, connection))
            {
                long recordCount = (long)countCommand.ExecuteScalar();

                // Jeśli rekordów jest więcej niż 150, usuń najstarsze
                if (recordCount > history_limit)
                {
                    string deleteQuery = @"
                    DELETE FROM history
                    WHERE id IN (
                        SELECT id FROM history
                        ORDER BY view_date ASC
                        LIMIT @excessCount
                    )";

                    using (var deleteCommand = new SQLiteCommand(deleteQuery, connection))
                    {
                        // Obliczanie liczby rekordów do usunięcia
                        int excessCount = (int)(recordCount - history_limit);
                        deleteCommand.Parameters.AddWithValue("@excessCount", excessCount);

                        deleteCommand.ExecuteNonQuery();
                    }
                }
            }
        }
    }

    public int? CheckLastHistoryRecipeId()
    {
        // Zmieniamy typ zwracany na int? aby obsłużyć przypadek, gdy nie ma rekordów w tabeli history
        using (var connection = new SQLiteConnection(_connectionString))
        {
            try
            {
                connection.Open();

                // Zapytanie SQL do pobrania ostatniego recipe_id
                string checkQuery = "SELECT recipe_id FROM history ORDER BY view_date DESC LIMIT 1";

                using (var checkCommand = new SQLiteCommand(checkQuery, connection))
                {
                    using (var reader = checkCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Zwracamy wartość lub null, jeśli pole jest puste
                            return reader.IsDBNull(0) ? (int?)null : reader.GetInt32(0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas sprawdzania ostatniego ID: {ex.Message}");
            }
        }
        return null;
    }
}


