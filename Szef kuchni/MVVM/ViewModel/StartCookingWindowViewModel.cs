using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Szef_kuchni.Core;
using Szef_kuchni.MVVM.Model;
using Szef_kuchni.MVVM.View;
using Szef_kuchni.MVVM.ViewModel;


internal class StartCookingWindowViewModel : ObservableObject
{
    private readonly Datahelper _dataHelper;
    private ObservableCollection<Step> _steps;
    private Step _currentStep;
    private ObservableCollection<Ingredient> _ingredients;
    private string _title;
    int _stepCounter;
    private IngredientsWindow _ingredientsWindow;

    public ICommand ShowIngredientsCommand { get; }
    public ICommand NextStepCommand { get; }
    public ICommand PreviousStepCommand { get; }

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
        }
    }

    public StartCookingWindowViewModel(int recipeId)
    {
        _dataHelper = new Datahelper(@"../../recipes.db");
        LoadRecipeDetails(recipeId);
        CurrentStep = Steps.FirstOrDefault();
        ShowIngredientsCommand = new RelayCommand(ExecuteShowIngredients);
        NextStepCommand = new RelayCommand(NextStep);
        PreviousStepCommand = new RelayCommand(PreviousStep);
        _stepCounter = _steps?.Count ?? 0;
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

    private void PreviousStep(object obj)
    {
        if (CurrentStep.StepNumber > 1)
        {
            CurrentStep = Steps[CurrentStep.StepNumber-2];
        }

    }

    private void NextStep(object obj)
    {
        if (_stepCounter - CurrentStep.StepNumber > 0)
        {
            CurrentStep = Steps[CurrentStep.StepNumber];
        }
    }
}

