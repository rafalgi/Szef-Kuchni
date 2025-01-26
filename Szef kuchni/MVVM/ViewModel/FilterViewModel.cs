using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Szef_kuchni.Core;

namespace Szef_kuchni.MVVM.ViewModel
{
    internal class FilterViewModel : ObservableObject
    {
        private readonly Datahelper _dataHelper;
        private ObservableCollection<Recipe> _filteredSortedRecipes;

        public ObservableCollection<Recipe> FilteredSortedRecipes
        {
            get => _filteredSortedRecipes;
            set
            {
                _filteredSortedRecipes = value;
                OnPropertyChanged();
            }
        }

        public FilterViewModel()
        {
            string dbPath = @"../../recipes.db"; // ścieżka do bazy danych
            _dataHelper = new Datahelper(dbPath);
            FilteredSortedRecipes= new ObservableCollection<Recipe>(_dataHelper.LoadRecipes());
        }
    }
}
