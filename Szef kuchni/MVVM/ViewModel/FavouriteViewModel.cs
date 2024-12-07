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
    internal class FavouriteViewModel
    {
        public ICommand GoBackCommand { get; }

        public FavouriteViewModel()
        {
            GoBackCommand = new RelayCommand(ExecuteGoBack);
        }

        private void ExecuteGoBack(object obj) // Metoda, która umożliwia powrót na stronę główną
        {
            Application.Current.MainWindow.DataContext = new MainViewModel();
        }
    }
}
