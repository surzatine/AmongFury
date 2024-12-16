using System.Collections;
using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Demo.AdditiveScenes;
using FishNet.Object;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerHealth : NetworkBehaviour
{
    public static Dictionary<int, PlayerHealth> Players = new Dictionary<int, PlayerHealth>();
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private ParticleSystem bloodSplashDeadEffect;
    private int _currentHealth;

    private void Awake()
    {
        _currentHealth = maxHealth;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        Players.Add(OwnerId, this);

        if (!IsOwner)
        {
            enabled = false;
            return;
        }

        GameUIManager.SetHealthText(maxHealth.ToString());
    }

    public override void OnStopClient()
    {
        base.OnStopClient();

        Players.Remove(OwnerId);
    }


    [ServerRpc(RequireOwnership = false)]
    public void ResetHealth()
    {
        _currentHealth = maxHealth;
        LocalSetHealth(Owner, _currentHealth);
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamage(int damage, int attackerID)
    {
        _currentHealth -= damage;
        // Debug.Log("New Player Health: " + _currentHealth);


        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            // PlayBloodSplash();
            Die(attackerID);
        }
        LocalSetHealth(Owner, _currentHealth);

    }

    private void Die(int attackerID)
    {
        // Debug.Log("Player died");
        PlayerController.TogglePlayer(OwnerId, false);
        PlayerManager.PlayerDied(OwnerId, attackerID);
    }

    [ObserversRpc]
    public void PlayBloodSplash()
    {
        bloodSplashDeadEffect.Play();
    }


    [TargetRpc]
    private void LocalSetHealth(NetworkConnection conn, int newHealth)
    {
        GameUIManager.SetHealthText(newHealth.ToString());
    }
}
