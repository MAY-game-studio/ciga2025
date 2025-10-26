using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Serialization;

public class MainMenu : UIPrefabBase
{
    
    #region SerializeField
    
    [Header("UI组件")]

    [Header("主菜单")]
    
    [SerializeField] private GameObject MainMenuGroup;
    [SerializeField] private Image ImgBG;
    [SerializeField] private Image ImgTitle;
    [SerializeField] private Button BtnStart;
    [SerializeField] private Button BtnHelp;
    [SerializeField] private Button BtnSetting;
    [SerializeField] private Button BtnExit;

    #region SaveMenu

    [Header("存档菜单")]
    
    [SerializeField] private GameObject SaveGroup;
    [SerializeField] private Sprite SpriteSave,SpriteEmpty;
    [SerializeField] private Image ImgSlot1;
    [SerializeField] private Button BtnSlot1;
    [SerializeField] private TextMeshProUGUI TxtSlot1;
    [SerializeField] private Image ImgSlot2;
    [SerializeField] private Button BtnSlot2;
    [SerializeField] private TextMeshProUGUI TxtSlot2;
    [SerializeField] private Image ImgSlot3;
    [SerializeField] private Button BtnSlot3;
    [SerializeField] private TextMeshProUGUI TxtSlot3;
    [SerializeField] private Button BtnSaveReturn;

    #endregion
    
    #region HelpMenu

    [Header("帮助菜单")]
    
    [SerializeField] private GameObject HelpGroup;
    [SerializeField] private Sprite[] HelpSprite;
    [SerializeField] private Image ImgHelp;
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

    private enum MainMenuState
    {
        MainMenu,
        SaveMenu,
        HelpMenu,
        SettingMenu
    }
    
    private MainMenuState _mainMenuState = MainMenuState.MainMenu;

    #endregion
    
    #region Main

    private void ShowMainMenu()
    {
        BtnStart.gameObject.TrySetActive(true);
        BtnHelp.gameObject.TrySetActive(true);
        BtnSetting.gameObject.TrySetActive(true);
        BtnExit.gameObject.TrySetActive(true);
    }
    
    private void HideMainMenu()
    {
        BtnStart.gameObject.TrySetActive(false);
        BtnHelp.gameObject.TrySetActive(false);
        BtnSetting.gameObject.TrySetActive(false);
        BtnExit.gameObject.TrySetActive(false);
    }   

    public void OnBtnStartClicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        HideMainMenu();
        ShowSaveMenu();
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
    
    #region SaveMenu

    private GameSaveData _gameSaveFileSlot1,_gameSaveFileSlot2,_gameSaveFileSlot3;

    private void ShowSaveMenu()
    {
        _mainMenuState = MainMenuState.SaveMenu;
        
        SaveGroup.gameObject.TrySetActive(true);
        
        _gameSaveFileSlot1 = SaveManager.GetInstance().GameSaveDataLoad(1);
        _gameSaveFileSlot2 = SaveManager.GetInstance().GameSaveDataLoad(2);
        _gameSaveFileSlot3 = SaveManager.GetInstance().GameSaveDataLoad(3);
        if (_gameSaveFileSlot1 == null)
        {
            ImgSlot1.sprite = SpriteEmpty;
            TxtSlot1.text = "新游戏";
        }
        else
        {
            ImgSlot1.sprite = SpriteSave;
            TxtSlot1.text = "继续游戏" + _gameSaveFileSlot1.GameSaveTime;
        }
        if (_gameSaveFileSlot2 == null)
        {
            ImgSlot2.sprite = SpriteEmpty;
            TxtSlot2.text = "新游戏";
        }
        else
        {
            ImgSlot2.sprite = SpriteSave;
            TxtSlot2.text = "继续游戏" + _gameSaveFileSlot2.GameSaveTime;
        }
        if (_gameSaveFileSlot3 == null)
        {
            ImgSlot3.sprite = SpriteEmpty;
            TxtSlot3.text = "新游戏";
        }
        else
        {
            ImgSlot3.sprite = SpriteSave;
            TxtSlot3.text = "继续游戏" + _gameSaveFileSlot3.GameSaveTime;
        }
    }

    private void HideSaveMenu()
    {
        ShowMainMenu();
        _mainMenuState = MainMenuState.MainMenu;
        SaveGroup.TrySetActive(false);
    }

    public void OnBtnSlot1Clicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        GameManager.GetInstance().SaveSlot = 1;
        GameManager.GetInstance().GameSaveData = _gameSaveFileSlot1;
        if (GameManager.GetInstance().GameSaveData == null)
            GameManager.GetInstance().GameSaveData = SaveManager.GetInstance().CreateGameSaveData();
        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.LOADING));
    }

    public void OnBtnSlot2Clicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        GameManager.GetInstance().SaveSlot = 2;
        GameManager.GetInstance().GameSaveData = _gameSaveFileSlot2;
        if (GameManager.GetInstance().GameSaveData == null)
            GameManager.GetInstance().GameSaveData = SaveManager.GetInstance().CreateGameSaveData();
        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.LOADING));
    }

    public void OnBtnSlot3Clicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        GameManager.GetInstance().SaveSlot = 3;
        GameManager.GetInstance().GameSaveData = _gameSaveFileSlot3;
        if (GameManager.GetInstance().GameSaveData == null)
            GameManager.GetInstance().GameSaveData = SaveManager.GetInstance().CreateGameSaveData();
        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.LOADING));
    }
    
    public void OnBtnSaveReturnClicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        HideSaveMenu();
    }
    
    #endregion
    
    #region HelpMenu
    
    private int _page;

    private void ShowHelpMenu()
    {
        _mainMenuState = MainMenuState.HelpMenu;
        
        HelpGroup.gameObject.TrySetActive(true);
        _page = 0;
        ImgHelp.sprite = HelpSprite[_page];
    }

    private void HideHelpMenu()
    {
        ShowMainMenu();
        _mainMenuState = MainMenuState.MainMenu;
        HelpGroup.TrySetActive(false);
    }
    public void OnBtnLeftClicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        _page = _page - 1 < 0 ? HelpSprite.Length - 1 : _page - 1;
        ImgHelp.sprite = HelpSprite[_page];
    }
    public void OnBtnRightClicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        _page = _page + 1 > HelpSprite.Length - 1 ? 0 : _page + 1;
        ImgHelp.sprite = HelpSprite[_page];
    }
    
    public void OnBtnHelpReturnClicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        HideHelpMenu();
    }
    
    #endregion
    
    #region SettingMenu

    private bool _flagInitializing;
    
    private void ShowSettingMenu()
    {
        _mainMenuState = MainMenuState.SettingMenu;
        
        SettingGroup.gameObject.TrySetActive(true);
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
        _mainMenuState = MainMenuState.MainMenu;
        SettingGroup.TrySetActive(false);
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
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        HideSettingMenu();
    }

    #endregion
    
    void Update()
    {
        if (Input.GetKeyDown(GameManager.GetInstance().GameSettingData.Return))
        {
            switch (_mainMenuState)
            {
                case MainMenuState.MainMenu:
                    break;
                case MainMenuState.SaveMenu:
                    OnBtnSaveReturnClicked();
                    HideSaveMenu();
                    break;
                case MainMenuState.HelpMenu:
                    OnBtnHelpReturnClicked();
                    HideHelpMenu();
                    break;
                case MainMenuState.SettingMenu:
                    OnBtnSettingReturnClicked();
                    HideSettingMenu();
                    break;
            }
        }
    }
}