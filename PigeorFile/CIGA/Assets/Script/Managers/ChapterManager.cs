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
    private int _effectNum,_effectID,_score;
    private float _timeOffset;
    public float TimeOffset
    {
        get => _timeOffset;
        set => _timeOffset = value;
    }
    
    #endregion

    private void ChapterInit(int id)
    {
        UIManager.GetInstance().GameUI.ChapterInit(id);
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

    public int TimeJudge()
    {
        for (int i = 0; i < _effectNum; i++)
        {
            if (_chapterEffectType[i] == 0)
            {
                float effectTime = _chapterEffectTime[i] + _timeOffset;
                if (Mathf.Abs(_currentTime - effectTime) <= 0.2f)//严判
                    return 2;
            }
        }
        for (int i = 0; i < _effectNum; i++)
        {
            if (_chapterEffectType[i] == 0)
            {
                float effectTime = _chapterEffectTime[i] + _timeOffset;
                if (Mathf.Abs(_currentTime - effectTime) <= 0.5f)//宽判
                    return 1;
            }
        }
        return 0; //miss
    }

    IEnumerator ChapterFinish()
    {
        yield return new WaitForSecondsRealtime(3f);
        UIManager.GetInstance().ChapterFinish(PassScore[CurrentChapter],_score);
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
                    Debug.Log("effect"+effectType);
//            HandleEffect(effectType); // 触发效果逻辑
                UIManager.GetInstance().EffectInit((int)effectType);
                _effectID++;
            }
            if (_effectID >= _effectNum)
            {
                StartCoroutine(ChapterFinish());
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("A");
            UIManager.GetInstance().HandInit(0);
            UIManager.GetInstance().JudgeInit(TimeJudge());
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("L");
            UIManager.GetInstance().HandInit(1);
            UIManager.GetInstance().JudgeInit(TimeJudge());
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            UIManager.GetInstance().JudgeInit(1);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            UIManager.GetInstance().JudgeInit(2);
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
            ChapterInit(msg.ID);
        }
    }
    #endregion
}
