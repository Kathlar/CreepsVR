using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetupInfo
{
    public int numberOfPlayers;
    public int numberOfCharacters;
    public bool timerGame;
    public bool destructableGame;

    public LevelSetupInfo(int numberOfPlayers, int numberOfCharacters, bool timerGame, bool destructableGame)
    {
        this.numberOfPlayers = numberOfPlayers;
        this.numberOfCharacters = numberOfCharacters;
        this.timerGame = timerGame;
        this.destructableGame = destructableGame;
    }

    public static LevelSetupInfo DefaultLevelSetupInfo()
    {
        return new LevelSetupInfo(2, 2, false, true);
    }
}
