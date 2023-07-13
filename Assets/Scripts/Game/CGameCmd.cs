using System;
using sdw.game;

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

namespace majiang0
{
    public static class CGameCmd
    {
        public const n32 INVALID_CHAIR = 0xFF;
        public const n32 INVALID_BYTE = 0xFF;

        public const n32 GAME_PLAYER = 4;
        public const n32 MAX_WEAVE = 4;
        public const n32 MAX_INDEX = 34;
        public const n32 MAX_REPERTORY = 136;
        public const n32 MAX_COUNT = 14;
        public const n32 MAX_DISCARD = 60;

        public class CCMD_S_GameStart
        {
            public n32 DiceCount = 0;
            public n32 BankerUser = 0;
            public n32 CurrentUser = 0;
            public u8[] CardData = new u8[MAX_COUNT * GAME_PLAYER];
            public n32 LeftCardCount = 0;
        }

        public class CCMD_S_OutCard
        {
            public n32 OutCardUser = 0;
            public u8 OutCardData = 0;

            public void Reset()
            {
                OutCardUser = 0;
                OutCardData = 0;
            }
        }

        public class CCMD_S_SendCard
        {
            public u8 CardData = 0;
            public u8 ActionMask = 0;
            public n32 CurrentUser = 0;
            public n32 GangCount = 0;
            public u8[] GangCard = new u8[MAX_WEAVE];
            public bool Tail = false;

            public void Reset()
            {
                CardData = 0;
                ActionMask = 0;
                CurrentUser = 0;
                GangCount = 0;
                GangCard.RecursiveClear();
                Tail = false;
            }
        }

        public class CCMD_S_OperateNotify
        {
            public n32 ResumeUser = 0;
            public u8 ActionMask = 0;
            public u8 ActionCard = 0;
            public n32 GangCount = 0;
            public u8[] GangCard = new u8[MAX_WEAVE];

            public void Reset()
            {
                ResumeUser = 0;
                ActionMask = 0;
                ActionCard = 0;
                GangCount = 0;
                GangCard.RecursiveClear();
            }
        }

        public class CCMD_S_OperateResult
        {
            public n32 OperateUser = 0;
            public n32 ProvideUser = 0;
            public u8 OperateCode = 0;
            public u8 OperateCard = 0;
        }

        public class CCMD_S_GameEnd : ICloneable
        {
            public n32[] CardCount = new n32[GAME_PLAYER];
            public u8[][] CardData = CJaggedArray.CreateInstance<u8[][]>(GAME_PLAYER, MAX_COUNT);
            public n32 HuUser = 0;
            public n32 ProvideUser = 0;
            public u8 HuCard = 0;
            public u64[] HuRight = new u64[GAME_PLAYER];
            public u8[] HuKind = new u8[GAME_PLAYER];
            public u8[] HuSpecial = new u8[GAME_PLAYER];
            public n32[] WeaveCount = new n32[GAME_PLAYER];
            public CWeaveItem[][] WeaveItemArray = CJaggedArray.CreateInstance<CWeaveItem[][], CWeaveItem>(GAME_PLAYER, MAX_WEAVE);
            public n64[] NormalGameScore = new n64[GAME_PLAYER];
            public n64[] GameScore = new n64[GAME_PLAYER];
            public n64[] GameScoreTable = new n64[GAME_PLAYER];

            public void Reset()
            {
                CardCount.RecursiveClear();
                CardData.RecursiveClear();
                HuUser = 0;
                ProvideUser = 0;
                HuCard = 0;
                HuRight.RecursiveClear();
                HuKind.RecursiveClear();
                HuSpecial.RecursiveClear();
                WeaveCount.RecursiveClear();
                WeaveItemArray.RecursiveClear<CWeaveItem>();
                NormalGameScore.RecursiveClear();
                GameScore.RecursiveClear();
                GameScoreTable.RecursiveClear();
            }

            public object Clone()
            {
                CCMD_S_GameEnd copy = new CCMD_S_GameEnd();
                copy.CardCount = reinterpret_cast<n32[]>(CardCount.Clone());
                copy.CardData = reinterpret_cast<u8[][]>(CardData.Clone());
                copy.HuUser = HuUser;
                copy.ProvideUser = ProvideUser;
                copy.HuCard = HuCard;
                copy.HuRight = reinterpret_cast<u64[]>(HuRight.Clone());
                copy.HuKind = reinterpret_cast<u8[]>(HuKind.Clone());
                copy.HuSpecial = reinterpret_cast<u8[]>(HuSpecial.Clone());
                copy.WeaveCount = reinterpret_cast<n32[]>(WeaveCount.Clone());
                copy.WeaveItemArray = reinterpret_cast<CWeaveItem[][]>(WeaveItemArray.Clone());
                copy.NormalGameScore = reinterpret_cast<n64[]>(NormalGameScore.Clone());
                copy.GameScore = reinterpret_cast<n64[]>(GameScore.Clone());
                copy.GameScoreTable = reinterpret_cast<n64[]>(GameScoreTable.Clone());
                return copy;
            }
        }

        public class CCMD_C_OutCard
        {
            public u8 CardData = 0;

            public void Reset()
            {
                CardData = 0;
            }
        }

        public class CCMD_C_OperateCard
        {
            public n32 OperateUser = 0;
            public u8 OperateCode = 0;
            public u8 OperateCard = 0;

            public void Reset()
            {
                OperateUser = 0;
                OperateCode = 0;
                OperateCard = 0;
            }
        }
    }
}
