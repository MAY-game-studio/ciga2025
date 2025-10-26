using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using NaughtyAttributes;

public class UIFadeAnim : UIAnim
{
    #region SerializeField

    [Header("动画开关")]
    [Tooltip("启用透明度渐变")]
    [SerializeField] private bool FlagAnimFade = true;
    [Tooltip("启用缩放动画")]
    [SerializeField] private bool FlagAnimScale;
    [Tooltip("启用位移动画")]
    [SerializeField] private bool FlagAnimOffset;
    
    [Header("动画参数")]
    [Tooltip("入场延时")]
    [SerializeField] private float Delay;
    [Tooltip("动画时长")]
    [SerializeField] private float Duration = 0.5f;
    [Tooltip("动画曲线")]
    [SerializeField] private Ease EaseType = Ease.OutQuad;
    
    
    [Header("Fade Settings")]
    [Tooltip("起始透明度")]
    [MinMaxSlider(0f, 1f)]
    [SerializeField] private float StartAlpha = 0f;
    [ShowIf("FlagAnimScale")]
    [Tooltip("起始缩放大小")]
    [SerializeField] private float StartScale = 0.5f;
    [ShowIf("FlagAnimOffset")]
    [Tooltip("起始偏移量")]
    [SerializeField] private Vector2 StartPositionOffset = new Vector2(0f, 0f);
    
    #endregion

    #region property

    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private bool _flagDestroy;
    private Coroutine _enableCoroutine;
    
    #endregion

    protected override void Register()
    {
        UIAnimManager.GetInstance().AnimRegister(gameObject,this);
        _rectTransform = GetComponent<RectTransform>();
        if (!TryGetComponent<CanvasGroup>(out _canvasGroup))
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        // 定义此组件相关的状态
        PossibleState = UIAnimState.FADEIN | UIAnimState.FADEOUT;
    }
    
    public override void Init()
    {
        Sequence?.Kill(); 
        Sequence = DOTween.Sequence().SetAutoKill(false).SetTarget(this).SetUpdate(true).Pause();
        Property = UIAnimProperty.NONE;
        if (FlagAnimFade) // 透明度渐变动画
        {
            Sequence.Join(_canvasGroup.DOFade(_canvasGroup.alpha, Duration).From(StartAlpha).SetEase(EaseType));
            Property |= UIAnimProperty.COLOR; //登记该动画property
        }
        if (FlagAnimScale) // 从*StartScale缩放到原大小
        {
            Sequence.Join(_rectTransform.DOScale(_rectTransform.localScale, Duration).From(_rectTransform.localScale * StartScale).SetEase(EaseType));
            Property |= UIAnimProperty.SCALE;
        }
        if (FlagAnimOffset) // 从+offset位移到原位置
        {
            Sequence.Join(_rectTransform.DOAnchorPos(_rectTransform.anchoredPosition, Duration).From(_rectTransform.anchoredPosition + StartPositionOffset).SetEase(EaseType));
            Property |= UIAnimProperty.POSITION;
        }
        Sequence.OnComplete(() => {
            UIAnimManager.GetInstance().StateUpdate(gameObject, UIAnimState.FADEIN, false); //更新状态机
            UIAnimManager.GetInstance().PropertyUpdate(gameObject, Property, false); // 释放anim属性
        });
        Sequence.OnRewind(() => {
            Debug.Log("UIFadeAnim OnRewind");
            UIAnimManager.GetInstance().StateUpdate(gameObject, UIAnimState.FADEOUT, false); //更新状态机
            UIAnimManager.GetInstance().PropertyUpdate(gameObject, Property, false); // 释放anim属性
            if (_flagDestroy)
                Destroy(gameObject);
            else
                gameObject.SetActive(false);
        });
    }
    
    public override void CancelAnim(bool flagEvent = false)
    {
        Sequence?.Complete(flagEvent);
        if (UIAnimManager.GetInstance().GetState(gameObject, UIAnimState.FADEIN)) // 注销可能的FadeIn状态
            UIAnimManager.GetInstance().StateUpdate(gameObject, UIAnimState.FADEIN, false);
        if (UIAnimManager.GetInstance().GetState(gameObject, UIAnimState.FADEOUT)) // 注销可能的FadeOut状态
            UIAnimManager.GetInstance().StateUpdate(gameObject, UIAnimState.FADEOUT, false);
        UIAnimManager.GetInstance().PropertyUpdate(gameObject, Property, false);
    }

    private void OnEnable() { OnFadeIn(); }

    private IEnumerator DelayAnim()
    {
        Sequence.Goto(Mathf.Epsilon); //回到初始状态，延时后重新播放(避免触发OnRewind)
        if (Delay > 0) yield return new WaitForSecondsRealtime(Delay);
        Sequence.PlayForward(); //延时结束进行入场动画
        _enableCoroutine = null;
    }
    
    public void OnFadeIn() //入场动画
    {
        if ((UIAnimManager.GetInstance().GetState(gameObject) & UIAnimState.FADEIN) == UIAnimState.FADEIN) return; //防止动画重复
        if ((UIAnimManager.GetInstance().GetState(gameObject) & UIAnimState.FADEOUT) == UIAnimState.FADEOUT) //反转播放
        {
            UIAnimManager.GetInstance().StateUpdate(gameObject, UIAnimState.FADEOUT, false);
            UIAnimManager.GetInstance().StateUpdate(gameObject, UIAnimState.FADEIN, true);
            Sequence.PlayForward();
            _flagDestroy = false;
        }
        else if (UIAnimManager.GetInstance().QueryPropertyAvailable(gameObject, Property, priority)) //查询是否有冲突动画
        {
            UIAnimManager.GetInstance().CancelConflictAnims(gameObject, Property);
            UIAnimManager.GetInstance().PropertyUpdate(gameObject, Property, true);
            UIAnimManager.GetInstance().StateUpdate(gameObject, UIAnimState.FADEIN, true);
            if (_enableCoroutine != null) StopCoroutine(_enableCoroutine);
            _enableCoroutine = StartCoroutine(DelayAnim()); //完整播放存在延时
        }
    }

    public void OnFadeOut(bool flagDestroy = false) //出场动画
    {
        _flagDestroy = flagDestroy;
        if ((UIAnimManager.GetInstance().GetState(gameObject) & UIAnimState.FADEOUT) == UIAnimState.FADEOUT) return; //防止动画重复
        if (_enableCoroutine != null) //入场动画延时还没结束，直接停止入场动画
        {
            StopCoroutine(_enableCoroutine);
            _enableCoroutine = null;
            if (_flagDestroy)
                Destroy(gameObject);
            else
                gameObject.SetActive(false);
            return;
        }
        if ((UIAnimManager.GetInstance().GetState(gameObject) & UIAnimState.FADEIN) == UIAnimState.FADEIN) //反转播放
        {
            UIAnimManager.GetInstance().StateUpdate(gameObject, UIAnimState.FADEIN, false);
            UIAnimManager.GetInstance().StateUpdate(gameObject, UIAnimState.FADEOUT, true);
            Sequence.PlayBackwards();
        }
        else if (UIAnimManager.GetInstance().QueryPropertyAvailable(gameObject, Property, priority)) //查询是否有冲突动画
        {
            UIAnimManager.GetInstance().CancelConflictAnims(gameObject, Property);
            UIAnimManager.GetInstance().PropertyUpdate(gameObject, Property, true);
            UIAnimManager.GetInstance().StateUpdate(gameObject, UIAnimState.FADEOUT, true);
            Sequence.PlayBackwards();
        }
    }
}
