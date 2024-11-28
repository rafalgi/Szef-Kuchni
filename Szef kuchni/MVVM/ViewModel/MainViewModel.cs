using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Szef_kuchni.Core;

namespace Szef_kuchni.MVVM.ViewModel
{
   
    internal class MainViewModel : ObservableObject
    {
        public ICommand CloseAppCommand { get; }
        public ICommand MinimizeAppCommand { get; }
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand FavouriteViewCommand { get; set; }
        public RelayCommand SearchViewCommand { get; set; }
        public RelayCommand HistoryViewCommand { get; set; }
        


        public HomeViewModel HomeVM { get; set; }   
        public FavouriteViewModel FavouriteVM { get; set; }
        public SearchViewModel SearchVM { get; set; }
        public HistoryViewModel HistoryViewModel { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
               
        }

        public MainViewModel()
        {
            HomeVM = new HomeViewModel();
            CurrentView = HomeVM;
            FavouriteVM = new FavouriteViewModel();
            SearchVM = new SearchViewModel();
            HistoryViewModel = new HistoryViewModel();

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });

            FavouriteViewCommand = new RelayCommand(o =>
            {
                CurrentView = FavouriteVM;
            });

            SearchViewCommand = new RelayCommand(o =>
            {
                CurrentView = SearchVM;
            });


            HistoryViewCommand = new RelayCommand(o =>
            {
                CurrentView = CurrentView;
            });

            CloseAppCommand = new RelayCommand(ExecuteCloseApp);
            MinimizeAppCommand = new RelayCommand(ExecuteMinimizeApp);
        }

        private void ExecuteCloseApp(object obj)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void ExecuteMinimizeApp(object obj)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
    }
}
