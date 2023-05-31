using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
    CarControllerPro class is the main class for controlling the car
*/
[Serializable]
public class CameraController
{
    public float datch;
    public float offsetX;
    private float smoothX = 0;
    public void UpdateOffset(float horizontal)
    {
        if (horizontal > 0)
        {
            smoothX = (Time.deltaTime * 1.1f);
           datch = Mathf.Lerp(datch, 6, smoothX);
            offsetX = Mathf.Lerp(offsetX, 1.84f, smoothX);
        }
        if (horizontal < 0)
        {
            smoothX = (Time.deltaTime * 1.1f);
            datch = Mathf.Lerp(datch, -6, smoothX);
            offsetX = Mathf.Lerp(offsetX, -1.84f, smoothX);
        }
        if (horizontal == 0)
        {
            smoothX = (Time.deltaTime * 1.1f);
          datch = Mathf.Lerp(datch, 0, smoothX);
            offsetX = Mathf.Lerp(offsetX, 0, smoothX);
        }
    }
}
public class CarControllerPro : MonoBehaviour 
{
   
    public CameraController CameraController;
    public enum CarType
    {
        FrontWheelDrive,   // Motor torque just applies to the front wheels
        RearWheelDrive,    // Motor torque just applies to the rear wheels
        FourWheelDrive     // Motor torque applies to the all wheels
    }
    public string name;
    //************** Drag each wheel collider to the corresponding variable *********************
    [SerializeField] public WheelCollider Wheel_Collider_Front_Left;
    [SerializeField] public WheelCollider Wheel_Collider_Front_Right;
    [SerializeField] public WheelCollider Wheel_Collider_Rear_Left;
    [SerializeField] public WheelCollider Wheel_Collider_Rear_Right;
    //*******************************************************************************************
    //************** Drag each wheel mesh to the corresponding variable *************************
    [SerializeField] public GameObject Wheel_Mesh_Front_Left;
    [SerializeField] public GameObject Wheel_Mesh_Front_Right;
    [SerializeField] public GameObject Wheel_Mesh_Rear_Left;
    [SerializeField] public GameObject Wheel_Mesh_Rear_Right;
    //*******************************************************************************************

    [SerializeField] protected GameManager gameManager;                            // Reference to the gamemanager class
    [SerializeField] public float maxMotorTorque;                        // Maximum torque the motor can apply to wheels
    [SerializeField] public float maxSteeringAngle = 20;                 // Maximum steering angle the wheels can have    
    [SerializeField] public float maxSpeed;                              // Car maximum speed
    [SerializeField] public float brakePower;                            // Maximum brake power
    [SerializeField] public Transform CenterOfMass;                      // Center of mass of the car
    [SerializeField] public CarType carType = CarType.FourWheelDrive;    // Set car type here
    [SerializeField] public Transform steeringWheel;                     // Drag the car's Steering wheel mesh to here 
    [SerializeField] public float carSpeed;                                     // The car speed in meter per second 
    [SerializeField] public float carSpeedConverted;                            // The car speed in kilometer per hour
    [SerializeField] protected  float motorTorque;                                  // Current Motor torque
    [SerializeField] protected  float tireAngle;                                    // Current steer angle
    [SerializeField] protected float steeringWheelAngle;                           // Steering wheel angle
    [SerializeField] protected float steeringWheelAngleLerp;                       // Lerp the steering wheel angle
    [SerializeField] protected float vertical = 0;                                 // The vertical input
    [SerializeField] protected float horizontal = 0;                               // The horizontal input    
    [SerializeField] protected Rigidbody carRigidbody;                             // Rigidbody of the car
    [SerializeField] protected AudioSource engineAudioSource;                      // The engine audio source
    [SerializeField] public AudioSource bumpAudioSource;                 // Bump audio source
    [SerializeField] protected float engineAudioPitch = 1.4f;                      // The engine audio pitch
    [SerializeField] public Transform brakeLightLeft;                    // Drag the left brake light mesh here
    [SerializeField] public Transform brakeLightRight;                   // Drag the right brake light mesh here
    [SerializeField] protected  Material brakeLightLeftMat;                         // Reference to the left brake light material
    [SerializeField] protected Material brakeLightRightMat;                        // Reference to the right brake light material
    [SerializeField] protected Color brakeColor = new Color32(180, 0, 10, 0);      // The emission color of the brake lights

    [SerializeField] public bool isAI = false;
    [SerializeField] public float radiusOverlap;
    [SerializeField] public List< MeshRenderer> carMeshes;
    [SerializeField] public Color carColor;

