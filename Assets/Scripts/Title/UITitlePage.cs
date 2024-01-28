using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game;
public class UITitlePage : MonoBehaviour
{
    public int page = 0;  // 현재 페이지
    public int Max = 4;   // 최대 페이지
    GameObject _menu=null;// 조절할 메뉴페이지(save or load)
    void Start()
    {
        Max = 4;
        ApplyPage();
        
    }
    // 다음 페이지 이동
    public void OnNextClik()
    {
        //엑스트라가 활성화 되있으면 엑스트라도 페이지 이동 실행
        if (TitleSystem._Instance._UIManager._Extra.IsActivate())  TitleSystem._Instance._UIManager._Extra.OnExtraNext();
        // 최대 페이지보다 작은 경우 페이지 증가, 최대페이지면 종료
        if (page < Max) page += 1;
        else return;
        // 로드 페이지의 오브젝트 이름을 1~6,7~12와 같이 변경(저장시 슬롯 이름 참조)
        if (TitleSystem._Instance._UIManager._uiTitleLoadMenu.IsActivate())
        {
            for (int i = (page - 1) * 6 + 6; i >= (page - 1) * 6 + 1; i--)
            {
                _menu = TitleSystem._Instance._UIManager._uiTitleLoadMenu.gameObject;
                Transform slot = _menu.transform.Find(i.ToString()).transform;
                int number = int.Parse(slot.gameObject.name);
                slot.name = (number + 6).ToString();

            }
        }
        // 페이지 변경시 변경사항 적용
        ApplyPage();
    }

    // 이전 페이지 이동
    public void OnPrevClik()
    {
        //엑스트라가 활성화 되있으면 엑스트라도 페이지 이동 실행
        if (TitleSystem._Instance._UIManager._Extra.IsActivate())
        {
            TitleSystem._Instance._UIManager._Extra.OnExtraPrev();
        }
        // 페이지가 0(맨앞)이 아니면 페이지 감소
        if (page >= 1) page -= 1;
        else return;
        // 로드페이지가 열려 있는경우 1~6,7~12..와 같이 이름 변경(로드시 슬롯 이름 참조)
        if (TitleSystem._Instance._UIManager._uiTitleLoadMenu.IsActivate())
        {
            for (int i = (page + 1) * 6 + 1; i < (page + 1) * 6 + 6 + 1; i++)
            {
                // 슬롯 별 이름 바꾸기
                _menu = TitleSystem._Instance._UIManager._uiTitleLoadMenu.gameObject;
                Transform slot = _menu.transform.Find(i.ToString()).transform;
                int number = int.Parse(slot.gameObject.name);
                slot.name = (number - 6).ToString();

            }
        }
        // 변경사항 적용
        ApplyPage();
    }
    // 페이지 적용
    public void ApplyPage()
    {
        // 로드창이 열려있으면 로드창의 슬롯 이름 변경, 데이터 받아 적용
        if (TitleSystem._Instance._UIManager._uiTitleLoadMenu.IsActivate())
        {
            _menu = TitleSystem._Instance._UIManager._uiTitleLoadMenu.gameObject;
            for (int i = page * 6 + 1; i < page * 6 + 6 + 1; i++)
            {
                // 슬롯박스 이름 바꾸기
                Transform slot = _menu.transform.Find(i.ToString()).transform;
                int number = int.Parse(slot.gameObject.name);
                Transform slotBox = slot.transform.Find("Slot");
                Text slotText = slotBox.GetComponentInChildren<Text>();
                slotText.text = "Slot " + i.ToString();
                // 데이터값이 있으면 불러오기
                if (PlayerPrefs.HasKey(i.ToString()))
                {
                    //현 슬롯, 저장했던 시나리오 idx, 저장한 날짜 정보
                    string[] parseInfo = PlayerPrefs.GetString(i.ToString()).Split("|");//**슬롯의 데이터들 구분자에 따라서 파싱
                    if (parseInfo[0] != "")
                    {
                        Transform Date = slot.transform.Find("Date").transform;
                        Date.gameObject.SetActive(true);
                        Text dateText = Date.GetComponentInChildren<Text>();
                        dateText.text = parseInfo[2];
                    }
                    

                    //TODO: 시나리오이름, 이미지
                }
                // 없으면 빈 숨김
                else
                {
                    Transform Date = slot.transform.Find("Date").transform;
                    Date.gameObject.SetActive(false);
                }
            }
        }
        // 엑스트라가 활성화 된 상태면 엑스트라 적용
        else if (TitleSystem._Instance._UIManager._Extra.IsActivate())
        {
            // 엑스트라의 목표 리스트에 변화적용
            if(TitleSystem._Instance._UIManager._Extra.TargetList != null)
                TitleSystem._Instance._UIManager._Extra.UpdateExtraPage();
        }
        //페이지 텍스트 변경
        Transform _Page = gameObject.transform.Find("Page");
        Transform Pslot = _Page.transform.Find("Page");
        Text PslotText = Pslot.GetComponentInChildren<Text>();
        PslotText.text = "Page " + (page + 1).ToString();
        // 색 저장
        Color color;
        string green = "#00A762";
        string white = "#FFFFFF";

        
        //이전 버튼 색 변경
        Transform prev = _Page.transform.Find("Prev");
        Image prevImage = prev.GetComponent<Image>();
        ColorUtility.TryParseHtmlString(green, out color);
        if (page == 0) ColorUtility.TryParseHtmlString(white, out color);
        prevImage.color = color;

        //다음 버튼 색 변경
        Transform next = _Page.transform.Find("Next");
        Image nextImage = next.GetComponent<Image>();
        ColorUtility.TryParseHtmlString(green, out color);
        if (page == Max) ColorUtility.TryParseHtmlString(white, out color);
        nextImage.color = color;
    }
}
