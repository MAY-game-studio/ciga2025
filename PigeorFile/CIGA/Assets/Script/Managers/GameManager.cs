using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonDontDestory<GameManager>
{
    #region SerializeField
    
    [Header("Debug调试选项")]
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
    
    #endregion
    
    #region GameProcess

    /// <summary>
    /// 主控流程开始
    /// </summary>
    private void GameStart()
    {
        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.GAMEINIT)); //主控流程，游戏初始化
//        UIManager.GetInstance().MouseInit();//初始化鼠标
//        MessageManager.GetInstance().Send(MessageTypes.SwitchMouseMode,new SwitchMouseMode(MouseMode.DEFAULT)); //修改鼠标样式
        UIManager.GetInstance().AwakeVideoInit(); //播放起始动画
//        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.MAINMENU));
    }

    /// <summary>
    /// 初始化游戏设置
    /// </summary>
    private void GameInit()
    {
        //重载设置
        GameSettingData = SaveManager.GetInstance().GameSettingDataLoad();
        //初始化分辨率（仅在此处游戏开始时重置）
        UIManager.GetInstance().SetResolution();
        
        //初始化音量与背景音乐
        AudioManager.GetInstance().MainVolume = GameSettingData.Volumes.x;
        AudioManager.GetInstance().MusicVolume = GameSettingData.Volumes.y;
        AudioManager.GetInstance().SoundVolume = GameSettingData.Volumes.z;
        
//        MessageManager.GetInstance().Send(MessageTypes.ChapterStart,new ChapterStart(1));
    }

    /// <summary>
    /// 游戏开始或返回时显示主菜单
    /// </summary>
    private void MainMenuInit()
    {
        //回到主菜单时的清理游戏组件
        UIManager.GetInstance().GameUIDestroy();
        UIManager.GetInstance().MainMenuInit();
    }

    private void NewChapter()
    {
        UIManager.GetInstance().MainMenuDestroy();
//        UIManager.GetInstance().GameUIDestroy();
//        StartCoroutine(GameUIInit());
        UIManager.GetInstance().GameUIInit();
    }

    private IEnumerator GameUIInit()
    {
        while (UIManager.GetInstance().GameUI != null)
        {
            yield return null; // 每帧检查一次
        }
//        yield return new 
        UIManager.GetInstance().GameUIInit();
    }
    
    /// <summary>
    /// 退出游戏
    /// </summary>
    private void GameExit()
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
        //Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
        MessageRegister();
        Invoke(nameof(GameStart),0);
    }

    void Update()
    {
//            MessageManager.GetInstance().Send(MessageTypes.AddNotification,new AddNotification("Ciallo～"));
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
                    Time.timeScale = 1;
                    MainMenuInit();
                    break;
/*                case GameModeType.LOAD:
                    SaveDataLoad();
                    break;
                case GameModeType.RELOAD:
                    SaveDataReLoad();
                    break;*/
                case GameModeType.CHAPTER://常规游戏态
                    NewChapter();
                    Time.timeScale = 1;
                    break;
                case GameModeType.PAUSE:
                    Time.timeScale = 0;
                    break;
                case GameModeType.EXIT:
                    GameExit();
                    break;
            }
        }
    }
    #endregion
}
