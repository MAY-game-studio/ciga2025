using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    
    #region SerializeField
    
    [Header("UI组件")]

    [Header("主菜单")]
    
    [SerializeField] private GameObject MainMenuGroup;
    [SerializeField] private Image MainMenu_BG;
    [SerializeField] private Image MainMenu_Title;
    [SerializeField] private Button MainMenu_BtnStart;
    [SerializeField] private Button MainMenu_BtnHelp;
    [SerializeField] private Button MainMenu_BtnSetting;
    [SerializeField] private Button MainMenu_BtnExit;

    #region ChapterMenu

    [Header("存档菜单")]
    
    [SerializeField] private GameObject ChapterGroup;
    [SerializeField] private Sprite SpriteChapter,SpriteLocked;
    [SerializeField] private Button BtnChapter1;
    [SerializeField] private TextMeshProUGUI TxtChapter1;
    [SerializeField] private Button BtnChapter2;
    [SerializeField] private TextMeshProUGUI TxtChapter2;
    [SerializeField] private Button BtnChapter3;
    [SerializeField] private TextMeshProUGUI TxtChapter3;
    [SerializeField] private Button BtnChapterReturn;

    #endregion
    
    #region HelpMenu

    [Header("帮助菜单")]
    
    [SerializeField] private GameObject HelpGroup;
    [SerializeField] private Sprite[] Help_Sprite;
    [SerializeField] private Image Help_Image;
    [SerializeField] private Button BtnLeft;
    [SerializeField] private Button BtnRight;
    [SerializeField] private Button BtnHelpReturn;

    #endregion
    
    #region SettingMenu

    [Header("设置菜单")]
    
    [SerializeField] private GameObject SettingGroup;
    [SerializeField] private TMP_Dropdown DropdownScreenMode;
    [SerializeField] private Vector2[] ResolutionRatio;
    [SerializeField] private TMP_Dropdown DropdownResolutionRatio;
    [SerializeField] private Slider MainVolumeSlider; 
    [SerializeField] private Slider MusicVolumeSlider; 
    [SerializeField] private Slider SoundVolumeSlider; 
    [SerializeField] private Button BtnSettingReturn;
    //todo 键位
    
    #endregion
    
    #endregion

    #region Property

    [HideInInspector] public string Focus;

    #endregion
    
    #region Main

    private void ShowMainMenu()
    {
        DOTween.Complete(MainMenuGroup);
        MainMenuGroup.gameObject.SetActive(true);
    }

    private void HideMainMenu()
    {
        DOTween.Complete(MainMenuGroup);
        Transform stand = MainMenuGroup.transform.Find("StandObject");
        if (stand != null)
            stand.gameObject.SetActive(false);
        else
            MainMenuGroup.SetActive(false);
    }
    
    public void OnBtnStartClicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        HideMainMenu();
        ShowChapterMenu();
    }
    public void OnBtnHelpClicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        HideMainMenu();
        ShowHelpMenu();
    }
    public void OnBtnSettingClicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        HideMainMenu();
        ShowSettingMenu();
    }
    public void OnBtnExitClicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.EXIT));
    }

    #endregion
    
    #region ChapterMenu

    private GameSaveData _gameSaveFile;

    private void ShowChapterMenu()
    {
        Focus = "ChapterMenu";
        DOTween.Complete(ChapterGroup);
        ChapterGroup.gameObject.SetActive(true);
        
        _gameSaveFile = SaveManager.GetInstance().GameSaveDataLoad();

        if (_gameSaveFile == null)
        {
            
        }

    }

    private void HideChapterMenu()
    {
        ShowMainMenu();
        Focus = "MainMenu";
        DOTween.Complete(ChapterGroup);
        Transform stand = ChapterGroup.transform.Find("StandObject");
        if (stand != null)
            stand.gameObject.SetActive(false);
        else
            MainMenuGroup.SetActive(false);
    }

    public void OnBtnChapter1Clicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        MessageManager.GetInstance().Send(MessageTypes.ChapterStart,new ChapterStart(1));
    }

    public void OnBtnChapter2Clicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        MessageManager.GetInstance().Send(MessageTypes.ChapterStart,new ChapterStart(2));
    }

    public void OnBtnChapter3Clicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        MessageManager.GetInstance().Send(MessageTypes.ChapterStart,new ChapterStart(3));
    }
    
    public void OnBtnChapterReturnClicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        HideChapterMenu();
    }
    
    #endregion
    
    #region HelpMenu
    
    private int _page;

    private void ShowHelpMenu()
    {
        Focus = "HelpMenu";
        DOTween.Complete(HelpGroup);
        HelpGroup.gameObject.SetActive(true);
        _page = 0;
        Help_Image.sprite = Help_Sprite[_page];
    }

    private void HideHelpMenu()
    {
        ShowMainMenu();
        Focus = "MainMenu";
        DOTween.Complete(HelpGroup);
        Transform stand = HelpGroup.transform.Find("StandObject");
        if (stand != null)
            stand.gameObject.SetActive(false);
        else
            MainMenuGroup.SetActive(false);
    }
    public void OnBtnLeftClicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        _page = _page - 1 < 0 ? Help_Sprite.Length - 1 : _page - 1;
        Help_Image.sprite = Help_Sprite[_page];
    }
    public void OnBtnRightClicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        _page = _page + 1 > Help_Sprite.Length - 1 ? 0 : _page + 1;
        Help_Image.sprite = Help_Sprite[_page];
    }
    
    public void OnBtnHelpReturnClicked()
    {
//        if (MainMenuGroup.activeSelf) return; // MainMenuGroup 动画进程冲突，不处理
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        HideHelpMenu();
    }
    
    #endregion
    
    #region SettingMenu

    private bool _flagInitializing;
    
    private void ShowSettingMenu()
    {
        Focus = "SettingMenu";
        DOTween.Complete(SettingGroup);
        SettingGroup.gameObject.SetActive(true);
        MainVolumeSlider.value = AudioManager.GetInstance().MainVolume;
        MusicVolumeSlider.value = AudioManager.GetInstance().MusicVolume;
        SoundVolumeSlider.value = AudioManager.GetInstance().SoundVolume;
        _flagInitializing = true;
        DropdownScreenMode.value = (int)GameManager.GetInstance().GameSettingData.ScreenMode;
        DropdownResolutionRatio.value = (int)GameManager.GetInstance().GameSettingData.ResolutionType;
        _flagInitializing = false;
    }

    private void HideSettingMenu()
    {
        ShowMainMenu();
        Focus = "MainMenu";
        DOTween.Complete(SettingGroup);
        Transform stand = SettingGroup.transform.Find("StandObject");
        if (stand != null)
            stand.gameObject.SetActive(false);
        else
            MainMenuGroup.SetActive(false);
    }
    
    public void OnDropdownScreenModeChange()
    {
        if (_flagInitializing) return;
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        switch (DropdownScreenMode.value)
        {
            case 0:
                GameManager.GetInstance().GameSettingData.ScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                GameManager.GetInstance().GameSettingData.ScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                GameManager.GetInstance().GameSettingData.ScreenMode = FullScreenMode.MaximizedWindow;
                break;
            case 3:
                GameManager.GetInstance().GameSettingData.ScreenMode = FullScreenMode.Windowed;
                break;
        }
        MessageManager.GetInstance().Send(MessageTypes.AddNotification,new AddNotification("重启后生效"));
        MessageManager.GetInstance().Send(MessageTypes.SettingDataUpdate,new SettingDataUpdate());
    }
    
    public void OnDropdownResolutionRatioChange()
    {
        if (_flagInitializing) return;
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        GameManager.GetInstance().GameSettingData.ResolutionRatio = ResolutionRatio[DropdownResolutionRatio.value];
        GameManager.GetInstance().GameSettingData.ResolutionType = (ResolutionType)DropdownResolutionRatio.value;

        MessageManager.GetInstance().Send(MessageTypes.AddNotification,new AddNotification("重启后生效"));
        MessageManager.GetInstance().Send(MessageTypes.SettingDataUpdate,new SettingDataUpdate());
    }
    
    public void OnMainVolumeChange()
    {
        AudioManager.GetInstance().MainVolume=MainVolumeSlider.value;
        GameManager.GetInstance().GameSettingData.Volumes.x = MainVolumeSlider.value;
        MessageManager.GetInstance().Send(MessageTypes.SettingDataUpdate,new SettingDataUpdate());
    }
    public void OnMusicVolumeChange()
    {
        AudioManager.GetInstance().MusicVolume=MusicVolumeSlider.value;
        GameManager.GetInstance().GameSettingData.Volumes.y = MusicVolumeSlider.value;
        MessageManager.GetInstance().Send(MessageTypes.SettingDataUpdate,new SettingDataUpdate());
    }
    public void OnSoundVolumeChange()
    {
        AudioManager.GetInstance().SoundVolume=SoundVolumeSlider.value;
        GameManager.GetInstance().GameSettingData.Volumes.z = SoundVolumeSlider.value;
        MessageManager.GetInstance().Send(MessageTypes.SettingDataUpdate,new SettingDataUpdate());
    }

    public void OnBtnSettingReturnClicked()
    {
//        if (MainMenuGroup.activeSelf) return; // MainMenuGroup 动画进程冲突，不处理
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        HideSettingMenu();
    }

    #endregion

    void Start()
    {
        Focus = "MainMenu";
    }

    void Update()
    {
        if (Input.GetKeyDown(GameManager.GetInstance().GameSettingData.Return))
        {
            switch (Focus)
            {
                case "MainMenu":
                    break;
                case "ChapterMenu":
                    DOTween.Complete(MainMenuGroup);
                    DOTween.Complete(ChapterGroup);
                    HideChapterMenu();
                    break;
                case "HelpMenu":
                    DOTween.Complete(MainMenuGroup);
                    DOTween.Complete(HelpGroup);
                    HideHelpMenu();
                    break;
                case "SettingMenu":
                    DOTween.Complete(MainMenuGroup);
                    DOTween.Complete(SettingGroup);
                    HideSettingMenu();
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.Keypad1)) ShowMainMenu();
        
    }
}