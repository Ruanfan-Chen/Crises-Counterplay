using System;
using System.Collections.Generic;
using UnityEngine;
using static Utility;

public class LevelManager
{
    private static readonly string tileTexturePath = "Sprites/TilesetFloor";

    private static int levelNum;

    public static string GetLevelName() => levelNum switch
    {
        0 => "tarin 1",
        1 => "tarin 2",
        2 => "tarin 3",
        3 => "thunder 1",
        4 => "thunder 2",
        5 => "thunder 3",
        6 => "rain 1",
        7 => "rain 2",
        8 => "rain 3",
        9 => "trainthunder",
        10 => "trainrain",
        11 => "thunderrain",
        _ => "Infinite"
    };

    public static float GetTimeLimit() => levelNum switch
    {
        0 => 20.0f,
        1 => 20.0f,
        2 => 45.0f,
        3 => 20.0f,
        4 => 15.0f,
        5 => 45.0f,
        6 => 20.0f,
        7 => 20.0f,
        8 => 45.0f,
        9 => 50.0f,
        10 => 50.0f,
        11 => 50.0f,
        _ => 10.0f
    };

    public static Vector2 GetMapSize() => new(75.0f, 50.0f);

    public static Sprite GetTile() => Resources.Load<Sprite>(tileTexturePath);

    public static bool GetSpawnVehicle() => levelNum switch
    {
        0 => true,
        1 => true,
        2 => true,
        3 => false,
        4 => false,
        5 => false,
        6 => false,
        7 => false,
        8 => false,
        9 => true,
        10 => true,
        11 => false,
        _ => true
    };

    public static bool GetSpawnElectricityField() => levelNum switch
    {
        0 => false,
        1 => false,
        2 => false,
        3 => true,
        4 => true,
        5 => true,
        6 => false,
        7 => false,
        8 => false,
        9 => true,
        10 => false,
        11 => true,
        _ => true
    };

    public static bool GetSpawnEnemy() => levelNum switch
    {
        0 => false,
        1 => false,
        2 => true,
        3 => false,
        4 => false,
        5 => true,
        6 => true,
        7 => true,
        8 => true,
        9 => false,
        10 => true,
        11 => true,
        _ => true
    };

    public static bool GetCharaterDisarm() => levelNum switch
    {
        0 => true,
        1 => true,
        2 => false,
        3 => true,
        4 => true,
        5 => false,
        6 => true,
        7 => true,
        8 => false,
        9 => false,
        10 => false,
        11 => false,
        _ => false
    };

    public static bool GetEnemieDisarm() => levelNum switch
    {
        0 => false,
        1 => false,
        2 => false,
        3 => false,
        4 => false,
        5 => false,
        6 => true,
        7 => true,
        8 => false,
        9 => false,
        10 => false,
        11 => false,
        _ => false
    };

    public static bool GetEnemieRain() => levelNum switch
    {
        0 => false,
        1 => false,
        2 => false,
        3 => false,
        4 => false,
        5 => false,
        6 => true,
        7 => true,
        8 => true,
        9 => false,
        10 => true,
        11 => true,
        _ => true
    };

    public static IReadOnlyList<OptionConfig> GetShopConfig() => levelNum switch
    {
        0 => new List<OptionConfig>() { OptionConfig.GRAVITYGRASP },
        3 => new List<OptionConfig>() { OptionConfig.SUPERCHARGE },
        6 => new List<OptionConfig>() { OptionConfig.SURFMANIA },
        _ => new List<OptionConfig>() { OptionConfig.RANDOMINSTANT, OptionConfig.RANDOMPASSIVE }
    };

