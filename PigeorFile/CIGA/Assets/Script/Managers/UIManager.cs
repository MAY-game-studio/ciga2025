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
    [Tooltip("消息预制体")]
    [SerializeField] private GameObject Mouse_Prefab;


    [Tooltip("启动视频预制体")]
    [SerializeField] private GameObject AwakeVideo_Prefab;

    #endregion

    [Header("Chapter预制体")]

    #region Chapter
    
    [SerializeField] private GameObject[] Hand_Prefab;

    [SerializeField] private GameObject[] Hit_Prefab;

    [SerializeField] private GameObject ChapterFinish_Prefab;

    #endregion
    
    [Header("精灵素材")]
    
    #region Sprite

    [Tooltip("鼠标")]
    [SerializeField] public Sprite[] MouseSprite;
    [Tooltip("背景")]
    [SerializeField] public Sprite[] ChapterBGSprite;
    [Tooltip("角色")]
    [SerializeField] public Sprite[] CharacterSprite;
    [SerializeField] public Sprite[] CharacterHitSprite;
    
    [SerializeField] public Sprite[] FrameSprite;
    [SerializeField] public Sprite[] ChapterDeskSprite;
    [SerializeField] public Sprite[] ChapterReadyGoSprite;
    [SerializeField] public Sprite[] ChapterFinishSprite;
    
    #endregion

    #region Videos
    
    
    [Header("视频参数")]

    [Tooltip("视频时长")]
    [SerializeField] private float[] VideoDuration;

    #endregion

    #endregion

    public void SetResolution()
    {
        Screen.SetResolution((int)GameManager.GetInstance().GameSettingData.ResolutionRatio.x,
            (int)GameManager.GetInstance().GameSettingData.ResolutionRatio.y,
            GameManager.GetInstance().GameSettingData.ScreenMode);
        Canvas.scaleFactor = GameManager.GetInstance().GameSettingData.ResolutionRatio.x / 1920f;//ui缩放
    }

    #region Mouse

    [HideInInspector] public Mouse Mouse;

    public void MouseInit()
    {
        if (Mouse==null) Mouse = Instantiate(Mouse_Prefab, Canvas.transform).GetComponent<Mouse>();
        Mouse.transform.SetAsLastSibling();
    }

    public void MouseDestroy()
    {
        if (Mouse!=null) Destroy(Mouse.gameObject);
    }

    #endregion
    
    #region Videos
    
    #region AwakeVideo

    [HideInInspector] public AwakeVideo AwakeVideo;

    public void AwakeVideoInit()
    {
        MessageManager.GetInstance().Send(MessageTypes.SwitchMouseMode,new SwitchMouseMode(MouseMode.HIDE));
        AwakeVideo = Instantiate(AwakeVideo_Prefab, Canvas.transform).GetComponent<AwakeVideo>();
        AwakeVideo.Init(VideoDuration[0],AudioManager.GetInstance().MainVolume);
    }

    public void AwakeVideoDestroy()
    {
        if (AwakeVideo != null) Destroy(AwakeVideo.gameObject);
        MessageManager.GetInstance().Send(MessageTypes.SwitchMouseMode,new SwitchMouseMode(MouseMode.DEFAULT));
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
        if (GameUI==null)
            GameUI = Instantiate(GameUI_Prefab,Canvas.transform).GetComponent<GameUI>();
    }

    public void GameUIDestroy()
    {
        if (GameUI!=null) Destroy(GameUI.gameObject);
    }
    
    #endregion

    #region Effect
    
    public void EffectInit()
    {
        GameUI.CharacterEffect();
    }

    public void HandInit(int id)
    {
        Instantiate(Hand_Prefab[id],Canvas.transform);
    }

    public void JudgeInit(int id)
    {
        Instantiate(Hit_Prefab[id],Canvas.transform);
        GameUI.CharacterSwitch(id == 0 ? 1 : 2);
    }

    public void ChapterStart(int id)
    {
        GameUI.ReadyGoInit(ChapterReadyGoSprite[id]);
    }

    public void ChapterFinish(int scoreLimit,int score)
    {
        if (score >= scoreLimit)
            GameUI.FinishInit(ChapterFinishSprite[ChapterManager.GetInstance().CurrentChapter*2-1]);
        else
            GameUI.FinishInit(ChapterFinishSprite[ChapterManager.GetInstance().CurrentChapter*2-2]);
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
