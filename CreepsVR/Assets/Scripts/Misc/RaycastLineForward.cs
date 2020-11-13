using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastLineForward : MonoBehaviour
{
    public LineRenderer line { get; private set; }

    private void Awake()
    {
        line = GetComponentInChildren<LineRenderer>();
        line.positionCount = 2;
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        line.SetPosition(0, transform.position);
        if (Physics.Raycast(ray, out RaycastHit hit))
            line.SetPosition(1, hit.point);
        else
            line.SetPosition(1, transform.position + transform.forward * 500);
    }
}
