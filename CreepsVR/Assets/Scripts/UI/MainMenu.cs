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

    protected int numberOfPlayers = 2, numberOfCharacters = 3;

    private int currentChosenLevelNumber = 0;

    private void Start()
    {
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
    }

    public void Button_StartGameWindow()
    {
        startGameWindow.SetActive(true);
        mainMenuWindow.SetActive(false);
    }

    public void Button_SettingsWindow()
    {
        settingsWindow.SetActive(true);
        mainMenuWindow.SetActive(false);
    }

    public void Button_CreditsWindow()
    {
        creditsWindow.SetActive(true);
        mainMenuWindow.SetActive(false);
    }

    public void Button_Back()
    {
        mainMenuWindow.SetActive(true);
        startGameWindow.SetActive(false);
        settingsWindow.SetActive(false);
        creditsWindow.SetActive(false);
    }

    public void Button_ExitGame()
    {
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

    public void Button_StartGame()
    {
        LevelSetupInfo setupInfo = new LevelSetupInfo(numberOfPlayers, numberOfCharacters);
        LevelFlow.levelSetupInfo = setupInfo;
        SceneManager.LoadScene(Database.Levels.gameLevelInfos[currentChosenLevelNumber].sceneAssetName);
    }
}
