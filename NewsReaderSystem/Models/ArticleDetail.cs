using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsReaderSystem.Models
{
    public class ArticleDetail : ObservableObject
    {
        public string Title { get; set; }
        public string Sapo { get; set; }
        public Author Author { get; set; }

        public ObservableCollection<IArticleContentElement> Elements { get; set; }

        public void SetProperties(ArticleDetail articleDetail)
        {
            this.Title = articleDetail.Title;
            this.Sapo = articleDetail.Sapo;
            this.Author = articleDetail.Author;
            this.Elements = articleDetail.Elements;
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Sapo));
            OnPropertyChanged(nameof(Author));
            OnPropertyChanged(nameof(Elements));
        }
    }
}
