using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlat : Player
{
    protected override Transform raycastPoint { get { return mainCamera.transform; } }

    public override PlayerType playerType { get { return PlayerType.flat; } }

    private float rotationX, rotationY;

    protected override void Start()
    {
        base.Start();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    protected override void Update()
    {
        rotationValue = Inputs.SecondaryHorizontal;

        base.Update();

        rotationX += Inputs.SecondaryVertical * rotationSpeed;
        rotationY += Inputs.SecondaryHorizontal * rotationSpeed;
        rotationX = Mathf.Clamp(rotationX, -90, 90);
        mainCamera.transform.localEulerAngles = new Vector3(-rotationX, rotationY, 0);

        if (lastClickable != null & Inputs.LeftMouse.WasPressed) lastClickable.OnClick();
    }
}
