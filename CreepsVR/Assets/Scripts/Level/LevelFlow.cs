using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFlow : Singleton<LevelFlow>
{
    public static LevelSetupInfo levelSetupInfo;
    private int currentTurnNumber = 1;
    private int currentPlayerNumber = 0;

    public GameObject characterSoldierPrefab;
    private Dictionary<int, List<Character>> soldiers = new Dictionary<int, List<Character>>();

    protected override void SingletonAwake()
    {
        //for(int i = 0; i < levelSetupInfo.numberOfPlayers; i++)
        //{
        //    soldiers.Add(i, new List<Character>());
        //    for (int j = 0; j < levelSetupInfo.numberOfCharacters; j++)
        //    {

        //    }
        //}
    }

    private void Update()
    {
        if (Inputs.Escape.WasPressed || InputsVR.LeftHand.menuButton.WasPressed)
        {
            Game.PauseUnpauseGame();
        }
    }
}
