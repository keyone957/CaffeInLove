using Game;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UICafeOpen : UIWindow
{
    [SerializeField]
    public Text _visitor = null;
    [SerializeField]
    public Text _profit = null;
    [SerializeField]
    public Text _WorkingText = null;
    [SerializeField]
    public Slider _Slider = null;
    [SerializeField]
    public GameObject _Complete = null;
    [SerializeField]
    public GameObject _Comment = null;
    [SerializeField]
    public GameObject _Result = null;
    public float _Duration = 5.0f;
    private float _OriSize = 0.0f;
    public bool showed = false;

    public int _TargetVisitor = 100; // 최종손님 오픈시 랜덤으로 변화
    public int _TargetSale = 100;    // 최종인당소비액 오픈시 랜덤으로 변화
    public int _NewReview = 0;       // 신규리뷰수 1~4 오른시 랜덤으로 변화
    //public int _Revi
    
    // 계산정보
    //public 
    public void OpenCafe()
    {
        // TODO: 카메라 축소
        _OriSize = Camera.main.orthographicSize;
        // 다른 불필요한 ui 닫기
        CafeSystem._Instance._UI._Cafe._DecorButton.SetActive(false);
        CafeSystem._Instance._UI._Cafe._EpisodeList.SetActive(false);
        CafeSystem._Instance._UI._Cafe._OpenButton.SetActive(false);
        CafeSystem._Instance._UI._Cafe._EpisodeButton.SetActive(false);
        CafeSystem._Instance._UI._Cafe._LastEpisodeInfo.SetActive(false);
        _Result.SetActive(false);  // 결과창 닫기
        _WorkingText.text = "영업 중..."; // 진행 메시지 초기화
        // 손님수와 매출액을 랜덤으로 선정
        int VisitorBonus = CafeSystem._Instance._UI._Cafe._visBonus; 
        System.Random random = new System.Random();
        int visitor = random.Next(10, 16); //10 ~15사이의 변수로 손님결정 
        float TargetVisitor = visitor * ((100 + VisitorBonus) / 100.0f); // 보너스 방문객
        CafeSystem._Instance._UI._open._TargetVisitor = (int)TargetVisitor;
        int sales = random.Next(visitor, visitor + 5) * 100; // 판매량 ,임의값: 차후 수정할것
        CafeSystem._Instance._UI._open._TargetSale = sales;
        CafeSystem._Instance._UI._open._NewReview = random.Next(1, 5);  // 리뷰갯수를 0 ~4에 발견

        StartCoroutine(OpenTask(_Duration));
        
        
    }
    public void CloseCafe()
    {
        // 카페 닫기, 버튼 다시 활성화
        CafeSystem._Instance._UI._Cafe._DecorButton.SetActive(true);
        CafeSystem._Instance._UI._Cafe._OpenButton.SetActive(true);
        CafeSystem._Instance._UI._Cafe._EpisodeButton.SetActive(true);
        CafeSystem._Instance._UI._Cafe._LastEpisodeInfo.SetActive(true);
        _Comment.SetActive(false);  // 같이 일하는 동료의 말풍선 비활성화
        _Complete.SetActive(false); // 완료 이미지 비활성화
        CafeSystem._Instance._UI._open.SetActivate(false);
        Camera.main.orthographicSize = _OriSize;
        // TODO: 카메라 확대
    }

    public IEnumerator OpenTask(float duration)
    {
        float startTime = Time.time;
        while (Time.time - (startTime + duration) < 0.0f)
        {
            //현재 시작과 목표시간에 대한 비율을 얻고 슬라이더 스크롤에 반영
            float rate = (Time.time - startTime) / duration;
            _Slider.value = (int)(rate*100);
            yield return null;
        }
       
    }
    public void ChangeSliderValue()
    {
        // 슬라이더가 특정 값일 경우 처리
        float nowValue = _Slider.value;
        if (nowValue == 10)
        {
            // 단계별 UI
            _Slider.value = 10;
            int nowVisitor = Convert.ToInt32(CafeSystem._Instance._UI._open._TargetVisitor * 0.1f);
            _visitor.text = "방문객: " + nowVisitor.ToString("#,##0") + "명";
            int profit = Convert.ToInt32((CafeSystem._Instance._UI._open._TargetVisitor * CafeSystem._Instance._UI._open._TargetSale) * 0.1f);
            string formattedString = profit.ToString("C0").Replace("$", "￦ ");
            _profit.text = "영업이익: " + formattedString;
        }
        else if (nowValue == 30)
        {
            // 말풍선 생성
            // 단계별 UI
            ShowComment("잔말말고 일이나 하세요!", "예지");
            _Slider.value = 30;
            nowValue = _Slider.value;
            int nowVisitor = Convert.ToInt32(CafeSystem._Instance._UI._open._TargetVisitor * 0.3f);
            _visitor.text = "방문객: " + nowVisitor.ToString("#,##0") + "명";
            int profit = Convert.ToInt32((CafeSystem._Instance._UI._open._TargetVisitor * CafeSystem._Instance._UI._open._TargetSale) * 0.3f);
            string formattedString = profit.ToString("C0").Replace("$", "￦ ");
            _profit.text = "영업이익: " + formattedString;
        }
        else if (nowValue == 60)
        {
            // 단계별 UI
            _Slider.value = 60;
            nowValue = _Slider.value;
            int nowVisitor = Convert.ToInt32(CafeSystem._Instance._UI._open._TargetVisitor * 0.6f);
            _visitor.text = "방문객: " + nowVisitor.ToString("#,##0") + "명";
            int profit = Convert.ToInt32((CafeSystem._Instance._UI._open._TargetVisitor * CafeSystem._Instance._UI._open._TargetSale) * 0.6f);
            string formattedString = profit.ToString("C0").Replace("$", "￦ ");
            _profit.text = "영업이익: " + formattedString;
        }

        else if (nowValue >= 99 && nowValue < 100)
        {
            // 단계별 UI 적용, 
            _Slider.value = 100;
            int nowVisitor = CafeSystem._Instance._UI._open._TargetVisitor;
            _visitor.text = "방문객: " + nowVisitor.ToString("#,##0") + "명";  // 방문객
            int profit = CafeSystem._Instance._UI._open._TargetVisitor * CafeSystem._Instance._UI._open._TargetSale; // 중간 수익
            string formattedString = profit.ToString("C0").Replace("$", "￦ "); // ￦ 888,888,888 형식
            int review = CafeSystem._Instance._UI._open._NewReview;
            _profit.text = "영업이익: " + formattedString; // 중간 수익 표시
            _WorkingText.text = "영업 완료!"; // 상태 메시지 변화
            _Complete.SetActive(true); // 완료 이미지 활성화
            if (!CafeSystem._Instance._UI._open.showed)
            {
                CafeSystem._Instance._UI._open.showed = true;
                StartCoroutine(WaitAndUpFinal((int)CafeSystem._Instance._UI._open._TargetSale, profit,review));
            }
        }
        else
            // 완료 이미지 비활성화
            _Complete.SetActive(false);
    }
    // 최종 결과 창표시
    IEnumerator WaitAndUpFinal(int sales,int profit, int review)
    {
        // 완료 이미지 출력 후 0.8초 이후 아래 코드 실행
        yield return new WaitForSeconds(0.8f);
        _Comment.SetActive(false); // 대화팝업 지우기
        _Complete.SetActive(true); // 완료이미지 활성화
        _Result.SetActive(true);   // 결과창 활성화
        _Result.GetComponent<UICafeResult>().SetInfo(sales, profit,review); // 판매량, 인당 수익, 리뷰수로 결과창 세팅
        CafeSystem._Instance._UI._open.showed = false; // 진행 UI는 숨기지
    }
    // 대화 팝업 생성 루틴 , 내용과 이름(등장인물), 스프라이트 주소입력으로 변경가능
    public void ShowComment(string text, string name = "", string spritePath = "") => StartCoroutine(ShowTask(text, name, spritePath));
    public IEnumerator ShowTask(string text,string name="",string spritePath = "")
    {
        // 처음에는 보여 준 후 2.5초 동안 점점 사라짐
        _Comment.SetActive(true);
        Image image = _Comment.GetComponent<Image>();
        float _Duration = 2.5f;
        // 텍스트 색깔 변경
        GameObject ComText = _Comment.transform.Find("Text").gameObject;
        ComText.GetComponent<Text>().text = text;

        Image commentImage = _Comment.transform.Find("ImageMask").transform.Find("Image").GetComponent<Image>();
        Image MaskImage = _Comment.transform.Find("ImageMask").GetComponent<Image>();
        if (spritePath != "")
        {
            
            UnityEngine.Sprite img = Resources.Load<UnityEngine.Sprite>(spritePath);
            commentImage.sprite = img;
        }
        Color color = Color.white;
        if (name == "예지") ColorUtility.TryParseHtmlString("#803080", out color);
        else if (name == "유리") ColorUtility.TryParseHtmlString("#FF9C9F", out color);
        else if (name == "한별") ColorUtility.TryParseHtmlString("#FFC599", out color);
        ComText.GetComponent<Text>().color = color;

        // Duration 시간이 지난 뒤 사라짐   
        float timer = 0f;
        while (timer < _Duration)
        {
            timer += Time.deltaTime;
            float alpha = 1f - timer / _Duration;
            Color Cocolor = image.color;
            Color Cocolor2 = commentImage.color;
            Color Cocolor3 = MaskImage.color;
            Cocolor.a = alpha;
            Cocolor2.a = alpha;
            Cocolor3.a = alpha;
            image.color = Cocolor;
            commentImage.color = Cocolor2;
            MaskImage.color = Cocolor3;
            if (alpha < 0.1f) break;
            yield return null;
        }
        showed = false;
        _Comment.SetActive(false);
        // 대화 팝업이 사라지면 안 보이도록
    }
}
