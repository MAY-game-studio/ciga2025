using UnityEngine;
using UnityEngine.Serialization;

public class DetailsPanelUI : UIPrefabBase
{
    #region SerializeField
    
    [Header("UI预制体")]
    [Tooltip("DetailUI子预制体")]
    [SerializeField]
    private GameObject DetailUIPrefab;

    [Header("参数")]
    [Tooltip("切换显示位置的阈值")]
    [SerializeField]
    private Vector2 PivotThresholds = new Vector2(0.7f, 0.7f);
    
    [FormerlySerializedAs("Padding")]
    [Tooltip("和原组件的偏移距离")]
    [SerializeField]
    private float Offset = 10f;
    
    #endregion

    private RectTransform _rectTransform;
    
    private void RePosition(RectTransform componentTransform) // 根据相对位置设置轴心
    {
        Vector3[] anchorCorners = new Vector3[4];
        componentTransform.GetLocalCorners(anchorCorners); //获取组件的本地四角
        RectTransform canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        Vector3 topLeft = canvasRect.InverseTransformPoint(componentTransform.TransformPoint(anchorCorners[1])); // 左上角的当前canvas位置
        Vector3 topRight = canvasRect.InverseTransformPoint(componentTransform.TransformPoint(anchorCorners[2])); // 右上角的当前canvas位置

        Vector3 targetLocalPosition;
        Vector2 targetPivot;
        if ((RectTransformUtility.WorldToScreenPoint(Camera.main, componentTransform.position).x / Screen.width) < PivotThresholds.x) //根据阈值判定显示在那侧
        {
            targetPivot = new Vector2(0f, 1f);
            targetLocalPosition = topRight + new Vector3(Offset, 0f, 0f); //向右增加偏移
        }
        else
        {
            targetPivot = new Vector2(1f, 1f);
            targetLocalPosition = topLeft - new Vector3(Offset, 0f, 0f); //向左增加偏移
        }
        _rectTransform.pivot = targetPivot;
        _rectTransform.localPosition = targetLocalPosition;
    }
    
    public void Init(string details, RectTransform componentTransform, float width) //初始化Details面板
    {
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        string[] blocks = details.Split('/'); // 拆分不同的DetailUI
        foreach (string block in blocks) // 解析文本并实例化所有 DetailUI (子预制体)
        {
            string[] parts = block.Split('*'); // 拆分DetailUI的key和detail
            if (parts.Length == 2)
                InstantiateDetailUI(description: parts[1], key: parts[0]); //包含key和detail
            else if (parts.Length == 1)
                InstantiateDetailUI(description: parts[0], key: null); //仅有detail
        }
        RePosition(componentTransform); //重新定位，此条必须在所有DetailUI实例化后调用
    }

    private void InstantiateDetailUI(string description, string key) //实例化DetailUI
    {
        GameObject detailUI = Instantiate(DetailUIPrefab, transform);
        detailUI.GetComponent<DetailUI>().Init(description, key);
    }
}