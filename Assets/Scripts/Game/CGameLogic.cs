using System;
using System.Diagnostics;
using sdw.game;

using static majiang0.CGameCmd;
using static majiang0.CGameLogicConst;

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

using CAnalyseItemArray = sdw.std.vectorp<majiang0.CGameLogicConst.CAnalyseItem>;

namespace majiang0
{
    public class CGameLogic
    {
        private static readonly u8[] s_uMJCardDataArray = new u8[MAX_REPERTORY]
        {
            0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
            0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
            0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
            0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
            0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19,
            0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19,
            0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19,
            0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19,
            0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29,
            0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29,
            0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29,
            0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29,
            0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37,
            0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37,
            0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37,
            0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37
        };

        public static void Shuffle(u8[] a_uCardData)
        {
            n32 nMaxCount = a_uCardData.Length;
            Debug.Assert(nMaxCount == s_uMJCardDataArray.Length);
            Random random = new Random();
            u8[] uCardDataTemp = new u8[s_uMJCardDataArray.Length];
            Array.Copy(s_uMJCardDataArray, uCardDataTemp, s_uMJCardDataArray.Length);
            n32 nRandomCount = 0;
            do
            {
                n32 nPosition = random.Next() % (nMaxCount - nRandomCount);
                a_uCardData[nRandomCount++] = uCardDataTemp[nPosition];
                uCardDataTemp[nPosition] = uCardDataTemp[nMaxCount - nRandomCount];
            } while (nRandomCount < nMaxCount);
        }

        public static bool RemoveCard(u8[] a_uCardIndex, u8 a_uRemoveCard)
        {
            Debug.Assert(a_uCardIndex.Length == MAX_INDEX);
            n32 nRemoveIndex = SwitchToCardIndex(a_uRemoveCard);
            if (a_uCardIndex[nRemoveIndex] > 0)
            {
                a_uCardIndex[nRemoveIndex]--;
                return true;
            }
            return false;
        }

        public static bool RemoveCard(u8[] a_uCardIndex, u8[] a_uRemoveCard, n32 a_nRemoveCount)
        {
            Debug.Assert(a_uCardIndex.Length == MAX_INDEX);
            Debug.Assert(a_nRemoveCount <= a_uRemoveCard.Length);
            for (n32 i = 0; i < a_nRemoveCount; i++)
            {
                n32 nRemoveIndex = SwitchToCardIndex(a_uRemoveCard[i]);
                if (a_uCardIndex[nRemoveIndex] == 0)
                {
                    for (n32 j = 0; j < i; j++)
                    {
                        a_uCardIndex[SwitchToCardIndex(a_uRemoveCard[j])]++;
                    }
                    return false;
                }
                else
                {
                    a_uCardIndex[nRemoveIndex]--;
                }
            }
            return true;
        }

        public static bool RemoveCard(u8[] a_uCardData, n32 a_nCardCount, u8[] a_uRemoveCard, n32 a_nRemoveCount)
        {
            Debug.Assert(a_nCardCount <= a_uCardData.Length);
            Debug.Assert(a_nRemoveCount <= a_uRemoveCard.Length);
            n32 nDeleteCount = 0;
            u8[] uTempCardData = new u8[MAX_COUNT];
            if (a_nCardCount > uTempCardData.Length)
            {
                return false;
            }
            Array.Copy(a_uCardData, uTempCardData, a_nCardCount);
            for (n32 i = 0; i < a_nRemoveCount; i++)
            {
                for (n32 j = 0; j < a_nCardCount; j++)
                {
                    if (a_uRemoveCard[i] == uTempCardData[j])
                    {
                        nDeleteCount++;
                        uTempCardData[j] = 0;
                        break;
                    }
                }
            }
            if (nDeleteCount != a_nRemoveCount)
            {
                return false;
            }
            n32 nCardPos = 0;
            for (n32 i = 0; i < a_nCardCount; i++)
            {
                if (uTempCardData[i] != 0)
                {
                    a_uCardData[nCardPos++] = uTempCardData[i];
                }
            }
            return true;
        }

