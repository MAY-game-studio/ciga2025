public class GameStateDefault : IGameState
{
    public GameStateDefault() : base() { }
    /// <summary>
    /// 初始化游戏UI
    /// </summary>
    public override void Enter()
    {
        UIManager.GetInstance().GameUIInit();
    }

    /// <summary>
    /// 销毁游戏UI
    /// </summary>
    public override void Exit()
    {
        UIManager.GetInstance().GameUIDestroy();
    }
}