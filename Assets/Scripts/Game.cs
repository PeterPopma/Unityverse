using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static Game Instance;

    [SerializeField] Canvas canvasMain;
    [SerializeField] Canvas canvasInfo;
    [SerializeField] TMP_Dropdown dropdownCameraFollow;
    [SerializeField] TMP_Dropdown dropdownTimeScale;
    [SerializeField] TMP_Dropdown dropdownSpaceshipSpeed;
    [SerializeField] TMP_Dropdown dropdownSpaceshipDestination1;
    [SerializeField] TMP_Dropdown dropdownSpaceshipDestination2;
    [SerializeField] TextMeshProUGUI textMessage;
    [SerializeField] TextMeshProUGUI buttonPlayText;
    [SerializeField] TextMeshProUGUI textTime;
    [SerializeField] Toggle toggleCameraFollow;
    [SerializeField] Toggle toggleCameraLockOnObject;
    [SerializeField] Toggle toggleLights;
    [SerializeField] Toggle toggleTrails;
    [SerializeField] Toggle toggleStars;
    [SerializeField] Toggle togglePoles;
    [SerializeField] Toggle toggleShowLabels;
    [SerializeField] GameObject[] spaceObjects;
    [SerializeField] GameObject poles;
    [SerializeField] GameObject pfLabel;
    [SerializeField] Spaceship spaceShip;
    [SerializeField] Earth earth;
    [SerializeField] CameraController cameraController;
    [SerializeField] CinemachineVirtualCamera vmCamera;
    GameObject cameraFollowObject;
    private int scaleFactor = 1;

    List<GameObject> labels = new List<GameObject>();

    public Toggle ToggleCameraLockOnObject { get => toggleCameraLockOnObject; set => toggleCameraLockOnObject = value; }
    public Toggle ToggleCameraFollow { get => toggleCameraFollow; set => toggleCameraFollow = value; }
    public bool Playing { get => playing; set => playing = value; }
    public CinemachineVirtualCamera VMCamera { get => vmCamera; set => vmCamera = value; }
    public GameObject CameraFollowObject { get => cameraFollowObject; set => cameraFollowObject = value; }
    public int ScaleFactor { get => scaleFactor; set => scaleFactor = value; }
    public Spaceship SpaceShip { get => spaceShip; set => spaceShip = value; }
    public Earth Earth { get => earth; set => earth = value; }

    private float simulationTime;    // in years
    private bool playing = true;

    public void ShowInfoScreen()
    {
        canvasInfo.enabled = true;
    }

    void Awake()
    {
        Instance = this;
        canvasInfo.enabled = false;
        textMessage.text = "";
        foreach (var spaceObject in spaceObjects)
        {
            dropdownCameraFollow.options.Add(new TMP_Dropdown.OptionData() { text = spaceObject.name });
            dropdownSpaceshipDestination1.options.Add(new TMP_Dropdown.OptionData() { text = spaceObject.name });
            dropdownSpaceshipDestination2.options.Add(new TMP_Dropdown.OptionData() { text = spaceObject.name });
            GameObject newLabel = Instantiate(pfLabel);
            newLabel.GetComponent<TextMeshProUGUI>().SetText(spaceObject.name);
            newLabel.transform.SetParent(canvasMain.transform, false);
            labels.Add(newLabel);
        }
        dropdownSpaceshipDestination2.options.Add(new TMP_Dropdown.OptionData() { text = "<none>" });
    }

    private void Start()
    {
        dropdownTimeScale.value = dropdownTimeScale.options.FindIndex(option => option.text == "Real-time");
        dropdownCameraFollow.value = dropdownCameraFollow.options.FindIndex(option => option.text == "Earth");
        dropdownSpaceshipDestination1.value = dropdownSpaceshipDestination1.options.FindIndex(option => option.text == "Earth");
        OnDropdownDestination1Changed();
        dropdownSpaceshipDestination2.value = dropdownSpaceshipDestination2.options.FindIndex(option => option.text == "<none>");
        OnDropdownDestination2Changed();
        // position spaceship at earth
        float angle = 2 * Mathf.PI * Time.time * Settings.TimeScale % (2 * Mathf.PI);
        spaceShip.transform.position = new Vector3(Mathf.Cos(angle) * Utilities.KmToWorldspaceUnits(149600000), 0, Mathf.Sin(angle) * Utilities.KmToWorldspaceUnits(149600000));
        simulationTime = 0;
        OnDropdownTimeScale();
        spaceShip.NextSpaceshipDestination();
        OnToggleCameraLockOnObject();
        OnDropdownSpaceshipSpeedChanged();
        togglePoles.isOn = poles.activeSelf;
        toggleShowLabels.isOn = Settings.ShowLabels;
    }

    public void OnDropdownDestination1Changed()
    {
        spaceShip.Destination1 = Array.Find(spaceObjects, o => o.name.Equals(dropdownSpaceshipDestination1.options[dropdownSpaceshipDestination1.value].text));
        spaceShip.ReachedDestination = false;
        spaceShip.NextSpaceshipDestination();
    }

    public void OnDropdownDestination2Changed()
    {
        spaceShip.Destination2 = Array.Find(spaceObjects, o => o.name.Equals(dropdownSpaceshipDestination2.options[dropdownSpaceshipDestination2.value].text));
        spaceShip.ReachedDestination = false;
        spaceShip.NextSpaceshipDestination();
    }

    public void OnDropdownSpaceshipSpeedChanged()
    {
        switch(dropdownSpaceshipSpeed.value)
        {
            case 0:
                spaceShip.Speed = 17.0833f;
                break;
            case 1:
                spaceShip.Speed = 176.46278f;
                break;
            case 2:
                spaceShip.Speed = 29989;
                break;
            case 3:
                spaceShip.Speed = 299792;
                break;
        }

    }

    public void OnDropdownCameraFollowChanged()
    {
        cameraController.ChangeFollowObject(Array.Find(spaceObjects, o => o.name.Equals(dropdownCameraFollow.options[dropdownCameraFollow.value].text)));
        toggleCameraLockOnObject.enabled = true;
        toggleCameraFollow.isOn = true;
    }

    public void OnButtonPlayClick()
    {
        playing = !playing;
        if (playing)
        {
            buttonPlayText.text = "Pause";
        }
        else
        {
            buttonPlayText.text = "Play >";
        }
    }

    public void OnDropdownTimeScale()
    {
        switch(dropdownTimeScale.options[dropdownTimeScale.value].text)
        {
            case "Real-time":
                Settings.TimeScale = 0.000000031709791983764586504312531708333f;
                break;
            case "1 minute/s":
                Settings.TimeScale = 0.0000019025875190258751902587518f;
                break;
            case "1 hour/s":
                Settings.TimeScale = 0.00011415525114155251141552511415525f;
                break;
            case "1 day/s":
                Settings.TimeScale = 0.00273972602739726027397260273973f;
                break;
            case "1 week/s":
                Settings.TimeScale = 0.01917808219178082191780821917811f;
                break;
            case "1 month/s":
                Settings.TimeScale = 0.083333f;
                break;
            case "1 year/s":
                Settings.TimeScale = 1;
                break;
            case "500 year/s":
                Settings.TimeScale = 500;
                break;
            case "2000 year/s":
                Settings.TimeScale = 2000;
                break;
            case "10000 year/s":
                Settings.TimeScale = 10000;
                break;
            default:
                Settings.TimeScale = 0.00011415525114155251141552511415525f;
                break;
        }
    }

    public void OnToggleShowStars()
    {
        if (toggleStars.isOn)
        {
            Camera.main.clearFlags = CameraClearFlags.Skybox;
        }
        else
        {
            Camera.main.clearFlags = CameraClearFlags.SolidColor;
        }
    }

    public void OnToggleShowPoles()
    {
        poles.SetActive(togglePoles.isOn);
    }

    public void OnToggleShowLabels()
    {
        Settings.ShowLabels = toggleShowLabels.isOn;
    }

    public void OnToggleCameraFollowChanged()
    {
        if (!toggleCameraFollow.isOn)
        {
            cameraController.ChangeFollowObject(null);
            toggleCameraLockOnObject.enabled = false;
            toggleCameraLockOnObject.isOn = false;
        }
        else
        {
            OnDropdownCameraFollowChanged();
        }
    }

    public void OnToggleCameraLockOnObject()
    {
        cameraController.SetLockOnObject(toggleCameraLockOnObject.isOn);
    }

    public void OnToggleTrailsChanged()
    {
        if (toggleTrails.isOn)
        {
            SetTrailsActive(true);
        }
        else
        {
            SetTrailsActive(false);
        }
    }

    public void OnToggleLightsChanged()
    {
        foreach (Transform t in Utilities.FindChildren(spaceShip.transform, "light"))
        {
            t.gameObject.GetComponent<Light>().enabled = toggleLights.isOn;
        }                              
    }

    public void OnClickButtonExit()
    {
        Application.Quit();
    }

    public void OnClickCloseInfoScreen()
    {
        canvasInfo.enabled = false;
    }

    private void SetTrailsActive(bool active)
    {
        foreach(GameObject spaceObject in spaceObjects)
        {
            if (spaceObject.GetComponent<TrailRenderer>() != null)
            {
                spaceObject.GetComponent<TrailRenderer>().enabled = active;
            }
        }
    }

    private string CalculateTime()
    {
        DateTime time = DateTime.Now;
        int yearsPassed = (int)simulationTime;
        time = time.AddYears(yearsPassed);
        time = time.AddSeconds((simulationTime-yearsPassed)*365*24*60*60);
        return time.ToString(CultureInfo.InvariantCulture);
    }

    public void FixedUpdate()
    {
        textMessage.text = "spaceship: " + spaceShip.GetProgress() + "%";
    }

    public void Update()
    {
        float cameraDistanceToSun = Camera.main.transform.position.magnitude;
        for (int i = 0; i < spaceObjects.Length; i++)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(spaceObjects[i].transform.position);
            Vector2 canvasPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasMain.GetComponent<RectTransform>(), screenPos, null, out canvasPos);

            if (screenPos.z > 0 && Settings.ShowLabels && spaceObjects[i].transform.position.magnitude < cameraDistanceToSun)
            {
                labels[i].SetActive(true);
                labels[i].GetComponent<RectTransform>().anchoredPosition = canvasPos;
            }
            else
            {
                labels[i].SetActive(false);
            }
        }

        if (!Playing)
        {
            return;
        }
        simulationTime += Time.deltaTime * Settings.TimeScale;
        textTime.text = CalculateTime();
    }

}
