using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GameObjectExtensions
{
    /// <summary>
    /// 尝试激活或禁用一个GameObject，并智能触发动画。
    /// </summary>
    public static void TrySetActive(this GameObject obj, bool value)
    {
        if (obj == null) return;
        if (value)
            TryEnable(obj);
        else
            TryDisable(obj);
    }
    
    /// <summary>
    /// 尝试激活一个GameObject并播放入场动画。
    /// </summary>
    public static void TryEnable(this GameObject obj)
    {
        if (obj == null) return;
        if (!obj.activeInHierarchy)
            obj.SetActive(true);
        else
        {
            var fadeAnim = obj.GetComponent<UIFadeAnim>();
            if (fadeAnim != null)
                fadeAnim.OnFadeIn(); // 调用OnFadeIn，它内部的逻辑会智能处理（无论是首次入场还是中断出场）
        }
    }
    
    /// <summary>
    /// 尝试禁用一个GameObject。如果该对象上有UIFadeAnim组件，则会先播放出场动画。
    /// </summary>
    public static void TryDisable(this GameObject obj)
    {
        if (obj == null) return;
        if (obj.activeInHierarchy)
        {
            var fadeAnim = obj.GetComponent<UIFadeAnim>();
            if (fadeAnim != null) // 假设UIFadeAnim内部有一个useFadeOut的开关
                fadeAnim.OnFadeOut(); // 调用OnFadeOut，它内部的逻辑会智能处理（无论是首次出场还是中断入场）
            else
                obj.SetActive(false);
        }
    }
    
    /// <summary>
    /// 尝试销毁一个GameObject。如果该对象上有UIFadeAnim组件，则会先播放出场动画。
    /// </summary>
    public static void TryDestroy(this GameObject obj)
    {
        if (obj == null) return;
        if (obj.activeInHierarchy)
        {
            var fadeAnim = obj.GetComponent<UIFadeAnim>();
            if (fadeAnim != null)
                fadeAnim.OnFadeOut(true); // 调用OnFadeOut，它内部的逻辑会智能处理（无论是首次出场还是中断入场）
            else
                UnityEngine.Object.Destroy(obj);
        }
        else
            UnityEngine.Object.Destroy(obj);
    }
}
