using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeVideo : MonoBehaviour
{
    #region SerializeField

    [Header("媒体组件")]
    
    [Tooltip("视频媒体组件")]
    [SerializeField] public AudioSource AudioSource;
    
    [Header("视频参数")]
    
    [Tooltip("可跳过标记")]
    [SerializeField] public bool FlagSkip;
    
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
        StartCoroutine(PlayAnim(8f));
        AudioSource.volume = volume;
    }

    void Update()
    {
        if (FlagSkip && Input.GetKeyDown(GameManager.GetInstance().GameSettingData.Skip)) FinishPlay();
    }
}