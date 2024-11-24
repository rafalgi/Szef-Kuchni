using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Recipe
{
    public string RecipeName { get; set; }
    public List<string> Ingredients { get; set; }
    public string Occasion { get; set; }
    public int PreparationTime { get; set; }  // W minutach
    public string Difficulty { get; set; }
    public int Calories { get; set; }
    public List<string> Steps { get; set; }  // Krok po kroku
}