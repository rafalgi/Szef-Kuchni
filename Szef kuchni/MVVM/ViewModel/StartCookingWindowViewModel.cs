using System.Collections.ObjectModel;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Windows.Input;
using System.Windows;
using Szef_kuchni.Core;
using Szef_kuchni.MVVM.Model;
using Szef_kuchni.MVVM.View;
using Szef_kuchni.MVVM.ViewModel;
using System.Linq;
using System.Threading.Tasks;
using System;

internal class StartCookingWindowViewModel : ObservableObject
{
    private readonly Datahelper _dataHelper;
    private ObservableCollection<Step> _steps;
    private Step _currentStep;
    public bool IsReadingEnabled { get; private set; }
    private ObservableCollection<Ingredient> _ingredients;
    private string _title;
    int _stepCounter;
    private IngredientsWindow _ingredientsWindow;

    public ICommand ShowIngredientsCommand { get; }
    public ICommand CloseIngredientsCommand { get; } 
    public ICommand NextStepCommand { get; }
    public ICommand PreviousStepCommand { get; }
    public ICommand StopReadingCommand { get; }

    private SpeechRecognitionEngine _recognizer;
    private SpeechSynthesizer _synthesizer;


    private bool _isNextStepVisible;
    public bool IsNextStepVisible
    {
        get => _isNextStepVisible;
        set
        {
            _isNextStepVisible = value;
            OnPropertyChanged();
        }
    }

    private bool _isPreviousStepVisible;
    public bool IsPreviousStepVisible
    {
        get => _isPreviousStepVisible;
        set
        {
            _isPreviousStepVisible = value;
            OnPropertyChanged();
        }
    }
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

    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged();
        }
    }

    public Step CurrentStep
    {
        get => _currentStep;
        set
        {
            _currentStep = value;
            OnPropertyChanged();
            UpdateStepVisibility();
        }
    }

    public StartCookingWindowViewModel(int recipeId, bool isReadingEnabled)
    {
        _dataHelper = new Datahelper(@"../../recipes.db");
        LoadRecipeDetails(recipeId);
        CurrentStep = Steps.FirstOrDefault();
        IsReadingEnabled = isReadingEnabled;

        _synthesizer = new SpeechSynthesizer();
        StopReadingCommand = new RelayCommand(StopReading);

        if (CurrentStep != null)
        {
            IsNextStepVisible = true;
            if (IsReadingEnabled)
            {
                ReadStepAloud(CurrentStep);
            }
        }

        ShowIngredientsCommand = new RelayCommand(ExecuteShowIngredients);
        CloseIngredientsCommand = new RelayCommand(ExecuteCloseIngredients);
        NextStepCommand = new RelayCommand(NextStep);
        PreviousStepCommand = new RelayCommand(PreviousStep);
        _stepCounter = _steps?.Count ?? 0;

        InitializeSpeechRecognition();
    }

    private void InitializeSpeechRecognition()
    {
        if (SpeechRecognitionEngine.InstalledRecognizers().Count == 0)
        {
            ShowMessage("Brak zainstalowanych modułów rozpoznawania mowy.", "Błąd!");
            return;
        }

        var recognizerInfo = SpeechRecognitionEngine
            .InstalledRecognizers()
            .FirstOrDefault(r => r.Culture.Name == "en-GB");

        if (recognizerInfo == null)
        {
            ShowMessage("Rozpoznawanie mowy w języku angielskim nie jest dostępne. Sprawdź, czy masz dodany język angielski z funkcją rozpoznawania mowy.", "Ostrzeżenie!");
            return;
        }

        try
        {
            _recognizer = new SpeechRecognitionEngine(recognizerInfo);

            if (SpeechRecognitionEngine.InstalledRecognizers().Count > 0)
            {
                try
                {
                    _recognizer.SetInputToDefaultAudioDevice();
                }
                catch (InvalidOperationException)
                {
                    ShowMessage("Nie wykryto mikrofonu.", "Błąd!");
                    return;
                }
            }
            else
            {
                ShowMessage("Brak dostępnych urządzeń wejściowych (mikrofonu).", "Błąd!");
                return;
            }

            var commands = new Choices("next", "previous", "ingredients", "close ingredients");
            var grammarBuilder = new GrammarBuilder(commands)
            {
                Culture = recognizerInfo.Culture
            };

            var grammar = new Grammar(grammarBuilder);
            _recognizer.LoadGrammar(grammar);
            _recognizer.SpeechRecognized += Recognizer_SpeechRecognized;

            _recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }
        catch (Exception ex)
        {
            ShowMessage($"Wystąpił błąd podczas inicjalizacji rozpoznawania mowy: {ex.Message}", "Błąd!");
        }
    }

    private void ShowMessage(string message, string title)
    {
        Task.Run(() =>
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                var owner = System.Windows.Application.Current.MainWindow;
                System.Windows.MessageBox.Show(owner, message, title, MessageBoxButton.OK, MessageBoxImage.Information);
            });
        });
    }

    private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
    {
        if (e.Result.Text.ToLower() == "next")
        {
            NextStepCommand.Execute(null);
        }
        else if (e.Result.Text.ToLower() == "previous")
        {
            PreviousStepCommand.Execute(null);
        }
        else if (e.Result.Text.ToLower() == "ingredients")
        {
            ShowIngredientsCommand.Execute(null);
        }
        else if (e.Result.Text.ToLower() == "close ingredients")
        {
            CloseIngredientsCommand.Execute(null);
        }
    }

    private void StopReading(object obj)
    {
        _synthesizer.SpeakAsyncCancelAll(); // Przerwij wszystkie wypowiedzi
    }

    private void ReadStepAloud(Step step)
    {
        if (step != null)
        {
            _synthesizer.SpeakAsyncCancelAll();
            var textToSpeak = $"Krok {step.StepNumber}: {step.Description}";
            _synthesizer.SpeakAsync(textToSpeak);
        }
    }

    private void LoadRecipeDetails(int recipeId)
    {
        var recipe = _dataHelper.LoadRecipeById(recipeId);

        if (recipe != null)
        {
            Steps = new ObservableCollection<Step>(_dataHelper.LoadSteps(recipeId));
            Ingredients = new ObservableCollection<Ingredient>(_dataHelper.LoadIngredients(recipeId));
            Title = recipe.Title;
        }
        else
        {
            MessageBox.Show("Nie znaleziono przepisu.");
        }
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

    private void ExecuteCloseIngredients(object obj)
    {
        if (_ingredientsWindow != null && _ingredientsWindow.IsVisible)
        {
            _ingredientsWindow.Close();
            _ingredientsWindow = null;
        }
    }
    private void UpdateStepVisibility()
    {
        IsPreviousStepVisible = CurrentStep.StepNumber > 1;
        IsNextStepVisible = _stepCounter - CurrentStep.StepNumber > 0;
    }

    private void PreviousStep(object obj)
    {
        if (CurrentStep.StepNumber > 1)
        {
            CurrentStep = Steps[CurrentStep.StepNumber - 2];
            if (IsReadingEnabled)
            {
                ReadStepAloud(CurrentStep);
            }
        }
    }

    private void NextStep(object obj)
    {
        if (_stepCounter - CurrentStep.StepNumber > 0)
        {
            CurrentStep = Steps[CurrentStep.StepNumber];
            if (IsReadingEnabled)
            {
                ReadStepAloud(CurrentStep);
            }
        }
    }
}
