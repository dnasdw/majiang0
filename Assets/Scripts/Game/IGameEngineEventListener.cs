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
    public interface IGameEngineEventListener
    {
        public void SetPlayer(CPlayer a_Player);

        public bool OnUserEnterEvent(CPlayer a_Player);

        public bool OnGameStartEvent(CCMD_S_GameStart a_SGameStart);

        public bool OnSendCardEvent(CCMD_S_SendCard a_SSendCard);

        public bool OnOutCardEvent(CCMD_S_OutCard a_SOutCard);

        public bool OnOperateNotifyEvent(CCMD_S_OperateNotify a_SOperateNotify);

        public bool OnOperateResultEvent(CCMD_S_OperateResult a_SOperateResult);

        public bool OnGameEndEvent(CCMD_S_GameEnd a_SGameEnd);
    }
}
