using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Explosive))]
public class Grenade : Throwable
{
    public Explosive explosive { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        explosive = GetComponent<Explosive>();
    }

    public override void UseEnd()
    {
        base.UseEnd();
        explosive.Explode();
    }

    public override bool StillUsing()
    {
        return !explosive.exploded;
    }
}
