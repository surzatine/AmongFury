using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

public class ButtonLayout : NetworkBehaviour
{

    [SerializeField] public GameObject btnLayout;
    public bool isAndroid;
    PlayerWeapon playerWeapon = new PlayerWeapon();

    // Start is called before the first frame update
    private void Awake()
    {
        btnLayout.SetActive(false);
        isAndroid = false;
    }

    public void Update()
    {

    }

    public void OnClickBtn3()
    {
        playerWeapon.InitializeWeapon(1);
    }
    // Android Button
    public void OnToggleAndroid()
    {
        isAndroid = true;
        btnLayout.SetActive(true);
        Debug.Log("Toggle");
    }
    public void OnTogglePC()
    {
        isAndroid = false;
        btnLayout.SetActive(false);
        Debug.Log("Toggle");
    }

}
