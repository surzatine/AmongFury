using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText, usernameText, killsText, deathsText;
    public string username;


    public void Initialize(string nameID, string txtUsername)
    {
        nameText.text = nameID;
        usernameText.text = txtUsername;
        killsText.text = "0";
        deathsText.text = "0";
    }

    public void SetUsername(string txtUsername)
    {
        usernameText.text = txtUsername;
    }

    public void SetKills(int kills)
    {
        killsText.text = kills.ToString();
    }

    public void SetDeaths(int deaths)
    {
        deathsText.text = deaths.ToString();
    }

}
