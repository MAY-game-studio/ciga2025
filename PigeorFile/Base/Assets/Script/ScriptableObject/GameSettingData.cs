using UnityEngine;

/// <summary>
/// 游戏设置:全屏，分辨率，音量
/// </summary>
[CreateAssetMenu(fileName = "GameSettingData", menuName = "ScriptableObjects/GameSettingData", order = 0)]
public class GameSettingData : ScriptableObject
{
    #region BasicSetting

    public FullScreenMode ScreenMode; //显示模式
    public Vector2 ResolutionRatio; //分辨率
    public ResolutionType ResolutionType; //分辨率类别
    public Vector3 Volumes; //音量

    #endregion

    #region KeySetting

    public KeyCode MoveUp;       // 上移 (W)
    public KeyCode MoveDown;     // 下移 (S)
    public KeyCode MoveLeft;     // 左移 (A)
    public KeyCode MoveRight;    // 右移 (D)
    public KeyCode Return;       // 返回 (Esc)
    public KeyCode Skip;         // 跳过 (Ctrl)
    
    #endregion
}