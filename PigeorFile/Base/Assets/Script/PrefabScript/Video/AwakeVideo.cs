using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeVideo : VideoPrefabBase
{
    protected override void Finish()
    {
        UIManager.GetInstance().AwakeVideoDestroy();
    }
}