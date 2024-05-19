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
    public abstract class NewsCrawler
    {
        protected readonly string pageUrl;

        protected readonly HttpClient client;

        public NewsCrawler(string pageUrl)
        {
            this.pageUrl = pageUrl;
            this.client = new HttpClient();
            this.NewestArticles = new ObservableCollection<Article>();
        }

        public ObservableCollection<Article> NewestArticles;

        public virtual async void CrawlNewestArticle()
        {
            await Task.Delay(0);

        }

        public async Task<BitmapImage> DownloadImageAsync(string imageUrl)
        {
            try
            {
                // Send a GET request to the image URL
                HttpResponseMessage response = await client.GetAsync(imageUrl);

                // Ensure the request was successful
                response.EnsureSuccessStatusCode();

                // Read the image data
                byte[] imageData = await response.Content.ReadAsByteArrayAsync();

                // Write the image data to the specified file

                // Create a BitmapImage from the byte array
                using (var stream = new MemoryStream(imageData))
                {
                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze(); // Freeze the BitmapImage to make it cross-thread accessible
                    return bitmapImage;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to download image from {imageUrl}. Error: {ex.Message}");
            }

            return null;
        }
    }
}
