using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
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
    public class CGameOverDlg : MonoBehaviour
    {
        [SerializeField]
        private RectTransform m_RootRectTransform = null;
        [SerializeField]
        private CBindPlayerView[] m_PlayerView = new CBindPlayerView[GAME_PLAYER];
        [SerializeField]
        private Image m_OverResultImage = null;

        private CGamePanel m_GamePanel = null;
        private CCMD_S_GameEnd m_SGameEnd = null;

        private void Awake()
        {
            if (m_RootRectTransform == null)
            {
                Debug.LogError("set RootRectTransform first");
                return;
            }
            if (m_PlayerView.Length != GAME_PLAYER)
            {
                Debug.LogError("set PlayerView first");
                return;
            }
            for (n32 i = 0; i < m_PlayerView.Length; i++)
            {
                if (m_PlayerView[i] == null)
                {
                    Debug.LogError("set PlayerView first");
                    return;
                }
            }
            if (m_OverResultImage == null)
            {
                Debug.LogError("set OverResultImage first");
                return;
            }
        }

        // Start is called before the first frame update
        private void Start()
        {
            m_RootRectTransform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
            m_RootRectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutElastic);
            showResult();
        }

        // Update is called once per frame
        private void Update()
        {
        }

        public void OnClickClose()
        {
            CDialogManager.Instance.CloseAllDialog();
            CGameEngine.Instance.OnGameRestart();
        }

        public void SetGameUI(CGamePanel a_GamePanel, CCMD_S_GameEnd a_SGameEnd)
        {
            m_GamePanel = a_GamePanel;
            m_SGameEnd = reinterpret_cast<CCMD_S_GameEnd>(a_SGameEnd.Clone());
        }

        private void showResult()
        {
            string sOverResultImagePath = "Assets/Textures/GameOver/result_draw.png";
            if (m_SGameEnd.HuUser != 0)
            {
                sOverResultImagePath = m_SGameEnd.GameScore[m_GamePanel.GetMeChairID()] < 0 ? "Assets/Textures/GameOver/result_lose.png" : "Assets/Textures/GameOver/result_win.png";
            }
            m_OverResultImage.gameObject.SetActive(false);
            Addressables.LoadAssetAsync<Sprite>(sOverResultImagePath).Completed += (AsyncOperationHandle<Sprite> sprite) =>
            {
                if (sprite.Status == AsyncOperationStatus.Succeeded)
                {
                    m_OverResultImage.sprite = sprite.Result;
                    m_OverResultImage.gameObject.SetActive(true);
                }
            };
            for (n32 i = 0; i < m_GamePanel.GetGameEngine().GetPlayerCount(); i++)
            {
                n32 nViewID = m_GamePanel.GetGameEngine().SwitchViewChairID(i, m_GamePanel.GetMeChairID());
                CBindPlayerView playerView = m_PlayerView[nViewID];
                Addressables.LoadAssetAsync<Sprite>(Format("Assets/Textures/Game/im_defaulthead_{0:d}.png", m_GamePanel.GetGameEngine().GetPlayer(i).GetSex())).Completed += (AsyncOperationHandle<Sprite> sprite) =>
                {
                    if (sprite.Status == AsyncOperationStatus.Succeeded)
                    {
                        playerView.m_OverHeadImage.sprite = sprite.Result;
                    }
                };
                playerView.m_OverScoreText.text = Format("{0}{1:d}", (m_SGameEnd.GameScore[i] >= 0 ? "+" : ""), m_SGameEnd.GameScore[i]);
                RectTransform overHandCard = playerView.m_OverHandCardRectTransform;
                overHandCard.DestroyAllChildren();
                Image overHuCard = playerView.m_OverHuCardImage;
                Image overHuFlag = playerView.m_OverHuFlagImage;
                overHuCard.gameObject.SetActive(false);
                overHuFlag.gameObject.SetActive(false);
                playerView.m_OverBankerGameObject.SetActive(i == m_GamePanel.GetBankerChair());

                n32 nWeaveCount = m_SGameEnd.WeaveCount[i];
                if ((m_SGameEnd.HuUser & SDW_BIT32(i)) != 0)
                {
                    CGameLogic.RemoveCard(m_SGameEnd.CardData[i], m_SGameEnd.CardCount[i], new u8[] { m_SGameEnd.HuCard }, 1);
                    m_SGameEnd.CardCount[i]--;
                    Addressables.LoadAssetAsync<Sprite>(CUIHelper.GetDiscardCardImagePath(0, m_SGameEnd.HuCard)).Completed += (AsyncOperationHandle<Sprite> sprite) =>
                    {
                        if (sprite.Status == AsyncOperationStatus.Succeeded && overHuCard != null)
                        {
                            overHuCard.sprite = sprite.Result;
                        }
                    };
                    overHuCard.gameObject.SetActive(true);
                    overHuFlag.gameObject.SetActive(true);
                }
                n32 nX = 0;
                for (n32 j = 0; j < nWeaveCount; j++)
                {
                    CWeaveItem weaveItem = m_SGameEnd.WeaveItemArray[i][j];
                    CBindMeld weave = null;
                    if (weaveItem.WeaveKind == WIK_G)
                    {
                        weave = Instantiate(m_GamePanel.m_Gang0Meld, overHandCard);
                    }
                    if (weaveItem.WeaveKind == WIK_P)
                    {
                        weave = Instantiate(m_GamePanel.m_Peng0Meld, overHandCard);
                    }
                    weave.transform.localScale = new Vector2(0.6f, 0.6f);
                    weave.transform.localPosition = new Vector2(nX, 0);
                    n32 nProvideViewID = m_GamePanel.GetGameEngine().SwitchViewChairID(weaveItem.ProvideUser, m_GamePanel.GetMeChairID());
                    Addressables.LoadAssetAsync<Sprite>(CUIHelper.GetDiscardCardImagePath(0, weaveItem.CenterCard)).Completed += (AsyncOperationHandle<Sprite> sprite) =>
                    {
                        if (sprite.Status == AsyncOperationStatus.Succeeded && weave != null)
                        {
                            switch (nViewID)
                            {
                            case 0:
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
                                break;
                            case 1:
                                switch (nProvideViewID)
                                {
                                case 0:
                                    weave.m_CenterImage.sprite = sprite.Result;
                                    weave.m_LeftImage.sprite = sprite.Result;
                                    break;
                                case 1:
                                    weave.m_CenterImage.sprite = sprite.Result;
                                    if (weaveItem.PublicCard)
                                    {
                                        weave.m_LeftImage.sprite = sprite.Result;
                                        weave.m_RightImage.sprite = sprite.Result;
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
                                break;
                            case 2:
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
                                    weave.m_CenterImage.sprite = sprite.Result;
                                    if (weaveItem.PublicCard)
                                    {
                                        weave.m_LeftImage.sprite = sprite.Result;
                                        weave.m_RightImage.sprite = sprite.Result;
                                    }
                                    break;
                                case 3:
                                    weave.m_LeftImage.sprite = sprite.Result;
                                    weave.m_CenterImage.sprite = sprite.Result;
                                    break;
                                default:
                                    break;
                                }
                                break;
                            case 3:
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
                                    weave.m_CenterImage.sprite = sprite.Result;
                                    if (weaveItem.PublicCard)
                                    {
                                        weave.m_LeftImage.sprite = sprite.Result;
                                        weave.m_RightImage.sprite = sprite.Result;
                                    }
                                    break;
                                default:
                                    break;
                                }
                                break;
                            default:
                                break;
                            }
                        }
                    };

                    nX += 132;
                }
                for (n32 j = 0; j < MAX_COUNT - 1 - 3 * nWeaveCount; j++)
                {
                    Image card = Instantiate(m_GamePanel.m_DiscardCardImage[0], overHandCard);
                    Addressables.LoadAssetAsync<Sprite>(CUIHelper.GetDiscardCardImagePath(0, m_SGameEnd.CardData[i][j])).Completed += (AsyncOperationHandle<Sprite> sprite) =>
                    {
                        if (sprite.Status == AsyncOperationStatus.Succeeded && card != null)
                        {
                            card.sprite = sprite.Result;
                            card.gameObject.SetActive(true);
                        }
                    };
                    card.rectTransform.localPosition = new Vector2(nX, 0);
                    card.rectTransform.localScale = new Vector2(0.6f, 0.6f);
                    nX += 43;
                }
            }
            for (n32 i = m_GamePanel.GetGameEngine().GetPlayerCount(); i < m_PlayerView.Length; i++)
            {
                m_PlayerView[i].gameObject.SetActive(false);
            }
        }
    }
}
