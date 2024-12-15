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

}


