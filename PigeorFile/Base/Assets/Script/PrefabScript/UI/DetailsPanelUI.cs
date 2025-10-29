using UnityEngine;

public class DetailsPanelUI : UIPrefabBase
{
    #region SerializeField
    
    [Header("UI预制体")]
    [Tooltip("DetailUI子预制体")]
    [SerializeField]
    private GameObject DetailUIPrefab;

    [Header("参数")]
    
    [Tooltip("和原组件的偏移距离")]
    [SerializeField]
    private float Offset = 10f;
    
    #endregion

    #region property
    
    private Vector2 _pivotThresholds; //判断显示位置的阈值
    private RectTransform _rectTransform;
    
    #endregion
    private void RePosition(RectTransform componentTransform) // 根据相对位置设置轴心
    {
        Vector3[] anchorCorners = new Vector3[4];
        componentTransform.GetLocalCorners(anchorCorners); //获取组件的本地四角
        RectTransform canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        Vector3 topLeft = canvasRect.InverseTransformPoint(componentTransform.TransformPoint(anchorCorners[1])); // 左上角的当前canvas位置
        Vector3 bottomRight = canvasRect.InverseTransformPoint(componentTransform.TransformPoint(anchorCorners[3])); // 右下角的当前canvas位置
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, componentTransform.position); // 获取组件的屏幕位置
        Vector3 targetLocalPosition = new Vector3(bottomRight.x + Offset,topLeft.y - Offset,0f); //默认从右上角向下显示
        Vector2 targetPivot = new Vector2(0f,1f);
        if (screenPoint.x > _pivotThresholds.x * Screen.width) //显示在目标左侧
        {
            targetPivot.x = 1f;
            targetLocalPosition.x = topLeft.x - Offset;
        }
        if (screenPoint.y < _pivotThresholds.y * Screen.height) //从目标顶部向上显示
        {
            targetPivot.y = 0f;
        }
        _rectTransform.pivot = targetPivot;
        _rectTransform.localPosition = targetLocalPosition;
    }
    
    public void Init(string details, RectTransform componentTransform, float width, Vector2 pivotThresholds) //初始化Details面板
    {
        _pivotThresholds = pivotThresholds;
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