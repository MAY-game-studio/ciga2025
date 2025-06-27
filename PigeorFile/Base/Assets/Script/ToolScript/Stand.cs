using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stand : MonoBehaviour
{
    private void OnDisable()
    {
        if (transform.parent != null && transform.parent.GetComponent<UIFadeOutAnim>() != null)
            transform.parent.GetComponent<UIFadeOutAnim>().BeforeDisable();
    }

    private void OnDestroy()
    {
        if (transform.parent != null && transform.parent.GetComponent<UIFadeOutAnim>() != null)
            transform.parent.GetComponent<UIFadeOutAnim>().BeforeDestroy();
    }
}
