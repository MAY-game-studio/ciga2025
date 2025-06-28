using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIHoverAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region SerializeField

    [Header("启用精灵渐变")]
    [SerializeField] private bool FlagHighlightSpriteAnim;
    
    [Header("动画配置参数")]
    [Tooltip("动画缓动类型")]
    [SerializeField] private Ease easeType = Ease.OutQuad;

    [Tooltip("偏移动画时间")]
    [SerializeField] private float Duration = 0.5f;
    
    [Tooltip("偏移向量")]
    [SerializeField] private Vector2 Offset;

    [Tooltip("缩放倍率")]
    [SerializeField] private Vector2 Scale = Vector2.one;

    [Tooltip("替换精灵")]
    [SerializeField] private Sprite HighLightSprite;
    
    [Tooltip("高光渐入动画时间")]
    [SerializeField] private float HighlightFadeDuration = 0.5f;

    [Tooltip("高光渐出动画时间")]
    [SerializeField] private float FadeOutDuration = 0.5f;
    
    #endregion

    #region Property
    
    private RectTransform _rectTransform;
    private Image _image;
    private Image _hoverImage;
    
    private Vector2 _originalPos;
    private Vector2 _originalScale;
    
    private Sequence _sequence;
    
    #endregion
    
    /// <summary>
    /// 创建一个额外的Hover图层 Image 组件
    /// </summary>
    private void CreateHoverImage()
    {
        GameObject extraImage = new GameObject("HoverImage");
        extraImage.transform.SetParent(transform, false); // 放在本对象下
        extraImage.transform.SetSiblingIndex(0);//设置在最前面防止遮盖
        _hoverImage = extraImage.AddComponent<Image>();
        _hoverImage.sprite = HighLightSprite;
        _hoverImage.raycastTarget = false; // 不拦截鼠标事件
        // 尺寸匹配当前对象
        RectTransform highlightRect = extraImage.GetComponent<RectTransform>();
        highlightRect.anchorMin = Vector2.zero;
        highlightRect.anchorMax = Vector2.one;
        highlightRect.offsetMin = Vector2.zero;
        highlightRect.offsetMax = Vector2.zero;
        // 初始设置为透明
        Color c = _hoverImage.color;
        c.a = 0f;
        _hoverImage.color = c;
    }
    
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _originalPos = _rectTransform.anchoredPosition;
        _originalScale = _rectTransform.localScale;
        if (FlagHighlightSpriteAnim)
        {
            _image = GetComponent<Image>();
            CreateHoverImage();
        }
    }
    
    /// <summary>
    /// 停止旧动画
    /// 创建并播放新动画
    /// sprite切换
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("enter");
        _sequence?.Kill();
        _sequence = DOTween.Sequence()
            .SetTarget(this)
            .SetUpdate(true)
            .Append(_rectTransform.DOAnchorPos(_originalPos + Offset, Duration).SetEase(easeType))
            .Join(_rectTransform.DOScale(new Vector3(Scale.x, Scale.y, 1f), Duration).SetEase(easeType));
        if (FlagHighlightSpriteAnim)
        {
            _sequence.Join(_image.DOFade(0f, HighlightFadeDuration).SetTarget(this));
            _sequence.Join(_hoverImage.DOFade(1f, HighlightFadeDuration).SetTarget(this));
        }
        _sequence.Play();
    }

    /// <summary>
    /// 停止旧动画
    /// 创建并播放还原动画
    /// sprite切换
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("exit");
        _sequence?.Kill();
        _sequence = DOTween.Sequence()
            .SetTarget(this)
            .SetUpdate(true)
            .Append(_rectTransform.DOAnchorPos(_originalPos, Duration).SetEase(easeType))
            .Join(_rectTransform.DOScale(new Vector3(_originalScale.x, _originalScale.y, 1f), Duration).SetEase(easeType));
        if (FlagHighlightSpriteAnim)
        {
            _sequence.Join(_image.DOFade(1f, HighlightFadeDuration).SetTarget(this));
            _sequence.Join(_hoverImage.DOFade(0f, HighlightFadeDuration).SetTarget(this));
        }
        _sequence.Play();
    }
}
