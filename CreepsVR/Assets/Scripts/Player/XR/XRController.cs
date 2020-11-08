using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRController : MonoBehaviour
{
    public LineRenderer raycastLine;

    public HorizontalSide side;

    private void Awake()
    {
        raycastLine = GetComponentInChildren<LineRenderer>();
    }
}
