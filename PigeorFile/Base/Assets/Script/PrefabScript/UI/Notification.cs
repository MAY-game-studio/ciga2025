using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class Notification : MonoBehaviour
{
    #region SerializeField

    [Header("消息参数设置")]
    [Tooltip("间隔之间的空隙")]
    [SerializeField] private float NotificationSpace;
    [Tooltip("动画时长")]
    [SerializeField] private float AnimDuration;

    [Header("UI组件")]
    [SerializeField] private TextMeshProUGUI text;
        
    #endregion

    #region ProPerty

    private float countdown,time;
    
    #endregion
    
    public void Init(string notification, float duration)
    {
        text.text = notification;
        countdown = duration;
    }

    public void RePosition(int num)
    {
        Vector3 targetPos = transform.localPosition + new Vector3(0, num * NotificationSpace, 0);
        transform.DOLocalMove(targetPos, AnimDuration).SetEase(Ease.OutQuad);
    }
    
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown<0) NotificationManager.GetInstance().DeleteNotification(this);
    }
}