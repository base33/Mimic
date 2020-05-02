using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mimic.Context;
using Mimic.Factory;
using Mimic.PropertyMapperAttributes;
using Umbraco.Core.Models.PublishedContent;

namespace Mimic
{
    public static class IPublishedContentExtensions
    {

        public static T As<T>(this IPublishedContent content)
        {
            var type = typeof (T);

            if (content == null)
                return default(T);

            var context = new MapperContext { Content = content };

            T instance = MsilBuilderWithCachingWithGeneric<T>.Build();

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

        public static List<T> As<T>(this IEnumerable<IPublishedContent> contents)
        {
            var type = typeof(T);

            return contents.Select(content => As<T>(content)).ToList();
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
