using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGod : Character
{
    public void Button_Ready()
    {
        canvas.gameObject.SetActive(false);
        LevelFlow.SetTurnPart(LevelFlow.TurnPart.characterChoice);
    }
}
