using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Szef_kuchni.MVVM.View
{
    public partial class StartCookingWindow : UserControl
    {
        public StartCookingWindow(int recipeId)
        {
            InitializeComponent();
            this.DataContext = new StartCookingWindowViewModel(recipeId);
        }

        // Obsługa zdarzenia Unloaded
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            // Wywołanie komendy w ViewModelu
            var viewModel = (StartCookingWindowViewModel)this.DataContext;
            if (viewModel.StopReadingCommand.CanExecute(null))
            {
                viewModel.StopReadingCommand.Execute(null);
            }
        }
    }
}
