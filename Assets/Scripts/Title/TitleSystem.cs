using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Game;

public class TitleSystem : MonoBehaviour
{
    public static TitleSystem _Instance { get; private set; }
    public TitleUIManager _UIManager { get; private set; }
    private SpriteManager _spriteManager = new SpriteManager();
    public SpriteManager _SpriteManager { get { return _spriteManager; } }
    private const float _fadeDuration = 1.5f;

    private void Initialize()
    {
       _Instance = this;
        App.AppSystem.TryInitializeApplication();
        InitializeGameSetting();
        SoundManager.Create();
        InitializeUIManager();
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

    private void Awake()
    {
        Initialize();
        StartCoroutine(SceneStartSequence());
    }
    private void InitializeGameSetting()
    {   //환경설정 기초 세팅들
        if (PlayerPrefs.GetString("automode") == "")
        {
            PlayerPrefs.SetString("automode", "Manual");//수동모드
           
        }
        if (PlayerPrefs.GetString("screenMode") == "")
        {
            PlayerPrefs.SetString("screenMode", "FullScreen");
            
        }
        if (PlayerPrefs.GetString("soundVolume") == "")
        {
            PlayerPrefs.SetString("soundVolume", "1.0");
        }
        if (PlayerPrefs.GetString("bgmVolume") == "")
        {
            PlayerPrefs.SetString("bgmVolume", "1.0");
         
        }
        if (PlayerPrefs.GetString("textSpeed") == "")
        {
            PlayerPrefs.SetString("textSpeed", "1.0");
           
        }

    }

    private void OnDestroy() => Clear();

    private void Update() => _UIManager.OnKeyInput();

    private void InitializeUIManager()
    {
        _UIManager = FindObjectOfType<TitleUIManager>();
        _UIManager.Initialize();
    }

    /// <summary>
    /// 씬 시작 시퀸스
    /// </summary>
    private IEnumerator SceneStartSequence()
    {
        // 로딩
        SoundManager._Instance.LoadBGM(Define._titleBGM);
        SoundManager._Instance.LoadSound(Define._menuSelectSound);
        yield return new WaitForSeconds(0.3f);

        _UIManager._FadeOverlay.DoFadeIn(_fadeDuration);
        yield return new WaitForSeconds(0.5f);
        SoundManager._Instance.PlayBGM(Define._titleBGM);
    }

    public void DoGameStartSequence() => StartCoroutine(GameStartSequence());

    private IEnumerator GameStartSequence()
    {
        _UIManager._FadeOverlay.DoFadeOut(_fadeDuration);
        yield return new WaitForSeconds(_fadeDuration);
        // 회상모드 해제
        PlayerPrefs.SetString("ONReview", "False");
        SceneManager.LoadScene(Define.SceneName._game);
    }
    public void LoadDoGameStartSequence() => StartCoroutine(LoadGameStartSequence());
    public void LoadDoGameStartSequence(string Scenario, string Label) => StartCoroutine(LoadGameStartSequence(Scenario, Label));
    private IEnumerator LoadGameStartSequence()
    {
        _UIManager._FadeOverlay.DoFadeOut(_fadeDuration);
        yield return new WaitForSeconds(_fadeDuration);//1.5초정도 페이드아웃
        string saveScene = PlayerPrefs.GetString(_UIManager._uiTitleLoadMenu.objname() + "_cafe").Split("|")[7]; //저장 된 씬에 따라 씬 호출
        if (saveScene == "Game")
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(Define.SceneName._game);

            operation.completed += (AsyncOperation op) =>//로딩이 끝나고 게임 시스템에 있는 updateloadgame을 가져와야하므로 추가
            {
                string[] sceninfo = PlayerPrefs.GetString(_UIManager._uiTitleLoadMenu.objname()).Split("|");
                GameSystem._Instance._ScenarioManager.Load(sceninfo[3]); //시나리오 이름

                GameSystem._Instance._ScenarioManager.UpdateLoadGame(int.Parse(sceninfo[1]),_UIManager._uiTitleLoadMenu.objname()); // 시나리오 인덱스
                GameSystem._Instance._UI._Menu.SetActivate(true);
                // 게임 정보 적용 추가
                string cafeInfo = PlayerPrefs.GetString(_UIManager._uiTitleLoadMenu.objname() + "_cafe");
                PlayerPrefs.SetString("1" + "_cafe", cafeInfo);
            };
        }
        else if (saveScene == "Cafe")
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(Define.SceneName._cafe);

            operation.completed += (AsyncOperation op) =>//로딩이 끝나고 카페시스템에 있는 loadcafeinfo를 가져와야하므로 추가
            {
                CafeSystem._Instance._UI.LoadCafeInfo(_UIManager._uiTitleLoadMenu.objname());
            };
        }
    }
    // 씬 회상 시퀀스
    private IEnumerator LoadGameStartSequence(string ScenarioName, string LabelName)
    {
        _UIManager._FadeOverlay.DoFadeOut(_fadeDuration);
        yield return new WaitForSeconds(_fadeDuration);
        AsyncOperation operation = SceneManager.LoadSceneAsync(Define.SceneName._game);
        operation.completed += (AsyncOperation op) =>
        {
            PlayerPrefs.SetString("ONReview", "True"); // 회상 진행중
            GameSystem._Instance._ScenarioManager.Load(ScenarioName); //시나리오 이름
            GameSystem._Instance._ScenarioManager.JumpToLabel(LabelName);
            int idx = GameSystem._Instance._ScenarioManager.rtCommnadIdx();
            GameSystem._Instance._ScenarioManager.UpdateLoadGame(idx+1); // 시나리오 인덱스
            GameSystem._Instance._UI._Menu.SetActivate(true);
        };
    }
    public void DoExit() => Application.Quit();
}
