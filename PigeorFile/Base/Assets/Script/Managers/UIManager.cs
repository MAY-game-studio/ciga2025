using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class UIManager : SingletonDontDestory<UIManager>
{
    #region SerializeField

    #region Canvas

    [SerializeField] public Canvas Canvas;

    #endregion

    #region Prefab

    [SerializeField] private GameObject MainMenu_Prefab;
    [SerializeField] private GameObject GameUI_Prefab;
    [SerializeField] private GameObject Notification_Prefab;

    #endregion

    #region Sprite

    [SerializeField] public Sprite[] Sprite;

    #endregion

    #region Videos
    
    [SerializeField] private GameObject AwakeVideo_Prefab;

    #endregion

    #endregion
    
    #region Videos
    
    #region AwakeVideo

    [HideInInspector] public AwakeVideo AwakeVideo;

    public void AwakeVideoInit()
    {
        AwakeVideo = Instantiate(AwakeVideo_Prefab, Canvas.transform).GetComponent<AwakeVideo>();
        AwakeVideo.Init(10f,AudioManager.GetInstance().MainVolume);
    }

    public void AwakeVideoDestroy()
    {
        if (AwakeVideo != null) Destroy(AwakeVideo.gameObject);
        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.MAINMENU));
    }
    
    #endregion
    
    #endregion
    
    #region GameUI

    [HideInInspector] public GameUI GameUI;
    
    public void GameUIInit()
    {
        if (GameUI==null) GameUI = Instantiate(GameUI_Prefab,Canvas.transform).GetComponent<GameUI>();
    }

    public void GameUIDestroy()
    {
        if (GameUI!=null) Destroy(GameUI.gameObject);
    }
    
    #endregion
    
    
    #region Notification
    /// <summary>
    /// 初始化通知系统
    /// </summary>
    
    public Notification NotificationInit(string text,float duration)
    {
        Notification notification = Instantiate(Notification_Prefab,Canvas.transform).GetComponent<Notification>();
        notification.Init(text, duration);
        return notification;
    }

    public void NotificationDestroy(Notification notification)
    {
        Destroy(notification.gameObject);
    }

    #endregion
}
