using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRController : MonoBehaviour
{
    public LineRenderer raycastLine { get; private set; }
    public Animator animator { get; private set; }

    public HorizontalSide side;

    private void Awake()
    {
        raycastLine = GetComponentInChildren<LineRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        animator.SetFloat("Trigger", InputsVR.Hands[side].trigger.Value);
        animator.SetFloat("Grip", InputsVR.Hands[side].grip.Value);
    }
}
