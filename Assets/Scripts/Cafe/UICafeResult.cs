using Game;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class UICafeResult :UIWindow
{
    [SerializeField]
    public Text _FinProfit = null;
    [SerializeField]
    public Text _MidProfit = null;
    [SerializeField]
    public Text _Sales = null;
    [SerializeField]
    public Text _DecorBonus = null;
    [SerializeField]
    public Text _BonusPro = null;
    [SerializeField]
    public Text _visitor = null;
    [SerializeField]
    public Text _TotalReview = null;
    [SerializeField]
    public Text _TotalVisitor = null;
    [SerializeField]
    public Text _NewReview = null;
    [SerializeField]
    public Text _Rate1 = null;
    [SerializeField]
    public Text _Rate2 = null;
    [SerializeField]
    public GameObject _ReviewBox = null;
    public int _Result = 0;
    public void OnCloseCafeClicked()
    {
        // 카페 영업 종료
        // 정보갱신
        CafeSystem._Instance._UI._Cafe.UpdateInfo();
        // UI 정리
        CafeSystem._Instance._UI._Cafe._DecorButton.SetActive(true);
        CafeSystem._Instance._UI._Cafe._OpenButton.SetActive(true);
        CafeSystem._Instance._UI._Cafe._EpisodeButton.SetActive(true);
        CafeSystem._Instance._UI._Cafe._LastEpisodeInfo.SetActive(true);
        CafeSystem._Instance._UI._open._Comment.SetActive(false);
        CafeSystem._Instance._UI._open._Complete.SetActive(false);
        CafeSystem._Instance._UI._open._Result.SetActive(false);
        CafeSystem._Instance._UI._open.SetActivate(false);
    }
    public void SetInfo(int Sales, int Profit,int review)
    {
        // 게임정보 불러오기
        string[] cafeinfo = PlayerPrefs.GetString("1" + "_cafe").Split("|");
        // 결과창 생성
        _MidProfit.text = Profit.ToString("C0").Replace("$", "￦ "); // 중간 총매출
        _Sales.text = Sales.ToString("C0").Replace("$", "￦ ");  // 1인당 소비액
        _TotalReview.text = (int.Parse(cafeinfo[4]) + review).ToString() +"명";//누적리뷰수
        _NewReview.text = review.ToString() + "개";// 신규 리뷰수 
        // 가구 - (수익율)
        int decorBonus = CafeSystem._Instance._UI._Cafe._decorBonus;
        if (decorBonus == -1) { decorBonus = 0; }
        _DecorBonus.text =  decorBonus.ToString() + "%"; // 수익율
        // 이번주 손님
        string visitor = (CafeSystem._Instance._UI._open._TargetVisitor).ToString();
        _visitor.text = visitor.ToString() + "명";  // 손님
        // 누적손님
        _TotalVisitor.text = (int.Parse(visitor) + int.Parse(cafeinfo[8])).ToString();
        int res = Convert.ToInt32(Profit * ((100 + decorBonus) / 100.0f)); // 최종수익
        _BonusPro.text = (res-Profit).ToString("C0").Replace("$", "￦ "); //순이익
        // 이번주 평점 설정
        System.Random random = new System.Random();
        // 랜덤범위에서 1.0~5.0사이의 실수
        double [] ReveiwScore = { random.NextDouble() * 4.0+1, random.NextDouble() * 4.0 + 1, random.NextDouble() * 4.0 + 1, random.NextDouble() * 4.0 + 1, };
        double sum = 0;
        for(int i=0;i<review;i++) { sum += ReveiwScore[i]; }
        // 평균 점수 반영
            Double rate = Math.Round((Double)sum / review, 1);
            _Rate1.text = rate.ToString("0.#")+"점";
            _Rate2.text = rate.ToString("0.#")+"점";
        // 댓글 구현 테이블 - 랜덤으로 무작위 선정
        string[] badCommnet = { "가격이 비싸요", "직원이 불친절해요","주문이 오래걸려요" };
        string[] sosoCommnet = { "가격은 적당해요", "집 근처에 있어서 가요","맛은 평범해요" };
        string[] goodCommnet = { "가격은 너무 착해요", "직원들이 친절해요","맛은 최고에요" };
        string[] badName = { "엄격진지 근엄", "밥상경찰", "까다로운 까마귀" };
        string[] sosoName = { "푸근한 아저씨", "집근처 집순이", "지나가는 회사원" };
        string[] goodName = { "맛집사냥꾼", "쪼앙TV", "맛집탐방대" };
        for (int i =0;i<4;i++) 
        {
            // ReviewBox의 자식중에 Review0~3까지에 대해
            GameObject rev = _ReviewBox.transform.Find("Review" + i.ToString()).gameObject;
            // i가 갯수보다 크면 해당 자식 숨김
            if (i >= review) { rev.SetActive(false); continue; }
            else rev.SetActive(true);
            rev.transform.Find("Score").GetComponent<Text>().text = Math.Round(ReveiwScore[i], 1).ToString("0.#");
            // 평점에 따른 평점,이름,댓글내용 설정
            if (ReveiwScore[i] <=2.5)
            {
                rev.transform.Find("Comment").GetComponent<Text>().text = badCommnet[random.Next(0, 3)].ToString();
                rev.transform.Find("Name").GetComponent<Text>().text = badName[random.Next(0, 3)].ToString();
                //rev.transform.Find("ImageMask").transform.Find("Image").GetComponent<Image>().sprite = "";
            }
            else if (ReveiwScore[i] <= 3.6)
            {
                rev.transform.Find("Comment").GetComponent<Text>().text = sosoCommnet[random.Next(0, 3)].ToString();
                rev.transform.Find("Name").GetComponent<Text>().text = sosoName[random.Next(0, 3)].ToString();
            }
            else if (ReveiwScore[i] >3.6)
            {
                rev.transform.Find("Comment").GetComponent<Text>().text = goodCommnet[random.Next(0, 3)].ToString();
                rev.transform.Find("Name").GetComponent<Text>().text = goodName[random.Next(0, 3)].ToString();
            }
        }

        // 소지금 증가
        int Poket = int.Parse(cafeinfo[1]) + res;
        cafeinfo[1] = Poket.ToString();
        // 누적 리뷰수 증가
        cafeinfo[4] = (int.Parse(cafeinfo[4]) +review).ToString();
        // 영업여부 저장
        cafeinfo[6] = "1";
        // 누적 손님 저장
        cafeinfo[8] = ((int.Parse(visitor) + int.Parse(cafeinfo[8]))).ToString();
        // 데이터 저장
        string newInfo = string.Join("|", cafeinfo);
        PlayerPrefs.SetString("1" + "_cafe", newInfo); // 통합후 1번 슬롯에 저장
        CafeSystem._Instance._UI._Cafe.UpdateInfo();   // 1번슬롯으로 ui최신화
        _FinProfit.text = res.ToString("C0").Replace("$", "￦ "); // 최종 수익 반환(￦888,888,888)
    }
}
