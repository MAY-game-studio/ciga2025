public class GameStateExiting : IGameState
{
    /// <summary>
    /// 退出游戏
    /// </summary>
    public override void Enter()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public override void Exit() { }
}