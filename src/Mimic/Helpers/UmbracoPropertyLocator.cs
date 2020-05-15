using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace Mimic.Helpers
{
    internal class UmbracoPropertyLocator
    {
        public IPublishedProperty GetClosestProperty(IPublishedContent content, string propertyName = "")
        {
            return content.Properties.FirstOrDefault(property => property.Alias.ToLower() == propertyName.ToLower());
        }

        public IPublishedProperty GetClosestProperty(IPublishedElement element, string propertyName = "")
        {
            return element.Properties.FirstOrDefault(property => property.Alias.ToLower() == propertyName.ToLower());
        }
    }
}
