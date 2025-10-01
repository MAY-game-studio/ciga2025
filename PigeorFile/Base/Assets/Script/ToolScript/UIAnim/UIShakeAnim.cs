using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using NaughtyAttributes;

public class UIShakeAnim : UIAnim
{
    #region SerializeField

    [Header("动画开关")]
    [Tooltip("启用位置抖动")]
    [SerializeField] private bool FlagAnimShakePosition = true;
    [Tooltip("启用旋转抖动")]
    [SerializeField] private bool FlagAnimShakeRotation = false;

    [Header("动画参数")]
    [Tooltip("动画时长")]
    [SerializeField] private float Duration = 0.5f;
    [Tooltip("抖动频率")]
    [SerializeField] private int Vibrato = 10;
    [Tooltip("结束淡出")]
    [SerializeField] private bool FadeOut = true;
    
    [ShowIf("FlagAnimShakePosition")]
    [Tooltip("位置抖动强度")]
    [SerializeField] private Vector2 PositionStrength = new Vector2(10, 0);
    [ShowIf("FlagAnimShakeRotation")]
    [Tooltip("旋转抖动强度 (Z轴角度)")]
    [SerializeField] private float RotationStrength = 15f;

    #endregion

    #region Property

    private RectTransform _rectTransform;

    #endregion
    
    protected override void Register()
    {
        UIAnimManager.GetInstance().AnimRegister(gameObject, this);
        _rectTransform = GetComponent<RectTransform>();
        PossibleState = UIAnimState.SHAKE;
    }
    
    public override void Init()
    {
        Sequence?.Kill();
        Sequence = DOTween.Sequence().SetAutoKill(false).SetTarget(this).SetUpdate(true).Pause();
        Property = UIAnimProperty.NONE;
        if (FlagAnimShakePosition)
        {
            Sequence.Append(_rectTransform.DOShakeAnchorPos(Duration, PositionStrength, Vibrato, 90, FadeOut));
            Property |= UIAnimProperty.POSITION;
        }
        if (FlagAnimShakeRotation)
        {
            // 旋转的Strength通常Z轴有值
            Sequence.Join(_rectTransform.DOShakeRotation(Duration, new Vector3(0, 0, RotationStrength), Vibrato, 90, FadeOut));
            Property |= UIAnimProperty.ROTATION;
        }

        // 动画播放完成后，自动清理状态并解锁属性
        Sequence.OnComplete(() =>
        {
            UIAnimManager.GetInstance().StateUpdate(gameObject, UIAnimState.SHAKE, false);
            UIAnimManager.GetInstance().PropertyUpdate(gameObject, Property, false);
        });
    }
    public override void CancelAnim(bool flagEvent = false)
    {
        Sequence?.Rewind(flagEvent);
        UIAnimManager.GetInstance().StateUpdate(gameObject, UIAnimState.SHAKE, false);
        UIAnimManager.GetInstance().PropertyUpdate(gameObject, Property, false);
    }
    
    public void OnShake()
    {
        if (UIAnimManager.GetInstance().GetState(gameObject, UIAnimState.SHAKE))
        {
            Sequence.Restart();
            return; // 如果正在抖动，则不允许重复播放
        }
        if (UIAnimManager.GetInstance().QueryPropertyAvailable(gameObject, Property, priority))
        {
            UIAnimManager.GetInstance().CancelConflictAnims(gameObject, Property);
            UIAnimManager.GetInstance().PropertyUpdate(gameObject, Property, true); // 锁定属性
            UIAnimManager.GetInstance().StateUpdate(gameObject, UIAnimState.SHAKE, true); // 设置状态
            Sequence.Restart();
        }
    }
}
