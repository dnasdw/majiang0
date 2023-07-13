using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using sdw.game;
using sdw.unity;

using static majiang0.CGameCmd;
using static majiang0.CGameLogicConst;

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
    public class CGamePanel : CBaseScene, IAudioSource, IGameEngineEventListener, IPointerEventHandler<CTile>
    {
        private const bool s_bCheatData = true;

        private const n32 s_nUserOpMaxTime = 30;

        [SerializeField]
        private AudioSource m_AudioSource = null;
        [SerializeField]
        private GameObject[] m_WheelGameObject = new GameObject[GAME_PLAYER];
        [SerializeField]
        private Image m_Timer0Image = null;
        [SerializeField]
        private Image m_Timer1Image = null;
        [SerializeField]
        private RectTransform[] m_DiscardCardRectTransform = new RectTransform[GAME_PLAYER];
        public Image[] m_DiscardCardImage = new Image[GAME_PLAYER];
        [SerializeField]
        private RectTransform m_SignAnimRectTransform = null;
        [SerializeField]
        private CBindTing m_Ting = null;
        public CBindMeld m_Gang0Meld = null;
        public CBindMeld m_Gang1Meld = null;
        public CBindMeld m_Peng0Meld = null;
        public CBindMeld m_Peng1Meld = null;
        [SerializeField]
        private CTile m_HandCard0Tile = null;
        [SerializeField]
        private Image[] m_HandCardImage = new Image[GAME_PLAYER - 1];
        [SerializeField]
        private RectTransform[] m_RecvCardRectTransform = new RectTransform[GAME_PLAYER];
        [SerializeField]
        private COperateButton m_GuoButton = null;
        [SerializeField]
        private COperateButton m_PengButton = null;
        [SerializeField]
        private COperateButton m_GangButton = null;
        [SerializeField]
        private COperateButton m_HuButton = null;
        [SerializeField]
        private RectTransform m_EffectPanelRectTransform = null;
        [SerializeField]
        private RectTransform m_EffectPengRectTransform = null;
        [SerializeField]
        private RectTransform m_EffectGangRectTransform = null;
        [SerializeField]
        private RectTransform m_EffectHuRectTransform = null;
        [SerializeField]
        private RectTransform m_EffectZmRectTransform = null;
        [SerializeField]
        private RectTransform m_SettingPanelRectTransform = null;
        [SerializeField]
        private CSettingDlg m_SettingDlg = null;
        [SerializeField]
        private RectTransform m_GameOverPanelRectTransform = null;
        [SerializeField]
        private CGameOverDlg m_GameOverDlg = null;

        [SerializeField]
        private CBindFaceFrame[] m_FaceFrame = new CBindFaceFrame[GAME_PLAYER];
        [SerializeField]
        private CBindPlayerPanel[] m_PlayerPanel = new CBindPlayerPanel[GAME_PLAYER];
        [SerializeField]
        private RectTransform m_OperateNotifyGroup = null;
        [SerializeField]
        private TextMeshProUGUI m_CardNumText = null;

        private Coroutine m_AIEnterGameCoroutine = null;
        private Coroutine m_SendCardTimerUpdateCoroutine = null;

        private RectTransform m_SignAnim = null;

        private n32? m_nPointerId = null;
        private bool m_bTileBack = false;

        private CStorageManager m_StorageManager = new CStorageManager();

        private CGameEngine m_GameEngine = CGameEngine.Instance;
        private CTile m_OutCard = null;
        private n32 m_nMeChairID = 0;
        private n32 m_nOutCardTimeOut = s_nUserOpMaxTime;

        private CWeaveItem[][] m_WeaveItemArray = CJaggedArray.CreateInstance<CWeaveItem[][], CWeaveItem>(GAME_PLAYER, MAX_WEAVE);
        private u8[][] m_uCardIndex = CJaggedArray.CreateInstance<u8[][]>(GAME_PLAYER, MAX_INDEX);
        private n32[] m_nWeaveItemCount = new n32[GAME_PLAYER];
        private n32[] m_nDiscardCount = new n32[GAME_PLAYER];
        private u8[][] m_uDiscardCard = CJaggedArray.CreateInstance<u8[][]>(GAME_PLAYER, MAX_DISCARD);
        private n32 m_nLeftCardCount = 0;
        private n32 m_nBankerChair = INVALID_CHAIR;
        private bool m_bOperate = false;
        private bool m_bMove = false;
        private Vector2 m_StartVec = new Vector2(0, 0);
        private const n32 s_nOutY = 30;
        private const n32 s_nCardPosY = 0;

        public CGamePanel()
        {
            m_GameEngine = CGameEngine.Instance;
            m_bOperate = false;
            m_bMove = false;
            m_OutCard = null;
            m_nOutCardTimeOut = s_nUserOpMaxTime;
            m_nMeChairID = 0;
            InitGame();
        }

        public CGameEngine GetGameEngine()
        {
            return m_GameEngine;
        }

        public n32 GetMeChairID()
        {
            return m_nMeChairID;
        }

        public n32 GetBankerChair()
        {
            return m_nBankerChair;
        }

        private void Awake()
        {
            m_GameEngine.Init(m_StorageManager, this);
            if (m_AudioSource == null)
            {
                Debug.LogError("set AudioSource first");
                return;
            }
            if (m_WheelGameObject.Length != GAME_PLAYER)
            {
                Debug.LogError("set WheelGameObject first");
                return;
            }
            for (n32 i = 0; i < m_WheelGameObject.Length; i++)
            {
                if (m_WheelGameObject[i] == null)
                {
                    Debug.LogError("set WheelGameObject first");
                    return;
                }
            }
            if (m_Timer0Image == null)
            {
                Debug.LogError("set Timer0Image first");
                return;
            }
            if (m_Timer1Image == null)
            {
                Debug.LogError("set Timer1Image first");
                return;
            }
            if (m_DiscardCardRectTransform.Length != GAME_PLAYER)
            {
                Debug.LogError("set DiscardCardRectTransform first");
                return;
            }
            for (n32 i = 0; i < m_DiscardCardRectTransform.Length; i++)
            {
                if (m_DiscardCardRectTransform[i] == null)
                {
                    Debug.LogError("set DiscardCardRectTransform first");
                    return;
                }
            }
            if (m_DiscardCardImage.Length != GAME_PLAYER)
            {
                Debug.LogError("set DiscardCardImage first");
                return;
            }
            for (n32 i = 0; i < m_DiscardCardImage.Length; i++)
            {
                if (m_DiscardCardImage[i] == null)
                {
                    Debug.LogError("set DiscardCardImage first");
                    return;
                }
            }
            if (m_SignAnimRectTransform == null)
            {
                Debug.LogError("set SignAnimRectTransform first");
                return;
            }
            if (m_Ting == null)
            {
                Debug.LogError("set Ting first");
                return;
            }
            if (m_Gang0Meld == null)
            {
                Debug.LogError("set Gang0Meld first");
                return;
            }
            if (m_Gang1Meld == null)
            {
                Debug.LogError("set Gang1Meld first");
                return;
            }
            if (m_Peng0Meld == null)
            {
                Debug.LogError("set Peng0Meld first");
                return;
            }
            if (m_Peng1Meld == null)
            {
                Debug.LogError("set Peng1Meld first");
                return;
            }
            if (m_HandCard0Tile == null)
            {
                Debug.LogError("set HandCard0Tile first");
                return;
            }
            if (m_HandCardImage.Length != GAME_PLAYER - 1)
            {
                Debug.LogError("set HandCardImage first");
                return;
            }
            for (n32 i = 0; i < m_HandCardImage.Length; i++)
            {
                if (m_HandCardImage[i] == null)
                {
                    Debug.LogError("set HandCardImage first");
                    return;
                }
            }
            if (m_RecvCardRectTransform.Length != GAME_PLAYER)
            {
                Debug.LogError("set RecvCardRectTransform first");
                return;
            }
            for (n32 i = 0; i < m_RecvCardRectTransform.Length; i++)
            {
                if (m_RecvCardRectTransform[i] == null)
                {
                    Debug.LogError("set RecvCardRectTransform first");
                    return;
                }
            }
            if (m_GuoButton == null)
            {
                Debug.LogError("set GuoButton first");
                return;
            }
            if (m_PengButton == null)
            {
                Debug.LogError("set PengButton first");
                return;
            }
            if (m_GangButton == null)
            {
                Debug.LogError("set GangButton first");
                return;
            }
            if (m_GangButton.m_Image == null)
            {
                Debug.LogError("set GangButton.Image first");
                return;
            }
            if (m_HuButton == null)
            {
                Debug.LogError("set HuButton first");
                return;
            }
            if (m_EffectPanelRectTransform == null)
            {
                Debug.LogError("set EffectPanelRectTransform first");
                return;
            }
            if (m_EffectPengRectTransform == null)
            {
                Debug.LogError("set EffectPengRectTransform first");
                return;
            }
            if (m_EffectGangRectTransform == null)
            {
                Debug.LogError("set EffectGangRectTransform first");
                return;
            }
            if (m_EffectHuRectTransform == null)
            {
                Debug.LogError("set EffectHuRectTransform first");
                return;
            }
            if (m_EffectZmRectTransform == null)
            {
                Debug.LogError("set EffectZmRectTransform first");
                return;
            }
            if (m_SettingPanelRectTransform == null)
            {
                Debug.LogError("set SettingPanelRectTransform first");
                return;
            }
            if (m_SettingDlg == null)
            {
                Debug.LogError("set SettingDlg first");
                return;
            }
            if (m_GameOverPanelRectTransform == null)
            {
                Debug.LogError("set GameOverPanelRectTransform first");
                return;
            }
            if (m_GameOverDlg == null)
            {
                Debug.LogError("set GameOverDlg first");
                return;
            }
            if (m_FaceFrame.Length != GAME_PLAYER)
            {
                Debug.LogError("set FaceFrame first");
                return;
            }
            for (n32 i = 0; i < m_FaceFrame.Length; i++)
            {
                if (m_FaceFrame[i] == null)
                {
                    Debug.LogError("set FaceFrame first");
                    return;
                }
                else
                {
                    m_FaceFrame[i].m_ScoreText.text = "";
                }
            }
            if (m_PlayerPanel.Length != GAME_PLAYER)
            {
                Debug.LogError("set PlayerPanel first");
                return;
            }
            for (n32 i = 0; i < m_PlayerPanel.Length; i++)
            {
                if (m_PlayerPanel[i] == null)
                {
                    Debug.LogError("set PlayerPanel first");
                    return;
                }
                else
                {
                    m_PlayerPanel[i].m_HandCardRectTransform.DestroyAllChildren();
                }
            }
            if (m_OperateNotifyGroup == null)
            {
                Debug.LogError("set OperateNotifyGroup first");
                return;
            }
            if (m_CardNumText == null)
            {
                Debug.LogError("set CardNumText first");
                return;
            }
            else
            {
                m_CardNumText.text = "";
            }
        }

        // Start is called before the first frame update
        private void Start()
        {
            CPlayer realPlayer = new CPlayer(false, CPlayer.EPlayerSex.kMale, this);
            m_GameEngine.OnUserEnter(realPlayer);
            m_nMeChairID = realPlayer.GetChairID();
            m_AIEnterGameCoroutine = StartCoroutine(aiEnterGame(1.0f, 1.0f));
        }

        // Update is called once per frame
        private void Update()
        {
            update();
        }

        public void OnClickExit()
        {
            CAlertDlg alertDlg = Instantiate(m_AlertDlg, m_AlertPanelRectTransform);
            alertDlg.SetAlertType(CAlertDlg.EAlertType.kConfirm);
            alertDlg.SetCallback(onClickExitAlertDlgYes, onClickExitAlertDlgNo);
            alertDlg.SetText("退出游戏后，本局游戏将直接结束无法恢复，确定是否退出？");
            CDialogManager.Instance.ShowDialog(alertDlg);
        }

        public void OnClickSet()
        {
            CSettingDlg settingDlg = Instantiate(m_SettingDlg, m_SettingPanelRectTransform);
            settingDlg.Init(m_StorageManager, this);
            CDialogManager.Instance.ShowDialog(settingDlg);
        }

        private void onClickExitAlertDlgYes()
        {
            CApplication.Quit();
        }

        private void onClickExitAlertDlgNo()
        {
            CDialogManager.Instance.CloseAllDialog();
        }

        private void OnApplicationPause(bool a_bPause)
        {
            //print("OnApplicationPause: " + a_bPause);
            if (a_bPause)
            {
                saveAll();
            }
        }

        private void OnApplicationQuit()
        {
            saveAll();
        }

        private void saveAll()
        {
            if (m_StorageManager != null)
            {
                m_StorageManager.SaveAll();
            }
        }

        public void SetVolume(f32 a_fVolume)
        {
            if (m_AudioSource != null)
            {
                m_AudioSource.volume = clamp(a_fVolume, 0.0f, 1.0f);
            }
        }

        private IEnumerator aiEnterGame(f32 a_fTime, f32 a_fRepeatRate)
        {
            if (a_fTime != 0.0f)
            {
                yield return new WaitForSeconds(a_fTime);
            }
            WaitForSeconds wait = new WaitForSeconds(a_fRepeatRate);
            do
            {
                CPlayer aiPlayer = new CPlayer(true, DateTime.Now.Second % 2 == 0 ? CPlayer.EPlayerSex.kFemale : CPlayer.EPlayerSex.kMale, new CAIEngine(this));
                if (!m_GameEngine.OnUserEnter(aiPlayer))
                {
                    this.SafeStopCoroutine(ref m_AIEnterGameCoroutine);
                    break;
                }
                yield return wait;
            } while (true);
        }

        public void InitGame()
        {
            m_nLeftCardCount = 0;
            m_nBankerChair = INVALID_CHAIR;
            m_nWeaveItemCount.RecursiveClear();
            m_nDiscardCount.RecursiveClear();
            m_uCardIndex.RecursiveClear();
            m_WeaveItemArray.RecursiveClear<CWeaveItem>();
            m_uDiscardCard.RecursiveClear();
        }

        public IEnumerator SendCardTimerUpdate(f32 a_fTime, f32 a_fRepeatRate)
        {
            if (a_fTime != 0.0f)
            {
                yield return new WaitForSeconds(a_fTime);
            }
            WaitForSeconds wait = new WaitForSeconds(a_fRepeatRate);
            do
            {
                m_nOutCardTimeOut = m_nOutCardTimeOut-- < 1 ? 0 : m_nOutCardTimeOut;
                n32 nTime0 = m_nOutCardTimeOut / 10;
                n32 nTime1 = m_nOutCardTimeOut % 10;
                Addressables.LoadAssetAsync<Sprite>(Format("Assets/Textures/Game/timer_{0:d}.png", nTime0)).Completed += (AsyncOperationHandle<Sprite> sprite) =>
                {
                    if (sprite.Status == AsyncOperationStatus.Succeeded)
                    {
                        m_Timer0Image.sprite = sprite.Result;
                    }
                };
                Addressables.LoadAssetAsync<Sprite>(Format("Assets/Textures/Game/timer_{0:d}.png", nTime1)).Completed += (AsyncOperationHandle<Sprite> sprite) =>
                {
                    if (sprite.Status == AsyncOperationStatus.Succeeded)
                    {
                        m_Timer1Image.sprite = sprite.Result;
                    }
                };
                if (m_nOutCardTimeOut == 0)
                {
                    this.SafeStopCoroutine(ref m_SendCardTimerUpdateCoroutine);
                    break;
                }
                yield return wait;
            } while (true);
        }

        public void OnPointer(CTile a_Object, PointerEventData a_EventData, EPointerEventType a_ePointerEventType)
        {
            if (m_bOperate)
            {
                if (m_OutCard != null && m_OutCard != a_Object)
                {
                    return;
                }
                n32 nPointerId = a_EventData.pointerId;
                switch (a_ePointerEventType)
                {
                case EPointerEventType.kPointerDown:
                    if (m_nPointerId.HasValue)
                    {
                        m_OutCard = null;
                        m_nPointerId = null;
                        a_Object.transform.localPosition = m_StartVec;
                        return;
                    }
                    else
                    {
                        m_nPointerId = nPointerId;
                    }
                    break;
                case EPointerEventType.kPointerUp:
                case EPointerEventType.kPointerMove:
                    if (!m_nPointerId.HasValue || m_nPointerId.Value != nPointerId)
                    {
                        return;
                    }
                    break;
                }
                Transform card = a_Object.transform;
                if (card != null)
                {
                    if (a_ePointerEventType == EPointerEventType.kPointerDown || a_ePointerEventType == EPointerEventType.kPointerUp)
                    {
                        mapp<n32, CTile> mOrderedTile = new mapp<n32, CTile>();
                        RectTransform handCard = m_PlayerPanel[0].m_HandCardRectTransform;
                        n32 nChildCount = handCard.childCount;
                        for (n32 i = 0; i < nChildCount; i++)
                        {
                            CTile child = handCard.GetChild(i).GetComponent<CTile>();
                            if (child != a_Object)
                            {
                                mOrderedTile.insert(make_pair(child.m_nTileIndex, child));
                            }
                        }
                        for (IEnumerator<KeyValuePair<n32, CTile>> it = mOrderedTile.GetEnumerator(); it.MoveNext(); /**/)
                        {
                            CTile child = it.Current.Value;
                            child.transform.localPosition = new Vector2(child.transform.localPosition.x, 0);
                            child.transform.SetAsLastSibling();
                        }
                        RectTransform recvCard = m_RecvCardRectTransform[0];
                        nChildCount = recvCard.childCount;
                        for (n32 i = 0; i < nChildCount; i++)
                        {
                            CTile child = recvCard.GetChild(i).GetComponent<CTile>();
                            if (child != a_Object)
                            {
                                child.transform.localPosition = new Vector2(child.transform.localPosition.x, 0);
                            }
                        }
                        card.SetAsLastSibling();
                        card.parent.SetAsLastSibling();
                        handCard.parent.parent.SetAsLastSibling();
                    }
                    string sCardName = card.name;
                    switch (a_ePointerEventType)
                    {
                    case EPointerEventType.kPointerDown:
                        {
                            m_OutCard = a_Object;
                            m_bMove = false;
                            m_StartVec = card.localPosition;
                            Vector2 pressPosition = default(Vector2);
                            RectTransformUtility.ScreenPointToLocalPointInRectangle(reinterpret_cast<RectTransform>(m_PlayerPanel[0].transform), a_EventData.pressPosition, null, out pressPosition);
                            m_bTileBack = pressPosition.y > 118;
                            u8 uCardData = a_Object.m_uTileData;
                            n32 nWeaveItemCount = m_nWeaveItemCount[m_nMeChairID];
                            CWeaveItem[] weaveItem = m_WeaveItemArray[m_nMeChairID];
                            u8[] uTingCard = new u8[MAX_INDEX];
                            u8[] uCardIndex = m_uCardIndex[m_nMeChairID];
                            Array.Copy(uCardIndex, uTingCard, uTingCard.Length);
                            uTingCard[CGameLogic.SwitchToCardIndex(uCardData)]--;
                            showTingResult(uTingCard, weaveItem, nWeaveItemCount);
                        }
                        break;
                    case EPointerEventType.kPointerUp:
                        {
                            m_OutCard = null;
                            m_nPointerId = null;
                            if (card.localPosition.y > 118)
                            {
                                Debug.Log("out card");
                                m_bOperate = false;
                                CCMD_C_OutCard COutCard = new CCMD_C_OutCard();
                                COutCard.CardData = a_Object.m_uTileData;
                                m_GameEngine.OnUserOutCard(COutCard);
                            }
                            else
                            {
                                if (m_StartVec.y == s_nCardPosY)
                                {
                                    if (m_bMove)
                                    {
                                        card.localPosition = m_StartVec;
                                    }
                                    else
                                    {
                                        card.localPosition = m_StartVec + new Vector2(0, s_nOutY);
                                    }
                                }
                                else if (m_StartVec.y == s_nCardPosY + s_nOutY)
                                {
                                    if (m_bTileBack)
                                    {
                                        card.localPosition = m_StartVec - new Vector2(0, s_nOutY);
                                    }
                                    else
                                    {
                                        card.localPosition = m_StartVec;
                                        m_bOperate = false;
                                        CCMD_C_OutCard COutCard = new CCMD_C_OutCard();
                                        COutCard.CardData = a_Object.m_uTileData;
                                        m_GameEngine.OnUserOutCard(COutCard);
                                    }
                                }
                                else
                                {
                                    card.localPosition = m_StartVec;
                                }
                            }
                        }
                        break;
                    case EPointerEventType.kPointerMove:
                        {
                            f32 fDeltaX = a_EventData.position.x - a_EventData.pressPosition.x;
                            f32 fDeltaY = a_EventData.position.y - a_EventData.pressPosition.y;
                            card.localPosition = m_StartVec + new Vector2(fDeltaX, fDeltaY);
                            if (fDeltaY > 60 || abs(fDeltaX) > 30)
                            {
                                m_bMove = true;
                            }
                        }
                        break;
                    }
                }
            }
        }

        public void SetPlayer(CPlayer a_Player)
        {
            // do nothing
        }

        public bool OnUserEnterEvent(CPlayer a_Player)
        {
            if (m_GameEngine.GetPlayerCount() >= GAME_PLAYER)
            {
                this.SafeStopCoroutine(ref m_AIEnterGameCoroutine);
            }
            if (a_Player != null)
            {
                n32 nChairID = a_Player.GetChairID();
                if (nChairID >= 0 && nChairID < GAME_PLAYER)
                {
                    CBindFaceFrame faceFrame = m_FaceFrame[nChairID];
                    if (faceFrame != null)
                    {
                        Image headImage = faceFrame.m_HeadImage;
                        faceFrame.m_ScoreText.text = Format("{0:d}", CGameData.m_GameState.Score[nChairID]);
                        Addressables.LoadAssetAsync<Sprite>(Format("Assets/Textures/Game/im_defaulthead_{0:d}.png", a_Player.GetSex())).Completed += (AsyncOperationHandle<Sprite> sprite) =>
                        {
                            if (sprite.Status == AsyncOperationStatus.Succeeded)
                            {
                                headImage.sprite = sprite.Result;
                            }
                        };
                    }
                }
            }
            return true;
        }

        public bool OnGameStartEvent(CCMD_S_GameStart a_SGameStart)
        {
            Debug.Log("接收到游戏开始事件");
            InitGame();
            m_FaceFrame[0].transform.DOLocalMove(new Vector2(80 - 640, 250 - 360), 0.5f);
            m_FaceFrame[1].transform.DOLocalMove(new Vector2(80 - 640, 380 - 360), 0.5f);
            m_FaceFrame[2].transform.DOLocalMove(new Vector2(1060 - 640, 640 - 360), 0.5f);
            m_FaceFrame[3].transform.DOLocalMove(new Vector2(1200 - 640, 380 - 360), 0.5f);
            m_nLeftCardCount = a_SGameStart.LeftCardCount;
            m_nBankerChair = a_SGameStart.BankerUser;
            CGameLogic.SwitchToCardIndex(new ArraySegment<u8>(a_SGameStart.CardData, MAX_COUNT * m_nMeChairID, MAX_COUNT), MAX_COUNT - 1, m_uCardIndex[m_nMeChairID]);
            if (s_bCheatData)
            {
                for (n32 i = 0; i < GAME_PLAYER; i++)
                {
                    CGameLogic.SwitchToCardIndex(new ArraySegment<u8>(a_SGameStart.CardData, MAX_COUNT * i, MAX_COUNT), MAX_COUNT - 1, m_uCardIndex[i]);
                }
            }
            m_CardNumText.text = Format("{0:d}", m_nLeftCardCount);
            showAndUpdateHandCard();
            showAndUpdateDiscardCard();
            return true;
        }

        public bool OnSendCardEvent(CCMD_S_SendCard a_SSendCard)
        {
            m_nOutCardTimeOut = s_nUserOpMaxTime;
            m_nLeftCardCount--;
            if (a_SSendCard.CurrentUser == m_nMeChairID || s_bCheatData)
            {
                m_uCardIndex[a_SSendCard.CurrentUser][CGameLogic.SwitchToCardIndex(a_SSendCard.CardData)]++;
            }
            return showSendCard(a_SSendCard);
        }

        public bool OnOutCardEvent(CCMD_S_OutCard a_SOutCard)
        {
            if (a_SOutCard.OutCardUser == m_nMeChairID || s_bCheatData)
            {
                m_uCardIndex[a_SOutCard.OutCardUser][CGameLogic.SwitchToCardIndex(a_SOutCard.OutCardData)]--;
            }
            m_uDiscardCard[a_SOutCard.OutCardUser][m_nDiscardCount[a_SOutCard.OutCardUser]++] = a_SOutCard.OutCardData;
            n32 nViewID = m_GameEngine.SwitchViewChairID(a_SOutCard.OutCardUser, m_nMeChairID);
            if (m_SignAnim != null)
            {
                Destroy(m_SignAnim.gameObject);
                m_SignAnim = null;
            }
            RectTransform recvCard = m_RecvCardRectTransform[nViewID];
            if (nViewID != 0)
            {
                recvCard.gameObject.SetActive(false);
            }
            if (nViewID == 0 || CDebugValue.IsShowAllMahjong())
            {
                showAndUpdateHandCard();
            }
            mapp<n32, Image> mOrederedImage = new mapp<n32, Image>();
            switch (nViewID)
            {
            case 0:
                {
                    recvCard.DestroyAllChildren();
                    RectTransform discardCard = m_DiscardCardRectTransform[nViewID];
                    discardCard.DestroyAllChildren();
                    n32 nDiscardCount = m_nDiscardCount[a_SOutCard.OutCardUser];
                    n32 nX = 0;
                    n32 nY = 0;
                    for (n32 i = 0; i < nDiscardCount; i++)
                    {
                        n32 nCol = i % 12;
                        n32 nRow = i / 12;
                        nX = nCol == 0 ? 0 : nX;
                        nY = nRow * 90;
                        Image card = Instantiate(m_DiscardCardImage[nViewID], m_DiscardCardRectTransform[nViewID]);
                        Addressables.LoadAssetAsync<Sprite>(CUIHelper.GetDiscardCardImagePath(nViewID, m_uDiscardCard[a_SOutCard.OutCardUser][i])).Completed += (AsyncOperationHandle<Sprite> sprite) =>
                        {
                            if (sprite.Status == AsyncOperationStatus.Succeeded && card != null)
                            {
                                card.sprite = sprite.Result;
                                card.gameObject.SetActive(true);
                            }
                        };
                        card.rectTransform.localPosition = new Vector2(nX, nY);
                        mOrederedImage.insert(make_pair(-nRow * 12 + nCol, card));
                        nX += 76;
                        if (i == nDiscardCount - 1)
                        {
                            m_SignAnim = Instantiate(m_SignAnimRectTransform, card.rectTransform);
                            m_SignAnim.localPosition = new Vector2(39, 110);
                        }
                    }
                    n32 nWeaveItemCount = m_nWeaveItemCount[a_SOutCard.OutCardUser];
                    CWeaveItem[] weaveItem = m_WeaveItemArray[a_SOutCard.OutCardUser];
                    u8[] uCardIndex = m_uCardIndex[m_nMeChairID];
                    showTingResult(uCardIndex, weaveItem, nWeaveItemCount);
                }
                break;
            case 1:
                {
                    RectTransform discardCard = m_DiscardCardRectTransform[nViewID];
                    discardCard.DestroyAllChildren();
                    n32 nDiscardCount = m_nDiscardCount[a_SOutCard.OutCardUser];
                    n32 nX = 0;
                    n32 nY = 0;
                    for (n32 i = 0; i < nDiscardCount; i++)
                    {
                        n32 nCol = i % 11;
                        n32 nRow = i / 11;
                        nY = nCol == 0 ? 0 : nY;
                        nX = 116 * nRow;
                        Image card = Instantiate(m_DiscardCardImage[nViewID], m_DiscardCardRectTransform[nViewID]);
                        Addressables.LoadAssetAsync<Sprite>(CUIHelper.GetDiscardCardImagePath(nViewID, m_uDiscardCard[a_SOutCard.OutCardUser][i])).Completed += (AsyncOperationHandle<Sprite> sprite) =>
                        {
                            if (sprite.Status == AsyncOperationStatus.Succeeded && card != null)
                            {
                                card.sprite = sprite.Result;
                                card.gameObject.SetActive(true);
                            }
                        };
                        card.rectTransform.localPosition = new Vector2(nX, 740 - nY);
                        mOrederedImage.insert(make_pair(nCol + nRow * 11, card));
                        nY += 74;
                        if (i == nDiscardCount - 1)
                        {
                            m_SignAnim = Instantiate(m_SignAnimRectTransform, card.rectTransform);
                            m_SignAnim.localPosition = new Vector2(81, 110);
                        }
                    }
                }
                break;
            case 2:
                {
                    RectTransform discardCard = m_DiscardCardRectTransform[nViewID];
                    discardCard.DestroyAllChildren();
                    n32 nDiscardCount = m_nDiscardCount[a_SOutCard.OutCardUser];
                    n32 nX = 0;
                    n32 nY = 0;
                    for (n32 i = 0; i < nDiscardCount; i++)
                    {
                        n32 nCol = i % 12;
                        n32 nRow = i / 12;
                        nX = nCol == 0 ? 0 : nX;
                        nY = 90 - nRow * 90;
                        Image card = Instantiate(m_DiscardCardImage[nViewID], m_DiscardCardRectTransform[nViewID]);
                        Addressables.LoadAssetAsync<Sprite>(CUIHelper.GetDiscardCardImagePath(nViewID, m_uDiscardCard[a_SOutCard.OutCardUser][i])).Completed += (AsyncOperationHandle<Sprite> sprite) =>
                        {
                            if (sprite.Status == AsyncOperationStatus.Succeeded && card != null)
                            {
                                card.sprite = sprite.Result;
                                card.gameObject.SetActive(true);
                            }
                        };
                        card.rectTransform.localPosition = new Vector2(nX, nY);
                        mOrederedImage.insert(make_pair(nRow * 12 + nCol, card));
                        nX += 76;
                        if (i == nDiscardCount - 1)
                        {
                            m_SignAnim = Instantiate(m_SignAnimRectTransform, card.rectTransform);
                            m_SignAnim.localPosition = new Vector2(39, 59);
                        }
                    }
                }
                break;
            case 3:
                {
                    RectTransform discardCard = m_DiscardCardRectTransform[nViewID];
                    discardCard.DestroyAllChildren();
                    n32 nDiscardCount = m_nDiscardCount[a_SOutCard.OutCardUser];
                    n32 nX = 0;
                    n32 nY = 0;
                    for (n32 i = 0; i < nDiscardCount; i++)
                    {
                        n32 nCol = i % 11;
                        n32 nRow = i / 11;
                        nY = nCol == 0 ? 0 : nY;
                        nX = 240 - 116 * nRow;
                        Image card = Instantiate(m_DiscardCardImage[nViewID], m_DiscardCardRectTransform[nViewID]);
                        Addressables.LoadAssetAsync<Sprite>(CUIHelper.GetDiscardCardImagePath(nViewID, m_uDiscardCard[a_SOutCard.OutCardUser][i])).Completed += (AsyncOperationHandle<Sprite> sprite) =>
                        {
                            if (sprite.Status == AsyncOperationStatus.Succeeded && card != null)
                            {
                                card.sprite = sprite.Result;
                                card.gameObject.SetActive(true);
                            }
                        };
                        card.rectTransform.localPosition = new Vector2(nX, nY);
                        mOrederedImage.insert(make_pair(-nCol + nRow * 11, card));
                        nY += 74;
                        if (i == nDiscardCount - 1)
                        {
                            m_SignAnim = Instantiate(m_SignAnimRectTransform, card.rectTransform);
                            m_SignAnim.localPosition = new Vector2(39, 110);
                        }
                    }
                }
                break;
            default:
                break;
            }
            for (IEnumerator<KeyValuePair<n32, Image>> it = mOrederedImage.GetEnumerator(); it.MoveNext(); /**/)
            {
                Image card = it.Current.Value;
                card.rectTransform.SetAsLastSibling();
            }
            for (n32 i = 0; i < GAME_PLAYER; i++)
            {
                GameObject highlight = m_WheelGameObject[i];
                highlight.SetActive(false);
            }
            Addressables.LoadAssetAsync<AudioClip>(Format("Assets/Audio/Mahjong/{0}/mjt{1:d}_{2:d}.mp3", m_GameEngine.GetPlayer(a_SOutCard.OutCardUser).GetSexAsStr(), ((a_SOutCard.OutCardData & MASK_COLOR) >> 4) + 1, a_SOutCard.OutCardData & MASK_VALUE)).Completed += (AsyncOperationHandle<AudioClip> audioClip) =>
            {
                if (audioClip.Status == AsyncOperationStatus.Succeeded)
                {
                    m_AudioSource.PlayOneShot(audioClip.Result);
                }
            };
            return true;
        }

        public bool OnOperateNotifyEvent(CCMD_S_OperateNotify a_SOperateNotify)
        {
            return showOperateNotify(a_SOperateNotify);
        }

        public bool OnOperateResultEvent(CCMD_S_OperateResult a_SOperateResult)
        {
            CWeaveItem weaveItem = new CWeaveItem();
            switch (a_SOperateResult.OperateCode)
            {
            case WIK_NULL:
                break;
            case WIK_P:
                {
                    weaveItem.WeaveKind = WIK_P;
                    weaveItem.CenterCard = a_SOperateResult.OperateCard;
                    weaveItem.PublicCard = true;
                    weaveItem.ProvideUser = a_SOperateResult.ProvideUser;
                    weaveItem.Valid = 1;
                    m_WeaveItemArray[a_SOperateResult.OperateUser][m_nWeaveItemCount[a_SOperateResult.OperateUser]++] = weaveItem;
                    if (a_SOperateResult.OperateUser == m_nMeChairID || s_bCheatData)
                    {
                        u8[] uRemoveCard = { a_SOperateResult.OperateCard, a_SOperateResult.OperateCard };
                        CGameLogic.RemoveCard(m_uCardIndex[a_SOperateResult.OperateUser], uRemoveCard, uRemoveCard.Length);
                    }
                }
                break;
            case WIK_G:
                {
                    weaveItem.WeaveKind = WIK_G;
                    weaveItem.CenterCard = a_SOperateResult.OperateCard;
                    bool bPublicCard = a_SOperateResult.OperateUser != a_SOperateResult.ProvideUser;
                    n32 nIndex = -1;
                    for (n32 i = 0; i < m_nWeaveItemCount[a_SOperateResult.OperateUser]; i++)
                    {
                        CWeaveItem tempWeaveItem = m_WeaveItemArray[a_SOperateResult.OperateUser][i];
                        if (tempWeaveItem.CenterCard == a_SOperateResult.OperateCard)
                        {
                            bPublicCard = true;
                            nIndex = i;
                        }
                    }
                    weaveItem.PublicCard = bPublicCard;
                    weaveItem.ProvideUser = a_SOperateResult.ProvideUser;
                    weaveItem.Valid = 1;
                    if (nIndex == -1)
                    {
                        m_WeaveItemArray[a_SOperateResult.OperateUser][m_nWeaveItemCount[a_SOperateResult.OperateUser]++] = weaveItem;
                    }
                    else
                    {
                        m_WeaveItemArray[a_SOperateResult.OperateUser][nIndex] = weaveItem;
                    }
                    if (a_SOperateResult.OperateUser == m_nMeChairID || s_bCheatData)
                    {
                        CGameLogic.RemoveAllCard(m_uCardIndex[a_SOperateResult.OperateUser], a_SOperateResult.OperateCard);
                    }
                }
                break;
            case WIK_H:
                break;
            default:
                break;
            }

            n32 nViewID = m_GameEngine.SwitchViewChairID(a_SOperateResult.OperateUser, m_nMeChairID);
            switch (nViewID)
            {
            case 0:
                {
                    m_OperateNotifyGroup.DestroyAllChildren();
                    m_OperateNotifyGroup.gameObject.SetActive(false);
                    GameObject highlight = m_WheelGameObject[0];
                    highlight.SetActive(true);
                }
                break;
            default:
                break;
            }
            string sSex = m_GameEngine.GetPlayer(a_SOperateResult.OperateUser).GetSexAsStr();
            switch (a_SOperateResult.OperateCode)
            {
            case WIK_NULL:
                break;
            case WIK_P:
                {
                    Addressables.LoadAssetAsync<AudioClip>(Format("Assets/Audio/Mahjong/{0}/peng.mp3", sSex)).Completed += (AsyncOperationHandle<AudioClip> audioClip) =>
                    {
                        if (audioClip.Status == AsyncOperationStatus.Succeeded)
                        {
                            m_AudioSource.PlayOneShot(audioClip.Result);
                        }
                    };
                    if (nViewID == 0)
                    {
                        u8[] uTempCardData = new u8[MAX_COUNT];
                        CGameLogic.SwitchToCardData(m_uCardIndex[a_SOperateResult.OperateUser], uTempCardData, MAX_COUNT);
                        n32 nWeaveItemCount = m_nWeaveItemCount[a_SOperateResult.OperateUser];
                        CCMD_S_SendCard SSendCard = new CCMD_S_SendCard();
                        SSendCard.CurrentUser = a_SOperateResult.OperateUser;
                        SSendCard.CardData = uTempCardData[MAX_COUNT - 1 - 3 * nWeaveItemCount];
                        showSendCard(SSendCard);
                    }
                    m_nDiscardCount[a_SOperateResult.ProvideUser]--;
                    showAndUpdateDiscardCard();
                }
                break;
            case WIK_G:
                {
                    Addressables.LoadAssetAsync<AudioClip>(Format("Assets/Audio/Mahjong/{0}/gang.mp3", sSex)).Completed += (AsyncOperationHandle<AudioClip> audioClip) =>
                    {
                        if (audioClip.Status == AsyncOperationStatus.Succeeded)
                        {
                            m_AudioSource.PlayOneShot(audioClip.Result);
                        }
                    };
                    if (a_SOperateResult.ProvideUser != a_SOperateResult.OperateUser)
                    {
                        m_nDiscardCount[a_SOperateResult.ProvideUser]--;
                        showAndUpdateDiscardCard();
                    }
                    else
                    {
                        RectTransform recvCard = m_RecvCardRectTransform[0];
                        recvCard.DestroyAllChildren();
                    }
                }
                break;
            case WIK_H:
                {
                    if (a_SOperateResult.OperateUser == a_SOperateResult.ProvideUser)
                    {
                        Addressables.LoadAssetAsync<AudioClip>(Format("Assets/Audio/Mahjong/{0}/zimo.mp3", sSex)).Completed += (AsyncOperationHandle<AudioClip> audioClip) =>
                        {
                            if (audioClip.Status == AsyncOperationStatus.Succeeded)
                            {
                                m_AudioSource.PlayOneShot(audioClip.Result);
                            }
                        };
                    }
                    else
                    {
                        Addressables.LoadAssetAsync<AudioClip>(Format("Assets/Audio/Mahjong/{0}/hu.mp3", sSex)).Completed += (AsyncOperationHandle<AudioClip> audioClip) =>
                        {
                            if (audioClip.Status == AsyncOperationStatus.Succeeded)
                            {
                                m_AudioSource.PlayOneShot(audioClip.Result);
                            }
                        };
                    }
                }
                break;
            default:
                break;
            }
            showAndPlayOperateEffect(nViewID, a_SOperateResult.OperateCode, a_SOperateResult.ProvideUser == m_nMeChairID);
            showAndUpdateHandCard();
            return true;
        }

        public bool OnGameEndEvent(CCMD_S_GameEnd a_SGameEnd)
        {
            showAndUpdateUserScore(a_SGameEnd.GameScoreTable);
            for (n32 i = 0; i < GAME_PLAYER; i++)
            {
                if ((a_SGameEnd.HuUser & SDW_BIT32(i)) != 0)
                {
                    string sSex = m_GameEngine.GetPlayer(i).GetSexAsStr();
                    n32 nViewID = m_GameEngine.SwitchViewChairID(i, m_nMeChairID);
                    if ((a_SGameEnd.HuUser & SDW_BIT32(a_SGameEnd.ProvideUser)) != 0)
                    {
                        Addressables.LoadAssetAsync<AudioClip>(Format("Assets/Audio/Mahjong/{0}/zimo.mp3", sSex)).Completed += (AsyncOperationHandle<AudioClip> audioClip) =>
                        {
                            if (audioClip.Status == AsyncOperationStatus.Succeeded)
                            {
                                m_AudioSource.PlayOneShot(audioClip.Result);
                            }
                        };
                        showAndPlayOperateEffect(nViewID, WIK_H, true);
                    }
                    else
                    {
                        Addressables.LoadAssetAsync<AudioClip>(Format("Assets/Audio/Mahjong/{0}/hu.mp3", sSex)).Completed += (AsyncOperationHandle<AudioClip> audioClip) =>
                        {
                            if (audioClip.Status == AsyncOperationStatus.Succeeded)
                            {
                                m_AudioSource.PlayOneShot(audioClip.Result);
                            }
                        };
                        showAndPlayOperateEffect(nViewID, WIK_H, false);
                    }
                }
            }
            m_Ting.gameObject.SetActive(false);
            m_OperateNotifyGroup.DestroyAllChildren();
            m_OperateNotifyGroup.gameObject.SetActive(false);
            f32 fTime = a_SGameEnd.HuUser == 0 ? 0.0f : 1.0f;
            StartCoroutine(showGameOver(fTime + 0.1f, a_SGameEnd));
            return true;
        }

        private IEnumerator showGameOver(f32 a_fTime, CCMD_S_GameEnd a_SGameEnd)
        {
            yield return new WaitForSeconds(a_fTime);
            CGameOverDlg gameOverDlg = Instantiate(m_GameOverDlg, m_GameOverPanelRectTransform);
            gameOverDlg.SetGameUI(this, a_SGameEnd);
            CDialogManager.Instance.ShowDialog(gameOverDlg);
            removeEffectNode();
        }

        private bool showAndUpdateHandCard()
        {
            bool bIsShowAllMahjong = CDebugValue.IsShowAllMahjong();
            for (n32 i = 0; i < m_GameEngine.GetPlayerCount(); i++)
            {
                n32 nViewChairID = m_GameEngine.SwitchViewChairID(i, m_nMeChairID);
                u8[] uCardData = new u8[MAX_COUNT];
                CGameLogic.SwitchToCardData(m_uCardIndex[i], uCardData, MAX_COUNT);
                n32 nWeaveItemCount = m_nWeaveItemCount[i];
                CWeaveItem[] weaveItemArray = new CWeaveItem[MAX_WEAVE];
                CJaggedArray.Copy(m_WeaveItemArray[i], weaveItemArray, weaveItemArray.Length);
                switch (nViewChairID)
                {
                case 0:
                    {
                        RectTransform handCard = m_PlayerPanel[nViewChairID].m_HandCardRectTransform;
                        RectTransform comb = m_PlayerPanel[nViewChairID].m_CombRectTransform;
                        handCard.DestroyAllChildren();
                        comb.DestroyAllChildren();
                        n32 nX = 0;
                        for (n32 j = 0; j < nWeaveItemCount; j++)
                        {
                            CWeaveItem weaveItem = weaveItemArray[j];
                            CBindMeld weave = null;
                            if (weaveItem.WeaveKind == WIK_G)
                            {
                                weave = Instantiate(m_Gang0Meld, comb);
                            }
                            if (weaveItem.WeaveKind == WIK_P)
                            {
                                weave = Instantiate(m_Peng0Meld, comb);
                            }
                            weave.transform.localPosition = new Vector2(nX, 0);
                            n32 nProvideViewID = m_GameEngine.SwitchViewChairID(weaveItem.ProvideUser, m_nMeChairID);
                            Addressables.LoadAssetAsync<Sprite>(CUIHelper.GetDiscardCardImagePath(nViewChairID, weaveItem.CenterCard)).Completed += (AsyncOperationHandle<Sprite> sprite) =>
                            {
                                if (sprite.Status == AsyncOperationStatus.Succeeded && weave != null)
                                {
                                    switch (nProvideViewID)
                                    {
                                    case 0:
                                        weave.m_CenterImage.sprite = sprite.Result;
                                        if (weaveItem.PublicCard)
                                        {
                                            weave.m_LeftImage.sprite = sprite.Result;
                                            weave.m_RightImage.sprite = sprite.Result;
                                        }
                                        break;
                                    case 1:
                                        weave.m_CenterImage.sprite = sprite.Result;
                                        weave.m_RightImage.sprite = sprite.Result;
                                        break;
                                    case 2:
                                        weave.m_LeftImage.sprite = sprite.Result;
                                        weave.m_RightImage.sprite = sprite.Result;
                                        break;
                                    case 3:
                                        weave.m_CenterImage.sprite = sprite.Result;
                                        weave.m_LeftImage.sprite = sprite.Result;
                                        break;
                                    default:
                                        break;
                                    }
                                }
                            };
                            nX += 228;
                        }
                        for (n32 j = 0; j < MAX_COUNT - 1 - 3 * nWeaveItemCount; j++)
                        {
                            CTile card = Instantiate(m_HandCard0Tile, handCard);
                            Addressables.LoadAssetAsync<Sprite>(CUIHelper.GetHandCardImagePath(nViewChairID, uCardData[j])).Completed += (AsyncOperationHandle<Sprite> sprite) =>
                            {
                                if (sprite.Status == AsyncOperationStatus.Succeeded && card != null)
                                {
                                    card.m_TileImage.sprite = sprite.Result;
                                    card.gameObject.SetActive(true);
                                }
                            };
                            card.transform.localPosition = new Vector2(nX, 0);
                            card.m_TileImage.raycastTarget = true;
                            card.m_nTileIndex = j;
                            card.m_uTileData = uCardData[j];
                            card.m_TileImage.name = Format("bt_card_{0:d}", uCardData[j]);
                            card.SetPointerEventHandler(this);
                            nX += 76;
                        }
                    }
                    break;
                case 1:
                    {
                        RectTransform handCard = m_PlayerPanel[nViewChairID].m_HandCardRectTransform;
                        RectTransform comb = m_PlayerPanel[nViewChairID].m_CombRectTransform;
                        handCard.DestroyAllChildren();
                        comb.DestroyAllChildren();
                        n32 nY = 800;
                        for (n32 j = 0; j < nWeaveItemCount; j++)
                        {
                            nY -= 160;
                            CWeaveItem weaveItem = weaveItemArray[j];
                            CBindMeld weave = null;
                            if (weaveItem.WeaveKind == WIK_G)
                            {
                                weave = Instantiate(m_Gang1Meld, comb);
                            }
                            if (weaveItem.WeaveKind == WIK_P)
                            {
                                weave = Instantiate(m_Peng1Meld, comb);
                            }
                            weave.transform.localPosition = new Vector2(0, nY + 20);
                            n32 nProvideViewID = m_GameEngine.SwitchViewChairID(weaveItem.ProvideUser, m_nMeChairID);
                            if (weaveItem.PublicCard || bIsShowAllMahjong)
                            {
                                Addressables.LoadAssetAsync<Sprite>(CUIHelper.GetDiscardCardImagePath(nViewChairID, weaveItem.CenterCard)).Completed += (AsyncOperationHandle<Sprite> sprite) =>
                                {
                                    if (sprite.Status == AsyncOperationStatus.Succeeded && weave != null)
                                    {
                                        switch (nProvideViewID)
                                        {
                                        case 0:
                                            weave.m_CenterImage.sprite = sprite.Result;
                                            weave.m_LeftImage.sprite = sprite.Result;
                                            break;
                                        case 1:
                                            if (weaveItem.PublicCard)
                                            {
                                                weave.m_CenterImage.sprite = sprite.Result;
                                                weave.m_LeftImage.sprite = sprite.Result;
                                                weave.m_RightImage.sprite = sprite.Result;
                                            }
                                            else if (bIsShowAllMahjong)
                                            {
                                                weave.m_CenterImage.sprite = sprite.Result;
                                            }
                                            break;
                                        case 2:
                                            weave.m_CenterImage.sprite = sprite.Result;
                                            weave.m_RightImage.sprite = sprite.Result;
                                            break;
                                        case 3:
                                            weave.m_LeftImage.sprite = sprite.Result;
                                            weave.m_RightImage.sprite = sprite.Result;
                                            break;
                                        default:
                                            break;
                                        }
                                    }
                                };
                            }
                        }
                        for (n32 j = 0; j < MAX_COUNT - 1 - 3 * nWeaveItemCount; j++)
                        {
                            nY -= 60;
                            Image card = Instantiate(m_HandCardImage[nViewChairID - 1], handCard);
                            Addressables.LoadAssetAsync<Sprite>(CUIHelper.GetHandCardImagePath(nViewChairID, uCardData[j])).Completed += (AsyncOperationHandle<Sprite> sprite) =>
                            {
                                if (sprite.Status == AsyncOperationStatus.Succeeded && card != null)
                                {
                                    card.sprite = sprite.Result;
                                }
                            };
                            card.rectTransform.localPosition = new Vector2(0, nY - 20);
                        }
                    }
                    break;
                case 2:
                    {
                        RectTransform handCard = m_PlayerPanel[nViewChairID].m_HandCardRectTransform;
                        RectTransform comb = m_PlayerPanel[nViewChairID].m_CombRectTransform;
                        handCard.DestroyAllChildren();
                        comb.DestroyAllChildren();
                        n32 nX = 1027;
                        for (n32 j = 0; j < nWeaveItemCount; j++)
                        {
                            nX -= 228;
                            CWeaveItem weaveItem = weaveItemArray[j];
                            CBindMeld weave = null;
                            if (weaveItem.WeaveKind == WIK_G)
                            {
                                weave = Instantiate(m_Gang0Meld, comb);
                            }
                            if (weaveItem.WeaveKind == WIK_P)
                            {
                                weave = Instantiate(m_Peng0Meld, comb);
                            }
                            weave.transform.localPosition = new Vector2(nX + 23, 0);
                            n32 nProvideViewID = m_GameEngine.SwitchViewChairID(weaveItem.ProvideUser, m_nMeChairID);
                            if (weaveItem.PublicCard || bIsShowAllMahjong)
                            {
                                Addressables.LoadAssetAsync<Sprite>(CUIHelper.GetDiscardCardImagePath(nViewChairID, weaveItem.CenterCard)).Completed += (AsyncOperationHandle<Sprite> sprite) =>
                                {
                                    if (sprite.Status == AsyncOperationStatus.Succeeded && weave != null)
                                    {
                                        switch (nProvideViewID)
                                        {
                                        case 0:
                                            weave.m_LeftImage.sprite = sprite.Result;
                                            weave.m_RightImage.sprite = sprite.Result;
                                            break;
                                        case 1:
                                            weave.m_CenterImage.sprite = sprite.Result;
                                            weave.m_RightImage.sprite = sprite.Result;
                                            break;
                                        case 2:
                                            if (weaveItem.PublicCard)
                                            {
                                                weave.m_CenterImage.sprite = sprite.Result;
                                                weave.m_LeftImage.sprite = sprite.Result;
                                                weave.m_RightImage.sprite = sprite.Result;
                                            }
                                            else if (bIsShowAllMahjong)
                                            {
                                                weave.m_CenterImage.sprite = sprite.Result;
                                            }
                                            break;
                                        case 3:
                                            weave.m_LeftImage.sprite = sprite.Result;
                                            weave.m_CenterImage.sprite = sprite.Result;
                                            break;
                                        default:
                                            break;
                                        }
                                    }
                                };
                            }
                        }
                        for (n32 j = 0; j < MAX_COUNT - 1 - 3 * nWeaveItemCount; j++)
                        {
                            nX -= 76;
                            Image card = Instantiate(m_HandCardImage[nViewChairID - 1], handCard);
                            Addressables.LoadAssetAsync<Sprite>(CUIHelper.GetHandCardImagePath(nViewChairID, uCardData[j])).Completed += (AsyncOperationHandle<Sprite> sprite) =>
                            {
                                if (sprite.Status == AsyncOperationStatus.Succeeded && card != null)
                                {
                                    card.sprite = sprite.Result;
                                }
                            };
                            card.rectTransform.localPosition = new Vector2(nX, 0);
                        }
                    }
                    break;
                case 3:
                    {
                        RectTransform handCard = m_PlayerPanel[nViewChairID].m_HandCardRectTransform;
                        RectTransform comb = m_PlayerPanel[nViewChairID].m_CombRectTransform;
                        handCard.DestroyAllChildren();
                        comb.DestroyAllChildren();
                        n32 nY = 0;
                        for (n32 j = 0; j < nWeaveItemCount; j++)
                        {
                            CWeaveItem weaveItem = weaveItemArray[j];
                            CBindMeld weave = null;
                            if (weaveItem.WeaveKind == WIK_G)
                            {
                                weave = Instantiate(m_Gang1Meld, comb);
                            }
                            if (weaveItem.WeaveKind == WIK_P)
                            {
                                weave = Instantiate(m_Peng1Meld, comb);
                            }
                            weave.transform.localPosition = new Vector2(0, nY - 10);
                            n32 nProvideViewID = m_GameEngine.SwitchViewChairID(weaveItem.ProvideUser, m_nMeChairID);
                            if (weaveItem.PublicCard || bIsShowAllMahjong)
                            {
                                Addressables.LoadAssetAsync<Sprite>(CUIHelper.GetDiscardCardImagePath(nViewChairID, weaveItem.CenterCard)).Completed += (AsyncOperationHandle<Sprite> sprite) =>
                                {
                                    if (sprite.Status == AsyncOperationStatus.Succeeded && weave != null)
                                    {
                                        switch (nProvideViewID)
                                        {
                                        case 0:
                                            weave.m_CenterImage.sprite = sprite.Result;
                                            weave.m_LeftImage.sprite = sprite.Result;
                                            break;
                                        case 1:
                                            weave.m_LeftImage.sprite = sprite.Result;
                                            weave.m_RightImage.sprite = sprite.Result;
                                            break;
                                        case 2:
                                            weave.m_CenterImage.sprite = sprite.Result;
                                            weave.m_RightImage.sprite = sprite.Result;
                                            break;
                                        case 3:
                                            if (weaveItem.PublicCard)
                                            {
                                                weave.m_CenterImage.sprite = sprite.Result;
                                                weave.m_LeftImage.sprite = sprite.Result;
                                                weave.m_RightImage.sprite = sprite.Result;
                                            }
                                            else if (bIsShowAllMahjong)
                                            {
                                                weave.m_CenterImage.sprite = sprite.Result;
                                            }
                                            break;
                                        default:
                                            break;
                                        }
                                    }
                                };
                            }
                            nY += 160;
                        }
                        for (n32 j = 0; j < MAX_COUNT - 1 - 3 * nWeaveItemCount; j++)
                        {
                            Image card = Instantiate(m_HandCardImage[nViewChairID - 1], handCard);
                            Addressables.LoadAssetAsync<Sprite>(CUIHelper.GetHandCardImagePath(nViewChairID, uCardData[j])).Completed += (AsyncOperationHandle<Sprite> sprite) =>
                            {
                                if (sprite.Status == AsyncOperationStatus.Succeeded && card != null)
                                {
                                    card.sprite = sprite.Result;
                                }
                            };
                            card.rectTransform.localPosition = new Vector2(0, nY - 20);
                            card.rectTransform.SetAsFirstSibling();
                            nY += 60;
                        }
                    }
                    break;
                default:
                    break;
                }
            }
            return true;
        }

        private bool showAndUpdateDiscardCard()
        {
            for (n32 nChairID = 0; nChairID < m_GameEngine.GetPlayerCount(); nChairID++)
            {
                n32 nViewID = m_GameEngine.SwitchViewChairID(nChairID, m_nMeChairID);
                mapp<n32, Image> mOrederedImage = new mapp<n32, Image>();
                switch (nViewID)
                {
                case 0:
                    {
                        RectTransform discardCard = m_DiscardCardRectTransform[nViewID];
                        discardCard.DestroyAllChildren();
                        n32 nDiscardCount = m_nDiscardCount[nChairID];
                        n32 nX = 0;
                        n32 nY = 0;
                        for (n32 i = 0; i < nDiscardCount; i++)
                        {
                            n32 nCol = i % 12;
                            n32 nRow = i / 12;
                            nX = nCol == 0 ? 0 : nX;
                            nY = nRow * 90;
                            Image card = Instantiate(m_DiscardCardImage[nViewID], m_DiscardCardRectTransform[nViewID]);
                            Addressables.LoadAssetAsync<Sprite>(CUIHelper.GetDiscardCardImagePath(nViewID, m_uDiscardCard[nChairID][i])).Completed += (AsyncOperationHandle<Sprite> sprite) =>
                            {
                                if (sprite.Status == AsyncOperationStatus.Succeeded && card != null)
                                {
                                    card.sprite = sprite.Result;
                                    card.gameObject.SetActive(true);
                                }
                            };
                            card.rectTransform.localPosition = new Vector2(nX, nY);
                            mOrederedImage.insert(make_pair(-nRow * 12 + nCol, card));
                            nX += 76;
                        }
                    }
                    break;
                case 1:
                    {
                        RectTransform discardCard = m_DiscardCardRectTransform[nViewID];
                        discardCard.DestroyAllChildren();
                        n32 nDiscardCount = m_nDiscardCount[nChairID];
                        n32 nX = 0;
                        n32 nY = 0;
                        for (n32 i = 0; i < nDiscardCount; i++)
                        {
                            n32 nCol = i % 11;
                            n32 nRow = i / 11;
                            nY = nCol == 0 ? 0 : nY;
                            nX = 116 * nRow;
                            Image card = Instantiate(m_DiscardCardImage[nViewID], m_DiscardCardRectTransform[nViewID]);
                            Addressables.LoadAssetAsync<Sprite>(CUIHelper.GetDiscardCardImagePath(nViewID, m_uDiscardCard[nChairID][i])).Completed += (AsyncOperationHandle<Sprite> sprite) =>
                            {
                                if (sprite.Status == AsyncOperationStatus.Succeeded && card != null)
                                {
                                    card.sprite = sprite.Result;
                                    card.gameObject.SetActive(true);
                                }
                            };
                            card.rectTransform.localPosition = new Vector2(nX, 740 - nY);
                            mOrederedImage.insert(make_pair(nCol + nRow * 11, card));
                            nY += 74;
                        }
                    }
                    break;
                case 2:
                    {
                        RectTransform discardCard = m_DiscardCardRectTransform[nViewID];
                        discardCard.DestroyAllChildren();
                        n32 nDiscardCount = m_nDiscardCount[nChairID];
                        n32 nX = 0;
                        n32 nY = 0;
                        for (n32 i = 0; i < nDiscardCount; i++)
                        {
                            n32 nCol = i % 12;
                            n32 nRow = i / 12;
                            nX = nCol == 0 ? 0 : nX;
                            nY = 90 - nRow * 90;
                            Image card = Instantiate(m_DiscardCardImage[nViewID], m_DiscardCardRectTransform[nViewID]);
                            Addressables.LoadAssetAsync<Sprite>(CUIHelper.GetDiscardCardImagePath(nViewID, m_uDiscardCard[nChairID][i])).Completed += (AsyncOperationHandle<Sprite> sprite) =>
                            {
                                if (sprite.Status == AsyncOperationStatus.Succeeded && card != null)
                                {
                                    card.sprite = sprite.Result;
                                    card.gameObject.SetActive(true);
                                }
                            };
                            card.rectTransform.localPosition = new Vector2(nX, nY);
                            mOrederedImage.insert(make_pair(nRow * 12 + nCol, card));
                            nX += 76;
                        }
                    }
                    break;
                case 3:
                    {
                        RectTransform discardCard = m_DiscardCardRectTransform[nViewID];
                        discardCard.DestroyAllChildren();
                        n32 nDiscardCount = m_nDiscardCount[nChairID];
                        n32 nX = 0;
                        n32 nY = 0;
                        for (n32 i = 0; i < nDiscardCount; i++)
                        {
                            n32 nCol = i % 11;
                            n32 nRow = i / 11;
                            nY = nCol == 0 ? 0 : nY;
                            nX = 240 - 116 * nRow;
                            Image card = Instantiate(m_DiscardCardImage[nViewID], m_DiscardCardRectTransform[nViewID]);
                            Addressables.LoadAssetAsync<Sprite>(CUIHelper.GetDiscardCardImagePath(nViewID, m_uDiscardCard[nChairID][i])).Completed += (AsyncOperationHandle<Sprite> sprite) =>
                            {
                                if (sprite.Status == AsyncOperationStatus.Succeeded && card != null)
                                {
                                    card.sprite = sprite.Result;
                                    card.gameObject.SetActive(true);
                                }
                            };
                            card.rectTransform.localPosition = new Vector2(nX, nY);
                            mOrederedImage.insert(make_pair(-nCol + nRow * 11, card));
                            nY += 74;
                        }
                    }
                    break;
                default:
                    break;
                }
                for (IEnumerator<KeyValuePair<n32, Image>> it = mOrederedImage.GetEnumerator(); it.MoveNext(); /**/)
                {
                    Image card = it.Current.Value;
                    card.rectTransform.SetAsLastSibling();
                }
            }
            return true;
        }

        private bool showSendCard(CCMD_S_SendCard a_SSendCard)
        {
            m_OperateNotifyGroup.DestroyAllChildren();
            m_OperateNotifyGroup.gameObject.SetActive(true);
            this.SafeStopCoroutine(ref m_SendCardTimerUpdateCoroutine);
            m_SendCardTimerUpdateCoroutine = StartCoroutine(SendCardTimerUpdate(0.0f, 1.0f));
            m_CardNumText.text = Format("{0:d}", m_nLeftCardCount);
            n32 nViewID = m_GameEngine.SwitchViewChairID(a_SSendCard.CurrentUser, m_nMeChairID);
            m_bOperate = nViewID == 0;
            switch (nViewID)
            {
            case 0:
                {
                    RectTransform recvCard = m_RecvCardRectTransform[nViewID];
                    recvCard.DestroyAllChildren();
                    CTile card = Instantiate(m_HandCard0Tile, recvCard);
                    Addressables.LoadAssetAsync<Sprite>(CUIHelper.GetHandCardImagePath(nViewID, a_SSendCard.CardData)).Completed += (AsyncOperationHandle<Sprite> sprite) =>
                    {
                        if (sprite.Status == AsyncOperationStatus.Succeeded && card != null)
                        {
                            card.m_TileImage.sprite = sprite.Result;
                            card.gameObject.SetActive(true);
                        }
                    };
                    card.transform.localPosition = new Vector2(0, 0);
                    card.m_TileImage.raycastTarget = true;
                    card.m_uTileData = a_SSendCard.CardData;
                    card.m_TileImage.name = Format("bt_card_{0:d}", a_SSendCard.CardData);
                    card.SetPointerEventHandler(this);
                    m_Ting.gameObject.SetActive(false);
                    if (a_SSendCard.ActionMask != WIK_NULL)
                    {
                        CCMD_S_OperateNotify SOperateNotify = new CCMD_S_OperateNotify();
                        SOperateNotify.ActionMask = a_SSendCard.ActionMask;
                        SOperateNotify.ActionCard = a_SSendCard.CardData;
                        SOperateNotify.GangCount = a_SSendCard.GangCount;
                        Array.Copy(a_SSendCard.GangCard, SOperateNotify.GangCard, SOperateNotify.GangCard.Length);
                        showOperateNotify(SOperateNotify);
                    }
                }
                break;
            case 1:
            case 2:
            case 3:
                {
                    RectTransform recvCard = m_RecvCardRectTransform[nViewID];
                    recvCard.DestroyAllChildren();
                    if (CDebugValue.IsShowAllMahjong())
                    {
                        Image card = Instantiate(m_HandCardImage[nViewID - 1], recvCard);
                        Addressables.LoadAssetAsync<Sprite>(CUIHelper.GetHandCardImagePath(nViewID, a_SSendCard.CardData)).Completed += (AsyncOperationHandle<Sprite> sprite) =>
                        {
                            if (sprite.Status == AsyncOperationStatus.Succeeded && card != null)
                            {
                                card.sprite = sprite.Result;
                            }
                        };
                        card.rectTransform.localPosition = new Vector2(0, 0);
                    }
                }
                break;
            default:
                break;
            }
            for (n32 i = 0; i < GAME_PLAYER; i++)
            {
                if (i != 0)
                {
                    RectTransform recvCard = m_RecvCardRectTransform[i];
                    if (recvCard != null)
                    {
                        recvCard.gameObject.SetActive(nViewID == i);
                    }
                }
                GameObject highlight = m_WheelGameObject[i];
                if (highlight != null)
                {
                    highlight.SetActive(nViewID == i);
                }
            }
            return true;
        }

        private bool showOperateNotify(CCMD_S_OperateNotify a_SOperateNotify)
        {
            if (a_SOperateNotify.ActionMask == WIK_NULL)
            {
                return true;
            }
            m_OperateNotifyGroup.gameObject.SetActive(true);
            m_OperateNotifyGroup.DestroyAllChildren();
            n32 nX = 500;
            n32 nY = 65;
            if ((a_SOperateNotify.ActionMask & WIK_H) != 0)
            {
                COperateButton button = Instantiate(m_HuButton, m_OperateNotifyGroup);
                button.transform.localPosition = new Vector2(nX, nY);
                button.m_uOperateCode = WIK_H;
                button.m_uOperateCard = a_SOperateNotify.ActionCard;
                button.m_Button.onClick.AddListener(delegate { onClickOperateButton(button.m_uOperateCode, button.m_uOperateCard); });
                nX -= 160;
            }
            if ((a_SOperateNotify.ActionMask & WIK_G) != 0)
            {
                for (n32 i = 0; i < a_SOperateNotify.GangCount; i++)
                {
                    COperateButton button = Instantiate(m_GangButton, m_OperateNotifyGroup);
                    Addressables.LoadAssetAsync<Sprite>(CUIHelper.GetDiscardCardImagePath(0, a_SOperateNotify.GangCard[i])).Completed += (AsyncOperationHandle<Sprite> sprite) =>
                    {
                        if (sprite.Status == AsyncOperationStatus.Succeeded && button != null && button.m_Image != null)
                        {
                            button.m_Image.sprite = sprite.Result;
                            button.m_Image.gameObject.SetActive(true);
                        }
                    };
                    button.transform.localPosition = new Vector2(nX, nY + i * 120);
                    button.m_uOperateCode = WIK_G;
                    button.m_uOperateCard = a_SOperateNotify.GangCard[i];
                    button.m_Button.onClick.AddListener(delegate { onClickOperateButton(button.m_uOperateCode, button.m_uOperateCard); });
                }
                nX -= 160;
            }
            if ((a_SOperateNotify.ActionMask & WIK_P) != 0)
            {
                COperateButton button = Instantiate(m_PengButton, m_OperateNotifyGroup);
                button.transform.localPosition = new Vector2(nX, nY);
                button.m_uOperateCode = WIK_P;
                button.m_uOperateCard = a_SOperateNotify.ActionCard;
                button.m_Button.onClick.AddListener(delegate { onClickOperateButton(button.m_uOperateCode, button.m_uOperateCard); });
                nX -= 160;
            }
            {
                COperateButton button = Instantiate(m_GuoButton, m_OperateNotifyGroup);
                button.transform.localPosition = new Vector2(nX, nY);
                button.m_uOperateCode = WIK_NULL;
                button.m_uOperateCard = a_SOperateNotify.ActionCard;
                button.m_Button.onClick.AddListener(delegate { onClickOperateButton(button.m_uOperateCode, button.m_uOperateCard); });
            }
            return true;
        }

        private bool showAndPlayOperateEffect(n32 a_nViewID, u8 a_uOperateCode, bool a_bZm)
        {
            if (a_uOperateCode == WIK_NULL)
            {
                return true;
            }
            if (a_uOperateCode != WIK_H)
            {
                removeEffectNode();
            }
            Vector2 pos = default(Vector2);
            switch (a_nViewID)
            {
            case 0:
                pos.y = -160;
                break;
            case 1:
                pos.x = -200;
                break;
            case 2:
                pos.y = 160;
                break;
            case 3:
                pos.x = 200;
                break;
            default:
                break;
            }
            RectTransform effect = null;
            switch (a_uOperateCode)
            {
            case WIK_P:
                effect = Instantiate(m_EffectPengRectTransform, m_EffectPanelRectTransform);
                break;
            case WIK_G:
                effect = Instantiate(m_EffectGangRectTransform, m_EffectPanelRectTransform);
                break;
            case WIK_H:
                if (a_bZm)
                {
                    effect = Instantiate(m_EffectZmRectTransform, m_EffectPanelRectTransform);
                }
                else
                {
                    effect = Instantiate(m_EffectHuRectTransform, m_EffectPanelRectTransform);
                }
                break;
            default:
                break;
            }
            if (effect != null)
            {
                effect.localPosition = pos;
                effect.name = "EffectNode";
            }
            return true;
        }

        private void onClickOperateButton(u8 a_uOperateCode, u8 a_uOperateCard)
        {
            m_OperateNotifyGroup.DestroyAllChildren();
            m_OperateNotifyGroup.gameObject.SetActive(false);
            CCMD_C_OperateCard COperateCard = new CCMD_C_OperateCard();
            COperateCard.OperateCard = a_uOperateCard;
            COperateCard.OperateUser = m_nMeChairID;
            COperateCard.OperateCode = a_uOperateCode;
            m_GameEngine.OnUserOperateCard(COperateCard);
        }

        private bool showTingResult(u8[] a_uCardIndex, CWeaveItem[] a_WeaveItem, n32 a_nWeaveCount)
        {
            Debug.Assert(a_uCardIndex.Length == MAX_INDEX);
            Debug.Assert(a_nWeaveCount <= a_WeaveItem.Length);
            CTingResult tingResult = new CTingResult();
            if (CGameLogic.AnalyseTingCardResult(a_uCardIndex, a_WeaveItem, a_nWeaveCount, tingResult))
            {
                m_Ting.gameObject.SetActive(true);
                RectTransform tingCard = m_Ting.m_TingCardRectTransform;
                tingCard.DestroyAllChildren();
                for (n32 i = 0; i < tingResult.TingCount; i++)
                {
                    Image tingCardImage = Instantiate(m_DiscardCardImage[0], tingCard);
                    Addressables.LoadAssetAsync<Sprite>(CUIHelper.GetDiscardCardImagePath(0, tingResult.TingCard[i])).Completed += (AsyncOperationHandle<Sprite> sprite) =>
                    {
                        if (sprite.Status == AsyncOperationStatus.Succeeded && tingCardImage != null)
                        {
                            tingCardImage.sprite = sprite.Result;
                            tingCardImage.gameObject.SetActive(true);
                        }
                    };
                    tingCardImage.rectTransform.localPosition = new Vector2(76 * i + (76 * 3 - 76 * tingResult.TingCount) / 2, 0);
                }
            }
            else
            {
                m_Ting.gameObject.SetActive(false);
            }
            return true;
        }

        private bool showAndUpdateUserScore(n64[] a_nGameScoreTable)
        {
            Debug.Assert(a_nGameScoreTable.Length == GAME_PLAYER);
            for (n32 i = 0; i < GAME_PLAYER; i++)
            {
                n32 nViewID = m_GameEngine.SwitchViewChairID(i, m_nMeChairID);
                m_FaceFrame[nViewID].m_ScoreText.text = Format("{0:d}", a_nGameScoreTable[i]);
            }
            return true;
        }

        private void removeEffectNode()
        {
            m_EffectPanelRectTransform.DestroyAllChildren();
        }
    }
}
