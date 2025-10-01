using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testobj : MonoBehaviour
{
    [Header("测试设置")]
    [Tooltip("请将 testObj 的子按钮拖到这里")]
    [SerializeField] private Button childButton;

    private UIFadeAnim fadeAnim;
    private UIHoverAnim hoverAnim;

    void Start()
    {
        // 获取并检查必要的组件
        fadeAnim = GetComponent<UIFadeAnim>();
        if (childButton != null)
        {
            hoverAnim = childButton.GetComponent<UIHoverAnim>();
        }

        if (fadeAnim == null || hoverAnim == null)
        {
            Debug.LogError("测试失败：请确保 testObj 上有 UIFadeAnim 组件，并且 childButton 上有 UIHoverAnim 组件！", gameObject);
            this.enabled = false; // 禁用此脚本
            return;
        }

        // 确保优先级设置正确，以进行有效测试
        if (fadeAnim.priority <= hoverAnim.priority)
        {
            Debug.LogWarning("测试警告：为了验证“打断/锁定”逻辑，建议将 UIFadeAnim 的优先级设置得高于 UIHoverAnim。", gameObject);
        }

        Debug.Log("测试脚本已准备就绪。按 '1' 键播放退场动画，按 '2' 键播放入场动画。");
    }

    void Update()
    {
        // 按下键盘 1 键
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Test: Commanding this object to TryDisable()...");
            // 使用我们写好的扩展方法来禁用自己
            gameObject.TryDisable();
        }

        // 按下键盘 2 键
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Test: Commanding this object to TryEnable()...");
            // 使用扩展方法来启用自己
            gameObject.TryEnable();
        }
    }
}
