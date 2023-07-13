using System;

using static sdw.cpp;
using static sdw.std;
using static sdw.zzz;

// alias
// typedef begin
using n8 = System.SByte;
using n16 = System.Int16;
using n32 = System.Int32;
using n64 = System.Int64;
using u8 = System.Byte;
using u16 = System.UInt16;
using u32 = System.UInt32;
using u64 = System.UInt64;

using f32 = System.Single;
using f64 = System.Double;

using static_cast_n8 = System.SByte;
using static_cast_n16 = System.Int16;
using static_cast_n32 = System.Int32;
using static_cast_n64 = System.Int64;
using static_cast_u8 = System.Byte;
using static_cast_u16 = System.UInt16;
using static_cast_u32 = System.UInt32;
using static_cast_u64 = System.UInt64;

using static_cast_f32 = System.Single;
using static_cast_f64 = System.Double;
// typedef end

namespace sdw.game
{
    public class CJaggedArray
    {
        public static T CreateInstance<T>(params n32[] a_nLengths)
        {
            Type elementType = typeof(T).GetElementType();
            if (elementType == null)
            {
                throw new ArgumentException("T must be an array type.");
            }
            if (a_nLengths.Length < 1)
            {
                throw new ArgumentException("Jagged array must have at least one dimension.");
            }
            return reinterpret_cast<T>(initializeJaggedArray(elementType, a_nLengths, 0));
        }

        public static T CreateInstance<T, V>(params n32[] a_nLengths)
        {
            Type elementType = typeof(T).GetElementType();
            if (elementType == null)
            {
                throw new ArgumentException("T must be an array type.");
            }
            if (a_nLengths.Length < 1)
            {
                throw new ArgumentException("Jagged array must have at least one dimension.");
            }
            return reinterpret_cast<T>(initializeJaggedArray<V>(elementType, a_nLengths, 0));
        }

        public static void Copy<T>(T[] a_SourceArray, T[] a_DestinationArray, n32 a_nLength) where T : ICloneable
        {
            if (a_SourceArray == null)
            {
                throw new ArgumentNullException("a_SourceArray");
            }
            if (a_DestinationArray == null)
            {
                throw new ArgumentNullException("a_DestinationArray");
            }
            if (a_SourceArray.Length < a_nLength)
            {
                throw new ArgumentException("a_SourceArray is too small.");
            }
            if (a_DestinationArray.Length < a_nLength)
            {
                throw new ArgumentException("a_DestinationArray is too small.");
            }
            for (n32 i = 0; i < a_nLength; i++)
            {
                a_DestinationArray[i] = reinterpret_cast<T>(a_SourceArray[i].Clone());
            }
        }

        public static void Clear(Array a_Array)
        {
            recursiveClear(a_Array);
        }

        public static void Clear<V>(Array a_Array)
        {
            recursiveClear<V>(a_Array);
        }

        private static object initializeJaggedArray(Type a_Type, n32[] a_nLengths, n32 a_nLengthIndex)
        {
            n32 nLength = a_nLengths[a_nLengthIndex];
            Array array = Array.CreateInstance(a_Type, nLength);
            Type elementType = a_Type.GetElementType();
            if (elementType == null || a_nLengthIndex == a_nLengths.Length - 1)
            {
                // do nothing
            }
            else
            {
                for (n32 i = 0; i < nLength; i++)
                {
                    array.SetValue(initializeJaggedArray(elementType, a_nLengths, a_nLengthIndex + 1), i);
                }
            }
            return array;
        }

        private static object initializeJaggedArray<V>(Type a_Type, n32[] a_nLengths, n32 a_nLengthIndex)
        {
            n32 nLength = a_nLengths[a_nLengthIndex];
            Array array = Array.CreateInstance(a_Type, nLength);
            Type elementType = a_Type.GetElementType();
            if (elementType == null)
            {
                if (typeof(V) != a_Type)
                {
                    throw new ArgumentException("V must be an element type.");
                }
                CDefault defaultInstance = CDefault.Instance;
                for (n32 i = 0; i < nLength; i++)
                {
                    array.SetValue(defaultInstance.GetDefaultValue<V>(), i);
                }
            }
            else if (a_nLengthIndex == a_nLengths.Length - 1)
            {
                // do nothing
            }
            else
            {
                for (n32 i = 0; i < nLength; i++)
                {
                    array.SetValue(initializeJaggedArray<V>(elementType, a_nLengths, a_nLengthIndex + 1), i);
                }
            }
            return array;
        }

        public static void recursiveClear(Array a_Array)
        {
            if (a_Array == null)
            {
                return;
            }
            n32 nLength = a_Array.Length;
            Type elementType = a_Array.GetType().GetElementType();
            Type subElementType = elementType.GetElementType();
            if (subElementType == null)
            {
                Array.Clear(a_Array, 0, nLength);
            }
            else
            {
                for (n32 i = 0; i < nLength; i++)
                {
                    recursiveClear(dynamic_cast<Array>(a_Array.GetValue(i)));
                }
            }
        }

        public static void recursiveClear<V>(Array a_Array)
        {
            if (a_Array == null)
            {
                return;
            }
            n32 nLength = a_Array.Length;
            Type elementType = a_Array.GetType().GetElementType();
            Type subElementType = elementType.GetElementType();
            if (subElementType == null)
            {
                if (typeof(V) != elementType)
                {
                    throw new ArgumentException("V must be an element type.");
                }
                CDefault defaultInstance = CDefault.Instance;
                for (n32 i = 0; i < nLength; i++)
                {
                    a_Array.SetValue(defaultInstance.GetDefaultValue<V>(), i);
                }
            }
            else
            {
                for (n32 i = 0; i < nLength; i++)
                {
                    recursiveClear<V>(dynamic_cast<Array>(a_Array.GetValue(i)));
                }
            }
        }

        private static void getDimensionLength(Array a_Array, vectorp<n32> a_vLength)
        {
            Array array = a_Array;
            while (array != null)
            {
                a_vLength.push_back(array.GetLength(0));
                array = dynamic_cast<Array>(array.GetValue(0));
            }
        }
    }
}
