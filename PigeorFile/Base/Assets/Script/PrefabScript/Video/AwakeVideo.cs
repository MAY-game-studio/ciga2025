public class AwakeVideo : VideoPrefabBase
{
    protected override void Finish()
    {
        UIManager.GetInstance().AwakeVideoDestroy();
    }
}