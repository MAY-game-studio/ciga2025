using System;
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
    
    public void Play(Tween tween) //立刻完成当前动画并进行下一个
    {
        if (_currentTween != null && _currentTween.IsActive() && !_currentTween.IsComplete())
        {
            _currentTween.OnComplete(() =>
            {
                _currentTween = tween;
            });
            _currentTween.Complete();
            Debug.Log("complete");
        }
        else
        {
            _currentTween = tween;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && _currentTween != null)
        {       _currentTween.Complete();
            Debug.Log("skip");
        }
    }
}
