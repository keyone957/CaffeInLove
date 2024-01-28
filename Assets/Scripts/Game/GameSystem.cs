using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

namespace Game
{
    public class GameSystem : MonoBehaviour
    {
        public static GameSystem _Instance { get; private set; }
        public ScenarioInfo _ScenarioInfo { get; private set; }
        private ScenarioManager _scenarioManager = new ScenarioManager();
        public ScenarioManager _ScenarioManager { get { return _scenarioManager; } }
        public UIManager _UI { get; private set; }
        public Background _Background { get; private set; }
        public Foreground _Foreground { get; private set; }
        private SpriteManager _spriteManager = new SpriteManager();
        public SpriteManager _SpriteManager { get { return _spriteManager; } }
        public bool _DoingTask { get; set; }
        public bool _IsClicked { get; private set; }
        private delegate void UpdateFunc();
        private UpdateFunc _updateHandle = null;
        private static int _ReferenceResolutionWidth { get { return 1920; } }
        private static int _ReferenceResolutionHeight { get { return 1080; } }
        private UIItemSprite _itemSprite = new UIItemSprite();
        public UIItemSprite _ItemSprite { get { return _itemSprite; } }


        /// <summary>
        /// 기준 해상도 비율. 너비/높이
        /// </summary>
        private static float _ReferenceResolutionRatio { get { return (float)_ReferenceResolutionWidth / _ReferenceResolutionHeight; } }
        public static float _ReferenceWorldHeight { get { return 2.0f; } }
        private static float _ReferenceWorldWidth { get { return _ReferenceWorldHeight * _ReferenceResolutionRatio; } }

        private void Initalize()
        {
            _Instance = this;
            App.AppSystem.TryInitializeApplication();
            SoundManager.Create();
            InitializeCamera();
            InitializeUIManager();
            InitializeBackground();
            InitializeForeground();
            _ScenarioManager.Initialize();
            // 미니게임 정보초기화
            InitializeCafe();
            _updateHandle = UpdateEmpty;

            StartCoroutine(Loading());
        }

        private void Clear()
        {
            if (SoundManager._Instance != null)
            {
                SoundManager._Instance.StopBGM();
                SoundManager._Instance.ClearLoadedAudioClip();
            }
            _Instance = null;
        }

        private void Awake() => Initalize();

        private void OnDestroy() => Clear();

        private void Update()
        {
            _updateHandle();
            _UI.OnKeyInput();
        }

        private void InitializeCamera()
        {
            if ((Camera.main.aspect < _ReferenceResolutionRatio))
            {
                // 카메라 가로 비율이 기준보다 작다면
                // 예: 아이패드 프로 = 4:3, 기준 = 16:9
                // 카메라 가로가 기준 가로를 커버할 수 있도록
                Camera.main.orthographicSize = _ReferenceWorldWidth / 2.0f / Camera.main.aspect;
            }
        }

        private void InitializeUIManager()
        {
            _UI = FindObjectOfType<UIManager>();
            _UI.Initialize();
        }

        private void InitializeBackground()
        {
            Background prefab = Resources.Load<Background>(Define._backgroundPrefabPath);
            _Background = Instantiate<Background>(prefab);
            _Background.Initalize();
            _Background.SetColor(Color.black);
            _Background.name = prefab.name;
        }

        private void InitializeForeground()
        {
            Foreground prefab = Resources.Load<Foreground>(Define._foregroundPrefabPath);
            _Foreground = Instantiate<Foreground>(prefab);
            _Foreground.Initialize();
            _Foreground.SetActivate(false);
            _Foreground.name = prefab.name;
        }

        private IEnumerator Loading()
        {
            LoadScenarioInfo();
            _ScenarioManager.Load(GetStartScenarioName());
            yield return null;

            // 로딩 종료
            _updateHandle = UpdateGame;
        }

        private string GetStartScenarioName()
        {
            if (!string.IsNullOrEmpty(App.AppSystem._GameSystemParam._ScenarioName))
                return App.AppSystem._GameSystemParam._ScenarioName;
            else
                return _ScenarioInfo._defaultMainFileName;
        }

        private void LoadScenarioInfo()
        {
            TextAsset loadedText = Resources.Load<TextAsset>(Define._scenarioInfoPath);
            ScenarioInfoJson loadedJson = JsonUtility.FromJson<ScenarioInfoJson>(loadedText.text);
            _ScenarioInfo = ScenarioInfo.ConvertFromJson(loadedJson);
        }

        private void UpdateEmpty() { }

        private void UpdateGame()
        {
            UpdateInput();

            while (!_DoingTask && _ScenarioManager.HasRemainedCommand())
                _ScenarioManager.UpdateCommand();
        }

        private void UpdateInput() => _IsClicked = _UI._Input.PopIsClicked();

        public void RegisterTask(IEnumerator task) => StartCoroutine(DoTask(task));

