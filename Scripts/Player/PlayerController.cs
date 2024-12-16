using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Demo.AdditiveScenes;
using Unity.Jobs.LowLevel.Unsafe;


// Username Json Class
class JsonReadUsername
{
    public string username;
    public string password;
    public string serverIP;
}

//This is made by Bobsi Unity - Youtube
public class PlayerController : NetworkBehaviour
{
    public string txtUsername;
    public static Dictionary<int, PlayerController> Players = new Dictionary<int, PlayerController>();

    [Header("Base setup")]
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    [SerializeField] private int playerSelfLayer = 7;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    [SerializeField]
    private float cameraYOffset = 0.4f;
    private float cameraZOffset = 0.29f;
    private Camera playerCamera;

    // Joystick
    public Joystick joystick;
    float xMove = 0f;
    float zMove = 0f;



    public override void OnStartClient()
    {
        string json = File.ReadAllText(Application.dataPath + "/userconfig.json");
        JsonReadUsername jsonReadUsername = JsonUtility.FromJson<JsonReadUsername>(json);
        txtUsername = jsonReadUsername.username;
        base.OnStartClient();

        Players.Add(OwnerId, this);
        PlayerManager.InitializeNewPlayer(OwnerId);

        // File read and send Username to PlayerJoined() method
        GameUIManager.PlayerJoined(OwnerId, txtUsername);


        if (base.IsOwner)
        {
            playerCamera = Camera.main;
            playerCamera.transform.position = new Vector3(transform.position.x, transform.position.y + cameraYOffset, transform.position.z + cameraZOffset);
            playerCamera.transform.SetParent(transform);

            if (TryGetComponent(out PlayerWeapon playerWeapon))
            {
                playerWeapon.InitializeWeapons(playerCamera.transform);
            }

            gameObject.layer = playerSelfLayer;
            foreach (Transform child in transform)
            {
                child.gameObject.layer = playerSelfLayer;
            }
        }
        else
        {
            gameObject.GetComponent<PlayerController>().enabled = false;
        }
    }

    public override void OnStopClient()
    {
        base.OnStopClient();

        Players.Remove(OwnerId);
        PlayerManager.PlayerDisconnected(OwnerId);
        GameUIManager.PlayerLeft(OwnerId);

    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }


    void Start()
    {

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        bool isRunning = false;

        // Press Left Shift to run
        isRunning = Input.GetKey(KeyCode.LeftShift);

        // We are grounded, so recalculate move direction based on axis
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove && playerCamera != null)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        // Android
        xMove = joystick.Horizontal * walkingSpeed;
        zMove = joystick.Vertical * walkingSpeed;

        if (xMove != 0 || zMove != 0)
        {

            Vector3 move = transform.right * xMove + transform.forward * zMove;

            characterController.Move(move * walkingSpeed * Time.deltaTime);

            // velocity.y += gravity * Time.deltaTime;

            // controller.Move(velocity * Time.deltaTime);
        }

    }

    public static void SetPlayerPostion(int clientID, Vector3 position)
    {
        if (!Players.TryGetValue(clientID, out PlayerController player))
        {
            return;
        }
        player.SetPlayerPostionServer(position);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerPostionServer(Vector3 position)
    {
        SetPlayerPostionTarget(Owner, position);
    }

    [TargetRpc]
    private void SetPlayerPostionTarget(NetworkConnection conn, Vector3 position)
    {
        transform.position = position;
    }



    public static void TogglePlayer(int clientID, bool toggle)
    {
        if (!Players.TryGetValue(clientID, out PlayerController player))
        {
            return;
        }
        player.TogglePlayerServer(toggle);

    }


    [ServerRpc(RequireOwnership = false)]
    public void TogglePlayerServer(bool toggle)
    {
        DisablePlayerObserver(toggle);
    }


    [ObserversRpc]
    public void DisablePlayerObserver(bool toogle)
    {
        canMove = toogle;
        if (TryGetComponent(out Renderer playerRenderer))
        {
            playerRenderer.enabled = toogle;
        }

        if (TryGetComponent(out MeshCollider collider))
        {
            collider.enabled = toogle;
        }

        characterController.enabled = toogle;


        Renderer[] allRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in allRenderers)
        {
            renderer.enabled = toogle;
        }

    }
}