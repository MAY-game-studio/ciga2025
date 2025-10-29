using UnityEngine;
using UnityEngine.UI;

public class Mouse : UIPrefabBase
{
    #region SerializeField
    
    [Header("UI组件")]
    [SerializeField] private Image ImgMouse;
    
    [Header("拖尾粒子")]
    [SerializeField] private ParticleSystem TrailParticle; 
    
    #endregion

    #region property

    private Vector3 _lastPosition;
    private Camera _mainCamera;
    #endregion
    
    public void UpdateIcon(Sprite icon)
    {
        ImgMouse.sprite = icon;
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;// 获取鼠标在屏幕上的位置（单位：像素）
        Vector3 worldPosition = _mainCamera!.ScreenToWorldPoint(mousePosition);// 将屏幕坐标转为世界坐标
        worldPosition.z = 0;
        transform.position = worldPosition;
        float speed = (mousePosition - _lastPosition).magnitude / Time.deltaTime;// 计算鼠标速度：本帧和上一帧鼠标位置差 / 时间
        var tmp = TrailParticle.emission;
        tmp.enabled = speed > 0f;
        _lastPosition = worldPosition;
    }
}
