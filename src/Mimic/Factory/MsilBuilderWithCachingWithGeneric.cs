using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Mimic.Factory
{
    public static class MsilBuilderWithCachingWithGeneric<T>
    {
        private static Type t = typeof(T);
        private static Type list = typeof(List<T>);

        private static Func<T> func;

        private static Func<List<T>> listFunc;

        public static T Build()
        {
            if (func != null)
            {
                return func();
            }

            var dynamicMethod = new DynamicMethod("CreateInstance", t, Type.EmptyTypes, true);
            var ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Nop);
            ConstructorInfo emptyConstructor = t.GetConstructor(Type.EmptyTypes);
            ilGenerator.Emit(OpCodes.Newobj, emptyConstructor);
            ilGenerator.Emit(OpCodes.Ret);

            func = (Func<T>)dynamicMethod.CreateDelegate(typeof(Func<T>));
            return func();
        }

        public static List<T> BuildList()
        {
            if (func != null)
            {
                return listFunc();
            }

            var dynamicMethod = new DynamicMethod("CreateInstance", list, Type.EmptyTypes, true);
            var ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Nop);
            ConstructorInfo emptyConstructor = t.GetConstructor(Type.EmptyTypes);
            ilGenerator.Emit(OpCodes.Newobj, emptyConstructor);
            ilGenerator.Emit(OpCodes.Ret);

            listFunc = (Func<List<T>>)dynamicMethod.CreateDelegate(typeof(Func<List<T>>));
            return listFunc();
        }
    }
}
