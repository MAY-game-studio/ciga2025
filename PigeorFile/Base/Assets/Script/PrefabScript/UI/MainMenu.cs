using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        MainMenuGroup.gameObject.SetActive(true);
    }

    private void HideMainMenu()
    {
        MainMenuGroup.gameObject.SetActive(false);
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
        Focus = "SaveMenu";
        SaveGroup.gameObject.SetActive(true);
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
        Focus = "MainMenu";
        SaveGroup.gameObject.SetActive(false);
    }

    public void OnBtnSlot1Clicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        GameManager.GetInstance().SaveSlot = 1;
        GameManager.GetInstance().GameSaveData = _gameSaveFileSlot1;
        if (GameManager.GetInstance().GameSaveData == null)
            GameManager.GetInstance().GameSaveData = SaveManager.GetInstance().CreateGameSaveData();
        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.LOAD));
    }

    public void OnBtnSlot2Clicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        GameManager.GetInstance().SaveSlot = 2;
        GameManager.GetInstance().GameSaveData = _gameSaveFileSlot2;
        if (GameManager.GetInstance().GameSaveData == null)
            GameManager.GetInstance().GameSaveData = SaveManager.GetInstance().CreateGameSaveData();
        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.LOAD));
    }

    public void OnBtnSlot3Clicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        GameManager.GetInstance().SaveSlot = 3;
        GameManager.GetInstance().GameSaveData = _gameSaveFileSlot3;
        if (GameManager.GetInstance().GameSaveData == null)
            GameManager.GetInstance().GameSaveData = SaveManager.GetInstance().CreateGameSaveData();
        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.LOAD));
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
        Focus = "HelpMenu";
        HelpGroup.gameObject.SetActive(true);
        _page = 0;
        Help_Image.sprite = Help_Sprite[_page];
    }

    private void HideHelpMenu()
    {
        ShowMainMenu();
        Focus = "MainMenu";
        HelpGroup.gameObject.SetActive(false);
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
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        HideHelpMenu();
    }
    
    #endregion
    
    #region SettingMenu

    private bool _flagInitializing;
    
    private void ShowSettingMenu()
    {
        Focus = "SettingMenu";
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
        SettingGroup.gameObject.SetActive(false);
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
                case "SaveMenu":
                    HideSaveMenu();
                    break;
                case "HelpMenu":
                    HideHelpMenu();
                    break;
                case "SettingMenu":
                    HideSettingMenu();
                    break;
            }
        }
    }
}