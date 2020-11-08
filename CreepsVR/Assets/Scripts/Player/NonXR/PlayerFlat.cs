using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlat : Player
{
    public override PlayerType playerType { get { return PlayerType.flat; } }

    protected override void Update()
    {
        rotationValue = Inputs.SecondaryHorizontal;

        base.Update();
    }
}
