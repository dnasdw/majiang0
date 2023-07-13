using UnityEngine;
using UnityEngine.EventSystems;
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
    public class CTile : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
    {
        public Image m_TileImage = null;

        public n32 m_nTileIndex = 0;
        public u8 m_uTileData = 0;

        private IPointerEventHandler<CTile> m_PointerEventHandler = null;

        private void Awake()
        {
            if (m_TileImage == null)
            {
                Debug.LogError("set TileImage first");
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

        public void OnPointerDown(PointerEventData a_EventData)
        {
            if (m_PointerEventHandler != null)
            {
                m_PointerEventHandler.OnPointer(this, a_EventData, EPointerEventType.kPointerDown);
            }
        }

        public void OnPointerUp(PointerEventData a_EventData)
        {
            if (m_PointerEventHandler != null)
            {
                m_PointerEventHandler.OnPointer(this, a_EventData, EPointerEventType.kPointerUp);
            }
        }

        public void OnPointerMove(PointerEventData a_EventData)
        {
            if (m_PointerEventHandler != null)
            {
                m_PointerEventHandler.OnPointer(this, a_EventData, EPointerEventType.kPointerMove);
            }
        }

        public void SetPointerEventHandler(IPointerEventHandler<CTile> a_PointerEventHandler)
        {
            m_PointerEventHandler = a_PointerEventHandler;
        }
    }
}
