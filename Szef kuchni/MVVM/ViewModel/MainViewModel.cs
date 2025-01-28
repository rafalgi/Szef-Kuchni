﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
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
        public ICommand MaximizeAppCommand { get; }
        public ICommand StartCookingCommand { get; }

        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand FavouriteViewCommand { get; set; }
        public RelayCommand SearchViewCommand { get; set; }
        public RelayCommand HistoryViewCommand { get; set; }
        public RelayCommand FilterViewCommand { get; set; }

        public HomeViewModel HomeVM { get; set; }
        public FavouriteViewModel FavouriteVM { get; set; }
        public SearchViewModel SearchVM { get; set; }
        public FilterViewModel FilterVM { get; set; }
        public HistoryViewModel HistoryViewModel { get; set; }

        private object _filterText;
        private object _currentView;
        private readonly Stack<object> _viewHistory = new Stack<object>();
        private ObservableCollection<Recipe> _filteredSortedRecipes;
        private Datahelper _dataHelper;

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public object FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                OnPropertyChanged();

                if (HomeVM != null && value is string filterText)
                {
                    HomeVM.FilterText = filterText;
                    SearchVM.FilterText = filterText;
                    HistoryViewModel.FilterText = filterText;
                    FavouriteVM.FilterText = filterText;
                }
            }
        }

        private bool _isTextBoxEnabled = true;

        public bool IsTextBoxEnabled
        {
            get => _isTextBoxEnabled;
            set
            {
                if (_isTextBoxEnabled != value)
                {
                    _isTextBoxEnabled = value;
                    OnPropertyChanged();  // Wywołanie PropertyChanged w celu powiadomienia UI.
                }
            }
        }

        public ObservableCollection<Recipe> FilteredSortedRecipes
        {
            get => _filteredSortedRecipes;
            set
            {
                if (_filteredSortedRecipes != value)
                {
                    _filteredSortedRecipes = value;
                    OnPropertyChanged(nameof(FilteredSortedRecipes));
                }
            }
        }

        public MainViewModel()
        {
            HomeVM = new HomeViewModel();
            FavouriteVM = new FavouriteViewModel();
            SearchVM = new SearchViewModel();
            FilterVM = new FilterViewModel();
            HistoryViewModel = new HistoryViewModel();
            CurrentView = HomeVM;

            string dbPath = @"../../recipes.db"; // ścieżka do bazy danych
            _dataHelper = new Datahelper(dbPath);

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
                IsTextBoxEnabled = true;
            });
            FavouriteViewCommand = new RelayCommand(o =>
            {
                IsTextBoxEnabled = true;
                FavouriteVM.LoadFavouriteRecipes(); // odśwież ulubione przepisy
                FavouriteVM.ApplyFilter();
                CurrentView = FavouriteVM;
            });
            SearchViewCommand = new RelayCommand(o =>
            {
                IsTextBoxEnabled = true;
                CurrentView = SearchVM;
            });
            FilterViewCommand = new RelayCommand(o =>
            {
                FilterText = string.Empty;
                IsTextBoxEnabled = false;
                CurrentView = FilterVM;
            });
            HistoryViewCommand = new RelayCommand(o =>
            {
                IsTextBoxEnabled = true;
                HistoryViewModel.LoadAllRecipes(); // odśwież ulubione przepisy
                HistoryViewModel.ApplyFilter();
                CurrentView = HistoryViewModel;
            });

            CloseAppCommand = new RelayCommand(ExecuteCloseApp);
            MinimizeAppCommand = new RelayCommand(ExecuteMinimizeApp);
            MaximizeAppCommand = new RelayCommand(ExecuteMaximizeApp);
            OpenRecipeCommand = new RelayCommand(OpenRecipe);
            GoBackCommand = new RelayCommand(GoBack);
            StartCookingCommand = new RelayCommand(StartCooking);

            FilterVM.PropertyChanged += FilterVM_PropertyChanged;
        }

        private void ExecuteCloseApp(object obj)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void ExecuteMinimizeApp(object obj)
        {
            System.Windows.Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void ExecuteMaximizeApp(object obj)
        {
            var mainWindow = System.Windows.Application.Current.MainWindow;
            if (mainWindow != null)
            {
                mainWindow.WindowState = mainWindow.WindowState == WindowState.Maximized
                    ? WindowState.Normal
                    : WindowState.Maximized;
            }
        }

        private void OpenRecipe(object parameter)
        {
            if (parameter is int recipeId)
            {
                _dataHelper.SaveHistory(recipeId);
                _dataHelper.EnsureHistoryLimit();
                _viewHistory.Push(CurrentView); 
                CurrentView = new RecipeDetailsWindow(recipeId);
                IsTextBoxEnabled = false;
            }
            else
            {
                System.Windows.MessageBox.Show("Nie udało się rozpoznać ID przepisu.");
            }
        }

        private void GoBack(object parameter)
        {

            if (_viewHistory.Count > 0)
            {
                if (CurrentView is StartCookingWindow)
                {
                    IsTextBoxEnabled = false;
                }
                else
                {
                    IsTextBoxEnabled = true;
                }

                HistoryViewModel.LoadAllRecipes();
                HistoryViewModel.ApplyFilter();
                FavouriteVM.LoadFavouriteRecipes();
                FavouriteVM.ApplyFilter();

                CurrentView = _viewHistory.Pop();
            }
            else
            {
                System.Windows.MessageBox.Show("Brak poprzednich widoków w historii.");
            }
        }

        private void StartCooking(object parameter)
        {
            if (parameter is int recipeId)
            {
                DialogResult result = System.Windows.Forms.MessageBox.Show("W kolejnym oknie będziesz miał możliwość sterowania głosem. Komendy, które są obsługiwane:                                                                                                    next (/nekst/) - następny krok                                                                             previous (/ˈpriːviəs/) - poprzedni krok                                                   ingredients (/ɪnˈɡriːdiənts/) - otwórz listę składników                                            close ingredients (/kloʊz ɪnˈɡriːdiənts/) - zamknij listę składników                       Jest także możliwość, aby kroki były czytane. Czy chcesz skorzystać z tej opcji?", "Informacja", MessageBoxButtons.YesNo);
                bool isReadingEnabled = (result == DialogResult.Yes);
                FilterText = string.Empty;
                _viewHistory.Push(CurrentView);
                CurrentView = new StartCookingWindow(recipeId, isReadingEnabled);
            }
            else
            {
                System.Windows.MessageBox.Show("Nie udało się rozpoznać ID przepisu.");
            }
        }

        private void FilterVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FilterViewModel.FilteredSortedRecipes))
            {
                SearchVM.FilteredSortedRecipes = FilterVM.FilteredSortedRecipes;
            }
        }
    }
}
