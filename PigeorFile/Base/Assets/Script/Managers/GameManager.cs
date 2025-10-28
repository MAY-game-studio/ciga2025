using UnityEngine;

public class GameManager : SingletonDontDestroy<GameManager>
{
    #region SerializeField
    
    [Header("Debug调试选项")]
    [Tooltip("开发者模式")]
    [SerializeField] public bool FlagDebugMod;

    #endregion
    
    #region Property
    
    private IGameState _currentState;

    public GameModeType GameModeType { get; private set; } //当前游戏主控流程

    public GameSettingData GameSettingData { get; set; } //设置存档

    public GameSaveData GameSaveData { get; set; } //游戏存档

    [HideInInspector] public int SaveSlot; // 当前使用的游戏挡位槽
    
    #endregion
    
    #region GameProcess

    /// <summary>
    /// 状态切换的核心方法,大多数逻辑均在GameState实现
    /// </summary>
    private void ChangeState(IGameState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
    
    /// <summary>
    /// 主控流程开始
    /// </summary>
    private void GameStart()
    {
        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.GAME_INIT)); //主控流程，游戏初始化
        UIManager.GetInstance().PrefabInit<Mouse>(); //初始化鼠标
        UIManager.GetInstance().AwakeVideoInit(); //播放起始动画
    }

    #endregion

    private void Start()
    {
        //Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
        MessageRegister();
        Invoke(nameof(GameStart),0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            MessageManager.GetInstance().Send(MessageTypes.AddNotification,new AddNotification("Ciallo～"));
    }
    
    #region Message

    private void MessageRegister()
    {
        MessageManager.GetInstance().Register(MessageTypes.GameModeChange, OnGameModeChange);
    }

    private void OnGameModeChange(Message message)
    {
        if (message is GameModeChange msg)
        {
            GameModeType = msg.GameModeType;
            switch (msg.GameModeType) //切换游戏状态机
            {
                case GameModeType.GAME_INIT:
                    ChangeState(new GameStateInit());
                    break;
                case GameModeType.MAINMENU:
                    ChangeState(new GameStateMainMenu());
                    break;
                case GameModeType.LOADING:
                    ChangeState(new GameStateLoadingToDefault());
                    break;
                case GameModeType.RELOADING:
                    ChangeState(new GameStateReloadingToDefault());
                    break;
                case GameModeType.MAINMENU_LOADING:
                    ChangeState(new GameStateLoadingToMainMenu());
                    break;
                case GameModeType.DEFAULT:
                    ChangeState(new GameStateDefault());
                    break;
                case GameModeType.PAUSE:
                    ChangeState(new GameStatePause());
                    break;
                case GameModeType.EXIT:
                    ChangeState(new GameStateExiting());
                    break;
            }
        }
    }
    #endregion
}
