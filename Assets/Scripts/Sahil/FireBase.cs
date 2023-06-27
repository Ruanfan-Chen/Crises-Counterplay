using System.Collections;using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Collections;
using Proyecto26;

[System.Serializable]
public class PlayerData
{
    public string name;
    public int score;
}

public class FireBase : MonoBehaviour
{
    private IEnumerator Start()
    {
        Debug.Log("OVER HERE");

        // Create a PlayerData object with sample data
        PlayerData playerData = new PlayerData();
        playerData.name = "John";
        playerData.score = 100;

        // Convert the PlayerData object to JSON string
        string json = JsonUtility.ToJson(playerData);

        // Send the data to Firebase Realtime Database using the RestClient
        yield return RestClient.Post("https://trinity-1eba4-default-rtdb.firebaseio.com/.json", json);
    }
}
