using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeVideo : Video
{
    protected override void Finish()
    {
        UIManager.GetInstance().AwakeVideoDestroy();
    }
}