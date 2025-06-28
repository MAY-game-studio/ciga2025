using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ChapterManager : SingletonDontDestory<ChapterManager>
{
    #region SerializeField
    
    [Header("章节谱")]
    [SerializeField] private string[] Chapter;

    [Header("分数需求")]
    [SerializeField] private int[] PassScore; 
    
    #endregion
    
    #region Property

    private int _currentChapter;
    public int CurrentChapter
    {
        get => _currentChapter;
        private set => _currentChapter = value;
    }
    
    private float _currentTime;
    public float CurrentTime
    {
        get => _currentTime;
        private set => _currentTime = value;
    }

    private float[] _chapterEffectTime;
    private float[] _chapterEffectType;
    private int _effectNum,_effectID,Score;
    private float _timeOffset;
    public float TimeOffset
    {
        get => _timeOffset;
        set => _timeOffset = value;
    }
    
    #endregion

    private void ChapterInit(int id)
    {
        // 从 Resources 加载章节文本
        id = 0;
        TextAsset chapterFile = Resources.Load<TextAsset>($"Chapter/Chapter{id}");
        if (chapterFile == null)
        {
            Debug.LogError($"无法加载章节文件 Chapters/Chapter{id}.txt");
            return;
        }

        string chapterData = chapterFile.text;
        string[] lines = chapterData.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        _effectNum = lines.Length;
        _chapterEffectTime = new float[_effectNum];
        _chapterEffectType = new float[_effectNum];

        for (int i = 0; i < _effectNum; i++)
        {
            string[] parts = lines[i].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2 &&
                float.TryParse(parts[0], out float time) &&
                float.TryParse(parts[1], out float effect))
            {
                _chapterEffectTime[i] = time;
                _chapterEffectType[i] = effect;
            }
            else
            {
                Debug.LogWarning($"第 {i} 行格式错误: {lines[i]}");
            }
        }
        _currentChapter = id;
        _effectID = 0;
        MessageManager.GetInstance().Send(MessageTypes.PlayMusic,new PlayMusic((MusicClip)id));
    }

    public bool TimeJudge()
    {
        bool inZeroEffectWindow = false;
        for (int i = 0; i < _effectNum; i++)
        {
            if (_chapterEffectType[i] == 0)
            {
                float effectTime = _chapterEffectTime[i] + _timeOffset;
                if (Mathf.Abs(_currentTime - effectTime) <= 0.5f)//判定区域
                {
                    inZeroEffectWindow = true;
                    break;
                }
            }
        }
        return inZeroEffectWindow;
    }

    IEnumerator ChapterFinish()
    {
        yield return new WaitForSecondsRealtime(3f);
        UIManager.GetInstance().ChapterFinish(PassScore[CurrentChapter],Score);
        MessageManager.GetInstance().Send(MessageTypes.PlayMusic, new PlayMusic(MusicClip.BGMMainMenu));
    }
    
    void Start()
    {
        MessageRegister();   
    }
    
    void Update()
    {
        if (GameManager.GetInstance().GameModeType == GameModeType.DEFAULT)
        {
            _currentTime += Time.deltaTime;
            if (_effectID < _effectNum && _currentTime >= _chapterEffectTime[_effectID] + _timeOffset)
            {
                float effectType = _chapterEffectType[_effectID];
                if (effectType!=0)
                    Debug.Log(effectType);
//            HandleEffect(effectType); // 触发效果逻辑
                _effectID++;
            }
            if (_effectID >= _effectNum)
            {
                StartCoroutine(ChapterFinish());
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            UIManager.GetInstance().HitInit(0);
            if (TimeJudge())
            {
                
                UIManager.GetInstance().JudgeInit(1);
            }
            else
                UIManager.GetInstance().JudgeInit(0);
            
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            UIManager.GetInstance().HitInit(1);
            if (TimeJudge())
            {
                UIManager.GetInstance().JudgeInit(1);
            }
            else
                UIManager.GetInstance().JudgeInit(0);
        }
        
    }
    
    
    #region Message

    protected void MessageRegister()
    {
        MessageManager.GetInstance().Register(MessageTypes.ChapterStart, OnChapterStart);
    }

    public void OnChapterStart(Message message)
    {
        if (message is ChapterStart msg)
        {
            MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.DEFAULT));
            ChapterInit(msg.ID);
        }
    }
    #endregion
}
