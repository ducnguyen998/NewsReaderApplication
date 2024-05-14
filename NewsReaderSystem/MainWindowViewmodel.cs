using CommunityToolkit.Mvvm.ComponentModel;
using NewsReaderSystem.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsReaderSystem
{
    public class MainWindowViewmodel : ObservableObject
    {
        private readonly NavigationStore navigationStore;
        public MainWindowViewmodel(NavigationStore navigationStore)
        {
            this.navigationStore = navigationStore;
            this.navigationStore.CurrentViewmodelChanged += () => OnPropertyChanged(nameof(CurrentViewmodel));
        }
        public ObservableObject CurrentViewmodel => navigationStore.CurrentViewmodel;
    }
}
