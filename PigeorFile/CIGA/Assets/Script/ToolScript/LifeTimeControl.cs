using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTimeControl : MonoBehaviour
{
    #region SerializeField

    [SerializeField] private bool type; 
    
    [Header("生命时间")]
    [SerializeField] private float lifeTime = 0.5f;
    
    #endregion

    IEnumerator LifeCounting()
    {
        yield return new WaitForSeconds(lifeTime);
        Transform stand = transform.Find("StandObject");
        if (stand != null)
            if (!type)
                Destroy(stand.gameObject);
            else
                stand.gameObject.SetActive(false);
        else
            if (!type)
                Destroy(gameObject);
            else
                gameObject.SetActive(false);

    }
    
    void Start()
    {
        StartCoroutine(LifeCounting());
    }

}
