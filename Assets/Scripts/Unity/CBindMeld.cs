using UnityEngine;
using UnityEngine.UI;

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
    public class CBindMeld : MonoBehaviour
    {
        public Image m_CenterImage = null;
        public Image m_LeftImage = null;
        public Image m_RightImage = null;

        private void Awake()
        {
            if (m_CenterImage == null)
            {
                Debug.LogError("set CenterImage first");
                return;
            }
            if (m_LeftImage == null)
            {
                Debug.LogError("set LeftImage first");
                return;
            }
            if (m_RightImage == null)
            {
                Debug.LogError("set RightImage first");
                return;
            }
        }

        // Start is called before the first frame update
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}
