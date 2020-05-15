using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mimic.Context;
using Mimic.PropertyMapperAttributes;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Persistence.Mappers;

namespace Mimic
{
    public static class IPublishedElementExtensions
    {
        public static T As<T>(this IPublishedElement content)
        {
            var type = typeof(T);

            return (T)As(content, type);
        }

        public static List<T> As<T>(this IEnumerable<IPublishedElement> elements)
        {
            var type = typeof(T);

            return elements.Select(element => (T)As(element, type)).ToList();
        }

        public static object As(this IPublishedElement element, Type type)
        {
            if (element == null)
                return null;

            var context = new MapperContext { Element = element };

            var instance = Activator.CreateInstance(type);

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanWrite))
            {
                context.Property = property;

                var mapper = ResolveMapper(element, type, property);

                if (mapper is Ignore)
                    continue;

                mapper.Context = context;

                var value = mapper.ProcessValue(true);

                property.SetValue(instance, value);
            }

            return instance;
        }

        private static PropertyMapperAttribute ResolveMapper(IPublishedElement element, Type type, PropertyInfo property)
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
