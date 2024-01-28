using Game;
using UnityEngine;
using UnityEngine.UI;


public class UIEpiPopup : UIWindow
{
    [SerializeField]
    private GameObject _Title = null;
    [SerializeField]
    private GameObject _Name = null;
    [SerializeField]
    private GameObject _Coast = null;
    [SerializeField]
    private GameObject _Image = null;
    private int EpiCoast = 0;
    private string _ScenarioName = "";
    private string _LabelName = "";
    public void SetEpisodeInfo(string Title, string Name, int Coast,string ScenarioName,string LabelName, UnityEngine.Sprite thumbnail = null)
    {
        EpiCoast = Coast;
        // 제목 세팅
        Text TitleText = _Title.GetComponentInChildren<Text>();
        TitleText.text = Title;
        // 이름 세팅
        Text epiText = _Name.GetComponent<Text>();
        epiText.text = Name;    
        // 필요가격 세팅
        Text coast = _Coast.GetComponent<Text>();
        int number = Coast;
        string formattedString = number.ToString("C0").Replace("$", "￦ ");
        coast.text = formattedString;
        // 시나리오 이름 세팅
        _ScenarioName = ScenarioName;
        // 라벨 세팅
        _LabelName = LabelName;
        // 이미지가 있다면 이미지 세팅
        if (thumbnail != null) { _Image.GetComponent<Image>().sprite = thumbnail; }
        SetActivate(false);
        SetActivate(true);
    }
    // 에피소드 팝업 클릭 시
    public void OnEpiPopupClicked()
    {
        // 소지금이 가격보다 크면 에피소드 해금
        int Poket = int.Parse(PlayerPrefs.GetString("1" + "_cafe").Split("|")[1]);
        if (Poket >= EpiCoast)
        {
            CafeSystem._Instance._UI._popUp.SetActivate(true);
            //처음글자 메인스토리인지 서브인지 판단
            if (_Title.GetComponentInChildren<Text>().text.StartsWith("메인")) CafeSystem._Instance._UI._popUp.Mode = "MainEpiBuy";
            else if (_Title.GetComponentInChildren<Text>().text.StartsWith("서브")) CafeSystem._Instance._UI._popUp.Mode = "SubEpiBuy";
            // 모드 설정및 정보 세팅
            CafeSystem._Instance._UI._popUp.SetText("선택한 에피소드를 보시겠습니까?");
            CafeSystem._Instance._UI._popUp._Coast = EpiCoast;
            CafeSystem._Instance._UI._popUp._Target = gameObject;
            CafeSystem._Instance._UI._popUp._ScenarioName = _ScenarioName;
            CafeSystem._Instance._UI._popUp._LabelName = _LabelName;
            CafeSystem._Instance._UI._popUp._Poket = Poket;
        }
        // 소지금 부족시 팝업 창 띄우기
        else
        {
            CafeSystem._Instance._UI._ok.SetActivate(true);
            CafeSystem._Instance._UI._ok.SetText("돈이 부족합니다.", "카페 영업을 통해 돈을 벌 수 있습니다.");
        }

    }
}
