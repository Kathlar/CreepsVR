using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Throwable : Item
{
    private Rigidbody rb;

    private List<Vector3> positions = new List<Vector3>();

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
    }

    public override void UseStart()
    {
        base.UseStart();
        positions.Add(transform.position);
    }

    public override void UseContinue()
    {
        base.UseContinue();
        positions.Add(transform.position);
        if (positions.Count > 5) positions.RemoveAt(0);
    }

    public override void UseEnd()
    {
        base.UseEnd();
        transform.SetParent(null);
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.AddForce((transform.position - positions[0]) * 2500);
    }
}
