using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRPlayer : MonoBehaviour
{
    public Camera xrCamera { get; private set; }

    public XRController leftController { get; private set; }
    public XRController rightController { get; private set; }

    public float rotationSpeed = 80;

    private void Awake()
    {
        xrCamera = GetComponentInChildren<Camera>();

        foreach (XRController controller in GetComponentsInChildren<XRController>())
        {
            if (controller.side == HorizontalSide.left) leftController = controller;
            else if (controller.side == HorizontalSide.right) rightController = controller;
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, Inputs.RightHand.joystick.Value.x *
            Time.deltaTime * rotationSpeed);
    }
}
