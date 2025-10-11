using System;
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

    [Tooltip("UI面板预制体列表")]
    [SerializeField] private List<UIPrefabBase> PanelPrefabs;
    
    [Tooltip("消息预制体")]
    [SerializeField] private GameObject Notification_Prefab;

    [Tooltip("启动视频预制体")]
    [SerializeField] private GameObject AwakeVideo_Prefab;

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

    private Dictionary<Type, UIPrefabBase> _prefabInstances = new Dictionary<Type, UIPrefabBase>();

    private T FindPrefab<T>() where T : UIPrefabBase //在 PanelPrefabs 列表中查找匹配的预制体。
    {
        foreach (UIPrefabBase prefab in PanelPrefabs) //遍历在Inspector中配置的所有UI面板预制体
        {
            if (prefab is T)
                return prefab as T;
        }
        return null;
    }
    
    public void PrefabInit<T>(Action<T> onInitCallback = null) where T : UIPrefabBase
    {
        Type prefabType = typeof(T);
        if (_prefabInstances.TryGetValue(prefabType, out UIPrefabBase existingInstance)) return;
        T prefab = FindPrefab<T>(); // 查找预制体
        if (prefab == null) return;
        T panelInstance = Instantiate(prefab, Canvas.transform); 
        _prefabInstances.Add(prefabType, panelInstance); // 实例化并存入字典
        onInitCallback?.Invoke(panelInstance); // 对新创建的实例执行回调
    }
    
    public void PrefabDestroy<T>(Action<T> onDestroyCallback = null) where T : UIPrefabBase
    {
        Type prefabType = typeof(T);
        if (!_prefabInstances.TryGetValue(prefabType, out UIPrefabBase instanceToDestroy)) return;
        onDestroyCallback?.Invoke(instanceToDestroy as T); //先实现回调
        Destroy(instanceToDestroy.gameObject);
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
        AwakeVideo = Instantiate(AwakeVideo_Prefab, Canvas.transform).GetComponent<AwakeVideo>();
        AwakeVideo.Init();
    }

    public void AwakeVideoDestroy()
    {
        if (AwakeVideo != null) Destroy(AwakeVideo.gameObject);
        MessageManager.GetInstance().Send(MessageTypes.SwitchMouseMode,new SwitchMouseMode(MouseMode.DEFAULT));
        MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.MAINMENU));
    }
    
    #endregion
    
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
