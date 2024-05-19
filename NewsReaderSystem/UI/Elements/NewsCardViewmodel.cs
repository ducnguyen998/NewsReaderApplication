using CommunityToolkit.Mvvm.ComponentModel;
using NewsReaderSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsReaderSystem.UI.Elements
{
    public class NewsCardViewmodel : ObservableObject
    {
        private Article article;

        public NewsCardViewmodel(Article article)
        {
            this.article = article;
        }

        public Article Article 
        { 
            get
            {
                return article;
            }
            set
            {
                article = value;
                OnPropertyChanged();
            }
        }
    }
}
