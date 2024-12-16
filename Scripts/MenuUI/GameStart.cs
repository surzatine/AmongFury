using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class GameStart : MonoBehaviour
{
    // public GameObject btnJoin;
    public string serverIP;
    public string serverUrl;
    public TMP_InputField tMP_InputLobbyID;
    public TextMeshProUGUI lobbySuccess;

    public GameObject canvasToHide;

    void Awake()
    {
        string json = File.ReadAllText(Application.dataPath + "/userconfig.json");
        JsonData jsonData = JsonUtility.FromJson<JsonData>(json);
        serverIP = jsonData.serverIP;
    }


    public void funcCanvasToHide()
    {

        // canvasToHide.SetActive(false);
        StartCoroutine(RequestToStartGame());
    }


    [System.Obsolete]
    private IEnumerator RequestToStartGame()
    {
        serverUrl = "http://" + serverIP + ":3001/getLastRoomId/" + tMP_InputLobbyID.text;



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
                if (response.Equals("\"Incorrect ID\""))
                {
                    Debug.LogError("Incorrect ID");
                    lobbySuccess.text = "Incorrect ID";
                    // Handle invalid credentials (e.g., display an error message)
                }
                else
                {
                    // Successful login! Parse and handle user data from response
                    Debug.Log("Correct ID!");
                    canvasToHide.SetActive(false);

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
