using System;
using System.Collections;
using UnityEngine;
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
    public class CAIEngine : IGameEngineEventListener
    {
        private MonoBehaviour m_MonoBehaviour = null;

        private CGameEngine m_GameEngine = null;
        private CPlayer m_MePlayer = null;
        private u8 m_uSendCardData = 0;

        private CWeaveItem[][] m_WeaveItemArray = CJaggedArray.CreateInstance<CWeaveItem[][], CWeaveItem>(GAME_PLAYER, MAX_WEAVE);
        private u8[][] m_uCardIndex = CJaggedArray.CreateInstance<u8[][]>(GAME_PLAYER, MAX_INDEX);
        private n32[] m_nWeaveItemCount = new n32[GAME_PLAYER];
        private n32[] m_nDiscardCount = new n32[GAME_PLAYER];
        private u8[][] m_uDiscardCard = CJaggedArray.CreateInstance<u8[][]>(GAME_PLAYER, MAX_DISCARD);
        private n32 m_nLeftCardCount = 0;
        private n32 m_nBankerChair = INVALID_CHAIR;
        private n32 m_nMeChairID = INVALID_CHAIR;

        public CAIEngine(MonoBehaviour a_MonoBehaviour)
        {
            m_MonoBehaviour = a_MonoBehaviour;

            m_GameEngine = CGameEngine.Instance;
            m_uSendCardData = 0;
            m_nMeChairID = INVALID_CHAIR;
            InitGame();
        }

        public void InitGame()
        {
            m_uSendCardData = 0;
            m_nLeftCardCount = 0;
            m_nBankerChair = INVALID_CHAIR;
            m_nWeaveItemCount.RecursiveClear();
            m_nDiscardCount.RecursiveClear();
            m_uCardIndex.RecursiveClear();
            m_WeaveItemArray.RecursiveClear<CWeaveItem>();
            m_uDiscardCard.RecursiveClear();
        }

        public void SetPlayer(CPlayer a_Player)
        {
            m_MePlayer = a_Player;
        }

        public bool OnUserEnterEvent(CPlayer a_Player)
        {
            m_nMeChairID = m_MePlayer.GetChairID();
            return true;
        }

        public bool OnGameStartEvent(CCMD_S_GameStart a_SGameStart)
        {
            Debug.Log("机器人接收到游戏开始事件");
            InitGame();
            m_nLeftCardCount = a_SGameStart.LeftCardCount;
            m_nBankerChair = a_SGameStart.BankerUser;
            CGameLogic.SwitchToCardIndex(new ArraySegment<u8>(a_SGameStart.CardData, MAX_COUNT * m_nMeChairID, MAX_COUNT), MAX_COUNT - 1, m_uCardIndex[m_nMeChairID]);
            return true;
        }

        public bool OnSendCardEvent(CCMD_S_SendCard a_SSendCard)
        {
            m_nLeftCardCount--;
            if (a_SSendCard.CurrentUser == m_nMeChairID)
            {
                Debug.Log("机器人接收到发牌事件");
                m_uCardIndex[m_nMeChairID][CGameLogic.SwitchToCardIndex(a_SSendCard.CardData)]++;
                m_uSendCardData = a_SSendCard.CardData;
                if (a_SSendCard.ActionMask != WIK_NULL)
                {
                    CCMD_S_OperateNotify SOperateNotify = new CCMD_S_OperateNotify();
                    SOperateNotify.ActionMask = a_SSendCard.ActionMask;
                    SOperateNotify.ActionCard = a_SSendCard.CardData;
                    SOperateNotify.GangCount = a_SSendCard.GangCount;
                    Array.Copy(a_SSendCard.GangCard, SOperateNotify.GangCard, SOperateNotify.GangCard.Length);
                    OnOperateNotifyEvent(SOperateNotify);
                }
                else
                {
                    m_MonoBehaviour.StartCoroutine(sendCard(DateTime.Now.Second % 2 + 0.8f));
                }
            }
            return true;
        }

        public bool OnOutCardEvent(CCMD_S_OutCard a_SOutCard)
        {
            Debug.Log(Format("出牌人 {0:d}，坐席 {1:d}", a_SOutCard.OutCardUser, m_nMeChairID));
            if (a_SOutCard.OutCardUser == m_nMeChairID)
            {
                Debug.Log("机器人接收到出牌事件");
                m_uCardIndex[m_nMeChairID][CGameLogic.SwitchToCardIndex(a_SOutCard.OutCardData)]--;
            }
            m_uDiscardCard[a_SOutCard.OutCardUser][m_nDiscardCount[a_SOutCard.OutCardUser]++] = a_SOutCard.OutCardData;
            return true;
        }

        public bool OnOperateNotifyEvent(CCMD_S_OperateNotify a_SOperateNotify)
        {
            Debug.Log("机器人接收到操作通知事件");
            if (a_SOperateNotify.ActionMask == WIK_NULL)
            {
                return true;
            }
            CCMD_C_OperateCard COperateCard = new CCMD_C_OperateCard();
            COperateCard.OperateUser = m_nMeChairID;
            if ((a_SOperateNotify.ActionMask & WIK_H) != 0)
            {
                COperateCard.OperateCode = WIK_H;
                COperateCard.OperateCard = a_SOperateNotify.ActionCard;
            }
            else if ((a_SOperateNotify.ActionMask & WIK_G) != 0)
            {
                COperateCard.OperateCode = WIK_G;
                COperateCard.OperateCard = a_SOperateNotify.GangCard[0];
            }
            else if ((a_SOperateNotify.ActionMask & WIK_P) != 0)
            {
                COperateCard.OperateCode = WIK_P;
                COperateCard.OperateCard = a_SOperateNotify.ActionCard;
            }
            return m_GameEngine.OnUserOperateCard(COperateCard);
        }

        public bool OnOperateResultEvent(CCMD_S_OperateResult a_SOperateResult)
        {
            Debug.Log("机器人接收到操作结果事件");
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
                    if (a_SOperateResult.OperateUser == m_nMeChairID)
                    {
                        u8[] uRemoveCard = { a_SOperateResult.OperateCard, a_SOperateResult.OperateCard };
                        CGameLogic.RemoveCard(m_uCardIndex[a_SOperateResult.OperateUser], uRemoveCard, uRemoveCard.Length);
                        u8[] uTempCardData = new u8[MAX_COUNT];
                        CGameLogic.SwitchToCardData(m_uCardIndex[m_nMeChairID], uTempCardData, MAX_COUNT - 1 - m_nWeaveItemCount[m_nMeChairID] * 3);
                        m_uSendCardData = uTempCardData[0];
                        m_MonoBehaviour.StartCoroutine(sendCard(DateTime.Now.Second % 2 + 0.5f));
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
                    if (a_SOperateResult.OperateUser == m_nMeChairID)
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
            return true;
        }

        public bool OnGameEndEvent(CCMD_S_GameEnd a_SGameEnd)
        {
            Debug.Log("机器人接收到游戏结束事件");
            return true;
        }

        private IEnumerator sendCard(f32 a_fTime)
        {
            yield return new WaitForSeconds(a_fTime);
            Debug.Log(Format("机器人出牌:{0:x}", m_uSendCardData));
            CCMD_C_OutCard COutCard = new CCMD_C_OutCard();
            COutCard.CardData = m_uSendCardData;
            m_GameEngine.OnUserOutCard(COutCard);
        }
    }
}
