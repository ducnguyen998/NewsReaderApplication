using CommunityToolkit.Mvvm.ComponentModel;
using NewsReaderSystem.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsReaderSystem.Services
{
    public class NavigationService<T> : INavigationService where T : ObservableObject
    {
        private readonly NavigationStore navigationStore;

        private readonly Func<T> createViewmodel;

        public NavigationService(NavigationStore navigationStore, Func<T> createViewmodel)
        {
            this.navigationStore = navigationStore;
            this.createViewmodel = createViewmodel;
        }

        public void Navigate()
        {
            this.navigationStore.CurrentViewmodel = createViewmodel();
        }
    }
}

