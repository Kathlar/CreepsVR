using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Core manager of the game, holds information about main objects in game and takes care of scene management.
/// </summary>
public class Game : Singleton<Game>
{
    public Database database;
    public static Database GameDatabase { get { return Instance.database; } }

    public Player.PlayerType playerTypeToSpawn = Player.PlayerType.flat;
    public GameObject flatPlayerPrefab, vrPlayerPrefab;
    protected Player player;
    public static Player Player { get { return Instance.player; } }

    private bool paused;
    public static bool Paused { get { return Instance.paused; } }
    private bool raycastOnBeforePause;

    public AsyncOperation operation;
    public static AsyncOperation Operation { get { return Instance.operation; } }

    protected override void SingletonAwake()
    {
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        playerTypeToSpawn = Player.PlayerType.flat;
#endif

        Player playerOnScene = FindObjectOfType<Player>();
        if (playerOnScene) player = playerOnScene;
        else
        {
            GameObject playerToSpawn = null;
            switch (playerTypeToSpawn)
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

    /// <summary>
    /// Pauses the game if it's not, unpauses the game if it's paused.
    /// </summary>
    public static void PauseUnpauseGame()
    {
        if (Instance.paused) UnpauseGame();
        else PauseGame();
    }

    public static void PauseGame()
    {
        Instance.paused = true;
        Player.ShowPauseMenu(true);
        Instance.raycastOnBeforePause = Player.raycastOn;
        Player.SetRaycast(true);
        Time.timeScale = 0;
    }

    public static void UnpauseGame()
    {
        Instance.paused = false;
        Player.ShowPauseMenu(false);
        Player.SetRaycast(Instance.raycastOnBeforePause);
        Time.timeScale = 1;
    }

    public static void RestartLevel()
    {
        UnpauseGame();
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void GoToMainMenu()
    {
        LoadScene(Database.Levels.mainMenuLevelInfo.sceneAssetName);
    }

    public static void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public static void LoadSceneWithLoadingScreen(string name)
    {
        Instance.operation = SceneManager.LoadSceneAsync(name);
    }
}
