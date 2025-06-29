using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ChapterManager : SingletonDontDestory<ChapterManager>
{
    #region SerializeField
    
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
        HitAvailable = true;
        FlagComplete = false;
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
        HitAvailable = false;
        _currentTime = 0f;
        if (_currentChapter==1) MessageManager.GetInstance().Send(MessageTypes.AddNotification,new AddNotification("这将是一个可怕的夜晚……"));
        if (_currentChapter==2) MessageManager.GetInstance().Send(MessageTypes.AddNotification,new AddNotification("周围回荡着尖叫声……"));
        if (_currentChapter==3) MessageManager.GetInstance().Send(MessageTypes.AddNotification,new AddNotification("你对桌子使用了过肩摔，因为你觉得你做得到！"));
        if (_currentChapter==4) MessageManager.GetInstance().Send(MessageTypes.AddNotification,new AddNotification("你这个橡皮鸭，满脑子都想着压榨员工呢"));
        if (_currentChapter==5) MessageManager.GetInstance().Send(MessageTypes.AddNotification,new AddNotification("我从来没觉得上班有意思过"));
        yield return new WaitForSecondsRealtime(1f);
        UIManager.GetInstance().ChapterFinish(PassScore[CurrentChapter],Score);
        MessageManager.GetInstance().Send(MessageTypes.PlayMusic, new PlayMusic(MusicClip.BGMMainMenu));
        yield return new WaitForSecondsRealtime(1f);

        while (!Input.anyKeyDown)
        {
            yield return null;
        }
        MessageManager.GetInstance().Send(MessageTypes.AddNotification,new AddNotification("按任意键继续"));
        UIManager.GetInstance().GameUIDestroy();
        yield return null;
        if (_currentChapter==5)
            MessageManager.GetInstance().Send(MessageTypes.GameModeChange,new GameModeChange(GameModeType.MAINMENU));
        else
        {
            if (Score >= PassScore[CurrentChapter])
                MessageManager.GetInstance().Send(MessageTypes.ChapterStart, new ChapterStart(_currentChapter + 1));
            else
                MessageManager.GetInstance().Send(MessageTypes.ChapterStart, new ChapterStart(_currentChapter));
        }
    }
    
    void Start()
    {
        MessageRegister();   
    }

    private bool HitAvailable;
    
    void Update()
    {
        if (GameManager.GetInstance().GameModeType == GameModeType.CHAPTER&&!FlagComplete)
        {
            _currentTime += Time.deltaTime*1000f;
            if (_effectID < _effectNum && _currentTime >= _chapterEffectTime[_effectID] + _timeOffset)
            {
                float effectType = _chapterEffectType[_effectID];
                if (effectType != 0)
                {
                    MessageManager.GetInstance()
                        .Send(MessageTypes.PlaySound, new PlaySound((SoundClip)_currentChapter + 1));
                    UIManager.GetInstance().GameUI.SwitchFrame(1);
                }
                else
                {
//                    MessageManager.GetInstance().Send(MessageTypes.PlaySound, new PlaySound((SoundClip)_currentChapter + 8));
                    UIManager.GetInstance().GameUI.SwitchFrame(2);
                }
                UIManager.GetInstance().EffectInit();
                _effectID++;
            }

            if (_currentChapter == 4)
            {
                if (_currentTime is >= 19217 and <= 29132)
                {
                    UIManager.GetInstance().GameUI.chapterID = 6;
                }
                else
                {
                    UIManager.GetInstance().GameUI.chapterID = 4;
                }
            }
            if (_currentChapter == 5)
            {
                if (_currentTime is >= 18289 and <= 34461)
                {
                    UIManager.GetInstance().GameUI.chapterID = 7;
                }
                else
                {
                    UIManager.GetInstance().GameUI.chapterID = 5;
                }
            }
            if (_effectID > 0 && _effectID >= _effectNum && !FlagComplete)
            {
                FlagComplete = true;
                StartCoroutine(ChapterFinish());
            }
        }
        if (HitAvailable&&_currentChapter!=6&&Input.GetKeyDown(KeyCode.A))
        {
//            Debug.Log("A");
            UIManager.GetInstance().HandInit(0);
            UIManager.GetInstance().JudgeInit(TimeJudge());
        }
        if (HitAvailable&&Input.GetKeyDown(KeyCode.L))
        {
//            Debug.Log("L");
            UIManager.GetInstance().HandInit(1);
            UIManager.GetInstance().JudgeInit(TimeJudge());
        }
    }

    private IEnumerator ChapterStart(int id)
    {
/*        if (UIManager.GetInstance().GameUI == null)
        {
            while (UIManager.GetInstance().GameUI == null)
            {
                yield return null; // 每帧检查一次
            }

        }
*/
        if (id==1) MessageManager.GetInstance().Send(MessageTypes.AddNotification,new AddNotification("一大个老板正在接近！"));
        if (id==2) MessageManager.GetInstance().Send(MessageTypes.AddNotification,new AddNotification("变色龙同事出现了，愤怒爬上了你的脊背"));
        if (id==3) MessageManager.GetInstance().Send(MessageTypes.AddNotification,new AddNotification("你感到有个邪恶的东西在看着你"));
        if (id==4) MessageManager.GetInstance().Send(MessageTypes.AddNotification,new AddNotification("KPI从天而降！"));
        if (id==5) MessageManager.GetInstance().Send(MessageTypes.AddNotification,new AddNotification("办公室正在变得焦躁不安……"));
        
        UIManager.GetInstance().ChapterStart(id);
        MessageManager.GetInstance().Send(MessageTypes.PlaySound,new PlaySound(SoundClip.READYGO));
        yield return new WaitForSecondsRealtime(5f);
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
