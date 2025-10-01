using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class UIAnim : MonoBehaviour
{
    #region Serelizefield

    [Header("动画优先级")]
    [Tooltip("数字越大，优先级越高，可以打断低优先级的动画")]
    public int priority;

    #endregion
    
    private UIAnimProperty property;
    public UIAnimProperty Property
    {
        get => property;
        protected set => property = value;
    }
 
    private Sequence _Sequence;
    public Sequence Sequence
    {
        get => _Sequence;
        protected set => _Sequence = value;
    }
    
    private UIAnimState possibleState;
    public UIAnimState PossibleState
    {
        get => possibleState;
        protected set => possibleState = value;
    }
    
    protected abstract void Register();
    public abstract void Init(); //设置初始状态,更新位置后需重置动画内容
    protected virtual void Awake()
    {
        Register();
        Init();
    }
    public abstract void CancelAnim(bool flagEvent = false); //立即返回到静息状态
    
    public virtual void CompleteAnim(bool flagCallbacks = true) //强制立即完成动画，跳转到结束状态,根据flagCallbacks判定是否在完成后触发OnComplete等回调
    {
        if (Sequence == null || !Sequence.IsActive()) return;
        if (!Sequence.IsBackwards())
            Sequence.Complete(flagCallbacks);
        else
            Sequence.Rewind(flagCallbacks);
    }
    
    protected virtual void OnDestroy()
    {
        Sequence?.Kill();
        UIAnimManager.GetInstance().AnimUnregister(gameObject);
    }
}