        public static bool RemoveAllCard(u8[] a_uCardIndex, u8 a_uRemoveCard)
        {
            Debug.Assert(a_uCardIndex.Length == MAX_INDEX);
            n32 nRemoveIndex = SwitchToCardIndex(a_uRemoveCard);
            a_uCardIndex[nRemoveIndex] = 0;
            return true;
        }

        public static bool IsValidCard(u8 a_uCardData)
        {
            n32 nValue = a_uCardData & MASK_VALUE;
            n32 nColor = (a_uCardData & MASK_COLOR) >> 4;
            return (nValue >= 1 && nValue <= 9 && nColor <= 2) || (nValue >= 1 && nValue <= 7 && nColor == 3);
        }

        public static u8 SwitchToCardData(n32 a_nCardIndex)
        {
            return (static_cast_u8)(((a_nCardIndex / 9) << 4) | (a_nCardIndex % 9 + 1));
        }

        public static n32 SwitchToCardIndex(u8 a_uCardData)
        {
            return ((a_uCardData & MASK_COLOR) >> 4) * 9 + (a_uCardData & MASK_VALUE) - 1;
        }

        public static n32 SwitchToCardData(u8[] a_uCardIndex, ArraySegment<u8> a_uCardData, n32 a_nMaxCount)
        {
            Debug.Assert(a_uCardIndex.Length == MAX_INDEX);
            Debug.Assert(a_nMaxCount <= a_uCardData.Count);
            Debug.Assert(a_nMaxCount <= MAX_COUNT);
            n32 nPosition = 0;
            for (n32 i = 0; i < MAX_INDEX; i++)
            {
                if (a_uCardIndex[i] != 0)
                {
                    u8 uCardData = SwitchToCardData(i);
                    for (n32 j = 0; j < a_uCardIndex[i]; j++)
                    {
                        a_uCardData[nPosition++] = uCardData;
                    }
                }
            }
            return nPosition;
        }

        public static void SwitchToCardIndex(ArraySegment<u8> a_uCardData, n32 a_nCardCount, u8[] a_uCardIndex)
        {
            Debug.Assert(a_nCardCount <= a_uCardData.Count);
            Debug.Assert(a_uCardIndex.Length == MAX_INDEX);
            a_uCardIndex.RecursiveClear();
            for (n32 i = 0; i < a_nCardCount; i++)
            {
                a_uCardIndex[SwitchToCardIndex(a_uCardData[i])]++;
            }
        }

        public static n32 GetCardCount(u8[] a_uCardIndex)
        {
            Debug.Assert(a_uCardIndex.Length == MAX_INDEX);
            n32 nCardCount = 0;
            for (n32 i = 0; i < MAX_INDEX; i++)
            {
                nCardCount += a_uCardIndex[i];
            }
            return nCardCount;
        }

        public static u8 EstimatePengCard(u8[] a_uCardIndex, u8 a_uCurrentCard)
        {
            Debug.Assert(a_uCardIndex.Length == MAX_INDEX);
            return a_uCardIndex[SwitchToCardIndex(a_uCurrentCard)] >= 2 ? WIK_P : WIK_NULL;
        }

        public static u8 EstimateGangCard(u8[] a_uCardIndex, u8 a_uCurrentCard)
        {
            Debug.Assert(a_uCardIndex.Length == MAX_INDEX);
            return a_uCardIndex[SwitchToCardIndex(a_uCurrentCard)] == 3 ? WIK_G : WIK_NULL;
        }

        public static n32 GetUserActionRank(u32 a_uUserAction)
        {
            if ((a_uUserAction & WIK_H) != 0)
            {
                return 3;
            }
            if ((a_uUserAction & WIK_G) != 0)
            {
                return 2;
            }
            if ((a_uUserAction & WIK_P) != 0)
            {
                return 1;
            }
            return 0;
        }

