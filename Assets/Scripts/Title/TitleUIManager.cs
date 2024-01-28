using UnityEngine;
using UnityEngine.UI;

public class TitleUIManager : UIManager
{
    [SerializeField]
    private Transform _mainPanel = null;
    [SerializeField]
    public GameObject _UIBack = null;
    [SerializeField]
    public GameObject _FullCG = null;
    public FadeOverlay _FadeOverlay { get; private set; }
    private UITitleMenu _titleMenu = null;
    public UITitleMenu _TitleMenu { get { return _titleMenu; } }
    public UITitleMenuPanel _uiTitleMenuPanel { get; private set; }
    
    public UITitleLoadMenu _uiTitleLoadMenu { get; private set; }

    public UITitlePopUp _uiTitlePopUp { get; private set; }
    public UIOk _uiOk { get; private set; }
   
    public UIExtra _Extra { get; private set; }

    public UITitleSetting _uiTitleSetting { get; private set; }

    public void Initialize()
    {
        _FadeOverlay = GetComponentInChildren<FadeOverlay>(true);
        _FadeOverlay.DoFadeOut(0.0f);

        OpenTitleMenu();
        CreateTitleMenuPanel();
        CreateTitleLoadMenu();
        CreateExtra();
        CreateTitlePopUp();
        CreateSettingMenu();
        CreateOk();
    }

    protected override bool OnKeyInputOnComponent() => _FadeOverlay.IsFading();

    public void OpenTitleMenu()
    {
        if (_titleMenu == null)
            _titleMenu = OpenWindow(Define._uiTitleMenuPrefabPath, _mainPanel) as UITitleMenu;
    }
    public void CreateTitleMenuPanel()
    {
        if (_uiTitleMenuPanel == null)
            _uiTitleMenuPanel = OpenWindow(Define._uiLoadMenuPanelPrefabPath, _mainPanel) as UITitleMenuPanel;
        _uiTitleMenuPanel.SetActivate(false);
    }
    public void CreateTitleLoadMenu()
    {
        if (_uiTitleLoadMenu == null)
            _uiTitleLoadMenu = OpenWindow(Define._uiTitleLoadMenuPrefabPath, _uiTitleMenuPanel.transform) as UITitleLoadMenu;
        _uiTitleLoadMenu.SetActivate(false);
    }
    public void CreateExtra()
    {
        _Extra = OpenWindow(Define._uiExtraPrefabPath, _uiTitleMenuPanel.transform) as UIExtra;
        _Extra.SetActivate(false);
    }    
    public void CreateTitlePopUp()
    {
        if (_uiTitlePopUp == null)
            _uiTitlePopUp = OpenWindow(Define._uiTitlePopUpPrefabPath, _mainPanel) as UITitlePopUp;
        _uiTitlePopUp.SetActivate(false);
    }
    public void CreateOk()
    {
        if (_uiOk == null)
            _uiOk = OpenWindow(Define._uiOKPrefabPath, _mainPanel) as UIOk;
        _uiOk.SetActivate(false);
    }
    public void CreateSettingMenu()
    {
        if (_uiTitleSetting == null)
            _uiTitleSetting = OpenWindow(Define._uiSettingMenuPrefabPath, _uiTitleMenuPanel.transform) as UITitleSetting;
        _uiTitleSetting.SetActivate(false);
    }
    public void CloseTitleMenu()
    {
        if (_titleMenu != null)
        {
            CloseWindow(_titleMenu);
            _titleMenu = null;
        }
    }
}