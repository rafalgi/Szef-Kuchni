using System.Windows;
using System.Windows.Input;
using Szef_kuchni.Core;
using System;
using Szef_kuchni.MVVM.View;


namespace Szef_kuchni.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        public ICommand CloseAppCommand { get; }
        public ICommand MinimizeAppCommand { get; }
        public ICommand OpenRecipeCommand { get; }
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand FavouriteViewCommand { get; set; }
        public RelayCommand SearchViewCommand { get; set; }
        public RelayCommand HistoryViewCommand { get; set; }

        public HomeViewModel HomeVM { get; set; }
        public FavouriteViewModel FavouriteVM { get; set; }
        public SearchViewModel SearchVM { get; set; }
        public HistoryViewModel HistoryViewModel { get; set; }

        // aktualnie wyświetlany widok
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        private string _selectedImage;
        public string SelectedImage
        {
            get => _selectedImage;
            set
            {
                _selectedImage = value;
                OnPropertyChanged();
            }
        }

        //konstruktor
        public MainViewModel()
        {
            HomeVM = new HomeViewModel();
            FavouriteVM = new FavouriteViewModel();
            SearchVM = new SearchViewModel();
            HistoryViewModel = new HistoryViewModel();
            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o => CurrentView = HomeVM);
            FavouriteViewCommand = new RelayCommand(o => CurrentView = FavouriteVM);
            SearchViewCommand = new RelayCommand(o => CurrentView = SearchVM);
            HistoryViewCommand = new RelayCommand(o => CurrentView = HistoryViewModel);
            CloseAppCommand = new RelayCommand(ExecuteCloseApp);
            MinimizeAppCommand = new RelayCommand(ExecuteMinimizeApp);

            OpenRecipeCommand = new RelayCommand(OpenRecipe);
        }

        // zamykanie aplikacji na X
        private void ExecuteCloseApp(object obj)
        {
            Application.Current.Shutdown();
        }

        // minimalizacja aplikacji na -
        private void ExecuteMinimizeApp(object obj)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
        // otwarcie nowego okna z dokładniejszym przepisem
        private void OpenRecipe(object parameter)
        {
            if (parameter is int recipeId)
            {
                CurrentView = new RecipeDetailsWindow(recipeId);
            }
            else
            {
                MessageBox.Show("Nie udało się rozpoznać ID przepisu.");  // sprawdzenie 
            }
        }

    }
}