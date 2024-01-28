using App;
using Game;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UICafePopup : UIWindow
{
    public string Mode = "";
    public int _Coast = 99999999;
    public int _Poket = 0;
    public int _epiCode = -1;
    public string _Slot = "-1";
    public string _ScenarioName = "";
    public string _LabelName = "";
    public GameObject _Target = null;
    public void OnYesClicked()
    {
        // 모드별 처리
        // 가구 구매
        if (Mode == "Buy" && _Target != null)
        {
            // 구매 정보 저장
            // 가구 구입 여부(0,1)로 색 바뀜
            string[] CafeInfo = PlayerPrefs.GetString("1" + "_cafe").Split("|");
            string decInfo = CafeInfo[2];
            decInfo = decInfo.Replace("[", "").Replace("]", "");
            string[] decorInfo = decInfo.Split(",");
            for (int i = 0; i < _Target.transform.parent.childCount; i++)
            {
                Transform childObject = _Target.transform.parent.GetChild(i);
                if (childObject.gameObject == _Target)
                {
                    decorInfo[i] = "1";
                    break;
                }
            }
            string newDecor = string.Join(",", decorInfo);
            CafeInfo[2] = newDecor; // 새 정보를 저장

            // 소지금 감소
            int remain = _Poket - _Coast;
            CafeInfo[1] = remain.ToString();

            // 보너스 증가
            string decorBonus = _Target.transform.Find("Effect").GetComponentInChildren<Text>().text;
            int index = decorBonus.IndexOf('%'); //효과가 "ssss xx% 증가"일 경우 "%의 위치를 찾음
            // 보너스량 확인(XX%)후 보너스 추가
            int bonus = int.Parse(decorBonus.Substring(index - 2, 2)); // % 이전의 두 글자 추출 후 int로 변환
            // 보너스 종류 확인(손님, 수익)
            if (decorBonus.Contains("수익"))
            {
                CafeInfo[3] = (bonus + int.Parse(CafeInfo[3])).ToString();// 최종 수익 증가
            }

            else if (decorBonus.Contains("손님"))
            {
                CafeInfo[5] = (bonus + int.Parse(CafeInfo[5])).ToString(); // 손님 증가,보너스 후 원래 보너스와 합하고량을 최종 수익 증가
            }

            string newInfo = string.Join("|", CafeInfo);
            // --- 데이터 저장
            PlayerPrefs.SetString("1" + "_cafe", newInfo);
            // 색 변경
            _Target.GetComponent<UIDecorItem>().SetWhite();
            gameObject.SetActive(false);
            // UI 갱신
            CafeSystem._Instance._UI._Cafe.UpdateInfo();
            CafeSystem._Instance._UI._decorStore.UpdateDecor();
            CafeSystem._Instance._UI._popUp.SetActivate(false);
            CafeSystem._Instance._UI._Menu.SetActivate(true);
        }
        // 메인 에피소드 불러오기 Main_00형식
        else if (Mode == "MainEpiBuy")
        {
            // 소지금 감소
            int remain = _Poket - _Coast;
            string[] Cafeinfo = PlayerPrefs.GetString("1" + "_cafe").Split("|");
            Cafeinfo[1] = remain.ToString();
            // 다음 메인 에피소드 열람 기록 변경
            Cafeinfo[0] = (int.Parse(Cafeinfo[0]) + 1).ToString();
            // 씬 회상 목록 업데이트

            string[] Sce = PlayerPrefs.GetString("SCENE").Split("|");
            for (int i = 0; Sce[i].Length > i; i++)
            {
                // 찾는 라벨의 열람기록을 시나리오파일 이름으로 (ex:Main_01:0 -> Main_01:Scenario001)
                if (Sce[i].StartsWith(_LabelName))
                {
                    Sce[i] = Sce[i].Split(":")[0] + ":" + _ScenarioName;
                }
            }
            string newRev = string.Join("|", Sce);
            PlayerPrefs.SetString("SCENE", newRev);
            // Main_XX부분에서 XX를 정수로 가져옴
            int idx = int.Parse(_LabelName.Substring(_LabelName.Length - 2));

            // 카페 오픈 가능
            Cafeinfo[6] = "0";
            // 에피소드 이름 저장
            string[] sceInfo = PlayerPrefs.GetString("1").Split("|");
            sceInfo[3] = _ScenarioName;
            string newSce = string.Join("|", sceInfo);
            PlayerPrefs.SetString("1", newSce);
            // 카페데이터 저장
            string newInfo = string.Join("|", Cafeinfo);
            PlayerPrefs.SetString("1" + "_cafe", newInfo);


            // 코드로 메인 시나리오 불러오기
            App.AppSystem._GameSystemParam.Set(_ScenarioName);
            LoadDoGameSequence();
            CafeSystem._Instance._UI._popUp.SetActivate(false);
        }
        // 서브에피소드 불러오기 현재 해당 사항 없음
        else if (Mode == "SubEpiBuy")
        {
            // 소지금 감소
            int remain = _Poket - _Coast;
            string[] Cafeinfo = PlayerPrefs.GetString("1" + "_cafe").Split("|");
            Cafeinfo[1] = remain.ToString();
            // 에피소드 이름저장

            string[] sceInfo = PlayerPrefs.GetString("1").Split("|");
            sceInfo[3] = _ScenarioName;
            string newSce = string.Join("|", sceInfo);
            PlayerPrefs.SetString("1", newSce);



            // 서브 에피소드 기록
            string subepi = Cafeinfo[9].Replace("[", "").Replace("]", "");
            string[] subArry = subepi.Split(",");
            int idx = int.Parse(_LabelName.Substring(_LabelName.Length - 2));
            if (subArry[idx] == "0") subArry[idx] = "1";
            subepi = string.Join(",", subArry);
            Cafeinfo[9] = "[" + subepi + "]";

            //저장
            string newInfo = string.Join("|", Cafeinfo);
            PlayerPrefs.SetString("1" + "_cafe", newInfo);

            // 시나리오 불러오기
            App.AppSystem._GameSystemParam.Set(_ScenarioName);
            LoadDoGameSequence();
            CafeSystem._Instance._UI._popUp.SetActivate(false);
        }
        // 영업 시작 
        else if (Mode == "OpenCafe")
        {
            CafeSystem._Instance._UI._open.SetActivate(true);
            CafeSystem._Instance._UI._open.OpenCafe();
            CafeSystem._Instance._UI._popUp.SetActivate(false);
            CafeSystem._Instance._UI._Menu.SetActivate(true);
        }
        // Save 시도
        else if (Mode == "Save")
        {
            // 데이터 저장
            // 데이터 불러오기
            string[] sceInfo = PlayerPrefs.GetString("1").Split("|");
            string saveDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            // 시나리오 정보 저장
            if (sceInfo[0] == "")
            {
                Array.Resize(ref sceInfo, 4);
            }

            sceInfo[0] = _Slot;
            //1번은 이미 있는 인덱사용
            sceInfo[2] = saveDate;
            // 없으면 초기화, 있으면 그대로
            sceInfo[3] = AppSystem._GameSystemParam._ScenarioName;
            string newSce = string.Join("|", sceInfo);
            PlayerPrefs.SetString(_Slot, newSce);

            // 미니게임 정보 저장
            string[] parseInfo = PlayerPrefs.GetString("1" + "_cafe").Split("|");
            if (CafeSystem._Instance._UI._Cafe._hasOpened) parseInfo[6] = "1";
            else parseInfo[6] = "0";
            parseInfo[7] = "Cafe";
            // 데이터 저장
            string newInfo = string.Join("|", parseInfo);
            PlayerPrefs.SetString(_Slot + "_cafe", newInfo);
            CafeSystem._Instance._UI._menuPanel.GetComponent<UIPage>().ApplyPage();
            CafeSystem._Instance._UI._popUp.SetActivate(false);
        }
        // 로드
        else if (Mode == "Load")
        {
            string saveScene = PlayerPrefs.GetString(_Slot + "_cafe").Split("|")[7]; //저장 된 씬에 따라 씬 호출
            // 슬롯이 게임에서 저장된 경우
            if (saveScene == "Game")
            {
                // 게임씬 로드
                AsyncOperation operation = SceneManager.LoadSceneAsync(Define.SceneName._game);
                operation.completed += (AsyncOperation op) =>
                {
                    GameSystem._Instance._UI._Menu.SetActivate(true);
                    string[] parseinfo = PlayerPrefs.GetString(_Slot).Split("|");
                    GameSystem._Instance._ScenarioManager.Load(parseinfo[3]);
                    // 해당 인덱스로 로딩<- cafe 다음으로 넘어감 //GameSystem._Instance._ScenarioManager.UpdateLoadGame(int.Parse(parseinfo[1]));
                    GameSystem._Instance._ScenarioManager.UpdateLoadGame(int.Parse(parseinfo[1]), _Slot);
                    // 게임 정보 적용 추가
                    string cafeInfo = PlayerPrefs.GetString(_Slot + "_cafe");
                    PlayerPrefs.SetString("1" + "_cafe", cafeInfo);
                };
            }
            else if (saveScene == "Cafe")
            {
                AsyncOperation operation = SceneManager.LoadSceneAsync(Define.SceneName._cafe);
                operation.completed += (AsyncOperation op) =>
                {
                    CafeSystem._Instance._UI.LoadCafeInfo(_Slot);
                };
            }
            CafeSystem._Instance._UI._popUp.SetActivate(false);
        }
        else if (Mode == "Title")
        {
            CafeSystem._Instance.LoadTitleScene();
            CafeSystem._Instance._UI._Menu.SetActivate(true);
        }
    }
    public void OnNoClicked()
    {
        gameObject.SetActive(false);

    }
    // 메인과 서브로 텍스트 설정
    public void SetText(string Main = "", string Sub = "")
    {
        Transform main = gameObject.transform.Find("Main").transform;
        Transform sub = gameObject.transform.Find("Sub").transform;
        Text mainText = main.GetComponent<Text>();
        Text subText = sub.GetComponent<Text>();
        mainText.text = Main;
        subText.text = Sub;
    }
    public void LoadDoGameSequence() => StartCoroutine(LoadGameSequence());
    private IEnumerator LoadGameSequence(string slot = "1")
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(Define.SceneName._game);

        operation.completed += (AsyncOperation op) =>//로드 후에 다른 시나리오 함수 쓸것
        {
            // 해당시나리오 로딩
            GameSystem._Instance._ScenarioManager.Load(_ScenarioName);
            // 해당 라벨로 이동
            if (!string.IsNullOrEmpty(_LabelName))
                GameSystem._Instance._ScenarioManager.JumpToLabel(_LabelName);
            // 이동후 인덱스 적용
            GameSystem._Instance._ScenarioManager.UpdateCommand();
            GameSystem._Instance._UI._Menu.SetActivate(true);
            int idx = GameSystem._Instance._ScenarioManager.rtCommnadIdx();
            GameSystem._Instance._ScenarioManager.UpdateLoadGame(idx);
            // 게임 정보 적용 추가
        };
        yield return new WaitForSeconds(1.5f);// 대기
    }
}
