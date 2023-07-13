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
        /// <summary>
        /// Enumerating k-combinations
        /// <para>Comb(5, 3):</para>
        /// <para>[[0, 1, 2], [0, 1, 3], [0, 1, 4], [0, 2, 3], [0, 2, 4], [0, 3, 4], [1, 2, 3], [1, 2, 4], [1, 3, 4], [2, 3, 4]]</para>
        /// </summary>
        /// <param name="a_nN"></param>
        /// <param name="a_nK"></param>
        /// <returns></returns>
        public static vectorp<vectorp<int>> Comb(int a_nN, int a_nK)
        {
            vectorp<vectorp<n32>> vComb = new vectorp<vectorp<n32>>();
            if (a_nK <= a_nN && a_nK >= 0)
            {
                vectorp<n32> vPrevComb = new vectorp<n32>();
                // [0, 1, 2]
                for (n32 i = 0; i < a_nK; i++)
                {
                    vPrevComb.push_back(i);
                }
                vComb.push_back(vPrevComb);
                // 2
                n32 nIndex = a_nK - 1;
                bool bInitRight = false;
                while (a_nK != a_nN && nIndex >= 0)
                {
                    // [2, 3, 4] <- [5 - 3 + 0, 5 - 3 + 1, 5 - 3 + 2]
                    // 2 < 4 | 3 < 4 | !(4 < 4) | 1 < 3 | 3 < 4 | !(4 < 4) | 2 < 3
                    if (vPrevComb[nIndex] < a_nN - a_nK + nIndex)
                    {
                        vectorp<n32> vCurrentComb = new vectorp<n32>(vPrevComb);
                        // [0, 1, 3] | [0, 1, 4] | [0, 2, 4] -> [0, 2, 3] | [0, 2, 4] | [0, 3, 4]
                        vCurrentComb[nIndex]++;
                        if (bInitRight)
                        {
                            // [2, 3, 4] <- [5 - 3 + 0, 5 - 3 + 1, 5 - 3 + 2]
                            // 2 < 3 | !(3 < 3)
                            if (vCurrentComb[nIndex] < a_nN - a_nK + nIndex)
                            {
                                // [0, 2, 4] -> [0, 2, 3]
                                for (n32 i = nIndex + 1; i < a_nK; i++)
                                {
                                    vCurrentComb[i] = vCurrentComb[i - 1] + 1;
                                }
                                // 2
                                nIndex = a_nK - 1;
                            }
                            bInitRight = false;
                        }
                        vComb.push_back(vCurrentComb);
                        vPrevComb = vCurrentComb;
                    }
                    else
                    {
                        // 1
                        nIndex--;
                        bInitRight = true;
                    }
                }
            }
            return vComb;
        }
    }
}
