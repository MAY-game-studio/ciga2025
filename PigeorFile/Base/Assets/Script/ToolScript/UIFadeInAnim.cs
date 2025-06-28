using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIFadeInAnim : MonoBehaviour
{
    #region SerializeField

    [Header("启用Awake动画")]
    [SerializeField] private bool FlagAwakeAnim;

    [Header("启用Enable动画")]
    [SerializeField] private bool FlagEnableAnim;

    [Header("动画配置参数")]
    [Tooltip("动画缓动类型")]
    [SerializeField] private Ease EaseType = Ease.OutQuad;

    [Tooltip("动画延迟")]
    [SerializeField] private float Delay;

    [Tooltip("淡入时间")]
    [SerializeField] private float Duration = 0.5f;

    [Tooltip("淡入向量")]
    [SerializeField] private Vector2 Offset;
    
    #endregion

    #region Property

    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private Vector2 _originalPosition;
    private UIAnimController _animController;
    
    #endregion

    void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
        _animController = GetComponent<UIAnimController>() ?? gameObject.AddComponent<UIAnimController>();
        _rectTransform = GetComponent<RectTransform>();
        _originalPosition = _rectTransform.anchoredPosition;
        _canvasGroup.alpha = 0f;
        _rectTransform.anchoredPosition += Offset;
    }

    void Start()
    {
        if (FlagAwakeAnim)
        {
            _animController.Play(
                DOTween.Sequence()
                    .AppendInterval(Delay)
                    .Append(_canvasGroup.DOFade(1f, Duration))
                    .Join(_rectTransform.DOAnchorPos(_originalPosition, Duration).SetEase(EaseType))
            );
            /*DOTween.Sequence()
                .AppendInterval(Delay)
                .Append(_canvasGroup.DOFade(1f, Duration))
                .Join(_rectTransform.DOAnchorPos(_originalPosition, Duration).SetEase(EaseType))
                .SetTarget(this).Play();*/
        }
    }

    private void OnEnable()
    {
        _canvasGroup.alpha = 0f;
        _rectTransform.anchoredPosition = _originalPosition + Offset;
        Debug.Log(_rectTransform.anchoredPosition);
        Debug.Log(_originalPosition);
        if (FlagEnableAnim)
        {
            _animController.Play(
                DOTween.Sequence()
                    .AppendInterval(Delay)
                    .Append(_canvasGroup.DOFade(1f, Duration))
                    .Join(_rectTransform.DOAnchorPos(_originalPosition, Duration).SetEase(EaseType))
            );
            /*DOTween.Sequence()
                .AppendInterval(Delay)
                .Append(_canvasGroup.DOFade(1f, Duration))
                .Join(_rectTransform.DOAnchorPos(_originalPosition, Duration).SetEase(EaseType))
                .SetTarget(this).Play();*/
        }
    }
}
