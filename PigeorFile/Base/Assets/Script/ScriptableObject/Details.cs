using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Details", menuName = "ScriptableObjects/Details", order = 2)]
public class Details : ScriptableObject ,ISerializationCallbackReceiver
{
    #region SerializeField

    [System.Serializable]
    public class DetailPair
    {
        public string key; //关键字
        [TextArea(3, 10)]
        public string detail; //对应的详细描述
    }
    
    [Header("所有Detail词条")]
    [SerializeField]
    private List<DetailPair> details = new List<DetailPair>();

    #endregion

    #region Property

    private Dictionary<string, string> _details;
    
    #endregion

    public string GetDetail(string keyword) //由details获取详细文本的方法
    {
        if (string.IsNullOrEmpty(keyword)) { return null; }
        return _details.GetValueOrDefault(keyword);
    }

    public void OnBeforeSerialize() {} // 在 Unity 准备序列化（保存）此对象之前调用。

    public void OnAfterDeserialize()
    {
        _details ??= new Dictionary<string, string>();
        _details.Clear();
        foreach (var tmp in details) // 遍历所有在 Inspector 中设置的词条
        {
            if (!string.IsNullOrEmpty(tmp.key) && !_details.ContainsKey(tmp.key)) // 确保关键字不为空，并且防止重复关键字导致字典抛出异常
            {
                _details.Add(tmp.key, tmp.detail);
            }
        }
    }
}
