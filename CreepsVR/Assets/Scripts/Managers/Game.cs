using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : Singleton<Game>
{
    public Database database;
    public static Database GameDatabase { get { return Instance.database; } }

    public Player.PlayerType playerType = Player.PlayerType.flat;
    public GameObject flatPlayerPrefab, vrPlayerPrefab;
    protected Player player;
    public static Player Player { get { return Instance.player; } }

    protected override void SingletonAwake()
    {
        GameObject playerToSpawn = null;
        switch(playerType)
        {
            case Player.PlayerType.flat:
                playerToSpawn = flatPlayerPrefab;
                break;
            case Player.PlayerType.vr:
                playerToSpawn = vrPlayerPrefab;
                break;
        }
        player = Instantiate(playerToSpawn).GetComponent<Player>();
    }
}
