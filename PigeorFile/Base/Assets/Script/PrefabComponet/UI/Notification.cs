using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Notification : MonoBehaviour
{
    #region SerializeField

    [SerializeField] private TextMeshProUGUI text;
//    [SerializeField] public GameObject NotificationUI;
        
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
        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x,
            gameObject.transform.localPosition.y + num * 75f, gameObject.transform.localPosition.z);
    }
    
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown<0) NotificationManager.GetInstance().DeleteNotification(gameObject);
    }
}