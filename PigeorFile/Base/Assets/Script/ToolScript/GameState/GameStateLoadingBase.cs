using System.Collections;
using UnityEngine;

public abstract class GameStateLoadingBase : IGameState
{
    #region property

    private bool _flagLoadComplete;

    #endregion
    
    /// <summary>
    /// 负责展现加载界面，等待加载完成发送的LoadFinish消息后销毁加载界面
    /// </summary>
    public override void Enter()
    {
        GameManager.GetInstance().StartCoroutine(LoadingUIInit());
        _flagLoadComplete = false;
        MessageManager.GetInstance().Register(MessageTypes.LoadFinish, OnLoadFinish,0,MessageTemporaryType.Temporary); //注册监听
    }
    
    protected abstract void LoadingProcess(); // 抽象加载逻辑函数
    
    private IEnumerator LoadingUIInit()
    {
        UIManager.GetInstance().PrefabInit<LoadingUI>(); //创建LoadingUI作为加载界面
        yield return new WaitForSecondsRealtime(1f); //最短等待时间，确保加载界面有足够时间显示
        LoadingProcess();
        yield return new WaitUntil(() => _flagLoadComplete); //等待加载完成
        UIManager.GetInstance().PrefabDestroy<LoadingUI>(); //销毁LoadingUI
    }

    public override void Exit() { }

    private void OnLoadFinish(Message msg)
    {
        MessageManager.GetInstance().Remove(MessageTypes.LoadFinish, OnLoadFinish); //移除监听
        _flagLoadComplete = true;
    }
}