using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
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

namespace majiang0
{
    public class CStorageManager : IStorageManager
    {
        private const string s_sGameStateKey = "majiang0";

        private CStorage m_Storage = null;
        private SGameState? m_GameState = null;

        // https://docs.unity3d.com/Manual/ScriptingRestrictions.html
        public void UsedOnlyForAOTCodeGeneration()
        {
            AotHelper.EnsureList<SGameState>();

            // Include an exception so we can be sure to know if this method is ever called.
            throw new InvalidOperationException("This method is used for AOT code generation only. Do not call it at runtime.");
        }

        public bool Init()
        {
            string sDirPath = Format("{0}/save", Application.persistentDataPath);
            m_Storage = new CStorage(sDirPath);
            if (m_Storage != null)
            {
                string sGameState = m_Storage.GetItem(s_sGameStateKey);
                if (sGameState != null)
                {
                    m_GameState = JsonConvert.DeserializeObject<SGameState>(sGameState);
                }
            }
            return m_Storage != null;
        }

        public SGameState? GetGameState()
        {
            return m_GameState;
        }

        public void SetGameState(SGameState a_GameState)
        {
            setGameState(a_GameState);
        }

        public void ClearGameState()
        {
            if (m_Storage != null)
            {
                m_GameState = null;
                m_Storage.RemoveItem(s_sGameStateKey);
            }
        }

        public void SaveAll()
        {
            if (m_GameState.HasValue)
            {
                setGameState(m_GameState.Value, true);
            }
            else
            {
                ClearGameState();
            }
        }

        private void setGameState(SGameState a_GameState, bool a_bForceSave = false)
        {
            m_GameState = a_GameState;
            if (m_Storage != null && a_bForceSave)
            {
                string sGameState = JsonConvert.SerializeObject(m_GameState);
                m_Storage.SetItem(s_sGameStateKey, sGameState);
            }
        }
    }
}
