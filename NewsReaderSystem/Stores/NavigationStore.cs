using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsReaderSystem.Stores
{
    public class NavigationStore
    {
        public event Action CurrentViewmodelChanged;

        public ObservableObject CurrentViewmodel
        {
            get => currentViewmodel;
            set
            {
                currentViewmodel = value;
                OnCurrentViewmodelChanged();
            }
        }

        private void OnCurrentViewmodelChanged()
        {
            this.CurrentViewmodelChanged?.Invoke();
        }

        private ObservableObject currentViewmodel;
    }
}
