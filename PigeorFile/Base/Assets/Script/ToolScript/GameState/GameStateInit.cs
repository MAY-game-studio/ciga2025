public class GameStateInit : IGameState
{
    /// <summary>
    /// 初始化游戏设置
    /// </summary>
    public override void Enter()
    {
        //重载设置
        GameManager.GetInstance().GameSettingData = SaveManager.GetInstance().GameSettingDataLoad();
        //初始化分辨率（仅在此处游戏开始时重置）
        UIManager.GetInstance().SetResolution();
        //初始化音量与背景音乐
        AudioManager.GetInstance().MainVolume = GameManager.GetInstance().GameSettingData.Volumes.x;
        AudioManager.GetInstance().MusicVolume = GameManager.GetInstance().GameSettingData.Volumes.y;
        AudioManager.GetInstance().SoundVolume = GameManager.GetInstance().GameSettingData.Volumes.z;
    }
    public override void Exit() { }
}