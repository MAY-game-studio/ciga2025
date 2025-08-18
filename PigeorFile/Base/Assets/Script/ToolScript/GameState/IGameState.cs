public abstract class IGameState // 游戏状态的基类
{
    public abstract void Enter(); // 进入此状态时调用的方法
    public abstract void Exit(); // 离开此状态时调用的方法
}
