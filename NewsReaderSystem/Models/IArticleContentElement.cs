using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsReaderSystem.Models
{
    public interface IArticleContentElement
    {
        EContentType ContentType { get; }
    }
}
