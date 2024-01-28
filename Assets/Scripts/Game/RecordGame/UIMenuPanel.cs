using UnityEngine;
using Game;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMenuPanel : UIWindow
{
    [SerializeField]
    public GameObject _UIBack = null;

    [SerializeField]
    private Button _btnExit = null;
    [SerializeField]
    private Button _btnSave = null;
    [SerializeField]
    private Button _btnLoad = null;

    // 메뉴창 위치 고정
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            GameSystem._Instance._UI._menuPanel.transform.Find("Menu").transform.SetAsLastSibling();
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").transform.SetAsLastSibling();
        }
        else if (SceneManager.GetActiveScene().name == "Cafe")
        {
            CafeSystem._Instance._UI._menuPanel.transform.Find("Menu").transform.SetAsLastSibling();
            CafeSystem._Instance._UI._menuPanel.transform.Find("Page").transform.SetAsLastSibling();
        }
        //GameSystem._Instance._UI._menuPanel.transform.Find("YesOrNo").transform.SetAsLastSibling();
    }
    // 타이틀 창으로 돌아가기
    public void OnTitleClicked()
    {
        if (SceneManager.GetActiveScene().name == "Game") GameSystem._Instance.LoadTitleScene();
        else if (SceneManager.GetActiveScene().name == "Cafe") CafeSystem._Instance.LoadTitleScene();
    }
    // 저장하기 창 클릭
    public void OnSaveMenuClicked()
    {
        setMenuText("Save / 저장하기");
        // 게임씬의 경우
        if (SceneManager.GetActiveScene().name == "Game")
        {
            GameSystem._Instance._UI.tempMode = "SAVE";
            GameSystem._Instance._UI._loadMenu.SetActivate(false);
            GameSystem._Instance._UI._saveMenu.SetActivate(true);
            GameSystem._Instance._UI._menuPanel.GetComponent<UIPage>().Max = 4;
            //페이지를 제일 앞으로 이동
            while (GameSystem._Instance._UI._menuPanel.GetComponent<UIPage>().page != 0)
            {
                GameSystem._Instance._UI._menuPanel.GetComponent<UIPage>().OnPrevClik();
            }
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").gameObject.SetActive(true);
            // 엑스트라에서 생성한 오브젝트 삭제
            Destroy(GameSystem._Instance._UI._Extra.TargetList);
            GameSystem._Instance._UI._Extra.SetActivate(false);
            GameSystem._Instance._UI._settingMenu.SetActivate(false);
            // 페이지 조절 버튼 활성화
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").gameObject.SetActive(true);
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Page").gameObject.SetActive(true);
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Prev").gameObject.SetActive(true);
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Next").gameObject.SetActive(true);
        }
        // 미니게임 씬의 경우
        else if (SceneManager.GetActiveScene().name == "Cafe")
        {
            CafeSystem._Instance._UI._loadMenu.SetActivate(false);
            CafeSystem._Instance._UI._saveMenu.SetActivate(true);
            CafeSystem._Instance._UI._menuPanel.GetComponent<UIPage>().Max = 4;
            CafeSystem._Instance._UI._UIBack.SetActive(true);
            CafeSystem._Instance._UI._Input.SetActivate(false);
            CafeSystem._Instance._UI._Cafe.SetActivate(false);
            // 페이지를 제일 앞으로 이동(페이지 싱크 맞추기 위해)
            while (CafeSystem._Instance._UI._menuPanel.GetComponent<UIPage>().page != 0)
            {
                CafeSystem._Instance._UI._menuPanel.GetComponent<UIPage>().OnPrevClik();
            }
            CafeSystem._Instance._UI._menuPanel.transform.Find("Page").gameObject.SetActive(true);
            // 엑스트라에서 생성한 오브젝트 삭제
            Destroy(CafeSystem._Instance._UI._Extra.TargetList);
            CafeSystem._Instance._UI._Extra.SetActivate(false);
            CafeSystem._Instance._UI._settingMenu.SetActivate(false);
            // 페이지 조절 활성화
            CafeSystem._Instance._UI._menuPanel.transform.Find("Page").gameObject.SetActive(true);
            CafeSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Page").gameObject.SetActive(true);
            CafeSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Prev").gameObject.SetActive(true);
            CafeSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Next").gameObject.SetActive(true);
        }

    }
    // 불러오기 버튼
    public void OnLoadMenuClicked()
    {
        setMenuText("Load / 불러오기");
        if (SceneManager.GetActiveScene().name == "Game")
        {
            GameSystem._Instance._UI.tempMode = "LOAD";
            GameSystem._Instance._UI._saveMenu.SetActivate(false);
            GameSystem._Instance._UI._loadMenu.SetActivate(true);
            GameSystem._Instance._UI._menuPanel.GetComponent<UIPage>().Max = 4;
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").gameObject.SetActive(true);
            GameSystem._Instance._UI._settingMenu.SetActivate(false);
            // 페이지를 제일 앞으로
            while (GameSystem._Instance._UI._menuPanel.GetComponent<UIPage>().page != 0)
            {
                GameSystem._Instance._UI._menuPanel.GetComponent<UIPage>().OnPrevClik();
            }
            Destroy(GameSystem._Instance._UI._Extra.TargetList);
            GameSystem._Instance._UI._Extra.SetActivate(false);
            // 페이지 조절 활성화
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").gameObject.SetActive(true);
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Page").gameObject.SetActive(true);
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Prev").gameObject.SetActive(true);
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Next").gameObject.SetActive(true);

        }
        else if (SceneManager.GetActiveScene().name == "Cafe")
        {
            CafeSystem._Instance._UI._saveMenu.SetActivate(false);
            CafeSystem._Instance._UI._loadMenu.SetActivate(true);
            CafeSystem._Instance._UI._menuPanel.GetComponent<UIPage>().Max = 4;
            CafeSystem._Instance._UI._UIBack.SetActive(true);
            CafeSystem._Instance._UI._Input.SetActivate(false);
            CafeSystem._Instance._UI._Cafe.SetActivate(false);
            // 페이지 맨 앞으로 이동
            while (CafeSystem._Instance._UI._menuPanel.GetComponent<UIPage>().page != 0)
            {
                CafeSystem._Instance._UI._menuPanel.GetComponent<UIPage>().OnPrevClik();
            }
            // 엑스트라에서 생성한 오브젝트 삭제
            Destroy(CafeSystem._Instance._UI._Extra.TargetList);
            CafeSystem._Instance._UI._Extra.SetActivate(true);
            CafeSystem._Instance._UI._settingMenu.SetActivate(false);
            // 페이지 조절 활성화
            CafeSystem._Instance._UI._menuPanel.transform.Find("Page").gameObject.SetActive(true);
            CafeSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Page").gameObject.SetActive(true);
            CafeSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Prev").gameObject.SetActive(true);
            CafeSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Next").gameObject.SetActive(true);
            CafeSystem._Instance._UI._Extra.SetActivate(false);
        }

    }
    // 추가요소
    public void OnExtraMenuClicked()
    {

        if (SceneManager.GetActiveScene().name == "Game")
        {
            GameSystem._Instance._UI._Extra.SetActivate(true);
            GameSystem._Instance._UI._menuPanel.setMenuText("Extra / 추가요소", "수집된 요소나 보너스로 제공된 이야기를 볼 수 있습니다.");
            GameSystem._Instance._UI._Menu.SetActivate(false);
            GameSystem._Instance._UI._saveMenu.SetActivate(false);
            GameSystem._Instance._UI._loadMenu.SetActivate(false);
            // 페이지 맨앞으로
            while (GameSystem._Instance._UI._menuPanel.GetComponent<UIPage>().page != 0)
            {
                GameSystem._Instance._UI._menuPanel.GetComponent<UIPage>().OnPrevClik();
            }
            // 페이지 조절 요소 활성화 후 전체 비활성화(추기요소 시작시는 숨기고 리스트 선택시 활성화)
            GameSystem._Instance._UI._settingMenu.SetActivate(false);
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").gameObject.SetActive(true);
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Page").gameObject.SetActive(true);
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Prev").gameObject.SetActive(true);
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Next").gameObject.SetActive(true);
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").gameObject.SetActive(false);
            // 이전에 생성된 오브젝트 삭제
            Destroy(GameSystem._Instance._UI._Extra.TargetList);
        }
        else if (SceneManager.GetActiveScene().name == "Cafe")
        {
            CafeSystem._Instance._UI._Extra.SetActivate(true);
            CafeSystem._Instance._UI._menuPanel.setMenuText("Extra / 추가요소", "수집된 요소나 보너스로 제공된 이야기를 볼 수 있습니다.");
            CafeSystem._Instance._UI._Menu.SetActivate(false);
            CafeSystem._Instance._UI._saveMenu.SetActivate(false);
            CafeSystem._Instance._UI._loadMenu.SetActivate(false);
            CafeSystem._Instance._UI._UIBack.SetActive(true);
            CafeSystem._Instance._UI._Input.SetActivate(false);
            CafeSystem._Instance._UI._Cafe.SetActivate(false);
            // 페이지 맨앞으로
            while (CafeSystem._Instance._UI._menuPanel.GetComponent<UIPage>().page != 0)
            {
                CafeSystem._Instance._UI._menuPanel.GetComponent<UIPage>().OnPrevClik();
            }
            // 페이지 조절 요소 활성화 후 전체 비활성화(추기요소 시작시는 숨기고 리스트 선택시 활성화)
            CafeSystem._Instance._UI._settingMenu.SetActivate(false);
            CafeSystem._Instance._UI._menuPanel.transform.Find("Page").gameObject.SetActive(true);
            CafeSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Page").gameObject.SetActive(true);
            CafeSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Prev").gameObject.SetActive(true);
            CafeSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Next").gameObject.SetActive(true);
            CafeSystem._Instance._UI._menuPanel.transform.Find("Page").gameObject.SetActive(false);
            Destroy(CafeSystem._Instance._UI._Extra.TargetList);
        }


    }
    // 환경설정
    public void OnSettingMenuClicked()
    {
        setMenuText("Setting / 환경설정");

        if (SceneManager.GetActiveScene().name == "Game")
        {
            // 다른 메뉴 비활성화
            GameSystem._Instance._UI._saveMenu.SetActivate(false);
            GameSystem._Instance._UI._loadMenu.SetActivate(false);
            GameSystem._Instance._UI._Extra.SetActivate(false);
            GameSystem._Instance._UI._settingMenu.SetActivate(true); // 자신은 활성화
            Destroy(GameSystem._Instance._UI._Extra.TargetList); // 생성된 오브젝트 삭제
            // 환경설정 요소 세팅
            GameSystem._Instance._UI._settingMenu.DialogueModeSpr(PlayerPrefs.GetString("automode"));
            // 페이지 조절 오브젝트 비활성화
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").gameObject.SetActive(true);
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Page").gameObject.SetActive(false);
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Prev").gameObject.SetActive(false);
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Next").gameObject.SetActive(false);
        }
        else if (SceneManager.GetActiveScene().name == "Cafe")
        {
            // 다른 메뉴 비활성화
            CafeSystem._Instance._UI._saveMenu.SetActivate(false);
            CafeSystem._Instance._UI._loadMenu.SetActivate(false);
            CafeSystem._Instance._UI._Extra.SetActivate(false);
            CafeSystem._Instance._UI._Input.SetActivate(false);
            CafeSystem._Instance._UI._Cafe.SetActivate(false);
            CafeSystem._Instance._UI._settingMenu.SetActivate(true);// 자신은 활성화
            Destroy(CafeSystem._Instance._UI._Extra.TargetList);// 생성된 오브젝트 삭제
            // 환경설정 요소 세팅
            CafeSystem._Instance._UI._settingMenu.DialogueModeSpr(PlayerPrefs.GetString("automode"));
            // 페이지 조절 오브젝트 비활성화
            CafeSystem._Instance._UI._menuPanel.transform.Find("Page").gameObject.SetActive(true);
            CafeSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Page").gameObject.SetActive(false);
            CafeSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Prev").gameObject.SetActive(false);
            CafeSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Next").gameObject.SetActive(false);
        }
    }
    // 메뉴 텍스트 설정
    public void setMenuText(string text,string sub = "")
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            Text menuText = GameSystem._Instance._UI._menuPanel.transform.Find("Menu").GetComponent<Text>();
            menuText.text = text;
            Text SubText = GameSystem._Instance._UI._menuPanel.transform.Find("Sub").GetComponent<Text>();
            SubText.text = sub;
        }
        else if (SceneManager.GetActiveScene().name == "Cafe")
        {
            Text menuText = CafeSystem._Instance._UI._menuPanel.transform.Find("Menu").GetComponent<Text>();
            menuText.text = text;
            Text SubText = CafeSystem._Instance._UI._menuPanel.transform.Find("Sub").GetComponent<Text>();
            SubText.text = sub;
        }

    }
}
