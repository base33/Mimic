using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapper.Context;
using Umbraco.Core.Models;

namespace Mapper.PropertyMapperAttributes
{
    public abstract class PropertyMapperAttribute : Attribute
    {
        public MapperContext Context { get; set; }
        public IPublishedContent Content => Context.Content;

        protected IPublishedProperty GetClosestProperty(string propertyName = "")
        {
            propertyName = !string.IsNullOrEmpty(propertyName) ? propertyName : Context.Property.Name;

            return Content.Properties.FirstOrDefault(property => property.PropertyTypeAlias.ToLower() == propertyName.ToLower());
        }

        public abstract object ProcessValue();
    }
}
