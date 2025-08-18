public class GameStateReloading : IGameState
{
    /// <summary>
    /// 游戏中回档
    /// </summary>
    public override void Enter()
    {
        //todo 回档
        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.DEFAULT));
    }
    public override void Exit() { }
}