using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIAnimManager : SingletonDontDestory<UIAnimManager>
{
    #region property
    
    private Dictionary<GameObject, UIAnimProperty> _uiAnimProperty = new(); //当前占用的property状态
    private Dictionary<GameObject, UIAnimState> _uiAnimState = new(); //当前的state状态机
    private Dictionary<GameObject, List<UIAnim>> _registeredAnims = new(); //注册的ui动画
    
    #endregion

    public void AnimRegister(GameObject obj,UIAnim anim)
    {
        if (!_registeredAnims.ContainsKey(obj)) // 如果该物体是第一次被注册，则初始化所有相关的字典
        {
            _uiAnimProperty[obj] = UIAnimProperty.NONE;
            _uiAnimState[obj] = UIAnimState.IDLE;
            _registeredAnims[obj] = new List<UIAnim>();
        }
        _registeredAnims[obj].Add(anim);
    }
    
    public void AnimUnregister(GameObject obj) //注销obj
    {
        _uiAnimProperty.Remove(obj);
        _uiAnimState.Remove(obj);
        _registeredAnims.Remove(obj);
    }
  
    public void PropertyUpdate(GameObject obj, UIAnimProperty property, bool flagAdd)
    {
        if (_uiAnimProperty.ContainsKey(obj))
        {
            if (flagAdd) // 锁定property
            {
                _uiAnimProperty[obj] |= property;
            }
            else // 解锁property
                _uiAnimProperty[obj] &= ~property;
        }
    }

    public void StateUpdate(GameObject obj, UIAnimState state, bool flagAdd)
    {
        if (_uiAnimState.ContainsKey(obj))
        {
            if (flagAdd) // 添加状态
                _uiAnimState[obj] |= state;
            else // False: 移除状态
                _uiAnimState[obj] &= ~state;
        }
    }
    public UIAnimProperty GetProperty(GameObject obj) { return _uiAnimProperty[obj]; }
    public UIAnimState GetState(GameObject obj) { return _uiAnimState[obj]; } //返回状态机
    public bool GetState(GameObject obj, UIAnimState state) { return (_uiAnimState[obj] & state) != 0; } //检查是否有特定状态
    
    public bool QueryPropertyAvailable(GameObject obj, UIAnimProperty property,int priority) //查询property状态是否可用
    {
        if ((GetProperty(obj) & property) == 0) return true; //无property状态占用
        foreach (var anim in _registeredAnims[obj])
        {
            if (GetState(obj, anim.PossibleState) && (anim.Property & property) != 0 && priority < anim.priority)
                return false; // 被更高优先级的活动动画阻止
        }
        return true;
    }

    public void CancelConflictAnims(GameObject obj, UIAnimProperty property) //取消所有与property冲突的动画
    {
        foreach (var anim in _registeredAnims[obj])
        {
            if (GetState(obj, anim.PossibleState) && (anim.Property & property) != 0)
                anim.CancelAnim();
        }
    }
    public void ReInitAnim(GameObject obj) // 重新初始化指定GameObject上所有已注册的UIAnim组件。
    {
        if (_registeredAnims.TryGetValue(obj, out var anims))
        {
            foreach (var anim in anims)
                anim.Init(); // 调用每个组件的 public Init() 方法
        }
    }
}

