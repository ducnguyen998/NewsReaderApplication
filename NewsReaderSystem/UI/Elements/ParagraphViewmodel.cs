using CommunityToolkit.Mvvm.ComponentModel;
using NewsReaderSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace NewsReaderSystem.UI.Elements
{
    public class ParagraphViewmodel : ObservableObject
    {
        public ParagraphViewmodel(IArticleContentElement element)
        {
            Element = element;
        }

        public IArticleContentElement Element { get; set; }
    }
}
