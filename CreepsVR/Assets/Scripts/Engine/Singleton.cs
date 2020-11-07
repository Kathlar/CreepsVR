using UnityEngine;

public abstract class Singleton<T> : SingletonBase where T : SingletonBase
{
    private static T instance;
    protected static T Instance { get { return instance; } }

    protected sealed override void Awake()
    {
        instance = this as T;
        base.Awake();
    }
}

public abstract class SingletonBase : MonoBehaviour
{
    protected virtual void Awake()
    {
        SingletonAwake();
    }

    protected abstract void SingletonAwake();
}