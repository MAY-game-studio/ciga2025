using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void ONBtnStartClicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.START));
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundType.BTNCLICK));
    }

    public void ONBtnExitClicked()
    {
        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.EXIT));
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundType.BTNCLICK));
    }
}