using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
// using FishNet.Managing.Scened;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;


public class MenuUI : MonoBehaviour
{
    // public string txtServerIP = "localhost";
    // public string txtUsername = "sushan";

    public TMP_InputField txtServerIP;
    public TMP_InputField txtUsername;
    public TMP_InputField txtPassword;

    public int loginFlag = 0;

    // Start is called before the first frame update
    // void Start()
    // {

    // }

    // Update is called once per frame
    public void ButtonPlay()
    {
        // string config = "{\n\"username\": \"" + txtUsername.text + "\",\n\"serverip\": \"" + txtServerIP.text + "\"\n}";

        // File.WriteAllText("userconfig.json", config);
        // Debug.Log("config");

        StartCoroutine(PostData_Coroutine());

        // Debug.Log("login")


        if (loginFlag == 1)
        {
            Debug.Log("[#] " + loginFlag);
            // SceneManager.LoadScene("Game");
        }
    }

    IEnumerator PostData_Coroutine()
    {
        if (txtServerIP.text != null)
        {
            // string uri = "http://" + txtServerIP.text + ":3001/UnityLogin";
            // string uri = "http://localhost:3001/UnityLogin";
            string uri = "https://my-json-server.typicode.com/typicode/demo/posts";
            WWWForm form = new WWWForm();
            // form.AddField("unityUsername", txtUsername.text);
            // form.AddField("unityPassword", txtPassword.text);
            // form.AddField("unityUsername", "a");
            // form.AddField("unityPassword", "a");
            // Debug.Log("[#] " + txtUsername.text + " " + txtServerIP.text + "  " + txtPassword.text);
            form.AddField("title", txtUsername.text);


            using (UnityWebRequest request = UnityWebRequest.Post(uri, form))
            {
                // Debug.Log("[#] " + request.url);
                // request.SetRequestHeader("Content-Type", "application/json");
                // request.SetRequestHeader("Accept", "application/json");
                yield return request.SendWebRequest();

                if (request.isNetworkError || request.isHttpError)
                {
                    loginFlag = 0;
                    Debug.Log(request.error);
                }
                else if (request.downloadHandler.text == "\"Invalid username or password\"")
                {
                    loginFlag = 0;
                    Debug.Log("[!] " + loginFlag);
                }
                else
                {
                    loginFlag = 1;
                    Debug.Log("[*] " + loginFlag);

                    File.WriteAllText("userconfig.json", request.downloadHandler.text);
                    Debug.Log(request.downloadHandler.text);
                }
            }
        }
        else
        {
            Debug.Log("Server IP is null");
        }
    }



}
