using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
public class SendToGoogle : MonoBehaviour
{

    
    private string URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSf2kGtw3hJ3SFHG_DOrPxSWfZJdaQ1ZrBrBjZY0Yp2CHEV1zA/formResponse";

    private string URLDamage = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSdLztQlbfYbbxGSMqvbKeBdBDc8OHCthihR7YNa_XP8arGY7A/formResponse";
    private long sessionID;

    private long sessionID_Dam;

    private int levelCount;
    private int selectionAttri;
	private int selectionSkill;



    private void Start()
    {
        // Assign sessionID to identify playtests
        sessionID = DateTime.Now.Ticks;
    }

   
    public void Send(int levelCount, int APitem, int selectionAttri ,int selectionSkill)
    {
        StartCoroutine(Post(sessionID.ToString(), levelCount.ToString(), selectionAttri.ToString(), selectionSkill.ToString(),APitem.ToString()));
    }
    
	 public int GetLevelCount()
    {
        return levelCount;
    }



     private IEnumerator Post(string sessionID, string levelCount, string selectionAttri, string selectionSkill, string APitem)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.1477553278", sessionID);
        form.AddField("entry.1570433801", levelCount);
        form.AddField("entry.1212750405", selectionAttri);
        form.AddField("entry.1882634750", selectionSkill);
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

    public void SendDamageSource(string sourceName)
    {
        StartCoroutine(PostDamageSource(sessionID.ToString(), sourceName));
    }

    
    private IEnumerator PostDamageSource(string sessionID_Dam, string sourceName)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.366340186", sessionID_Dam);
        form.AddField("entry.582082116", sourceName); 
        
        using (UnityWebRequest www = UnityWebRequest.Post(URLDamage, form))
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
