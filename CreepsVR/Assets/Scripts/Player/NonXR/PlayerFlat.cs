using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlat : Player
{
    public override PlayerType playerType { get { return PlayerType.flat; } }

    protected override Transform rayCastPoint { get { return mainCamera.transform; } }
    private LineRenderer myRaycastLine;
    protected override LineRenderer raycastLine { get { return myRaycastLine; } }

    private float rotationX, rotationY;

    protected override void Awake()
    {
        base.Awake();
        myRaycastLine = GetComponentInChildren<LineRenderer>();
    }

    protected override void Update()
    {
        rotationValue = Inputs.SecondaryHorizontal;

        base.Update();

        rotationX += Inputs.SecondaryVertical * rotationSpeed;
        rotationY += Inputs.SecondaryHorizontal * rotationSpeed;
        rotationX = Mathf.Clamp(rotationX, -90, 90);
        mainCamera.transform.localEulerAngles = new Vector3(-rotationX, rotationY, 0);
    }
}
