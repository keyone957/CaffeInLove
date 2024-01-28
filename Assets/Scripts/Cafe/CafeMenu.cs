using Game;
using UnityEngine;

public class CafeMenu : UIWindow
{
    [SerializeField]
    private GameObject _container = null;

    public override bool OnKeyInput()
    {
        //메뉴 활성화
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnOptionClicked();
            return true;
        }
        return false;
    }
    private void MoveAnchorToSafeArea()
    {
        // 우상단이 Safe Area 우상단과 일치하도록 이동
        // 예) iPhone X
        // Screen w:2436, h:1125
        // SafeArea x:132.00, y:63.00, width:2172.00, height:1062.00

        Vector2 rightTopAnchor = new Vector2(
            (Screen.safeArea.x + Screen.safeArea.width) / (float)Screen.width,
            (Screen.safeArea.y + Screen.safeArea.height) / (float)Screen.height
        );

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchorMin = rightTopAnchor;
        rectTransform.anchorMax = rightTopAnchor;
    }
    public void OnOptionClicked() => ToggleContainerActive();
    private void ToggleContainerActive() => _container.SetActive(!_container.activeSelf); //메뉴 온오프
    //저장 메뉴 열기
    public void OnSaveClicked()
    {
        // 메뉴 UI정리. True: 대상,menupanel,UIBack,페이지(이전,다음,닫기)  false: 그외 다른 메뉴창
        if (CafeSystem._Instance._UI._loadMenu.IsActivate())
        {
            CafeSystem._Instance._UI._loadMenu.SetActivate(false);
        }
        CafeSystem._Instance._UI._menuPanel.setMenuText("Save / 저장하기");
        CafeSystem._Instance._UI._menuPanel.SetActivate(true);
        CafeSystem._Instance._UI._Menu.SetActivate(false);
        CafeSystem._Instance._UI._saveMenu.SetActivate(true);
        CafeSystem._Instance._UI._UIBack.SetActive(true);
        CafeSystem._Instance._UI._Input.SetActivate(false);
        CafeSystem._Instance._UI._Cafe.SetActivate(false);
        CafeSystem._Instance._UI._settingMenu.SetActivate(false);
        CafeSystem._Instance._UI._menuPanel.transform.Find("Page").gameObject.SetActive(true);
        CafeSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Page").gameObject.SetActive(true);
        CafeSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Prev").gameObject.SetActive(true);
        CafeSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Next").gameObject.SetActive(true);
        CafeSystem._Instance._UI._Extra.SetActivate(false);
    }
    // 로드메뉴 열기
    public void OnLoadClicked()
    {
        // 메뉴 UI정리. True: 대상,menupanel,UIBack,페이지(이전,다음,닫기)  false: 그외 다른 기능의메뉴창
        if (CafeSystem._Instance._UI._saveMenu.IsActivate())
        {
            CafeSystem._Instance._UI._saveMenu.SetActivate(false);
        }
        CafeSystem._Instance._UI._menuPanel.setMenuText("Load / 불러오기"); // 판넬 글자 변경
        CafeSystem._Instance._UI._menuPanel.SetActivate(true);
        CafeSystem._Instance._UI._Menu.SetActivate(false);
        CafeSystem._Instance._UI._loadMenu.SetActivate(true);
        CafeSystem._Instance._UI._UIBack.SetActive(true);
        //**
        CafeSystem._Instance._UI._Input.SetActivate(false);
        CafeSystem._Instance._UI._Cafe.SetActivate(false);
        CafeSystem._Instance._UI._settingMenu.SetActivate(false);
        CafeSystem._Instance._UI._menuPanel.transform.Find("Page").gameObject.SetActive(true);
        CafeSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Page").gameObject.SetActive(true);
        CafeSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Prev").gameObject.SetActive(true);
        CafeSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Next").gameObject.SetActive(true);

        CafeSystem._Instance._UI._Extra.SetActivate(false);
    }

    // 환경설정 열기
    public void OnSettingClicked()
    {
        CafeSystem._Instance._UI._menuPanel.setMenuText("Setting / 환경설정");
        CafeSystem._Instance._UI._menuPanel.SetActivate(true);
        CafeSystem._Instance._UI._loadMenu.SetActivate(false);
        CafeSystem._Instance._UI._Menu.SetActivate(false);
        CafeSystem._Instance._UI._UIBack.SetActive(true);
        CafeSystem._Instance._UI._Input.SetActivate(false);
        CafeSystem._Instance._UI._Cafe.SetActivate(false);
        CafeSystem._Instance._UI._settingMenu.SetActivate(true);
        CafeSystem._Instance._UI._settingMenu.DialogueModeSpr(PlayerPrefs.GetString("automode"));
        // 페이지 표시, 방향버튼 제거
        CafeSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Page").gameObject.SetActive(false);
        CafeSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Prev").gameObject.SetActive(false);
        CafeSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Next").gameObject.SetActive(false);
    }

    // 엑스트라 창 구현
    public void OnExtraClicked()
    {   // 메뉴 UI정리. True: 대상,menupanel,UIBack,페이지(이전,다음,닫기)  false: 그외 다른 기능의메뉴창
        CafeSystem._Instance._UI._Extra.SetActivate(true);
        CafeSystem._Instance._UI._saveMenu.SetActivate(false);
        CafeSystem._Instance._UI._loadMenu.SetActivate(false);
        CafeSystem._Instance._UI._menuPanel.SetActivate(true);
        CafeSystem._Instance._UI._UIBack.SetActive(true);
        CafeSystem._Instance._UI._Cafe.SetActivate(false);
        CafeSystem._Instance._UI._Input.SetActivate(false);
        CafeSystem._Instance._UI._settingMenu.SetActivate(false);
        CafeSystem._Instance._UI._menuPanel.setMenuText("Extra / 추가요소", "수집된 요소나 보너스로 제공된 이야기를 볼 수 있습니다.");
        CafeSystem._Instance._UI._menuPanel.transform.Find("Page").gameObject.SetActive(true);

        CafeSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Page").gameObject.SetActive(true);
        CafeSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Prev").gameObject.SetActive(true);
        CafeSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Next").gameObject.SetActive(true);
        CafeSystem._Instance._UI._menuPanel.transform.Find("Page").gameObject.SetActive(false);
        CafeSystem._Instance._UI._Menu.SetActivate(false);
    }
    
    //타이틀 메뉴 클릭
    public void OnTitleClicked()
    {
        // 선택팝업 생성
        CafeSystem._Instance._UI._popUp.SetActivate(true);
        CafeSystem._Instance._UI._popUp.SetText("타이틀로 돌아갑니까?", "저장되지 않은 데이터는 삭제됩니다.");
        CafeSystem._Instance._UI._popUp.Mode = "Title";

    }
}
