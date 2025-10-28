using UnityEngine;

public class SingletonDontDestroy <T>: MonoBehaviour where T: MonoBehaviour
{
    private static T _instance;
    public static T GetInstance()
    {
        return _instance;
    }

    protected void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
