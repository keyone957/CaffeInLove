using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game;
using UnityEngine.SceneManagement;
public class UISaveMenu : UIWindow
{
    
    [SerializeField]
    private Button[] _saveSlot = null;

    public string curSaveSlot; // 누른 슬롯
    private void Awake()
    {
        foreach (Button button in _saveSlot)
        {
            button.onClick.AddListener(() => OnClickSlot(button.gameObject));
        }
    }
    // 세이브 메뉴에서 슬롯 클릭시
    public void OnClickSlot(GameObject clickedObj)
    {
        // 자동저장 슬롯인 경우 종료
        if (clickedObj.transform.parent.name == "1")
        {
            return;
        }
        // 현재 씬이 게임이면
        else if (SceneManager.GetActiveScene().name == "Game")
        {
            // 회상중이면 저장 불가
            if(PlayerPrefs.GetString("ONReview") == "True")
            {
                GameSystem._Instance._UI._ok.SetActivate(true);
                GameSystem._Instance._UI._ok.SetText("회상중 저장 불가","회상 중에는 저장이 불가능 합니다.");
                return;
            }
            // 해당슬롯에 저장하는지 확인 팝업 활성화, 누른 슬롯 저장
            curSaveSlot = clickedObj.transform.parent.name;//**현재 클릭한 슬롯의 idxcurSaveSlot에 저장(playerprefs를 사용해 오브젝트 이름에따라서 값저장)
            GameSystem._Instance._UI._popUp.SetActivate(true);
            GameSystem._Instance._UI._popUp.SetText("해당 슬롯에 저장하시겠습니까?", "기존 데이터는 삭제 됩니다.");
        }
        // 카페에서 저장시 확인 팝업 활성화, 현재 누른 슬롯 저장
        else if (SceneManager.GetActiveScene().name == "Cafe")
        {
            curSaveSlot = clickedObj.transform.parent.name;//**현재 클릭한 슬롯의 idxcurSaveSlot에 저장(playerprefs를 사용해 오브젝트 이름에따라서 값저장)
            CafeSystem._Instance._UI._popUp.SetActivate(true);
            CafeSystem._Instance._UI._popUp.SetText("해당 슬롯에 저장하시겠습니까?", "기존 데이터는 삭제 됩니다.");
            CafeSystem._Instance._UI._popUp._Slot = curSaveSlot;
            CafeSystem._Instance._UI._popUp.Mode = "Save";
        }
    }
    // 창 닫기, 세이브창 닫기
    public void OnClickExit()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            GameSystem._Instance._UI._saveMenu.SetActivate(false);
            GameSystem._Instance._UI._menuPanel.SetActivate(false);
        }
        else if (SceneManager.GetActiveScene().name == "Cafe")
        {
            CafeSystem._Instance._UI._saveMenu.SetActivate(false);
            CafeSystem._Instance._UI._menuPanel.SetActivate(false);
            CafeSystem._Instance._UI._Cafe.SetActivate(true);
        }

    }
}
