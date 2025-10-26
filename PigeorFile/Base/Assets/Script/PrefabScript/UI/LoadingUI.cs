using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingUI : UIPrefabBase
{
    #region SerializeField

    [Header("UI组件")]
    [SerializeField] private TextMeshProUGUI LoadingText;
    
    [Header("动画参数")]
    [Tooltip("省略号切换的间隔时间（秒）")]
    [SerializeField] private float dotInterval = 0.5f;
    
    #endregion
    
    #region Property
    
    private Coroutine _textAnim;
    private string[] _loadingTexts = {"Loading", "Loading.", "Loading..", "Loading..."}; //预存的loading文字
    
    #endregion
    
    private void OnEnable()
    {
        if (_textAnim != null) StopCoroutine(_textAnim);
        _textAnim = StartCoroutine(LoadingTextAnim());
    }

    private void OnDisable()
    {
        if (_textAnim != null)
        {
            StopCoroutine(_textAnim);
            _textAnim = null;
        }
    }

    private IEnumerator LoadingTextAnim() //更新loading文字
    {
        int num = 0;
        while (true)
        {
            LoadingText.text = _loadingTexts[num];
            num = (num + 1) % _loadingTexts.Length; // 循环切换loading文字
            yield return new WaitForSecondsRealtime(dotInterval);
        }
    }
}
