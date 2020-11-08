using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    public Camera mainCamera { get; private set; }

    protected virtual void Awake()
    {
        mainCamera = GetComponentInChildren<Camera>();
    }
}
