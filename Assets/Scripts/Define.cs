public static class Define
{
    public static class SceneName
    {
        public const string _title = "Title";
        public const string _game = "Game";
        public const string _cafe = "Cafe";
    }

    public const string _systemRoot = "System";
    public const string _soundManagerPrefabPath = _systemRoot + "/SoundManager";
    public const string _bgmRoot = "BGM";
    public const string _soundRoot = "Sound";
    public const string _scenarioInfoFileName = "ScenarioInfo";
    public const string _scenarioInfoPath = _systemRoot + "/" + _scenarioInfoFileName;

    // 사운드 리소스
    public const string _titleBGM = "title";
    public const string _menuSelectSound = "decide1";
    public const string _selectSound = "select";
    // UI 리소스
    public const string _uiTitleMenuPrefabPath = "Title/TitleMenu";
    public const string _uiInputPrefabPath = "Game/Input";
    public const string _uiMenuPrefabPath = "Game/Menu";
    public const string _uiDialoguePrefabPath = "Game/Dialogue";
    public const string _uiSelectPrefabPath = "Game/Select";
    public const string _uiLoadingPrefabPath = "Game/Loading";
    //**추가
    public const string _uiSaveMenuPrefabPath = "Game/SaveMenu";
    public const string _uiLoadMenuPrefabPath = "Game/LoadMenu";
    public const string _uiMenuPanelPrefabPath = "Game/UIMenuPanel";
    public const string _uiYesOrNoPrefabPath = "Game/YesOrNo";
    public const string _uiLoadMenuPanelPrefabPath = "Title/UITitleMenuPanel";
    public const string _uiTitleLoadMenuPrefabPath = "Title/UITitleLoadMenu";
    public const string _uiTitlePopUpPrefabPath = "Title/TitleYesOrNo";
    public const string _uiSettingMenuPrefabPath = "Title/UISettingMenu";
    // ++++ 추가본
    public const string _uiOKPrefabPath = "Game/OK";
    public const string _uiCafePrefabPath = "Cafe/Cafe";
    public const string _uiCafeMenuPrefabPath = "Cafe/CafeMenu";
    public const string _uiCafeDecorStorePrefabPath = "Cafe/CafeDecorStore";
    public const string _uiDecorItemPrefabPath = "Cafe/DocorItem";
    public const string _uiEpiPopUpPrefabPath = "Cafe/EpiPopUp";
    public const string _uiCafeYesOrNoPrefabPath = "Cafe/UICafeYesOrNo";
    public const string _uiOpenCafePrefabPath = "Cafe/CafeOpen";
    public const string _uiExtraPrefabPath = "Game/Extra";
    // 배경 리소스
    public const string _backgroundPrefabPath = _systemRoot + "/Background";
    public const string _backgroundRoot = "Texture";
    public const string _spritePrefabPath = _systemRoot + "/Sprite";
    public const string _itemSpritePrefabPath = _systemRoot + "/ItemBg";

    public const string _spriteRoot = "Texture/Sprite";
    public const string _maskSpriteRoot = _spriteRoot + "/MaskSprite"; // spritem의 마스크 이미지

    public const string _foregroundPrefabPath = _systemRoot + "/Foreground";
    public const string _scenarioRoot = "Scenario";
    public const int _invalidLabelIndex = -1;
    // 씬 회상 이미지
    public const string _ReviewScenFolder = _spriteRoot + "/" + "ReviewScene/";
    // CG 이미지
    public const string _ReviewCGFolder = _spriteRoot + "/" + "ReviewCG/";
    

    //환경설정 이미지
    public const string _settingSpriteRoot = _spriteRoot + "/SettingSprite";
    // 게임 규칙
    private const int _textSpeed = 120; // 초당 출력 글자 수
    public const float _textInterval = 1.0f / _textSpeed; // 글자간 출력 시간
    public const float _waitIconInterval = 0.5f;
    public const float _foregroundCoverDefaultDuration = 1.0f;
    public const float _autoModeDelay = 2f;// 자동모드 딜레이 시간
}