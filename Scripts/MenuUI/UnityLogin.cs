using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System.IO;
// using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class JsonData
{
    public string username;
    public string password;
    public string serverIP;
}


public class UnityLogin : MonoBehaviour
{
    // public string username = "a";
    // public string password = "a";

    public TMP_InputField txtServerIP;
    public TMP_InputField txtUsername;
    public TMP_InputField txtPassword;

    public string serverUrl;
    public string username;
    public string password;
    // public string serverUrl = "http://localhost:3001/UnityLogin"; 
    // Replace with your actual server address and port

    public GameObject canvaLoginMenu;
    public GameObject canvaMainMenu;
    private void Start()
    {
        string json = File.ReadAllText(Application.dataPath + "/userconfig.json");
        JsonData jsonData = JsonUtility.FromJson<JsonData>(json);
        txtServerIP.text = jsonData.serverIP;
        txtUsername.text = jsonData.username;
        txtPassword.text = jsonData.password;
    }

    [System.Obsolete]
    private IEnumerator LoginCoroutine()
    {
        serverUrl = "http://" + txtServerIP.text + ":3001/UnityLogin";
        username = txtUsername.text;
        password = txtPassword.text;

        WWWForm form = new WWWForm();
        form.AddField("unityUsername", username);

        // Hash password using a secure hashing algorithm before sending (replace with your preferred hashing method)
        // string hashedPassword = password;
        form.AddField("unityPassword", password);

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
                    // ... handle user data
                    // string jsonResponse = "{ \n\"username\": \"" + username + "\", \n\"password\": \"" + hashedPassword + "\", \n\"serverIP\": \"" + txtServerIP.text + "\"\n}";

                    JsonData jsonData = new JsonData();
                    jsonData.username = username;
                    jsonData.password = password;
                    jsonData.serverIP = txtServerIP.text;

                    string jsonResponse = JsonUtility.ToJson(jsonData);
                    File.WriteAllText(Application.dataPath + "/userconfig.json", jsonResponse);
                    // Load Game
                    // SceneManager.LoadScene("Game");
                    canvaLoginMenu.SetActive(false);
                    canvaMainMenu.SetActive(true);
                }
            }
        }
    }

    [System.Obsolete]
    public void Login()
    {
        StartCoroutine(LoginCoroutine());
    }

    public void Register()
    {
        Application.OpenURL("http://" + txtServerIP.text + ":3000/Register");
    }

    // Replace this placeholder with your preferred secure password hashing method
    // private string HashPassword(string password)
    // {
    //     // Implement secure password hashing using a reputable library
    //     // (e.g., BCrypt.Net-Next for .NET Standard libraries)
    //     throw new System.NotImplementedException("Password hashing is not implemented. Please replace with a secure hashing method.");
    // }
}