using App;
using Game;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UICafe : UIWindow
{
    [SerializeField]
    public GameObject _WeekInfo = null;
    [SerializeField]
    public GameObject _MoneyInfo = null;
    [SerializeField]
    public GameObject _DecorInfo = null;
    [SerializeField]
    public GameObject _LastEpisodeInfo = null;
    [SerializeField]
    public GameObject _EpisodeList = null;
    [SerializeField]
    public GameObject _EpisodeButton = null;
    [SerializeField]
    public GameObject _DecorButton = null;
    [SerializeField]
    public GameObject _OpenButton = null;
    public bool _hasOpened = false;
    public int _decorBonus = -1; 
    public int _review = -1; 
    public int _visBonus = -1;
    public string _AutoSlot = "1";

    public  static int _DecorType = 4;
    public string[] _DecorArry = new string[_DecorType];
    public override bool OnKeyInput()
    {
        return false;
    }
    // 가구점 열기
    public void OnDecorStoreClicked()
    {
        CafeSystem._Instance._UI._decorStore.SetActivate(true);
        CafeSystem._Instance._UI._decorStore.UpdateDecor();
        CafeSystem._Instance._UI._Menu.gameObject.transform.Find("Container").gameObject.SetActive(false);
    }
    // 미니게임 실행 팝업
    public void OnOpenStoreClicked()
    {
        CafeSystem._Instance._UI._popUp.SetActivate(true);
        if (!_hasOpened)
        {
            CafeSystem._Instance._UI._popUp.SetText("영업을 시작하시겠습니까?", "영업은 한 주에 한 번만 가능합니다.");
            CafeSystem._Instance._UI._popUp.Mode = "OpenCafe";
        }
        else
        {
            CafeSystem._Instance._UI._ok.SetActivate(true);
            CafeSystem._Instance._UI._ok.SetText("이번주 영업은 끝났습니다.", "영업은 한주에 한번만 가능합니다.");
        }
        CafeSystem._Instance._UI._Menu.gameObject.transform.Find("Container").gameObject.SetActive(false);
    }
    // 에피소드 리스트 출력/숨김
    public void OnEpisodeClicked()
    {
        _EpisodeList.SetActive(!_EpisodeList.activeSelf);
        CafeSystem._Instance._UI._Menu.gameObject.transform.Find("Container").gameObject.SetActive(false);
    }
    public void UpdateInfo()
    {
        // 1번 슬롯(가장 최근, 불러오기시 저장장소
        string[] Sceninfo = PlayerPrefs.GetString("1").Split("|"); //시나리오 정보
        string[] Cafeinfo = PlayerPrefs.GetString("1" + "_cafe").Split("|"); //미니게임 정보
        gameObject.transform.Find("Window").gameObject.SetActive(true);
        // 데이터가 없으면 초기화
        if (Sceninfo[0]=="" || Cafeinfo[0]=="")
        {
            Array.Resize(ref Sceninfo, 4);
            Sceninfo[0] = "1"; // 주차
            Sceninfo[1] = "0"; // 시나리오 인덱스
            Sceninfo[2] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // 날짜
            if (Sceninfo[3] == "") Sceninfo[3] = GameSystem._Instance._ScenarioInfo._defaultMainFileName; //기본파일
            Array.Resize(ref Cafeinfo, 10);
            Cafeinfo[0] = "1"; // 주차
            Cafeinfo[1] = "0"; // 소지금
            Cafeinfo[2] = "[0,0,0,0,0,0]"; // 소지 가구 리스트 0: 미소지, 1: 소지
            Cafeinfo[3] = "0";// 가구 보너스(수익)
            Cafeinfo[4] = "0";// 누적리뷰
            Cafeinfo[5] = "0";// 가구 보너스(손님수)
            Cafeinfo[6] = "0";// 카페 오픈
            Cafeinfo[7] = "Cafe";// 카페 오픈
            int[] SceneList = new int[36];
            SceneList[0] = 1;
            SceneList.ToString();
            Cafeinfo[8] = "0";//누적 방문자
            Cafeinfo[9] = "[1,0,0,0]"; //서브시나리오리스트
        }
        _WeekInfo.GetComponentInChildren<Text>().text = Cafeinfo[0] + "주차";
        string moneyString = int.Parse(Cafeinfo[1]).ToString("C0").Replace("$", "￦ ");
        _MoneyInfo.GetComponentInChildren<Text>().text = "소지금 " + moneyString;
        string decor = Cafeinfo[2];
        decor = decor.Replace("[", "").Replace("]", "");
        string[] decorArry = decor.Split(",");
        int decorCount = 0;
        for (int i = decorArry.Length - 1; i >= 0; i--)
        {
            if (decorArry[i] == "1") { decorCount++; }
        }
        _DecorInfo.GetComponentInChildren<Text>().text = "구매한 가구 " + decorCount.ToString() + "개"; //구매가구수
        _decorBonus = int.Parse(Cafeinfo[3]);                                     // 가구보너스 추가수익
        _review = int.Parse(Cafeinfo[4]);                                         // 누적리뷰
        _visBonus = int.Parse(Cafeinfo[5]);                                       // 가구보너스 손님수
        if (Cafeinfo[6] == "1") CafeSystem._Instance._UI._Cafe._hasOpened = true; // 영업여부
        else CafeSystem._Instance._UI._Cafe._hasOpened = false;                   // 안구하면 배드인겅
        //주차, 루트 별 에피소드 팝업
        SetEpisode(Cafeinfo[0], Sceninfo[3]);//주차와 시나리오 이름으로 팝업 결정
        // 에피소드가 있으면 마크 활성화, 없으면 비활성화
        if (_EpisodeList.transform.childCount > 0) { _EpisodeButton.transform.Find("Mark").gameObject.SetActive(true); }
        else _EpisodeButton.transform.Find("Mark").gameObject.SetActive(false);

    }
    public void AddEpiPopup(string title, string name,int Coast, string ScenarioName,string LabelName)
    {
        UIEpiPopup newEpi = CafeSystem._Instance._UI.OpenWindow(Define._uiEpiPopUpPrefabPath,_EpisodeList.transform) as UIEpiPopup;
        //마크 활성화
        _EpisodeButton.transform.Find("Mark").gameObject.SetActive(true);
        newEpi.SetActivate(true);
        newEpi.SetEpisodeInfo(title, name,Coast, ScenarioName,LabelName);
    }
    // 에피소드 리스트 최신화를 위해 제작
    public void ClearList()
    {
        int childCount = _EpisodeList.transform.childCount;

        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = _EpisodeList.transform.GetChild(i);
            if( "EpiPopUp" == child.name) { Destroy(child.gameObject); }
        }

       
    }
    // 최근 에피소드 텍스트 세팅
    public void SetEpisodeInfo(int epiNum,string epiDir)
    {
        Transform Head = _LastEpisodeInfo.transform.Find("Head");
        Text HeadText = Head.GetComponentInChildren<Text>();
        HeadText.text = "Episode #" + epiNum.ToString("D2");
        Transform Text = _LastEpisodeInfo.transform.Find("Text");
        Text epiText = Text.GetComponent<Text>();
        epiText.text = epiDir;
        
    }
    // 에피소드를 주차와 루트(파일)에 따라 구분
    private void SetEpisode(string week,string scenario ="")
    {
        // ---- 기본 공동 루트
        if (week == "1")
        {
            ClearList();
            SetEpisodeInfo(1, "갑작스런 형의 부탁으로 카페를 운영하게 된 나 \n 거기다 하나뿐인 알바도 만만치 않다?!");
            AddEpiPopup("메인 에피소드", "사장님은 연습중", 500, "Scenario001", "Main_01");

        }
        else if (week == "2")
        {
            ClearList();
            SetEpisodeInfo(2, "대체 내 주변 사람들은 멀쩡하지 않는거지라며 자신이 항상 을이라는걸 깨닫는다");
            AddEpiPopup("메인 에피소드", "3주차로", 500, "Scenario001", "Main_02");

        }
        else if (week == "3")
        {
            ClearList();
            SetEpisodeInfo(3, "술의 힘으로 밤새 지아와의 사이가 돈돈(?)해졌다");
            AddEpiPopup("메인 에피소드", "4주차로", 500, "Scenario001", "Main_03");
        }
        else if (week == "4")
        {
            ClearList();
            SetEpisodeInfo(4, "비에 젖은 루왁을 구해주는 동안에 봤던 아린씨의 얼굴은 잊지 못 할 것이다.");
            AddEpiPopup("메인 에피소드", "5주차로", 500, "Scenario001", "Main_04");
        }
        else if (week == "5")
        {
            ClearList();
            SetEpisodeInfo(5, "버릇없는 손놈이 나타났다. 다음에 다시 나타나진 않겠지? 그것보다 서연씨와의 데이트가 기대된다.");
            AddEpiPopup("메인 에피소드", "6주차로", 500, "Scenario001", "Main_05");
        }
        else if (week == "6")
        {
            ClearList();
            SetEpisodeInfo(6, "서연씨와의 데이트는 두 사람의 방해로 무산 됬다. 하지만 서연씨에 마지막 모습이 마음에 남는다.");
            AddEpiPopup("메인 에피소드", "7주차로", 500, "Scenario001", "Main_06");
        }
        else if (week == "7")
        {
            ClearList();
            SetEpisodeInfo(7, "콜라보 기획은 대성공으로 끝났다. 우리들은 그동안의 수고를 달래기 위해 뒷풀이로 향한다.");
            AddEpiPopup("메인 에피소드", "8주차로", 500, "Scenario001", "Main_07");
        }
        else if (week == "8")
        {
            ClearList();
            SetEpisodeInfo(8, "지난 회식에서 두 약속을 받고 난 뒤, 나는 어떻게 해야 할지 고민한다.");
            AddEpiPopup("메인 에피소드", "9주차로", 500, "Scenario001", "Main_08");
        }
        // ----------------지아루트 -----------------
        else if (week == "9" && scenario == "jia_route")
        {
            ClearList();
            SetEpisodeInfo(9, "나는 지아를 선택했다. 나는 앞으로 서연씨를 어떻게 보지?");
            AddEpiPopup("메인 에피소드", "10주차 지아", 500, "jia_route", "Main_09");
        }
        else if (week == "10" && scenario == "jia_route")
        {
            ClearList();
            SetEpisodeInfo(10, "지아와 빛축제에서 행복한 시간을 보냈다. 이순간이 계속 되 길");
            AddEpiPopup("메인 에피소드", "11주차 지아", 500, "jia_route", "Main_10");
        }
        else if (week == "11" && scenario == "jia_route")
        {
            ClearList();
            SetEpisodeInfo(11, "지아와의 성수못 데이트에서 그녀의 다른 매력을 볼 수 있었다.");
            AddEpiPopup("메인 에피소드", "12주차 지아", 500, "jia_route", "Main_11");
        }
        else if (week == "12" && scenario == "jia_route")
        {
            ClearList();
            SetEpisodeInfo(12, "지아와 같이 예전에 다니던 학교를 갔다. 지아의 기분을 풀기 위해 OO랜드에 간다.");
            AddEpiPopup("메인 에피소드", "13주차 지아", 500, "jia_route", "Main_12");
        }
        else if (week == "13" && scenario == "jia_route")
        {
            ClearList();
            SetEpisodeInfo(13, "놀이공원에서 서로의 감정을 나눈 우리는 결말을 향한다.");
            AddEpiPopup("메인 에피소드", "14주차 지아", 500, "jia_route", "Main_13");
        }
        // ----------------서연루트 -----------------
        else if (week == "9" && scenario == "yeon_route")
            
        {
            ClearList();
            SetEpisodeInfo(9, "나는 서연씨를 선택했다.");
            AddEpiPopup("메인 에피소드", "10주차 서연", 500, "yeon_route", "Main_14");
        }
        else if (week == "10" && scenario == "yeon_route")
        {
            ClearList();
            SetEpisodeInfo(10, "서연씨와 아쿠아 리움에서 즐거운 시간을 보냈다.");
            AddEpiPopup("메인 에피소드", "11주차 서연", 500, "yeon_route", "Main_15");
        }
        else if (week == "11" && scenario == "yeon_route")
        {
            ClearList();
            SetEpisodeInfo(11, "둘의 거리가 조금 더 가까워졌다.");
            AddEpiPopup("메인 에피소드", "12주차 서연", 500, "yeon_route", "Main_16");
        }
        else if (week == "12" && scenario == "yeon_route")
        {
            ClearList();
            SetEpisodeInfo(12, "여러 계획이 무산 됬지만 이제부터 이다.");
            AddEpiPopup("메인 에피소드", "13주차 서연", 500, "yeon_route", "Main_17");
        }
        else if (week == "13" && scenario == "yeon_route")
        {
            ClearList();
            SetEpisodeInfo(13, "이상한 손놈을 해치웠다. 그리고 그녀와의 동거가 시작된다..");
            AddEpiPopup("메인 에피소드", "14주차 서연", 500, "yeon_route", "Main_18");
        }
        else if (week == "14" && scenario == "yeon_route")
        {
            ClearList();
            SetEpisodeInfo(14, "둘이서 하나, 하나가 된 둘은 끝을 결말에 다가간다.");
            AddEpiPopup("메인 에피소드", "15주차 서연", 500, "yeon_route", "Main_19");
        }
    }
}