        public static n32 GetHuFanShu(u64 a_uHuRight, u8 a_uHuKind, u8 a_uHuSpecial)
        {
            return 1;
        }

        public static u8 AnalyseGangCard(u8[] a_uCardIndex, CWeaveItem[] a_WeaveItem, n32 a_nWeaveCount, CGangCardResult a_GangCardResult)
        {
            Debug.Assert(a_uCardIndex.Length == MAX_INDEX);
            Debug.Assert(a_nWeaveCount <= a_WeaveItem.Length);
            u8 uActionMask = WIK_NULL;
            a_GangCardResult.Reset();
            for (n32 i = 0; i < MAX_INDEX; i++)
            {
                if (a_uCardIndex[i] == 4)
                {
                    uActionMask |= WIK_G;
                    a_GangCardResult.Public[a_GangCardResult.CardCount] = false;
                    a_GangCardResult.CardData[a_GangCardResult.CardCount++] = SwitchToCardData(i);
                }
            }
            for (n32 i = 0; i < a_nWeaveCount; i++)
            {
                if (a_WeaveItem[i].WeaveKind == WIK_P)
                {
                    if (a_uCardIndex[SwitchToCardIndex(a_WeaveItem[i].CenterCard)] == 1)
                    {
                        uActionMask |= WIK_G;
                        a_GangCardResult.Public[a_GangCardResult.CardCount] = true;
                        a_GangCardResult.CardData[a_GangCardResult.CardCount++] = a_WeaveItem[i].CenterCard;
                    }
                }
            }
            return uActionMask;
        }

        public static u8 AnalyseHuCard(u8[] a_uCardIndex, CWeaveItem[] a_WeaveItem, n32 a_nWeaveCount, u8 a_uCurrentCard, ref u8 a_uHuKind, ref u64 a_uHuRight, ref u8 a_uHuSpecial, n32 a_nSendCardCount, n32 a_nOutCardCount, bool a_bGangStatus, bool a_bZimo, bool a_bQiangGangStatus, n32 a_nFanShu, bool a_bCheck)
        {
            Debug.Assert(a_uCardIndex.Length == MAX_INDEX);
            Debug.Assert(a_nWeaveCount <= a_WeaveItem.Length);
            u8[] uCardIndexTemp = new u8[MAX_INDEX];
            Array.Copy(a_uCardIndex, uCardIndexTemp, uCardIndexTemp.Length);
            if (a_uCurrentCard != 0)
            {
                uCardIndexTemp[SwitchToCardIndex(a_uCurrentCard)]++;
            }
            n32 nCardCountTemp = GetCardCount(uCardIndexTemp);
            n32 nCardCount = nCardCountTemp - 1;
            CAnalyseItemArray vAnalyseItemArray = new CAnalyseItemArray();
            AnalyseCard(uCardIndexTemp, nCardCountTemp, a_WeaveItem, a_nWeaveCount, vAnalyseItemArray);
            a_uHuRight |= PingHu(uCardIndexTemp, nCardCountTemp, a_uCardIndex, nCardCount, a_WeaveItem, a_nWeaveCount, vAnalyseItemArray);
            a_uHuRight |= PengPengHu(uCardIndexTemp, nCardCountTemp, a_uCardIndex, nCardCount, a_WeaveItem, a_nWeaveCount, vAnalyseItemArray);
            a_uHuRight |= QingSe(uCardIndexTemp, nCardCountTemp, a_uCardIndex, nCardCount, a_WeaveItem, a_nWeaveCount, vAnalyseItemArray);
            a_uHuRight |= QiDui(uCardIndexTemp, nCardCountTemp, a_uCardIndex, nCardCount, a_WeaveItem, a_nWeaveCount, vAnalyseItemArray);
            a_uHuRight |= DiaoYu(uCardIndexTemp, nCardCountTemp, a_uCardIndex, nCardCount, a_WeaveItem, a_nWeaveCount, vAnalyseItemArray);
            a_uHuKind |= ZiMo(a_uHuRight, a_bGangStatus, a_bZimo);
            a_uHuKind |= GangKai(a_uHuRight, a_bGangStatus, a_bZimo);
            a_uHuKind |= QiangGang(a_uHuRight, a_bQiangGangStatus, a_bZimo);
            a_uHuKind |= JiePao(a_uHuRight, a_bQiangGangStatus, a_bZimo);
            a_uHuSpecial |= GangPai(uCardIndexTemp);
            a_uHuSpecial |= DanZhang(nCardCount);
            a_uHuSpecial |= DiHu(a_nSendCardCount, a_nOutCardCount);
            a_uHuSpecial |= TianHu(a_nSendCardCount, a_nOutCardCount);
            a_uHuSpecial |= KaZhang(a_uCardIndex, a_WeaveItem, a_nWeaveCount);

            if (a_uHuRight != 0)
            {
                //n32 nFs = GetHuFanShu(a_uHuRight, a_uHuKind, a_uHuSpecial);
                //if ((a_uHuKind & CHK_QG) == 0 && !a_bZimo)
                //{
                //    if (!a_bCheck && nFs <= a_nFanShu)
                //    {
                //        return WIK_NULL;
                //    }
                //}
                //a_nFanShu = nFs;
                //if (nFs < 2)
                //{
                //    if ((a_uHuKind & CHK_ZM) == 0 && (a_uHuKind & CHK_QG) == 0)
                //    {
                //        return WIK_NULL;
                //    }
                //}
                return WIK_H;
            }
            return WIK_NULL;
        }

