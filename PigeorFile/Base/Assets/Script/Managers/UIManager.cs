using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : SingletonDontDestroy<UIManager>
{
    #region SerializeField
    
    [Header("UI组件")]
    
    #region Canvas

    [Tooltip("画布")]
    [SerializeField] public Canvas Canvas;

    #endregion

    [Header("UI预制体")]

    #region Prefab

    [Tooltip("UI面板预制体列表")]
    [SerializeField] private List<UIPrefabBase> PanelPrefabs;
    
    [Tooltip("消息预制体")]
    [SerializeField] private GameObject NotificationPrefab;

    [Header("视频预制体")]

    [Tooltip("启动视频")]
    [SerializeField] private GameObject AwakeVideoPrefab;

    #endregion

    [Header("精灵素材")]
    
    #region Sprite

    [Tooltip("鼠标")]
    [SerializeField] public Sprite[] MouseSprite;
    [Tooltip("精灵")]
    [SerializeField] public Sprite[] Sprite;

    #endregion

    public void SetResolution()
    {
        Screen.SetResolution((int)GameManager.GetInstance().GameSettingData.ResolutionRatio.x,
            (int)GameManager.GetInstance().GameSettingData.ResolutionRatio.y,
            GameManager.GetInstance().GameSettingData.ScreenMode);
    }
    #endregion
    
    #region UIPrefab

    private readonly Dictionary<Type, UIPrefabBase> _prefabInstances = new();

    private T FindPrefab<T>() where T : UIPrefabBase //在 PanelPrefabs 列表中查找匹配的预制体。
    {
        return PanelPrefabs.OfType<T>().FirstOrDefault();
    }
    
    public void PrefabInit<T>(Action<T> onInitCallback = null) where T : UIPrefabBase //初始化一个UI预制体实例。
    {
        Type prefabType = typeof(T);
        if (_prefabInstances.TryGetValue(prefabType, out UIPrefabBase existingInstance)) return;
        T prefab = FindPrefab<T>(); // 查找预制体
        if (!prefab) return;
        T instance = Instantiate(prefab, Canvas.transform); 
        _prefabInstances.Add(prefabType, instance); // 实例化并存入字典
        onInitCallback?.Invoke(instance); // 对新创建的实例执行回调
    }
    
    public void PrefabDestroy<T>(Action<T> onDestroyCallback = null) where T : UIPrefabBase
    {
        Type prefabType = typeof(T);
        if (!_prefabInstances.TryGetValue(prefabType, out UIPrefabBase instance)) return;
        onDestroyCallback?.Invoke(instance as T); //先实现回调
        instance.gameObject.TryDestroy();
        _prefabInstances.Remove(prefabType); // 从字典中移除记录
    }
    
    public T GetPrefab<T>() where T : UIPrefabBase // 获取一个的UI预制体实例。
    {
        Type prefabType = typeof(T);
        if (_prefabInstances.TryGetValue(prefabType, out UIPrefabBase instance)) return instance as T;// 尝试从字典中获取实例
        return null;
    }
    
    #endregion
    
    #region Videos
    
    #region AwakeVideo

    [HideInInspector] public AwakeVideo AwakeVideo;

    public void AwakeVideoInit()
    {
        MessageManager.GetInstance().Send(MessageTypes.SwitchMouseMode,new SwitchMouseMode(MouseMode.HIDE));
        AwakeVideo = Instantiate(AwakeVideoPrefab, Canvas.transform).GetComponent<AwakeVideo>();
        AwakeVideo.Init();
    }

    public void AwakeVideoDestroy()
    {
        if (AwakeVideo) Destroy(AwakeVideo.gameObject);
        MessageManager.GetInstance().Send(MessageTypes.SwitchMouseMode,new SwitchMouseMode(MouseMode.DEFAULT));
        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.MAINMENU));
    }
    
    #endregion
    
    #endregion
    
    #region Notification
    
    public Notification NotificationInit(string text,float duration)
    {
        Notification notification = Instantiate(NotificationPrefab,Canvas.transform).GetComponent<Notification>();
        notification.Init(text, duration);
        return notification;
    }

    public void NotificationDestroy(Notification notification)
    {
        Destroy(notification.gameObject);
    }

    #endregion
}
