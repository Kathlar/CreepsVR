using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : Singleton<Game>
{
    public Database database;
    public static Database GameDatabase { get { return Instance.database; } }

    public Player.PlayerType playerType = Player.PlayerType.flat;
    public GameObject flatPlayerPrefab, vrPlayerPrefab;
    protected Player player;
    public static Player Player { get { return Instance.player; } }

    private bool paused;

    protected override void SingletonAwake()
    {
        Player playerOnScene = FindObjectOfType<Player>();
        if (playerOnScene) player = playerOnScene;
        else
        {
            GameObject playerToSpawn = null;
            switch (playerType)
            {
                case Player.PlayerType.flat:
                    playerToSpawn = flatPlayerPrefab;
                    break;
                case Player.PlayerType.vr:
                    playerToSpawn = vrPlayerPrefab;
                    break;
            }
            player = Instantiate(playerToSpawn).GetComponentInChildren<Player>();
        }
    }

    public static void PauseUnpauseGame()
    {
        if (Instance.paused) UnpauseGame();
        else PauseGame();
    }

    public static void PauseGame()
    {
        Instance.paused = true;
        Player.PauseMenu(true);
        Time.timeScale = 0;
    }

    public static void UnpauseGame()
    {
        Instance.paused = false;
        Player.PauseMenu(false);
        Time.timeScale = 1;
    }

    public static void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void GoToMainMenu()
    {
        SceneManager.LoadScene(Database.Levels.mainMenuLevelInfo.sceneAssetName);
    }
}
