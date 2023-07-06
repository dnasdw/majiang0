using System.Collections.Generic;
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
    public class COnScreenLog : MonoBehaviour
    {
        public GUIStyle Style;
        public u32 QueueSize = 15;
        private queuep<string> m_qLog = new queuep<string>();
        private string m_sLogCache = "";

        // Start is called before the first frame update
        private void Start()
        {
            Debug.Log("Started up logging.");
        }

        // Update is called once per frame
        private void Update()
        {
        }

        private void OnGUI()
        {
            GUILayout.Label(m_sLogCache, Style);
        }

        private void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        public void HandleLog(string a_sCondition, string a_sStackTrace, LogType a_eType)
        {
            m_qLog.push("[" + a_eType + "]: " + a_sCondition);
            if (a_eType == LogType.Exception)
            {
                m_qLog.push(a_sStackTrace);
            }
            while (m_qLog.size() > QueueSize)
            {
                m_qLog.pop();
            }
            m_sLogCache = "";
            for (IEnumerator<string> it = m_qLog.GetEnumerator(); it.MoveNext(); /**/)
            {
                string sLog = it.Current;
                m_sLogCache += "\n" + sLog;
            }
        }
    }
}
