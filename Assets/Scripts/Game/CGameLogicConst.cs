using System;
using sdw.game;

using static majiang0.CGameCmd;

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

namespace majiang0
{
    public static class CGameLogicConst
    {
        public const n32 MASK_COLOR = 0xF0;
        public const n32 MASK_VALUE = 0x0F;

        public const u8 WIK_NULL = 0x00;
        public const u8 WIK_P = 0x01;
        public const u8 WIK_G = 0x02;
        public const u8 WIK_H = 0x04;
        public const u8 WIK_S = 0x08;

        public const u8 CHR_NULL = 0x00;
        public const u8 CHR_PH = 0x01;
        public const u8 CHR_PPH = 0x02;
        public const u8 CHR_QS = 0x04;
        public const u8 CHR_DY = 0x08;
        public const u8 CHR_QD = 0x10;

        public const u8 CHK_NULL = 0x00;
        public const u8 CHK_ZM = 0x01;
        public const u8 CHK_JP = 0x02;
        public const u8 CHK_QG = 0x04;
        public const u8 CHK_GK = 0x08;

        public const u8 CHS_NULL = 0x00;
        public const u8 CHS_DZ = 0x01;
        public const u8 CHS_DH = 0x02;
        public const u8 CHS_TH = 0x04;
        public const u8 CHS_GP = 0x08;
        public const u8 CHS_KZ = 0x10;

        public class CKindItem
        {
            public u8 WeaveKind = 0;
            public u8 CenterCard = 0;
            public n32[] CardIndex = new n32[3];
        }

        public class CWeaveItem : ICloneable
        {
            public u8 WeaveKind = 0;
            public u8 CenterCard = 0;
            public bool PublicCard = false;
            public n32 ProvideUser = 0;
            public n32 Valid = 0;

            public void Reset()
            {
                WeaveKind = 0;
                CenterCard = 0;
                PublicCard = false;
                ProvideUser = 0;
                Valid = 0;
            }

            public object Clone()
            {
                CWeaveItem copy = new CWeaveItem();
                copy.WeaveKind = WeaveKind;
                copy.CenterCard = CenterCard;
                copy.PublicCard = PublicCard;
                copy.ProvideUser = ProvideUser;
                copy.Valid = Valid;
                return copy;
            }
        }

        public class CGangCardResult
        {
            public n32 CardCount = 0;
            public u8[] CardData = new u8[MAX_WEAVE];
            public bool[] Public = new bool[MAX_WEAVE];

            public void Reset()
            {
                CardCount = 0;
                CardData.RecursiveClear();
                Public.RecursiveClear();
            }
        }

        public class CAnalyseItem
        {
            public u8 CardEye = 0;
            public u8[] WeaveKind = new u8[MAX_WEAVE];
            public u8[] CenterCard = new u8[MAX_WEAVE];

            public void Reset()
            {
                CardEye = 0;
                WeaveKind.RecursiveClear();
                CenterCard.RecursiveClear();
            }
        }

        public class CTingResult
        {
            public n32 TingCount = 0;
            public u8[] TingCard = new u8[MAX_INDEX];

            public void Reset()
            {
                TingCount = 0;
                TingCard.RecursiveClear();
            }
        }
    }
}
