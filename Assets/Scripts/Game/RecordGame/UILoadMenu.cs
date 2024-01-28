using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game;
using UnityEngine.SceneManagement;

public class UILoadMenu : UIWindow
{
    [SerializeField]
    private Button[] _loadSlot = null;
    public string curLoadSlot; // 클릭한 슬롯 저장용
    private void Awake()
    {
        foreach (Button button in _loadSlot)
        {
            button.onClick.AddListener(() => OnClickSlot(button.gameObject));
        }

    }
    // 닫기 창
    private void OnClickExit()
    {
        string curScene = SceneManager.GetActiveScene().name;
        if (curScene == "Title")
        {
            return;
        }
        else if (curScene == "Game")
        {
            GameSystem._Instance._UI._loadMenu.SetActivate(false);
            GameSystem._Instance._UI._menuPanel.SetActivate(false);
        }
        else if (curScene == "Cafe")
        {
            CafeSystem._Instance._UI._loadMenu.SetActivate(false);
            CafeSystem._Instance._UI._menuPanel.SetActivate(false);

        }
    }
    // 로드 슬롯 클릭시
    private void OnClickSlot(GameObject clickedObj)
    {
        string curScene = SceneManager.GetActiveScene().name;
        curLoadSlot = clickedObj.transform.parent.name; // 클릭한 슬롯 저장
        // 해당 슬롯에 해당하는 PlayerPrefs가 없으면 빈슬롯 안내
        if (!PlayerPrefs.HasKey(curLoadSlot))
        {
            if (curScene == "Game")
            {
                GameSystem._Instance._UI._ok.SetActivate(true);
                GameSystem._Instance._UI._ok.SetText("빈 슬롯", "해당 슬롯에는 데이터가 없습니다.");
            }
            else if (curScene == "Cafe")
            {
                CafeSystem._Instance._UI._ok.SetActivate(true);
                CafeSystem._Instance._UI._ok.SetText("빈 슬롯", "해당 슬롯에는 데이터가 없습니다.");
            }
            return;
        }
        // 해당하는 데이터가 있으면 확인 팝업 생성
        else
        {
            if (curScene == "Game")
            {
                GameSystem._Instance._UI._popUp.SetActivate(true);
                GameSystem._Instance._UI._popUp.SetText("데이터를 불러오시겠습니까?", "저장 되지 않은 진행사항은 삭제 됩니다.");
            }
            else if (curScene == "Cafe")
            {
                CafeSystem._Instance._UI._popUp.SetActivate(true);
                CafeSystem._Instance._UI._popUp.SetText("데이터를 불러오시겠습니까?", "저장 되지 않은 진행사항은 삭제 됩니다.");
                CafeSystem._Instance._UI._popUp._Slot = curLoadSlot;
                CafeSystem._Instance._UI._popUp.Mode = "Load";
            }
        }
    }
    // 세이브시 날짜 저장한것을 로드에도 반영
    public void setDate(string slot,string date)
    {
        if (PlayerPrefs.HasKey(slot))//**savemenu에서 날짜를 저장할때 loadmenu에서도 같이 저장을 해놔야함
        {
            if (SceneManager.GetActiveScene().name == "Game")
            {
                GameObject dateObj = GameSystem._Instance._UI._loadMenu.transform.Find(slot).gameObject.transform.Find("Date").gameObject;
                dateObj.SetActive(true);
                dateObj.GetComponent<Text>().text = date;
            }
            else if (SceneManager.GetActiveScene().name == "Cafe")
            {
                GameObject dateObj = CafeSystem._Instance._UI._loadMenu.transform.Find(slot).gameObject.transform.Find("Date").gameObject;
                dateObj.SetActive(true);
                dateObj.GetComponent<Text>().text = date;
            }

        }
    }
}

