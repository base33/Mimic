using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Mimic.Factory
{
    public static class MsilBuilderWithCachingWithGeneric
    {
        private static ConcurrentDictionary<Type, Func<object>> PrecompiledSingle = new ConcurrentDictionary<Type, Func<object>>();
        private static ConcurrentDictionary<Type, Func<IList>> PrecompiledList = new ConcurrentDictionary<Type, Func<IList>>();

        public static object Build(Type type)
        {
            if (PrecompiledSingle.ContainsKey(type))
            {
                return PrecompiledSingle[type]();
            }

            var dynamicMethod = new DynamicMethod("CreateInstance", type, Type.EmptyTypes, true);
            var ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Nop);
            ConstructorInfo emptyConstructor = type.GetConstructor(Type.EmptyTypes);
            ilGenerator.Emit(OpCodes.Newobj, emptyConstructor);
            ilGenerator.Emit(OpCodes.Ret);

            var func = (Func<object>)dynamicMethod.CreateDelegate(typeof(Func<object>));

            PrecompiledSingle.TryAdd(type, func);

            return func();
        }

        public static object BuildList(Type type)
        {
            if (PrecompiledList.ContainsKey(type))
            {
                return PrecompiledList[type]();
            }

            var listType = typeof(List<>).MakeGenericType(type);
            var dynamicMethod = new DynamicMethod("CreateInstance", listType, Type.EmptyTypes, true);
            var ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Nop);
            ConstructorInfo emptyConstructor = listType.GetConstructor(Type.EmptyTypes);
            ilGenerator.Emit(OpCodes.Newobj, emptyConstructor);
            ilGenerator.Emit(OpCodes.Ret);

            var func = (Func<IList>)dynamicMethod.CreateDelegate(typeof(Func<>).MakeGenericType(listType));

            PrecompiledList.TryAdd(type, func);

            return func();
        }
    }
}
