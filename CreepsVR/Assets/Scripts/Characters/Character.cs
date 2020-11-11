using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Transform canvas;

    public float moveSpeed = 6;

    protected bool isPlayer;

    public void SetAsPlayer()
    {
        Transform playerTransform = Game.Player.transform.parent;
        playerTransform.SetParent(transform);
        playerTransform.ResetLocalTransform();
        Game.Player.SetRaycastSize(transform.localScale.y);

        canvas.eulerAngles = new Vector3(0, Game.Player.mainCamera.transform.eulerAngles.y, 0);

        isPlayer = true;
    }

    public void SetAsNotPlayer()
    {
        isPlayer = false;
    }
}
