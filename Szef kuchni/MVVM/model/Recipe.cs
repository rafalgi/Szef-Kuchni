using System;

public class Recipe
{
    // Klasa reprezentuj�ca przepis
    public int Id { get; set; }
    public string Title { get; set; }        // Tytu� przepisu

    public string Servings { get; set; }     // Liczba porcji

    public string Difficulty { get; set; }   // Poziom trudno�ci

    public int PrepTime { get; set; }        // Czas przygotowania w minutach

    public string Description { get; set; }  // Opis przepisu

    public int Steps { get; set; }           // Liczba krok�w

    public float Rating { get; set; }        // Ocena przepisu (nullable)

    public int RatingCount { get; set; }     // Liczba ocen (nullable)


    public string SavePath { get; set; }     // �cie�ka do obrazu


    public string FullImagePath
    {
        get
        {
            return $"pack://application:,,,/{SavePath}";
        }
    }

}
