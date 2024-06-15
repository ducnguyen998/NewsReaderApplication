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

        public ICommand HoverCommand { get; set; }

        public ICommand LeaveCommand { get; set; }

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

        #region Scrollable

        private double _verticalOffset;
        public double VerticalOffset
        {
            get => _verticalOffset;
            set
            {
                _verticalOffset = value;
                OnPropertyChanged();
            }
        }

        private double _horizontalOffset;
        public double HorizontalOffset
        {
            get => _horizontalOffset;
            set
            {
                _horizontalOffset = value;
                OnPropertyChanged();
            }
        }
        public void Scroll(string direction)
        {
            var step = 1;

            switch (direction)
            {
                case "Up":
                    VerticalOffset -= step;
                    break;
                case "Down":
                    VerticalOffset += step;
                    break;
                case "Left":
                    HorizontalOffset -= step;
                    break;
                case "Right":
                    HorizontalOffset += step;
                    break;
            }
        }

        public ICommand ScrollCommand { get; }


        private enum ScrollMode
        {
            Off = 0, On = 1,
        }

        private enum ScrollDirection
        {
            Up= 0, Down= 1,
        }

        private ScrollMode scrollMode = ScrollMode.Off;
        private ScrollDirection scrollDirection = ScrollDirection.Up;

        #endregion

        public ReadingViewmodel()
        {
            ArticleDetail = new ArticleDetail();
            CloseCommand = new RelayCommand(Close);
            Elements = new ObservableCollection<ObservableObject>();
            ScrollCommand = new RelayCommand<string>(Scroll);
            HoverCommand = new RelayCommand<string>(MouseHover);
            LeaveCommand = new RelayCommand<string>(LeaveHover);

            Task.Factory.StartNew(() => {

                while (true)
                {
                    if (this.scrollMode == ScrollMode.On)
                    {
                        switch (scrollDirection)
                        {
                            case ScrollDirection.Up:
                                this.Scroll("Up");
                                break;
                            case ScrollDirection.Down:
                                this.Scroll("Down");
                                break;
                        }
                    }

                    Task.Delay(1200);
                }
            });
        }

        private void MouseHover(string obj)
        {
            var arr = obj;

            if (arr == "up")
            {
                this.scrollDirection = ScrollDirection.Up;
            }
            else
            {
                this.scrollDirection = ScrollDirection.Down;
            }

            this.scrollMode = ScrollMode.On;

        }

        private void LeaveHover(string obj)
        {
            var arr = obj;

            if (arr == "up")
            {
                this.scrollDirection = ScrollDirection.Up;
            }
            else
            {
                this.scrollDirection = ScrollDirection.Down;
            }

            this.scrollMode = ScrollMode.Off;
        }

        private void Close()
        {
            this.ReadingModeVisibility = Visibility.Hidden;
            this.OnPropertyChanged(nameof(ReadingModeVisibility));
        }
    }
}
