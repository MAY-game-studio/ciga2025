using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateLoadingToMainMenu : GameStateLoadingBase
{
    protected override void LoadingProcess()
    {
        MessageManager.GetInstance().Register(MessageTypes.SaveDataComplete, OnSaveDataComplete,0,MessageTemporaryType.Temporary);
        MessageManager.GetInstance().Send(MessageTypes.SaveDataUpdate, new SaveDataUpdate());
    }
    
    private void OnSaveDataComplete(Message msg)
    {
        MessageManager.GetInstance().Remove(MessageTypes.SaveDataComplete, OnSaveDataComplete);
        //todo 销毁游戏资源
        MessageManager.GetInstance().Send(MessageTypes.GameModeChange, new GameModeChange(GameModeType.MAINMENU));
    }
}
