using DG.Tweening;
using TMPro;
using UnityEngine;

public class Notification : MonoBehaviour
{
    #region SerializeField

    [Header("消息参数设置")]
    [Tooltip("间隔之间的空隙")]
    [SerializeField] private float NotificationSpace;
    [Tooltip("动画时长")]
    [SerializeField] private float AnimDuration;
    
    [Header("UI组件")]
    [SerializeField] private TextMeshProUGUI Text;
        
    #endregion

    #region property
    
    private float _countdown,_time;
    
    #endregion
    
    public void Init(string notification, float duration)
    {
        Text.text = notification;
        _countdown = duration;
    }

    public void RePosition(int num)
    {
        Vector3 targetPos = transform.localPosition + new Vector3(0, num * NotificationSpace, 0);
        transform.DOLocalMove(targetPos, AnimDuration).SetEase(Ease.OutQuad);
    }
    
    private void Update()
    {
        _countdown -= Time.deltaTime;
        if (_countdown<0) NotificationManager.GetInstance().DeleteNotification(this);
    }
}