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
    public class CDialogManager
    {
        public static CDialogManager Instance { get; } = CSpecializer<CDialogManager>.GetDefaultValue();

        private stackp<MonoBehaviour> m_sDialogList = new stackp<MonoBehaviour>();

        public void ShowDialog(MonoBehaviour a_Dialog)
        {
            if (a_Dialog == null)
            {
                return;
            }
            if (!m_sDialogList.empty())
            {
                MonoBehaviour currentDialog = m_sDialogList.top();
                if (currentDialog != null)
                {
                    currentDialog.gameObject.SetActive(false);
                }
            }
            m_sDialogList.push(a_Dialog);
        }

        public void CloseAllDialog()
        {
            while (!m_sDialogList.empty())
            {
                MonoBehaviour currentDialog = m_sDialogList.top();
                m_sDialogList.pop();
                if (currentDialog != null)
                {
                    currentDialog.gameObject.SetActive(false);
                    Object.Destroy(currentDialog.gameObject);
                }
            }
        }

        public void CloseCurrentDialog()
        {
            if (!m_sDialogList.empty())
            {
                MonoBehaviour currentDialog = m_sDialogList.top();
                m_sDialogList.pop();
                if (currentDialog != null)
                {
                    currentDialog.gameObject.SetActive(false);
                    Object.Destroy(currentDialog.gameObject);
                }
                if (!m_sDialogList.empty())
                {
                    currentDialog = m_sDialogList.top();
                    if (currentDialog != null)
                    {
                        currentDialog.gameObject.SetActive(true);
                    }
                }
            }
        }

        public MonoBehaviour GetCurrentDialog()
        {
            MonoBehaviour currentDialog = null;
            if (!m_sDialogList.empty())
            {
                currentDialog = m_sDialogList.top();
            }
            return currentDialog;
        }
    }
}
