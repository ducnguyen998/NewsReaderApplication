using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsReaderSystem.Models
{
    public class ParagraphElement : IArticleContentElement
    {
        public EContentType ContentType { get; set; } = EContentType.Paragraph;

        public string Content { get; set; }
    }
}
