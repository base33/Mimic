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
        private static readonly ConcurrentDictionary<string, (TypeAccessor TypeAccessor,IEnumerable<(Member Member, PropertyMapperAttribute Converter)> MemberSet)> TypeDescriptors = 
            new ConcurrentDictionary<string, (TypeAccessor TypeAccessor, IEnumerable<(Member, PropertyMapperAttribute)> MemberSet)>();

        public static T As<T>(this IPublishedContent content) where T : new()
        {
            var type = typeof (T);

            if (content == null)
                return default(T);

            var context = new MapperContext { Content = content };

            T instance = new T();

            string typeName = typeof(T).FullName;

            if (!TypeDescriptors.ContainsKey(typeName))
            {
                var typeAccessor = TypeAccessor.Create(type);
                var memberSet = typeAccessor.GetMembers().Where(p => p.CanWrite);
                var memberToAttributeMap = memberSet.Select(m => (m, ResolveMapper(content, type, m)));
                TypeDescriptors.TryAdd(typeName, (typeAccessor, memberToAttributeMap));
            }

            var typeDescriptor = TypeDescriptors[typeName];

            foreach (var member in typeDescriptor.MemberSet)
            {
                if (member.Converter is Ignore)
                    continue;

                context.Property = member.Member;

                member.Converter.Context = context;

                var value = member.Converter.ProcessValue();

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
