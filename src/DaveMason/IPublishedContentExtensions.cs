using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DaveMason.Context;
using DaveMason.PropertyMapperAttributes;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Persistence.Mappers;
using Umbraco.Web;

namespace DaveMason
{
    public static class IPublishedContentExtensions
    {
        public static T As<T>(this IPublishedContent content)
        {
            var type = typeof (T);

            return (T)As(content, type);
        }

        public static List<T> As<T>(this IEnumerable<IPublishedContent> contents)
        {
            var type = typeof(T);

            return contents.Select(content => (T) As(content, type)).ToList();
        }

        public static object As(this IPublishedContent content, Type type)
        {
            if (content == null)
                return null;

            var context = new MapperContext {Content = content};

            var instance = Activator.CreateInstance(type);

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanWrite))
            {
                context.Property = property;

                var mapper = ResolveMapper(content, type, property);

                if (mapper is Ignore)
                    continue;

                mapper.Context = context;

                var value = mapper.ProcessValue();

                property.SetValue(instance, value);
            }

            return instance;
        }

        private static PropertyMapperAttribute ResolveMapper(IPublishedContent content, Type type, PropertyInfo property)
        {
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
