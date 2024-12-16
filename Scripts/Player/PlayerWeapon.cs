using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerWeapon : NetworkBehaviour
{
    [SerializeField] private List<APlayerWeapon> weapons = new List<APlayerWeapon>();
    [SerializeField] private APlayerWeapon currentWeapon;
    // [SerializeField] private TextMeshProUGUI isAndroidText;

    // public Button btnToggleAndroid;

    // Android Button
    // [SerializeField] private Button btnFire;
    // [SerializeField] private Button btn1;
    // [SerializeField] private Button btn2;
    // [SerializeField] private Button btn3;
    // [SerializeField] private Button btn4;
    // [SerializeField] private Button btnAndroid;
    // [SerializeField] private Button btnPC;
    // [SerializeField] private Button btnScore;
    // [SerializeField] private Button btnJoystick;
    // [SerializeField] private GameObject btnLayout;


    public int isAndroid;


    private readonly SyncVar<int> _currentWeaponIndex = new(-1);

    // private void Start()
    // {

    //     btnToggleAndroid.onClick.AddListener(OnToggleAndroid);
    // }

    private void Awake()
    {
        _currentWeaponIndex.OnChange += OnCurrentWeaponIndexChanged;
        isAndroid = 0;
    }



    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
        {
            enabled = false;
            return;
        }
    }

    public void Update()
    {
        // Fire
        if (Input.GetKey(KeyCode.Mouse0) && isAndroid == 0)
        {
            FireWeapon();
        }
        if (Input.GetKey(KeyCode.Mouse1) && currentWeapon.name == "pistol_knife")
        {
            KnifeStabb();
        }
        // WEapon Swap

        if (Input.GetKey(KeyCode.V))
        {
            OnToggleAndroid();
        }

        // AWM
        if (Input.GetKey(KeyCode.Alpha1))
        {
            Debug.Log(isAndroid);
            InitializeWeapon(0);
        }
        // Pistol
        if (Input.GetKey(KeyCode.Alpha2))
        {
            InitializeWeapon(3);
        }
        // AR15
        if (Input.GetKey(KeyCode.Alpha3))
        {
            InitializeWeapon(1);
        }
        // Vector
        if (Input.GetKey(KeyCode.Alpha4))
        {
            InitializeWeapon(2);
        }
    }

    public void InitializeWeapons(Transform parentOfWeapons)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].transform.SetParent(parentOfWeapons);
        }

        InitializeWeapon(0);
    }

    public void InitializeWeapon(int weaponIndex)
    {
        SetWeaponIndex(weaponIndex);
    }

    [ServerRpc] private void SetWeaponIndex(int weaponIndex) => _currentWeaponIndex.Value = weaponIndex;

    private void OnCurrentWeaponIndexChanged(int oldIndex, int newIndex, bool asServer)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].gameObject.SetActive(false);
        }

        if (weapons.Count > newIndex)
        {
            currentWeapon = weapons[newIndex];
            currentWeapon.gameObject.SetActive(true);
        }
    }

    private void FireWeapon()
    {
        currentWeapon.Fire();
        // Debug.Log(currentWeapon);
    }
    private void KnifeStabb()
    {
        currentWeapon.Knife();
        // Debug.Log(currentWeapon.name);
    }

    // Android Button
    public void OnToggleAndroid()
    {
        isAndroid = 1;
        // isAndroidText.text = "Android";
        Debug.Log("Toggle");
    }
    public void OnTogglePC()
    {
        isAndroid = 0;
        // isAndroidText.text = "PC";
        Debug.Log("Toggle");
    }
    // public void OnFire()
    // {
    //     FireWeapon();
    // }
    // public void OnBtn1()
    // {
    //     InitializeWeapon(0);
    // }
    // public void OnBtn2()
    // {
    //     InitializeWeapon(3);
    // }
    // public void OnBtn3()
    // {
    //     InitializeWeapon(1);
    // }
    // public void OnBtn4()
    // {
    //     InitializeWeapon(2);
    // }

}
