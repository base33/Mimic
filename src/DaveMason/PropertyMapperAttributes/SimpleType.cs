using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace DaveMason.PropertyMapperAttributes
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

            var propertyValue = property.Value;

            if (propertyValue is Udi[])
            {
                propertyValue = ((Udi[]) propertyValue).Select(c => new UmbracoHelper(UmbracoContext.Current).TypedContent(c)).ToList();
            }

            //if the property value is an IPublishedContent array and needs mapping to a custom List
            if (propertyValue is IEnumerable<IPublishedContent> && (Context.Property.PropertyType.GenericTypeArguments.Length > 0 && Context.Property.PropertyType.GenericTypeArguments[0] != typeof(IPublishedContent)))
            {
                var propertyValueEnumerable = (IEnumerable<IPublishedContent>)propertyValue;
                var value = (IList)Activator.CreateInstance(Context.Property.PropertyType);
                foreach (IPublishedContent item in propertyValueEnumerable)
                {
                    value.Add(item.As(Context.Property.PropertyType.GenericTypeArguments[0]));
                }
                return value;
            }
            else if (propertyValue is IEnumerable<IPublishedContent> && Context.Property.PropertyType == typeof(IPublishedContent))
            {
                return ((IEnumerable<IPublishedContent>)propertyValue).FirstOrDefault();
            }
            else if (propertyValue is HtmlString)
            {
                return propertyValue.ToString();
            }

            return propertyValue;
        }
    }
}
