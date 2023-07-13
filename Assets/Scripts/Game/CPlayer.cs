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
    public class CPlayer
    {
        public enum EPlayerSex
        {
            kFemale,
            kMale
        }

        private bool m_bIsBot = false;
        private n32 m_nChairID = 0;
        private IGameEngineEventListener m_GameEngineEventListener = null;
        private EPlayerSex m_eSex = EPlayerSex.kMale;

        public CPlayer(bool a_bIsBot, EPlayerSex a_eSex, IGameEngineEventListener a_GameEngineEventListener)
        {
            m_bIsBot = a_bIsBot;
            m_eSex = a_eSex;
            m_GameEngineEventListener = a_GameEngineEventListener;
            m_GameEngineEventListener.SetPlayer(this);
        }

        public bool IsBot()
        {
            return m_bIsBot;
        }

        public void SetChairID(n32 a_nChairID)
        {
            m_nChairID = a_nChairID;
        }

        public n32 GetChairID()
        {
            return m_nChairID;
        }

        public IGameEngineEventListener GetGameEngineEventListener()
        {
            return m_GameEngineEventListener;
        }

        public EPlayerSex GetSex()
        {
            return m_eSex;
        }

        public string GetSexAsStr()
        {
            return m_eSex == EPlayerSex.kFemale ? "female" : "male";
        }
    }
}
