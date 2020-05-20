using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using FastMember;
using Mimic.Context;
using Mimic.Factory;
using Mimic.PropertyMapperAttributes;
using NPoco.Expressions;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;

namespace Mimic
{
    public static class IPublishedElementExtensions
    {
        private static readonly ConcurrentDictionary<string, (TypeAccessor TypeAccessor,IEnumerable<(Member Member, PropertyMapperAttribute Converter)> MemberSet)> TypeDescriptors = 
            new ConcurrentDictionary<string, (TypeAccessor TypeAccessor, IEnumerable<(Member, PropertyMapperAttribute)> MemberSet)>();

        private static readonly ConcurrentDictionary<string, Func<object>> Factories = new ConcurrentDictionary<string, Func<object>>();

        public static T As<T>(this IPublishedElement content) where T : class, new()
        {
            var type = typeof(T);

            if (content == null)
                return default(T);

            var context = new MapperContext { Content = content };

            if(!Factories.ContainsKey(type.FullName))
            {
                var constructor = type.GetConstructors().FirstOrDefault(c =>
                {
                    var param = c.GetParameters();
                    if (param.Length == 1 && param[0].ParameterType == typeof(IPublishedContent))
                        return true;

                    return false;
                });

                Func<T> factory = constructor != null ?
                    new Func<T>(() => (T)constructor.Invoke(new[] { (IPublishedContent)content })) :
                    new Func<T>(() => new T());

                Factories.TryAdd(type.FullName, factory);
            }

            var make = Factories[type.FullName];

            return As(content, make as Func<T>);
        }

        public static T As<T>(this IPublishedElement content, Func<T> createNewInstance) where T : class, new()
        {
            var type = typeof(T);

            if (content == null)
                return default(T);

            var context = new MapperContext { Content = content };

            T instance = createNewInstance();

            string typeName = type.FullName;

            if (!TypeDescriptors.ContainsKey(typeName))
            {
                var typeAccessor = TypeAccessor.Create(type);
                var memberSet = typeAccessor.GetMembers().Where(p => p.CanWrite);
                var memberToAttributeMap = memberSet.Select(m => (m, ResolveMapper(m)));
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

        public static List<T> As<T>(this IEnumerable<IPublishedContent> contents) where T : class, new()
        {
            var type = typeof(T);

            return contents.Select(content => As<T>(content)).ToList();
        }
        
        private static PropertyMapperAttribute ResolveMapper(Member property)
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
