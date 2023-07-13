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
    private static Sprite watermark;
    private static bool spawnVehicle;
    private static bool spawnElectricityField;
    private static bool spawnEnemy;
    private static List<ShopOption> shopOptions;

    public static string GetLevelName() => levelName;

    public static float GetTimeLimit() => timeLimit;

    public static Vector2 GetMapSize() => mapSize;

    public static Sprite GetTile() => tile;

    public static Sprite GetWatermark() => watermark;

    public static bool GetSpawnVehicle() => spawnVehicle;

    public static bool GetSpawnElectricityField() => spawnElectricityField;

    public static bool GetSpawnEnemy() => spawnEnemy;

    public static IReadOnlyList<ShopOption> GetShopOptions() => shopOptions;

    public static void Reset()
    {
        levelName = "Level 1";
        timeLimit = 30.0f;
        mapSize = new Vector2(50.0f, 50.0f);
        Texture2D tileTexture = Resources.Load<Texture2D>(tileTexturePath);
        tile = Sprite.Create(tileTexture, new Rect(0, 0, tileTexture.width, tileTexture.height), Vector2.one * 0.5f, 10.0f);
        watermark = Resources.Load<Sprite>(wasdSpritePath);
        switch (UnityEngine.Random.Range(0, 3))
        {
            case 0:
                spawnVehicle = true;
                spawnElectricityField = false;
                spawnEnemy = false;

                break;
            case 1:
                spawnVehicle = false;
                spawnElectricityField = true;
                spawnEnemy = false;
                break;
            case 2:
                spawnVehicle = false;
                spawnElectricityField = false;
                spawnEnemy = true;
                break;
        }

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
            logo = Resources.Load<Sprite>("");
            name = "+" + hpRecovery.ToString() + " HP";
            action = delegate {
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