        public static n32 AnalyseHuCardCount(u8[] a_uCardIndex, CWeaveItem[] a_WeaveItem, n32 a_nWeaveCount)
        {
            Debug.Assert(a_uCardIndex.Length == MAX_INDEX);
            Debug.Assert(a_nWeaveCount <= a_WeaveItem.Length);
            n32 nCount = 0;
            u8[] uCardIndexTemp = new u8[MAX_INDEX];
            Array.Copy(a_uCardIndex, uCardIndexTemp, uCardIndexTemp.Length);
            for (n32 i = 0; i < MAX_INDEX; i++)
            {
                u8 uCurrentCard = SwitchToCardData(i);
                if (AnalyseCanHuCard(uCardIndexTemp, a_WeaveItem, a_nWeaveCount, uCurrentCard))
                {
                    nCount++;
                }
            }
            return nCount;
        }

        public static bool AnalyseCard(u8[] a_uCardIndex, n32 a_nCardCount, CWeaveItem[] a_WeaveItem, n32 a_nItemCount, CAnalyseItemArray a_vAnalyseItemArray)
        {
            Debug.Assert(a_uCardIndex.Length == MAX_INDEX);
            Debug.Assert(a_nItemCount <= a_WeaveItem.Length);
            if (a_nCardCount < 2 || a_nCardCount > MAX_COUNT || (a_nCardCount - 2) % 3 != 0)
            {
                return false;
            }
            n32 nKindItemCount = 0;
            CKindItem[] kindItem = CJaggedArray.CreateInstance<CKindItem[], CKindItem>(2 * MAX_INDEX);
            n32 nLessKindItem = (a_nCardCount - 2) / 3;
            if (nLessKindItem == 0)
            {
                for (n32 i = 0; i < MAX_INDEX; i++)
                {
                    if (a_uCardIndex[i] == 2)
                    {
                        CAnalyseItem analyseItem = new CAnalyseItem();
                        for (n32 j = 0; j < a_nItemCount; j++)
                        {
                            analyseItem.WeaveKind[j] = a_WeaveItem[j].WeaveKind;
                            analyseItem.CenterCard[j] = a_WeaveItem[j].CenterCard;
                        }
                        analyseItem.CardEye = SwitchToCardData(i);
                        a_vAnalyseItemArray.push_back(analyseItem);
                        return true;
                    }
                }
                return false;
            }
            if (a_nCardCount >= 3)
            {
                for (n32 i = 0; i < MAX_INDEX; i++)
                {
                    if (a_uCardIndex[i] >= 3)
                    {
                        kindItem[nKindItemCount].CenterCard = SwitchToCardData(i);
                        kindItem[nKindItemCount].CardIndex[0] = i;
                        kindItem[nKindItemCount].CardIndex[1] = i;
                        kindItem[nKindItemCount].CardIndex[2] = i;
                        kindItem[nKindItemCount++].WeaveKind = WIK_P;
                    }
                    if (i < MAX_INDEX - 7 - 2 && a_uCardIndex[i] > 0 && i % 9 < 7)
                    {
                        for (n32 j = 1; j <= a_uCardIndex[i]; j++)
                        {
                            if (a_uCardIndex[i + 1] >= j && a_uCardIndex[i + 2] >= j)
                            {
                                kindItem[nKindItemCount].CenterCard = SwitchToCardData(i + 1);
                                kindItem[nKindItemCount].CardIndex[0] = i;
                                kindItem[nKindItemCount].CardIndex[1] = i + 1;
                                kindItem[nKindItemCount].CardIndex[2] = i + 2;
                                kindItem[nKindItemCount++].WeaveKind = WIK_S;
                            }
                        }
                    }
                }
            }

            if (nKindItemCount >= nLessKindItem)
            {
                u8[] uCardIndexTemp = new u8[MAX_INDEX];
                CKindItem[] kindItemPointer = new CKindItem[MAX_WEAVE];
                vectorp<vectorp<n32>> vComb = Comb(nKindItemCount, nLessKindItem);
                n32 nCombSize = vComb.size();
                for (n32 nCombIndex = 0; nCombIndex < nCombSize; nCombIndex++)
                {
                    vectorp<n32> vIndex = vComb[nCombIndex];
                    Array.Copy(a_uCardIndex, uCardIndexTemp, uCardIndexTemp.Length);
                    for (n32 i = 0; i < nLessKindItem; i++)
                    {
                        kindItemPointer[i] = kindItem[vIndex[i]];
                    }
                    bool bEnoughCard = true;
                    for (n32 i = 0; i < nLessKindItem * 3; i++)
                    {
                        n32 nTempCardIndex = kindItemPointer[i / 3].CardIndex[i % 3];
                        if (uCardIndexTemp[nTempCardIndex] == 0)
                        {
                            bEnoughCard = false;
                            break;
                        }
                        else
                        {
                            uCardIndexTemp[nTempCardIndex]--;
                        }
                    }

                    if (bEnoughCard)
                    {
                        u8 uCardEye = 0;
                        for (n32 i = 0; i < MAX_INDEX; i++)
                        {
                            if (uCardIndexTemp[i] == 2)
                            {
                                uCardEye = SwitchToCardData(i);
                                break;
                            }
                        }

                        if (uCardEye != 0)
                        {
                            CAnalyseItem analyseItem = new CAnalyseItem();
                            for (n32 i = 0; i < a_nItemCount; i++)
                            {
                                analyseItem.WeaveKind[i] = a_WeaveItem[i].WeaveKind;
                                analyseItem.CenterCard[i] = a_WeaveItem[i].CenterCard;
                            }
                            for (n32 i = 0; i < nLessKindItem; i++)
                            {
                                analyseItem.WeaveKind[i + a_nItemCount] = kindItemPointer[i].WeaveKind;
                                analyseItem.CenterCard[i + a_nItemCount] = kindItemPointer[i].CenterCard;
                            }
                            analyseItem.CardEye = uCardEye;
                            a_vAnalyseItemArray.push_back(analyseItem);
                        }
                    }
                }
            }
            return !a_vAnalyseItemArray.empty();
        }

