using UnityEngine;
using UnityEngine.UI;
using Game;
using System.Text.RegularExpressions;
using System.Linq;

public class UIDecorItem : UIWindow
{
    [SerializeField]
    GameObject _BuyButton = null;
    [SerializeField]
    GameObject _Coast = null;
    [SerializeField]
    GameObject _Name = null;
    [SerializeField]
    GameObject _Info = null;

    public bool selled = false;
    // 구입 버튼 클릭시
    public void OnBuyClicked()
    {
        // 숫자만 가져오기
        string inputString = _Coast.GetComponent<Text>().text;
        string pattern = @"\d+";
        MatchCollection matches = Regex.Matches(inputString, pattern);
        //int number = 888888888;
        //string formattedString = number.ToString("C0").Replace("$", "￦ ");
        // 가격을 정수로 저장
        string Coast = string.Join("", matches.Cast<Match>().Select(m => m.Value));

        //소지금이 가격보다 같거나 높은 상태에서만 구매 가능
        int Poket =  int.Parse(PlayerPrefs.GetString("1" + "_cafe").Split("|")[1]);
        if (Poket >= int.Parse(Coast))
        {
            // 아직 사지 않았으면 팝업 생성
            if (!selled)
            {
                CafeSystem._Instance._UI._popUp.SetActivate(true);
                Text name = _Name.GetComponent<Text>();
                Text coast = _Coast.GetComponent<Text>();
                Text info = _Info.GetComponent<Text>();
                // 모드설명, ui값 설정
                CafeSystem._Instance._UI._popUp.SetText( name.text + "를 구매하시겠습니까?",(coast.text +"\n\n"+info.text)); 
                CafeSystem._Instance._UI._popUp.Mode = "Buy";     
                CafeSystem._Instance._UI._popUp._Target = gameObject;
                CafeSystem._Instance._UI._popUp._Coast = int.Parse(Coast);
                CafeSystem._Instance._UI._popUp._Poket = Poket;
               
            }
           
        }
        // 소지금 부족시 팝업 창 띄우기
        else
        {
            CafeSystem._Instance._UI._ok.SetActivate(true);
            CafeSystem._Instance._UI._ok.SetText("돈이 부족합니다.","카페 영업을 통해 돈을 벌 수 있습니다.");
        }
       
        // 구매가능 상태 
    }
    // 이미 구매되있으면 이미지 전환
    public void SetWhite()
    {
        Image buyImage = _BuyButton.GetComponent<Image>();
        UnityEngine.Sprite white = Resources.Load<UnityEngine.Sprite>("Texture/Sprite/Rectangle White");
        buyImage.sprite = white;
        selled = true;
    }
}
