using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterGod : Character
{
    public Text turnText, playerText;

    public void Button_Ready()
    {
        canvas.gameObject.SetActive(false);
        LevelFlow.SetTurnPart(LevelFlow.TurnPart.characterChoice);
    }

    private void Update()
    {
        if (!isPlayer) return;

        Transform cameraTransform = Game.Player.mainCamera.transform;
        Vector2 moveInputValue = new Vector2(Mathf.Clamp(Inputs.MainHorizontal +
            InputsVR.LeftHand.joystick.Value.x, -1, 1), Mathf.Clamp(Inputs.MainVertical +
            InputsVR.LeftHand.joystick.Value.y, -1, 1));
        Vector3 walkVector = cameraTransform.forward.FlatY() * moveInputValue.y +
            cameraTransform.right.FlatY() * moveInputValue.x;
        Vector3 moveVector = walkVector * moveSpeed;

        transform.position += moveVector * Time.deltaTime;
    }
}
