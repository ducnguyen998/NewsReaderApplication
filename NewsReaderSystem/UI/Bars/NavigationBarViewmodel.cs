using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NewsReaderSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NewsReaderSystem.UI.Bars
{
    public class NavigationBarViewmodel : ObservableObject
    {
        public ICommand NavigateTopStoriesCommand { get; set; }
        public ICommand NavigateSectionsCommand { get; set; }

        public NavigationBarViewmodel(IServiceProvider serviceProvider)
        {
            this.NavigateTopStoriesCommand 
                = new NavigateCommand(serviceProvider.CreateSingleViewNavigationService(Defines.EView.TopStories));
            this.NavigateSectionsCommand 
                = new NavigateCommand(serviceProvider.CreateSingleViewNavigationService(Defines.EView.Sections));
        }
    }

    public class NavigateCommand : ICommand
    {
        private readonly INavigationService navigationService;

        public NavigateCommand(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            navigationService.Navigate();
        }
    }
}
