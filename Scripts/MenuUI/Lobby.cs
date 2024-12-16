using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class Lobby : MonoBehaviour
{

    public string room_id;
    public string datetime;
    public string game_mode;
    public string host_username;
    // Start is called before the first frame update
    // [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject lobbyPanel;
    // [SerializeField] private GameObject startPanel;
    // [SerializeField] private Button create, join;
    [SerializeField] private TMP_InputField lobbyInput;
    [SerializeField] private TextMeshProUGUI lobbySuccess, lobbyIDText;
    public string serverIP, username;

    public GameObject mapTaumadhi, mapPascal, mapKhwopa, mapRoom;
    public string map;

    void Awake()
    {
        string json = File.ReadAllText(Application.dataPath + "/userconfig.json");
        JsonData jsonData = JsonUtility.FromJson<JsonData>(json);
        serverIP = jsonData.serverIP;
        username = jsonData.username;

        // canvas.SetActive(true);
        lobbyPanel.SetActive(true);

        // startPanel.SetActive(false);
    }

    void Start()
    {

    }



    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // public void MainActive()
    // {

    //     mainPanel.SetActive(true);
    //     // startPanel.SetActive(false);
    // }



    public void CloseCanvas()
    {
        // canvas.SetActive(false);
        lobbyPanel.SetActive(false);
        // startPanel.SetActive(false);
    }

    // Button Create
    public void CreateLobby()
    {
        lobbySuccess.text = "Lobby is Created";


        if (mapTaumadhi.activeSelf)
        {
            map = "Taumadhi";
        }
        if (mapPascal.activeSelf)
        {
            map = "Pascal";
        }
        if (mapKhwopa.activeSelf)
        {
            map = "Khwopa";
        }
        if (mapRoom.activeSelf)
        {
            map = "Room";
        }

        Debug.Log("[&] Map: " + map);

        string generate_lobby_id = UnityEngine.Random.Range(10000000, 99999999).ToString();
        lobbyInput.text = generate_lobby_id;
        // lobbyIDText.text = "Lobby ID: " + generate_lobby_id.ToString();
        // mainPanel.SetActive(false);
        // startPanel.SetActive(true);

        String host_username = username;
        string game_mode = "Full DeathMatch";
        String game_map = map;


        StartCoroutine(CreateLobbyCoroutine(generate_lobby_id, game_mode, game_map, host_username));
    }

    // Button Join
    public void JoinLobby()
    {
        string lobby_id = lobbyInput.text;
        string player_user_id = "123";
        string player_username = username;
        /**/
        // lobbyIDText.text = "Lobby ID: " + lobby_id.ToString();

        StartCoroutine(JoinLobbyCoroutine(lobby_id, player_user_id, player_username));

        // lobbyPanel.SetActive(false);
        // startPanel.SetActive(true);

        Debug.Log("[*] JoinLobby");

        // PlayerSpawnObject playerSpawnObject = new PlayerSpawnObject();
        // playerSpawnObject.MainSpawnObject();

    }





    // [ServerRpc]
    // public void HideCanvasServer(GameObject canvas, GameObject mainPanel, GameObject startPanel)
    // {
    //     Debug.Log("[*] ServerRpc");
    //     HideCanvasObserver(canvas, mainPanel, startPanel);
    // }

    // [ObserversRpc]
    // public void HideCanvasObserver(GameObject canvas, GameObject mainPanel, GameObject startPanel)
    // {
    //     Debug.Log("[*] ObserversRpc");
    //     canvas.SetActive(false);
    //     mainPanel.SetActive(false);
    //     startPanel.SetActive(false);
    // }

    IEnumerator CreateLobbyCoroutine(string room_id, string game_mode, string game_map, string host_username)
    {

        // string json = File.ReadAllText(Application.dataPath + "/userconfig.json");
        // JsonServerUrl jsonServerUrl = JsonUtility.FromJson<JsonServerUrl>(json);
        // serverUrl = jsonServerUrl.serverIP;
        // 2000-12-31T18:15:00.000+00:00
        // 2024-07-16T09:48:49.000+00:00
        DateTime gmtDateTime = DateTime.Now;
        TimeSpan ts = new TimeSpan(05, 45, 00);

        DateTime currentDateTime = gmtDateTime.Add(ts);
        string datetime = gmtDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz");

        using (UnityWebRequest request = UnityWebRequest.Post("http://" + serverIP + ":3001/UnityCreateLobby", "{ \"room_id\": \"" + room_id + "\", \"datetime\": \"" + datetime + "\" , \"game_mode\": \"" + game_mode + "\",\"game_map\": \"" + game_map + "\", \"host_username\": \"" + host_username + "\" }", "application/json"))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }


    IEnumerator JoinLobbyCoroutine(string room_id, string player_user_id, string player_username)
    {

        using (UnityWebRequest request = UnityWebRequest.Post("http://" + serverIP + ":3001/UnityJoinLobby", "{ \"room_id\": \"" + room_id + "\", \"player_user_id\": \"" + player_user_id + "\" ,  \"player_username\": \"" + player_username + "\" }", "application/json"))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
            }
            else
            {
                string response = request.downloadHandler.text;
                Debug.Log(response);
                if (response != null)
                {
                    response = response.Replace("\"", "");
                    string[] splitString = response.Split(' ');

                    // Accessing the split parts
                    string firstPart = splitString[0]; // Output: "1"
                    string host_username = splitString[1]; // Output: "sushan"
                    int response_match_count = int.Parse(firstPart);
                    if (response_match_count > 0)
                    {
                        // Load Lobby Panel
                        // lobbyPanel.SetActive(false);
                        // startPanel.SetActive(true);
                    }

                    // Read file from JSON config file
                    if (host_username == "sushan")
                    {
                        // buttonToHide.SetActive(true);
                    }
                    else
                    {
                        // buttonToHide.SetActive(false);
                    }
                }
            }
        }
    }

}

