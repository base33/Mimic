using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models.PublishedContent;

namespace Mimic.Factory
{
    public static class MimicAsDynamicCaller
    {
        private static ConcurrentDictionary<Type, Func<IPublishedContent, object>> Precompiled = new ConcurrentDictionary<Type, Func<IPublishedContent, object>>();

        public static Func<IPublishedContent, object> GetAsForType(Type type)
        {
            if (Precompiled.ContainsKey(type))
            {
                return Precompiled[type];
            }

            var source = Expression.Parameter(
                typeof(IPublishedContent), "source");

            var call = Expression.Call(
                typeof(IPublishedContentExtensions), "As", new Type[] { type }, source);

            var compiled = (Func<IPublishedContent, object>)Expression.Lambda(call, source).Compile();

            Precompiled.TryAdd(type, compiled);

            return compiled;
        }
    }
}
