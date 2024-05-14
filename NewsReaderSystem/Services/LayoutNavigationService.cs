using CommunityToolkit.Mvvm.ComponentModel;
using NewsReaderSystem.Stores;
using NewsReaderSystem.UI.Bars;
using NewsReaderSystem.UI.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsReaderSystem.Services
{
    public class LayoutNavigationService<T> : INavigationService where T : ObservableObject
    {
        private readonly NavigationStore navigationStore;

        private readonly Func<NavigationBarViewmodel> createNavigationBar;

        private readonly Func<T> createViewmodel;

        public LayoutNavigationService(NavigationStore navigationStore, Func<NavigationBarViewmodel> createNavigationBar, Func<T> createViewmodel)
        {
            this.navigationStore = navigationStore;
            this.createNavigationBar = createNavigationBar;
            this.createViewmodel = createViewmodel;
        }

        public void Navigate()
        {
            this.navigationStore.CurrentViewmodel = new MainWindowLayoutViewmodel(createNavigationBar(), createViewmodel());
        }
    }
}
