using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using Cinemachine;
using System.Collections;
using DG.Tweening;
using TMPro;
/*
GameManager class is the main class for getting input from the user and also updating UI elements
*/
public class GameManager : MonoBehaviour
{
    // There are 4 types of controller for the car
    public enum controllerType
    {
        ArrowKeys,
        ArrowTouch,
        gyroscope,
        wheel,
        mobile
    }
    public Cinemachine.CinemachineVirtualCamera virtualCamera;
   public Cinemachine.CinemachineTransposer transposer;

   public controllerType controller = controllerType.ArrowKeys;   // Set the default controller type
    public Transform startPosition;                         // Start position of the car. The car appears and the positon and rotation of this variable
    [HideInInspector]
    public float VerticalInput = 0;                         // Vertical input of the current device
    [HideInInspector]
    public float HorizontalInput = 0;                       // Horizontal input of the current device
    [HideInInspector]
    public bool hBrake = false;                             // If handbrake button pressed it becomes true
[Header("touching button")]
    public TouchButton gas_pedal;                           // Reference to the gas pedal UI element
    public TouchButton brake_reverse_pedal;                 // Reference to the brake(reverse) pedal UI element
    public TouchButton handbrake;                           // Reference to the handbrake pedal UI element
    public TouchButton leftArrow;                           // Reference to the left arrow UI element
    public TouchButton rightArrow;                          // Reference to the right arrow UI element
    public List<TouchButton> buttons;
    public Image wheel;                                     // Reference to the wheel UI element
    [HideInInspector]
    public float speed;                                     // The car speed
    public Transform needle;                                // Reference to the needle of speedometer UI
    float needleMin = 222f;
    float needleMax = -47f;
    float needlePosition;

    public steerWheel sw;                                   // Reference to the steerWheel class

    //Transform[] cameras = new Transform[4];                 // We have 4 cameras in the scene. Two of them are in the car game object
    //int activeCameraIndex = 0;                              // Current active camera. Always one camera should be active in the scene

    public Transform[] controls = new Transform[4];         // Reference to 4 controller types in the game
    int activeControlIndex = 0;                             // Current active controller

    int activeQualityIndex = 2;                             // Current graphic quality setting
    public Text qualityText;                                // Current graphic quality

    float smoothTarget = 0;                                 // When user press left or right touch arrow keys slowly decrease it to -1 or increase it to 1
    float smoothResult = 0;                         

    public Text speedText;                                  // Showing the car speed
    string speedString;

    public GameObject[] cars = new GameObject[3];           // Reference to the car prefabs. You can add as many car as you wish here.

    Transform car;
   public CarControllerPro player;
    public int startGameDelay;
    public bool policeScene;
    public bool policeWin;
    public List<AiCarController> policeCars;
    public Helicopter helicopter;
    public float timeStopSpeedByPoliceScene;
    public static GameManager instance;
    public TextMeshProUGUI timeStartText;
    public StartFlag startFlag;
    // Current instantiated car 
    public int currentSceneIndex;
    public Button restartButton;
    public int SeasonIndex;
    public bool mobilePlatform;

    private void Awake()
    {
        instance = this;
       
    }

    void Start()
    {
        StartUp();
        if (Application.isMobilePlatform == false )
        {
            buttons.ForEach(b => b.gameObject.SetActive(false));
           

        }
        else if ((Application.isMobilePlatform))
        {
            controller = GameManager.controllerType.mobile;
        }
        if (policeScene == false)
        {
            if (TableRacers.instance)
                TableRacers.instance.Init(player);
            StartCoroutine(StartGameDelay());

            currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            restartButton.onClick.AddListener(Restart);
            //Debug.Log("restart на полицейской сцене");
        }
        //if(mobilePlatform == true) {
        //    buttons.ForEach(b => b.gameObject.SetActive(true));
        //    controller = GameManager.controllerType.mobile;
        //}

    }

    private void Restart()
    {
        SceneManager.LoadScene(currentSceneIndex);
    }

