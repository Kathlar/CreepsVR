using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGod : Character
{
    public GameObject myCanvas;

    public void Button_Ready()
    {
        myCanvas.SetActive(false);
        LevelFlow.SetTurnPart(LevelFlow.TurnPart.characterChoice);
    }
}
