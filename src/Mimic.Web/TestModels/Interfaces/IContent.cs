using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models.PublishedContent;

namespace Mimic.Web.TestModels.Interfaces
{
    public interface IContent
    {
        string PageTitle { get; set; }
        string BodyText { get; set; }
        bool Checkbox { get; set; }
        IPublishedElement ContentPicker { get; set; }
        DateTime DateAndTime { get; set; }
        object Maps { get; set; }
        IEnumerable<IPublishedContent> MultinodeTree { get; set; }
        decimal NumberDec { get; set; }
    }
}
