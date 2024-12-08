using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Szef_kuchni.Core;
using Szef_kuchni.MVVM.Model;
using Szef_kuchni.MVVM.View;

namespace Szef_kuchni.MVVM.ViewModel
{
    internal class IngredientsWindowViewModel : ObservableObject
    {
        public ObservableCollection<Ingredient> Ingredients { get; }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public IngredientsWindowViewModel(ObservableCollection<Ingredient> ingredients, string title)
        {
            Ingredients = ingredients;
            Title = title;
        }
    }
}
