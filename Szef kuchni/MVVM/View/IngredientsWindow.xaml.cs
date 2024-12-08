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
using Szef_kuchni.MVVM.ViewModel;

namespace Szef_kuchni.MVVM.View
{
    /// <summary>
    /// Logika interakcji dla klasy IngredientsWindow.xaml
    /// </summary>
    public partial class IngredientsWindow : Window
    {
        public IngredientsWindow()
        {
            InitializeComponent();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        // Zamykanie aplikacji
        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Zamyka okno
        }

        // Minimalizowanie aplikacji
        private void MinimizeApp_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized; // Minimalizuje okno
        }
    }
}
