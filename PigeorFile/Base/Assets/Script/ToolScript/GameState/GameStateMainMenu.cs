public class GameStateMainMenu : IGameState
{
    /// <summary>
    /// 游戏开始或返回时显示主菜单
    /// 会先尝试销毁游戏UI
    /// </summary>
    public override void Enter()
    {
        UIManager.GetInstance().PrefabDestroy<GameUI>();
        UIManager.GetInstance().PrefabInit<MainMenu>();
        MessageManager.GetInstance().Send(MessageTypes.LoadFinish,new LoadFinish()); //可能需要结束加载
    }

    /// <summary>
    /// 销毁主菜单
    /// </summary>
    public override void Exit()
    {
        UIManager.GetInstance().PrefabDestroy<MainMenu>();
    }
}