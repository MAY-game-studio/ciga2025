using System.Collections;
using UnityEngine;

public abstract class VideoPrefabBase : MonoBehaviour
{
    #region SerializeField

    [Header("视频通用参数")]

    [Header("媒体组件")]
    
    [Tooltip("视频媒体组件")]
    [SerializeField] public AudioSource AudioSource;
    
    [Header("视频参数")]
    
    [Tooltip("可跳过标记")]
    [SerializeField] public bool FlagSkip;

    [Tooltip("视频时长")]
    [SerializeField] protected float Duration;

    #endregion
    
    private IEnumerator Play()
    {
        float endTime = Time.time + Duration;
        while (Time.time < endTime)
        {
            if (FlagSkip && Input.GetKeyDown(GameManager.GetInstance().GameSettingData.Skip)) break;
            yield return null; 
        }
        Finish();
    }

    public virtual void Init() // 初始化播放视频并设置音量
    {
        StartCoroutine(Play());
        AudioSource.volume = AudioManager.GetInstance().MainVolume;
    }
    
    protected abstract void Finish(); //子类需要实现视频结束后的逻辑
}