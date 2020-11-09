using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Transform canvas;

    public void SetAsPlayer()
    {
        Transform playerTransform = Game.Player.transform.parent;
        playerTransform.SetParent(transform);
        playerTransform.ResetLocalTransform();

        canvas.eulerAngles = 
            new Vector3(0, Game.Player.mainCamera.transform.eulerAngles.y, 0);
    }
}
