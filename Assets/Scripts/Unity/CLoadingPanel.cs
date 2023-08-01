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
            alertDlg.SetText(@"��ӭ���д���Ϸ������Ϸ�ǻ���Unity�Ŀ�Դ�����齫��
����Ϸʹ�õ���Դȫ���ռ��Ի����������ַ�������Ȩ������ϵ�ҽ���ɾ����
ϣ������ϷԴ����԰���������˿�������ʹ��C#����Unity������Ŀ��ַ��
https://github.com/dnasdw/majiang0
ԭ�ģ�
��ӭ���д���Ϸ������Ϸ�ǻ���Cocos2d-X�Ŀ�Դ�����齫��
����Ϸʹ�õ���Դȫ���ռ��Ի����������ַ�������Ȩ������ϵ�ҽ���ɾ����
ϣ������ϷԴ����԰���������˿�������ʹ��CPP����Cocos2d-X����ȡ�����������Ϸ������Ϣ���ע�ҵĲ��ͣ�
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
