public class GameStateMainMenu : IGameState
{
    /// <summary>
    /// 游戏开始或返回时显示主菜单
    /// </summary>
    public override void Enter()
    {
        UIManager.GetInstance().MainMenuInit(); 
    }

    /// <summary>
    /// 销毁主菜单
    /// </summary>
    public override void Exit()
    {
        UIManager.GetInstance().MainMenuDestroy();
    }
}