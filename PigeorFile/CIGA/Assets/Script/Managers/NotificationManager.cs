using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : SingletonDontDestory<NotificationManager>
{
    #region SerializeField
    
    [Header("消息参数设置")]
    
    [Tooltip("消息显示时间")]
    [SerializeField] private float DefultDuration;
    
    [Tooltip("最大显示的消息数")]
    [Range(2,8)]
    [SerializeField] private int MaxNotifications;

    [Tooltip("显示消息的间隔")]
    [Range(0.5f,3f)]
    [SerializeField] private float NotificationInterval;

    #endregion
    
    #region Property

    private List<Notification> notificationList = new List<Notification>(); //当前显示的消息
    private List<string> notificationTextList = new List<string>(); //寄存的消息内容
    private float _counting = 0f; //实例化消息间隔计时器
    #endregion

    public void DeleteNotification(GameObject notification)
    {
        int index = notificationList.FindIndex(n => n.gameObject == notification);
        Notification tmp = notificationList[index];
        notificationList.RemoveAt(index);
        UIManager.GetInstance().NotificationDestroy(tmp);
    }

    public void NewNotification() //从消息缓存中实例化一条
    {
        foreach (var tmp in notificationList)
            tmp.RePosition(-1);
        Notification notification = UIManager.GetInstance().NotificationInit(notificationTextList[0],DefultDuration);
        notificationTextList.RemoveAt(0);
        notificationList.Add(notification);
        _counting = 0f;
    }
    
    void Start()
    {
        MessageInit();
    }

    void Update()
    {
        _counting += Time.deltaTime;
        if (_counting < NotificationInterval) return;
        if (notificationTextList.Count > 0 && notificationList.Count < MaxNotifications)
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
            notificationTextList.Add(msg.Text);
        }
    }
    #endregion
}