    public bool IsAI => isAI;
    public int indexCurrentCheckPoint;
    public int CurrentPlace;
    public bool police  ;
  
    public void SetNextPoint()
    {
     

        indexCurrentCheckPoint++;
    }

    private void Awake()
    {
        // Fing the "Game Manager" game object and get gamemanager component. *WARNING*: Make sure the "Game Manager" object be in the scene. 

        CameraController = new CameraController();
    }
    void Start() {
        gameManager = GameManager.instance; 
        carMeshes.ForEach(m=>m.material.color = carColor);

        if (police == false)
        {
            brakeLightLeftMat = brakeLightLeft.GetComponent<Renderer>().material;
            brakeLightRightMat = brakeLightRight.GetComponent<Renderer>().material;

            brakeLightLeftMat.EnableKeyword("_EMISSION");
            brakeLightRightMat.EnableKeyword("_EMISSION");
        }

        carRigidbody = GetComponent<Rigidbody>();

        carRigidbody.centerOfMass = CenterOfMass.localPosition;

        engineAudioSource = GetComponent<AudioSource>();         
        engineAudioSource.Play();
        VirtualStart();
        SetNextPoint();

    }
    public virtual void VirtualStart()
    { }

    void Update()
    {
        if (isAI == false)
            PlayerController();

    }
    private void FixedUpdate()
    {
        if(isAI == true)
            ApplyAIControlling();



        //if(Vector3.Distance(transform.position,  CurrentCheckPoint.transform.position) <10)
        //{
        //    SetNextPoint();
        //}
    }

    public virtual void ApplyAIControlling()
    {
       
    }
    
    private void PlayerController()
    {
        horizontal = gameManager.HorizontalInput;                   // Get horizontal input value from the gamemanager object
        vertical = gameManager.VerticalInput;                       // Get vertical input value from the gamemanager object

        tireAngle = maxSteeringAngle * horizontal;                  // Calculate the front tires angles

        carSpeed = carRigidbody.velocity.magnitude;                 // Calculate the car speed in meter/second                 

        carSpeedConverted = Mathf.Round(carSpeed * 3.6f);             // Convert the car speed from meter/second to kilometer/hour
                                                                      // carSpeedRounded = Mathf.Round(carSpeed * 2.237f);         // Use this formula for mile/hour
        CameraController.UpdateOffset(horizontal);



        gameManager.speed = carSpeedConverted;                      // Pass the car speed to the gamemanager to show it on the speedometer

        Wheel_Collider_Front_Left.steerAngle = tireAngle;           // Set front wheel colliders steer angles
        Wheel_Collider_Front_Right.steerAngle = tireAngle;
        if(gameManager.startGameDelay > 0)
        {
            gameManager.hBrake = true;
        }
        else
        {
            gameManager.hBrake = false;
        }

        if (gameManager.hBrake)
        {
            // If handbrake button pressed run this part

            motorTorque = 0;
            Wheel_Collider_Rear_Left.brakeTorque = brakePower;
            Wheel_Collider_Rear_Right.brakeTorque = brakePower;

            brakeLightLeftMat.SetColor("_EmissionColor", brakeColor);
            brakeLightRightMat.SetColor("_EmissionColor", brakeColor);
        }
        else
        {
            // Else if vertical is pressed change brake light color and set brakeTorques to 0
            if (vertical >= 0)
            {
                brakeLightLeftMat.SetColor("_EmissionColor", Color.black);
                brakeLightRightMat.SetColor("_EmissionColor", Color.black);
            }
            else
            {
                brakeLightLeftMat.SetColor("_EmissionColor", brakeColor);
                brakeLightRightMat.SetColor("_EmissionColor", brakeColor);
            }

            Wheel_Collider_Front_Left.brakeTorque = 0;
            Wheel_Collider_Front_Right.brakeTorque = 0;
            Wheel_Collider_Rear_Left.brakeTorque = 0;
            Wheel_Collider_Rear_Right.brakeTorque = 0;

            // Check if car speed has exceeded from maxSpeed
            if (carSpeedConverted < maxSpeed && gameManager.startGameDelay == 0)
                motorTorque = maxMotorTorque * vertical;
            else
                motorTorque = 0;
        }

        // Set the motorTorques based on the carType
        if (carType == CarType.FrontWheelDrive)
        {
            Wheel_Collider_Front_Left.motorTorque = motorTorque;
            Wheel_Collider_Front_Right.motorTorque = motorTorque;
        }
        else if (carType == CarType.RearWheelDrive)
        {
            Wheel_Collider_Rear_Left.motorTorque = motorTorque;
            Wheel_Collider_Rear_Right.motorTorque = motorTorque;
        }
        else if (carType == CarType.FourWheelDrive)
        {
            Wheel_Collider_Front_Left.motorTorque = motorTorque;
            Wheel_Collider_Front_Right.motorTorque = motorTorque;
            Wheel_Collider_Rear_Left.motorTorque = motorTorque;
            Wheel_Collider_Rear_Right.motorTorque = motorTorque;
        }

        // Calculate the engine sound
        engineSound();

        // Set the wheel meshes to the correct position and rotation based on their wheel collider position and rotation
        ApplyTransformToWheels();

        // Calculate steering wheel rotation angle based on tires angle. I multiplied it by 2 so it looks nicer. You can change it to your desired value
        steeringWheelAngle = tireAngle * 2;

        // When users control the car with the mobile gyroscope the steering wheel shake badly so I used the lerp function to prevent it
        steeringWheelAngleLerp = Mathf.Lerp(steeringWheelAngleLerp, steeringWheelAngle, 0.1f);
        steeringWheel.localEulerAngles = new Vector3(steeringWheel.localEulerAngles.x, steeringWheel.localEulerAngles.y, -steeringWheelAngleLerp);
    }

