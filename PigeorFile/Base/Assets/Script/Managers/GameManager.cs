using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonDontDestory<GameManager>
{
    #region SerializeField
    
    [Tooltip("开发者模式")]
    [SerializeField] public bool FlagDebugMod;

    #endregion
    
    #region Property
    
    private GameModeType _gameModeType;
    public GameModeType GameModeType //当前游戏主控流程
    {
        get => _gameModeType;
        private set => _gameModeType = value;
    }
    
    private GameSettingData _gameSettingData;
    public GameSettingData GameSettingData
    {
        get => _gameSettingData;
        set => _gameSettingData = value;
    }//设置存档

    private GameSaveData _gameSaveData;
    public GameSaveData GameSaveData
    {
        get => _gameSaveData;
        set => _gameSaveData = value;
    }//游戏存档

    [HideInInspector] public int SaveSlot; // 当前使用的游戏挡位槽
    
    #endregion
    
    #region GameProcess

    /// <summary>
    /// 主控流程开始
    /// </summary>
    public void GameStart()
    {
        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.GAMEINIT));//主控流程，游戏初始化
        UIManager.GetInstance().AwakeVideoInit();//播放起始动画
    }

    /// <summary>
    /// 初始化游戏设置
    /// </summary>
    public void GameInit()
    {
        //重载设置
        GameSettingData = SaveManager.GetInstance().GameSettingDataLoad();
        //初始化分辨率（仅在此处游戏开始时重置）
        Screen.SetResolution((int)GameSettingData.ResolutionRatio.x, (int)GameSettingData.ResolutionRatio.y,
            GameSettingData.ScreenMode);
        //初始化音量与背景音乐
        AudioManager.GetInstance().MainVolume = GameSettingData.Volumes.x;
        AudioManager.GetInstance().MusicVolume = GameSettingData.Volumes.y;
        AudioManager.GetInstance().SoundVolume = GameSettingData.Volumes.z;
        
        //设置canvas缩放，初始化主菜单
//        UIManager.GetInstance().SetCanvasScale(SettingData.ResolutionRatio.x/1920);
        
//        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.MAINMENU));
        //不使用开场动画时取消此注释
    }

    /// <summary>
    /// 游戏开始或返回时显示主菜单
    /// </summary>
    public void MainMenuInit()
    {
        //回到主菜单时的清理游戏组件
        UIManager.GetInstance().GameUIDestroy();
        UIManager.GetInstance().MainMenuInit();
    }
    
    public void SaveDataLoad()
    {
        /*if (GameSaveData == null) //新游戏,创建存档
        {
            GameSaveData=SaveManager.GetInstance().CreateGameSaveData();
        }*/
        MessageManager.GetInstance().Send(MessageTypes.SaveDataUpdate, new SaveDataUpdate());

        UIManager.GetInstance().MainMenuDestroy();
        UIManager.GetInstance().GameUIInit();
        
        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.DEFAULT));
    }
    
    /// <summary>
    /// 游戏中回档
    /// </summary>
    public void SaveDataReLoad()
    {
        //todo 回档
        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.DEFAULT));
    }
    
    /// <summary>
    /// 退出游戏
    /// </summary>
    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    #endregion
    
    void Start()
    {
//        Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
        MessageRegister();
        Invoke(nameof(GameStart),0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            MessageManager.GetInstance().Send(MessageTypes.AddNotification,new AddNotification("Ciallo～"));
    }
    
    #region Message

    protected void MessageRegister()
    {
        MessageManager.GetInstance().Register(MessageTypes.GameModeChange, OnGameModeChange);
    }
    
    public void OnGameModeChange(Message message)
    {
        if (message is GameModeChange msg)
        {
            GameModeType = msg.GameModeType;
            switch (GameModeType)
            {
                case GameModeType.GAMEINIT:
                    GameInit();
                    break;
                case GameModeType.MAINMENU:
                    MainMenuInit();
                    break;
                case GameModeType.LOAD:
                    SaveDataLoad();
                    break;
                case GameModeType.RELOAD:
                    SaveDataReLoad();
                    break;
                case GameModeType.DEFAULT://常规游戏态
                    break;
                case GameModeType.PAUSE:
                    break;
                case GameModeType.EXIT:
                    GameExit();
                    break;
            }
        }
    }
    #endregion
}
