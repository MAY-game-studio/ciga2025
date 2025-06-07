using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonDontDestory<GameManager>
{
    private GameModeType _gameModeType;
    public GameModeType GameModeType
    {
        get { return _gameModeType; }
        private set { _gameModeType = value; }
    }

    
    
    
    // Start is called before the first frame update
    void Start()
    {

        MessageRegister();        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    #region Message

    protected void MessageRegister()
    {
        MessageManager.GetInstance().Register(MessageTypes.GameModeChange, OnGameModeChange);
    }
    
    public void OnGameModeChange(Message message)
    {
        if (message is GameModeChange msg)
        {
            GameModeType = msg.GameModeType;
            switch (GameModeType)
            {
                case GameModeType.GAMEINIT:
//                    GameInit();
                    break;
                case GameModeType.MAINMENU:
//                   UIManager.GetInstance().MainMenuInit();
                    break;
                case GameModeType.START:
//                    GameStart();
                    break;
                case GameModeType.JOUNARY:
//                   Jounary();
                    break;
                case GameModeType.REST:
//                    Rest();
                    break;
                case GameModeType.EXIT:
//                    GameExit();
                    break;
            }
        }
    }
    #endregion
}
