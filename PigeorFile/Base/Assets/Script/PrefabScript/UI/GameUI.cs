using UnityEngine;
using UnityEngine.UI;

public class GameUI : UIPrefabBase
{
    #region SerializeField

    [Header("UI组件")]
    
    #region PauseMenu

    [Header("暂停菜单")]
    
    [SerializeField] private Button BtnPause;
    [SerializeField] private GameObject PauseGroup;
    [SerializeField] private Button BtnSave;
    [SerializeField] private Button BtnReload;

    #region SettingMenu

    [Header("设置菜单")]
    
    [SerializeField] private Button BtnSetting;
    [SerializeField] private GameObject SettingGroup;
    [SerializeField] private Slider MainVolumeSlider; 
    [SerializeField] private Slider MusicVolumeSlider; 
    [SerializeField] private Slider SoundVolumeSlider; 
    [SerializeField] private Button BtnSettingReturn;

    #endregion
    
    [SerializeField] private Button BtnMainMenu;
    [SerializeField] private Button BtnExit;
    [SerializeField] private Button BtnPauseReturn;

    #endregion
    
    #endregion
    
    #region Property

    private enum GameUIState
    {
        GameCanvas,
        PauseMenu,
        SettingMenu
    }
    
    private GameUIState _gameUIState = GameUIState.GameCanvas;
    
    #endregion
    
    #region PauseMenu

    private void ShowPauseMenu()
    {
        _gameUIState = GameUIState.PauseMenu;
        PauseGroup.gameObject.TrySetActive(true);
        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.PAUSE));
    }

    private void HidePauseMenu()
    {
        _gameUIState = GameUIState.GameCanvas;
        PauseGroup.gameObject.TrySetActive(false);
        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.DEFAULT));
    }
    
    public void OnBtnPauseClicked()
    {
        if (_gameUIState != GameUIState.GameCanvas) return;
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        ShowPauseMenu();
    }
    
    public void OnBtnSaveClicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        MessageManager.GetInstance().Send(MessageTypes.SaveDataUpdate, new SaveDataUpdate());
        MessageManager.GetInstance().Send(MessageTypes.AddNotification,new AddNotification("Saved"));
    }
    
    public void OnBtnReloadClicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.RELOADING));
        MessageManager.GetInstance().Send(MessageTypes.AddNotification,new AddNotification("ReLoad"));
        HidePauseMenu();
    }

    public void OnBtnSettingClicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound, new PlaySound(SoundClip.BTN_CLICK));
        ShowSettingMenu();
    }

    #region SettingMenu

    private void ShowSettingMenu()
    {
        _gameUIState = GameUIState.SettingMenu;
        SettingGroup.gameObject.SetActive(true);
        MainVolumeSlider.value = AudioManager.GetInstance().MainVolume;
        MusicVolumeSlider.value = AudioManager.GetInstance().MusicVolume;
        SoundVolumeSlider.value = AudioManager.GetInstance().SoundVolume;
    }

    private void HideSettingMenu()
    {
        _gameUIState = GameUIState.GameCanvas;
        SettingGroup.gameObject.SetActive(false);
    }
    
    public void OnMainVolumeChange()
    {
        AudioManager.GetInstance().MainVolume=MainVolumeSlider.value;
        GameManager.GetInstance().GameSettingData.Volumes.x = MainVolumeSlider.value;
        MessageManager.GetInstance().Send(MessageTypes.SettingDataUpdate, new SettingDataUpdate());
    }
    public void OnMusicVolumeChange()
    {
        AudioManager.GetInstance().MusicVolume=MusicVolumeSlider.value;
        GameManager.GetInstance().GameSettingData.Volumes.y = MusicVolumeSlider.value;
        MessageManager.GetInstance().Send(MessageTypes.SettingDataUpdate, new SettingDataUpdate());
    }
    public void OnSoundVolumeChange()
    {
        AudioManager.GetInstance().SoundVolume=SoundVolumeSlider.value;
        GameManager.GetInstance().GameSettingData.Volumes.z = SoundVolumeSlider.value;
        MessageManager.GetInstance().Send(MessageTypes.SettingDataUpdate, new SettingDataUpdate());
    }

    public void OnBtnSettingReturnClicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        HideSettingMenu();
    }


    #endregion

    public void OnBtnMainMenuClicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.MAINMENU_LOADING));//触发加载界面的回到主菜单
    }
    public void OnBtnExitClicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        MessageManager.GetInstance().Send(MessageTypes.SaveDataUpdate, new SaveDataUpdate());
        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.EXIT));
    }
    public void OnBtnPauseReturnClicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        HidePauseMenu();
    }
    
    #endregion

    private void Start()
    {
        _gameUIState = GameUIState.GameCanvas;
    }

    private void Update()
    {
        if (Input.GetKeyDown(GameManager.GetInstance().GameSettingData.Return))
        {
            switch (_gameUIState)
            {
                case GameUIState.GameCanvas:
                    ShowPauseMenu();
                    break;
                case GameUIState.PauseMenu:
                    HidePauseMenu();
                    break;
                case GameUIState.SettingMenu:
                    HideSettingMenu();
                    break;
            }
        }
    }
}
