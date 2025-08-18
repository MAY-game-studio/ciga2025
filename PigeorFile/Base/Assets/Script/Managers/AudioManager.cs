using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonDontDestory<AudioManager>
{
    #region SerializeField
    
    [Header("媒体组件")]
    [Tooltip("用于播放背景音乐的AudioSource")]
    [SerializeField] public AudioSource BGMSource;

    [Tooltip("用于播放音效的AudioSource")]
    [SerializeField] public AudioSource SFXSource;
    
    
    [Header("媒体素材片段")]
    [Tooltip("背景音乐")]
    [SerializeField] public AudioClip[] MusicClip;
    [Tooltip("音效")]
    [SerializeField] public AudioClip[] SoundClip;

    #endregion
    
    #region Property

    private float _mainVolume = 0.8f;
    public float MainVolume  //主音量
    {
        get => _mainVolume;
        set
        {
            _mainVolume = Mathf.Clamp01(value);
            BGMSource.volume = _mainVolume * MusicVolume;
            SFXSource.volume = _mainVolume * SoundVolume;
        }
    }
    
    private float _musicVolume = 0.8f;
    public float MusicVolume //音乐音量
    {
        get => _musicVolume;
        set {
            _musicVolume = Mathf.Clamp01(value);
            BGMSource.volume = MainVolume * _musicVolume;
        }
    }
    
    private float _soundVolume = 0.8f;
    public float SoundVolume //音效音量
    {
        get => _soundVolume;
        set
        {
            _soundVolume = Mathf.Clamp01(value); 
            SFXSource.volume = MainVolume * _soundVolume;
        }
    }
    
    private Coroutine _musicCoroutine; // 用于追踪当前正在运行的协程
    #endregion
    
    void Start()
    {
        MessageInit();
    }
    
    #region MessageHandler
    private void MessageInit()
    {
        MessageManager.GetInstance().Register(MessageTypes.PlayMusic,OnPlayMusic);
        MessageManager.GetInstance().Register(MessageTypes.PlaySound,OnPlaySound);
    }
    
    IEnumerator SwitchMusic(MusicClip musicClip,float duration) //背景音乐渐变切换
    {
        float timer = 0f, startVolume = BGMSource.volume, targetVolume = MainVolume * MusicVolume;
        
        while (timer < duration) //原音乐淡出
        {
            timer += Time.deltaTime;
            BGMSource.volume = Mathf.Lerp(startVolume, 0f, timer / duration);
            yield return null; //等待下一帧
        }
        BGMSource.volume = 0f; //确保完全静音
        BGMSource.Stop();
        BGMSource.clip = MusicClip[(int)musicClip]; //切换音乐
        BGMSource.Play();
        timer = startVolume = 0f;
        while (timer < duration) //新音乐淡入
        {
            timer += Time.deltaTime;
            BGMSource.volume = Mathf.Lerp(startVolume, targetVolume, timer / duration);
            yield return null; // 等待下一帧
        }
        
        BGMSource.volume = targetVolume; //确保音量在最后被精确设置
        _musicCoroutine = null; // 任务完成，清空追踪
    }
    
    public void OnPlayMusic(Message message)
    {
        if (message is PlayMusic msg)
        {
            if (_musicCoroutine != null) StopCoroutine(_musicCoroutine);// 如果当前已有切换任务，先停掉旧的
            _musicCoroutine = StartCoroutine(SwitchMusic(msg.MusicClip, msg.Duration));
        }
    }

    public void OnPlaySound(Message message)
    {
        if (message is PlaySound msg)
        {
            SFXSource.PlayOneShot(SoundClip[(int)msg.SoundClip], MainVolume * SoundVolume);
        }
    }

    #endregion
}
