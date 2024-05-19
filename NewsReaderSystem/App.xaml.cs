using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using NewsReaderSystem.Defines;
using NewsReaderSystem.Services;
using NewsReaderSystem.Stores;
using NewsReaderSystem.UI.Bars;
using NewsReaderSystem.UI.Layouts;
using NewsReaderSystem.UI.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace NewsReaderSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton(s => s.CreateServiceProvider());

            services.AddSingleton<NavigationStore>();
            services.AddSingleton<NavigationBarViewmodel>();

            services.AddSingleton<PaginationBarViewmodel>();

            services.AddSingleton<SectionsViewmodel>();
            services.AddSingleton<TopStoriesViewmodel>();

            services.AddSingleton(s => s.CreateSingleViewNavigationService(EView.TopStories));

            services.AddSingleton<MainWindowViewmodel>();
            services.AddSingleton(s => s.CreateMainWindow());

            this.serviceProvider = services.BuildServiceProvider(); 
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            serviceProvider.GetRequiredService<INavigationService>().Navigate();
            serviceProvider.GetRequiredService<MainWindow>().Show();
        }
    }

    public static class ApplicationExtension
    {
        public static IServiceProvider CreateServiceProvider(this IServiceProvider serviceProvider)
        {
            return serviceProvider;
        }
        public static MainWindow CreateMainWindow(this IServiceProvider serviceProvider)
        {
            return new MainWindow() { DataContext = serviceProvider.GetRequiredService<MainWindowViewmodel>() };
        }
        public static INavigationService CreateSingleViewNavigationService(this IServiceProvider serviceProvider, EView eView)
        {
            ObservableObject contentViewmodel;

            switch (eView)
            {
                case EView.TopStories:
                    contentViewmodel = serviceProvider.GetRequiredService<TopStoriesViewmodel>();
                    break;
                case EView.Sections:
                    contentViewmodel = serviceProvider.GetRequiredService<SectionsViewmodel>();
                    break;
                default:
                    contentViewmodel = null;
                    break;
            }

            return new LayoutNavigationService<ObservableObject>(
                    serviceProvider.GetRequiredService<NavigationStore>(),
                    () => serviceProvider.CreateNavigationBarViewmodel(),
                    () => contentViewmodel);
        }
        public static NavigationBarViewmodel CreateNavigationBarViewmodel(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<NavigationBarViewmodel>();
        }
    }
}
