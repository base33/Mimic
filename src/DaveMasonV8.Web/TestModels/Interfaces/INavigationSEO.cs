using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaveMasonV8.Web.TestModels.Interfaces
{
    public interface INavigationSEO
    {
        string SeoMetaDescription { get; set; }
        bool UmbracoNavihide { get; set; }
        IEnumerable<string> Keywords { get; set; }
    }
}
