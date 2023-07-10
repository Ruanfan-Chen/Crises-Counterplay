using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
public class SendToGoogle : MonoBehaviour
{

    private string URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSf2kGtw3hJ3SFHG_DOrPxSWfZJdaQ1ZrBrBjZY0Yp2CHEV1zA/formResponse";
    private long sessionID;

    private void Start()
    {
        // Assign sessionID to identify playtests
        sessionID = DateTime.Now.Ticks;
    }

    public void Send(int levelCount, int APitem, int playerSelection)
    {
        StartCoroutine(Post(sessionID.ToString(), levelCount.ToString(), playerSelection.ToString(), APitem.ToString()));
    }
    
	 public int GetLevelCount()
    {
        return levelCount;
    }



    private IEnumerator Post(string sessionID, string levelCount, string playerSelection, string APitem)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.1477553278", sessionID);
        form.AddField("entry.1570433801", levelCount);
        form.AddField("entry.1212750405", playerSelection);
        form.AddField("entry.360297603", APitem);

        // Send responses and verify result
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }
}