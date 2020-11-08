using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlat : Player
{
    protected override void Update()
    {
        rotationValue = Inputs.SecondaryHorizontal;

        base.Update();
    }
}
