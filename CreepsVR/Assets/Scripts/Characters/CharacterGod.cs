using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterGod : Character
{
    public Text turnText, playerText;

    public GameObject newTurnInfoObject, endGameObject;
    public Text endGameText;

    private void Start()
    {
        endGameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!isPlayer) return;

        Transform cameraTransform = Game.Player.mainCamera.transform;
        Vector2 moveInputValue = new Vector2(Mathf.Clamp(Inputs.MainHorizontal +
            InputsVR.LeftHand.joystick.Value.x, -1, 1), Mathf.Clamp(Inputs.MainVertical +
            InputsVR.LeftHand.joystick.Value.y, -1, 1));
        Vector3 walkVector = cameraTransform.forward.FlatY() * moveInputValue.y +
            cameraTransform.right.FlatY() * moveInputValue.x;
        Vector3 moveVector = walkVector * moveSpeed;

        Vector3 newPos = transform.position + moveVector * Time.fixedDeltaTime;
        newPos.x = Mathf.Clamp(newPos.x, LevelFlow.ClampMovementValuesX.x, LevelFlow.ClampMovementValuesX.y);
        newPos.z = Mathf.Clamp(newPos.z, LevelFlow.ClampMovementValuesZ.x, LevelFlow.ClampMovementValuesZ.y);
        transform.position = newPos;
    }

    public override void SetAsPlayer()
    {
        base.SetAsPlayer();
        transform.localPosition = Vector3.zero;
        canvas.gameObject.SetActive(true);
    }

    public void SetTurnInfoText(int turnNumber, int playerNumber, Color playerColor)
    {
        turnText.text = "TURN " + turnNumber.ToString();
        playerText.text = "PLAYER " + (playerNumber + 1).ToString();
        playerText.color = playerColor;
    }

    public void Button_Ready()
    {
        canvas.gameObject.SetActive(false);
        LevelFlow.SetTurnPart(LevelFlow.TurnPart.characterChoice);
    }

    public void Button_ExitGame()
    {
        Game.GoToMainMenu();
    }

    public void SetEndGame(string text)
    {
        newTurnInfoObject.SetActive(false);
        endGameObject.SetActive(true);
        endGameText.text = text;
    }
}
