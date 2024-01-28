using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    public class UIManager : global::UIManager
    {
        [SerializeField]
        public GameObject _UIBack = null;
        [SerializeField]
        public GameObject _FullCG = null;
        public string tempMode = "";
        public UIInput _Input { get; private set; }
        public UIMenu _Menu { get; private set; }
        public UIDialogue _Dialogue { get; private set; }
        public UISelect _Select { get; private set; }
        private Dictionary<string, UIWindow> _dynamicUIs = new Dictionary<string, UIWindow>(); // <UI명, UI>
        public UIMenuPanel _menuPanel { get; private set; }
        public UISaveMenu _saveMenu { get; private set; }
        public UILoadMenu _loadMenu { get; private set; }
        public UIPopup _popUp { get; private set; }
        public UIOk _ok { get; private set; }
        public UIExtra _Extra { get; private set; }
        public UITitleSetting _settingMenu { get; private set; }
        public void Initialize()
        {
            CreateInput();
            CreateDialogue();
            CreateMenu();
            CreateMenuPanel();
            CreateSaveMenu();
            CreateLoadMenu();
            CreateExtra();
            CreatePopup();
            CreateOK();
            CreateSettingMenu();
        }

        private void CreateInput() => _Input = OpenWindow(Define._uiInputPrefabPath) as UIInput;
        private void CreateMenuPanel()
        {
            _menuPanel = OpenWindow(Define._uiMenuPanelPrefabPath) as UIMenuPanel;
            _menuPanel.SetActivate(false);
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
            _popUp= OpenWindow(Define._uiYesOrNoPrefabPath) as UIPopup;
            _popUp.SetActivate(false);
        }
        private void CreateOK()
        {
            _ok = OpenWindow(Define._uiOKPrefabPath) as UIOk;
            _ok.SetActivate(false);
        }

        private void CreateMenu()
        {
            _Menu = OpenWindow(Define._uiMenuPrefabPath) as UIMenu;
            _Menu.SetActivate(false);
        }
        private void CreateExtra()
        {
            _Extra = OpenWindow(Define._uiExtraPrefabPath, _menuPanel.transform) as UIExtra;
            _Extra.SetActivate(false);
        }
        private void CreateSettingMenu()
        {
            _settingMenu = OpenWindow(Define._uiSettingMenuPrefabPath, _menuPanel.transform) as UITitleSetting;
            _settingMenu.SetActivate(false);
        }
        private void CreateDialogue()
        {
            _Dialogue = OpenWindow(Define._uiDialoguePrefabPath) as UIDialogue;
            _Dialogue.SetActivate(false);
        }

        public void CreateSelect()
        {
            if (_Select != null)
            {
                Debug.LogError("[UIManager.AlreadSelectUICreated]");
                return;
            }
            _Select = OpenWindow(Define._uiSelectPrefabPath) as UISelect;
        }

        public void RemoveSelect()
        {
            if (_Select != null)
            {
                CloseWindow(_Select);
                _Select = null;
            }
        }
        public void SetNullSelect(UISelect select)
        {
            select = null;

        }

        public UIWindow GetDynamicUI(string name)
        {
            UIWindow ui;
            if (_dynamicUIs.TryGetValue(name, out ui))
                return ui;
            else
                return null;
        }

        public UIWindow AddDynamicUI(string name, string prefab)
        {
            if (GetDynamicUI(name) != null)
            {
                Debug.LogError($"[UIManager.AddDynamicUI.AlreadyExistName]{name}");
                return null;
            }

            UIWindow ui = OpenWindow(prefab) as UIWindow;
            _dynamicUIs.Add(name, ui);
            return ui;
        }

        public void RemoveDynamicUI(string name)
        {
            UIWindow ui = GetDynamicUI(name);
            if (ui == null)
            {
                Debug.LogError($"[UIManager.RemoveDynamicUI.NotExistsName]{name}");
                return;
            }

            _dynamicUIs.Remove(name);
            CloseWindow(ui);
        }
    }
}