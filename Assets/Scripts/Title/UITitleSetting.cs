using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UITitleSetting : UIWindow
{
    [SerializeField]
    public Slider _bgmSlieder;
    [SerializeField]
    public Slider _soundSlider;
    [SerializeField]
    public Slider _textSpeedSlider;
    [SerializeField]
    public Button _fullScreen;
    [SerializeField]
    public Button _windowScreen;
    [SerializeField]
    public Button _manualBtn;
    [SerializeField]
    public Button _autoBtn;

    public static UITitleSetting _Instance { get; private set; }
    private string _autoMode = "Manual";

    private void Awake()
    {
        _Instance = this;
       
    }
   
    private void Start()
    {
        //배경음, 효과음 , 텍스트 속도는 슬라이더의 value로 제어를함
        _fullScreen.onClick.AddListener(SetFullScreen);
        _windowScreen.onClick.AddListener(SetWindowScreen);
        //게임이 시작하고 나면 사용자가 설정했던 환경설정에 따라서 슬라이더 value, 스프라이트 이미지들이 세팅이 되어있음
        _bgmSlieder.value = float.Parse(PlayerPrefs.GetString("bgmVolume"));
        _soundSlider.value = float.Parse(PlayerPrefs.GetString("soundVolume"));
        _textSpeedSlider.value = float.Parse(PlayerPrefs.GetString("textSpeed"));
        DialogueModeSpr(PlayerPrefs.GetString("automode"));
        ScreenModeSpr(PlayerPrefs.GetString("screenMode"));

    }

    public float SliderSoundVolume()
    {
        return _soundSlider.value;
    }


    public void SetSoundVolume()
    {   //슬라이더의 value에 따라서 효과음 조절
        PlayerPrefs.SetString("soundVolume", _soundSlider.value.ToString());
        SoundManager._Instance.SetSoundVolume(_soundSlider.value);

    }

    public void SetBgmVolume()
    {
        //배경음 조절
        PlayerPrefs.SetString("bgmVolume", _bgmSlieder.value.ToString());
        SoundManager._Instance.SetBgmVolume(_bgmSlieder.value);
    }


    public void SetTextSpeed()
    {   //텍스트 속도 조절
        PlayerPrefs.SetString("textSpeed", _textSpeedSlider.value.ToString());
    }


    public void SetFullScreen()
    {   //전체모드
        ScreenModeSpr("FullScreen");
        PlayerPrefs.SetString("screenMode", "FullScreen");
        Screen.fullScreen = true;
        Screen.SetResolution(1920, 1080, true);
    }

    public void SetWindowScreen()
    {
        //창모드
        ScreenModeSpr("Window");
        PlayerPrefs.SetString("screenMode", "Window");
        Screen.SetResolution(1280, 720, false);
    }

    public void AutoMode()
    {
        //텍스트 출력 자동
        _autoMode = "Auto";
        DialogueModeSpr("Auto");
        PlayerPrefs.SetString("automode", _autoMode);
    }
    public void ManualMode()
    {   
        //수동
        _autoMode = "Manual";
        DialogueModeSpr("Manual");
        PlayerPrefs.SetString("automode", _autoMode);
    }

    public void DialogueModeSpr(string mode)
    {
        //대화출력 모드 버튼 이미지 세팅
        if (mode == "Manual")
        {   
            _autoBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>(Define._settingSpriteRoot + "/enableDialogue");
            _manualBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>(Define._settingSpriteRoot + "/ableDialogue");
        }
        else if (mode == "Auto")
        {
            _autoBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>(Define._settingSpriteRoot + "/ableDialogue");
            _manualBtn.GetComponent<Image>().sprite = Resources.Load<Sprite>(Define._settingSpriteRoot + "/enableDialogue");
        }
    }

    public void ScreenModeSpr(string mode)
    {
        //창모드 버튼 이미지 세팅
        if (mode == "FullScreen")
        {
            _windowScreen.GetComponent<Image>().sprite = Resources.Load<Sprite>(Define._settingSpriteRoot + "/enableScreen");
            _fullScreen.GetComponent<Image>().sprite = Resources.Load<Sprite>(Define._settingSpriteRoot + "/ableScreen");
        }
        else if (mode == "Window")
        {
            _windowScreen.GetComponent<Image>().sprite = Resources.Load<Sprite>(Define._settingSpriteRoot + "/ableScreen");
            _fullScreen.GetComponent<Image>().sprite = Resources.Load<Sprite>(Define._settingSpriteRoot + "/enableScreen");
        }
    }
}
