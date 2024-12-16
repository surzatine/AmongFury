using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class DatabaseKillDeath : MonoBehaviour
{

    public string serverUrl;
    public string killer;
    public string victim;
    public string room;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
    [System.Obsolete]
    public IEnumerator KillDeathCoroutine1()
    {

        Debug.Log("[#] Api Calling");
        serverUrl = "http://" + serverUrl + ":3001/UnityKillDeath";

        DateTime date = new DateTime();

        WWWForm form = new WWWForm();
        form.AddField("killer", "killer");
        form.AddField("victim", "victim");
        form.AddField("room_id", "room");
        form.AddField("date", date.ToString());
        // form.AddField("date", "");


        using (UnityWebRequest request = UnityWebRequest.Post(serverUrl, form))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.LogError("Network error during login: " + request.error);
                // Handle network errors (e.g., display an error message to the user)
            }
            else if (request.isHttpError)
            {
                Debug.LogError("HTTP error during login: " + request.responseCode + " - " + request.downloadHandler.text);
                // Handle HTTP errors (e.g., invalid credentials)
            }
            else
            {
                string response = request.downloadHandler.text;
                Debug.Log("[+] " + response);
            }
        }
    }
    */

    IEnumerator KillDeathCoroutine(int killer, int victim)
    {

        string json = File.ReadAllText(Application.dataPath + "/userconfig.json");
        JsonServerUrl jsonServerUrl = JsonUtility.FromJson<JsonServerUrl>(json);
        serverUrl = jsonServerUrl.serverIP;
        // 2000-12-31T18:15:00.000+00:00
        // 2024-07-16T09:48:49.000+00:00
        DateTime gmtDateTime = DateTime.Now;
        TimeSpan ts = new TimeSpan(05, 45, 00);

        DateTime currentDateTime = gmtDateTime.Add(ts);
        string currentDateTimeString = currentDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz");

        using (UnityWebRequest www = UnityWebRequest.Post("http://" + serverUrl + ":3001/UnityKillDeath", "{ \"killer\": \"" + killer + "\", \"victim\": \"" + victim + "\" , \"date\": \"" + currentDateTime + "\", \"room_id\": \"3\" }", "application/json"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }


    // [System.Obsolete]
    public void DbKillDeath(int killer, int victim)
    {
        Debug.Log("[%] Api Calling");
        // StartCoroutine(KillDeathCoroutine());
        StartCoroutine(KillDeathCoroutine(killer, victim));
    }
}

class JsonServerUrl
{
    public string username;
    public string password;
    public string serverIP;
}