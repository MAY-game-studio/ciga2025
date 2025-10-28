using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : SingletonDontDestroy<NotificationManager>
{
    #region SerializeField
    
    [Header("消息参数设置")]
    
    [Tooltip("消息显示时间")]
    [SerializeField] private float DefaultDuration;
    
    [Tooltip("最大显示的消息数")]
    [Range(2,8)]
    [SerializeField] private int MaxNotifications;

    [Tooltip("显示消息的间隔")]
    [Range(0.5f,3f)]
    [SerializeField] private float NotificationInterval;

    #endregion
    
    #region Property

    private readonly List<Notification> _notificationList = new List<Notification>(); //当前显示的消息
    private readonly List<string> _notificationTextList = new List<string>(); //寄存的消息内容
    private float _counting; //实例化消息间隔计时器
    
    #endregion

    public void DeleteNotification(Notification notification) //删除指定的消息
    {
        if (_notificationList.Remove(notification))
            UIManager.GetInstance().NotificationDestroy(notification); // 移除成功后销毁对应的UI元素
    }

    private void NewNotification() //从消息缓存中实例化一条
    {
        foreach (var tmp in _notificationList)
            tmp.RePosition(-1);
        Notification notification = UIManager.GetInstance().NotificationInit(_notificationTextList[0],DefaultDuration);
        _notificationTextList.RemoveAt(0);
        _notificationList.Add(notification);
        _counting = 0f;
    }

    private void Start()
    {
        MessageInit();
    }

    private void Update()
    {
        _counting += Time.deltaTime;
        if (_counting < NotificationInterval) return;
        if (_notificationTextList.Count > 0 && _notificationList.Count < MaxNotifications)
            NewNotification();
    }

    #region MessageHandler
    private void MessageInit()
    {
        MessageManager.GetInstance().Register(MessageTypes.AddNotification,OnAddNotification);
    }

    private void OnAddNotification(Message message)
    {
        if (message is AddNotification msg)
        {
            _notificationTextList.Add(msg.Text);
        }
    }
    #endregion
}