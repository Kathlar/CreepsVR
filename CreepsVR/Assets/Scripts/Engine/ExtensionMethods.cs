using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Adds custom methods to built in Unity components.
/// </summary>
public static class ExtensionMethods
{
    public static void ResetLocalTransform(this Transform trans)
    {
        trans.localPosition = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = Vector3.one;
    }

    public static Vector3 FlatY(this Vector3 vector3)
    {
        vector3.y = 0;
        return vector3;
    }
}