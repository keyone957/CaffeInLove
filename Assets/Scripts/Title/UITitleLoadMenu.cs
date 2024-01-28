using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITitleLoadMenu :UIWindow
{
    [SerializeField]
    private Button[] _loadSlot = null;
   

    public string curLoadSlot;

    private void Awake()
    {
        foreach (Button button in _loadSlot)
        {
            button.onClick.AddListener(() => OnClickSlot(button.gameObject));  
        }
    }
    public string objname()//현재 클릭한 슬롯 오브젝트 이름  return
    {
        return curLoadSlot;
    }
    public void OnClickSlot(GameObject clickedObj)
    {
        //클릭한 슬롯에 데이터가 있으면 불러올것인지 아닌지 팝업창을 띄우고 없으면 빈슬롯이라고 팝업을 띄워줌
        if (PlayerPrefs.HasKey(clickedObj.transform.parent.name))
        {
            curLoadSlot = clickedObj.transform.parent.name;
            TitleSystem._Instance._UIManager._uiTitlePopUp.SetText("데이터 불러오기", "해당 슬롯에 저장된 데이터를 불러옵니까?");
            TitleSystem._Instance._UIManager._uiTitlePopUp.SetActivate(true);
        }
        else if(!PlayerPrefs.HasKey(clickedObj.transform.parent.name))
        {
            TitleSystem._Instance._UIManager._uiOk.SetActivate(true);
            TitleSystem._Instance._UIManager._uiOk.SetText("빈 데이터 슬롯", "해당 슬롯은 비어 있습니다");

        }
    }

}
