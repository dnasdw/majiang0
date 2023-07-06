using UnityEngine;

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

namespace sdw.unity
{
    public static class CTransformExtension
    {
        public static void DestroyAllChildren(this Transform a_Transform)
        {
            if (a_Transform == null)
            {
                return;
            }
            n32 nChildCount = a_Transform.childCount;
            for (n32 i = 0; i < nChildCount; i++)
            {
                Object.Destroy(a_Transform.GetChild(i).gameObject);
            }
        }
    }
}
