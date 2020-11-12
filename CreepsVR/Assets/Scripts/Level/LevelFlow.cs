using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelFlow : Singleton<LevelFlow>
{
    public static LevelSetupInfo levelSetupInfo;
    private int currentTurnNumber = 1;
    private int currentPlayerNumber = -1;

    public CharacterGod characterGod;
    public GameObject characterSoldierPrefab;
    private Dictionary<int, List<CharacterSoldier>> soldiers = new Dictionary<int, List<CharacterSoldier>>();
    private List<SpawnPoint> soldierSpawnPoints = new List<SpawnPoint>();
    private CharacterSoldier currentSoldier;

    public enum TurnPart { turnStart, characterChoice, soldierWeaponChoice, soldierMovement, soliderAttack, other }
    private TurnPart turnPart = TurnPart.turnStart;

    public Camera nonVrCamera;
    private bool showingNonVRCamera;

    public Vector2 clampMovementValuesX, clampMovementValuesZ;
    public static Vector2 ClampMovementValuesX { get { return Instance.clampMovementValuesX; } }
    public static Vector2 ClampMovementValuesZ { get { return Instance.clampMovementValuesZ; } }

    protected override void SingletonAwake()
    {
        if (levelSetupInfo == null) levelSetupInfo = LevelSetupInfo.DefaultLevelSetupInfo();
        soldierSpawnPoints = FindObjectsOfType<SpawnPoint>().ToList();
        for (int i = 0; i < levelSetupInfo.numberOfPlayers; i++)
        {
            soldiers.Add(i, new List<CharacterSoldier>());
            for (int j = 0; j < levelSetupInfo.numberOfCharacters; j++)
            {
                int randomSpawnPointNumber = Random.Range(0, soldierSpawnPoints.Count);
                CharacterSoldier soldier = Instantiate(characterSoldierPrefab,
                    soldierSpawnPoints[randomSpawnPointNumber].transform.position,
                    soldierSpawnPoints[randomSpawnPointNumber].transform.rotation).GetComponent<CharacterSoldier>();
                soldier.transform.SetParent(characterGod.transform.parent);
                soldierSpawnPoints.RemoveAt(randomSpawnPointNumber);
                soldiers[i].Add(soldier);
                soldier.playerNumber = i;
            }
        }
    }

    public bool timerGame;
    public static bool TimerGame { get { return Instance.timerGame; } }

    public bool destructableGame;
    public static bool DestructableGame { get { return Instance.destructableGame; } }

    private void Start()
    {
        SetTurnPart(TurnPart.turnStart);
        if (nonVrCamera) nonVrCamera.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Inputs.Escape.WasPressed || InputsVR.LeftHand.menuButton.WasPressed)
        {
            Game.PauseUnpauseGame();
        }

        if(Inputs.Space.WasPressed && nonVrCamera)
        {
            showingNonVRCamera = !showingNonVRCamera;
            nonVrCamera.gameObject.SetActive(showingNonVRCamera);
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
                //Game.Player.LookAt(Vector3.zero);
                currentPlayerNumber++;
                if(currentPlayerNumber >= levelSetupInfo.numberOfPlayers)
                {
                    currentPlayerNumber = 0;
                    currentTurnNumber++;
                }
                characterGod.transform.localPosition = Vector3.zero;
                characterGod.SetAsPlayer();
                characterGod.canvas.gameObject.SetActive(true);
                Game.Player.SetRaycast(true);
                Game.Player.SetHandMaterial(Database.PlayerInfos[currentPlayerNumber].material);
                foreach (var s in soldiers.Values)
                    foreach (CharacterSoldier soldier in s)
                    {
                        soldier.SetInfoWindowSize(false);
                        soldier.SetChoice(false);
                    }
                characterGod.turnText.text = "TURN " + currentTurnNumber.ToString();
                characterGod.playerText.text = "PLAYER " + (currentPlayerNumber + 1).ToString();
                characterGod.playerText.color = Database.PlayerInfos[currentPlayerNumber].color;
                break;
            case TurnPart.characterChoice:
                for(int i = 0; i < soldiers.Count; i++)
                    foreach(CharacterSoldier soldier in soldiers[i])
                        soldier.SetChoice(i == currentPlayerNumber);
                break;

            case TurnPart.soldierWeaponChoice:
                characterGod.SetAsNotPlayer();
                foreach (CharacterSoldier soldier in soldiers[currentPlayerNumber])
                    soldier.SetChoice(false);
                foreach (var s in soldiers.Values)
                    foreach (CharacterSoldier soldier in s)
                    {
                        soldier.SetInfoWindowSize(true);
                        soldier.HideChoice();
                    }
                break;
            case TurnPart.soldierMovement:
                Game.Player.SetRaycast(false);
                Game.Player.timer.SetTimer(15);
                break;
        }
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

    public static void SetCurrentSoldier(CharacterSoldier soldier)
    {
        Instance.currentSoldier = soldier;
    }

    public static void NotifyOfDeath(CharacterSoldier soldier)
    {
        Instance.soldiers[soldier.playerNumber].Remove(soldier);
        if (Instance.soldiers[soldier.playerNumber].Count == 0)
            Instance.LevelFinish();
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
