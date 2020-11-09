using UnityEngine;

/// <summary>
/// Base class of all managers, containg static Instance field of the one and only object of this type on the scene.
/// </summary>
/// <typeparam name="T">The class itself.</typeparam>
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

/// <summary>
/// Base class of Singleton class, allowing Singleton to make Awake method sealed.
/// </summary>
public abstract class SingletonBase : MonoBehaviour
{
    protected virtual void Awake()
    {
        SingletonAwake();
    }

    protected abstract void SingletonAwake();
}