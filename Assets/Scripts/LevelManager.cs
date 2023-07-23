using System;
using System.Collections.Generic;
using UnityEngine;

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

    public static float GetTimeLimit() => 30.0f;

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

    public static IReadOnlyList<ShopOption> GetShopOptions()
    {
        List<ShopOption> options = levelNum switch
        {
            0 => new() { ShopOption.CHISTRIKE },
            1 => new(),
            2 => new() { new ShopOption(UnityEngine.Random.Range(15, 36)), ShopOption.TOXICFOOTPRINT },
            3 => new() { ShopOption.SUPERCHARGE },
            4 => new(),
            5 => new() { new ShopOption(UnityEngine.Random.Range(15, 36)), ShopOption.TOXICFOOTPRINT },
            6 => new() { ShopOption.EBBTIDE },
            7 => new(),
            8 => new() { new ShopOption(UnityEngine.Random.Range(15, 36)), ShopOption.TOXICFOOTPRINT },
            9 => new() { new ShopOption(UnityEngine.Random.Range(15, 36)), ShopOption.TOXICFOOTPRINT },
            10 => new() { new ShopOption(UnityEngine.Random.Range(15, 36)), ShopOption.TOXICFOOTPRINT },
            11 => new() { new ShopOption(UnityEngine.Random.Range(15, 36)), ShopOption.TOXICFOOTPRINT },
            _ => new() { new ShopOption(UnityEngine.Random.Range(15, 36)), ShopOption.TOXICFOOTPRINT }
        };
        if (GameplayManager.getCharacter().GetComponent<PassiveItem_0>())
            options.Remove(ShopOption.TOXICFOOTPRINT);
        if (GameplayManager.getCharacter().GetComponent<ActiveItem_0>())
            options.Remove(ShopOption.SUPERCHARGE);
        if (GameplayManager.getCharacter().GetComponent<ActiveItem_1>())
            options.Remove(ShopOption.EBBTIDE);
        if (GameplayManager.getCharacter().GetComponent<ActiveItem_2>())
            options.Remove(ShopOption.TRAINBOUND);
        if (GameplayManager.getCharacter().GetComponent<ActiveItem_2_0>())
            options.Remove(ShopOption.CHISTRIKE);
        //Debug.Log("GetShopOptions curr level = " + levelNum);
        return options;
    }

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


    public class ShopOption
    {
        public static readonly ShopOption TRAINBOUND = new ShopOption(ActiveItem_2.GetDescription(), ActiveItem_2.GetLogo(), ActiveItem_2.GetName(), delegate
        {
            GameplayManager.getCharacter().GetComponent<Character>().GiveItem<ActiveItem_2>(KeyCode.K);
        }, ActiveItem_2.GetTutorial());
        public static readonly ShopOption CHISTRIKE = new ShopOption(ActiveItem_2_0.GetDescription(), ActiveItem_2_0.GetLogo(), ActiveItem_2_0.GetName(), delegate
        {
            GameplayManager.getCharacter().GetComponent<Character>().GiveItem<ActiveItem_2_0>(KeyCode.K);
        }, ActiveItem_2_0.GetTutorial());
        public static readonly ShopOption TOXICFOOTPRINT = new ShopOption(PassiveItem_0.GetDescription(), PassiveItem_0.GetLogo(), PassiveItem_0.GetName(), delegate
        {
            GameplayManager.getCharacter().GetComponent<Character>().GiveItem<PassiveItem_0>();
        }, null);
        public static readonly ShopOption SUPERCHARGE = new ShopOption(ActiveItem_0.GetDescription(), ActiveItem_0.GetLogo(), ActiveItem_0.GetName(), delegate
        {
            GameplayManager.getCharacter().GetComponent<Character>().GiveItem<ActiveItem_0>(KeyCode.J);
        }, ActiveItem_0.GetTutorial());
        public static readonly ShopOption EBBTIDE = new ShopOption(ActiveItem_1.GetDescription(), ActiveItem_1.GetLogo(), ActiveItem_1.GetName(), delegate
        {
            GameplayManager.getCharacter().GetComponent<Character>().GiveItem<ActiveItem_1>(KeyCode.L);
        }, ActiveItem_1.GetTutorial());
        private readonly string description;
        private readonly Sprite logo;
        private readonly string name;
        private readonly Action action;
        private readonly Sprite tutorial;

        public ShopOption(string description, Sprite logo, string name, Action action, Sprite tutorial)
        {
            this.description = description;
            this.logo = logo;
            this.name = name;
            this.action = action;
            this.tutorial = tutorial;
        }

        public ShopOption(int hpRecovery)
        {
            description = "+" + hpRecovery.ToString() + " HP";
            logo = Resources.Load<Sprite>("Sprites/HPRecovery");
            name = "+" + hpRecovery.ToString() + " HP";
            action = delegate
            {
                Character script = GameplayManager.getCharacter().GetComponent<Character>();
                script.SetHealth(Mathf.Clamp(script.GetHealth() + hpRecovery, 0.0f, script.GetMaxHealth()));
            };
            tutorial = null;
        }

        public string GetDescription()
        {
            return description;
        }

        public Sprite GetLogo()
        {
            return logo;
        }

        public string GetName()
        {
            return name;
        }

        public Action GetAction()
        {
            return action;
        }

        public Sprite GetTutorial()
        {
            return tutorial;
        }
    }

    public static void SetLevelNum(int level)
    {
        levelNum = level;
    }

    public static int GetLevelNum()
    {
        return levelNum;
    }
}