    // Set the wheel meshes to the correct position and rotation based on their wheel collider position and rotation
    public virtual void ApplyTransformToWheels()
    {
        Vector3 position;
        Quaternion rotation;

        Wheel_Collider_Front_Left.GetWorldPose(out position, out rotation);
        Wheel_Mesh_Front_Left.transform.position = position;
        Wheel_Mesh_Front_Left.transform.rotation = rotation;

        Wheel_Collider_Front_Right.GetWorldPose(out position, out rotation);
        Wheel_Mesh_Front_Right.transform.position = position;
        Wheel_Mesh_Front_Right.transform.rotation = rotation;

        Wheel_Collider_Rear_Left.GetWorldPose(out position, out rotation);
        Wheel_Mesh_Rear_Left.transform.position = position;
        Wheel_Mesh_Rear_Left.transform.rotation = rotation;

        Wheel_Collider_Rear_Right.GetWorldPose(out position, out rotation);
        Wheel_Mesh_Rear_Right.transform.position = position;
        Wheel_Mesh_Rear_Right.transform.rotation = rotation;
    }

    // Calculate the engine sound based on the car speed by changing the audio pitch
    protected void engineSound()
    {
        float y = 0.4f;
        float z = 0.1f;

        engineAudioSource.volume = 0.8f;

        if (vertical == 0 && carSpeedConverted > 30)
        {
            engineAudioSource.volume = 0.5f;

            if (engineAudioPitch >= 0.35f)
            {
                engineAudioPitch -= Time.deltaTime * 0.1f;
            }
        }
        else if (carSpeedConverted <= 5)
        {
            engineAudioPitch = 0.15f;
        }
        else if (vertical != 0 && carSpeedConverted > 5 && carSpeedConverted <= 45)
        {

            float x = ((carSpeedConverted - 5) / 40) * y;
            engineAudioPitch = z + x;
        }
        else if (vertical != 0 && carSpeedConverted > 45 && carSpeedConverted <= 85)
        {
            float x = ((carSpeedConverted - 45) / 40) * y;
            engineAudioPitch = z + x + 0.2f;
        }
        else if (vertical != 0 && carSpeedConverted > 85 && carSpeedConverted <= 115)
        {
            float x = ((carSpeedConverted - 85) / 30) * y;
            engineAudioPitch = z + x + 0.3f;
        }
        else if (vertical != 0 && carSpeedConverted > 115 && carSpeedConverted <= 145)
        {
            float x = ((carSpeedConverted - 115) / 30) * y;
            engineAudioPitch = z + x + 0.4f;
        }
        else if (vertical != 0 && carSpeedConverted > 145 && carSpeedConverted <= 165)
        {
            float x = ((carSpeedConverted - 125) / 20) * y;
            engineAudioPitch = z + x + 0.6f;
        }
        else if (vertical != 0 && carSpeedConverted > 165)
        {
            engineAudioPitch = 1.5f ;
        }

        engineAudioSource.pitch = engineAudioPitch;
    }
   
    // Playing bump sound when colliding with an object
    private void OnCollisionEnter(Collision collision)
    {
        bumpAudioSource.Play();
    }
}

