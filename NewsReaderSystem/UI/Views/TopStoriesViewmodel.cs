using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NewsReaderSystem.Crawler;
using NewsReaderSystem.Models;
using NewsReaderSystem.UI.Bars;
using NewsReaderSystem.UI.Elements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NewsReaderSystem.UI.Views
{
    public class TopStoriesViewmodel : ObservableObject
    {
        private readonly DantriCrawler dantriCrawler;

        public ObservableCollection<NewsCardViewmodel> NewsArticles { get; set; }
        public ObservableCollection<NewsCardViewmodel> DisplayNewsArticles { get; set; }

        public List<ObservableCollection<NewsCardViewmodel>> Pages { get; set; }

        public PaginationBarViewmodel PaginationBar { get; set; }

        public ICommand RefreshCommand { get; set; }

        public ReadingViewmodel ArticleContent { get; set; }

        public TopStoriesViewmodel(ReadingViewmodel readingViewmodel, PaginationBarViewmodel paginationBarViewmodel)
        {
            RefreshCommand = new RelayCommand(DoRefresh);
            PaginationBar = paginationBarViewmodel;
            PaginationBar.PaginationCommandRaised += OnPaginationCommandRaised;
            ArticleContent = readingViewmodel;
            NewsArticles = new ObservableCollection<NewsCardViewmodel>();
            DisplayNewsArticles = new ObservableCollection<NewsCardViewmodel>();
            dantriCrawler = new DantriCrawler();
            dantriCrawler.NewestArticles.CollectionChanged += OnCrawlerArticleChanged;
            dantriCrawler.CrawlNewestArticle();
        }

        private void DoRefresh()
        {
            dantriCrawler.CrawlNewestArticle();
        }

        private void OnPaginationCommandRaised(object sender, (PaginationBarViewmodel.ECommand, int) e)
        {
            Console.WriteLine("------------------ Command raised : " + e.Item1 + ", " + e.Item2);
            if (Pages == null || Pages.Count == 0) return;
            DisplayNewsArticles = Pages[e.Item2];
            OnPropertyChanged(nameof(DisplayNewsArticles));
        }

        private void OnCrawlerArticleChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                this.NewsArticles.Clear();
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Article item in e.NewItems)
                {
                    this.NewsArticles.Add(new NewsCardViewmodel(item));
                    this.NewsArticles.Last().AccessNewsExecuted += OnCardAccessNewsExecuted;
                }
            }
            //else if (e.Action == NotifyCollectionChangedAction.Remove)
            //{
            //    foreach (Article item in e.NewItems)
            //    {
            //        this.NewsArticles.Remove(new NewsCardViewmodel(item));
            //    }
            //}

            Pages = PaginationBar.SplitPages(NewsArticles, 2);

            OnPropertyChanged(nameof(Pages));
            OnPropertyChanged(nameof(NewsArticles));   
        }

        private async void OnCardAccessNewsExecuted(object sender, Article e)
        {
            var articleDetail = await this.dantriCrawler.CrawlArticleDetail(e);
            this.ArticleContent.SetArticleDetail(articleDetail);
        }
    }
}
