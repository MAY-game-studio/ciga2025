using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SaveManager : SingletonDontDestory<SaveManager>
{
    #region Property

    private string _gameSettingDataPath;//设置存档
    private string[] _gameSaveDataPath = new string[4];//游戏存档

    #endregion

    public GameSettingData CreateGameSettingData()//生成初始游戏设置
    {
        GameSettingData gameSettingData = ScriptableObject.CreateInstance<GameSettingData>();
        gameSettingData.ScreenMode = FullScreenMode.Windowed;
        gameSettingData.ResolutionRatio = new Vector2(1920, 1080);
        gameSettingData.ResolutionType = ResolutionType.Res1920x1080;
        gameSettingData.Volumes = new Vector3(0.8f, 0.8f, 0.8f);
        // 初始化默认键位配置
        gameSettingData.MoveUp = KeyCode.W;
        gameSettingData.MoveDown = KeyCode.S;
        gameSettingData.MoveLeft = KeyCode.A;
        gameSettingData.MoveRight = KeyCode.D;
        gameSettingData.Return = KeyCode.Escape;
        gameSettingData.Skip = KeyCode.LeftControl;
        return gameSettingData;
    }

    public GameSaveData CreateGameSaveData()//生成初始游戏存档
    {
        GameSaveData gameSaveData = ScriptableObject.CreateInstance<GameSaveData>();
        gameSaveData.GameSaveTime = gameSaveData.CurrentGameStartTime = DateTime.Now;
        gameSaveData.GameDuration = 0f;
        return gameSaveData;
    }
    
    public GameSettingData GameSettingDataLoad()//读取游戏设置信息
    {
        GameSettingData gameSettingData = ScriptableObject.CreateInstance<GameSettingData>();
        string jsonFile;
        if (!File.Exists(_gameSettingDataPath))//初始化游戏设置
        {
            gameSettingData = CreateGameSettingData();
            Directory.CreateDirectory(Path.GetDirectoryName(_gameSettingDataPath)!);//创建位置
            jsonFile = JsonUtility.ToJson(gameSettingData);
            File.WriteAllText(_gameSettingDataPath, jsonFile);
        }
        else
        {
            jsonFile = File.ReadAllText(_gameSettingDataPath);
            JsonUtility.FromJsonOverwrite(jsonFile, gameSettingData);
        }
        return gameSettingData;
    }

    public GameSaveData GameSaveDataLoad(int saveSlot)//读取游戏槽位存档信息
    {
        GameSaveData gameSaveFile=ScriptableObject.CreateInstance<GameSaveData>();
        if (!File.Exists(_gameSaveDataPath[saveSlot])) return null;//该槽位没有存档
        string jsonFile = File.ReadAllText(_gameSaveDataPath[saveSlot]);
        JsonUtility.FromJsonOverwrite(jsonFile, gameSaveFile);
        return gameSaveFile;
    }
    
    public void SavePathInit()//初始化存档路径
    {
        _gameSettingDataPath = Path.Combine(Application.persistentDataPath, "SaveFiles", "SettingData.json");
        _gameSaveDataPath[1] = Path.Combine(Application.persistentDataPath, "SaveFiles", "GameData1.json");
        _gameSaveDataPath[2] = Path.Combine(Application.persistentDataPath, "SaveFiles", "GameData2.json");
        _gameSaveDataPath[3] = Path.Combine(Application.persistentDataPath, "SaveFiles", "GameData3.json");
    }
    
    void Start()
    {
        MessageInit();
        SavePathInit();
    }
    #region MessageHandler
    private void MessageInit()
    {
        MessageManager.GetInstance().Register(MessageTypes.SettingDataUpdate, OnSettingDataUpdate);
        MessageManager.GetInstance().Register(MessageTypes.SaveDataUpdate, OnSaveDataUpdate);
    }

    public void OnSettingDataUpdate(Message message)//更新设置菜单预设
    {
        if (message is SettingDataUpdate msg)
        {
            if (!Directory.Exists(Path.GetDirectoryName(_gameSettingDataPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(_gameSettingDataPath)!);// 确保目录存在
            var jsonFile = JsonUtility.ToJson(GameManager.GetInstance().GameSettingData);
            File.WriteAllText(_gameSettingDataPath, jsonFile); //保存设置
        }
    }
    
    public void OnSaveDataUpdate(Message message)
    {
        if (message is SaveDataUpdate msg)
        {
            GameSaveData saveData = GameManager.GetInstance().GameSaveData;
            saveData.GameSaveTime = DateTime.Now; //最后游玩日期
            saveData.GameDuration += (float)(saveData.GameSaveTime - saveData.CurrentGameStartTime).TotalSeconds; //记录游戏时长
            saveData.CurrentGameStartTime = saveData.GameSaveTime;
            
            GameManager.GetInstance().GameSaveData = saveData;
            if (!Directory.Exists(Path.GetDirectoryName(_gameSettingDataPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(_gameSettingDataPath)!); // 确保目录存在
            string jsonFile = JsonUtility.ToJson(saveData);
            File.WriteAllText(_gameSaveDataPath[GameManager.GetInstance().SaveSlot], jsonFile);
        }
    }
    #endregion
}
