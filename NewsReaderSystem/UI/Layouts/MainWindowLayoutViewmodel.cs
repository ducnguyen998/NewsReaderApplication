using CommunityToolkit.Mvvm.ComponentModel;
using NewsReaderSystem.UI.Bars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsReaderSystem.UI.Layouts
{
    public class MainWindowLayoutViewmodel : ObservableObject
    {
        private NavigationBarViewmodel navigationBarViewmodel;

        private ObservableObject currentViewmodel;

        public MainWindowLayoutViewmodel(NavigationBarViewmodel navigationBarViewmodel, ObservableObject currentViewmodel)
        {
            this.navigationBarViewmodel = navigationBarViewmodel;
            this.currentViewmodel = currentViewmodel;
        }

        public NavigationBarViewmodel NavigationBar 
        { 
            get
            {
                return navigationBarViewmodel;
            }
            set
            {
                navigationBarViewmodel = value;
                OnPropertyChanged();
            }
        }

        public ObservableObject CurrentViewmodel
        {
            get
            {
                return currentViewmodel;
            }
            set
            {
                currentViewmodel = value;
                OnPropertyChanged();
            }
        }
    }
}
