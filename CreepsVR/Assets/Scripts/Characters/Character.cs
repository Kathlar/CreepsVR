using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Canvas canvas;

    public void SetAsPlayer()
    {
        Transform playerTransform = Game.Player.transform.parent;
        playerTransform.SetParent(transform);
        playerTransform.ResetLocalTransform();
    }
}
