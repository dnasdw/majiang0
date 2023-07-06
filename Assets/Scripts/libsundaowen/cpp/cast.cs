using System;

namespace sdw
{
    public static partial class cpp
    {
        public static T dynamic_cast<T>(object a_Value) where T : class
        {
            return a_Value as T;
        }

        public static T reinterpret_cast<T>(object a_Value)
        {
            return (T)a_Value;
        }

        public static T static_cast_slow<T>(object a_Value)
        {
            return (T)Convert.ChangeType(a_Value, typeof(T));
        }
    }
}
