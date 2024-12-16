using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

using System.IO;
public class JsonDataStats
{
    public string serverIP;
    public string username;
    public string create_date;
    public string transaction;
    public string kill;
    public string death;
}

public class Stats : MonoBehaviour
{

    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtAccountCreated;
    public TextMeshProUGUI txtTransactions;
    public TextMeshProUGUI txtLobbyCreated;
    public TextMeshProUGUI txtGamePlayed;
    // public TextMeshProUGUI txtDeath;

    public string username;
    public string password;
    public string serverIP;
    public string serverUrl;

    public double kd;
    // Start is called before the first frame update
    void Start()
    {
        string json = File.ReadAllText(Application.dataPath + "/userconfig.json");
        JsonData jsonData = JsonUtility.FromJson<JsonData>(json);
        serverIP = jsonData.serverIP;
        username = jsonData.username;
        password = jsonData.password;

        StartCoroutine(UserCoroutine());
        string jsonStats = File.ReadAllText(Application.dataPath + "/userstats.json");
        JsonDataStats jsonDataStats = JsonUtility.FromJson<JsonDataStats>(jsonStats);

        Debug.Log("[%] " + jsonStats);

        txtName.text = jsonDataStats.username;
        txtAccountCreated.text = jsonDataStats.create_date;
        txtTransactions.text = jsonDataStats.transaction;


        StartCoroutine(GamePlayedCoroutine());
        StartCoroutine(LobbyCreatedCoroutine());
        // txtKD.text = jsonDataStats.kd;
        // txtKill.text = jsonDataStats.kill.ToString();
        // txtDeath.text = jsonDataStats.death.ToString();

        // kd = double.Parse(jsonDataStats.kill) / double.Parse(jsonDataStats.death);
        // txtKD.text = kd.ToString();

    }




    [System.Obsolete]
    private IEnumerator UserCoroutine()
    {
        serverUrl = "http://" + serverIP + ":3001/api/Login";

        WWWForm form = new WWWForm();
        form.AddField("username", username);

        // Hash password using a secure hashing algorithm before sending (replace with your preferred hashing method)
        // string hashedPassword = password;
        form.AddField("password", password);

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
                if (response.Equals("\"Invalid username or password\""))
                {
                    Debug.LogError("Login failed: Invalid credentials");
                    // Handle invalid credentials (e.g., display an error message)
                }
                else
                {
                    // Successful login! Parse and handle user data from response
                    Debug.Log("Login successful!");
                    Debug.Log(response);

                    File.WriteAllText(Application.dataPath + "/userstats.json", response);
                    // ... handle user data
                    // string jsonResponse = "{ \n\"username\": \"" + username + "\", \n\"password\": \"" + hashedPassword + "\", \n\"serverIP\": \"" + txtServerIP.text + "\"\n}";

                    // JsonData jsonData = new JsonData();
                    // jsonData.username = username;
                    // jsonData.password = password;
                    // jsonData.serverIP = txtServerIP.text;

                    // string jsonResponse = JsonUtility.ToJson(jsonData);
                    // File.WriteAllText(Application.dataPath + "/userconfig.json", jsonResponse);

                }
            }
        }
    }



    [System.Obsolete]
    private IEnumerator GamePlayedCoroutine()
    {
        serverUrl = "http://" + serverIP + ":3001/api/totalGamePlayed/" + username;



        using (UnityWebRequest request = UnityWebRequest.Get(serverUrl))
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
                if (response.Equals("\"Invalid username or password\""))
                {
                    Debug.LogError("Login failed: Invalid credentials");
                    // Handle invalid credentials (e.g., display an error message)
                }
                else
                {
                    // Successful login! Parse and handle user data from response
                    Debug.Log("Login successful!");
                    Debug.Log(response);

                    txtGamePlayed.text = response;

                    // ... handle user data
                    // string jsonResponse = "{ \n\"username\": \"" + username + "\", \n\"password\": \"" + hashedPassword + "\", \n\"serverIP\": \"" + txtServerIP.text + "\"\n}";

                    // JsonData jsonData = new JsonData();
                    // jsonData.username = username;
                    // jsonData.password = password;
                    // jsonData.serverIP = txtServerIP.text;

                    // string jsonResponse = JsonUtility.ToJson(jsonData);
                    // File.WriteAllText(Application.dataPath + "/userconfig.json", jsonResponse);

                }
            }
        }
    }


    [System.Obsolete]
    private IEnumerator LobbyCreatedCoroutine()
    {
        serverUrl = "http://" + serverIP + ":3001/api/totalLobbyCreated/" + username;



        using (UnityWebRequest request = UnityWebRequest.Get(serverUrl))
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
                if (response.Equals("\"Invalid username or password\""))
                {
                    Debug.LogError("Login failed: Invalid credentials");
                    // Handle invalid credentials (e.g., display an error message)
                }
                else
                {
                    // Successful login! Parse and handle user data from response
                    Debug.Log("Login successful!");
                    Debug.Log(response);

                    txtLobbyCreated.text = response;

                    // ... handle user data
                    // string jsonResponse = "{ \n\"username\": \"" + username + "\", \n\"password\": \"" + hashedPassword + "\", \n\"serverIP\": \"" + txtServerIP.text + "\"\n}";

                    // JsonData jsonData = new JsonData();
                    // jsonData.username = username;
                    // jsonData.password = password;
                    // jsonData.serverIP = txtServerIP.text;

                    // string jsonResponse = JsonUtility.ToJson(jsonData);
                    // File.WriteAllText(Application.dataPath + "/userconfig.json", jsonResponse);

                }
            }
        }
    }

}
