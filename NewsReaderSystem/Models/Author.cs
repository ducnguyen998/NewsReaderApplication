using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace NewsReaderSystem.Models
{
    public class Author
    {
        public string Name { get; set; } = "Author";
        public string PostedTime { get; set; } = DateTime.Now.ToString();
        public BitmapImage Avatar { get; set; }
    }
}
