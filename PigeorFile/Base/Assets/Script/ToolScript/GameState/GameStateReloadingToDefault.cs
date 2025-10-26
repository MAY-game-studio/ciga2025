using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateReloadingToDefault : GameStateLoadingBase
{
    protected override void LoadingProcess()
    {
        //todo 重新加载游戏资源
        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.DEFAULT));
    }
}
