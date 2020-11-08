using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRPlayer : Player
{
    public XRController leftController { get; private set; }
    public XRController rightController { get; private set; }

    public float rotationSpeed = 80;

    protected override void Awake()
    {
        base.Awake();

        foreach (XRController controller in GetComponentsInChildren<XRController>())
        {
            if (controller.side == HorizontalSide.left) leftController = controller;
            else if (controller.side == HorizontalSide.right) rightController = controller;
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, InputsVR.RightHand.joystick.Value.x *
            Time.deltaTime * rotationSpeed);
    }
}
