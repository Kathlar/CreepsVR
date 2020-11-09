using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDatabase", menuName = "Kathlar/_GameDatabase")]
public class Database : ScriptableObject
{
    public static Database database { get { return Game.GameDatabase; } }
    public List<PlayerInformation> playerInfos = new List<PlayerInformation>();
    public static List<PlayerInformation> PlayerInfos { get { return database.playerInfos; } }
    [System.Serializable]
    public class LevelInformations
    {
        public LevelInformation mainMenuLevelInfo;
        public List<LevelInformation> gameLevelInfos = new List<LevelInformation>();
    }
    public LevelInformations levelInformations;
    public static LevelInformations Levels { get { return database.levelInformations; } }
    
    public List<GameObject> weaponPrefabs = new List<GameObject>();
    public static List<GameObject> WeaponPrefabs { get { return database.weaponPrefabs; } }
}

[System.Serializable]
public class PlayerInformation
{
    public Color color = Color.white;
    public Material material;
    public Material transparentMaterial;
}

[System.Serializable]
public class LevelInformation
{
    public string sceneAssetName;
    public string levelName;
}
