using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NewsReaderSystem.UI.Views
{
    /// <summary>
    /// Interaction logic for ReadingView.xaml
    /// </summary>
    public partial class ReadingView : UserControl
    {
        public ReadingView()
        {
            InitializeComponent();
            DataContextChanged += ReadingView_DataContextChanged;
        }

        private void ReadingView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ReadingViewmodel viewModel)
            {
                viewModel.PropertyChanged += ViewModel_PropertyChanged;
            }

        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is ReadingViewmodel viewModel)
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    if (e.PropertyName == nameof(viewModel.VerticalOffset))
                    {
                        scrollViewer.ScrollToVerticalOffset(viewModel.VerticalOffset);
                    }
                    else if (e.PropertyName == nameof(viewModel.HorizontalOffset))
                    {
                        scrollViewer.ScrollToHorizontalOffset(viewModel.HorizontalOffset);
                    }
                });
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (DataContext is ReadingViewmodel viewModel)
            {
                viewModel.VerticalOffset = scrollViewer.VerticalOffset;
                viewModel.HorizontalOffset = scrollViewer.HorizontalOffset;
            }
        }
    }
}
