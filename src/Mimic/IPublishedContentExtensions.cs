using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FastMember;
using Mimic.Context;
using Mimic.Factory;
using Mimic.PropertyMapperAttributes;
using Umbraco.Core.Models.PublishedContent;

namespace Mimic
{
    public static class IPublishedContentExtensions
    {
        private static readonly ConcurrentDictionary<string, (TypeAccessor TypeAccessor,IEnumerable<Member> MemberSet)> TypeDescriptors = new ConcurrentDictionary<string, (TypeAccessor TypeAccessor, IEnumerable<Member> MemberSet)>();

        public static T As<T>(this IPublishedContent content) where T : new()
        {
            var type = typeof (T);

            if (content == null)
                return default(T);

            var context = new MapperContext { Content = content };

            T instance = new T();

            string typeName = nameof(Type);

            if (!TypeDescriptors.ContainsKey(typeName))
            {
                var typeAccessor = TypeAccessor.Create(type);
                var memberSet = typeAccessor.GetMembers().Where(p => p.CanWrite);
                TypeDescriptors.TryAdd(typeName, (typeAccessor, memberSet));
            }

            var typeDescriptor = TypeDescriptors[typeName];

            foreach (var property in typeDescriptor.MemberSet)
            {
                context.Property = property;

                var mapper = ResolveMapper(content, type, property);

                if (mapper is Ignore)
                    continue;

                mapper.Context = context;

                var value = mapper.ProcessValue();

                typeDescriptor.TypeAccessor[instance, context.Property.Name] = value;
            }

            return instance;
        }

        public static List<T> As<T>(this IEnumerable<IPublishedContent> contents) where T : new()
        {
            var type = typeof(T);

            return contents.Select(content => As<T>(content)).ToList();
        }

        private static PropertyMapperAttribute ResolveMapper(IPublishedContent content, Type type, Member property)
        {
            //resolve by attribute
            var attribute = property.GetAttribute(typeof(PropertyMapperAttribute), true);
            if (attribute != null)
            {
                return (PropertyMapperAttribute)attribute;
            }

            return new SimpleType();
        }
    }
}