        public static bool AnalyseCanHuCard(u8[] a_uCardIndex, CWeaveItem[] a_WeaveItem, n32 a_nWeaveCount, u8 a_uCurrentCard)
        {
            Debug.Assert(a_uCardIndex.Length == MAX_INDEX);
            Debug.Assert(a_nWeaveCount <= a_WeaveItem.Length);
            u8[] uCardIndexTemp = new u8[MAX_INDEX];
            Array.Copy(a_uCardIndex, uCardIndexTemp, uCardIndexTemp.Length);
            if (a_uCurrentCard != 0)
            {
                uCardIndexTemp[SwitchToCardIndex(a_uCurrentCard)]++;
            }
            n32 nCardCountTemp = GetCardCount(uCardIndexTemp);
            n32 nCardCount = nCardCountTemp - 1;
            CAnalyseItemArray vAnalyseItemArray = new CAnalyseItemArray();
            AnalyseCard(uCardIndexTemp, nCardCountTemp, a_WeaveItem, a_nWeaveCount, vAnalyseItemArray);
            return CanHu(uCardIndexTemp, nCardCountTemp, a_uCardIndex, nCardCount, a_WeaveItem, a_nWeaveCount, vAnalyseItemArray);
        }

        public static bool AnalyseTingCardResult(u8[] a_uCardIndex, CWeaveItem[] a_WeaveItem, n32 a_nWeaveCount, CTingResult a_TingResult)
        {
            Debug.Assert(a_uCardIndex.Length == MAX_INDEX);
            Debug.Assert(a_nWeaveCount <= a_WeaveItem.Length);
            a_TingResult.Reset();
            u8[] uCardIndexTemp = new u8[MAX_INDEX];
            Array.Copy(a_uCardIndex, uCardIndexTemp, uCardIndexTemp.Length);
            for (n32 i = 0; i < MAX_INDEX; i++)
            {
                u8 uCurrentCard = SwitchToCardData(i);
                if (AnalyseCanHuCard(uCardIndexTemp, a_WeaveItem, a_nWeaveCount, uCurrentCard))
                {
                    a_TingResult.TingCard[a_TingResult.TingCount++] = uCurrentCard;
                }
            }
            return a_TingResult.TingCount > 0;
        }

