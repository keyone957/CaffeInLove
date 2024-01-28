using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICafeDecorStore : UIWindow
{
    [SerializeField]
    GameObject _Content = null;
    /// 가구점에 대하
    public void UpdateDecor()
    {
        // 가구 구입 여부(0,1)로 색 바뀜, "[0,0,0,0,0,0]"을 문자리스트로 전환
        string cafeInfo = PlayerPrefs.GetString("1" + "_cafe").Split("|")[2];
        cafeInfo = cafeInfo.Replace("[", "").Replace("]", "");
        string[] decorInfor = cafeInfo.Split(","); // 전환된 리스트
        for (int i = 0; i < _Content.transform.childCount; i++)
        {
            // 각 리스트별로 1(구입함)이면 색 변화
            Transform childObject = _Content.transform.GetChild(i);
            if (decorInfor[i] == "1") { childObject.GetComponent<UIDecorItem>().SetWhite(); }
        }
     }
    public void OnExitButton()
    {
        CafeSystem._Instance._UI._decorStore.SetActivate(false); // 가구점 닫기
    }
}
