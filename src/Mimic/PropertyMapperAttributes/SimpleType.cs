using Mimic.Factory;
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

namespace Mimic.PropertyMapperAttributes
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
            var content = Content as IPublishedContent;

            if (content != null)
            {
                switch (Context.Property.Name)
                {
                    case "Id":
                        return content.Id;
                    case "Name":
                        return content.Name;
                    case "Url":
                        return content.Url;
                }
            }

            return null;
        }

        protected object ResolveUmbracoProperty()
        {
            var property = GetClosestProperty();

            if (property == null || property.Value() == null)
                return Context.Property.Type.GetDefaultValue();// .GetMethod("GetDefaultGeneric").MakeGenericMethod(t).Invoke(this, null);

            var propertyValue = property.GetValue();

            if(Context.Property.Type == typeof(string))
            {
                return propertyValue.ToString();
            }
            if (propertyValue.GetType() == Context.Property.Type || propertyValue.GetType().Inherits(Context.Property.Type))
            {
                return propertyValue;
            }
            else if (propertyValue is Udi[])
            {
                propertyValue = ((Udi[])propertyValue).Select(c => Current.UmbracoHelper.Content(c)).ToList();
            }
            else if (propertyValue is JObject && Context.Property.Type != typeof(JObject))
            {
                if (Context.Property.Type == typeof(string))
                {
                    propertyValue = propertyValue.ToString();
                }
                else if (Context.Property.Type.IsClass)
                {
                    propertyValue = JsonConvert.DeserializeObject(propertyValue.ToString(), Context.Property.Type);
                }
            }
            //if the property value is an IPublishedContent array and needs mapping to a custom List
            else if (((propertyValue as IEnumerable<IPublishedElement>) != null) 
                && Context.Property.Type.GenericTypeArguments.Length > 0 
                && (Context.Property.Type.GenericTypeArguments[0] != typeof(IPublishedElement)
                && Context.Property.Type.GenericTypeArguments[0] != typeof(IPublishedContent)))
            {
                var propertyValueEnumerable = (IEnumerable<IPublishedElement>)propertyValue;

                var list = (IList)MsilBuilderWithCachingWithGeneric.BuildList(Context.Property.Type.GenericTypeArguments[0]);

                foreach (IPublishedElement item in propertyValueEnumerable)
                {
                    var arg = Context.Property.Type.GenericTypeArguments[0];

                    var typedItem = MimicAsDynamicCaller.GetAsForType(arg)(item);

                    list.Add(typedItem);
                }
                return list;
            }
            else if ((propertyValue.GetType() == typeof(IPublishedElement) || propertyValue.GetType().Inherits(typeof(IPublishedElement))) &&
                (Context.Property.Type != typeof(IPublishedElement)) && !Context.Property.Type.Inherits(typeof(IPublishedElement)))
            {
                return MimicAsDynamicCaller.GetAsForType(Context.Property.Type)((IPublishedElement)propertyValue);
            }
            //else if the property value is an array but the class property is a single item
            else if (propertyValue is IEnumerable<IPublishedContent> && Context.Property.Type == typeof(IPublishedContent))
            {
                return ((IEnumerable<IPublishedContent>)propertyValue).FirstOrDefault();
            }
            else if (propertyValue is IEnumerable<IPublishedElement> && Context.Property.Type == typeof(IPublishedElement))
            {
                return ((IEnumerable<IPublishedElement>)propertyValue).FirstOrDefault();
            }
            //if the property value is a HtmlString, convert to string
            else if (propertyValue is HtmlString)
            {
                return propertyValue.ToString();
            }

            if (propertyValue != null)
            {
                if (propertyValue.GetType() != Context.Property.Type && !propertyValue.GetType().Inherits(Context.Property.Type))
                {
                    var type = propertyValue.GetType();
                    var content = Content as IPublishedContent;
                    if (type == typeof(GuidUdi))
                    {
                        Current.Logger.Warn(this.GetType(), "Trying to map unpublished content. Id: " + (content?.Id ?? 0) + ". Property: " + Context.Property.Name);
                    }
                    else
                    {
                        Current.Logger.Warn(this.GetType(), "Trying to map to wrong type. Id: " + (content?.Id ?? 0) + ". Property: " + Context.Property.Name + ". Wrong Type: " + Context.Property.Type);
                    }

                    return Context.Property.Type.GetType().GetDefaultValue();
                }
            }
            //otherwise return the untouched value
            return propertyValue;
        }
    }
}

