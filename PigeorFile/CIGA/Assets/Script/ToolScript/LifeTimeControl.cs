using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTimeControl : MonoBehaviour
{
    #region SerializeField
    
    [Header("生命时间")]
    [SerializeField] private float lifeTime = 0.5f;
    
    #endregion

    IEnumerator LifeCounting()
    {
        yield return new WaitForSeconds(lifeTime);
        Transform stand = transform.Find("StandObject");
        if (stand != null)
            Destroy(stand.gameObject);
        else
            Destroy(gameObject);

    }
    
    void Start()
    {
        StartCoroutine(LifeCounting());
    }

}
