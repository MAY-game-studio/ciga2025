using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIFadeOutAnim : MonoBehaviour
{
    #region SerializeField

    [Header("启用Destroy动画")]
    [SerializeField] private bool FlagDestoryAnim;

    [Header("启用Disable动画")]
    [SerializeField] private bool FlagDisableAnim;

    [Header("动画配置参数")]
    [Tooltip("动画缓动类型")]
    [SerializeField] private Ease EaseType = Ease.OutQuad;

    [Tooltip("动画延迟")]
    [SerializeField] private float Delay;

    [Tooltip("淡出时间")]
    [SerializeField] private float Duration = 0.5f;

    [Tooltip("淡出向量")]
    [SerializeField] private Vector2 Offset;
    
    #endregion

    #region Property

    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private GameObject _stand;
    private UIAnimController _animController;
    
    #endregion
    
    private void CreateStandObject() //创建替身object，禁用与销毁时应优先尝试对替身操作，其会调用主体的动画序列并延时指令
    {
        _stand = new GameObject("StandObject");
        _stand.transform.SetParent(transform, false); // 设置为当前物体的子对象
        _stand.AddComponent<Stand>();
    }
    
    void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
        _animController = GetComponent<UIAnimController>() ?? gameObject.AddComponent<UIAnimController>();
        CreateStandObject();//创建替身子物体
        _rectTransform = GetComponent<RectTransform>();
    }

    public void BeforeDisable()
    {
        if (FlagDisableAnim)
        {
/*            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(Delay);
            seq.Append(_canvasGroup.DOFade(0f, Duration));
            seq.Join(_rectTransform.DOAnchorPos(_rectTransform.anchoredPosition + Offset, Duration).SetEase(EaseType));
            seq.OnComplete(() => { _stand.SetActive(true); gameObject.SetActive(false); });//动画结束后复原替身，隐藏自己*/            
            _animController.Play(
                DOTween.Sequence()
                    .AppendInterval(Delay)
                    .Append(_canvasGroup.DOFade(0f, Duration))
                    .Join(_rectTransform.DOAnchorPos(_rectTransform.anchoredPosition + Offset, Duration).SetEase(EaseType))
                    .OnComplete(() => { _stand.SetActive(true); gameObject.SetActive(false); })
            );
/*            DOTween.Sequence()
                .AppendInterval(Delay)
                .Append(_canvasGroup.DOFade(0f, Duration))
                .Join(_rectTransform.DOAnchorPos(_rectTransform.anchoredPosition + Offset, Duration).SetEase(EaseType))
                .OnComplete(() =>
                {
                    _stand.SetActive(true);
                    gameObject.SetActive(false);
                })
                .SetTarget(this).Play();*/
        }
        else
            gameObject.SetActive(false);
    }

    public void BeforeDestroy()
    {
        if (FlagDestoryAnim)
        {
            _animController.Play(
                DOTween.Sequence()
                    .AppendInterval(Delay)
                    .Append(_canvasGroup.DOFade(0f, Duration))
                    .Join(_rectTransform.DOAnchorPos(_rectTransform.anchoredPosition + Offset, Duration).SetEase(EaseType))
                    .OnComplete(() => { Destroy(gameObject); })
            );
        }
        else
            Destroy(gameObject);
    }
}
