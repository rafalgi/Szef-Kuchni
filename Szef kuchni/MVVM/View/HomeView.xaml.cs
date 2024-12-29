using System.Windows;
using System.Windows.Controls;
using Szef_kuchni.MVVM.ViewModel;

namespace Szef_kuchni.MVVM.View
{
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
            this.SizeChanged += OnWindowSizeChanged; 
        }

        private void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var viewModel = (HomeViewModel)this.DataContext;
            viewModel.SetColumnCount(e.NewSize.Width);
        }
    }
}
