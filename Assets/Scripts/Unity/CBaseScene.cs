using UnityEngine;
using sdw.unity;

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
    public class CBaseScene : MonoBehaviour
    {
        [SerializeField]
        protected RectTransform m_AlertPanelRectTransform = null;
        [SerializeField]
        protected CAlertDlg m_AlertDlg = null;

        public CBaseScene()
        {
            CEngineDefault.Init();
        }

        private void Awake()
        {
            Application.targetFrameRate = 60;

            if (m_AlertPanelRectTransform == null)
            {
                Debug.LogError("set AlertPanelRectTransform first");
                return;
            }
            if (m_AlertDlg == null)
            {
                Debug.LogError("set AlertDlg first");
                return;
            }
        }

        protected void update()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                onClickBack();
            }
#endif
        }

        private void onClickBack()
        {
            CAlertDlg alertDlg = dynamic_cast<CAlertDlg>(CDialogManager.Instance.GetCurrentDialog());
            if (alertDlg == null)
            {
                alertDlg = Instantiate(m_AlertDlg, m_AlertPanelRectTransform);
                alertDlg.SetAlertType(CAlertDlg.EAlertType.kConfirm);
                alertDlg.SetCallback(onClickAlertDlgYes, onClickAlertDlgNo);
                alertDlg.SetText("退出游戏后，本局游戏将直接结束无法恢复，确定是否退出？");
                CDialogManager.Instance.ShowDialog(alertDlg);
            }
            else
            {
                CDialogManager.Instance.CloseCurrentDialog();
            }
        }

        private void onClickAlertDlgYes()
        {
            CApplication.Quit();
        }

        private void onClickAlertDlgNo()
        {
            CDialogManager.Instance.CloseCurrentDialog();
        }
    }
}
