using UnityEngine.SceneManagement;

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
    public class CLoadingPanel : CBaseScene
    {
        // Start is called before the first frame update
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            update();
        }

        public void OnClickLogon()
        {
            CAlertDlg alertDlg = Instantiate(m_AlertDlg, m_AlertPanelRectTransform);
            alertDlg.SetAlertType(CAlertDlg.EAlertType.kAlert);
            alertDlg.SetCallback(onClickAlertDlgYes, null);
            alertDlg.SetText(@"欢迎运行此游戏，此游戏是基于Unity的开源单机麻将，
本游戏使用的资源全部收集自互联网，若侵犯了您的权益请联系我进行删除。
希望此游戏源码可以帮助更多的人快速入门使用C#开发Unity，本项目地址：
https://github.com/dnasdw/majiang0
原文：
欢迎运行此游戏，此游戏是基于Cocos2d-X的开源单机麻将，
本游戏使用的资源全部收集自互联网，若侵犯了您的权益请联系我进行删除。
希望此游戏源码可以帮助更多的人快速入门使用CPP开发Cocos2d-X，获取更多软件与游戏开发信息请关注我的博客：
https://www.xiyoufang.com");
            CDialogManager.Instance.ShowDialog(alertDlg);
        }

        private void onClickAlertDlgYes()
        {
            CDialogManager.Instance.CloseAllDialog();
            SceneManager.LoadSceneAsync("Scenes/Play");
        }
    }
}
