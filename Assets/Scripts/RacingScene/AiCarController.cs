using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AiCarController : CarControllerPro
{
    [SerializeField] private AiPoints points;
 
    [SerializeField] private float distanceForAIAction;
    [SerializeField] private float distanceSpeedReductionBeforePoint;
    public bool isCarLog;
    private float timerObstacle = 5f;
    private float offsetTurn;
    public bool isObstacle;
    private float constDrag;
   public bool isBrake;
    public bool isMobileTarget = false;
    public CarControllerPro mobileTarget;
    public override void VirtualStart()
    {
        base.VirtualStart();
        offsetTurn = Random.Range(-0.5f, 0.5f);
        constDrag = carRigidbody.drag;
    }
    public override void ApplyAIControlling()
    {
        base.ApplyAIControlling();
        GoAroundObstacle(isMobileTarget,mobileTarget.transform);
        GoToTarget();


        //horizontal = gameManager.HorizontalInput;                   // Get horizontal input value from the gamemanager object
        //vertical = gameManager.VerticalInput;                       // Get vertical input value from the gamemanager object

        tireAngle = maxSteeringAngle * horizontal;                  // Calculate the front tires angles

        carSpeed = carRigidbody.velocity.magnitude;                 // Calculate the car speed in meter/second                 

        carSpeedConverted = Mathf.Round(carSpeed * 3.6f);             // Convert the car speed from meter/second to kilometer/hour
                                                                      // carSpeedRounded = Mathf.Round(carSpeed * 2.237f);         // Use this formula for mile/hour

        gameManager.speed = carSpeedConverted;                      // Pass the car speed to the gamemanager to show it on the speedometer

        Wheel_Collider_Front_Left.steerAngle = tireAngle;           // Set front wheel colliders steer angles
        Wheel_Collider_Front_Right.steerAngle = tireAngle;


        if(gameManager.startGameDelay > 0)
        {
           isBrake = true;
           
        }
        else
        {
            isBrake = false;
        }

        if (isBrake == true)
        {
            // If handbrake button pressed run this part

            motorTorque = 0;
            Wheel_Collider_Rear_Left.brakeTorque = brakePower;
            Wheel_Collider_Rear_Right.brakeTorque = brakePower;

            brakeLightLeftMat.SetColor("_EmissionColor", brakeColor);
            brakeLightRightMat.SetColor("_EmissionColor", brakeColor);
        }
        if (!isBrake )
        {
            // Else if vertical is pressed change brake light color and set brakeTorques to 0
            if (vertical >= 0 && constDrag == carRigidbody.drag)
            {
                if (brakeLightLeftMat != false)
                {
                    brakeLightLeftMat.SetColor("_EmissionColor", Color.black);
                    brakeLightRightMat.SetColor("_EmissionColor", Color.black);
                }
            }
            else
            {
                if (brakeLightLeftMat != false)
                {
                    brakeLightLeftMat.SetColor("_EmissionColor", brakeColor);
                    brakeLightRightMat.SetColor("_EmissionColor", brakeColor);
                }
            }

            Wheel_Collider_Front_Left.brakeTorque = 0;
            Wheel_Collider_Front_Right.brakeTorque = 0;
            Wheel_Collider_Rear_Left.brakeTorque = 0;
            Wheel_Collider_Rear_Right.brakeTorque = 0;

            // Check if car speed has exceeded from maxSpeed
            if (carSpeedConverted < maxSpeed  &&  gameManager.startGameDelay == 0)
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
    public override void ApplyTransformToWheels()
    {
        Vector3 position;
        Quaternion rotation;

        Wheel_Collider_Front_Left.GetWorldPose(out position, out rotation);
        Wheel_Mesh_Front_Left.transform.position = position;
        //Wheel_Mesh_Front_Left.transform.rotation  = rotation;
        Wheel_Mesh_Front_Left.transform.Rotate(10 , 0, 0);
        Wheel_Collider_Front_Right.GetWorldPose(out position, out rotation);
        Wheel_Mesh_Front_Right.transform.position = position;
        //Wheel_Mesh_Front_Right.transform.rotation = rotation;
        Wheel_Mesh_Front_Right.transform.Rotate(10, 0, 0);


        Wheel_Collider_Rear_Left.GetWorldPose(out position, out rotation);
        Wheel_Mesh_Rear_Left.transform.position = position;
        Wheel_Mesh_Rear_Left.transform.rotation = rotation;

        Wheel_Collider_Rear_Right.GetWorldPose(out position, out rotation);
        Wheel_Mesh_Rear_Right.transform.position = position;
        Wheel_Mesh_Rear_Right.transform.rotation = rotation;
    }

    private void GoToTarget()
    {


        if (GameManager.instance.policeScene == false)
        {
            if (points.points.Count == 0 || isObstacle == true) return;
            var target = points.points.FirstOrDefault();
            SetTarget(target);
        }
        else
        {
            SetMobileTarget(mobileTarget);
        }
    }

    private void SetMobileTarget(CarControllerPro target)
    {
        Vector3 targetDirection = (target.transform.position - transform.position);
        float distanceToTarget = targetDirection.magnitude;


        var turn = GetRotationDirection(transform, target.transform, true) + offsetTurn;
        horizontal = turn;
        if (distanceToTarget < distanceSpeedReductionBeforePoint)
        {
            vertical = 1f;

            if (brakeLightLeftMat != false)
            {
                brakeLightLeftMat.SetColor("_EmissionColor", brakeColor);
                brakeLightRightMat.SetColor("_EmissionColor", brakeColor);
            }


        }
        else
        {
            vertical = 3f;
            carRigidbody.drag = constDrag;
            if (brakeLightLeftMat != false)
            {
                brakeLightLeftMat.SetColor("_EmissionColor", Color.black);
                brakeLightRightMat.SetColor("_EmissionColor", Color.black);
            }

        }
    }

    private void SetTarget(Point target)
    {
        Vector3 targetDirection = (target.transform.position - transform.position);
        float distanceToTarget = targetDirection.magnitude;


        var turn = GetRotationDirection(transform, target.transform, true) + offsetTurn;
        horizontal = turn;
        RemovePoint(target, distanceToTarget);
        if (distanceToTarget < distanceSpeedReductionBeforePoint)
        {
            vertical = 1f;

            carRigidbody.drag = target.Drag;
            if (brakeLightLeftMat != false)
            {
                brakeLightLeftMat.SetColor("_EmissionColor", brakeColor);
                brakeLightRightMat.SetColor("_EmissionColor", brakeColor);
            }


        }
        else
        {
            vertical = 3f;
            carRigidbody.drag = constDrag;
            if (brakeLightLeftMat != false)
            {
                brakeLightLeftMat.SetColor("_EmissionColor", Color.black);
                brakeLightRightMat.SetColor("_EmissionColor", Color.black);
            }

        }
    }

    private void CarDebug(string log)
    {
        if (isCarLog) Debug.Log(log);
    }

    private void RemovePoint(Point target, float distanceToTarget)
    {
        if (distanceToTarget < distanceForAIAction)
        {
            points.points.Remove(target);
            //Destroy(target.gameObject);
        }
    }


    private void GoAroundObstacle(bool isMobileTarget, Transform mobileTarget = null)
    {
        GameObject obstacle = GetNearObstacle();
        if (obstacle == null)
        {
            isObstacle = false;
            return;
        }
        isObstacle = true;
        if (isMobileTarget == false)
        {
            var target = points.points.FirstOrDefault();
            if (target)
            {
                Vector3 targetDirection = target.transform.position - transform.position;
                float distanceToTarget = targetDirection.magnitude;
                RemovePoint(target, distanceToTarget);

                if (timerObstacle > 0 && carSpeed < 1f)
                {
                    timerObstacle -= 1 * Time.fixedDeltaTime;
                    vertical = -1f;
                    horizontal = GetRotationDirection(transform, target.transform, false);
                }
                //if (vertical == -1 && carSpeed < 0.1f)
                //{
                //    isObstacle = false;
                //}
                if (carSpeed > 3f)
                {
                    isObstacle = false;
                    timerObstacle = 3f;
                }
            }
        }
        else if(mobileTarget != null)
        {
            Vector3 targetDirection = mobileTarget.transform.position - transform.position;
            float distanceToTarget = targetDirection.magnitude;
         

            if (timerObstacle > 0 && carSpeed < 1f)
            {
                timerObstacle -= 1 * Time.fixedDeltaTime;
                vertical = -1f;
                horizontal = GetRotationDirection(transform, mobileTarget.transform, false);
            }
         
            if (carSpeed > 3f)
            {
                isObstacle = false;
                timerObstacle = 3f;
            }
        }
    }
    private static float GetRotationDirection(Transform myTransform, Transform target, bool toTarget)
    {
        Vector3 targetDirection = (target.position - myTransform.position).normalized;
        float angle = Vector3.SignedAngle(myTransform.forward, targetDirection, Vector3.up);
       
        float rotationDirection = Mathf.Sign(angle);
        if (toTarget)
        {
            return rotationDirection;
        }
        else
        {
            return -rotationDirection;
        }
    }

    private GameObject GetNearObstacle()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 4f);
        if (colliders.Length > 0)
        {
            var listColliders = colliders.ToList();
            List<Collider> obstacles = listColliders.Where(go => go.CompareTag("Obstacle") == true /*|| go.CompareTag("PlayerCar") == true*/).ToList();
       
            if (obstacles.Count > 0)
            {
                float minDistance = 100000;
                Collider nearObstacle = null;

                foreach (var obstacle in obstacles)
                {
                    float distance = Vector3.Distance(transform.position, obstacle.transform.position);
                    if(distance < minDistance)
                    {
                        minDistance = distance;
                        nearObstacle = obstacle;
                    }
                }
                return nearObstacle.gameObject;
            }
        }
        return null;
    }
}
