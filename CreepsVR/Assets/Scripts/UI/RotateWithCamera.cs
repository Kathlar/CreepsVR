using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithCamera : MonoBehaviour
{
    void Update()
    {
        if(Game.Player.playerType != Player.PlayerType.flat)
            transform.eulerAngles = new Vector3(0, Game.Player.mainCamera.transform.eulerAngles.y, 0);
    }
}
