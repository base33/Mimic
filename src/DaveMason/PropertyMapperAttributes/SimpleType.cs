using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace Mapper.PropertyMapperAttributes
{
    public class SimpleType : PropertyMapperAttribute
    {
        public override object ProcessValue()
        {
            var value = ResolveStictProperty();

            return value ?? ResolveUmbracoProperty();
        }

        protected object ResolveStictProperty()
        {
            switch (Context.Property.Name)
            {
                case "Id":
                    return Content.Id;
                case "Name":
                    return Content.Name;
                case "Url":
                    return Content.Url;
            }

            return null;
        }

        protected object ResolveUmbracoProperty()
        {
            var property = GetClosestProperty();

            if (property == null)
                return null;

            //if the property value is an IPublishedContent array and needs mapping to a custom List
            if (property.Value is IEnumerable<IPublishedContent> && Context.Property.PropertyType.GenericTypeArguments[0] != typeof(IPublishedContent))
            {
                var propertyValueEnumerable = (IEnumerable<IPublishedContent>)property.Value;
                var value = (IList)Activator.CreateInstance(Context.Property.PropertyType);
                foreach (IPublishedContent item in propertyValueEnumerable)
                {
                    value.Add(item.As(Context.Property.PropertyType.GenericTypeArguments[0]));
                }
                return value;
            }
            else
            {
                return property.Value;
            }

            return null;
        }
    }
}
