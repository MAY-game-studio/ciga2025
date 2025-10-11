public class GameStateDefault : IGameState
{
    /// <summary>
    /// 初始化游戏UI
    /// </summary>
    public override void Enter()
    {
        UIManager.GetInstance().PrefabInit<GameUI>();
    }

    /// <summary>
    /// 销毁游戏UI
    /// </summary>
    public override void Exit()
    {
        UIManager.GetInstance().PrefabDestroy<GameUI>();
    }
}