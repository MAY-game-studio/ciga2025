public class GameStateDefault : IGameState
{
    /// <summary>
    /// 初始化游戏
    /// </summary>
    public override void Enter()
    {
        UIManager.GetInstance().PrefabInit<GameUI>();
        //todo：初始化游戏
        MessageManager.GetInstance().Send(MessageTypes.LoadFinish,new LoadFinish());
    }

    public override void Exit() { }
}