    IEnumerator StartGameDelay()
    {
        var startScale = timeStartText.transform.localScale;
        timeStartText.text = startGameDelay.ToString();
        while (startGameDelay > 0)
        {
            timeStartText.transform.localScale = startScale;
            yield return new WaitForSeconds(1f);
            startGameDelay --;
            timeStartText.transform.DOScale(new Vector3(startScale.x + 3,startScale.y + 3),1f);
            timeStartText.text =  startGameDelay.ToString();
            startFlag.Active(true);
            //timeText
        }
        startGameDelay = 0;
        timeStartText.transform.localScale = startScale;
        timeStartText.transform.DOScale(new Vector3(startScale.x + 3, startScale.y + 3), 1f);
        timeStartText.color = Color.green;
        timeStartText.text = "Гоу!!!";
        startFlag.Active(true);
        yield return new WaitForSeconds(1f);
        timeStartText.gameObject.SetActive(false);

        //timeText
    }
    void Update()
    {
        // Exit the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Leave();
        }

        // When user press left or right touch arrow keys slowly decrease it to -1 or increase it to 1
        smoothTo(smoothTarget);

        // If the current platform is editor or windows get vertical input values
        

        if ((Application.isMobilePlatform == false))
        {

            VerticalInput = Input.GetAxisRaw("Vertical");

            if (Input.GetKey(KeyCode.Space))
                hBrake = true;
            else
                hBrake = false;

            transposer.m_FollowOffset = new Vector3(player.CameraController.offsetX, transposer.m_FollowOffset.y, transposer.m_FollowOffset.z);
            virtualCamera.m_Lens.Dutch = player.CameraController.datch;


        }
        else if ((Application.isMobilePlatform) || mobilePlatform)

        {
            if (gas_pedal.Pressed)
            {
                VerticalInput = 1;

            }
            else if (brake_reverse_pedal.Pressed)
            {
                VerticalInput = -1;
            }
            else
            {
                VerticalInput = 0;
            }

            if (handbrake.Pressed)
                hBrake = true;
            else
                hBrake = false;
        }


        // Based on controller type get horizontal input
        switch (controller)
        {
            case controllerType.ArrowKeys:
                HorizontalInput = Input.GetAxis("Horizontal");
                break;
            case controllerType.ArrowTouch:
                if (rightArrow.Pressed)
                {
                    smoothTarget = 1;
                    HorizontalInput = smoothResult;
                }
                else if (leftArrow.Pressed)
                {
                    smoothTarget = -1;
                    HorizontalInput = smoothResult;
                }
                else
                {
                    smoothTarget = 0;
                    HorizontalInput = smoothResult;
                }
                break;
            case controllerType.gyroscope:
                HorizontalInput = Input.acceleration.x * 3;
                break;
            case controllerType.wheel:
                HorizontalInput = sw.horizontal;
                break;
            case controllerType.mobile:
                if (rightArrow.Pressed)
                {
                    smoothTarget = 1;
                    HorizontalInput = smoothResult;
                }
                else if (leftArrow.Pressed)
                {
                    smoothTarget = -1;
                    HorizontalInput = smoothResult;
                }
                else
                {
                    smoothTarget = 0;
                    HorizontalInput = smoothResult;
                }
                break;
        }

