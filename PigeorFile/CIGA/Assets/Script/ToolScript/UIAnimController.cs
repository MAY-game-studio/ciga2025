using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIAnimController : MonoBehaviour
{
    #region Property

    private Tween _currentTween;
    public Tween CurrentTween
    {
        get => _currentTween;
        private set => _currentTween = value;
    }
    
    #endregion
    
    /// <summary>
    /// 立刻完成当前动画并进行下一个
    /// </summary>
    /// <param name="newTween"></param>
    public void Play(Tween newTween)
    {
        if (_currentTween != null && _currentTween.IsActive() && !_currentTween.IsComplete())
            _currentTween.Complete();
        _currentTween = newTween;
    }
}
