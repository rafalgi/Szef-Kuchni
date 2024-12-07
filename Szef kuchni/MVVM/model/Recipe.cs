using System;

internal class Recipe
{
    public int Id { get; set; }
    public string Title { get; set; }        

    public string Servings { get; set; }     

    public string Difficulty { get; set; }   

    public int PrepTime { get; set; }        

    public string Description { get; set; }  

    public int Steps { get; set; }           

    public float Rating { get; set; }        

    public int RatingCount { get; set; }     


    public string SavePath { get; set; }    


    public string FullImagePath
    {
        get
        {
            return $"pack://application:,,,/{SavePath}";
        }
    }

}
