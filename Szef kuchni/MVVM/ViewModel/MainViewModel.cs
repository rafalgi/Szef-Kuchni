using System.Windows;
using System.Windows.Input;
using Szef_kuchni.Core;
using System;


namespace Szef_kuchni.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        // Komendy aplikacji
        public ICommand CloseAppCommand { get; }
        public ICommand MinimizeAppCommand { get; }
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand FavouriteViewCommand { get; set; }
        public RelayCommand SearchViewCommand { get; set; }
        public RelayCommand HistoryViewCommand { get; set; }

        // ViewModels
        public HomeViewModel HomeVM { get; set; }
        public FavouriteViewModel FavouriteVM { get; set; }
        public SearchViewModel SearchVM { get; set; }
        public HistoryViewModel HistoryViewModel { get; set; }

        // Aktualnie wyświetlany widok
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

        // Wybrany obraz
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

        // Konstruktor
        public MainViewModel()
        {
            // Inicjalizacja ViewModels
            HomeVM = new HomeViewModel();
            FavouriteVM = new FavouriteViewModel();
            SearchVM = new SearchViewModel();
            HistoryViewModel = new HistoryViewModel();
            CurrentView = HomeVM;

            // Inicjalizacja komend
            HomeViewCommand = new RelayCommand(o => CurrentView = HomeVM);
            FavouriteViewCommand = new RelayCommand(o => CurrentView = FavouriteVM);
            SearchViewCommand = new RelayCommand(o => CurrentView = SearchVM);
            HistoryViewCommand = new RelayCommand(o => CurrentView = HistoryViewModel);
            CloseAppCommand = new RelayCommand(ExecuteCloseApp);
            MinimizeAppCommand = new RelayCommand(ExecuteMinimizeApp);
        }

        // Metoda do zamknięcia aplikacji
        private void ExecuteCloseApp(object obj)
        {
            Application.Current.Shutdown();
        }

        // Metoda do minimalizacji okna
        private void ExecuteMinimizeApp(object obj)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
    }
}
