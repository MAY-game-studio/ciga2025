using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Message
{
    protected Message()
    {
    }

    public Message(MessageTypes Type)
    {
        type = Type;
    }

    public MessageTypes type { get; private set; }
}


/// <summary>
/// 游戏模式切换时使用,应只在GameManager接受再调用各manager接口
/// </summary>

public class GameModeChange : Message
{
    public GameModeChange(GameModeType gameModeType) : base(MessageTypes.GameModeChange)
    {
        GameModeType = gameModeType;
    }
    public GameModeType GameModeType;
}

public class PlayMusic : Message
{
    public PlayMusic(MusicClip musicClip, float duration = 1.0f) : base(MessageTypes.PlayMusic)
    {
        MusicClip = musicClip;
        Duration = duration;
    }
    public MusicClip MusicClip;
    public float Duration;
}

public class PlaySound : Message
{
    public PlaySound(SoundClip soundClip) : base(MessageTypes.PlaySound)
    {
        SoundClip = soundClip;
    }
    public SoundClip SoundClip;
}

/// <summary>
/// 设置更新
/// </summary>
 
public class SettingDataUpdate : Message
{
    public SettingDataUpdate() : base(MessageTypes.SettingDataUpdate)
    {
    }
}

/// <summary>
/// 存档更新
/// </summary>
 
public class SaveDataUpdate : Message
{
    public SaveDataUpdate() : base(MessageTypes.SaveDataUpdate)
    {
    }
}

/// <summary>
/// 新建通知
/// </summary>
public class AddNotification : Message
{
    public AddNotification(string text,float duration=-1f) : base(MessageTypes.AddNotification)
    {
        Text = text;
    }
    public string Text;
    public float Duration;
}

/// <summary>
/// 修改鼠标样式
/// </summary>
public class SwitchMouseMode : Message
{
    public SwitchMouseMode(MouseMode mouseMode) : base(MessageTypes.SwitchMouseMode)
    {
        MouseMode = mouseMode;
    }
    public MouseMode MouseMode;
}


/// <summary>
/// 鼠标旁显示细节信息，多个框体的细节由*分割
/// </summary>
public class ShowDetail : Message
{
    public ShowDetail(string detail) : base(MessageTypes.ShowDetail)
    {
        Detail = detail;
    }
    public string Detail;
}

public class MessageListener //消息监听器
{
    public UnityAction<Message> Action;
    public int Priority;
    public MessageTemporaryType TemporaryType;
    public MessageListener(UnityAction<Message> action, int priority, MessageTemporaryType tempType)
    {
        Action = action;
        Priority = priority;
        TemporaryType = tempType;
    }
}

public class MessageManager : SingletonDontDestory<MessageManager>
{
    private Dictionary<MessageTypes, List<MessageListener>> listeners;

    private void OnEnable()
    {
        listeners = new Dictionary<MessageTypes, List<MessageListener>>();
    }

    /// <summary>
    /// 注册消息,默认无优先级且临时类型为Default
    /// </summary>
    public void Register(MessageTypes messageType, UnityAction<Message> action, int priority = 0,
        MessageTemporaryType tempType = MessageTemporaryType.Default)
    {
        if (!listeners.ContainsKey(messageType))
            listeners[messageType] = new List<MessageListener>();
        listeners[messageType].Add(new MessageListener(action, priority, tempType));
        listeners[messageType].Sort((a, b) => b.Priority.CompareTo(a.Priority)); // 按优先级降序排列
    }

    /// <summary>
    /// 移除消息
    /// </summary>
    public void Remove(MessageTypes messageType, UnityAction<Message> action)
    {
        if (!listeners.ContainsKey(messageType)) return;
        listeners[messageType].RemoveAll(listener => listener.Action == action);
    }

    /// <summary>
    /// 发送消息,且在控制台输出一句log
    /// </summary>
    public void Send(MessageTypes messageType, Message message)
    {
#if UNITY_EDITOR
        Debug.Log(messageType);
#endif
        if (!listeners.TryGetValue(messageType, out var listenerList)) return;
        foreach (var listener in listenerList)
        {
            try
            {
                listener.Action?.Invoke(message);// 尝试执行回调函数
            }
            catch (System.Exception e)// 如果发生任何异常，捕获它
            {
                // 在控制台打印详细的错误信息，包括哪个消息类型和哪个监听者出错了
                Debug.LogError($"Error executing listener for message [{messageType}].\n" +
                               $"Listener: {listener.Action.Method.Name} in {listener.Action.Target.GetType().Name}\n" +
                               $"Exception: {e.Message}\n{e.StackTrace}");
            }
        }
    }

    public void Clear(MessageTemporaryType tempType = MessageTemporaryType.Default)
    {
        if (tempType == MessageTemporaryType.Default) // 默认情况：清除所有监听器
        {
            listeners.Clear();
            return;
        }
        var messageTypes = listeners.Keys.ToArray(); // 先复制key集合
        foreach (var type in messageTypes) // 遍历所有消息类型
        {
            // 移除指定临时类型的监听器
            listeners[type].RemoveAll(l => l.TemporaryType == tempType);
            // 如果该类型下已无监听器，移除整个条目
            if (listeners[type].Count == 0) listeners.Remove(type);
        }
    }
}