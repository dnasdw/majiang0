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
    public class CUIHelper
    {
        public static string GetDiscardCardImagePath(n32 a_nViewID, u8 a_uData)
        {
            string sImagePath = "";
            switch (a_nViewID)
            {
            case 0:
                sImagePath = Format("Assets/Textures/Game/Mahjong/2/mingmah_{0:d}{1:d}.png",
                    ((a_uData & MASK_COLOR) >> 4) + 1,
                    a_uData & MASK_VALUE);
                break;
            case 1:
                sImagePath = Format("Assets/Textures/Game/Mahjong/3/mingmah_{0:d}{1:d}.png",
                    ((a_uData & MASK_COLOR) >> 4) + 1,
                    a_uData & MASK_VALUE);
                break;
            case 2:
                sImagePath = Format("Assets/Textures/Game/Mahjong/2/mingmah_{0:d}{1:d}.png",
                    ((a_uData & MASK_COLOR) >> 4) + 1,
                    a_uData & MASK_VALUE);
                break;
            case 3:
                sImagePath = Format("Assets/Textures/Game/Mahjong/1/mingmah_{0:d}{1:d}.png",
                    ((a_uData & MASK_COLOR) >> 4) + 1,
                    a_uData & MASK_VALUE);
                break;
            default:
                break;
            }
            return sImagePath;
        }

        public static string GetHandCardImagePath(n32 a_nViewID, u8 a_uData)
        {
            bool bIsShowAllMahjong = CDebugValue.IsShowAllMahjong();
            string sImagePath = "";
            switch (a_nViewID)
            {
            case 0:
                sImagePath = Format("Assets/Textures/Game/Mahjong/2/handmah_{0:d}{1:d}.png",
                    ((a_uData & MASK_COLOR) >> 4) + 1,
                    a_uData & MASK_VALUE);
                break;
            case 1:
                sImagePath = "Assets/Textures/Game/Mahjong/hand_left.png";
                if (bIsShowAllMahjong)
                {
                    sImagePath = Format("Assets/Textures/Game/Mahjong/3/mingmah_{0:d}{1:d}.png",
                        ((a_uData & MASK_COLOR) >> 4) + 1,
                        a_uData & MASK_VALUE);
                }
                break;
            case 2:
                sImagePath = "Assets/Textures/Game/Mahjong/hand_top.png";
                if (bIsShowAllMahjong)
                {
                    sImagePath = Format("Assets/Textures/Game/Mahjong/2/mingmah_{0:d}{1:d}.png",
                        ((a_uData & MASK_COLOR) >> 4) + 1,
                        a_uData & MASK_VALUE);
                }
                break;
            case 3:
                sImagePath = "Assets/Textures/Game/Mahjong/hand_right.png";
                if (bIsShowAllMahjong)
                {
                    sImagePath = Format("Assets/Textures/Game/Mahjong/1/mingmah_{0:d}{1:d}.png",
                        ((a_uData & MASK_COLOR) >> 4) + 1,
                        a_uData & MASK_VALUE);
                }
                break;
            default:
                break;
            }
            return sImagePath;
        }
    }
}
