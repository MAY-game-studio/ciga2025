using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonDontDestory<AudioManager>
{
    #region SerializeField
    
    [Header("媒体组件")]
    [Tooltip("主媒体组件")]
    [SerializeField] public AudioSource Audio;
    
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
        set {
            _mainVolume = Mathf.Clamp01(value);
            Audio.volume = value * MusicVolume;
        }
    }
    
    private float _musicVolume = 0.8f;
    public float MusicVolume //音乐音量
    {
        get => _musicVolume;
        set {
            _musicVolume = Mathf.Clamp01(value);
            Audio.volume = MainVolume * value;
        }
    }
    
    private float _soundVolume = 0.8f;
    public float SoundVolume //音效音量
    {
        get => _soundVolume;
        set => _soundVolume = Mathf.Clamp01(value);
    }

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
    
    IEnumerator SwitchMusic(MusicClip musicClip)//背景音乐渐变切换
    {
        while (Audio.volume > 0.01f)
        {
            Audio.volume -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        Audio.Stop();
        Audio.clip = MusicClip[(int)musicClip]; //切换音乐
        Audio.Play();
        while (Audio.volume < MainVolume*MusicVolume)
        {
            Audio.volume += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }
    
    public void OnPlayMusic(Message message)
    {
        if (message is PlayMusic msg)
        {
            StartCoroutine(SwitchMusic(msg.MusicClip));
        }
    }

    public void OnPlaySound(Message message)
    {
        if (message is PlaySound msg)
        {
            Audio.PlayOneShot(SoundClip[(int)msg.SoundClip], MainVolume * SoundVolume / MusicVolume);
        }
    }

    #endregion
}
