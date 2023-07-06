using System;
using System.Diagnostics;

using static sdw.cpp;

namespace sdw
{
    public static partial class std
    {
        public static T max<T>(T a_A, T a_B) where T : IComparable<T>
        {
            return a_A.CompareTo(a_B) < 0 ? a_B : a_A;
        }

        public static T min<T>(T a_A, T a_B) where T : IComparable<T>
        {
            return a_B.CompareTo(a_A) < 0 ? a_B : a_A;
        }

        public static T clamp<T>(T a_V, T a_Lo, T a_Hi) where T : IComparable<T>
        {
            Debug.Assert(!(a_Hi.CompareTo(a_Lo) < 0));
            if (a_Hi.CompareTo(a_V) < 0)
            {
                return a_Hi;
            }
            if (a_V.CompareTo(a_Lo) < 0)
            {
                return a_Lo;
            }
            return a_V;
        }

        public static partial class ranges
        {
            public static vectorp<T> reverse<T>(vectorp<T> a_R)
            {
                a_R.Reverse();
                return a_R;
            }
        }
    }
}
