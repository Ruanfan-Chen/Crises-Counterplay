using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class LevelManager
{
    private static string tileTexturePath = "Sprites/TilesetFloor";

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
        4 => 20.0f,
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
        6 => false,
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

    public static IReadOnlyList<OptionConfig> GetShopConfig() => levelNum switch
    {
            0 => new List<OptionConfig>() { OptionConfig.GRAVITYGRASP },
            1 => new List<OptionConfig>(),
            2 => new List<OptionConfig>() { OptionConfig.HPRECOVERY, OptionConfig.RANDOMPASSIVE },
            3 => new List<OptionConfig>() { OptionConfig.SUPERCHARGE },
            4 => new List<OptionConfig>(),
            5 => new List<OptionConfig>() { OptionConfig.HPRECOVERY, OptionConfig.RANDOMPASSIVE },
            6 => new List<OptionConfig>() { OptionConfig.SURFMANIA },
            7 => new List<OptionConfig>(),
            8 => new List<OptionConfig>() { OptionConfig.HPRECOVERY, OptionConfig.RANDOMPASSIVE },
            9 => new List<OptionConfig>() { OptionConfig.HPRECOVERY, OptionConfig.RANDOMPASSIVE },
            10 => new List<OptionConfig>() { OptionConfig.HPRECOVERY, OptionConfig.RANDOMPASSIVE },
            11 => new List<OptionConfig>() { OptionConfig.HPRECOVERY, OptionConfig.RANDOMPASSIVE },
            _ => new List<OptionConfig>() { OptionConfig.HPRECOVERY, OptionConfig.RANDOMPASSIVE }
    };

    public static IReadOnlyDictionary<Vector2, Type[]> GetInitEneimies() => levelNum switch
    {
        0 => new Dictionary<Vector2, Type[]>(),
        1 => new() { [new Vector2(15.0f, 0.0f)] = new[] { typeof(HaltTimer) } },
        2 => new(),
        3 => new(),
        4 => new() { [new Vector2(15.0f, 0.0f)] = new[] { typeof(HaltTimer) } },
        5 => new(),
        6 => new()
        {
            [new Vector2(0.0f, 15.0f)] = new[] { typeof(Waterblight), typeof(EnemySpawn.AimlesslyMove) },
            [new Vector2(0.0f, -15.0f)] = new[] { typeof(Waterblight), typeof(EnemySpawn.DirectlyMoveToward) },
            [new Vector2(15.0f, 0.0f)] = new[] { typeof(Waterblight), typeof(EnemySpawn.MoveInCircle) }
        },
        7 => new() { [new Vector2(0.0f, 15.0f)] = new[] { typeof(HaltTimer), typeof(Waterblight), typeof(EnemySpawn.Patrol), typeof(BlockEnemySpawn) } },
        8 => new(),
        9 => new(),
        10 => new(),
        11 => new(),
        _ => new()
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
        public static readonly OptionConfig HPRECOVERY = new OptionConfig(() => ShopOption.HPRecovery(UnityEngine.Random.Range(15, 36)));
        public static readonly OptionConfig SUPERCHARGE = new OptionConfig(() => ActiveItem_0.getShopOption());
        public static readonly OptionConfig SURFMANIA = new OptionConfig(() => ActiveItem_1.getShopOption());
        public static readonly OptionConfig GRAVITYGRASP = new OptionConfig(() => ActiveItem_2_0.getShopOption());
        public static readonly OptionConfig RANDOMPASSIVE = new OptionConfig(() => PassiveItem_0.getShopOption());
        public static readonly OptionConfig RANDOMWEAPON = new OptionConfig(() => ShopOption.HPRecovery(1000));

        private readonly Func<GameObject> generator;

        private OptionConfig(Func<GameObject> generator) => this.generator = generator;

        public GameObject Instantiate() => generator();
    }
}
