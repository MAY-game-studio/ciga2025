using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class UIManager : SingletonDontDestory<UIManager>
{
    #region SerializeField
    
    [Header("UI组件")]
    
    #region Canvas

    [Tooltip("画布")]
    [SerializeField] public Canvas Canvas;

    #endregion

    [Header("UI预制体")]

    #region Prefab

    [Tooltip("主菜单预制体")]
    [SerializeField] private GameObject MainMenu_Prefab;
    [Tooltip("游戏UI预制体")]
    [SerializeField] private GameObject GameUI_Prefab;
    [Tooltip("消息预制体")]
    [SerializeField] private GameObject Notification_Prefab;

    [Tooltip("启动视频预制体")]
    [SerializeField] private GameObject AwakeVideo_Prefab;

    #endregion

    [Header("精灵素材")]
    
    #region Sprite

    [Tooltip("精灵")]
    [SerializeField] public Sprite[] Sprite;

    #endregion

    #region Videos
    
    
    [Header("视频参数")]

    [Tooltip("视频时长")]
    [SerializeField] private float[] VideoDuration;

    #endregion

    #endregion
    
    #region Videos
    
    #region AwakeVideo

    [HideInInspector] public AwakeVideo AwakeVideo;

    public void AwakeVideoInit()
    {
        AwakeVideo = Instantiate(AwakeVideo_Prefab, Canvas.transform).GetComponent<AwakeVideo>();
        AwakeVideo.Init(VideoDuration[0],AudioManager.GetInstance().MainVolume);
    }

    public void AwakeVideoDestroy()
    {
        if (AwakeVideo != null) Destroy(AwakeVideo.gameObject);
        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.MAINMENU));
    }
    
    #endregion
    
    #endregion
    
    #region MainMenu

    [HideInInspector] public MainMenu MainMenu;
    
    public void MainMenuInit()
    {
        MainMenu = Instantiate(MainMenu_Prefab,Canvas.transform).GetComponent<MainMenu>();
    }

    public void MainMenuDestroy()
    {
        if (MainMenu!=null) Destroy(MainMenu.gameObject);
    }

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
