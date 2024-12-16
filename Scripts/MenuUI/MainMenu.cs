using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JsonDataMainMenu
{
    public string username;
}
public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI txtName;
    public string username;

    private void Start()
    {
        string json = File.ReadAllText(Application.dataPath + "/userconfig.json");
        JsonData jsonData = JsonUtility.FromJson<JsonData>(json);
        Debug.Log("[*] Welcome, " + jsonData.username);
        txtName.text = "[*] Welcome, " + jsonData.username;
    }
    public void BtnMultiplayer()
    {
        // Load Game
        SceneManager.LoadScene("Game");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
