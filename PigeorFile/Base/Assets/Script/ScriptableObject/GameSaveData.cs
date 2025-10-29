using System;
using UnityEngine;

/// <summary>
/// 基本信息:最后存档时间,游戏时长
/// </summary>

[CreateAssetMenu(fileName = "GameSaveData", menuName = "ScriptableObjects/GameSaveData", order = 1)]
public class GameSaveData : ScriptableObject
{
    #region BasicData
    
    public DateTime GameSaveTime; //游戏最后保存时间
    public DateTime CurrentGameStartTime; //上次继续游戏的时间
    public float GameDuration; //游戏时长

    #endregion

    #region EventFlag

    public bool FlagGameStart;

    #endregion
}