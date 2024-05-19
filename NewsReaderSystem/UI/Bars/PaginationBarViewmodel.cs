using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NewsReaderSystem.Models;
using NewsReaderSystem.UI.Elements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NewsReaderSystem.UI.Bars
{
    public class PaginationBarViewmodel : ObservableObject
    {
        public List<bool> CheckedPages { get; set; }
        public List<bool> EnabledPages { get; set; }
        public List<string> ContentPages { get; set; }

        public ICommand NextCommand { get; set; }
        public ICommand PrevCommand { get; set; }

        public enum ECommand
        {
            Next, Previous
        }

        public event EventHandler<(ECommand, int)> PaginationCommandRaised;

        public PaginationBarViewmodel()
        {
            NextCommand = new AsyncRelayCommand(Next);
            PrevCommand = new AsyncRelayCommand(Previous);

            CheckedPages = new List<bool>();

            for (int i = 0; i < 5; i++)
            {
                CheckedPages.Add(false);
            }

            EnabledPages = new List<bool>();

            for (int i = 0; i < 5; i++)
            {
                EnabledPages.Add(false);
            }

            ContentPages = new List<string>();

            for (int i = 0; i < 5; i++)
            {
                ContentPages.Add((0).ToString());
            }
        }

        public List<ObservableCollection<NewsCardViewmodel>> SplitPages(ObservableCollection<NewsCardViewmodel> articles, int artilePerPage)
        {
            var totalArticle = articles.Count;

            this.totalPages = totalArticle % artilePerPage > 0 ? totalArticle / artilePerPage + 1 : totalArticle / artilePerPage;

            Console.WriteLine(totalPages);

            var splittedCollection = new List<ObservableCollection<NewsCardViewmodel>>();

            for (int i = 0; i < totalArticle; i++) 
            { 
                if (i % artilePerPage == 0)
                {
                    splittedCollection.Add(new ObservableCollection<NewsCardViewmodel>());
                }

                splittedCollection.Last().Add(articles[i]);
            }


            var pgIdx = 0;
            setPage(pgIdx);
            OnCommandRaised(ECommand.Previous, pgIdx);

            return splittedCollection;
        }

        private async Task Next()
        {
            if (currentPage < this.totalPages - 1)
            {
                await Task.Run(() =>
                {
                    setPage(currentPage + 1);
                    OnCommandRaised(ECommand.Next, currentPage);
                });
            }
        }

        private async Task Previous()
        {
            if (currentPage > 0)
            {
                await Task.Run(() => 
                {
                    setPage(currentPage - 1);
                    OnCommandRaised(ECommand.Previous, currentPage);
                });
            }
        }

        private void setPage(int idx)
        {
            setPageLayout(idx + 1);
            this.currentPage = idx;
            OnPropertyChanged(nameof(ContentPages));
            OnPropertyChanged(nameof(EnabledPages));
            OnPropertyChanged(nameof(CheckedPages));
        }

        private void setPageLayoutFirst()
        {
            for (int i = 0; i < 5; i++)
            {
                ContentPages[i] = (i + 1).ToString();

                if (i <= totalPages - 1)
                {
                    EnabledPages[i] = true;
                }
                else
                {
                    EnabledPages[i] = false;
                }
            }
        }

        private void setPageLayoutLast()
        {
            if (totalPages <= 5) return;

            for (int i = totalPages - 5; i < totalPages; i++)
            {
                ContentPages[i - totalPages + 5] = (i + 1).ToString();
            }
        }

        private void setPageLayout(int idx)
        {
            CheckedPages.ForEach(x => x = false);

            if (idx <= 3)
            {
                setPageLayoutFirst();
                CheckedPages[idx % 5 - 1] = true;
            }
            else if (idx >= totalPages - 2)
            {
                setPageLayoutLast();
                CheckedPages[idx % 5] = true;
            }
            else
            {
                for (int i = idx - 3; i < idx + 2; i++)
                {
                    ContentPages[i - idx + 3] = (i + 1).ToString();
                }

                CheckedPages[2] = true;
            }
        }

        private void OnCommandRaised(ECommand cmd, int currentPage)
        {
            this.PaginationCommandRaised?.Invoke(this, (cmd, currentPage));
        }

        private int totalPages = 9;

        private int currentPage;
    }
}
