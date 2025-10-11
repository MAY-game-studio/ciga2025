using System.Collections;
using System.Collections.Generic;
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

    [HideInInspector] public string Focus; //UI焦点
    
    #endregion
    
    #region PauseMenu

    private void ShowPauseMenu()
    {
        Focus="PauseMenu";
        PauseGroup.gameObject.SetActive(true);
    }

    private void HidePauseMenu()
    {
        Focus = "GameCanvas";
        PauseGroup.gameObject.SetActive(false);
    }
    
    public void ONBtnPauseClicked()
    {
        if (Focus != "GameCanvas") return;
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        ShowPauseMenu();
    }
    
    public void ONBtnSaveClicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        MessageManager.GetInstance().Send(MessageTypes.SaveDataUpdate, new SaveDataUpdate());
        MessageManager.GetInstance().Send(MessageTypes.AddNotification,new AddNotification("Saved"));
    }
    
    public void ONBtnReloadClicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.RELOADING));
        MessageManager.GetInstance().Send(MessageTypes.AddNotification,new AddNotification("ReLoad"));
        HidePauseMenu();
    }

    public void ONBtnSettingClicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound, new PlaySound(SoundClip.BTN_CLICK));
        ShowSettingMenu();
    }

    #region SettingMenu

    private void ShowSettingMenu()
    {
        Focus = "SettingMenu";
        SettingGroup.gameObject.SetActive(true);
        MainVolumeSlider.value = AudioManager.GetInstance().MainVolume;
        MusicVolumeSlider.value = AudioManager.GetInstance().MusicVolume;
        SoundVolumeSlider.value = AudioManager.GetInstance().SoundVolume;
    }

    private void HideSettingMenu()
    {
        Focus = "GameCanvas";
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

    public void ONBtnSettingReturnClicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        HideSettingMenu();
    }


    #endregion

    public void ONBtnMainMenuClicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        MessageManager.GetInstance().Send(MessageTypes.SaveDataUpdate, new SaveDataUpdate());
        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.MAINMENU));
    }
    public void ONBtnExitClicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        MessageManager.GetInstance().Send(MessageTypes.SaveDataUpdate, new SaveDataUpdate());
        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.EXIT));
    }
    public void ONBtnPauseReturnClicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.BTN_CLICK));
        HidePauseMenu();
    }
    
    #endregion
    
    public void UIModeChange(GameModeType Type)
    {
        switch (Type)
        {
            case GameModeType.DEFAULT:
                BtnPause.gameObject.SetActive(true);
                break;
            case GameModeType.PAUSE:
                BtnPause.gameObject.SetActive(false);
                break;
        }
    }
    
    void Start()
    {
        Focus = "GameCanvas";
    }
    
    void Update()
    {
        if (Input.GetKeyDown(GameManager.GetInstance().GameSettingData.Return))
        {
            switch (Focus)
            {
                case "GameCanvas":
                    ShowPauseMenu();
                    break;
                case "PauseMenu":
                    HidePauseMenu();
                    break;
                case "SettingMenu":
                    HideSettingMenu();
                    break;
            }
        }
    }
}
