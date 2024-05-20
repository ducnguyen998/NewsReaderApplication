using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace NewsReaderSystem.Models
{
    public class FigureElement : IArticleContentElement
    {
        public EContentType ContentType { get; set; } = EContentType.Figure;

        public BitmapImage Image { get; set; }

        public string Title { get; set; }
    }
}
