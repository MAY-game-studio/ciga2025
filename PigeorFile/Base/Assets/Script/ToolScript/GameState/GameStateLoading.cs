public class GameStateLoading : IGameState
{
    /// <summary>
    /// 读档，进入游戏内容初始化流程
    /// </summary>
    public override void Enter()
    {
        MessageManager.GetInstance().Send(MessageTypes.SaveDataUpdate, new SaveDataUpdate());//更新游玩数据
        //todo 加载动画
        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.DEFAULT));//进入游戏模式
    }
    public override void Exit() { }
}