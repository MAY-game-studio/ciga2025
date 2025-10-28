using System.Collections.Generic;
using UnityEngine;

public class UIAnimManager : SingletonDontDestroy<UIAnimManager>
{
    #region property
    
    private class UIAnimData
    {
        public UIAnimProperty Property = UIAnimProperty.NONE; //当前占用的property状态
        public UIAnimState State = UIAnimState.IDLE; //当前的state状态机
        public readonly List<UIAnim> RegisteredUIAnims = new(); //注册的ui动画
    }
    
    private readonly Dictionary<GameObject, UIAnimData> _uiAnimData = new();

    #endregion

    public void AnimRegister(GameObject obj, UIAnim anim)
    {
        if (!_uiAnimData.ContainsKey(obj))
            _uiAnimData[obj] = new UIAnimData(); //初次注册时进行初始化
        _uiAnimData[obj].RegisteredUIAnims.Add(anim);
    }
    
    public void AnimUnregister(GameObject obj) { _uiAnimData.Remove(obj); } //注销obj的所有动画信息
    
    public void PropertyUpdate(GameObject obj, UIAnimProperty property, bool flagAdd) //更新obj的property状态
    {
        if (!_uiAnimData.TryGetValue(obj, out var data)) return;
        if (flagAdd)
            data.Property |= property; // 锁定property
        else
            data.Property &= ~property; // 解锁property
    }
    
    public void StateUpdate(GameObject obj, UIAnimState state, bool flagAdd) //更新obj的state状态
    {
        if (!_uiAnimData.TryGetValue(obj, out var data)) return;
        if (flagAdd)
            data.State |= state; // 添加状态
        else
            data.State &= ~state; //移除状态
    }
    
    public UIAnimProperty GetProperty(GameObject obj) //返回占用的AnimProperty状态
    {
        return _uiAnimData.TryGetValue(obj, out var animData) ? animData.Property : UIAnimProperty.NONE;
    } 

    public UIAnimState GetState(GameObject obj) //返回状态机
    {
        return _uiAnimData.TryGetValue(obj, out var animData) ? animData.State : UIAnimState.IDLE;
    }

    public bool GetState(GameObject obj, UIAnimState state) //检查是否有特定state
    {
        return (GetState(obj) & state) != 0; 
    }
    
    public bool QueryPropertyAvailable(GameObject obj, UIAnimProperty property,int priority) //查询property状态是否可用
    {
        if (!_uiAnimData.TryGetValue(obj, out var animData)) return true;
        if ((animData.Property & property) == 0) return true; //无property状态占用
        foreach (var anim in animData.RegisteredUIAnims)
        {
            if (GetState(obj, anim.PossibleState) && (anim.Property & property) != 0 && priority < anim.priority)
                return false; // 被更高优先级的活动动画阻止
        }
        return true; //所有冲突动画优先级低于当前动画
    }

    public void CancelConflictAnims(GameObject obj, UIAnimProperty property) //取消所有与property冲突的动画
    {
        if (!_uiAnimData.TryGetValue(obj, out var animData)) return;
        foreach (var anim in animData.RegisteredUIAnims)
        {
            if (GetState(obj, anim.PossibleState) && (anim.Property & property) != 0)
                anim.CancelAnim(); // 取消该动画
        }
    }
    public void ReInitAnim(GameObject obj) // 重新初始化指定GameObject上所有已注册的UIAnim组件。
    {
        if (!_uiAnimData.TryGetValue(obj, out var animData)) return;
        foreach (var anim in animData.RegisteredUIAnims)
            anim.Init(); // 调用每个组件的 public Init() 方法
    }
}
