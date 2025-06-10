using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeVideo : MonoBehaviour
{
    #region SerializeField
    
    [SerializeField] public AudioSource AudioSource;
    [SerializeField] public bool FlagSkip; //可跳过标记
    
    #endregion
    
    private void FinishPlay()
    {
        UIManager.GetInstance().AwakeVideoDestroy();
    }
    private IEnumerator PlayAnim(float countdown)
    {
        yield return new WaitForSeconds(countdown);
        FinishPlay();
    }

    public void Init(float countdown,float volume)
    {
        StartCoroutine(PlayAnim(countdown));
        AudioSource.volume = volume;
    }

    void Update()
    {
        if (FlagSkip && Input.GetKeyDown(GameManager.GetInstance().GameSettingData.Skip)) FinishPlay();
    }
}