using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DaveMason.Helpers;
using DaveMason.PropertyMapperAttributes;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace DaveMason
{
    public class PropertyMapperResolver
    {
        public PropertyMapperAttribute ResolveMapper(IPublishedContent content, Type type, PropertyInfo property)
        {
            //try and resolve whether the class property has an umbraco property
            var umbracoPropertyLocator = new UmbracoPropertyLocator();
            var umbracoProperty = umbracoPropertyLocator.GetClosestProperty(content, property.Name);
            if (umbracoProperty == null)
            {
                //if not, try and resolve the sub class with the current item
                return new Self();
            }

            //resolve by attribute
            var attribute = property.GetCustomAttributes().FirstOrDefault(a => a is PropertyMapperAttribute);
            if (attribute != null)
            {
                return (PropertyMapperAttribute)attribute;
            }

            return new SimpleType();
        }
    }
}
