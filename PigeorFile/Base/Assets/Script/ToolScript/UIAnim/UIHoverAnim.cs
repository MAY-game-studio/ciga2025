using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using NaughtyAttributes;

public class UIHoverAnim : UIAnim, IPointerEnterHandler, IPointerExitHandler
{
    #region SerializeField

    [Header("动画开关")]
    [Tooltip("启用缩放动画")]
    [SerializeField] private bool FlagAnimScale = true;
    [Tooltip("启用位移动画")]
    [SerializeField] private bool FlagAnimOffset;
    [Tooltip("启用颜色动画")]
    [SerializeField] private bool FlagAnimColor;

    [Header("动画参数")]
    [Tooltip("动画时长")]
    [SerializeField] private float Duration = 0.2f;
    [Tooltip("动画曲线")]
    [SerializeField] private Ease EaseType = Ease.OutQuad;
    
    
    [ShowIf("FlagAnimScale")]
    [Tooltip("缩放大小")]
    [SerializeField] private float TargetScale = 1.2f;
    [ShowIf("FlagAnimOffset")]
    [Tooltip("偏移量")]
    [SerializeField] private Vector2 PositionOffset = new Vector2(0, 50f);
    [ShowIf("FlagAnimColor")]
    [Tooltip("高光")]
    [SerializeField] private Color TargetColor = new Color(1f, 1f, 1f, 0f);
    
    #endregion

    #region property

    private RectTransform _rectTransform;
    private Image _image;
    
    #endregion

    protected override void Register()
    {
        UIAnimManager.GetInstance().AnimRegister(gameObject,this);
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        PossibleState = UIAnimState.HOVERIN | UIAnimState.HOVEROUT;
    }

    public override void Init()
    {
        Sequence?.Kill(); 
        Sequence = DOTween.Sequence().SetAutoKill(false).SetTarget(this).SetUpdate(true).Pause();
        Property = UIAnimProperty.NONE;
        if (FlagAnimScale) // 缩放动画
        {
            Sequence.Join(_rectTransform.DOScale(_rectTransform.localScale * TargetScale, Duration).SetEase(EaseType));
            Property |= UIAnimProperty.SCALE; //登记该动画property
        }
        if (FlagAnimOffset) // 位移动画
        {
            Sequence.Join(_rectTransform.DOAnchorPos(_rectTransform.anchoredPosition + PositionOffset, Duration).SetEase(EaseType));
            Property |= UIAnimProperty.POSITION; //登记该动画property
        }
        if (FlagAnimColor && _image) // 颜色动画
        {
            Sequence.Join(_image.DOColor(TargetColor, Duration).SetEase(EaseType));
            Property |= UIAnimProperty.COLOR; //登记该动画property
        }
        Sequence.OnRewind(() =>
        {
            UIAnimManager.GetInstance().StateUpdate(gameObject, UIAnimState.HOVEROUT, false); //更新状态机
            UIAnimManager.GetInstance().PropertyUpdate(gameObject, Property, false); // 释放anim属性
        });
    }

    public override void CancelAnim(bool flagEvent = false)
    {
        Sequence?.Rewind(flagEvent); //回到静息状态
        Debug.Log("cancel");
        if (UIAnimManager.GetInstance().GetState(gameObject, UIAnimState.HOVERIN)) //注销可能的HOVERIN状态
            UIAnimManager.GetInstance().StateUpdate(gameObject, UIAnimState.HOVERIN, false);
        if (UIAnimManager.GetInstance().GetState(gameObject, UIAnimState.HOVEROUT)) //注销可能的HOVEROUT状态
            UIAnimManager.GetInstance().StateUpdate(gameObject, UIAnimState.HOVEROUT, false);
        UIAnimManager.GetInstance().PropertyUpdate(gameObject, Property, false); // 释放anim属性
    }
    
    public void OnPointerEnter(PointerEventData eventData) //获得鼠标焦点时触发
    {
        if (UIAnimManager.GetInstance().GetState(gameObject, UIAnimState.HOVERIN)) return; //防止动画重复
        if (UIAnimManager.GetInstance().GetState(gameObject, UIAnimState.HOVEROUT)) //反转播放
        {
            UIAnimManager.GetInstance().StateUpdate(gameObject, UIAnimState.HOVEROUT, false);
            UIAnimManager.GetInstance().StateUpdate(gameObject, UIAnimState.HOVERIN, true);
            Sequence.PlayForward();
        }
        else if (UIAnimManager.GetInstance().QueryPropertyAvailable(gameObject, Property, priority)) //查询是否有冲突动画
        {
            UIAnimManager.GetInstance().CancelConflictAnims(gameObject, Property); //取消冲突动画
            UIAnimManager.GetInstance().PropertyUpdate(gameObject, Property, true); // 登记anim属性
            UIAnimManager.GetInstance().StateUpdate(gameObject, UIAnimState.HOVERIN, true);
            Sequence.PlayForward();
        } 
    }

    public void OnPointerExit(PointerEventData eventData) //失去鼠标焦点时触发
    {
        if ((UIAnimManager.GetInstance().GetState(gameObject) & UIAnimState.HOVEROUT) == UIAnimState.HOVEROUT) return; //防止动画重复
        if ((UIAnimManager.GetInstance().GetState(gameObject) & UIAnimState.HOVERIN) == UIAnimState.HOVERIN) //反转播放
        {
            UIAnimManager.GetInstance().StateUpdate(gameObject, UIAnimState.HOVERIN, false);
            UIAnimManager.GetInstance().StateUpdate(gameObject, UIAnimState.HOVEROUT, true);
            Sequence.PlayBackwards();
        }
    }
}