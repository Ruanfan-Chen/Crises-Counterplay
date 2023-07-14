using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager
{
    private static string tileTexturePath = "Sprites/TilesetFloor";
    private static string wasdSpritePath = "Sprites/wasd";
    private static string levelName;
    private static float timeLimit;
    private static Vector2 mapSize;
    private static Sprite tile;
    private static List<Sprite> watermarks = new();
    private static bool spawnVehicle;
    private static bool spawnElectricityField;
    private static bool spawnEnemy;
    private static List<ShopOption> shopOptions = new();

    public static string GetLevelName() => levelName;

    public static float GetTimeLimit() => timeLimit;

    public static Vector2 GetMapSize() => mapSize;

    public static Sprite GetTile() => tile;

    public static IReadOnlyList<Sprite> GetWatermarks() => watermarks;

    public static bool GetSpawnVehicle() => spawnVehicle;

    public static bool GetSpawnElectricityField() => spawnElectricityField;

    public static bool GetSpawnEnemy() => spawnEnemy;

    public static IReadOnlyList<ShopOption> GetShopOptions() => shopOptions;

    public static void Reset()
    {
        levelName = "Level 1";
        timeLimit = 7.0f;
        mapSize = new Vector2(50.0f, 50.0f);
        Texture2D tileTexture = Resources.Load<Texture2D>(tileTexturePath);
        tile = Sprite.Create(tileTexture, new Rect(0, 0, tileTexture.width, tileTexture.height), Vector2.one * 0.5f, 10.0f);
        //watermarks.Add(Resources.Load<Sprite>(wasdSpritePath));
        shopOptions.Add(new ShopOption(PassiveItem_0.GetDescription(), PassiveItem_0.GetLogo(), PassiveItem_0.GetName(), () => { GameplayManager.getCharacter().GetComponent<Character>().GiveItem<PassiveItem_0>(); }));
        shopOptions.Add(new ShopOption(ActiveItem_2_0.GetDescription(), ActiveItem_2_0.GetLogo(), ActiveItem_2_0.GetName(), () => { GameplayManager.getCharacter().GetComponent<Character>().GiveItem<ActiveItem_2_0>(KeyCode.K); }));
        shopOptions.Add(new ShopOption(ActiveItem_2.GetDescription(), ActiveItem_2.GetLogo(), ActiveItem_2.GetName(), () => { GameplayManager.getCharacter().GetComponent<Character>().GiveItem<ActiveItem_2>(KeyCode.L); }));
        spawnVehicle = true;
    }

    public static void MoveNext()
    {
        Debug.Log("NotImplemented");
    }
    public class ShopOption
    {
        private readonly string description;
        private readonly Sprite logo;
        private readonly string name;
        private readonly Action action;

        public ShopOption(string description, Sprite logo, string name, Action action)
        {
            this.description = description;
            this.logo = logo;
            this.name = name;
            this.action = action;
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
    }
}
