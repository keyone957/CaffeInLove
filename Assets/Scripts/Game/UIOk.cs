using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Game;
public class UIOk : UIWindow
{
    // 메시지 창 확인 누름
    public void OnOKClicked()
    {
        gameObject.SetActive(false); // 자신 비활성화
        // 진행중인 씬 별로 계층 관계상 비활성화 했던 오브젝트 활성화
        if (SceneManager.GetActiveScene().name == "Title")
        {
            TitleSystem._Instance._UIManager._UIBack.SetActive(true);
            TitleSystem._Instance._UIManager._uiTitleMenuPanel.SetActivate(true);
        }
        else if (SceneManager.GetActiveScene().name == "Game")
        {
            GameSystem._Instance._UI._popUp.SetActivate(false);
            GameSystem._Instance._UI._menuPanel.SetActivate(true);
            GameSystem._Instance._UI._Menu.SetActivate(false);
            GameSystem._Instance._UI._Dialogue.SetActivate(false);
            GameSystem._Instance._UI._UIBack.SetActive(true);
        }
        else if (SceneManager.GetActiveScene().name == "Cafe")
        {
            CafeSystem._Instance._UI._popUp.SetActivate(false);
            CafeSystem._Instance._UI._UIBack.SetActive(true);
        }
    }
    // 메시지 창 제목, 부가설명 설정
    public void SetText(string Main = "", string Sub = "")
    {
        Transform main = gameObject.transform.Find("Main").transform;
        Transform sub = gameObject.transform.Find("Sub").transform;
        Text mainText = main.GetComponent<Text>();
        Text subText = sub.GetComponent<Text>();
        mainText.text = Main;
        subText.text = Sub;
    }
    
}
