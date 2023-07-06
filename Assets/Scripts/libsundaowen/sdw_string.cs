using System;

using static sdw.cpp;
using static sdw.std;

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

namespace sdw
{
    public static partial class zzz
    {
        public static n8 SToN8(string a_sString, int a_nRadix = 10)
        {
            return (static_cast_n8)(Convert.ToInt32(a_sString, a_nRadix));
        }

        public static n16 SToN16(string a_sString, int a_nRadix = 10)
        {
            return (static_cast_n16)(Convert.ToInt32(a_sString, a_nRadix));
        }

        public static n32 SToN32(string a_sString, int a_nRadix = 10)
        {
            return (static_cast_n32)(Convert.ToInt32(a_sString, a_nRadix));
        }

        public static n64 SToN64(string a_sString, int a_nRadix = 10)
        {
            return Convert.ToInt64(a_sString, a_nRadix);
        }

        public static u8 SToU8(string a_sString, int a_nRadix = 10)
        {
            return (static_cast_u8)(Convert.ToUInt32(a_sString, a_nRadix));
        }

        public static u16 SToU16(string a_sString, int a_nRadix = 10)
        {
            return (static_cast_u16)(Convert.ToUInt32(a_sString, a_nRadix));
        }

        public static u32 SToU32(string a_sString, int a_nRadix = 10)
        {
            return (static_cast_u32)(Convert.ToUInt32(a_sString, a_nRadix));
        }

        public static u64 SToU64(string a_sString, int a_nRadix = 10)
        {
            return Convert.ToUInt64(a_sString, a_nRadix);
        }

        public static f32 SToF32(string a_sString)
        {
            return (static_cast_f32)(Convert.ToDouble(a_sString));
        }

        public static f64 SToF64(string a_sString)
        {
            return Convert.ToDouble(a_sString);
        }

        public static vectorp<string> Split(string a_sString, string a_sSeparator)
        {
            string[] sString = a_sString.Split(a_sSeparator);
            vectorp<string> vString = new vectorp<string>();
            for (n32 i = 0; i < sString.Length; i++)
            {
                vString.push_back(sString[i]);
            }
            return vString;
        }

        public static string Format(string a_sFormat, params object[] a_Args)
        {
            return string.Format(a_sFormat, a_Args);
        }
    }
}
