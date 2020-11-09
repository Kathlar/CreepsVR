using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    void Update()
    {
        Vector3 lookAtVec = Game.Player.mainCamera.transform.position;
        //lookAtVec.y = transform.position.y;
        transform.rotation = Quaternion.LookRotation(transform.position - lookAtVec);
    }
}
