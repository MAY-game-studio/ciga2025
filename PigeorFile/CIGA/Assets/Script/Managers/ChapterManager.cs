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
    private int _effectNum, _effectID;
    private bool FlagComplete;
    public int Score;
    private float _timeOffset;
    public float TimeOffset
    {
        get => _timeOffset;
        set => _timeOffset = value;
    }
    
    #endregion

    private void ChapterInit(int id)
    {
        FlagComplete = false;
        UIManager.GetInstance().GameUI.ChapterInit(id);
        // 从 Resources 加载章节文本
        TextAsset chapterFile = Resources.Load<TextAsset>($"Chapter/Chapter{id}");
        if (chapterFile == null)
        {
            Debug.LogError($"无法加载章节文件 Chapters/Chapter{id}.txt");
            return;
        }

        string chapterData = chapterFile.text;
        string[] lines = chapterData.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        _effectNum = lines.Length;
        FlagComplete = false;
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

    private int TimeJudge()
    {
        for (int i = 0; i < _effectNum; i++)
        {
            if (_chapterEffectType[i] == 0)
            {
                float effectTime = _chapterEffectTime[i] + _timeOffset;
                if (Mathf.Abs(_currentTime - effectTime) <= 250f)//严判
                    return 2;
            }
        }
        for (int i = 0; i < _effectNum; i++)
        {
            if (_chapterEffectType[i] == 0)
            {
                float effectTime = _chapterEffectTime[i] + _timeOffset;
                if (Mathf.Abs(_currentTime - effectTime) <= 500f)//宽判
                    return 1;
            }
        }
        return 0; //miss
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
        if (GameManager.GetInstance().GameModeType == GameModeType.CHAPTER)
        {
            _currentTime += Time.deltaTime*1000f;
            if (_effectID < _effectNum && _currentTime >= _chapterEffectTime[_effectID] + _timeOffset)
            {
                float effectType = _chapterEffectType[_effectID];
                if (effectType!=0)
                    MessageManager.GetInstance().Send(MessageTypes.PlaySound, new PlaySound((SoundClip)_currentChapter+1));
//                else
//                    MessageManager.GetInstance().Send(MessageTypes.PlaySound, new PlaySound((SoundClip)_currentChapter+8));
                UIManager.GetInstance().EffectInit();
                _effectID++;
            }

            if (_effectID > 0 && _effectID >= _effectNum && !FlagComplete)
            {
                FlagComplete = true;
                StartCoroutine(ChapterFinish());
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
//            Debug.Log("A");
            UIManager.GetInstance().HandInit(0);
            UIManager.GetInstance().JudgeInit(TimeJudge());
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
//            Debug.Log("L");
            UIManager.GetInstance().HandInit(1);
            UIManager.GetInstance().JudgeInit(TimeJudge());
        }
/*
        if (Input.GetKeyDown(KeyCode.Alpha1))
            UIManager.GetInstance().JudgeInit(1);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            UIManager.GetInstance().JudgeInit(2);
            */
    }

    private IEnumerator ChapterStart(int id)
    {
        UIManager.GetInstance().ChapterStart();
        yield return new WaitForSecondsRealtime(3f);
        ChapterInit(id);
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
            MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.CHAPTER));
//            ChapterInit(msg.ID);
            StartCoroutine(ChapterStart(msg.ID));
        }
    }
    #endregion
}