        private IEnumerator DoTask(IEnumerator task)
        {
            _DoingTask = true;
            yield return StartCoroutine(task);
            _DoingTask = false;
        }

        public void Wait(float duration) => RegisterTask(WaitTask(duration));

        private IEnumerator WaitTask(float duration)
        {
            float startTime = Time.time;
            while (Time.time - (startTime + duration) < 0.0f)
                yield return null;
        }

        public void WaitClick() => RegisterTask(WaitClickTask());

        private IEnumerator WaitClickTask()
        {
            while (!_IsClicked)
                yield return null;
        }

        public void LoadTitleScene() => SceneManager.LoadScene(Define.SceneName._title);
        // ++ 카페 초기화, 로드 추가
        private void InitializeCafe()
        {
            string[] cafeInfo = PlayerPrefs.GetString("1" + "_cafe").Split("|");
            string[] scene = PlayerPrefs.GetString("SCENE").Split("|");
            string[] cg = PlayerPrefs.GetString("ECG").Split("|");
            string[] cgKey = { "pink_hydrangea_cg", "cat_rain", "snowdrop" }; // 들어가 있어야하는 키
            int cgComplite = 0; 
            for (int i= 0; i < cg.Length; i++)
            {
                for(int j=0; j < cgKey.Length; j++)
                {
                    if (cg[i].Split(":")[0] == cgKey[j]) cgComplite++;
                }
            }
            // 형식이 다르면cg개방여부 초기화
            if (cgComplite != cgKey.Length)
            {
                PlayerPrefs.SetString("ECG", "pink_hydrangea_cg:0|cat_rain:0|snowdrop:0|Cg_04:0|Cg_05:0|Cg_06:0|Cg_07:0|Cg_08:0|Cg_09:0|Cg_10:0" +
            "|Cg_11:0|Cg_12:0|Cg_13:0|Cg_14:0|Cg_15:0|Cg_16:0|Cg_17:0|Cg_18:0|Cg_19:0|Cg_20:0");
            }
            
            if (cafeInfo[0] == "")
            {
                // 씬 개방 여부
                PlayerPrefs.SetString("SCENE", "Main_01:0|Main_02:0|Main_03:0|Main_04:0|Main_05:0|Main_06:0|Main_07:0|Main_08:0|Main_09:0|Main_10:0" +
            "|Main_11:0|Main_12:0|Main_13:0|Main_14:0|Main_15:0|Main_16:0|Main_17:0|Main_18:0|Main_19:0|Main_20:0" +
            "|Main_21:0|Main_22:0|Main_23:0|Main_24:0|Main_25:0|Main_26:0|Main_27:0|Main_28:0|Main_29:0|Main_30:0" +
            "|Main_31:0|Main_32:0|Main_33:0|Main_34:0|Main_35:0|Main_36:0");
                
            }
            // 타이틀에서 호츨 된 경우거나 데이터가 없으면 초기화
            if (cafeInfo[7] == "Title" | cafeInfo[0] == "")
            {
                string[] sceInfo = new string[4];
                string saveDate =DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                
                sceInfo[0] = "1";
                sceInfo[1] = GameSystem._Instance._ScenarioManager.rtCommnadIdx().ToString();
                sceInfo[2] = saveDate;
                // 없으면 초기화, 있으면 그대로
                sceInfo[3] = "Scenario001";
                string newSce = string.Join("|", sceInfo);
                PlayerPrefs.SetString("1", newSce);
                string[] newInfo = new string[10];
                newInfo[0] = "1"; // 주차
                newInfo[1] = "0"; // 소지금
                newInfo[2] = "[0,0,0,0,0,0]"; // 소지 가구 리스트 0: 미소지, 1: 소지
                newInfo[3] = "0";// 가구 보너스(수익)
                newInfo[4] = "0";// 누적 리뷰수
                newInfo[5] = "0";// 가구 보너스(손님)
                newInfo[6] = "0";// 카페 오픈 여부
                newInfo[7] = "Game";// 세이브한 씬
                newInfo[8] = "0"; //누적 방문자
                newInfo[9] = "[1,0,0,0]"; //서브시나리오리스트
                string newString = string.Join("|", newInfo);
                PlayerPrefs.SetString("1" + "_cafe", newString);
            }
            
        }
        public void LoadDoCafeSequence(string slot ="1") => StartCoroutine(LoadCafeSequence(slot));
        private IEnumerator LoadCafeSequence(string slot ="1")
        {   //게임씬에서 카페게임 씬으로 넘어갈때 로딩이 끝나고 카페 씬을 초기화 해야하므로 
            AsyncOperation operation = SceneManager.LoadSceneAsync(Define.SceneName._cafe);
            operation.completed += (AsyncOperation op) =>
            {
                // 자동저장 슬롯으로 바꿀것
                CafeSystem._Instance._UI.LoadCafeInfo(slot); 
            };
            yield return new WaitForSeconds(1.5f);// 대기
        }
    }
}