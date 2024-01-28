using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Game
{
    public class CafeSystem : MonoBehaviour
    {
        [SerializeField]
        public GameObject _UIBack = null;
        public static CafeSystem _Instance { get; private set; }
        public CafeUIManager _UI { get; private set; }
        public Background _Background { get; private set; }
        public Foreground _Foreground { get; private set; }
        public ScenarioManager _ScenarioManager { get { return _scenarioManager; } }
        private ScenarioManager _scenarioManager  = new ScenarioManager();
        private SpriteManager _spriteManager = new SpriteManager();
        public SpriteManager _SpriteManager { get { return _spriteManager; } }
        public bool _DoingTask { get; set; }
        public bool _IsClicked { get; private set; }
        private delegate void UpdateFunc();
        private UpdateFunc _updateHandle = null;
        private static int _ReferenceResolutionWidth { get { return 1920; } }
        private static int _ReferenceResolutionHeight { get { return 1080; } }


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
            _ScenarioManager.Initialize();
            _updateHandle = UpdateEmpty; //차후 ui말고 다른 터치가 생길경우용
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
            _UI = FindObjectOfType<CafeUIManager>();
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

        private IEnumerator Loading()
        {
            yield return null;

            // 로딩 종료
            _updateHandle = UpdateGame;
        }


        private void UpdateEmpty() { }

        private void UpdateGame()
        {
            UpdateInput();
        }

        private void UpdateInput() => _IsClicked = _UI._Input.PopIsClicked();

        public void RegisterTask(IEnumerator task) => StartCoroutine(DoTask(task));

        private IEnumerator DoTask(IEnumerator task)
        {
            _DoingTask = true;
            yield return StartCoroutine(task);
            _DoingTask = false;
        }
        private string GetStartScenarioName()
        {
            if (!string.IsNullOrEmpty(App.AppSystem._GameSystemParam._ScenarioName))
                return App.AppSystem._GameSystemParam._ScenarioName;
            else
                return "Defalut";
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
    }
}