using UnityEngine;
using UnityEngine.UI;

public class UITitleMenu : UIWindow
{
    [SerializeField]
    private Button _btnStart = null;
    [SerializeField]
    private Button _btnExit = null;
    [SerializeField]
    private Button _btnLoad = null;
    [SerializeField]
    public GameObject _UIBack = null;
    [SerializeField]
    private Button _btnCollect = null;
    [SerializeField]
    private Button _btnSetting = null;
    private void Awake()
    {
        _btnStart.onClick.AddListener(OnClickStart);
        _btnLoad.onClick.AddListener(OnClickMenuPanel);
        _btnCollect.onClick.AddListener(OnClicCollection);
        _btnSetting.onClick.AddListener(OnClickSetting);
        if (IsNeedExitMenu())
            _btnExit.onClick.AddListener(OnClickExit);
        else
            _btnExit.gameObject.SetActive(false);
    }
    public override bool OnKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayerPrefs.DeleteAll();
           return true;
        }
        return false;
    }
    private void OnClickStart()
    {
        PlaySelectSound();
        TitleSystem._Instance._UIManager.CloseTitleMenu();
        string[] newInfo = new string[10];
        newInfo[0] = "1"; // 주차
        newInfo[1] = "0"; // 소지금
        newInfo[2] = "[0,0,0,0,0,0]"; // 소지 가구 리스트 0: 미소지, 1: 소지
        newInfo[3] = "0";// 가구 보너스(수익)
        newInfo[4] = "0";// 누적 리뷰수
        newInfo[5] = "0";// 가구 보너스(손님)
        newInfo[6] = "0";// 카페 오픈 여부
        newInfo[7] = "Title";// 세이브한 씬
        newInfo[8] = "0";// 누적 방문자
        newInfo[9] = "[1,0,0,0]"; //서브시나리오리스트
        string newString = string.Join("|", newInfo);
        PlayerPrefs.SetString("1" + "_cafe", newString);
        PlayerPrefs.SetString("ONReview", "False");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    private void OnClickExit()
    {
        PlaySelectSound();
        TitleSystem._Instance.DoExit();
    }
    private void OnClicCollection()
    {   //컬렉션 버튼 눌렀을때 메뉴패널, 메뉴 상단 페이지바, 메뉴 맨뒤에 배경 true
        //타이틀 메뉴 버튼들 false
        TitleSystem._Instance._UIManager._Extra.SetActivate(true);
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.SetActivate(true);
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.setMenuText("Extra / 추가요소","수집된 요소나 보너스로 제공된 이야기를 볼 수 있습니다.");
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.gameObject.transform.Find("Page").transform.Find("Page").gameObject.SetActive(true);
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.gameObject.transform.Find("Page").transform.Find("Prev").gameObject.SetActive(true);
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.gameObject.transform.Find("Page").transform.Find("Next").gameObject.SetActive(true);
        TitleSystem._Instance._UIManager._UIBack.SetActive(true);
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.transform.Find("Page").gameObject.SetActive(false);
        TitleSystem._Instance._UIManager._TitleMenu.SetActivate(false);
    }
    private void OnClickMenuPanel()
    {
        PlaySelectSound();
        
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.SetActivate(true);
        TitleSystem._Instance._UIManager._Extra.SetActivate(false);
        TitleSystem._Instance._UIManager._uiTitleLoadMenu.SetActivate(true);
        TitleSystem._Instance._UIManager._TitleMenu.SetActivate(false);
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.setMenuText("불러오기", "게임을 불러옵니다.");
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.transform.Find("Page").gameObject.SetActive(true);
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.GetComponent<UITitlePage>().Max = 4;
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.gameObject.transform.Find("Page").transform.Find("Page").gameObject.SetActive(true);
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.gameObject.transform.Find("Page").transform.Find("Prev").gameObject.SetActive(true);
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.gameObject.transform.Find("Page").transform.Find("Next").gameObject.SetActive(true);
        TitleSystem._Instance._UIManager._UIBack.SetActive(true);
    }

    private void OnClickSetting()
    {
        PlaySelectSound();
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.setMenuText("Setting / 환경설정");
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.SetActivate(true);
        TitleSystem._Instance._UIManager._uiTitleLoadMenu.SetActivate(false);
        TitleSystem._Instance._UIManager._TitleMenu.SetActivate(false);
        TitleSystem._Instance._UIManager._UIBack.SetActive(true);
        TitleSystem._Instance._UIManager._uiTitleSetting.SetActivate(true);
        //환경설정 창 눌렀을때 사용자 세팅에 따라 이미지 세팅
        TitleSystem._Instance._UIManager._uiTitleSetting.DialogueModeSpr(PlayerPrefs.GetString("automode"));
        // 상단바 필요 없으므로 false
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.gameObject.transform.Find("Page").transform.Find("Page").gameObject.SetActive(false);
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.gameObject.transform.Find("Page").transform.Find("Prev").gameObject.SetActive(false);
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.gameObject.transform.Find("Page").transform.Find("Next").gameObject.SetActive(false);
        
    }

    private void PlaySelectSound() => SoundManager._Instance.PlaySound(Define._menuSelectSound);

    private bool IsNeedExitMenu() => Application.platform != RuntimePlatform.IPhonePlayer;

}