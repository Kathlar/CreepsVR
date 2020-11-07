using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : Singleton<Game>
{
    public Database database;
    public static Database GameDatabase { get { return Instance.database; } }

    protected override void SingletonAwake()
    {

    }
}
