using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Game;
using UnityEngine.SceneManagement;
using App;

public class UIPopup : UIWindow
{
    public bool GoTitle=false;
    [SerializeField]
    public GameObject _UIBack = null;

    public void clickedYes()
    {
        if (GoTitle) { GameSystem._Instance.LoadTitleScene(); }
        int saveIndex;
        string saveSlot;
        saveSlot = GameSystem._Instance._UI._saveMenu.curSaveSlot;//누른 슬롯 이름
        saveIndex = GameSystem._Instance._ScenarioManager.rtCommnadIdx();//현재 진행중인 시나리오 인덱스
        
        if (GameSystem._Instance._UI._saveMenu.IsActivate())//**savemenu가 활성화 되어있을때
        {
            GameObject gs = GameSystem._Instance._UI._saveMenu.transform.Find(saveSlot).gameObject;
            gs.transform.Find("Date").gameObject.SetActive(true);
            // 시나리오 저장
            string []sceInfo = PlayerPrefs.GetString("1").Split("|");
            if (sceInfo[0] =="")
            {
                System.Array.Resize(ref sceInfo, 4);
            }
            string saveDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //**각 슬롯마다 현재 슬롯 idx, 저장할때 현재 게임 시나리오 idx, 저장한 날짜 정보를 담아 놓음. 구분자 |를 사용함
            sceInfo[0] = saveSlot;
            //저장할때도 선택지 있을때 일정 수를 빼줘야함
            sceInfo[1] = (saveIndex - GameSystem._Instance._ScenarioManager.RtSeclectCount(saveIndex)).ToString();
            sceInfo[2] = saveDate;
            sceInfo[3] = AppSystem._GameSystemParam._ScenarioName;//현재 실행되고 있는 시나리오
            string newSce = string.Join("|", sceInfo);
            PlayerPrefs.SetString(saveSlot, newSce);
            
            gs.transform.Find("Date").GetComponent<Text>().text = saveDate;//**저장하기 메뉴에 저장한 날짜 set
            GameSystem._Instance._UI._loadMenu.setDate(saveSlot, saveDate);//**불러오기 메뉴에 저장한 날짜 set

            // 미니게임정보 : 슬롯넘버_cafe로 접속
            string[] parseInfo = PlayerPrefs.GetString("1"+ "_cafe").Split("|");
            // 미니게임 정보가 없으면 초기화값
            if (parseInfo[0] == "")
            {
                string[] newInfo = new string[10];
                newInfo[0] = "1"; // 주차
                newInfo[1] = "0"; // 소지금
                newInfo[2] = "[0,0,0,0,0,0]"; // 소지 가구 리스트 0: 미소지, 1: 소지
                newInfo[3] = "0";// 가구 보너스(수익)
                newInfo[4] = "0";// 누적 리뷰
                newInfo[5] = "0";// 가구 보너스(손님)
                newInfo[6] = "0";// 카페 오픈 여부
                newInfo[7] = "Game"; // 저장위치
                newInfo[8] = "0";// 누적손님
                newInfo[9] = "[1,0,0,0]"; //서브시나리오리스트
                string newString = string.Join("|", newInfo);
                PlayerPrefs.SetString(saveSlot+"_cafe", newString);
            }
            else parseInfo[7] = "Game";
            string parseString = string.Join("|", parseInfo);
            PlayerPrefs.SetString(saveSlot + "_cafe", parseString);
           
            GameSystem._Instance._UI._UIBack.SetActive(true);
            GameSystem._Instance._UI._Dialogue.SetActivate(false);
            GameSystem._Instance._UI._Menu.SetActivate(false);
            GameSystem._Instance._UI._menuPanel.SetActivate(true);
        }
        else if(GameSystem._Instance._UI._loadMenu.IsActivate())//**loadmenu가 활성화 되어있을때 
        {
            GameSystem._Instance._UI._menuPanel.GetComponent<UIPage>().OnPrevClik();
            GameSystem._Instance._UI._menuPanel.GetComponent<UIPage>().OnPrevClik();
            GameSystem._Instance._UI._menuPanel.GetComponent<UIPage>().OnPrevClik();
            GameSystem._Instance._UI._menuPanel.GetComponent<UIPage>().OnPrevClik();
            string loadSlotKey = GameSystem._Instance._UI._loadMenu.curLoadSlot;
            string[] parseInfo = PlayerPrefs.GetString(loadSlotKey).Split("|");//**슬롯의 데이터들 구분자에 따라서 파싱
            // 회상모드 해제
            PlayerPrefs.SetString("ONReview", "False");
            if (parseInfo[1] == PlayerPrefs.GetString("1").Split("|")[1])
            {
                
                GameSystem._Instance._ScenarioManager.UpdateLoadGame(int.Parse(parseInfo[1]), loadSlotKey);//**시나리오 idx대로 게임 불러오기
                GameSystem._Instance._UI._Menu.SetActivate(true);
            }
            else
            {
                LoadDoOtherGameSequence(loadSlotKey);
            }
            GameSystem._Instance._UI._menuPanel.SetActivate(false);//**다불러왔으면 메뉴 닫기
            GameSystem._Instance._UI._Menu.SetActivate(true);
            GameSystem._Instance._UI._Dialogue.SetActivate(true);
            GameSystem._Instance._UI._UIBack.SetActive(false);
            GameSystem._Instance._UI._Input.SetActivate(true);
            // 미니게임 정보 불러오기
            string cafeInfo = PlayerPrefs.GetString(loadSlotKey + "_cafe");
            PlayerPrefs.SetString("1" + "_cafe", cafeInfo);
        }
        GameSystem._Instance._UI._popUp.SetActivate(false);//**팝업창 닫기
        
        
        
    }
    private void clickedNo()
    {
        if (GameSystem._Instance._UI._saveMenu.IsActivate() || GameSystem._Instance._UI._loadMenu.IsActivate())
        {
            GameSystem._Instance._UI._popUp.SetActivate(false);
            GameSystem._Instance._UI._menuPanel.SetActivate(true);
            GameSystem._Instance._UI._Menu.SetActivate(false);
            GameSystem._Instance._UI._Dialogue.SetActivate(false);
            GameSystem._Instance._UI._UIBack.SetActive(true);
            if (GameSystem._Instance._UI.tempMode == "SAVE")
                GameSystem._Instance._UI._saveMenu.SetActivate(true);
            else if (GameSystem._Instance._UI.tempMode == "LOAD")
                GameSystem._Instance._UI._loadMenu.SetActivate(true);
            return;
        }
        else if (GameSystem._Instance._UI.tempMode == "Title")
            gameObject.SetActive(false);
            
    }
    public void SetText(string Main="", string Sub="")
    {
        Transform main = gameObject.transform.Find("Main").transform;
        Transform sub = gameObject.transform.Find("Sub").transform;
        Text mainText = main.GetComponent<Text>();
        Text subText = sub.GetComponent<Text>();
        mainText.text = Main;
        subText.text = Sub;
    }
    public void LoadDoOtherGameSequence(string slot) => StartCoroutine(LoadOtherGameSequence(slot));
    private IEnumerator LoadOtherGameSequence(string slot)//시나리와 시나리오 사이에 이동을 위함
    {
        string saveScene = PlayerPrefs.GetString(slot + "_cafe").Split("|")[7]; //저장 된 씬에 따라 씬 호출
        if (saveScene == "Game")
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(Define.SceneName._game);

            operation.completed += (AsyncOperation op) =>
            {

                string[] sceninfo = PlayerPrefs.GetString(slot).Split("|");

                GameSystem._Instance._ScenarioManager.Load(sceninfo[3]); //시나리오 이름
                //선택지 나오는 부분에서 불러올 때 load할때 spritem의 갯수와 select selectend에 따라서 불러올 idx를 빼줘야함
                GameSystem._Instance._ScenarioManager.UpdateLoadGame(int.Parse(sceninfo[1])-GameSystem._Instance._ScenarioManager.loadSelectIdx,slot); // 시나리오 인덱스
                GameSystem._Instance._UI._Menu.SetActivate(true);
                GameSystem._Instance._UI._Menu.transform.Find("Container").gameObject.SetActive(false);   
                // 게임 정보 적용 추가
                string cafeInfo = PlayerPrefs.GetString(slot + "_cafe");
                PlayerPrefs.SetString("1" + "_cafe", cafeInfo);
            };
        }
        else if (saveScene == "Cafe")
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(Define.SceneName._cafe);

            operation.completed += (AsyncOperation op) =>//**로딩이 끝나고 카페 시스템에있는 함수를 가져와야하므로
            {
                CafeSystem._Instance._UI.LoadCafeInfo(slot);
                CafeSystem._Instance._UI._Menu.SetActivate(true);
            };
        }
        yield return new WaitForSeconds(1.5f);// 대기
    }
}
