using UnityEngine;

public abstract class SingletonBase: MonoBehaviour
{
    public abstract void In�t();
}

public abstract class Singleton<T> : SingletonBase where T : Component
{
    public static T Instance { get; protected set; }

    public override void In�t()
    {
        if (Instance == null)
        {
            Instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

public abstract class DontDestroySingleton<T> : Singleton<T> where T : Component
{
    public override void In�t()
    {
        if(Instance == null)
        {
            Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }
}
