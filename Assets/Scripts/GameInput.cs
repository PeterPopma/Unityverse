using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    CinemachineVirtualCamera vcamMainCamera;

    [SerializeField] TextMeshProUGUI textMessage;
    [SerializeField] private float moveByKeySpeed = 4f; 
    [SerializeField] private float lookSpeed = 2f;
    [SerializeField] private float zoomSpeed = 2f;
    bool lockOnObject;
    Vector3 cameraPosition;

    private float cameraYaw = 0f;
    private float cameraPitch = 0f;
    private float cameraRoll = 0f;
    private float cameraDistance = 3f;
    private bool buttonCameraForward;
    private bool buttonCameraBack;
    private bool buttonCameraLeft;
    private bool buttonCameraRight;
    private bool buttonCameraUp;
    private bool buttonCameraDown;
    private bool buttonCameraLookAround;
    private bool buttonCameraRollLeft;
    private bool buttonCameraRollRight;
    private Vector2 look;
    private float timeMovingStarted;

    public Vector3 CameraPosition { get => cameraPosition; set => cameraPosition = value; }
    public bool LockOnObject { get => lockOnObject; set => lockOnObject = value; }

    public void Awake()
    {
        vcamMainCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void Start()
    {
    }

    private void OnLook(InputValue value)
    {
        look = value.Get<Vector2>();
    }

    private void OnInfoScreen()
    {
        Game.Instance.ShowInfoScreen();
    }

    public void ChangeFollowObject(GameObject followObject)
    {
        if (followObject != null)
        {
            // place the camera at the minimum distance by default.
            cameraPosition = followObject.transform.position + new Vector3(followObject.GetComponent<SpaceObject>().MinimumViewDistance, 0, 0);
            cameraDistance = followObject.GetComponent<SpaceObject>().MinimumViewDistance;
        }
        Game.Instance.CameraFollowObject = followObject;
    }

    private void OnCameraForward(InputValue value)
    {
        timeMovingStarted = Time.time;
        buttonCameraForward = value.isPressed;
    }

    private void OnCameraBack(InputValue value)
    {
        timeMovingStarted = Time.time;
        buttonCameraBack = value.isPressed;
    }

    private void OnCameraLeft(InputValue value)
    {
        timeMovingStarted = Time.time;
        buttonCameraLeft = value.isPressed;
    }

    private void OnCameraRight(InputValue value)
    {
        timeMovingStarted = Time.time;
        buttonCameraRight = value.isPressed;
    }

    private void OnCameraUp(InputValue value)
    {
        timeMovingStarted = Time.time;
        buttonCameraUp = value.isPressed;
    }

    private void OnCameraDown(InputValue value)
    {
        timeMovingStarted = Time.time;
        buttonCameraDown = value.isPressed;
    }

    private void OnCameraLookAround(InputValue value)
    {
        buttonCameraLookAround = value.isPressed;
    }

    private void OnCameraRollLeft(InputValue value)
    {
        buttonCameraRollLeft = value.isPressed;
    }

    private void OnCameraRollRight(InputValue value)
    {
        buttonCameraRollRight = value.isPressed;
    }

    private void OnCameraReset()
    {
        cameraPitch = 0;
        cameraYaw = 0;
        cameraRoll = 0;
    }

    public void LateUpdate()
    {
        vcamMainCamera.transform.eulerAngles = new Vector3(cameraPitch, cameraYaw, cameraRoll);

        if (Game.Instance.CameraFollowObject == null)
        {
            vcamMainCamera.transform.position = cameraPosition;
        }
        else if (!LockOnObject)
        {
            vcamMainCamera.transform.position = Game.Instance.CameraFollowObject.transform.position + cameraPosition;
        }
        else
        {
            vcamMainCamera.transform.position = Game.Instance.CameraFollowObject.transform.position - vcamMainCamera.transform.forward * cameraDistance;
        }
    }

    private void Update()
    {
        textMessage.text = "camera-sun distance: " + string.Format("{0:0}", Utilities.WorldspaceUnitsToKM(Camera.main.transform.position.magnitude));
        float moveSpeed = moveByKeySpeed * Mathf.Pow(2, (Time.time - timeMovingStarted));

        if (buttonCameraRollLeft)
        {
            cameraRoll -= Time.deltaTime * 80;
        }
        if (buttonCameraRollRight)
        {
            cameraRoll += Time.deltaTime * 80;
        }
        if (buttonCameraForward)
        {
            if (LockOnObject)
            {
                if (cameraDistance - Time.deltaTime * moveSpeed > Game.Instance.CameraFollowObject.GetComponent<SpaceObject>().MinimumViewDistance)
                {
                    cameraDistance -= Time.deltaTime * moveSpeed;
                }
            }
            else
            {
                cameraPosition += transform.forward * Time.deltaTime * moveSpeed;
            }
        }
        if (buttonCameraBack)
        {
            if (LockOnObject)
            {
                cameraDistance += Time.deltaTime * moveSpeed;
            }
            else
            {
                cameraPosition -= transform.forward * Time.deltaTime * moveSpeed;
            }
        }
        if (buttonCameraRight)
        {
            cameraPosition += transform.right * Time.deltaTime * moveSpeed;
        }
        if (buttonCameraLeft)
        {
            cameraPosition -= transform.right * Time.deltaTime * moveSpeed;
        }
        if (buttonCameraUp)
        {
            cameraPosition += transform.up * Time.deltaTime * moveSpeed;
        }
        if (buttonCameraDown)
        {
            cameraPosition -= transform.up * Time.deltaTime * moveSpeed;
        }

        // Look around when right mouse is pressed
        if (buttonCameraLookAround)
        {
            cameraYaw += lookSpeed * look.x;
            cameraPitch -= lookSpeed * look.y;
        }
    }

    public void SetLockOnObject(bool isLocked)
    {
        LockOnObject = isLocked;
        if (!LockOnObject)
        {
            // we have changed to free-look. start from the same position and rotation as when looking to the object
            cameraPosition = -vcamMainCamera.transform.forward * cameraDistance;
        }
    }
}