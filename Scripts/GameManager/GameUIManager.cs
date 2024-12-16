using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using FishNet.Object;



public class GameUIManager : NetworkBehaviour
{
    private static GameUIManager instance;
    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] private GameObject scoreboard;
    [SerializeField] private PlayerCard playerCardPrefab;
    [SerializeField] private Transform playerCardParent;

    private Dictionary<int, PlayerCard> _playerCards = new Dictionary<int, PlayerCard>();


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;

        scoreboard.SetActive(false);


    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            scoreboard.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            scoreboard.SetActive(false);
        }
    }

    public static void PlayerJoined(int clientID, string txtUsername)
    {
        PlayerCard newCard = Instantiate(instance.playerCardPrefab, instance.playerCardParent);
        instance._playerCards.Add(clientID, newCard);
        newCard.Initialize(clientID.ToString(), txtUsername);

        // Add username to Player Card file and SetUsername method
        SetUsername(clientID, txtUsername);
    }

    public static void PlayerLeft(int clientID)
    {
        if (instance._playerCards.TryGetValue(clientID, out PlayerCard playerCard))
        {
            Destroy(playerCard.gameObject);
            instance._playerCards.Remove(clientID);
        }
    }

    public static void SetHealthText(string healthText)
    {
        instance.healthText.text = healthText;
    }

    // Username
    public static void SetUsername(int clientID, string txtUsername)
    {
        instance.SetUsernameServer(clientID, txtUsername);
    }

    [Server]
    public void SetUsernameServer(int clientID, string txtUsername)
    {
        SetUsernameObserver(clientID, txtUsername);
    }

    [ObserversRpc]
    private void SetUsernameObserver(int clientID, string txtUsername)
    {
        instance._playerCards[clientID].SetUsername(txtUsername);
    }

    // Kills
    public static void SetKills(int clientID, int kills)
    {
        instance.SetKillsServer(clientID, kills);
    }

    [Server]
    public void SetKillsServer(int clientID, int kills)
    {
        SetKillsObserver(clientID, kills);
    }

    [ObserversRpc]
    private void SetKillsObserver(int clientID, int kills)
    {
        instance._playerCards[clientID].SetKills(kills);
    }

    // Deaths
    public static void SetDeaths(int clientID, int deaths)
    {
        instance.SetDeathsServer(clientID, deaths);
    }

    [Server]
    public void SetDeathsServer(int clientID, int deaths)
    {
        SetDeathsObserver(clientID, deaths);
    }

    [ObserversRpc]
    private void SetDeathsObserver(int clientID, int deaths)
    {
        instance._playerCards[clientID].SetDeaths(deaths);
    }

}
