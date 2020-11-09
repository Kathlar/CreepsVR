using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFlow : Singleton<LevelFlow>
{
    public static LevelSetupInfo levelSetupInfo;
    private int currentTurnNumber = 1;
    private int currentPlayerNumber = -1;

    public CharacterGod characterGod;
    public GameObject characterSoldierPrefab;
    private Dictionary<int, List<CharacterSoldier>> soldiers = 
        new Dictionary<int, List<CharacterSoldier>>();
    public List<Transform> spawnPoints = new List<Transform>();

    public enum TurnPart { turnStart, characterChoice, weaponChoice, movement, attack }
    private TurnPart turnPart = TurnPart.turnStart;

    protected override void SingletonAwake()
    {
        if (levelSetupInfo == null) levelSetupInfo = LevelSetupInfo.DefaultLevelSetupInfo();
        for (int i = 0; i < levelSetupInfo.numberOfPlayers; i++)
        {
            soldiers.Add(i, new List<CharacterSoldier>());
            for (int j = 0; j < levelSetupInfo.numberOfCharacters; j++)
            {
                int randomSpawnPointNumber = Random.Range(0, spawnPoints.Count);
                CharacterSoldier soldier = Instantiate(characterSoldierPrefab,
                    spawnPoints[randomSpawnPointNumber].position,
                    spawnPoints[randomSpawnPointNumber].rotation).GetComponent<CharacterSoldier>();
                soldier.transform.SetParent(characterGod.transform.parent);
                spawnPoints.RemoveAt(randomSpawnPointNumber);
                soldiers[i].Add(soldier);
                soldier.playerNumber = i;
            }
        }
    }

    private void Start()
    {
        SetTurnPart(TurnPart.turnStart);
    }

    private void Update()
    {
        if (Inputs.Escape.WasPressed || InputsVR.LeftHand.menuButton.WasPressed)
        {
            Game.PauseUnpauseGame();
        }
    }

    public static void SetTurnPart(TurnPart part)
    {
        Instance.DoSetTurnPart(part);
    }

    private void DoSetTurnPart(TurnPart part)
    {
        turnPart = part;

        switch(turnPart)
        {
            case TurnPart.turnStart:
                currentPlayerNumber++;
                if(currentPlayerNumber >= levelSetupInfo.numberOfPlayers)
                {
                    currentPlayerNumber = 0;
                    currentTurnNumber++;
                }
                characterGod.SetAsPlayer();
                characterGod.myCanvas.SetActive(true);
                Game.Player.SetRaycast(true);
                break;
            case TurnPart.characterChoice:
                for(int i = 0; i < soldiers.Count; i++)
                {
                    foreach(CharacterSoldier soldier in soldiers[i])
                    {
                        soldier.SetChoice(i == currentPlayerNumber);
                    }
                }
                break;
            case TurnPart.weaponChoice:
                foreach (CharacterSoldier soldier in soldiers[currentPlayerNumber])
                    soldier.SetChoice(false);
                break;
            case TurnPart.movement:
                Game.Player.SetRaycast(false);
                break;
            case TurnPart.attack:
                break;
        }
    }
}
