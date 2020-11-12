using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRPlayer : Player
{
    public XRController leftController { get; private set; }
    public XRController rightController { get; private set; }
    public Dictionary<HorizontalSide, XRController> controllers { get; private set; } = new Dictionary<HorizontalSide, XRController>();
    private XRController activeController;

    protected override Transform raycastPoint { get { return activeController.transform; } }
    public override PlayerType playerType { get { return PlayerType.vr; } }

    protected override void Awake()
    {
        base.Awake();

        foreach (XRController controller in GetComponentsInChildren<XRController>())
        {
            if (controller.side == HorizontalSide.left) leftController = controller;
            else if (controller.side == HorizontalSide.right) rightController = controller;
            controllers.Add(controller.side, controller);
            controller.Set(this);
        }

        if (rightController) activeController = rightController;
        else activeController = leftController;
    }

    protected override void Update()
    {
        rotationValue = InputsVR.RightHand.joystick.Value.x;

        base.Update();

        if (InputsVR.LeftHand.trigger.Value > .1f)
            activeController = leftController;
        else if (InputsVR.RightHand.trigger.Value > .1f)
            activeController = rightController;

        if (raycastOn && lastClickable != null && InputsVR.Hands.ContainsKey(activeController.side) &&
        InputsVR.Hands[activeController.side].triggerButton.WasPressed)
            lastClickable.OnClick();
    }

    public override void EquipItem(Item item)
    {
        rightController.EquipItem(item);
        if (item.twoHanded) leftController.EquipAsSecond();
    }

    public override void UnequipItem()
    {
        leftController.UnequipItem();
        rightController.UnequipItem();
    }

    public override void SetHandMaterial(Material m = null)
    {
        base.SetHandMaterial(m);
        leftController.SetMaterial(m);
        rightController.SetMaterial(m);
    }

    public override void LookAt(Vector3 point)
    {
        //Vector3 myForward = transform.forward;
        //Vector3 cameraForward = mainCamera.transform.forward;
        //myForward.y = cameraForward.y = 0;
        //float angle = Vector3.Angle(myForward, cameraForward);
        //Quaternion rotation = Quaternion.LookRotation(-transform.position, Vector3.up);
        //rotation *= Quaternion.Euler(Vector3.up * (angle + 180));
        //transform.rotation = rotation;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5);
    }
}
