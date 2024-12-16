using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FishNet.Connection;
using FishNet.Object;
public class NetworkSync : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        PlayerController playerController = new PlayerController();
        OnServerRPCNetworkSync(playerController);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // [ServerRPC]
    public void OnServerRPCNetworkSync(PlayerController playerController)
    {

        OnObserverRPCNetworkSync(playerController);
    }


    // [ObserverRPC]
    public void OnObserverRPCNetworkSync(PlayerController playerController)
    {
        playerController.enabled = true;
    }
}



