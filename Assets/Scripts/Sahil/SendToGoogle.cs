using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
public class SendToGoogle : MonoBehaviour
{

    
    private string URL_MX1 = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSf2kGtw3hJ3SFHG_DOrPxSWfZJdaQ1ZrBrBjZY0Yp2CHEV1zA/formResponse";

    private string URL_MX2 = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSdLztQlbfYbbxGSMqvbKeBdBDc8OHCthihR7YNa_XP8arGY7A/formResponse";

    private string URL_MX3 ="https://docs.google.com/forms/u/0/d/e/1FAIpQLSc1mBiFRJaKCCfws0KNUB4DVJ16JldVVshjg3S_41j1xbB41w/formResponse";

    private string URL_MX4 ="https://docs.google.com/forms/u/0/d/e/1FAIpQLSeEzHgxjpqBoyEot6PUlrw3up1ISi4DLLavK2LIZPQSIri-IA/formResponse";


    private long sessionID_MX1;
    private long sessionID_MX2;
    private long sessionID_MX3;
    private long sessionID_MX4;

    private int levelCount;
    private int DamageCount;
	


    private void Start() 
    {
        // Assign sessionID to identify playtests
        sessionID_MX1= DateTime.Now.Ticks;
        sessionID_MX2= DateTime.Now.Ticks;
        sessionID_MX3= DateTime.Now.Ticks;
        sessionID_MX4= DateTime.Now.Ticks;
    }
//Matrix 1
   
    public void Send(int levelCount, int DamageCount, string DamageName)
    {
        StartCoroutine(Post(sessionID_MX1.ToString(), levelCount.ToString(), DamageCount.ToString(), DamageName.ToString()));
    }
    

     private IEnumerator Post(string sessionID_MX1, string levelCount, string DamageCount, string DamageName)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.1477553278", sessionID_MX1);
        form.AddField("entry.1570433801", levelCount);
        form.AddField("entry.1212750405", DamageCount);
        form.AddField("entry.916383983", DamageName);
       
        using (UnityWebRequest www = UnityWebRequest.Post(URL_MX1, form))
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


//Matrix 2
    public void SendMatrix2(string TrainSpeed, string TrainHit)
    {
        StartCoroutine(PostMatrix2(sessionID_MX2.ToString(), TrainSpeed.ToString(), TrainHit.ToString()));
    }

    
    private IEnumerator PostMatrix2(string sessionID_MX2, string TrainSpeed, string TrainHit)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.366340186", sessionID_MX2);
        form.AddField("entry.582082116", TrainSpeed); 
        form.AddField("entry.401126771", TrainHit); 
        
        using (UnityWebRequest www = UnityWebRequest.Post(URL_MX2, form))
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
	

//Matrix 3
	public void SendMatrix3(string LevelName, string TrainDash, string SuperCH_count)
    {
        StartCoroutine(PostMatrix3(sessionID_MX3.ToString(), LevelName.ToString(), TrainDash.ToString(), SuperCH_count.ToString()));
    }

    
    private IEnumerator PostMatrix3(string sessionID_MX3, string LevelName ,string TrainDash, string SuperCH_count)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.366340186", sessionID_MX3);
        form.AddField("entry.1848486491", LevelName); 
        form.AddField("entry.465445524",  TrainDash); 
        form.AddField("entry.1833994225",  SuperCH_count); 
        
        using (UnityWebRequest www = UnityWebRequest.Post(URL_MX3, form))
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



//Matrix 4
	public void SendMatrix4(string ChoiceName, string HPCount)
    {
        StartCoroutine(PostMatrix4(sessionID_MX4.ToString(), ChoiceName.ToString(), HPCount.ToString()));
    }
	private IEnumerator PostMatrix4(string sessionID_MX4, string ChoiceName,string HPCount)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.366340186", sessionID_MX4);
        form.AddField("entry.1210868527", ChoiceName); 
        form.AddField("entry.1840849645", HPCount); 
        
        using (UnityWebRequest www = UnityWebRequest.Post(URL_MX4, form))
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
