using System;
using System.Reflection;

namespace BadAppleSkulls.Util
{
    public static class ReflectionUtils
    {
        private static readonly BindingFlags BindingFlagsFields =
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        
        public static object GetPrivate<T>(T instance, Type classType, string field)
        {
            FieldInfo privateField = classType.GetField(field, BindingFlagsFields);
            return privateField.GetValue(instance);
        }
    }
}