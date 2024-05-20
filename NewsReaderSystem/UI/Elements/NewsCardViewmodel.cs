using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NewsReaderSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NewsReaderSystem.UI.Elements
{
    public class NewsCardViewmodel : ObservableObject
    {
        private Article article;

        public NewsCardViewmodel(Article article)
        {
            this.article = article;
            this.AccessNewsCommand = new RelayCommand(OnAccessNewsExecuted);
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

        public ICommand AccessNewsCommand { get; set; }

        public event EventHandler<Article> AccessNewsExecuted;

        private void OnAccessNewsExecuted()
        {
            this.AccessNewsExecuted?.Invoke(this, article);
        }
    }
}
