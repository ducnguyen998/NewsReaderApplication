using HtmlAgilityPack;
using NewsReaderSystem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace NewsReaderSystem.Crawler
{
    public class DantriCrawler : NewsCrawler
    {
        public DantriCrawler(string pageUrl = "https://dantri.com.vn") : base(pageUrl)
        {
        }

        public override async void CrawlNewestArticle()
        {
            var newestUrl = pageUrl + "/tin-moi-nhat.htm";

            HttpResponseMessage response = await client.GetAsync(newestUrl);

            if (response.IsSuccessStatusCode)
            {
                this.NewestArticles.Clear();

                // Read the content of the page
                string pageContent = await response.Content.ReadAsStringAsync();

                // Load the HTML content into HtmlDocument
                HtmlDocument document = new HtmlDocument();

                document.LoadHtml(pageContent);

                // Find the news articles
                var articles = document.DocumentNode.SelectNodes("//article");

                if (articles != null)
                {
                    foreach (var article in articles)
                    {
                        // Attempt to extract the title from multiple potential elements
                        var titleNode = article.SelectSingleNode(".//h2") ??
                                        article.SelectSingleNode(".//h3") ??
                                        article.SelectSingleNode(".//h4");
                        if (titleNode == null)
                        {
                            continue;
                        }
                        string title = titleNode.InnerText.Trim().Replace("&quot;", "'");

                        // Extract the link
                        var linkNode = titleNode.SelectSingleNode(".//a");
                        if (linkNode == null)
                        {
                            continue;
                        }
                        string link = linkNode.GetAttributeValue("href", string.Empty);
                        string fullLink = link.StartsWith("/") ? "https://dantri.com.vn" + link : link;

                        // Extract the summary if available
                        var summaryNode = article.SelectSingleNode(".//div[contains(@class, 'article-excerpt')]");
                        string summary = summaryNode != null ? summaryNode.InnerText.Trim().Replace("&quot;", "'") : string.Empty;

                        // Extrct the time

                        var timeNode = article.SelectSingleNode(".//div[contains(@class, 'article-time')]");
                        string time = timeNode != null ? timeNode.InnerText.Trim().Replace("&#x27;", " phút") : string.Empty;

                        // Extrct the time

                        var thumbNode = article.SelectSingleNode(".//div[contains(@class, 'article-thumb')]");
                        var imgNode = thumbNode.SelectSingleNode(".//img");
                        string imageUrl = imgNode != null ? imgNode.GetAttributeValue("src", string.Empty) : string.Empty;


                        Console.WriteLine($"Title: {title}");
                        Console.WriteLine($"Link: {fullLink}");
                        Console.WriteLine($"Summary: {summary}");
                        Console.WriteLine($"Thumb: {imageUrl}");
                        Console.WriteLine(new string('-', 80));

                        BitmapImage image = await DownloadImageAsync(imageUrl);

                        this.NewestArticles.Add(new Article()
                        {
                            Page = "dantri.com.vn",
                            Title = title,
                            Url = fullLink,
                            Summary = summary,
                            Time = time,
                            ImageUrl = imageUrl,
                            ImageSource = image
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No articles found.");
                }
            }
            else
            {
                Console.WriteLine($"Failed to retrieve the page. Status code: {response.StatusCode}");
            }
        }

        public override async Task<ArticleDetail> CrawlArticleDetail(Article article)
        {
            var newestUrl = article.Url;

            HttpResponseMessage response = await client.GetAsync(newestUrl);

            if (response.IsSuccessStatusCode)
            {
                var retArticleDetail = new ArticleDetail();

                // Read the content of the page
                string pageContent = await response.Content.ReadAsStringAsync();

                // Load the HTML content into HtmlDocument
                HtmlDocument document = new HtmlDocument();

                document.LoadHtml(pageContent);

                // Find the article singular-container
                var detailArticles = document.DocumentNode.SelectNodes("//article[contains(@class, 'singular-container')]") ??
                                     document.DocumentNode.SelectNodes("//article[contains(@class, 'e-magazine')]");

                var detailArticle = detailArticles?.FirstOrDefault();

                if (detailArticle != null)
                {
                    #region Title

                    var titleNode = detailArticle.SelectSingleNode(".//h1[contains(@class, 'title-page')]") ??
                                    detailArticle.SelectSingleNode(".//h1[contains(@class, 'e-magazine__title')]");
                    string title = titleNode.InnerText.Trim().Replace("&quot;", "'");
                    retArticleDetail.Title = title;

                    #endregion

                    #region Author

                    var authorWrap = detailArticle.SelectSingleNode(".//div[contains(@class, 'author-wrap')]") ?? 
                                     detailArticle.SelectSingleNode(".//div[contains(@class, 'e-magazine__met')]");

                    // Extract the authoName if available
                    var authorNameNode = authorWrap.SelectSingleNode(".//div[contains(@class, 'author-name')]") ?? 
                                         authorWrap.SelectSingleNode(".//span[contains(@class, 'e-magazine__meta-item')]");
                    string authorName = authorNameNode != null ? authorNameNode.InnerText.Trim() : string.Empty;

                    // Extrct the time

                    var timeNode = authorWrap.SelectSingleNode(".//div[contains(@class, 'author-time')]") ?? 
                                   authorWrap.SelectSingleNode(".//time[contains(@class, 'e-magazine__meta-item')]");
                    string time = timeNode != null ? timeNode.InnerText.Trim() : string.Empty;

                    // Extrct the author image

                    var thumbNode = authorWrap.SelectSingleNode(".//div[contains(@class, 'author-avatar')]");
                    var imgNode = thumbNode?.SelectSingleNode(".//img");
                    string imageUrl = imgNode != null ? imgNode.GetAttributeValue("src", string.Empty) : string.Empty;

                    BitmapImage avatar = await DownloadImageAsync(imageUrl);

                    retArticleDetail.Author = new Author()
                    {
                        Name = authorName,
                        PostedTime = time,
                        Avatar = avatar
                    };

                    #endregion

                    #region Sapo

                    var sapoNode = detailArticle.SelectSingleNode(".//h2[contains(@class, 'singular-sapo')]") ?? 
                                   detailArticle.SelectSingleNode(".//h2[contains(@class, 'e-magazine__sapo')]");
                    string sapo = sapoNode.InnerText.Trim().Replace("&quot;", "'");

                    retArticleDetail.Sapo = sapo;

                    #endregion

                    #region Content elements

                    var contentNode = detailArticle.SelectSingleNode(".//div[contains(@class, 'singular-content')]") ?? 
                                      detailArticle.SelectSingleNode(".//div[contains(@class, 'e-magazine__body')]");
                    var elementNodes = contentNode.ChildNodes;

                    retArticleDetail.Elements = new ObservableCollection<IArticleContentElement>();

                    foreach (var elementNode in elementNodes)
                    {
                        if (elementNode.Name == "p")
                        {
                            retArticleDetail.Elements.Add(new ParagraphElement()
                            {
                                Content = elementNode.InnerText
                            });
                        }
                        else if (elementNode.Name == "figure")
                        {
                            var figNode = elementNode.SelectSingleNode(".//img");
                            string figUrl = figNode != null ? figNode.GetAttributeValue("data-original", string.Empty) : string.Empty;
                            string figTitle = figNode != null ? figNode.GetAttributeValue("title", string.Empty) : string.Empty;

                            BitmapImage fig = await DownloadImageAsync(figUrl);

                            Console.WriteLine("--------------------------- " + figUrl);

                            retArticleDetail.Elements.Add(new FigureElement()
                            {
                                Image = fig,
                                Title = figTitle
                            });
                        }
                    }

                    #endregion

                    Console.WriteLine($"Title: {title}");
                    Console.WriteLine($"Author: {authorName} - {time}");
                    Console.WriteLine($"Summary: {sapo}");
                    Console.WriteLine("Elements:");
                    foreach (var item in retArticleDetail.Elements)
                    {
                        if (item is ParagraphElement e1)
                        {
                            Console.WriteLine("p : " + e1.Content);
                        }

                        if (item is FigureElement e2)
                        {
                            Console.WriteLine("f : " + e2.Title);
                        }
                    }
                    Console.WriteLine(new string('-', 80));

                    BitmapImage image = await DownloadImageAsync(imageUrl);

                    return retArticleDetail;
                }
                else
                {
                    Console.WriteLine("No articles found.");
                }
            }
            else
            {
                Console.WriteLine($"Failed to retrieve the page. Status code: {response.StatusCode}");
            }

            return null;
        }
    }
}
