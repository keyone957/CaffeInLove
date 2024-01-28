using UnityEngine;
using UnityEngine.UI;
using Game;
using UnityEngine.SceneManagement;

public class UIPage : MonoBehaviour
{
    public int page = 0; // 현재 페이지
    public int Max = 4;  // 마지막 페이지
    GameObject _menu = null;
    // 처음에 창을 열었으면 페이지 적용
    void Start()
    {
        Max = 4;
        ApplyPage();
    }
    // 다음 페이지 이동
    public void OnNextClik()
    {
        // 페이지 증가
        if (page < Max) page += 1;
        else return;
        // 세이브와 로드 메뉴에 모두 적용시키기 위해 2번 반복
        for (int j = 0; j < 2; j++)
        {
            // 세이브 메뉴 적용
            if (j == 0)
            {
                if (SceneManager.GetActiveScene().name == "Game")
                {
                    // 엑스트라도 페이지 이동
                    if (GameSystem._Instance._UI._Extra.IsActivate()) GameSystem._Instance._UI._Extra.OnExtraNext();
                    // 세이브 메뉴적용
                    _menu = GameSystem._Instance._UI._saveMenu.gameObject;
                }
                else if (SceneManager.GetActiveScene().name == "Cafe")
                {
                    // 엑스트라에도 페이지 적용
                    if (CafeSystem._Instance._UI._Extra.IsActivate()) CafeSystem._Instance._UI._Extra.OnExtraNext();
                    // 세이브 메뉴 적용
                    _menu = CafeSystem._Instance._UI._saveMenu.gameObject;
                } 
            }
            // 로드 메뉴적용
            else
            {
                // 엑스트라 적용
                if (SceneManager.GetActiveScene().name == "Game") _menu = GameSystem._Instance._UI._loadMenu.gameObject;
                // 로드 메뉴 적용
                else if (SceneManager.GetActiveScene().name == "Cafe") _menu = CafeSystem._Instance._UI._loadMenu.gameObject;

            }
            // 선택한 메뉴의 이름 변경 1~6, 7~12...
            for (int i = (page - 1) * 6 + 6; i >= (page - 1) * 6 + 1; i--)
            {
                // 슬롯 별 이름 바꾸기
                Transform slot = _menu.transform.Find(i.ToString()).transform;
                int number = int.Parse(slot.gameObject.name);
                slot.name = (number + 6).ToString();
            }
        }
        //페이지 적용
        ApplyPage();
    }
    // 이전 페이지 이동
    public void OnPrevClik()
    {
        if (page >= 1) page -= 1;
        else return;
        for (int j = 0; j < 2; j++)
        {
            // 세이브 메뉴 변화
            if (j == 0)
            {
                if (SceneManager.GetActiveScene().name == "Game")
                {
                    // 엑스트라가 활성화 되 있으면 엑스트라도 적용
                    if (GameSystem._Instance._UI._Extra.IsActivate()) GameSystem._Instance._UI._Extra.OnExtraPrev();
                    _menu = GameSystem._Instance._UI._saveMenu.gameObject;
                }
                else if (SceneManager.GetActiveScene().name == "Cafe")
                {
                    if (CafeSystem._Instance._UI._Extra.IsActivate()) CafeSystem._Instance._UI._Extra.OnExtraPrev();
                    _menu = CafeSystem._Instance._UI._saveMenu.gameObject;
                } 
            }
            // 로드 메뉴 변화
            else
            {
                if (SceneManager.GetActiveScene().name == "Game") _menu = GameSystem._Instance._UI._loadMenu.gameObject;
                else if (SceneManager.GetActiveScene().name == "Cafe") _menu = CafeSystem._Instance._UI._loadMenu.gameObject;

            }
            // 선택한 메뉴의 이름 변경 1~6, 7~12...
            for (int i = (page + 1) * 6 + 1; i < (page + 1) * 6 + 6 + 1; i++)
            {
                // 슬롯 별 이름 바꾸기
                Transform slot = _menu.transform.Find(i.ToString()).transform;
                int number = int.Parse(slot.gameObject.name);
                slot.name = (number - 6).ToString();
            }
        }
        //페이지 적용
        ApplyPage();
    }
    // 창 닫기
    public void OnExitClik()
    {
        // 닫기 버튼
        if (SceneManager.GetActiveScene().name == "Game")
        {
            GameSystem._Instance._UI._Dialogue.SetActivate(true);
            // 1번 슬롯 유지를 위해 맨 앞으로
            OnPrevClik(); OnPrevClik(); OnPrevClik(); OnPrevClik();
            // 켜진 메뉴 닫고 게임의 메뉴창, 입력 활성화
            GameSystem._Instance._UI._UIBack.SetActive(false);
            GameSystem._Instance._UI._saveMenu.SetActivate(false);
            GameSystem._Instance._UI._loadMenu.SetActivate(false);
            GameSystem._Instance._UI._Menu.SetActivate(true);
            GameSystem._Instance._UI._Input.SetActivate(true);
            // 선택지 활성화
            if (GameSystem._Instance._UI._Select != null) GameSystem._Instance._UI._Select.SetActivate(true);
            //엑스트라가 활성화 되있으면 엑스트라 리셋
            GameSystem._Instance._UI._Extra.SetActivate(false);
            // 엑스트라 페이지를 맨앞으로 이동
            while (GameSystem._Instance._UI._menuPanel.GetComponent<UIPage>().page != 0)
            {
                GameSystem._Instance._UI._menuPanel.GetComponent<UIPage>().OnPrevClik();
            }
            // 페이지 조절 버튼 활성화(엑스트라에서 비활성화 하는걸 복구)
            GameSystem._Instance._UI._menuPanel.transform.Find("Page").gameObject.SetActive(true);
            // 메뉴창 비 활성활
            GameSystem._Instance._UI._menuPanel.SetActivate(false);
            GameSystem._Instance._UI._Menu.gameObject.transform.Find("Container").gameObject.SetActive(false);
            // 엑스트라에서 생성한 창 삭제
            Destroy(GameSystem._Instance._UI._Extra.TargetList);
        }
        // 카페에서의 경우
        else if (SceneManager.GetActiveScene().name == "Cafe")
        {
            // 1번 슬롯 유지를 위해 맨 앞으로
            OnPrevClik(); OnPrevClik(); OnPrevClik(); OnPrevClik();
            // 다른 메뉴 닫고 입력, 메뉴 활성화
            CafeSystem._Instance._UI._UIBack.SetActive(false);
            CafeSystem._Instance._UI._menuPanel.SetActivate(false);
            CafeSystem._Instance._UI._saveMenu.SetActivate(false);
            CafeSystem._Instance._UI._loadMenu.SetActivate(false);
            CafeSystem._Instance._UI._Menu.SetActivate(true);
            CafeSystem._Instance._UI._Input.SetActivate(true);
            CafeSystem._Instance._UI._Cafe.SetActivate(true);
            //엑스트라가 활성화 되있으면 엑스트라 리셋
            CafeSystem._Instance._UI._Extra.SetActivate(false);
            // 엑스트라를 첫페이지로 이동
            while (CafeSystem._Instance._UI._menuPanel.GetComponent<UIPage>().page != 0)
            {
                CafeSystem._Instance._UI._menuPanel.GetComponent<UIPage>().OnPrevClik();
            }
            // 페이지 조절 버튼 다시 활성화 후 메뉴창 비활성화
            CafeSystem._Instance._UI._menuPanel.transform.Find("Page").gameObject.SetActive(true);
            CafeSystem._Instance._UI._menuPanel.SetActivate(false);
            // 엑스트라에서 생성한 창 삭제
            Destroy(CafeSystem._Instance._UI._Extra.TargetList);
        }

    }
    // 페이지 적용
    public void ApplyPage()
    {
        for (int j = 0; j < 2; j++)
        {
            // 세이브 메뉴 적용
            if (j == 0)
            {
                if (SceneManager.GetActiveScene().name == "Game") _menu = GameSystem._Instance._UI._saveMenu.gameObject;
                else if (SceneManager.GetActiveScene().name == "Cafe") _menu = CafeSystem._Instance._UI._saveMenu.gameObject;
            }
            // 로드 메뉴 적용
            else
            {
                if (SceneManager.GetActiveScene().name == "Game") _menu = GameSystem._Instance._UI._loadMenu.gameObject;
                else if (SceneManager.GetActiveScene().name == "Cafe") _menu = CafeSystem._Instance._UI._loadMenu.gameObject;

            }
            // 슬롯박스의 이름 바꾸기, 데이터값 적용
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
                    //날짜
                    Transform Date = slot.transform.Find("Date").transform;
                    Date.gameObject.SetActive(true);
                    Text dateText = Date.GetComponentInChildren<Text>();
                    dateText.text = parseInfo[2];

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
        // 처음페이지인 경우 흰색으로 변경
        if (page == 0) ColorUtility.TryParseHtmlString(white, out color);
        prevImage.color = color;

        //다음 버튼 색 변경
        Transform next = _Page.transform.Find("Next");
        Image nextImage = next.GetComponent<Image>();
        ColorUtility.TryParseHtmlString(green, out color);
        // 마지막 페이지인 경우 흰색으로 변경
        if (page == Max) ColorUtility.TryParseHtmlString(white, out color);
        nextImage.color = color;

    }
}
