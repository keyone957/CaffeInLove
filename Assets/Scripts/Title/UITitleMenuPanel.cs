using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UITitleMenuPanel : UIWindow
{
    private void Start()
    {
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.transform.Find("Menu").SetAsLastSibling();
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.transform.Find("Page").SetAsLastSibling();
    }
    public void ExitMenu()
    {
        Destroy(TitleSystem._Instance._UIManager._Extra.TargetList);
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.SetActivate(false);
        TitleSystem._Instance._UIManager._UIBack.SetActive(false);
        TitleSystem._Instance._UIManager._TitleMenu.SetActivate(true);
        TitleSystem._Instance._UIManager._uiTitleLoadMenu.SetActivate(false);
        //엑스트라가 활성화 되있으면 엑스트라 리셋
        if (TitleSystem._Instance._UIManager._Extra.IsActivate()) TitleSystem._Instance._UIManager._Extra.Clear();
        TitleSystem._Instance._UIManager._Extra.SetActivate(false);
        while (TitleSystem._Instance._UIManager._uiTitleMenuPanel.GetComponent<UITitlePage>().page != 0)
        {
            TitleSystem._Instance._UIManager._uiTitleMenuPanel.GetComponent<UITitlePage>().OnPrevClik();
        }
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.transform.Find("Page").gameObject.SetActive(true);
        TitleSystem._Instance._UIManager._uiTitleSetting.SetActivate(false);
    }
    public void setMenuText(string text, string sub)
    {
        //메뉴에서 큰메뉴 이름 set해줌
        Text menuText = TitleSystem._Instance._UIManager._uiTitleMenuPanel.transform.Find("Menu").GetComponent<Text>();
        menuText.text = text;
        Text subText = TitleSystem._Instance._UIManager._uiTitleMenuPanel.transform.Find("Sub").GetComponent<Text>();
        subText.text = sub;
    }
    public void ExtraMenu()
    {
        if (TitleSystem._Instance._UIManager._Extra.TargetList != null) Destroy(TitleSystem._Instance._UIManager._Extra.TargetList);
        TitleSystem._Instance._UIManager._Extra.SetActivate(true);
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.setMenuText("Extra / 추가요소", "수집된 요소나 보너스로 제공된 이야기를 볼 수 있습니다.");
        TitleSystem._Instance._UIManager._TitleMenu.SetActivate(false);
        TitleSystem._Instance._UIManager._uiTitleLoadMenu.SetActivate(false);
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.gameObject.transform.Find("Page").transform.Find("Page").gameObject.SetActive(true);
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.gameObject.transform.Find("Page").transform.Find("Prev").gameObject.SetActive(true);
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.gameObject.transform.Find("Page").transform.Find("Next").gameObject.SetActive(true);
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.transform.Find("Page").gameObject.SetActive(false);
        TitleSystem._Instance._UIManager._uiTitleSetting.SetActivate(false);
    }
    public void LoadMenu()
    {
        while (TitleSystem._Instance._UIManager._uiTitleMenuPanel.GetComponent<UITitlePage>().page != 0)
        {
            TitleSystem._Instance._UIManager._uiTitleMenuPanel.GetComponent<UITitlePage>().OnPrevClik();
        }
        TitleSystem._Instance._UIManager._Extra.SetActivate(false);
        TitleSystem._Instance._UIManager._uiTitleLoadMenu.SetActivate(true);
        TitleSystem._Instance._UIManager._TitleMenu.SetActivate(false);
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.transform.Find("Page").gameObject.SetActive(true);
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.setMenuText("불러오기", "게임을 불러옵니다.");
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.GetComponent<UITitlePage>().Max = 4;
        TitleSystem._Instance._UIManager._uiTitleSetting.SetActivate(false);
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.transform.Find("Page").transform.Find("Page").gameObject.SetActive(true);
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.transform.Find("Page").transform.Find("Prev").gameObject.SetActive(true);
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.transform.Find("Page").transform.Find("Next").gameObject.SetActive(true);
    }

    public void ClickSettingMenu()
    {
        if (TitleSystem._Instance._UIManager._Extra.TargetList != null) Destroy(TitleSystem._Instance._UIManager._Extra.TargetList);
        TitleSystem._Instance._UIManager._Extra.SetActivate(false);
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.setMenuText("Setting / 환경설정");
        TitleSystem._Instance._UIManager._uiTitleLoadMenu.SetActivate(false);
        TitleSystem._Instance._UIManager._uiTitleSetting.SetActivate(true);
        TitleSystem._Instance._UIManager._uiTitleSetting.DialogueModeSpr(PlayerPrefs.GetString("automode"));
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.transform.Find("Page").gameObject.SetActive(true);
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.transform.Find("Page").transform.Find("Page").gameObject.SetActive(false);
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.transform.Find("Page").transform.Find("Prev").gameObject.SetActive(false);
        TitleSystem._Instance._UIManager._uiTitleMenuPanel.transform.Find("Page").transform.Find("Next").gameObject.SetActive(false);
    }


    public void setMenuText(string text)
    {
        if (SceneManager.GetActiveScene().name == "Title")
        {
            Text menuText = TitleSystem._Instance._UIManager._uiTitleMenuPanel.transform.Find("Menu").GetComponent<Text>();
            menuText.text = text;
        }
    }


}