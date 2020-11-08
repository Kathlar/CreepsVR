using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRPlayer : Player
{
    public XRController leftController { get; private set; }
    public XRController rightController { get; private set; }

    public override PlayerType playerType { get { return PlayerType.vr; } }

    protected override void Awake()
    {
        base.Awake();

        foreach (XRController controller in GetComponentsInChildren<XRController>())
        {
            if (controller.side == HorizontalSide.left) leftController = controller;
            else if (controller.side == HorizontalSide.right) rightController = controller;
        }
    }

    protected override void Update()
    {
        rotationValue = InputsVR.RightHand.joystick.Value.x;

        base.Update();
    }
}
