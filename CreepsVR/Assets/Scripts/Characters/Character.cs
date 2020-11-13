using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Transform canvas;

    public float moveSpeed = 6;
    protected bool isPlayer;

    private bool canvasWasOn;

    public virtual void SetAsPlayer()
    {
        Transform playerTransform = Game.Player.transform.parent;
        playerTransform.SetParent(transform);
        playerTransform.ResetLocalTransform();
        Game.Player.SetRaycastSize(transform.localScale.y);

        canvas.eulerAngles = new Vector3(0, Game.Player.mainCamera.transform.eulerAngles.y, 0);

        isPlayer = true;
        Game.Player.currentCharacter = this;
    }

    public virtual void SetAsNotPlayer()
    {
        isPlayer = false;
    }

    public virtual void ShowCanvas()
    {
        if(canvasWasOn) canvas.gameObject.SetActive(true);
    }

    public virtual void HideCanvas()
    {
        canvasWasOn = canvas.gameObject.activeSelf;
        canvas.gameObject.SetActive(false);
    }
}
