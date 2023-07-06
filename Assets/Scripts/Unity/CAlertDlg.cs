using DG.Tweening;
using TMPro;
using UnityEngine;
using sdw.game;

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
    public class CAlertDlg : MonoBehaviour
    {
        public enum EAlertType
        {
            kAlert,
            kConfirm,
            kMax
        }

        [SerializeField]
        private RectTransform m_RootRectTransform = null;
        [SerializeField]
        private TextMeshProUGUI m_ContentText = null;
        [SerializeField]
        private TextMeshProUGUI m_TitleText = null;
        [SerializeField]
        private RectTransform m_OneRectTransform = null;
        [SerializeField]
        private RectTransform m_TwoRectTransform = null;

        private CCallbackVoid m_CallbackYes = null;
        private CCallbackVoid m_CallbackNo = null;

        private void Awake()
        {
            if (m_RootRectTransform == null)
            {
                Debug.LogError("set RootRectTransform first");
                return;
            }
            if (m_ContentText == null)
            {
                Debug.LogError("set ContentText first");
                return;
            }
            if (m_TitleText == null)
            {
                Debug.LogError("set TitleText first");
                return;
            }
            if (m_OneRectTransform == null)
            {
                Debug.LogError("set OneRectTransform first");
                return;
            }
            if (m_TwoRectTransform == null)
            {
                Debug.LogError("set TwoRectTransform first");
                return;
            }
        }

        // Start is called before the first frame update
        private void Start()
        {
            m_RootRectTransform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
            m_RootRectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutElastic);
        }

        // Update is called once per frame
        private void Update()
        {
        }

        public void OnClickYes()
        {
            if (m_CallbackYes != null)
            {
                m_CallbackYes();
            }
        }

        public void OnClickNo()
        {
            if (m_CallbackNo != null)
            {
                m_CallbackNo();
            }
        }

        public void SetAlertType(EAlertType a_eAlertType)
        {
            switch (a_eAlertType)
            {
            case EAlertType.kAlert:
                m_OneRectTransform.gameObject.SetActive(true);
                m_TwoRectTransform.gameObject.SetActive(false);
                break;
            case EAlertType.kConfirm:
                m_OneRectTransform.gameObject.SetActive(false);
                m_TwoRectTransform.gameObject.SetActive(true);
                break;
            default:
                break;
            }
        }

        public void SetText(string a_sContent, string a_sTitle = "ÌבÊ¾")
        {
            m_ContentText.text = a_sContent;
            m_TitleText.text = a_sTitle;
        }

        public void SetCallback(CCallbackVoid a_CallbackYes, CCallbackVoid a_CallbackNo)
        {
            m_CallbackYes = a_CallbackYes;
            m_CallbackNo = a_CallbackNo;
        }
    }
}
