using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetupInfo
{
    public int numberOfPlayers;
    public int numberOfCharacters;

    public LevelSetupInfo(int numberOfPlayers, int numberOfCharacters)
    {
        this.numberOfPlayers = numberOfPlayers;
        this.numberOfCharacters = numberOfCharacters;
    }

    public static LevelSetupInfo DefaultLevelSetupInfo()
    {
        return new LevelSetupInfo(2, 2);
    }
}