        public static bool CanHu(u8[] a_uCardIndexTemp, n32 a_nCardCountTemp, u8[] a_uCardIndex, n32 a_nCardCount, CWeaveItem[] a_WeaveItem, n32 a_nWeaveCount, CAnalyseItemArray a_vAnalyseItemArray)
        {
            Debug.Assert(a_uCardIndexTemp.Length == MAX_INDEX);
            Debug.Assert(a_uCardIndex.Length == MAX_INDEX);
            Debug.Assert(a_nWeaveCount <= a_WeaveItem.Length);
            if (a_nWeaveCount == 0)
            {
                n32 nDuiCount = 0;
                for (n32 i = 0; i < MAX_INDEX; i++)
                {
                    if (a_uCardIndexTemp[i] % 2 == 0)
                    {
                        nDuiCount += a_uCardIndexTemp[i] / 2;
                    }
                }
                if (nDuiCount == 7)
                {
                    return true;
                }
            }
            return !a_vAnalyseItemArray.empty();
        }

        public static u64 PingHu(u8[] a_uCardIndexTemp, n32 a_nCardCountTemp, u8[] a_uCardIndex, n32 a_nCardCount, CWeaveItem[] a_WeaveItem, n32 a_nWeaveCount, CAnalyseItemArray a_vAnalyseItemArray)
        {
            Debug.Assert(a_uCardIndexTemp.Length == MAX_INDEX);
            Debug.Assert(a_uCardIndex.Length == MAX_INDEX);
            Debug.Assert(a_nWeaveCount <= a_WeaveItem.Length);
            if (!a_vAnalyseItemArray.empty())
            {
                for (n32 i = 0; i < a_vAnalyseItemArray.size(); i++)
                {
                    bool bLianCard = false;
                    bool bPengCard = false;
                    CAnalyseItem analyseItem = a_vAnalyseItemArray[i];
                    for (n32 j = 0; j < analyseItem.WeaveKind.Length; j++)
                    {
                        u8 uWeaveKind = analyseItem.WeaveKind[j];
                        bPengCard = (uWeaveKind & (WIK_G | WIK_P)) != 0 ? true : bPengCard;
                        bLianCard = (uWeaveKind & WIK_S) != 0 ? true : bLianCard;
                    }
                    if (bLianCard)
                    {
                        return CHR_PH;
                    }
                }
            }
            return CHR_NULL;
        }

