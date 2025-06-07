using System;
using System.Collections;
using System.Collections.Generic;
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
/// 游戏模式切换时使用,应该只在GameManager接受再调用各manager接口
/// </summary>

public class GameModeChange : Message
{
    public GameModeChange(GameModeType gameModeType) : base(MessageTypes.GameModeChange)
    {
        GameModeType = gameModeType;
    }

    public GameModeType GameModeType;
}
public class PlaySound : Message
{
    public PlaySound(SoundType soundType) : base(MessageTypes.PlaySound)
    {
        SoundType = soundType;
    }

    public SoundType SoundType;
}

public class AddCard : Message
{
    public AddCard(int id) : base(MessageTypes.AddCard)
    {
        ID = id;
    }
    public int ID;
}

public class ConsumeCard : Message
{
    public ConsumeCard(int id) : base(MessageTypes.ConsumeCard)
    {
        ID = id;
    }
    public int ID;
}

public class UnlockLocation : Message
{
    public UnlockLocation(Location location) : base(MessageTypes.UnlockLocation)
    {
        Location = location;
    }
    public Location Location;
}

public class ActivationEvent : Message
{
    public ActivationEvent(string unlockID) : base(MessageTypes.ActivationEvent)
    {
        UnlockId = unlockID;
    }
    public string UnlockId;
}
public class EventFinish : Message
{
    public EventFinish(int id) : base(MessageTypes.EventFinish)
    {
        ID = id;
    }
    public int ID;
}
public class EnterDialog : Message
{
    public EnterDialog(int id,NPCType type) : base(MessageTypes.EnterDialog)
    {
        ID = id;
        Type = type;
    }
    public int ID;
    public NPCType Type;
}

public class MessageManager : SingletonDontDestory<MessageManager>
{
    private Dictionary<MessageTypes, UnityAction<Message>> listeners;

    private void OnEnable()
    {
        listeners = new Dictionary<MessageTypes, UnityAction<Message>>();
    }

    /// <summary>
    /// 注册消息
    /// </summary>
    public void Register(MessageTypes messageType, UnityAction<Message> action)
    {
        if (!listeners.ContainsKey(messageType))
            listeners.Add(messageType, action);
        else
            listeners[messageType] += action;
    }

    /// <summary>
    /// 移除消息
    /// </summary>
    public void Remove(MessageTypes messageType, UnityAction<Message> action)
    {
        if (listeners.ContainsKey(messageType))
            listeners[messageType] -= action;
    }

    /// <summary>
    /// 发送消息,且在控制台输出一句log
    /// </summary>
    public void Send(MessageTypes messageType, Message Message)
    {
        Debug.Log(messageType);
        UnityAction<Message> action = null;
        if (listeners.TryGetValue(messageType, out action))
            action(Message);
    }

    public void Clear()
    {
        listeners.Clear();
    }
}