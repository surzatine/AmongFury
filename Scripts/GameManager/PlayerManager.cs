using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Example.ColliderRollbacks;
using Unity.VisualScripting;
using UnityEditor;
using System.IO;
using UnityEngine.Networking;

using UnityEngine.UI;
using TMPro;
using System;




// class JsonDataPlayerManager
// {
//     public string username;
//     public string password;
//     public string serverIP;
// }




//This is made by Bobsi Unity - Youtube
public class PlayerManager : NetworkBehaviour
{

    private static PlayerManager instance;
    public string serverUrl;
    public string killer;
    public string victim;
    public string room;
    // public GameObject go;


    // PlayerHealth playerHealth;
    [SerializeField] private float respawnTime = 3f;

    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    private Dictionary<int, Player> _players = new Dictionary<int, Player>();
    private List<int> _deathPlayers = new List<int>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }


    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!IsServerInitialized)
        {
            enabled = false;
            return;
        }
    }


    public void Update()
    {
        for (int i = 0; i < _deathPlayers.Count; i++)
        {
            if (_players[_deathPlayers[i]].deathTime < Time.time - respawnTime)
            {
                RespawnPlayer(_deathPlayers[i]);
                _deathPlayers.RemoveAt(i);
                return;
            }
        }
    }

    public void RespawnPlayer(int clientID)
    {
        PlayerController.SetPlayerPostion(clientID, spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)].position);
        PlayerController.TogglePlayer(clientID, true);
        if (PlayerHealth.Players.TryGetValue(clientID, out PlayerHealth playerHealth))
        {

            playerHealth.ResetHealth();

        }
    }


    public static void InitializeNewPlayer(int clientID)
    {
        instance._players.Add(clientID, new Player());
    }

    public static void PlayerDisconnected(int clientID)
    {
        instance._players.Remove(clientID);
    }


    public static void PlayerDied(int player, int killer)
    {
        if (instance._players.TryGetValue(killer, out Player killerPlayer))
        {
            killerPlayer.Score += 1;
        }
        if (instance._players.TryGetValue(player, out Player deadPlayer))
        {
            deadPlayer.Deaths += 1;
            deadPlayer.deathTime = Time.time;
        }

        Debug.Log($"[*] {killer} has killed [!] {player}. +");
        // Debug.Log($"[%] {killer} has  {killerPlayer.Score} kills. +");
        // Debug.Log($"[%] {player} has {deadPlayer.Deaths} deaths. +");

        GameUIManager.SetKills(killer, killerPlayer.Score);
        GameUIManager.SetDeaths(player, deadPlayer.Deaths);
        instance._deathPlayers.Add(player);


        // Use Delegates to call DatabaseKillDeath.DbKillDeath()
        PlayerManager playerManager = new PlayerManager();
        playerManager.CallDatabaseKillDeath(killer, player);


    }

    // Call function DAtabase Kill Death From Database/killDeath.cs file
    public void CallDatabaseKillDeath(int killer, int victim)
    {
        var tempgameObject = new GameObject();
        DatabaseKillDeath databaseKillDeath = tempgameObject.AddComponent<DatabaseKillDeath>();
        databaseKillDeath.DbKillDeath(killer, victim);
    }

    class Player
    {
        // public int clientID = -1;
        public int Score = 0;
        public int Deaths = 0;

        public float deathTime = -99;
    }

}