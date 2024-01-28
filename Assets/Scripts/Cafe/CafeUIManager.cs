using Game;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CafeUIManager : global::UIManager
{
    [SerializeField]
    public GameObject _UIBack = null;
    [SerializeField]
    public GameObject _FullCG = null;
    public UIInput _Input { get; private set; }
    public CafeMenu _Menu { get; private set; }
    public UICafe _Cafe { get; private set; }
    public UICafeDecorStore _decorStore { get; private set; }
    public UIMenuPanel _menuPanel { get; private set; }
    public UISaveMenu _saveMenu { get; private set; }
    public UILoadMenu _loadMenu { get; private set; }
    public UICafePopup _popUp { get; private set; }
    public UIOk _ok { get; private set; }
    public UICafeOpen _open { get; private set; }
    public UIExtra _Extra { get; private set; }
    public UITitleSetting _settingMenu { get; private set; }

    public string _CurrentSlot = "-1";
    public string _SaveIndex = "-1";

    public void Initialize()
    {
        CreateInput(); 
        CreateCafe();
        CreateMenu();
        CreateDecorStore();
        CreateOpen();
        CreateMenuPanel();
        CreateSaveMenu();
        CreateLoadMenu();
        CreateExtra();
        CreatePopup();
        CreateOK();
        CreateSettingMenu();
        SoundManager._Instance.StopBGM();
        SoundManager._Instance.PlayBGM("brightness");  // 기본 BGM
    }
    private void CreateInput() => _Input = OpenWindow(Define._uiInputPrefabPath) as UIInput;
    private void CreateCafe()
    {
        _Cafe = OpenWindow(Define._uiCafePrefabPath) as UICafe;
        _Cafe.SetActivate(true);

    }
    private void CreateDecorStore()
    {
        _decorStore = OpenWindow(Define._uiCafeDecorStorePrefabPath) as UICafeDecorStore; 
        _decorStore.SetActivate(false);
    }
    private void CreateMenu()
    {
        _Menu = OpenWindow(Define._uiCafeMenuPrefabPath) as CafeMenu;
        _Menu.SetActivate(true);
    }
    private void CreateMenuPanel()
    {
        _menuPanel = OpenWindow(Define._uiMenuPanelPrefabPath) as UIMenuPanel;
        _menuPanel.SetActivate(false);
    }
    private void CreateOpen()
    {
        _open = OpenWindow(Define._uiOpenCafePrefabPath) as UICafeOpen;
        _open.SetActivate(false);
    }
    private void CreateExtra()
    {
        _Extra = OpenWindow(Define._uiExtraPrefabPath, _menuPanel.transform) as UIExtra;
        _Extra.SetActivate(false);
    }
    private void CreateSaveMenu()
    {
        _saveMenu = OpenWindow(Define._uiSaveMenuPrefabPath, _menuPanel.transform) as UISaveMenu;
        _saveMenu.SetActivate(false);
    }

    private void CreateLoadMenu()
    {
        _loadMenu = OpenWindow(Define._uiLoadMenuPrefabPath, _menuPanel.transform) as UILoadMenu;
        _loadMenu.SetActivate(false);
    }
    private void CreatePopup()
    {
        _popUp = OpenWindow(Define._uiCafeYesOrNoPrefabPath) as UICafePopup;
        _popUp.SetActivate(false);
    }
    private void CreateOK()
    {
        _ok = OpenWindow(Define._uiOKPrefabPath) as UIOk;
        _ok.SetActivate(false);
    }
    private void CreateSettingMenu()
    {
        _settingMenu = OpenWindow(Define._uiSettingMenuPrefabPath, _menuPanel.transform) as UITitleSetting;
        _settingMenu.SetActivate(false);
    }
    public void LoadCafeInfo(string Slot)
    {
        //미니게임 정보 업데이트 1번 슬롯에 파리미터(누른 슬롯)의 정보를 1번 슬롯에 저장한 후
        // 1번 슬롯을 바탕으로 ui값 변경
        // 시나리오 정보 1번 슬롯에 저장
        string[] TargerSceninfo = PlayerPrefs.GetString("1").Split("|");
        TargerSceninfo[3] = PlayerPrefs.GetString(Slot).Split("|")[3];
        string newSce = string.Join("|", TargerSceninfo); // 변경사항만 바꾸고 다시 합쳐서 1번 슬롯에
        PlayerPrefs.SetString("1", newSce); //데이터 저장

        // 누른 슬롯의 미니게임 진행정보 1번 슬롯양에 저장
        string[] Cafeinfo = PlayerPrefs.GetString(Slot+"_cafe").Split("|");
        _Cafe._WeekInfo.GetComponentInChildren<Text>().text = Cafeinfo[0]+"주차";       // 진행주차 UI 변경
        string moneyString = int.Parse(Cafeinfo[1]).ToString("C0").Replace("$", "￦ "); //소지금을 ￦888,888,888형식으로 저장
        _Cafe._MoneyInfo.GetComponentInChildren<Text>().text = "소지금 "+moneyString;   //소지금 UI 변경
        string decor = Cafeinfo[2];  // 소지가구의 정보가 "[0,0,0,0...]"로 저장 되어있다. 미소유 0, 소유 1
        decor = decor.Replace("[","").Replace("]",""); // 
        string[] decorArry = decor.Split(",");  //각각의 정보가 들어있는 문자열 리스트로 변환
        int decorCount = 0;                      // 소지 가구 수
        for(int i = decorArry.Length-1; i >= 0; i--)
        {
            if (decorArry[i] =="1") { decorCount++; }
        }
        _Cafe._DecorInfo.GetComponentInChildren<Text>().text = "구매한 가구 "+decorCount.ToString()+"개"; // 소지 가구 UI반영
        _Cafe._decorBonus = int.Parse(Cafeinfo[3]);     // 가구 보너스(수익)
        _Cafe._review = int.Parse(Cafeinfo[4]);         // 누적리뷰수
        _Cafe._visBonus = int.Parse(Cafeinfo[5]);       // 가구 보너스(손님수)
        _Cafe._hasOpened = (Cafeinfo[6] == "1");        // 이번주차 카페운영여부
        string newString = string.Join("|", Cafeinfo);  // 합친후
        PlayerPrefs.SetString("1" + "_cafe", newString);// 1번 슬롯에 적당
        CafeSystem._Instance._UI._Cafe.UpdateInfo();
    }
}
