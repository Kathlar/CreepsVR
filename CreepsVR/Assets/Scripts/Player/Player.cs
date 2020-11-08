using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    public Camera mainCamera { get; private set; }

    public enum PlayerType { flat, vr }
    public abstract PlayerType playerType { get; }

    public float rotationSpeed = 80;
    protected float rotationValue;

    protected virtual void Awake()
    {
        mainCamera = GetComponentInChildren<Camera>();
    }

    protected virtual void Update()
    {
        transform.Rotate(Vector3.up, rotationValue * Time.deltaTime * rotationSpeed);
    }
}