        // Showing the car speed on the UI
        //if (speed < 10)
        //{
        //    speedString = "00" + speed.ToString();
        //}
        //else if (speed >= 10 && speed <= 99)
        //{
        //    speedString = "0" + speed.ToString();
        //}
        //else
        //    speedString = speed.ToString();


       


    }
   
    private void FixedUpdate()
    {
        speedText.text = (Convert.ToInt32(player.carSpeedConverted * 1.6F)).ToString();
        if(policeScene && policeWin == false)
        {
            if(Convert.ToInt32( speedText.text)< 30f )
            {
                timeStopSpeedByPoliceScene += Time.deltaTime;
                //Debug.Log(timeStopSpeedByPoliceScene + " > 6f");
                if(timeStopSpeedByPoliceScene > 15f)
                {
                    //Debug.Log("Тебя поймали копы");
                    if (EndPoliceScene.instance.end == EndType.none)
                        CanvasInfo.instance.SetInfoText("Пойман копами!");

                    policeWin = true;
                    EndPoliceScene.instance.SetTypeEnd(EndType.detainedByPolice);
                }

            }
            else if( timeStopSpeedByPoliceScene > 0 ) { timeStopSpeedByPoliceScene = 0; }
        }
    }


    public void controlButton()
    {

        if (activeControlIndex < controls.Length-1)
        {
            controls[activeControlIndex].gameObject.SetActive(false);
            activeControlIndex++;
            controls[activeControlIndex].gameObject.SetActive(true);
        }
        else
        {
            controls[activeControlIndex].gameObject.SetActive(false);
            activeControlIndex = 0;
            controls[activeControlIndex].gameObject.SetActive(true);
        }

        switch (activeControlIndex)
        {
            case 0:
                clearControls();
                controller = controllerType.ArrowKeys;
                break;
            case 1:
                clearControls();
                leftArrow.gameObject.SetActive(true);
                rightArrow.gameObject.SetActive(true);
                controller = controllerType.ArrowTouch;
                break;
            case 2:
                clearControls();
                controller = controllerType.gyroscope;
                break;
            case 3:
                clearControls();
                wheel.gameObject.SetActive(true);
                controller = controllerType.wheel;
                break;
        }
    }

    // By pressing the quality button go to the next quality setting
    public void qualityButton()
    {
        if (activeQualityIndex < 2)
        {
            activeQualityIndex++;
            QualitySettings.SetQualityLevel(activeQualityIndex, true);
        }
        else
        {
            activeQualityIndex = 0;
            QualitySettings.SetQualityLevel(activeQualityIndex, true);
        }

        if (activeQualityIndex == 0)
            qualityText.text = "Graphic: Low";
        else if (activeQualityIndex == 1)
            qualityText.text = "Graphic: Medium";
        else if (activeQualityIndex == 2)
            qualityText.text = "Graphic: High";
    }

    // It disables the touch arrows and the wheel control from the UI 
    void clearControls()
    {
        leftArrow.gameObject.SetActive(false);
        rightArrow.gameObject.SetActive(false);
        wheel.gameObject.SetActive(false);
    }

    // Smoothly increases the current value to the target vlaue
    void smoothTo(float target)
    {
        float smoothness = 3;
        
        if (smoothResult < target)
        {
            smoothResult = smoothResult + (Time.deltaTime * smoothness);
        }
        else if (smoothResult > target)
        {
            smoothResult = smoothResult - (Time.deltaTime * smoothness);
        }
        
    }

    // Calculates the needle rotation angle of the speedometer


    // Going back to car selection menu
    public void Leave()
    {
        PersistentData.Level = 1;
        SceneManager.LoadScene(0);
    }

    // This function initialize the selected car and cameras
    void StartUp()
    {
        // Read from the PersistentData class that which car is selected by the user and instantiate it
        car = (Instantiate(cars[PersistentData.selectedCarIndex], startPosition.position, startPosition.rotation)).transform;
        player = car.gameObject.GetComponent<CarControllerPro>();
        player.name = "Вы";
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

       
        // Find the cameras inside and in front of the car and assign them automatically
        //if (car.Find("CarCamera Inside") != null)
        //    cameras[1] = car.Find("CarCamera Inside").transform;

        //if (car.Find("CarCamera Front") != null)
        //    cameras[2] = car.Find("CarCamera Front").transform;

        //// Set the car as the target of the other 2 cameras in the scene
        //cameras[0].GetComponent<AutoCam>().Target = car.Find("Camera Pivot");
        //cameras[3].GetComponent<AutoCam>().Target = car.Find("Camera Pivot");
        virtualCamera.Follow = player.gameObject.transform;
        virtualCamera.LookAt = player.gameObject.transform;
        if(policeScene)
        {
            policeCars.ForEach(p => p.mobileTarget = player);
            helicopter.Init(player);
        }
    }

    // This function restart the game
    public void RestartPressed()
    {
        //cameras[activeCameraIndex].gameObject.SetActive(false);
        //activeCameraIndex = 0;
        //cameras[activeCameraIndex].gameObject.SetActive(true);
        Destroy(car.gameObject);
        StartUp();
    }
}
