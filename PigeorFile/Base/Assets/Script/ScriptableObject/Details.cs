using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Details", menuName = "ScriptableObjects/Details", order = 2)]
public class Details : ScriptableObject ,ISerializationCallbackReceiver
{
    #region SerializeField

    [Serializable]
    public class DetailPair
    {
        public string Key; //关键字
        [TextArea(3, 10)]
        public string Detail; //对应的详细描述
    }
    
    [Header("所有Detail词条")]
    [SerializeField]
    private List<DetailPair> DetailPairs = new List<DetailPair>();

    #endregion

    #region Property

    private Dictionary<string, string> _details;
    
    #endregion

    public string GetDetail(string keyword) //由details获取详细文本的方法
    {
        return string.IsNullOrEmpty(keyword) ? null : _details.GetValueOrDefault(keyword);
    }

    public void OnBeforeSerialize() {} // 在 Unity 准备序列化（保存）此对象之前调用。

    public void OnAfterDeserialize()
    {
        _details ??= new Dictionary<string, string>();
        _details.Clear();
        foreach (var tmp in DetailPairs) // 遍历所有在 Inspector 中设置的词条
        {
            if (string.IsNullOrEmpty(tmp.Key)||_details.ContainsKey(tmp.Key))
                Debug.LogWarning($"DetailPairs Key '{tmp.Key}' 为空、null或重复。该条目已被跳过。", this);
            else
                _details.Add(tmp.Key, tmp.Detail);
        }
    }
}
