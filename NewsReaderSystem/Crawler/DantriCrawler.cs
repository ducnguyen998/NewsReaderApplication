using HtmlAgilityPack;
using NewsReaderSystem.Models;
using System;
using System.Collections.Generic;
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
    }
}
