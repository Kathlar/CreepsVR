using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRController : MonoBehaviour
{
    public LineRenderer line { get; private set; }

    public HorizontalSide side;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }
}
