using UnityEngine;
using TMPro;

public class DetailUI : MonoBehaviour // 注意：不继承 UIPrefabBase
{
    #region SerializeField

    [Header("UI 组件")]
    [Tooltip("文本框")]
    [SerializeField] private TextMeshProUGUI _contentText;
    
    #endregion

    public void Init(string detail, string key = null) // 初始化此UI内容
    {
        _contentText.text = string.IsNullOrEmpty(key) ? detail : $"<b>{key}</b>: {detail}";
    }
}