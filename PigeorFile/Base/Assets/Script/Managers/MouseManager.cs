using UnityEngine;

public class MouseManager : SingletonDontDestroy<MouseManager>
{
    #region Property

    public MouseMode MouseMode { get; private set; } = MouseMode.ORIGIN;

    #endregion

    private void Start()
    {
        MessageRegister();
    }
    
    #region Message

    private void MessageRegister()
    {
        MessageManager.GetInstance().Register(MessageTypes.SwitchMouseMode, OnSwitchMouseMode);
        MessageManager.GetInstance().Register(MessageTypes.ShowDetail, OnShowDetail);
    }

    private void OnSwitchMouseMode(Message message) //修改鼠标样式
    {
        if (message is SwitchMouseMode msg)
        {
            if (msg.MouseMode == MouseMode.ORIGIN && MouseMode != MouseMode.ORIGIN) Cursor.visible = true;
            else if (msg.MouseMode != MouseMode.ORIGIN && MouseMode == MouseMode.ORIGIN) Cursor.visible = false;
            UIManager.GetInstance().GetPrefab<Mouse>().UpdateIcon(UIManager.GetInstance().MouseSprite[(int)msg.MouseMode]);
            MouseMode = msg.MouseMode;
        }
    }

    private void OnShowDetail(Message message)
    {
        if (message is ShowDetail msg)
        {
            if (string.IsNullOrEmpty(msg.Detail))
            {
                UIManager.GetInstance().PrefabDestroy<DetailsPanelUI>();
            }
            else
            {
                UIManager.GetInstance().PrefabInit<DetailsPanelUI>(panel =>
                {
                    panel.Init(msg.Detail, msg.ComponentTransform, msg.Width);
                });
            }
        }
    }

    #endregion
}