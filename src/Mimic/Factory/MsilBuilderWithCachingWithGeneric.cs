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
        private static Func<T> func;

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
    }
}