        public static u64 QingSe(u8[] a_uCardIndexTemp, n32 a_nCardCountTemp, u8[] a_uCardIndex, n32 a_nCardCount, CWeaveItem[] a_WeaveItem, n32 a_nWeaveCount, CAnalyseItemArray a_vAnalyseItemArray)
        {
            Debug.Assert(a_uCardIndexTemp.Length == MAX_INDEX);
            Debug.Assert(a_uCardIndex.Length == MAX_INDEX);
            Debug.Assert(a_nWeaveCount <= a_WeaveItem.Length);
            u8 uCardColor = 0xFF;
            for (n32 i = 0; i < MAX_INDEX; i++)
            {
                if (a_uCardIndexTemp[i] != 0)
                {
                    u8 uTempCardColor = (static_cast_u8)(SwitchToCardData(i) & MASK_COLOR);

                    if (uCardColor == 0xFF)
                    {
                        uCardColor = uTempCardColor;
                    }
                    if (uTempCardColor != uCardColor)
                    {
                        return CHR_NULL;
                    }
                }
            }
            for (n32 i = 0; i < a_nWeaveCount; i++)
            {
                u8 uCenterCard = a_WeaveItem[i].CenterCard;
                if ((uCenterCard & MASK_COLOR) != uCardColor)
                {
                    return CHR_NULL;
                }
            }
            if (CanHu(a_uCardIndexTemp, a_nCardCountTemp, a_uCardIndex, a_nCardCount, a_WeaveItem, a_nWeaveCount, a_vAnalyseItemArray))
            {
                return CHR_QS;
            }
            return CHR_NULL;
        }

        public static u64 PengPengHu(u8[] a_uCardIndexTemp, n32 a_nCardCountTemp, u8[] a_uCardIndex, n32 a_nCardCount, CWeaveItem[] a_WeaveItem, n32 a_nWeaveCount, CAnalyseItemArray a_vAnalyseItemArray)
        {
            Debug.Assert(a_uCardIndexTemp.Length == MAX_INDEX);
            Debug.Assert(a_uCardIndex.Length == MAX_INDEX);
            Debug.Assert(a_nWeaveCount <= a_WeaveItem.Length);
            if (!a_vAnalyseItemArray.empty())
            {
                for (n32 i = 0; i < a_vAnalyseItemArray.size(); i++)
                {
                    bool bLianCard = false;
                    bool bPengCard = false;
                    CAnalyseItem analyseItem = a_vAnalyseItemArray[i];
                    for (n32 j = 0; j < analyseItem.WeaveKind.Length; j++)
                    {
                        u8 uWeaveKind = analyseItem.WeaveKind[j];
                        bPengCard = (uWeaveKind & (WIK_G | WIK_P)) != 0 ? true : bPengCard;
                        bLianCard = (uWeaveKind & WIK_S) != 0 ? true : bLianCard;
                    }
                    if (!bLianCard && bPengCard)
                    {
                        return CHR_PPH;
                    }
                }
            }
            return CHR_NULL;
        }

        public static u64 QiDui(u8[] a_uCardIndexTemp, n32 a_nCardCountTemp, u8[] a_uCardIndex, n32 a_nCardCount, CWeaveItem[] a_WeaveItem, n32 a_nWeaveCount, CAnalyseItemArray a_vAnalyseItemArray)
        {
            Debug.Assert(a_uCardIndexTemp.Length == MAX_INDEX);
            Debug.Assert(a_uCardIndex.Length == MAX_INDEX);
            Debug.Assert(a_nWeaveCount <= a_WeaveItem.Length);
            if (a_nWeaveCount > 0)
            {
                return CHR_NULL;
            }
            for (n32 i = 0; i < MAX_INDEX; i++)
            {
                if (a_uCardIndexTemp[i] % 2 == 1)
                {
                    return CHR_NULL;
                }
            }
            return CHR_QD;
        }