    public static IReadOnlyDictionary<Vector2, Type[]> GetInitEneimies() => levelNum switch
    {
        0 => new Dictionary<Vector2, Type[]>(),
        1 => new() { [new Vector2(15.0f, 0.0f)] = new[] { typeof(HaltTimer) } },
        4 => new() { [new Vector2(15.0f, 0.0f)] = new[] { typeof(HaltTimer) } },
        7 => new() { [new Vector2(0.0f, 15.0f)] = new[] { typeof(HaltTimer), typeof(Waterblight), typeof(EnemySpawn.Patrol), typeof(BlockEnemySpawn) } },
        _ => new()
    };

    public static float GetVehicleSpeed() => levelNum switch
    {
        0 => UnityEngine.Random.Range(80.0f, 100.0f),
        1 => UnityEngine.Random.Range(80.0f, 100.0f),
        2 => UnityEngine.Random.Range(50.0f, 70.0f),
        _ => UnityEngine.Random.Range(50.0f, 60.0f)
    };

    public static float GetVehicleDamage() => levelNum switch
    {
        0 => 75.0f,
        1 => 75.0f,
        2 => 75.0f,
        _ => 50.0f
    };

    public static float GetElectricFieldInterval() => levelNum switch
    {
        0 => UnityEngine.Random.Range(2.0f, 2.0f),
        1 => UnityEngine.Random.Range(2.0f, 2.0f),
        _ => UnityEngine.Random.Range(3.0f, 2.0f)
    };

    public static float GetElectricFieldDuration() => levelNum switch
    {
        0 => 3.5f,
        1 => 3.5f,
        2 => 3.5f,
        3 => 3.5f,
        4 => 3.5f,
        5 => 3.5f,
        6 => 3.5f,
        7 => 3.5f,
        8 => 3.5f,
        _ => 2.0f
    };

    public static void Reset()
    {
        levelNum = 0;
    }

    public static void SetLevelNum(int level)
    {
        levelNum = level;
    }

    public static int GetLevelNum()
    {
        return levelNum;
    }

    public class OptionConfig
    {
        public static readonly OptionConfig SUPERCHARGE = new(() => ActiveItem_0.GetShopOption());
        public static readonly OptionConfig SURFMANIA = new(() => ActiveItem_1.GetShopOption());
        public static readonly OptionConfig GRAVITYGRASP = new(() => ActiveItem_2_0.GetShopOption());
        public static readonly OptionConfig RANDOMPASSIVE = new(() =>
        {
            GameObject character = GameplayManager.getCharacter();
            HashSet<Func<GameObject>> generators = new();
            if (character.GetComponent<PassiveItem_0>() == null)
                generators.Add(PassiveItem_0.GetShopOption);
            if (character.GetComponent<PassiveItem_1>() == null)
                generators.Add(PassiveItem_1.GetShopOption);
            if (character.GetComponent<PassiveItem_2>() == null)
                generators.Add(PassiveItem_2.GetShopOption);
            if (character.GetComponent<PassiveItem_3>() == null)
                generators.Add(PassiveItem_3.GetShopOption);
            if (character.GetComponent<PassiveItem_Weapon_0>() == null)
                generators.Add(PassiveItem_Weapon_0.GetShopOption);
            if (character.GetComponent<PassiveItem_Weapon_2>() == null)
                generators.Add(PassiveItem_Weapon_2.GetShopOption);
            if (character.GetComponent<PassiveItem_Weapon_3>() == null)
                generators.Add(PassiveItem_Weapon_3.GetShopOption);

            if (generators.Count == 0)
                return RANDOMINSTANT.Instantiate();
            else
                return RandomChoice(generators)();
        });

        public static readonly OptionConfig RANDOMINSTANT = new(() => UnityEngine.Random.Range(0, 2) switch
        {
            0 => ShopOption.HPRecovery(UnityEngine.Random.Range(15, 36)),
            1 => ShopOption.SpeedBoost(),
            _ => throw new Exception()
        });

        private readonly Func<GameObject> generator;

        private OptionConfig(Func<GameObject> generator) => this.generator = generator;

        public GameObject Instantiate() => generator();
    }
}
