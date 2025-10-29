using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

/// <summary>
/// 可以挂载在任何可交互的UI组件上。
/// 负责在鼠标进入/离开时，发送 ShowDetail 消息。
/// </summary>

public class Detail : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region SerializeField

    [FormerlySerializedAs("detailText")]
    [Header("细节信息")]
    [Tooltip("当鼠标悬停时需要显示的文本内容。使用 * 来分割多个框体。")]
    [SerializeField] [TextArea(3, 10)] private string DetailText;

    [Header("参数")]
    [Tooltip("面板宽度")]
    [SerializeField] private float PanelWidth = 300f;
    
    [Tooltip("判断显示位置阈值")]
    [SerializeField] private Vector2 PivotThresholds = new Vector2(0.7f, 0.3f);
    
    #endregion
    
    #region property

    private static Details _details; // 缓存Details，所有实例共享
    private const string DetailsPath = "ScriptableObject/Details"; // 数据库在Resources文件夹中的路径

    #endregion

    private void Awake() // 在脚本被加载时调用
    {
        if (_details == null)
        {
            _details = Resources.Load<Details>(DetailsPath);
            if (_details == null)
                Debug.LogError($"Detail 数据库未能在 'Assets/Resources/{DetailsPath}.asset' 找到！请检查路径。");
        }
    }
    
    public void AddDetail(string key)
    {
        string detail = _details.GetDetail(key);
        if (string.IsNullOrEmpty(detail)) return;
        if (DetailText.Length > 0) DetailText+="/";
        DetailText+=key+"*"+detail;
    }

    public void OnPointerEnter(PointerEventData eventData) //发送 ShowDetail消息
    {
        MessageManager.GetInstance().Send(MessageTypes.ShowDetail, new ShowDetail(DetailText,GetComponent<RectTransform>(),PanelWidth,PivotThresholds));
    }
    
    public void OnPointerExit(PointerEventData eventData) // 当鼠标指针离开此对象的范围时，清空显示的文本
    {
        MessageManager.GetInstance().Send(MessageTypes.ShowDetail, new ShowDetail(""));
    }
    
    private void OnDestroy() // 销毁时确保清空显示的文本
    {
        MessageManager.GetInstance().Send(MessageTypes.ShowDetail, new ShowDetail(""));
    }
}
