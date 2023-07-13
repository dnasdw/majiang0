using DG.Tweening;
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
    public class CSettingDlg : MonoBehaviour
    {
        [SerializeField]
        private RectTransform m_RootRectTransform = null;
        [SerializeField]
        private Slider m_VolumeSlider = null;
        [SerializeField]
        private Toggle m_CheatToggle = null;

        private IStorageManager m_StorageManager = null;
        private IAudioSource m_AudioSource = null;

        public void Init(IStorageManager a_StorageManager, IAudioSource a_AudioSource)
        {
            m_StorageManager = a_StorageManager;
            m_AudioSource = a_AudioSource;
        }

        private void Awake()
        {
            if (m_RootRectTransform == null)
            {
                Debug.LogError("set RootRectTransform first");
                return;
            }
            if (m_VolumeSlider == null)
            {
                Debug.LogError("set VolumeSlider first");
                return;
            }
            if (m_CheatToggle == null)
            {
                Debug.LogError("set CheatToggle first");
                return;
            }
        }

        // Start is called before the first frame update
        private void Start()
        {
            m_RootRectTransform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
            m_RootRectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutElastic);
            m_VolumeSlider.value = CGameData.m_GameState.Volume;
            m_CheatToggle.isOn = CGameData.m_GameState.IsShowAllMahjong;
        }

        // Update is called once per frame
        private void Update()
        {
        }

        public void OnClickClose()
        {
            CDialogManager.Instance.CloseCurrentDialog();
        }

        public void OnValueChangedVolume()
        {
            CGameData.m_GameState.Volume = m_VolumeSlider.value;
            if (m_AudioSource != null)
            {
                m_AudioSource.SetVolume(CGameData.m_GameState.Volume);
            }
            m_StorageManager.SetGameState(CGameData.m_GameState);
        }

        public void OnValueChangedCheat()
        {
            CGameData.m_GameState.IsShowAllMahjong = m_CheatToggle.isOn;
            CDebugValue.SetIsShowAllMahjong(CGameData.m_GameState.IsShowAllMahjong);
            m_StorageManager.SetGameState(CGameData.m_GameState);
        }
    }
}
