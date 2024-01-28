using UnityEngine;
using UnityEngine.UI;


public class UITitlePopUp : UIWindow
{
    // 게임로드 or 게임 회상 팝업 확인 클릭
    public void clickedYesOnTitle()
    {
        TitleSystem._Instance._UIManager._UIBack.SetActive(true);
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.SetActivate(true);
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.SetActivate(true);
        // 게임 씬 로드
        if (TitleSystem._Instance._UIManager._uiTitleLoadMenu.IsActivate())
            TitleSystem._Instance.LoadDoGameStartSequence();
        // 씬 회상
        else if (TitleSystem._Instance._UIManager._Extra.IsActivate())
        {
            string SceneName = TitleSystem._Instance._UIManager._Extra._ScenarioName;
            string label = TitleSystem._Instance._UIManager._Extra._Label;
            TitleSystem._Instance.LoadDoGameStartSequence(SceneName, label);
        }
    }
    // 확인창에 취소
    public void clickedNoOnTitle()
    {
        TitleSystem._Instance._UIManager._uiTitlePopUp.SetActivate(false);
        TitleSystem._Instance._UIManager._UIBack.SetActive(true);
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.SetActivate(true);
    }
    // 확인창 텍스트 설정 제목,부가 설명
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
