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
    [SerializeField] private Ease easeType = Ease.OutQuad;

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
    
    #endregion
    void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null) _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup.alpha = 0f;
        _rectTransform.anchoredPosition += Offset;
    }

    void Start()
    {
        if (!FlagAwakeAnim) return;
        Sequence seq = DOTween.Sequence(); // 动画同时淡入与移动
        seq.AppendInterval(Delay);
        seq.Append(_canvasGroup.DOFade(1f, Duration));
        seq.Join(_rectTransform.DOAnchorPos(_rectTransform.anchoredPosition - Offset, Duration).SetEase(easeType));
    }

    private void OnEnable()
    {
        if (!FlagEnableAnim) return;
        Sequence seq = DOTween.Sequence(); // 动画同时淡入与移动
        seq.AppendInterval(Delay);
        seq.Append(_canvasGroup.DOFade(1f, Duration));
        seq.Join(_rectTransform.DOAnchorPos(_rectTransform.anchoredPosition - Offset, Duration).SetEase(easeType));
    }
}