        public static u64 DiaoYu(u8[] a_uCardIndexTemp, n32 a_nCardCountTemp, u8[] a_uCardIndex, n32 a_nCardCount, CWeaveItem[] a_WeaveItem, n32 a_nWeaveCount, CAnalyseItemArray a_vAnalyseItemArray)
        {
            Debug.Assert(a_uCardIndexTemp.Length == MAX_INDEX);
            Debug.Assert(a_uCardIndex.Length == MAX_INDEX);
            Debug.Assert(a_nWeaveCount <= a_WeaveItem.Length);
            if (PengPengHu(a_uCardIndexTemp, a_nCardCountTemp, a_uCardIndex, a_nCardCount, a_WeaveItem, a_nWeaveCount, a_vAnalyseItemArray) == CHR_PPH && a_nCardCountTemp == 2)
            {
                return CHR_DY;
            }
            return CHR_NULL;
        }

        public static u8 GangPai(u8[] a_uCardIndex)
        {
            Debug.Assert(a_uCardIndex.Length == MAX_INDEX);
            for (n32 i = 0; i < MAX_INDEX; i++)
            {
                if (a_uCardIndex[i] == 4)
                {
                    return CHS_GP;
                }
            }
            return CHS_NULL;
        }

        public static u8 DanZhang(n32 a_nCardCount)
        {
            if (a_nCardCount == 1)
            {
                return CHS_DZ;
            }
            return CHS_NULL;
        }

        public static u8 TianHu(n32 a_nSendCardCount, n32 a_nOutCardCount)
        {
            if (a_nSendCardCount == 1 && a_nOutCardCount == 0)
            {
                return CHS_TH;
            }
            return CHS_NULL;
        }

        public static u8 DiHu(n32 a_nSendCardCount, n32 a_nOutCardCount)
        {
            if (a_nSendCardCount == 1 && a_nOutCardCount == 1)
            {
                return CHS_DH;
            }
            return CHS_NULL;
        }

        public static u8 KaZhang(u8[] a_uCardIndex, CWeaveItem[] a_WeaveItem, n32 a_nWeaveCount)
        {
            Debug.Assert(a_uCardIndex.Length == MAX_INDEX);
            Debug.Assert(a_nWeaveCount <= a_WeaveItem.Length);
            if (AnalyseHuCardCount(a_uCardIndex, a_WeaveItem, a_nWeaveCount) == 1)
            {
                return CHS_KZ;
            }
            return CHS_NULL;
        }

        public static u8 ZiMo(u64 a_uHuRight, bool a_bGangStatus, bool a_bZimo)
        {
            if (a_uHuRight != 0 && a_bZimo && !a_bGangStatus)
            {
                return CHK_ZM;
            }
            return CHK_NULL;
        }

        public static u8 GangKai(u64 a_uHuRight, bool a_bGangStatus, bool a_bZimo)
        {
            if (a_uHuRight != 0 && a_bZimo && a_bGangStatus)
            {
                return CHK_GK;
            }
            return CHK_NULL;
        }

        public static u8 QiangGang(u64 a_uHuRight, bool a_bGangStatus, bool a_bZimo)
        {
            if (a_uHuRight != 0 && !a_bZimo && a_bGangStatus)
            {
                return CHK_QG;
            }
            return CHK_NULL;
        }

        public static u8 JiePao(u64 a_uHuRight, bool a_bGangStatus, bool a_bZimo)
        {
            if (a_uHuRight != 0 && !a_bZimo && !a_bGangStatus)
            {
                return CHK_JP;
            }
            return CHK_NULL;
        }
    }
}
