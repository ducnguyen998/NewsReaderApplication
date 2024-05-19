using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace NewsReaderSystem.Models
{
    public class Article
    {
        public string Page { get; set; }
        public string Title { get; set; }
        public string Time { get; set; }
        public string Url { get; set; }
        public string Summary { get; set; }
        public string ImageUrl { get; set; }

        public BitmapImage ImageSource { get; set; }
    }
}
