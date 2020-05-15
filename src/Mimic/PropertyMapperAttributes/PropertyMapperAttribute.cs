using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mimic.Context;
using Mimic.Helpers;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
namespace Mimic.PropertyMapperAttributes
{
    public abstract class PropertyMapperAttribute : Attribute
    {
        public MapperContext Context { get; set; }
        public IPublishedContent Content => Context.Content;
        public IPublishedElement Element => Context.Element;

        protected IPublishedProperty GetClosestProperty(string propertyName = "")
        {
            propertyName = !string.IsNullOrEmpty(propertyName) ? propertyName : Context.Property.Name;

            return new UmbracoPropertyLocator().GetClosestProperty(Content, propertyName);
        }

        protected IPublishedProperty GetClosestElement(string propertyName = "")
        {
            propertyName = !string.IsNullOrEmpty(propertyName) ? propertyName : Context.Property.Name;

            return new UmbracoPropertyLocator().GetClosestProperty(Element, propertyName);
        }

        public abstract object ProcessValue(bool isElement = false);
    }
}
