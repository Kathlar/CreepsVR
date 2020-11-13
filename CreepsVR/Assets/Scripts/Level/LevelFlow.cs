using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelFlow : Singleton<LevelFlow>
{
    //LEVEL INFO
    public static LevelSetupInfo levelSetupInfo;
    private int currentTurnNumber = 1;

    //PLAYERS
    public List<PlayerInstance> players = new List<PlayerInstance>();
    private int currentPlayerNumber = -1;

    //CHARACTERS
    public Transform charactersParent;
    public CharacterGod characterGod;
    public GameObject characterSoldierPrefab;
    private List<SpawnPoint> soldierSpawnPoints = new List<SpawnPoint>();
    private CharacterSoldier currentSoldier;

    public enum TurnPart { turnStart, characterChoice, soldierWeaponChoice, soldierMovement, soliderAttack, other }
    private TurnPart turnPart = TurnPart.turnStart;

    public Camera nonVrCamera;

    public Vector2 clampMovementValuesX, clampMovementValuesZ;
    public static Vector2 ClampMovementValuesX { get { return Instance.clampMovementValuesX; } }
    public static Vector2 ClampMovementValuesZ { get { return Instance.clampMovementValuesZ; } }

    [Tooltip("Defines if player can choose characters, or if they are chosen randomly.")]
    public bool randomSoldierGame;

    protected override void SingletonAwake()
    {
        if (levelSetupInfo == null) levelSetupInfo = LevelSetupInfo.DefaultLevelSetupInfo();
        soldierSpawnPoints = FindObjectsOfType<SpawnPoint>().ToList();
        for (int i = 0; i < levelSetupInfo.numberOfPlayers; i++)
        {
            PlayerInstance player = new PlayerInstance(i, Database.PlayerInfos[i], Database.WeaponInformations.Clone());
            if (levelSetupInfo.infiniteAmmo) foreach (WeaponInformation w in player.weaponInformations.weapons)
                    w.usagesForStart = 999;
            players.Add(player);

            for (int j = 0; j < levelSetupInfo.numberOfCharacters; j++)
            {
                int randomSpawnPointNumber = Random.Range(0, soldierSpawnPoints.Count);
                CharacterSoldier soldier = Instantiate(characterSoldierPrefab,
                    soldierSpawnPoints[randomSpawnPointNumber].transform.position,
                    soldierSpawnPoints[randomSpawnPointNumber].transform.rotation).GetComponent<CharacterSoldier>();
                soldier.transform.SetParent(charactersParent);
                soldierSpawnPoints.RemoveAt(randomSpawnPointNumber);
                player.soldiers.Add(soldier);
                soldier.playerInstance = player;
            }
        }
    }

    private void Start()
    {
        SetTurnPart(TurnPart.turnStart);
        if (nonVrCamera) nonVrCamera.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Inputs.Escape.WasPressed || InputsVR.LeftHand.menuButton.WasPressed)
            Game.PauseUnpauseGame();
        if(Inputs.Space.WasPressed && nonVrCamera)
            nonVrCamera.gameObject.SetActive(!nonVrCamera.gameObject.activeSelf);
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
            //GOD VIEW
            case TurnPart.turnStart:
                do
                {
                    currentPlayerNumber++;
                    if (currentPlayerNumber >= levelSetupInfo.numberOfPlayers)
                    {
                        currentPlayerNumber = 0;
                        currentTurnNumber++;
                    }
                }
                while (players[currentPlayerNumber].dead);
                characterGod.SetAsPlayer();
                Game.Player.SetRaycast(true);
                Game.Player.SetHandMaterial(players[currentPlayerNumber].information.material);
                Game.Player.ResetRotation();
                foreach (PlayerInstance player in players)
                    foreach (CharacterSoldier soldier in player.soldiers)
                    {
                        soldier.SetInfoWindowSize(false);
                        soldier.ShowChoiceObject(false);
                    }
                characterGod.SetTurnInfoText(currentTurnNumber, currentPlayerNumber, players[currentPlayerNumber].information.color);
                break;

            case TurnPart.characterChoice:
                foreach (PlayerInstance player in players)
                    foreach (CharacterSoldier soldier in player.soldiers)
                        soldier.ShowChoiceObject(player.number == currentPlayerNumber);
                break;

            //SOLDIER VIEW
            case TurnPart.soldierWeaponChoice:
                characterGod.SetAsNotPlayer();
                foreach (CharacterSoldier soldier in players[currentPlayerNumber].soldiers)
                    soldier.ShowChoiceObject(false);
                foreach (PlayerInstance player in players)
                    foreach (CharacterSoldier soldier in player.soldiers)
                    {
                        soldier.SetInfoWindowSize(true);
                        soldier.HideChoiceObject();
                    }
                Game.Player.ResetRotation();
                break;

            case TurnPart.soldierMovement:
                Game.Player.SetRaycast(false);
                Game.Player.timer.SetTimer(15);
                break;
        }
    }

    public static void SetCurrentSoldier(CharacterSoldier soldier)
    {
        Instance.currentSoldier = soldier;
    }

    public static void OnTimerEnd()
    {
        switch (Instance.turnPart)
        {
            case TurnPart.soldierMovement:
                Instance.currentSoldier.StartAttackMode();
                break;
            case TurnPart.soliderAttack:
                Instance.currentSoldier.EndTurn();
                break;
        }
    }

    public static void NotifyOfSoldierDeath(CharacterSoldier soldier)
    {
        soldier.playerInstance.soldiers.Remove(soldier);
        if (soldier.playerInstance.soldiers.Count == 0)
        {
            soldier.playerInstance.dead = true;

            int numberOfDeadPlayers = 0;
            foreach (PlayerInstance player in Instance.players)
                if (player.dead) numberOfDeadPlayers++;

            if(numberOfDeadPlayers >= levelSetupInfo.numberOfPlayers - 1)
                Instance.LevelFinish();
        }
    }

    public void LevelFinish()
    {
        turnPart = TurnPart.other;

        //show end sequence
        Game.GoToMainMenu();
    }

    void OnDrawGizmosSelected()
    {
        Vector3 center = new Vector3((clampMovementValuesX.x + clampMovementValuesX.y) / 2, 0,
            (clampMovementValuesZ.x + clampMovementValuesZ.y) / 2);
        Vector3 size = new Vector3(Mathf.Abs(clampMovementValuesX.y - clampMovementValuesX.x), .5f,
            Mathf.Abs(clampMovementValuesZ.y - clampMovementValuesZ.x));
        Gizmos.DrawWireCube(center, size);
    }
}
