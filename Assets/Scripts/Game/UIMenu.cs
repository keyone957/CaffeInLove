using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace Game
{
    public class UIMenu : UIWindow
    {
        [SerializeField]
        private GameObject _container = null;

        public override bool OnKeyInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {

                OnOptionClicked();
                return true;

            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                PlayerPrefs.DeleteAll();
                // 씬 개방 여부
                PlayerPrefs.SetString("SCENE", "Main_01:0|Main_02:0|Main_03:0|Main_04:0|Main_05:0|Main_06:0|Main_07:0|Main_08:0|Main_09:0|Main_10:0" +
            "|Main_11:0|Main_12:0|Main_13:0|Main_14:0|Main_15:0|Main_16:0|Main_17:0|Main_18:0|Main_19:0|Main_20:0" +
            "|Main_21:0|Main_22:0|Main_23:0|Main_24:0|Main_25:0|Main_26:0|Main_27:0|Main_28:0|Main_29:0|Main_30:0" +
            "|Main_31:0|Main_32:0|Main_33:0|Main_34:0|Main_35:0|Main_36:0");
                // CG 개방 여부
                PlayerPrefs.SetString("ECG", "pink_hydrangea_cg:0|cat_rain:0|snowdrop:0|Cg_04:0|Cg_05:0|Cg_06:0|Cg_07:0|Cg_08:0|Cg_09:0|Cg_10:0" +
            "|Cg_11:0|Cg_12:0|Cg_13:0|Cg_14:0|Cg_15:0|Cg_16:0|Cg_17:0|Cg_18:0|Cg_19:0|Cg_20:0");
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
        // 메뉴 버튼 클릭시 하위 메뉴 활성화
        public void OnOptionClicked() => ToggleContainerActive();
        // 타이틀로 이동 메뉴 클릭시 확인 팝업 생성
        public void OnTitleClicked() 
        {
            GameSystem._Instance._UI._popUp.SetActivate(true);
            GameSystem._Instance._UI._popUp.SetText("타이틀로 돌아갑니까?", "저장 되지 않은 데이터는 삭제 됩니다");
            GameSystem._Instance._UI.tempMode = "Title";
            GameSystem._Instance._UI._popUp.GoTitle = true;
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").gameObject.SetActive(true);
        }
        // 저장하기 메뉴 클릭시 확인팝업 생성
        public void OnSaveBtnClicked()
        {
            if (GameSystem._Instance._UI._loadMenu.IsActivate())
            {
                GameSystem._Instance._UI._loadMenu.SetActivate(false);
            }
            // 메뉴 이름 세팅
            GameSystem._Instance._UI._menuPanel.setMenuText("Save / 저장하기");
            GameSystem._Instance._UI.tempMode = "SAVE";
            // 선택지, 메뉴등 위쪽 계층에 있는 오브젝트 비활성화
            if (GameSystem._Instance._UI._Select != null) GameSystem._Instance._UI._Select.SetActivate(false);
            GameSystem._Instance._UI._menuPanel.SetActivate(true); 
            GameSystem._Instance._UI._Menu.SetActivate(false);
            GameSystem._Instance._UI._Dialogue.SetActivate(false);
            GameSystem._Instance._UI._saveMenu.SetActivate(true);
            GameSystem._Instance._UI._UIBack.SetActive(true);
            // 페이지의 버튼들 활성화(환경설정에서 비활성화 된걸 복구)
            GameSystem._Instance._UI._Input.SetActivate(false);
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").gameObject.SetActive(true);
            GameSystem._Instance._UI._menuPanel.GetComponent<UIPage>().Max = 4;
            GameSystem._Instance._UI._settingMenu.SetActivate(false);
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Page").gameObject.SetActive(true);
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Prev").gameObject.SetActive(true);
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Next").gameObject.SetActive(true);

        }

        public void OnLoadBtnClicked()
        {
            if (GameSystem._Instance._UI._saveMenu.IsActivate())
            {
                GameSystem._Instance._UI._saveMenu.SetActivate(false);
            }
            GameSystem._Instance._UI._menuPanel.setMenuText("Load / 불러오기");
            GameSystem._Instance._UI.tempMode = "LOAD";
            GameSystem._Instance._UI._menuPanel.SetActivate(true);
            GameSystem._Instance._UI._Menu.SetActivate(false);
            GameSystem._Instance._UI._Dialogue.SetActivate(false);
            GameSystem._Instance._UI._loadMenu.SetActivate(true);
            GameSystem._Instance._UI._UIBack.SetActive(true);
            // 선택지 숨김
            if (GameSystem._Instance._UI._Select != null) GameSystem._Instance._UI._Select.SetActivate(false);
            //**
            GameSystem._Instance._UI._Input.SetActivate(false);
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").gameObject.SetActive(true);
            GameSystem._Instance._UI._menuPanel.GetComponent<UIPage>().Max = 4;
            GameSystem._Instance._UI._settingMenu.SetActivate(false);
            // 페이지의 버튼들 활성화(환경설정에서 비활성화 된걸 복구)
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Page").gameObject.SetActive(true);
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Prev").gameObject.SetActive(true);
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Next").gameObject.SetActive(true);
        }
        // 추가요소 버튼 눌림
        public void OnExtraClicked()
        {
            // 세이브창, 로드창  열려 있으면 닫기
            if (GameSystem._Instance._UI._saveMenu.IsActivate())
                GameSystem._Instance._UI._saveMenu.SetActivate(false);
            if (GameSystem._Instance._UI._loadMenu.IsActivate())
                GameSystem._Instance._UI._loadMenu.SetActivate(false);
            // 메뉴 이름 세팅
            GameSystem._Instance._UI._menuPanel.setMenuText("Extra / 추가요소","수집된 요소나 보너스로 제공된 이야기를 볼 수 있습니다.");
            GameSystem._Instance._UI.tempMode = "Extra";
            GameSystem._Instance._UI._Extra.SetActivate(true);
            GameSystem._Instance._UI._menuPanel.SetActivate(true);
            // 선택지 숨김
            if (GameSystem._Instance._UI._Select != null) GameSystem._Instance._UI._Select.SetActivate(false);
            // 상위계층에 있는 오브젝트 비활성화
            GameSystem._Instance._UI._Dialogue.SetActivate(false);
            GameSystem._Instance._UI._UIBack.SetActive(true);
            GameSystem._Instance._UI._Input.SetActivate(false);
            GameSystem._Instance._UI._settingMenu.SetActivate(false);
            // 환경설정에서 비활성화된 페이지 버튼들 활성화
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").gameObject.SetActive(true);
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Page").gameObject.SetActive(true);
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Prev").gameObject.SetActive(true);
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Next").gameObject.SetActive(true);
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").gameObject.SetActive(false);
            // 메뉴 창 닫기
            GameSystem._Instance._UI._Menu.SetActivate(false);
        }
        // 환경설정 창 열기
        public void OnSettingBtnClicked()
        {
            // 상위계층 오브젝트 비활성화
            GameSystem._Instance._UI._menuPanel.setMenuText("Setting / 환경설정");
            GameSystem._Instance._UI._menuPanel.SetActivate(true);
            GameSystem._Instance._UI._Dialogue.SetActivate(false);
            GameSystem._Instance._UI._loadMenu.SetActivate(true);
            GameSystem._Instance._UI._Menu.SetActivate(false);
            GameSystem._Instance._UI._UIBack.SetActive(true);
            // 선택지 비활성화
            if (GameSystem._Instance._UI._Select != null)
            { GameSystem._Instance._UI._Select.SetActivate(false); }
            GameSystem._Instance._UI._Input.SetActivate(false);
            GameSystem._Instance._UI._settingMenu.SetActivate(true);
            // 환경설정 창을 모드에 따라 열기, 페이지 조절버튼및 페이지 표시 비활성화
            GameSystem._Instance._UI._settingMenu.DialogueModeSpr(PlayerPrefs.GetString("automode"));
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Page").gameObject.SetActive(false);
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Prev").gameObject.SetActive(false);
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").transform.Find("Next").gameObject.SetActive(false);

        }
        private void ToggleContainerActive() => _container.SetActive(!_container.activeSelf);
        // 다른 메뉴: 저장하기, 불러오기, 환경설정, 엑스트라, 타이틀로 숨김, 활성화
    }
}