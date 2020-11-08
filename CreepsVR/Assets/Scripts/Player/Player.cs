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

    protected abstract Transform rayCastPoint { get; }
    private bool raycastOn;
    protected abstract LineRenderer raycastLine { get; }

    protected virtual void Awake()
    {
        mainCamera = GetComponentInChildren<Camera>();
    }

    protected virtual void Start()
    {
        raycastLine.positionCount = 2;
    }

    protected virtual void Update()
    {
        transform.Rotate(Vector3.up, rotationValue * Time.deltaTime * rotationSpeed);

        raycastLine.SetPosition(0, rayCastPoint.position);
        Ray ray = new Ray(rayCastPoint.position, rayCastPoint.forward);
        if(Physics.Raycast(ray, out RaycastHit rayHit))
        {
            raycastLine.SetPosition(1, rayHit.point);
        }
        else
        {
            raycastLine.SetPosition(1, rayCastPoint.position + rayCastPoint.forward * 1000);
        }
    }
}
