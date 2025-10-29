using TMPro;
using UnityEngine;

public class DetailUI : MonoBehaviour // 注意：不继承 UIPrefabBase
{
    #region SerializeField

    [Header("UI 组件")]
    [Tooltip("文本框")]
    [SerializeField] private TextMeshProUGUI ContentText;
    
    #endregion

    public void Init(string detail, string key = null) // 初始化此UI内容
    {
        ContentText.text = string.IsNullOrEmpty(key) ? detail : $"<b>{key}</b>: {detail}";
    }
}