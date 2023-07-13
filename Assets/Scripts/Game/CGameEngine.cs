using System;
using sdw.game;

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
    public class CGameEngine
    {
        private IStorageManager m_StorageManager = null;

        public static CGameEngine Instance { get; } = CSpecializer<CGameEngine>.GetDefaultValue();

        private CPlayer[] m_Player = new CPlayer[GAME_PLAYER];
        private n64[] m_nGameScoreTable = new n64[GAME_PLAYER];
        private n32 m_nDiceCount = 0;
        private n32 m_nCurrChair = 0;

        private n32 m_nBankerUser = INVALID_CHAIR;
        private u8[][] m_uCardIndex = CJaggedArray.CreateInstance<u8[][]>(GAME_PLAYER, MAX_INDEX);
        private u8 m_uProvideCard = 0;
        private n32 m_nProvideUser = INVALID_CHAIR;
        private n32 m_nResumeUser = INVALID_CHAIR;
        private n32 m_nCurrentUser = INVALID_CHAIR;
        private n32 m_nOutCardUser = INVALID_CHAIR;
        private u8 m_uOutCardData = 0;
        private n32[] m_nDiscardCount = new n32[GAME_PLAYER];
        private u8[][] m_uDiscardCard = CJaggedArray.CreateInstance<u8[][]>(GAME_PLAYER, MAX_DISCARD);

        private bool[] m_bResponse = new bool[GAME_PLAYER];
        private bool[][] m_bPassPeng = CJaggedArray.CreateInstance<bool[][]>(GAME_PLAYER, MAX_INDEX);
        private bool m_bGangStatus = false;
        private bool m_bQiangGangStatus = false;
        private u8[] m_uUserAction = new u8[GAME_PLAYER];
        private u8[] m_uTempUserAction = new u8[GAME_PLAYER];
        private u8[] m_uOperateCard = new u8[GAME_PLAYER];
        private u8[] m_uPerformAction = new u8[GAME_PLAYER];
        private n32[] m_nFanShu = new n32[GAME_PLAYER];
        private n32 m_nGangCount = 0;
        private u8[] m_uGangCard = new u8[MAX_WEAVE];
        private n32 m_nTargetUser = 0;
        private u8 m_uHuCard = 0;
        private u8[] m_uHuKind = new u8[GAME_PLAYER];
        private u8[] m_uHuSpecial = new u8[GAME_PLAYER];
        private u64[] m_uHuRight = new u64[GAME_PLAYER];

        private u8 m_uSendCardData = 0;
        private n32 m_nSendCardCount = 0;
        private n32 m_nOutCardCount = 0;
        private n32 m_nLeftCardCount = 0;
        private u8[] m_uRepertoryCard = new u8[MAX_REPERTORY];
        private n32[] m_nWeaveItemCount = new n32[GAME_PLAYER];
        private CWeaveItem[][] m_WeaveItemArray = CJaggedArray.CreateInstance<CWeaveItem[][], CWeaveItem>(GAME_PLAYER, MAX_WEAVE);

        public CGameEngine()
        {
            srand((static_cast_u32)(DateTimeOffset.UtcNow.ToUnixTimeSeconds()));

            m_nCurrChair = 0;
            m_nBankerUser = INVALID_CHAIR;
            Init();
        }

        public void Init(IStorageManager a_StorageManager, IAudioSource a_AudioSource)
        {
            m_StorageManager = a_StorageManager;
            m_StorageManager.Init();
            SGameState? gameState = m_StorageManager.GetGameState();
            if (gameState.HasValue)
            {
                CGameData.m_GameState = gameState.Value;
            }
            else
            {
                CGameData.m_GameState = new SGameState() { Score = new n64[4], Volume = 0.8f, IsShowAllMahjong = false };
            }
            Array.Copy(CGameData.m_GameState.Score, m_nGameScoreTable, 4);
            a_AudioSource.SetVolume(CGameData.m_GameState.Volume);
            CDebugValue.SetIsShowAllMahjong(CGameData.m_GameState.IsShowAllMahjong);
            m_StorageManager.SetGameState(CGameData.m_GameState);
        }

        public void Init()
        {
            m_nDiceCount = 0;
            m_nOutCardUser = INVALID_CHAIR;
            m_uOutCardData = 0;
            m_nDiscardCount.RecursiveClear();
            m_bResponse.RecursiveClear();
            m_bGangStatus = false;
            m_bQiangGangStatus = false;
            m_nGangCount = 0;
            m_nTargetUser = 0;
            m_uHuCard = 0;
            m_nOutCardCount = 0;
            m_nLeftCardCount = 0;
            m_nCurrentUser = INVALID_CHAIR;
            m_nProvideUser = INVALID_CHAIR;
            m_nResumeUser = INVALID_CHAIR;
            m_uProvideCard = 0;
            m_nSendCardCount = 0;
            m_uSendCardData = 0;
            m_uGangCard.RecursiveClear();
            m_uHuRight.RecursiveClear();
            m_uHuKind.RecursiveClear();
            m_uHuSpecial.RecursiveClear();
            m_uTempUserAction.RecursiveClear();
            m_nWeaveItemCount.RecursiveClear();
            m_uUserAction.RecursiveClear();
            m_uOperateCard.RecursiveClear();
            m_uPerformAction.RecursiveClear();
            m_nFanShu.RecursiveClear();
            m_uCardIndex.RecursiveClear();
            m_bPassPeng.RecursiveClear();
            m_uDiscardCard.RecursiveClear();
            m_WeaveItemArray.RecursiveClear<CWeaveItem>();
        }

        public bool OnGameStart()
        {
            CGameLogic.Shuffle(m_uRepertoryCard);
            m_nDiceCount = rand() % 6 + 1 + rand() % 6 + 1;
            if (m_nBankerUser == INVALID_CHAIR)
            {
                m_nBankerUser = m_nDiceCount % GAME_PLAYER;
            }
            m_nLeftCardCount = m_uRepertoryCard.Length;
            for (n32 i = 0; i < m_nCurrChair; i++)
            {
                m_nLeftCardCount -= MAX_COUNT - 1;
                CGameLogic.SwitchToCardIndex(new ArraySegment<u8>(m_uRepertoryCard, m_nLeftCardCount, MAX_COUNT - 1), MAX_COUNT - 1, m_uCardIndex[i]);
            }
            m_uProvideCard = 0;
            m_nProvideUser = INVALID_CHAIR;
            m_nCurrentUser = m_nBankerUser;
            CCMD_S_GameStart SGameStart = new CCMD_S_GameStart();
            SGameStart.DiceCount = m_nDiceCount;
            SGameStart.BankerUser = m_nBankerUser;
            SGameStart.CurrentUser = m_nCurrentUser;
            SGameStart.LeftCardCount = m_nLeftCardCount;

            for (n32 i = 0; i < m_nCurrChair; i++)
            {
                for (n32 j = 0; j < GAME_PLAYER; j++)
                {
                    CGameLogic.SwitchToCardData(m_uCardIndex[j], new ArraySegment<u8>(SGameStart.CardData, MAX_COUNT * j, MAX_COUNT), MAX_COUNT);
                }
                IGameEngineEventListener listener = m_Player[i].GetGameEngineEventListener();
                if (listener != null)
                {
                    listener.OnGameStartEvent(SGameStart);
                }
            }
            DispatchCardData(m_nCurrentUser);
            return true;
        }

        public bool OnGameRestart()
        {
            Init();
            OnGameStart();
            return true;
        }

        public bool OnUserOutCard(CCMD_C_OutCard a_COutCard)
        {
            if (m_uUserAction[m_nCurrentUser] != WIK_NULL)
            {
#if UNITY
                UnityEngine.Debug.Log("存在操作不允许出牌，需要等操作结束");
                for (n32 i = 0; i < GAME_PLAYER; i++)
                {
                    UnityEngine.Debug.Log(Format("坐席 {0:d} 的状态是 {1:d}", i, m_uUserAction[i]));
                }
#endif
                return true;
            }
            if (!CGameLogic.RemoveCard(m_uCardIndex[m_nCurrentUser], a_COutCard.CardData))
            {
#if UNITY
                UnityEngine.Debug.Log("removeCard fail");
#endif
                return true;
            }
            m_nProvideUser = m_nCurrentUser;
            m_uProvideCard = a_COutCard.CardData;
            m_nGangCount = 0;
            m_uGangCard.RecursiveClear();
            m_bGangStatus = false;
            m_bQiangGangStatus = false;
            m_uTempUserAction.RecursiveClear();
            m_nTargetUser = 0;
            m_uUserAction[m_nCurrentUser] = WIK_NULL;
            m_uPerformAction[m_nCurrentUser] = WIK_NULL;
            m_nOutCardCount++;
            m_nOutCardUser = m_nCurrentUser;
            m_uOutCardData = a_COutCard.CardData;

            CCMD_S_OutCard SOutCard = new CCMD_S_OutCard();
            SOutCard.OutCardUser = m_nProvideUser;
            SOutCard.OutCardData = a_COutCard.CardData;
            for (n32 i = 0; i < GAME_PLAYER; i++)
            {
                m_Player[i].GetGameEngineEventListener().OnOutCardEvent(SOutCard);
            }
            m_nResumeUser = (m_nCurrentUser + m_nCurrChair - 1) % m_nCurrChair;
            bool bAroseAction = EstimateUserRespond(m_nCurrentUser, a_COutCard.CardData, EEstimateKind.kOutCard);
            if (!bAroseAction)
            {
                m_nCurrentUser = m_nResumeUser;
                DispatchCardData(m_nCurrentUser);
            }
            else
            {
#if UNITY
                UnityEngine.Debug.Log("???");
#endif
            }
            return true;
        }

        public bool OnEventGameConclude()
        {
#if UNITY
            UnityEngine.Debug.Log("----游戏结束----");
#endif
            n32 m_nLastBankerUser = m_nBankerUser;

            CCMD_S_GameEnd SGameEnd = new CCMD_S_GameEnd();
            SGameEnd.ProvideUser = m_nProvideUser;
            SGameEnd.HuUser = m_nTargetUser;
            SGameEnd.HuCard = m_uHuCard;
            for (n32 i = 0; i < GAME_PLAYER; i++)
            {
                SGameEnd.CardCount[i] = CGameLogic.SwitchToCardData(m_uCardIndex[i], SGameEnd.CardData[i], MAX_COUNT);
                SGameEnd.HuRight[i] = m_uHuRight[i];
                SGameEnd.HuKind[i] = m_uHuKind[i];
                SGameEnd.HuSpecial[i] = m_uHuSpecial[i];
                SGameEnd.WeaveCount[i] = m_nWeaveItemCount[i];
                CJaggedArray.Copy(m_WeaveItemArray[i], SGameEnd.WeaveItemArray[i], m_WeaveItemArray[i].Length);
            }
            for (n32 i = 0; i < GAME_PLAYER; i++)
            {
                for (n32 j = 0; j < m_nWeaveItemCount[i]; j++)
                {
                    if (m_WeaveItemArray[i][j].WeaveKind == WIK_G)
                    {
                        if (m_WeaveItemArray[i][j].PublicCard && m_WeaveItemArray[i][j].ProvideUser != i)
                        {
                            n32 nProvideUser = m_WeaveItemArray[i][j].ProvideUser;
                            n32 nGangScore = 1;
                            SGameEnd.GameScore[nProvideUser] -= nGangScore;
                            SGameEnd.GameScore[i] += nGangScore;

                            SGameEnd.NormalGameScore[nProvideUser] -= nGangScore;
                            SGameEnd.NormalGameScore[i] += nGangScore;
                        }
                        else if (!m_WeaveItemArray[i][j].PublicCard && m_WeaveItemArray[i][j].ProvideUser == i)
                        {
                            for (n32 k = 0; k < GAME_PLAYER; k++)
                            {
                                if (k != i)
                                {
                                    n32 nGangScore = 2;
                                    SGameEnd.GameScore[k] -= nGangScore;
                                    SGameEnd.GameScore[i] += nGangScore;
                                    SGameEnd.NormalGameScore[k] -= nGangScore;
                                    SGameEnd.NormalGameScore[i] += nGangScore;
                                }
                            }
                        }
                    }
                }
            }

            if (m_nTargetUser != 0 && m_nProvideUser != INVALID_CHAIR)
            {
                if (m_uHuRight[m_nProvideUser] != 0 && (m_nTargetUser & SDW_BIT32(m_nProvideUser)) != 0)
                {
                    n32 nChiHuOrder = CGameLogic.GetHuFanShu(m_uHuRight[m_nProvideUser], m_uHuKind[m_nProvideUser], m_uHuSpecial[m_nProvideUser]);
                    for (n32 i = 0; i < m_nCurrChair; i++)
                    {
                        if (i != m_nProvideUser)
                        {
                            SGameEnd.GameScore[i] -= nChiHuOrder;
                            SGameEnd.GameScore[m_nProvideUser] += nChiHuOrder;
                            SGameEnd.NormalGameScore[i] -= nChiHuOrder;
                            SGameEnd.NormalGameScore[m_nProvideUser] += nChiHuOrder;
                        }
                    }
                    m_nBankerUser = m_nProvideUser;
                }
                else
                {
                    n32 nDistance = 0;
                    for (n32 i = 0; i < m_nCurrChair; i++)
                    {
                        if (i == m_nProvideUser)
                        {
                            continue;
                        }
                        if (m_uHuRight[i] != 0 && i != m_nProvideUser && (m_nTargetUser & SDW_BIT32(i)) != 0)
                        {
                            n32 nChiHuOrder = CGameLogic.GetHuFanShu(m_uHuRight[i], m_uHuKind[i], m_uHuSpecial[i]);
                            if ((m_uHuSpecial[i] & CHS_DH) != 0)
                            {
                                for (n32 j = 0; j < m_nCurrChair; j++)
                                {
                                    if (j != i)
                                    {
                                        SGameEnd.GameScore[j] -= nChiHuOrder;
                                        SGameEnd.GameScore[i] += nChiHuOrder;

                                        SGameEnd.NormalGameScore[j] -= nChiHuOrder;
                                        SGameEnd.NormalGameScore[i] += nChiHuOrder;
                                    }
                                }
                            }
                            else
                            {
                                SGameEnd.GameScore[m_nProvideUser] -= nChiHuOrder;
                                SGameEnd.GameScore[i] += nChiHuOrder;

                                SGameEnd.NormalGameScore[m_nProvideUser] -= nChiHuOrder;
                                SGameEnd.NormalGameScore[i] += nChiHuOrder;

                                if (m_uHuRight[i] != 0 && (m_uHuKind[i] & CHK_QG) != 0)
                                {
                                    for (n32 j = 0; j < m_nCurrChair; j++)
                                    {
                                        if (j != i && j != m_nProvideUser)
                                        {
                                            SGameEnd.GameScore[m_nProvideUser] -= nChiHuOrder;
                                            SGameEnd.GameScore[i] += nChiHuOrder;
                                            SGameEnd.NormalGameScore[m_nProvideUser] -= nChiHuOrder;
                                            SGameEnd.NormalGameScore[i] += nChiHuOrder;
                                        }
                                    }
                                }
                            }
                            n32 nDistanceTemp = m_nProvideUser < i ? i - m_nProvideUser : i + 4 - m_nProvideUser;
                            if (nDistance < nDistanceTemp)
                            {
                                nDistance = nDistanceTemp;
                                m_nBankerUser = i;
                            }
                        }
                    }
                }
            }
            else
            {
                SGameEnd.HuUser = 0;
            }
            for (n32 i = 0; i < GAME_PLAYER; i++)
            {
                m_nGameScoreTable[i] += SGameEnd.GameScore[i];
                SGameEnd.GameScoreTable[i] = m_nGameScoreTable[i];
            }
            for (n32 i = 0; i < m_nCurrChair; i++)
            {
                m_Player[i].GetGameEngineEventListener().OnGameEndEvent(SGameEnd);
            }
            Array.Copy(m_nGameScoreTable, CGameData.m_GameState.Score, 4);
            m_StorageManager.SetGameState(CGameData.m_GameState);
            return true;
        }

        public bool OnUserEnter(CPlayer a_Player)
        {
            if (m_nCurrChair >= GAME_PLAYER)
            {
#if UNITY
                UnityEngine.Debug.Log("玩家已满，无法加入！");
#endif
                return false;
            }
            a_Player.SetChairID(m_nCurrChair);
            m_Player[m_nCurrChair++] = a_Player;
            for (n32 i = 0; i < m_nCurrChair; i++)
            {
                IGameEngineEventListener listener = m_Player[i].GetGameEngineEventListener();
                if (listener != null)
                {
                    listener.OnUserEnterEvent(a_Player);
                }
            }
            if (m_nCurrChair == GAME_PLAYER)
            {
                OnGameStart();
            }
            return true;
        }

        public bool DispatchCardData(n32 a_nCurrentUser, bool a_bTail = false)
        {
            if (m_nOutCardUser != INVALID_CHAIR && m_uOutCardData != 0)
            {
                m_nDiscardCount[m_nOutCardUser]++;
                m_uDiscardCard[m_nOutCardUser][m_nDiscardCount[m_nOutCardUser] - 1] = m_uOutCardData;
            }
            m_nTargetUser = 0;
            m_uOutCardData = 0;
            m_nOutCardUser = INVALID_CHAIR;
            m_nGangCount = 0;
            m_uGangCard.RecursiveClear();
            m_uHuRight.RecursiveClear();
            m_uHuKind.RecursiveClear();
            m_uHuSpecial.RecursiveClear();
            m_uTempUserAction.RecursiveClear();
            m_nCurrentUser = a_nCurrentUser;
            m_nFanShu[a_nCurrentUser] = 0;
            m_bPassPeng[a_nCurrentUser].RecursiveClear();
            if (m_nLeftCardCount == 0)
            {
                m_uHuCard = 0;
                m_nProvideUser = INVALID_CHAIR;
                OnEventGameConclude();
                return true;
            }
            m_nSendCardCount++;
            m_uSendCardData = m_uRepertoryCard[--m_nLeftCardCount];
            m_uCardIndex[a_nCurrentUser][CGameLogic.SwitchToCardIndex(m_uSendCardData)]++;
            m_nProvideUser = a_nCurrentUser;
            m_uProvideCard = m_uSendCardData;
            if (m_nLeftCardCount > 0)
            {
                CGangCardResult gangCardResult = new CGangCardResult();
                m_uUserAction[a_nCurrentUser] |= CGameLogic.AnalyseGangCard(m_uCardIndex[a_nCurrentUser], m_WeaveItemArray[a_nCurrentUser], m_nWeaveItemCount[a_nCurrentUser], gangCardResult);
                if ((m_uUserAction[a_nCurrentUser] & WIK_G) != 0)
                {
                    m_nGangCount = gangCardResult.CardCount;
                    Array.Copy(gangCardResult.CardData, m_uGangCard, m_uGangCard.Length);
                }
            }
            u8[] uTempCardIndex = new u8[MAX_INDEX];
            Array.Copy(m_uCardIndex[m_nCurrentUser], uTempCardIndex, uTempCardIndex.Length);
            CGameLogic.RemoveCard(uTempCardIndex, m_uSendCardData);
            m_uUserAction[a_nCurrentUser] |= CGameLogic.AnalyseHuCard(uTempCardIndex, m_WeaveItemArray[a_nCurrentUser], m_nWeaveItemCount[a_nCurrentUser], m_uSendCardData, ref m_uHuKind[a_nCurrentUser], ref m_uHuRight[a_nCurrentUser], ref m_uHuSpecial[a_nCurrentUser], m_nSendCardCount, m_nOutCardCount, m_bGangStatus, true, m_bQiangGangStatus, m_nFanShu[a_nCurrentUser], false);
            if (m_uUserAction[a_nCurrentUser] != WIK_NULL)
            {
                m_uTempUserAction[a_nCurrentUser] = m_uUserAction[a_nCurrentUser];
            }
            CCMD_S_SendCard SSendCard = new CCMD_S_SendCard();
            SSendCard.CurrentUser = a_nCurrentUser;
            SSendCard.ActionMask = m_uUserAction[a_nCurrentUser];
            SSendCard.CardData = m_uSendCardData;
            SSendCard.GangCount = m_nGangCount;
            Array.Copy(m_uGangCard, SSendCard.GangCard, m_uGangCard.Length);
            SSendCard.Tail = a_bTail;

            for (n32 i = 0; i < GAME_PLAYER; i++)
            {
                m_Player[i].GetGameEngineEventListener().OnSendCardEvent(SSendCard);
            }
            return true;
        }

        public bool EstimateUserRespond(n32 a_nCurrentUser, u8 a_uCurrentCard, EEstimateKind a_eEstimateKind)
        {
            bool bAroseAction = false;
            m_bResponse.RecursiveClear();
            m_uUserAction.RecursiveClear();
            m_uPerformAction.RecursiveClear();
            m_uHuRight.RecursiveClear();
            m_uHuKind.RecursiveClear();
            m_uHuSpecial.RecursiveClear();

            for (n32 i = 0; i < m_nCurrChair; i++)
            {
                if (a_nCurrentUser == i)
                {
                    continue;
                }
                if (a_eEstimateKind == EEstimateKind.kOutCard)
                {
                    if (!m_bPassPeng[i][CGameLogic.SwitchToCardIndex(a_uCurrentCard)])
                    {
                        m_uUserAction[i] |= CGameLogic.EstimatePengCard(m_uCardIndex[i], a_uCurrentCard);
                        if ((m_uUserAction[i] & WIK_P) != 0)
                        {
                            m_bPassPeng[i][CGameLogic.SwitchToCardIndex(a_uCurrentCard)] = true;
                        }
                    }
                    if (m_nLeftCardCount > 0)
                    {
                        m_uUserAction[i] |= CGameLogic.EstimateGangCard(m_uCardIndex[i], a_uCurrentCard);
                        if ((m_uUserAction[i] & WIK_G) != 0)
                        {
                            m_nGangCount = 1;
                            m_uGangCard[0] = a_uCurrentCard;
                        }
                    }
                }
                n32 nWeaveCount = m_nWeaveItemCount[i];
                m_uUserAction[i] |= CGameLogic.AnalyseHuCard(m_uCardIndex[i], m_WeaveItemArray[i], nWeaveCount, a_uCurrentCard, ref m_uHuKind[i], ref m_uHuRight[i], ref m_uHuSpecial[i], m_nSendCardCount, m_nOutCardCount, m_bGangStatus, false, m_bQiangGangStatus, m_nFanShu[i], false);
                if (m_uUserAction[i] != WIK_NULL)
                {
                    bAroseAction = true;
                }
            }
            if (bAroseAction)
            {
                m_nProvideUser = a_nCurrentUser;
                m_uProvideCard = a_uCurrentCard;
                m_nCurrentUser = INVALID_CHAIR;
                SendOperateNotify();
                return true;
            }
            return false;
        }

        public bool SendOperateNotify()
        {
            n32 nCount = 0;
            for (n32 i = 0; i < m_nCurrChair; i++)
            {
                if (m_uUserAction[i] != WIK_NULL)
                {
                    nCount++;
                    CCMD_S_OperateNotify SOperateNotify = new CCMD_S_OperateNotify();
                    SOperateNotify.ResumeUser = m_nResumeUser;
                    SOperateNotify.ActionCard = m_uProvideCard;
                    SOperateNotify.ActionMask = m_uUserAction[i];
                    SOperateNotify.GangCount = m_nGangCount;
                    Array.Copy(m_uGangCard, SOperateNotify.GangCard, m_nGangCount);
                    m_Player[i].GetGameEngineEventListener().OnOperateNotifyEvent(SOperateNotify);
                }
            }
            return true;
        }

        public bool OnUserOperateCard(CCMD_C_OperateCard a_COperateCard)
        {
            n32 nChairID = a_COperateCard.OperateUser;
            u8 uOperateCode = a_COperateCard.OperateCode;
            u8 uOperateCard = a_COperateCard.OperateCard;
            if (m_nCurrentUser != nChairID && m_nCurrentUser != INVALID_CHAIR)
            {
                return true;
            }
            m_uHuRight.RecursiveClear();
            m_uHuKind.RecursiveClear();
            m_uHuSpecial.RecursiveClear();

            if (m_nCurrentUser == INVALID_CHAIR)
            {
                if (m_bResponse[nChairID])
                {
                    return true;
                }
                if (uOperateCode != WIK_NULL && (m_uUserAction[nChairID] & uOperateCode) == 0)
                {
                    return true;
                }
                n32 nTargetUser = nChairID;
                u8 uTargetAction = uOperateCode;
                if (uTargetAction != WIK_NULL)
                {
                    m_nTargetUser |= (static_cast_n32)(SDW_BIT32(nChairID));
                }
                n32 nTargetActionRank = CGameLogic.GetUserActionRank(uTargetAction);
                m_bResponse[nChairID] = true;
                m_uPerformAction[nChairID] = uOperateCode;
                m_uOperateCard[nChairID] = m_uProvideCard;

                for (n32 i = 0; i < m_nCurrChair; i++)
                {
                    if (i == nChairID)
                    {
                        continue;
                    }
                    u8 uUserAction = !m_bResponse[i] ? m_uUserAction[i] : m_uPerformAction[i];
                    if (uUserAction == WIK_NULL)
                    {
                        continue;
                    }
                    n32 nUserActionRank = CGameLogic.GetUserActionRank(uUserAction);
                    if (nUserActionRank > nTargetActionRank)
                    {
                        m_nTargetUser &= (static_cast_n32)(~SDW_BIT32(nTargetUser));
                        nTargetUser = i;
                        m_nTargetUser |= (static_cast_n32)(SDW_BIT32(nTargetUser));
                        uTargetAction = uUserAction;
                    }
                    if (nUserActionRank == nTargetActionRank)
                    {
                        if (uTargetAction != WIK_NULL)
                        {
                            m_nTargetUser |= (static_cast_n32)(SDW_BIT32(i));
                        }
                    }
                }
                for (n32 i = 0; i < m_nCurrChair; i++)
                {
                    if ((m_nTargetUser & SDW_BIT32(i)) != 0)
                    {
                        if (!m_bResponse[i])
                        {
                            return true;
                        }
                    }
                }

                if (uTargetAction == WIK_NULL)
                {
                    if (m_bQiangGangStatus)
                    {
                        m_bQiangGangStatus = false;
                    }
                    m_bResponse.RecursiveClear();
                    m_uUserAction.RecursiveClear();
                    m_uOperateCard.RecursiveClear();
                    m_uPerformAction.RecursiveClear();
                    DispatchCardData(m_nResumeUser);
                    return true;
                }
                u8 uTargetCard = m_uOperateCard[nTargetUser];
                m_uOutCardData = 0;
                if (uTargetAction == WIK_H)
                {
                    m_uHuCard = uTargetCard;
                    for (n32 i = 0; i < m_nCurrChair; i++)
                    {
                        if ((m_nTargetUser & SDW_BIT32(i)) != 0)
                        {
                            n32 nWeaveItemCount = m_nWeaveItemCount[i];
                            CWeaveItem[] weaveItem = m_WeaveItemArray[i];
                            CGameLogic.AnalyseHuCard(m_uCardIndex[i], weaveItem, nWeaveItemCount, m_uHuCard, ref m_uHuKind[i], ref m_uHuRight[i], ref m_uHuSpecial[i], m_nSendCardCount, m_nOutCardCount, m_bGangStatus, false, m_bQiangGangStatus, m_nFanShu[i], true);
                            if (m_uHuRight[i] != 0)
                            {
                                m_uCardIndex[i][CGameLogic.SwitchToCardIndex(m_uHuCard)]++;
                            }
                        }
                    }
                    if (m_bQiangGangStatus)
                    {
                        m_bQiangGangStatus = false;
                        for (n32 i = 0; i < m_nWeaveItemCount[m_nProvideUser]; i++)
                        {
                            u8 uWeaveKind = m_WeaveItemArray[m_nProvideUser][i].WeaveKind;
                            u8 uCenterCard = m_WeaveItemArray[m_nProvideUser][i].CenterCard;
                            if (uCenterCard == m_uProvideCard && uWeaveKind == WIK_G)
                            {
                                m_WeaveItemArray[m_nProvideUser][i].Valid = 0;
                            }
                        }
                    }
                    OnEventGameConclude();
                    return true;
                }

                m_bResponse.RecursiveClear();
                m_uUserAction.RecursiveClear();
                m_uOperateCard.RecursiveClear();
                m_uPerformAction.RecursiveClear();

                n32 nIndex = m_nWeaveItemCount[nTargetUser]++;
                m_WeaveItemArray[nTargetUser][nIndex].PublicCard = true;
                m_WeaveItemArray[nTargetUser][nIndex].CenterCard = uTargetCard;
                m_WeaveItemArray[nTargetUser][nIndex].WeaveKind = uTargetAction;
                m_WeaveItemArray[nTargetUser][nIndex].ProvideUser = m_nProvideUser;
                m_WeaveItemArray[nTargetUser][nIndex].Valid = 1;
                switch (uTargetAction)
                {
                case WIK_P:
                    {
                        u8[] uRemoveCard = { uTargetCard, uTargetCard };
                        CGameLogic.RemoveCard(m_uCardIndex[nTargetUser], uRemoveCard, uRemoveCard.Length);
                    }
                    break;
                case WIK_G:
                    {
                        u8[] uRemoveCard = { uTargetCard, uTargetCard, uTargetCard };
                        CGameLogic.RemoveCard(m_uCardIndex[nTargetUser], uRemoveCard, uRemoveCard.Length);
                        m_bGangStatus = true;
                    }
                    break;
                default:
                    break;
                }
                m_nCurrentUser = nTargetUser;
                CCMD_S_OperateResult SOperateResult = new CCMD_S_OperateResult();
                SOperateResult.OperateUser = nTargetUser;
                SOperateResult.OperateCard = uTargetCard;
                SOperateResult.OperateCode = uTargetAction;
                SOperateResult.ProvideUser = m_nProvideUser;
                m_nTargetUser = 0;
                for (n32 i = 0; i < m_nCurrChair; i++)
                {
                    m_Player[i].GetGameEngineEventListener().OnOperateResultEvent(SOperateResult);
                }
                if (uTargetAction == WIK_G)
                {
                    m_bQiangGangStatus = true;
                    m_nResumeUser = m_nCurrentUser;
                    bool bAroseAction = EstimateUserRespond(m_nCurrentUser, m_uProvideCard, EEstimateKind.kGangCard);
                    if (!bAroseAction)
                    {
                        m_bQiangGangStatus = false;
                        DispatchCardData(nChairID, true);
                    }
                }

                if (uTargetAction == WIK_P)
                {
                    if (m_nLeftCardCount > 0)
                    {
                        CGangCardResult gangCardResult = new CGangCardResult();
                        m_uUserAction[m_nCurrentUser] |= CGameLogic.AnalyseGangCard(m_uCardIndex[m_nCurrentUser], m_WeaveItemArray[m_nCurrentUser], m_nWeaveItemCount[m_nCurrentUser], gangCardResult);
                        if ((m_uUserAction[m_nCurrentUser] & WIK_G) != 0)
                        {
                            m_nGangCount = gangCardResult.CardCount;
                            Array.Copy(gangCardResult.CardData, m_uGangCard, m_uGangCard.Length);
                            SendOperateNotify();
                        }
                    }
                }
                return true;
            }

            if (m_nCurrentUser == nChairID)
            {
                if (uOperateCode != WIK_NULL && (m_uUserAction[nChairID] & uOperateCode) == 0)
                {
                    return true;
                }
                if (!CGameLogic.IsValidCard(uOperateCard))
                {
                    return true;
                }
                m_uUserAction[m_nCurrentUser] = WIK_NULL;
                m_uPerformAction[m_nCurrentUser] = WIK_NULL;

                switch (uOperateCode)
                {
                case WIK_G:
                    {
                        CGangCardResult gangCardResult = new CGangCardResult();
                        u8 uAction = CGameLogic.AnalyseGangCard(m_uCardIndex[nChairID], m_WeaveItemArray[nChairID], m_nWeaveItemCount[nChairID], gangCardResult);
                        if (uAction != WIK_G)
                        {
                            return true;
                        }
                        m_bGangStatus = true;
                        n32 nGangIndex = INVALID_BYTE;
                        for (n32 i = 0; i < gangCardResult.CardCount; i++)
                        {
                            if (gangCardResult.CardData[i] == uOperateCard)
                            {
                                nGangIndex = i;
                                break;
                            }
                        }
                        if (nGangIndex == INVALID_BYTE)
                        {
                            return true;
                        }
                        n32 nCardIndex = CGameLogic.SwitchToCardIndex(uOperateCard);
                        if (gangCardResult.Public[nGangIndex])
                        {
                            m_bQiangGangStatus = true;
                            for (n32 i = 0; i < m_nWeaveItemCount[m_nCurrentUser]; i++)
                            {
                                u8 uWeaveKind = m_WeaveItemArray[m_nCurrentUser][i].WeaveKind;
                                u8 uCenterCard = m_WeaveItemArray[m_nCurrentUser][i].CenterCard;
                                if (uCenterCard == uOperateCard && uWeaveKind == WIK_P)
                                {
                                    m_WeaveItemArray[m_nCurrentUser][i].PublicCard = true;
                                    m_WeaveItemArray[m_nCurrentUser][i].WeaveKind = uOperateCode;
                                    m_WeaveItemArray[m_nCurrentUser][i].CenterCard = uOperateCard;
                                    m_WeaveItemArray[m_nCurrentUser][i].Valid = 2;
                                    if (m_uSendCardData != uOperateCard)
                                    {
                                        m_WeaveItemArray[m_nCurrentUser][i].Valid = 0;
                                    }
                                    break;
                                }
                            }
                        }
                        if (!gangCardResult.Public[nGangIndex])
                        {
                            n32 nWeaveIndex = m_nWeaveItemCount[m_nCurrentUser]++;
                            m_WeaveItemArray[m_nCurrentUser][nWeaveIndex].PublicCard = false;
                            m_WeaveItemArray[m_nCurrentUser][nWeaveIndex].ProvideUser = m_nCurrentUser;
                            m_WeaveItemArray[m_nCurrentUser][nWeaveIndex].WeaveKind = uOperateCode;
                            m_WeaveItemArray[m_nCurrentUser][nWeaveIndex].CenterCard = uOperateCard;
                            m_WeaveItemArray[m_nCurrentUser][nWeaveIndex].Valid = 1;
                        }

                        m_uCardIndex[m_nCurrentUser][nCardIndex] = 0;
                        CCMD_S_OperateResult SOperateResult = new CCMD_S_OperateResult();
                        SOperateResult.OperateUser = m_nCurrentUser;
                        SOperateResult.ProvideUser = m_nCurrentUser;
                        SOperateResult.OperateCode = uOperateCode;
                        SOperateResult.OperateCard = uOperateCard;
                        m_nTargetUser = 0;
                        for (n32 i = 0; i < m_nCurrChair; i++)
                        {
                            m_Player[i].GetGameEngineEventListener().OnOperateResultEvent(SOperateResult);
                        }
                        bool bAroseAction = false;
                        if (gangCardResult.Public[nGangIndex])
                        {
                            m_nResumeUser = nChairID;
                            bAroseAction = EstimateUserRespond(nChairID, uOperateCard, EEstimateKind.kGangCard);
                        }
                        if (!bAroseAction)
                        {
                            m_bQiangGangStatus = false;
                            DispatchCardData(nChairID, true);
                        }
                    }
                    return true;
                case WIK_H:
                    {
                        m_uHuCard = uOperateCard;
                        n32 nWeaveItemCount = m_nWeaveItemCount[m_nCurrentUser];
                        CWeaveItem[] weaveItem = m_WeaveItemArray[m_nCurrentUser];
                        u8[] uTempCardIndex = new u8[MAX_INDEX];
                        Array.Copy(m_uCardIndex[m_nCurrentUser], uTempCardIndex, uTempCardIndex.Length);
                        CGameLogic.RemoveCard(uTempCardIndex, m_uHuCard);
                        CGameLogic.AnalyseHuCard(uTempCardIndex, weaveItem, nWeaveItemCount, m_uHuCard, ref m_uHuKind[nChairID], ref m_uHuRight[nChairID], ref m_uHuSpecial[nChairID], m_nSendCardCount, m_nOutCardCount, m_bGangStatus, true, m_bQiangGangStatus, m_nFanShu[nChairID], true);
                        m_nTargetUser |= (static_cast_n32)(SDW_BIT32(m_nCurrentUser));
                        OnEventGameConclude();
                    }
                    return true;
                default:
                    break;
                }
                return true;
            }

            return true;
        }

        public n32 SwitchViewChairID(n32 a_nChairID, n32 a_nMeChairID)
        {
            return (a_nChairID + m_nCurrChair - a_nMeChairID) % m_nCurrChair;
        }

        public CPlayer GetPlayer(n32 a_nChair)
        {
            if (a_nChair < GAME_PLAYER)
            {
                return m_Player[a_nChair];
            }
            return null;
        }

        public n32 GetPlayerCount()
        {
            return m_nCurrChair;
        }
    }
}
