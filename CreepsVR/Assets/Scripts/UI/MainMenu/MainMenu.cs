using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Windows")]
    public GameObject mainMenuWindow;
    public GameObject startGameWindow, settingsWindow, creditsWindow;
    [Header("Texts")]
    public Text levelNameText;
    public Text numberOfPlayersText, numberOfCharactersText;
    [Header("Other")]
    public List<Image> playerInfoTabs = new List<Image>();
    public UIToggle timerToggle, destructionToggle, infiniteAmmoToggle;

    protected int numberOfPlayers = 2, numberOfCharacters = 3;
    protected bool timerGame, enviroDestructionGame = true, infiniteAmmoGame;

    private int currentChosenLevelNumber = 0;

    public GameObject loadingIconObject;

    private const string prefsSet = "PrefsSet";
    private const string numberOfPlayersPref = "numberOfPlayers", numberOfCharactersPref = "numberOfCharacters";
    private const string timerGamePref = "timerGame", destructionGamePref = "destructionGame", infiniteAmmoPref = "infiniteAmmo";

    private void Start()
    {
        if (PlayerPrefs.GetInt(prefsSet) == 0)
            SavePreferences();
        else LoadPreferences();

        mainMenuWindow.SetActive(true);
        startGameWindow.SetActive(false);
        settingsWindow.SetActive(false);
        creditsWindow.SetActive(false);

        numberOfPlayersText.text = numberOfPlayers.ToString();
        numberOfCharactersText.text = numberOfCharacters.ToString();
        levelNameText.text = Database.Levels.gameLevelInfos[currentChosenLevelNumber].levelName;

        for (int i = 0; i < playerInfoTabs.Count; i++)
        {
            playerInfoTabs[i].color = Database.PlayerInfos[i].color;
            playerInfoTabs[i].GetComponentInChildren<Text>().text = "Player " + (i + 1).ToString();
        }

        for (int i = numberOfPlayers; i < playerInfoTabs.Count; i++)
            playerInfoTabs[i].gameObject.SetActive(false);

        timerToggle.ChangeValue(timerGame);
        destructionToggle.ChangeValue(enviroDestructionGame);
        infiniteAmmoToggle.ChangeValue(infiniteAmmoGame);
    }

    private void OnDestroy()
    {
        SavePreferences();
    }

    public void Button_StartGameWindow()
    {
        if (Time.timeSinceLevelLoad < .5f) return;
        startGameWindow.SetActive(true);
        mainMenuWindow.SetActive(false);
    }

    public void Button_SettingsWindow()
    {
        if (Time.timeSinceLevelLoad < .5f) return;
        settingsWindow.SetActive(true);
        mainMenuWindow.SetActive(false);
    }

    public void Button_CreditsWindow()
    {
        if (Time.timeSinceLevelLoad < .5f) return;
        creditsWindow.SetActive(true);
        mainMenuWindow.SetActive(false);
    }

    public void Button_Back()
    {
        SavePreferences();
        mainMenuWindow.SetActive(true);
        startGameWindow.SetActive(false);
        settingsWindow.SetActive(false);
        creditsWindow.SetActive(false);
    }

    public void Button_ExitGame()
    {
        SavePreferences();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Button_ChangePlayers(bool more)
    {
        numberOfPlayers = Mathf.Clamp(numberOfPlayers + (more ? 1 : -1), 2, 4);
        numberOfPlayersText.text = numberOfPlayers.ToString();

        for (int i = 0; i < numberOfPlayers; i++)
            playerInfoTabs[i].gameObject.SetActive(true);
        for (int i = numberOfPlayers; i < playerInfoTabs.Count; i++)
            playerInfoTabs[i].gameObject.SetActive(false);
    }

    public void Button_ChangeCharacters(bool more)
    {
        numberOfCharacters = Mathf.Clamp(numberOfCharacters + (more ? 1 : -1), 1, 5);
        numberOfCharactersText.text = numberOfCharacters.ToString();
    }

    public void Button_ChangeLevel(bool next)
    {
        currentChosenLevelNumber += next ? 1 : -1;
        int numberOfLevels = Database.Levels.gameLevelInfos.Count;
        if (currentChosenLevelNumber < 0) currentChosenLevelNumber = numberOfLevels - 1;
        else if (currentChosenLevelNumber >= numberOfLevels) currentChosenLevelNumber = 0;
        levelNameText.text = Database.Levels.gameLevelInfos[currentChosenLevelNumber].levelName;
    }

    public void Toggle_TimerSetting(bool on)
    {
        timerGame = on;
    }

    public void Toggle_DestructionSetting(bool on)
    {
        enviroDestructionGame = on;
    }

    public void Toggle_InfiniteAmmoSetting(bool on)
    {
        infiniteAmmoGame = on;
    }

    public void Button_StartGame()
    {
        SavePreferences();

        LevelSetupInfo setupInfo = new LevelSetupInfo(numberOfPlayers, numberOfCharacters, timerGame, enviroDestructionGame, infiniteAmmoGame);
        LevelFlow.levelSetupInfo = setupInfo;
        loadingIconObject.SetActive(true);
        transform.parent.gameObject.SetActive(false);
        Game.LoadSceneWithLoadingScreen(Database.Levels.gameLevelInfos[currentChosenLevelNumber].sceneAssetName);
    }

    private void SavePreferences()
    {
        PlayerPrefs.SetInt(prefsSet, 1);

        PlayerPrefs.SetInt(numberOfPlayersPref, numberOfPlayers);
        PlayerPrefs.SetInt(numberOfCharactersPref, numberOfCharacters);

        PlayerPrefs.SetInt(timerGamePref, timerGame ? 1 : 0);
        PlayerPrefs.SetInt(destructionGamePref, enviroDestructionGame ? 1 : 0);
        PlayerPrefs.SetInt(infiniteAmmoPref, infiniteAmmoGame ? 1 : 0);
    }

    private void LoadPreferences()
    {
        numberOfPlayers = PlayerPrefs.GetInt(numberOfPlayersPref, 2);
        numberOfCharacters = PlayerPrefs.GetInt(numberOfCharactersPref, 3);

        timerGame = PlayerPrefs.GetInt(timerGamePref, 0) == 1;
        enviroDestructionGame = PlayerPrefs.GetInt(destructionGamePref, 0) == 1;
        infiniteAmmoGame = PlayerPrefs.GetInt(infiniteAmmoPref, 0) == 1;
    }
}
