﻿using System;
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
    /// <summary>
    /// Interaction logic for StartCookingWindow.xaml
    /// </summary>
    public partial class StartCookingWindow : UserControl
    {
        public StartCookingWindow(int recipeId)
        {
            InitializeComponent();
            this.DataContext = new StartCookingWindowViewModel(recipeId);
        }
    }
}