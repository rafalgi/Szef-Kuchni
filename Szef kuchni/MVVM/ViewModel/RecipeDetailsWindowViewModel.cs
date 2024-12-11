using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using Szef_kuchni.Core;
using Szef_kuchni.MVVM.Model;
using Szef_kuchni.MVVM.View;
using Szef_kuchni.MVVM.ViewModel;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Windows.Documents;
using System.Xml.Linq;
using System;
using iText.Kernel.Font;
using System.Windows.Controls;
using System.Diagnostics;

internal class RecipeDetailsWindowViewModel : ObservableObject
{
    private readonly Datahelper _dataHelper;
    private ObservableCollection<Step> _steps;
    private ObservableCollection<Ingredient> _ingredients;
    public ICommand ExportToPDFCommand { get; }
    public ICommand GoBackCommand { get; }
    public ICommand ShowIngredientsCommand { get; }

    private IngredientsWindow _ingredientsWindow;

    public ObservableCollection<Step> Steps
    {
        get => _steps;
        set
        {
            _steps = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Ingredient> Ingredients
    {
        get => _ingredients;
        set
        {
            _ingredients = value;
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

    private string _title;
    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged();
        }
    }

    private int _prepTime;
    public int PrepTime
    {
        get => _prepTime;
        set
        {
            _prepTime = value;
            OnPropertyChanged();
        }
    }

    private string _difficulty;
    public string Difficulty
    {
        get => _difficulty;
        set
        {
            _difficulty = value;
            OnPropertyChanged();
        }
    }

    public RecipeDetailsWindowViewModel(int recipeId)
    {
        _dataHelper = new Datahelper(@"../../recipes.db");
        LoadRecipeDetails(recipeId);
        GoBackCommand = new RelayCommand(ExecuteGoBack);
        ShowIngredientsCommand = new RelayCommand(ExecuteShowIngredients);
        ExportToPDFCommand = new RelayCommand(ExecuteExportToPDF);
    }

    private void ExecuteGoBack(object obj)
    {
        Application.Current.MainWindow.DataContext = new MainViewModel();
    }

    private void ExecuteShowIngredients(object obj)
    {
        if (_ingredientsWindow == null || !_ingredientsWindow.IsVisible)
        {
            _ingredientsWindow = new IngredientsWindow
            {
                DataContext = new IngredientsWindowViewModel(Ingredients, Title)
            };
            _ingredientsWindow.Show();
        }
        else
        {
            _ingredientsWindow.Activate();
            if (_ingredientsWindow.WindowState == WindowState.Minimized)
            {
                _ingredientsWindow.WindowState = WindowState.Normal;
            }
        }
    }

    private void LoadRecipeDetails(int recipeId)
    {
        var recipe = _dataHelper.LoadRecipeById(recipeId);

        if (recipe != null)
        {
            RecipeImagePath = recipe.FullImagePath;
            Steps = new ObservableCollection<Step>(_dataHelper.LoadSteps(recipeId));
            Ingredients = new ObservableCollection<Ingredient>(_dataHelper.LoadIngredients(recipeId));
            Title = recipe.Title;
            PrepTime = recipe.PrepTime;
            Difficulty = recipe.Difficulty;
        }
        else
        {
            MessageBox.Show("Nie znaleziono przepisu.");
        }
    }

    private void ExecuteExportToPDF(object obj)
    {
        try
        {
            string outputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"{Title}.pdf");

            // Tworzenie dokumentu PDF
            using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (Document doc = new Document(PageSize.A4))
            using (PdfWriter writer = PdfWriter.GetInstance(doc, fs))
            {
                var backgroundColor = new BaseColor(240, 240, 220); // Jasny kolor przypominający papier
                writer.PageEvent = new PdfBackgroundHelper(backgroundColor);

                doc.Open();

                // Ładowanie czcionki TrueType, która obsługuje polskie znaki (np. Arial)
                string fontPath = @"C:\Windows\Fonts\arial.ttf"; // Ścieżka do czcionki Arial

                BaseFont baseFont1 = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                var font1 = new Font(baseFont1, 15, Font.BOLD);

                BaseFont baseFont2 = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                var font2 = new Font(baseFont2, 12);

                BaseFont baseFont3 = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                var font3 = new Font(baseFont2, 15);

                BaseFont baseFont4 = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                var font4 = new Font(baseFont2, 12, Font.BOLD);

                BaseFont baseFont5 = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                var font5 = new Font(baseFont1, 18, Font.BOLD);

                // Dodaj tytuł
                doc.Add(new iTextSharp.text.Paragraph(Title, font5) { Alignment = iTextSharp.text.Element.ALIGN_CENTER });
                doc.Add(Chunk.NEWLINE);

                Chunk czasPrzygotowania = new Chunk($"Czas przygotowania: ", font1);
                Chunk prepTime = new Chunk($"{PrepTime} min", font3);

                iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph();
                paragraph.Add(czasPrzygotowania);
                paragraph.Add(prepTime);
                doc.Add(paragraph);

                Chunk trudnosc = new Chunk($"Trudność: ", font1);
                Chunk difficulty = new Chunk($"{Difficulty}", font3);

                iTextSharp.text.Paragraph paragraph2 = new iTextSharp.text.Paragraph();
                paragraph2.Add(trudnosc);
                paragraph2.Add(difficulty);

                doc.Add(paragraph2);
                doc.Add(Chunk.NEWLINE);

                // Dodaj obrazek (jeśli istnieje)
                if (RecipeImagePath.StartsWith("pack://"))
                {
                    try
                    {
                        Uri imageUri = new Uri(RecipeImagePath, UriKind.Absolute);
                        var streamResourceInfo = Application.GetResourceStream(imageUri);

                        if (streamResourceInfo != null)
                        {
                            // Wczytaj obraz ze strumienia
                            using (var resourceStream = streamResourceInfo.Stream)
                            {
                                // Zapisywanie pliku tymczasowego
                                string tempFilePath = Path.GetTempFileName();
                                using (var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
                                {
                                    resourceStream.CopyTo(fileStream);
                                }

                                // Załaduj obraz z tymczasowej ścieżki
                                iTextSharp.text.Image recipeImage = iTextSharp.text.Image.GetInstance(tempFilePath);

                                // Skalowanie obrazu
                                float maxWidth = 250f;
                                float maxHeight = 250f;

                                float ratio = Math.Min(maxWidth / recipeImage.Width, maxHeight / recipeImage.Height);
                                recipeImage.ScalePercent(ratio * 100);

                                recipeImage.Alignment = iTextSharp.text.Image.ALIGN_LEFT;
                                doc.Add(recipeImage);
                                doc.Add(Chunk.NEWLINE);

                                // Usuń plik tymczasowy (opcjonalnie)
                                File.Delete(tempFilePath);
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Nie można załadować obrazu z URI: {RecipeImagePath}", "Błąd obrazu", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Wystąpił błąd podczas wczytywania obrazu: {ex.Message}\nŚcieżka: {RecipeImagePath}", "Błąd obrazu", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show($"Ścieżka do obrazu nie zaczyna się od 'pack://': {RecipeImagePath}", "Błąd ścieżki", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                // Dodaj składniki
                doc.Add(new iTextSharp.text.Paragraph("\nSkładniki:", font1));
                foreach (var ingredient in Ingredients)
                {
                    doc.Add(new iTextSharp.text.Paragraph($"- {ingredient.Ingredients}", font2));
                }

                doc.Add(new iTextSharp.text.Paragraph("\nKroki przygotowania:", font1));
                foreach (var step in Steps)
                {
                    Chunk stepNumberChunk = new Chunk($"Krok {step.StepNumber}: ", font4);
                    Chunk stepDescriptionChunk = new Chunk($"{step.Description}", font2);

                    iTextSharp.text.Paragraph stepParagraph = new iTextSharp.text.Paragraph();

                    stepParagraph.Add(stepNumberChunk);
                    stepParagraph.Add(stepDescriptionChunk);

                    doc.Add(stepParagraph);
                    doc.Add(Chunk.NEWLINE);
                }

                doc.Close();
            }

            MessageBox.Show($"Przepis został zapisany jako PDF na pulpicie: {outputPath}", "Eksport zakończony", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Wystąpił błąd podczas eksportu: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public class PdfBackgroundHelper : PdfPageEventHelper
    {
        private readonly BaseColor _backgroundColor;

        public PdfBackgroundHelper(BaseColor backgroundColor)
        {
            _backgroundColor = backgroundColor;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            // Dodawanie tła do każdej strony
            PdfContentByte canvas = writer.DirectContentUnder;
            Rectangle background = new Rectangle(document.PageSize);
            background.BackgroundColor = _backgroundColor;
            background.Border = Rectangle.NO_BORDER;
            canvas.Rectangle(background);
        }
    }

}
