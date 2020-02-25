using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.Composing;

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

            var propertyValue = property.GetValue();

            if (propertyValue is Udi[])
            {
                propertyValue = ((Udi[])propertyValue).Select(c => Current.UmbracoHelper.Content(c)).ToList();
            }

            if (propertyValue is JObject && Context.Property.PropertyType != typeof(JObject))
            {
                if (Context.Property.PropertyType == typeof(string))
                {
                    propertyValue = propertyValue.ToString();
                }
                else if (Context.Property.PropertyType.IsClass)
                {
                    propertyValue = JsonConvert.DeserializeObject(propertyValue.ToString(), Context.Property.PropertyType);
                }
            }

            //if the property value is an IPublishedContent array and needs mapping to a custom List
            if (propertyValue is IEnumerable<IPublishedContent> && (Context.Property.PropertyType.GenericTypeArguments.Length > 0 && Context.Property.PropertyType.GenericTypeArguments[0] != typeof(IPublishedContent)))
            {
                var propertyValueEnumerable = (IEnumerable<IPublishedContent>)propertyValue;
                //var value = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(Context.Property.PropertyType));

                var value = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(Context.Property.PropertyType.GenericTypeArguments[0]));

                var listType = typeof(List<>).MakeGenericType(Context.Property.PropertyType);
                var list = (IList)Activator.CreateInstance(listType);


                foreach (IPublishedContent item in propertyValueEnumerable)
                {
                    var arg = Context.Property.PropertyType.GenericTypeArguments[0];

                    var typedItem = item.As(arg);

                    value.Add(typedItem);

                    //value.Add(item.As(Context.Property.PropertyType.GenericTypeArguments[0]));
                }
                return value;
            }
            //else if the property value is an array but the class property is a single item
            else if (propertyValue is IEnumerable<IPublishedContent> && Context.Property.PropertyType == typeof(IPublishedContent))
            {
                return ((IEnumerable<IPublishedContent>)propertyValue).FirstOrDefault();
            }
            //if the property value is a HtmlString, convert to string
            else if (propertyValue is HtmlString)
            {
                return propertyValue.ToString();
            }

            if (propertyValue != null)
            {
                if (propertyValue.GetType() != Context.Property.PropertyType && !propertyValue.GetType().Inherits(Context.Property.PropertyType))
                {
                    var type = propertyValue.GetType();
                    if (type == typeof(GuidUdi))
                    {
                        Current.Logger.Warn(this.GetType(), "Trying to map unpublished content. Id: " + Context.Content.Id + ". Property: " + Context.Property.Name);
                    }
                    else
                    {
                        Current.Logger.Warn(this.GetType(), "Trying to map to wrong type. Id: " + Context.Content.Id + ". Property: " + Context.Property.Name + ". Wrong Type: " + Context.Property.PropertyType);
                    }

                    return Context.Property.PropertyType.GetType().GetDefaultValue();
                }
            }
            //otherwise return the untouched value
            return propertyValue;
        }
    }
}

