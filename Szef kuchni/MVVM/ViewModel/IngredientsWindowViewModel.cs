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

        public IngredientsWindowViewModel(ObservableCollection<Ingredient> ingredients)
        {
            Ingredients = ingredients;
        }

    }
}
