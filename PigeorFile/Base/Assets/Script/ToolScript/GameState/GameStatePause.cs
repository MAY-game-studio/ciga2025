using UnityEngine;

public class GameStatePause : IGameState
{
    public override void Enter()
    {
        Time.timeScale = 0f; //冻结游戏时间
    }

    public override void Exit()
    {
        Time.timeScale = 1f; //恢复游戏时间
    }
}