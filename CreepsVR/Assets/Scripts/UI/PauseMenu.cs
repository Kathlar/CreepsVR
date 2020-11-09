using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void Button_ContinueGame()
    {
        Game.UnpauseGame();
    }

    public void Button_RestartGame()
    {
        Game.RestartLevel();
    }

    public void Button_ExitToMainMenu()
    {
        Game.GoToMainMenu();
    }
}
