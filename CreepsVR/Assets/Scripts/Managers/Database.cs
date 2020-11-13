using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ScriptableObject class representing database of the game.
/// </summary>
[CreateAssetMenu(fileName = "GameDatabase", menuName = "Kathlar/_GameDatabase")]
public class Database : ScriptableObject
{
    public static Database database { get { return Game.GameDatabase; } }

    [System.Serializable]
    public class LevelInformations
    {
        public LevelInformation mainMenuLevelInfo;
        public List<LevelInformation> gameLevelInfos = new List<LevelInformation>();
    }
    public LevelInformations levelInformations;
    public static LevelInformations Levels { get { return database.levelInformations; } }

    public List<PlayerInformation> playerInfos = new List<PlayerInformation>();
    public static List<PlayerInformation> PlayerInfos { get { return database.playerInfos; } }

    public WeaponInformations weaponInformations;
    public static WeaponInformations WeaponInformations { get { return database.weaponInformations; } }

    [System.Serializable]
    public class LayerInfo
    {
        public LayerMask UILayer;
        public LayerMask walkableLayers;
    }
    public LayerInfo layers;
    public static LayerInfo Layers { get { return database.layers; } }
}

/// <summary>
/// Information of a potential player taking part in game.
/// </summary>
[System.Serializable]
public class PlayerInformation
{
    public Color color = Color.white;
    public Material material;
    public Material transparentMaterial;

    public Material polygonPrototypeMaterial;
}

[System.Serializable]
public class PlayerInstance
{
    public PlayerInformation information;
    public int number;
    public bool dead;
    public WeaponInformations weaponInformations;
    public List<CharacterSoldier> soldiers = new List<CharacterSoldier>();

    public PlayerInstance(int number, PlayerInformation information, WeaponInformations weapons)
    {
        this.number = number;
        this.information = information;
        this.weaponInformations = weapons;
    }
}

/// <summary>
/// Information about a game level.
/// </summary>
[System.Serializable]
public class LevelInformation
{
    public string sceneAssetName;
    public string levelName;
}

/// <summary>
/// Information about weapon.
/// </summary>
[System.Serializable]
public class WeaponInformation
{
    public GameObject weaponPrefab;
    public int usagesForStart;
    public bool canBeDroppedInRandomCrate;

    public WeaponInformation Clone()
    {
        WeaponInformation newWI = new WeaponInformation();
        newWI.weaponPrefab = weaponPrefab;
        newWI.usagesForStart = usagesForStart;
        newWI.canBeDroppedInRandomCrate = canBeDroppedInRandomCrate;
        return newWI;
    }
}

/// <summary>
/// Information about player's weapons.
/// </summary>
[System.Serializable]
public class WeaponInformations
{
    public List<WeaponInformation> weapons = new List<WeaponInformation>();

    public WeaponInformations Clone()
    {
        WeaponInformations clone = new WeaponInformations();
        foreach(WeaponInformation wi in weapons)
        {
            clone.weapons.Add(wi.Clone());
        }
        return clone;
    }
}