using Game;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIExtra : UIWindow
{
    [SerializeField]
    GameObject _SceneList = null;
    [SerializeField]
    GameObject _CGList = null;
    [SerializeField]
    UnityEngine.Sprite _voidSprite = null;

    public Image fullImage = null;
    public int Page = 0;
    public const int MaxScene = 19;
    public const int MaxCg = 3;
    public int MaxItem = 0;
    public GameObject TargetList = null;
    public string _ScenarioName = "";
    public string _Label = "";

    public void Clear()
    {
        TargetList = null;
        Page = 0;
    }
    public void OnSceneListClicked()
    {
        // 씬 회상 선택
        MaxItem = MaxScene;
        if (TargetList != null) return;
        GameObject newObject = Instantiate(_SceneList,transform.parent);
        newObject.transform.localScale = Vector3.one;
        TargetList = newObject;
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (GameSystem._Instance._UI._saveMenu.IsActivate())
                GameSystem._Instance._UI._saveMenu.SetActivate(false);
            if (GameSystem._Instance._UI._loadMenu.IsActivate())
                GameSystem._Instance._UI._loadMenu.SetActivate(false);
            GameSystem._Instance._UI._Extra.TargetList = newObject;
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").gameObject.SetActive(true);
            GameSystem._Instance._UI._menuPanel.setMenuText("Extra / 이벤트 목록", "수집된 이벤트를 다시 볼 수 있습니다.");
            int maxPage = (MaxItem - 1) / TargetList.transform.childCount;
            GameSystem._Instance._UI._menuPanel.GetComponent<UIPage>().Max = maxPage;
        }
        if (SceneManager.GetActiveScene().name == "Cafe")
        {

            if (CafeSystem._Instance._UI._saveMenu.IsActivate())
                CafeSystem._Instance._UI._saveMenu.SetActivate(false);
            if (CafeSystem._Instance._UI._loadMenu.IsActivate())
                CafeSystem._Instance._UI._loadMenu.SetActivate(false);
            CafeSystem._Instance._UI._Extra.TargetList = newObject;
            CafeSystem._Instance._UI._menuPanel.transform.Find("Page").gameObject.SetActive(true);
            CafeSystem._Instance._UI._menuPanel.setMenuText("Extra / 이벤트 목록", "수집된 이벤트를 다시 볼 수 있습니다.");
            int maxPage = (MaxItem - 1) / TargetList.transform.childCount;
            CafeSystem._Instance._UI._menuPanel.GetComponent<UIPage>().Max = maxPage;
        }
        else if (SceneManager.GetActiveScene().name == "Title")
        {

            TitleSystem._Instance._UIManager._Extra.TargetList = newObject;
            if (TitleSystem._Instance._UIManager._uiTitleLoadMenu.IsActivate())
                TitleSystem._Instance._UIManager._uiTitleLoadMenu.SetActivate(false);
            TitleSystem._Instance._UIManager._uiTitleMenuPanel.transform.Find("Page").gameObject.SetActive(true);
            TitleSystem._Instance._UIManager._uiTitleMenuPanel.setMenuText("Extra / 이벤트 목록", "수집된 이벤트를 다시 볼 수 있습니다.");
            int maxPage = (MaxItem - 1) / TargetList.transform.childCount;
            TitleSystem._Instance._UIManager._uiTitleMenuPanel.GetComponent<UITitlePage>().Max = maxPage;
        }
        UpdateExtraPage();
    }
    public void OnCGListClicked()
    {
        // CG리스트에 진입
        MaxItem = MaxCg;
        if (TargetList != null) return;
        GameObject newObject = Instantiate(_CGList, transform.parent);
        newObject.transform.localScale = Vector3.one;
        TargetList = newObject;
        if (SceneManager.GetActiveScene().name == "Game")
        {
            GameSystem._Instance._UI._Extra.TargetList = newObject;
            if (GameSystem._Instance._UI._saveMenu.IsActivate())
                GameSystem._Instance._UI._saveMenu.SetActivate(false);
            if (GameSystem._Instance._UI._loadMenu.IsActivate())
                GameSystem._Instance._UI._loadMenu.SetActivate(false);
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").gameObject.SetActive(true);
            GameSystem._Instance._UI._menuPanel.setMenuText("Extra / CG 목록", "수집 된 CG를 다시 볼 수 있습니다.");
            int maxPage = (MaxItem - 1) / TargetList.transform.childCount;
            GameSystem._Instance._UI._menuPanel.GetComponent<UIPage>().Max = maxPage;
        }
        else if (SceneManager.GetActiveScene().name == "Title")
        {
            TitleSystem._Instance._UIManager._Extra.TargetList = newObject;
            if (TitleSystem._Instance._UIManager._uiTitleLoadMenu.IsActivate())
                TitleSystem._Instance._UIManager._uiTitleLoadMenu.SetActivate(false);
            TitleSystem._Instance._UIManager._uiTitleMenuPanel.transform.Find("Page").gameObject.SetActive(true);
            TitleSystem._Instance._UIManager._uiTitleMenuPanel.setMenuText("Extra / CG 목록", "수집 된 CG를 다시 볼 수 있습니다.");
            int maxPage = (MaxItem - 1) / TargetList.transform.childCount;
            TitleSystem._Instance._UIManager._uiTitleMenuPanel.GetComponent<UITitlePage>().Max = maxPage;
        }
        else if (SceneManager.GetActiveScene().name == "Cafe")
        {
            CafeSystem._Instance._UI._Extra.TargetList = newObject;
            if (CafeSystem._Instance._UI._saveMenu.IsActivate())
                CafeSystem._Instance._UI._saveMenu.SetActivate(false);
            if (CafeSystem._Instance._UI._loadMenu.IsActivate())
                CafeSystem._Instance._UI._loadMenu.SetActivate(false);
            CafeSystem._Instance._UI._menuPanel.transform.Find("Page").gameObject.SetActive(true);
            CafeSystem._Instance._UI._menuPanel.setMenuText("Extra / CG 목록", "수집 된 CG를 다시 볼 수 있습니다.");
            int maxPage = (MaxItem - 1) / TargetList.transform.childCount;
            CafeSystem._Instance._UI._menuPanel.GetComponent<UIPage>().Max = maxPage;

        }
        UpdateExtraPage();
    }
    public void OnSceneSlotClicked(GameObject button)
    {
        // 씬 회상은 타이틀에서만 가능
        if (SceneManager.GetActiveScene().name == "Game")
        {
            GameSystem._Instance._UI._ok.SetActivate(true);
            GameSystem._Instance._UI._ok.SetText("게임중에는 씬 회상을 할 수 없습니다.");
            return;
        }
        else if (SceneManager.GetActiveScene().name == "Cafe")
        {
            CafeSystem._Instance._UI._ok.SetActivate(true);
            CafeSystem._Instance._UI._ok.SetText("게임중에는 씬 회상을 할 수 없습니다.");
            return;
        }
        else if (SceneManager.GetActiveScene().name == "Title")
        {
            // TODO: 시나리오 진행
            // 해당 칸에 이미지 없음(잠금)이면 무시
            if (button.transform.Find("Image").GetComponent<Image>().sprite == _voidSprite) return;
            TitleSystem._Instance._UIManager._uiTitlePopUp.SetActivate(true);
            TitleSystem._Instance._UIManager._uiTitlePopUp.SetText("해당 씬을 감상 하겠습니까?");
            // TitleSystem._Instance._UIManager._uiTitlePopUp.
            // 시나리오 이름과 타겟 라벨 저장 
            int idx = Page * 9 + int.Parse(button.name) - 1;
            string[] scene = PlayerPrefs.GetString("SCENE").Split("|");
            string label = scene[idx].Split(":")[0];
            string ScenariName = scene[idx].Split(":")[1];
            TitleSystem._Instance._UIManager._Extra._ScenarioName = ScenariName;
            TitleSystem._Instance._UIManager._Extra._Label = label;
        }
    }
    public void OnCGSlotClicked(GameObject button)
    {
        // 빈칸이면 종료
        UnityEngine.Sprite sp = button.transform.Find("Image").GetComponent<Image>().sprite;
        if (button.transform.Find("Image").GetComponent<Image>().sprite == _voidSprite) return;

        // 버튼이미지를 스프라이트 전체화면으로 띄우기        
        if (SceneManager.GetActiveScene().name == "Game")
        {
            GameSystem._Instance._UI._FullCG.SetActive(true);
            GameSystem._Instance._UI._FullCG.GetComponent<Image>().sprite = sp;
            GameSystem._Instance._UI._FullCG.transform.SetAsLastSibling();
        }
        else if (SceneManager.GetActiveScene().name == "Cafe")
        {
            CafeSystem._Instance._UI._FullCG.SetActive(true);
            CafeSystem._Instance._UI._FullCG.GetComponent<Image>().sprite = sp;
        }
        else if (SceneManager.GetActiveScene().name == "Title")
        {
            TitleSystem._Instance._UIManager._FullCG.SetActive(true);
            TitleSystem._Instance._UIManager._FullCG.GetComponent<Image>().sprite = sp;
        }
    }
    public void FullCGClicked(GameObject button)
    {
        button.SetActive(false);
    }
    public void UpdateExtraPage()
    {

        // 이미지의 활성화 여부는 "이미지 이름":"활성화여부(0,1)"|"이미지 이름":"활성화여부(0,1)"와 같이 저장된다.
        // 없으면 초기화, 다른 초기화는 UIMenu

        string[] Sce = PlayerPrefs.GetString("SCENE").Split("|");
        string[] Ecg = PlayerPrefs.GetString("ECG").Split("|");
        // 씬 회상
        int childCount = TargetList.transform.childCount;
        if (childCount == 9)
        {
            for (int i = 0; i < childCount; i++)
            {
                Transform child = TargetList.transform.GetChild(i);
                // TODO: 자식슬롯에서 이름 설정, 이미지 설정
                int number = Page * 9 + i + 1;
                // 최대 갯수만큼만 표시
                if (number > MaxItem)
                {
                    child.gameObject.SetActive(false);
                    continue;
                }
                else child.gameObject.SetActive(true);
                string name = Sce[number - 1].Split(":")[0];
                string ScnariName = Sce[number - 1].Split(":")[1];
                // 이름 설정
                child.Find("Slot").GetComponentInChildren<Text>().text = name.ToString();

                //이미지 설정 
                if (ScnariName != "0")
                {
                    // 해당 파일 위치에 있는 이미지 불러 옴
                    UnityEngine.Sprite sprite = Resources.Load<UnityEngine.Sprite>(Define._ReviewScenFolder + name);
                    if (sprite == null) sprite = Resources.Load<UnityEngine.Sprite>(Define._backgroundRoot+"/cafe_daytime");
                    child.Find("Image").GetComponent<Image>().sprite = sprite;
                }
                // 없으면 일반 빈칸
                else child.Find("Image").GetComponent<Image>().sprite = _voidSprite;
            }
        }
        // CG집
        else if (childCount == 6)
        {
            for (int i = 0; i < TargetList.transform.childCount; i++)
            {
                Transform child = TargetList.transform.GetChild(i);

                // TODO: 자식슬롯에서 이름 설정, 이미지 설정
                int number = Page * 6 + i + 1;
                // 최대 값 만큼만 표기
                if (number > MaxItem)
                {
                    child.gameObject.SetActive(false);
                    continue;
                }
                else child.gameObject.SetActive(true);

                // 이미지 설정
                string name = Ecg[number - 1].Split(":")[0];
                string activate = Ecg[number - 1].Split(":")[1];
                //이미지 설정 
                if (activate != "0")
                {
                    // 해당 위치에 있는 썸네일 이미지 불러 오기
                    UnityEngine.Sprite sprite = Resources.Load<UnityEngine.Sprite>(Define._spriteRoot+"/" + name);
                    child.Find("Image").GetComponent<Image>().sprite = sprite;
                }
                // 없으면 일반 빈칸
                else child.Find("Image").GetComponent<Image>().sprite = _voidSprite;
            }
        }

    }

    public void OnExtraPrev()
    {
        if (TargetList == null) return;
        int maxPage = (MaxItem - 1) / TargetList.transform.childCount;
        if (Page >= 1) Page -= 1;
        else return;
        UpdateExtraPage();
    }
    public void OnExtraNext()
    {
        int maxPage = (MaxItem - 1) / TargetList.transform.childCount;
        if (Page < maxPage) Page += 1;
        else return;
        UpdateExtraPage();
    }
}
