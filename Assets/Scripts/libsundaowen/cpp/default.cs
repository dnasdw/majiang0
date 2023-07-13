using System;

namespace sdw
{
    public static partial class cpp
    {
        public static class CSpecializer<T>
        {
            public static bool Default = false;

            public static T GetDefaultValue()
            {
                if (Default)
                {
                    return default(T);
                }
                else
                {
                    Type type = typeof(T);
                    if (type.IsArray)
                    {
                        return default(T);
                    }
                    return reinterpret_cast<T>(Activator.CreateInstance(type));
                }
            }
        }

        public class CDefault
        {
            static CDefault()
            {
                CSpecializer<bool>.Default = true;
                CSpecializer<char>.Default = true;
                CSpecializer<sbyte>.Default = true;
                CSpecializer<short>.Default = true;
                CSpecializer<int>.Default = true;
                CSpecializer<long>.Default = true;
                CSpecializer<byte>.Default = true;
                CSpecializer<ushort>.Default = true;
                CSpecializer<uint>.Default = true;
                CSpecializer<ulong>.Default = true;
                CSpecializer<float>.Default = true;
                CSpecializer<double>.Default = true;
                CSpecializer<decimal>.Default = true;
                CSpecializer<string>.Default = true;
            }

            public static CDefault Instance { get; } = CSpecializer<CDefault>.GetDefaultValue();

            public T GetDefaultValue<T>()
            {
                return CSpecializer<T>.GetDefaultValue();
            }
        }
    }
}
