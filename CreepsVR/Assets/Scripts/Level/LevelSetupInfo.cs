using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetupInfo
{
    public int numberOfPlayers;
    public int numberOfCharacters;
    public bool timerGame;
    public bool destructableGame;
    public bool infiniteAmmo;

    public LevelSetupInfo(int numberOfPlayers, int numberOfCharacters, bool timerGame, bool destructableGame, bool infiniteAmmo)
    {
        this.numberOfPlayers = numberOfPlayers;
        this.numberOfCharacters = numberOfCharacters;
        this.timerGame = timerGame;
        this.destructableGame = destructableGame;
        this.infiniteAmmo = infiniteAmmo;
    }

    public static LevelSetupInfo DefaultLevelSetupInfo()
    {
        return new LevelSetupInfo(2, 2, true, true, false);
    }
}
