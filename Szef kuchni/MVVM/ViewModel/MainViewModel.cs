﻿using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Szef_kuchni.Core;
using Szef_kuchni.MVVM.View;

namespace Szef_kuchni.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        public ICommand CloseAppCommand { get; }
        public ICommand MinimizeAppCommand { get; }
        public ICommand OpenRecipeCommand { get; }
        public ICommand GoBackCommand { get; }
        
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand FavouriteViewCommand { get; set; }
        public RelayCommand SearchViewCommand { get; set; }
        public RelayCommand HistoryViewCommand { get; set; }

        public HomeViewModel HomeVM { get; set; }
        public FavouriteViewModel FavouriteVM { get; set; }
        public SearchViewModel SearchVM { get; set; }
        public HistoryViewModel HistoryViewModel { get; set; }

        private object _currentView;
        private readonly Stack<object> _viewHistory = new Stack<object>();

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

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
            GoBackCommand = new RelayCommand(GoBack);
        }

        private void ExecuteCloseApp(object obj)
        {
            Application.Current.Shutdown();
        }

        private void ExecuteMinimizeApp(object obj)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void OpenRecipe(object parameter)
        {
            if (parameter is int recipeId)
            {
                _viewHistory.Push(CurrentView); // Zapisz obecny widok
                CurrentView = new RecipeDetailsWindow(recipeId);
            }
            else
            {
                MessageBox.Show("Nie udało się rozpoznać ID przepisu.");
            }
        }

        private void GoBack(object parameter)
        {
            if (_viewHistory.Count > 0)
            {
                CurrentView = _viewHistory.Pop(); // Pobierz poprzedni widok
            }
            else
            {
                MessageBox.Show("Brak poprzednich widoków w historii.");
            }
        }
    }
}
