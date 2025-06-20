using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : SingletonDontDestory<MouseManager>
{
    #region Property

    private MouseMode _mouseMode = MouseMode.ORIGIN;

    public MouseMode MouseMode
    {
        get => _mouseMode;
        private set => _mouseMode = value;
    }

    #endregion

    void Start()
    {
        MessageRegister();
    }


    #region Message

    protected void MessageRegister()
    {
        MessageManager.GetInstance().Register(MessageTypes.SwitchMouseMode, OnSwitchMouseMode);
        MessageManager.GetInstance().Register(MessageTypes.ShowDetail, OnShowDetail);
    }

    public void OnSwitchMouseMode(Message message) //修改鼠标样式
    {
        if (message is SwitchMouseMode msg)
        {
            if (msg.MouseMode == MouseMode.ORIGIN && MouseMode != MouseMode.ORIGIN) Cursor.visible = true;
            else if (msg.MouseMode != MouseMode.ORIGIN && MouseMode == MouseMode.ORIGIN) Cursor.visible = false;
            UIManager.GetInstance().Mouse.UpdateIcon(UIManager.GetInstance().MouseSprite[(int)msg.MouseMode]);
            MouseMode = msg.MouseMode;
        }
    }

    public void OnShowDetail(Message message)
    {
        if (message is ShowDetail msg)
        {
            string[] parts = msg.Detail.Split('*');
            foreach (string part in parts)
            {
                Debug.Log(part);
                //todo UI适配展示
            }
        }
    }

    #endregion
}