using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NewsReaderSystem.Models;
using NewsReaderSystem.UI.Elements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NewsReaderSystem.UI.Views
{
    public class ReadingViewmodel : ObservableObject
    {
        public ICommand CloseCommand { get; set; }

        public ArticleDetail ArticleDetail { get; set; }

        public Visibility ReadingModeVisibility { get; set; } = Visibility.Hidden;

        public ObservableCollection<ObservableObject> Elements { get; set; }

        public void SetArticleDetail(ArticleDetail articleDetail)
        {
            this.ArticleDetail.SetProperties(articleDetail);
            this.ReadingModeVisibility = Visibility.Visible;
            this.OnPropertyChanged(nameof(ReadingModeVisibility));
            this.Elements.Clear();
            foreach (var element in articleDetail.Elements)
            {
                if (element.ContentType == EContentType.Paragraph)
                {
                    this.Elements.Add(new ParagraphViewmodel(element));
                }

                if (element.ContentType == EContentType.Figure)
                {
                    this.Elements.Add(new FigureViewmodel(element));
                }
            }
        }

        public ReadingViewmodel()
        {
            ArticleDetail = new ArticleDetail();
            CloseCommand = new RelayCommand(Close);
            Elements = new ObservableCollection<ObservableObject>();
        }

        private void Close()
        {
            this.ReadingModeVisibility = Visibility.Hidden;
            this.OnPropertyChanged(nameof(ReadingModeVisibility));
        }
    }
}
