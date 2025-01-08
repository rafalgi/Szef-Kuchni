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
using System.Windows.Navigation;
using System.Windows;
using System.Windows.Controls;
using Szef_kuchni.MVVM.ViewModel;

namespace Szef_kuchni.MVVM.View
{
    /// <summary>
    /// Interaction logic for FavouriteView.xaml
    /// </summary>
    public partial class FavouriteView : UserControl
    {
        public FavouriteView()
        {
            InitializeComponent();
            this.SizeChanged += OnWindowSizeChanged;
        }

        private void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var viewModel = (FavouriteViewModel)this.DataContext;
            viewModel.SetColumnCount(e.NewSize.Width);
        }

    }
}
