using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models.PublishedContent;

namespace Mimic.Factory
{
    public static class MimicAsDynamicCaller
    {
        private static ConcurrentDictionary<Type, Func<IPublishedElement, object>> Precompiled = new ConcurrentDictionary<Type, Func<IPublishedElement, object>>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Func<IPublishedElement, object> GetAsForType(Type type)
        {
            if (Precompiled.ContainsKey(type))
            {
                return Precompiled[type];
            }

            var source = Expression.Parameter(
                typeof(IPublishedElement), "source");

            var call = Expression.Call(
                typeof(IPublishedElementExtensions), "As", new Type[] { type }, source);

            var compiled = (Func<IPublishedElement, object>)Expression.Lambda(call, source).Compile();

            Precompiled.TryAdd(type, compiled);

            return compiled;
        }
    }
}
