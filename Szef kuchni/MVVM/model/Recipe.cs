using System;

public class Recipe
{
    // Klasa reprezentuj¹ca przepis
    public int Id { get; set; }
    public string Title { get; set; }        // Tytu³ przepisu

    public string Servings { get; set; }     // Liczba porcji

    public string Difficulty { get; set; }   // Poziom trudnoœci

    public int PrepTime { get; set; }        // Czas przygotowania w minutach

    public string Description { get; set; }  // Opis przepisu

    public int Steps { get; set; }           // Liczba kroków

    public float Rating { get; set; }        // Ocena przepisu (nullable)

    public int RatingCount { get; set; }     // Liczba ocen (nullable)


    public string SavePath { get; set; }     // Œcie¿ka do obrazu


    public string FullImagePath
    {
        get
        {
            return $"pack://application:,,,/{SavePath}";
        }
    }

}
