using UnityEngine;

public class WheelController : MonoBehaviour {

    [SerializeField] private WheelAlignment[] steerableWheels;
    [SerializeField] private float BreakPower;
    [SerializeField] private float wheelRotateSpeed;
    [SerializeField] private float wheelSteeringAngle;
    [SerializeField] private float wheelAcceleration;
    [SerializeField] private float wheelMaxSpeed;

    [SerializeField] private bool playerInCar;
    [SerializeField] private float playerRange;
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private Rigidbody RB;

    private float Horizontal;
    private float Vertical;


    void Update ()
    {
        wheelControl();      
	}

    public void OnPlayerSitCar()
    {
        playerInCar = true;
    }

    void wheelControl()
    {
        if(!playerInCar)
            return;

        for (int i = 0; i < steerableWheels.Length; i++)
        {
            //Sets default steering angle
            steerableWheels[i].steeringAngle = Mathf.LerpAngle(steerableWheels[i].steeringAngle, 0, Time.deltaTime * wheelRotateSpeed);
            //Sets default motor speed
            steerableWheels[i].wheelCol.motorTorque = -Mathf.Lerp(steerableWheels[i].wheelCol.motorTorque, 0, Time.deltaTime * wheelAcceleration);



            //Motor controls

            Horizontal = Input.GetAxis("Horizontal");
            Vertical = Input.GetAxis("Vertical");

            if (Vertical < 0)
            {
                steerableWheels[i].wheelCol.motorTorque = -Mathf.Lerp(steerableWheels[i].wheelCol.motorTorque, wheelMaxSpeed, Time.deltaTime * wheelAcceleration);
            }

            if (Vertical > 0)
            {
                steerableWheels[i].wheelCol.motorTorque = Mathf.Lerp(steerableWheels[i].wheelCol.motorTorque, wheelMaxSpeed, Time.deltaTime * wheelAcceleration * BreakPower);
                RB.drag = 0.3f;
            }
            else
            {
                RB.drag = 0;
            }


            if (Horizontal < 0)
            {
                steerableWheels[i].steeringAngle = Mathf.LerpAngle(steerableWheels[i].steeringAngle, -wheelSteeringAngle, Time.deltaTime * wheelRotateSpeed);
            }

            if (Horizontal > 0)
            {
                steerableWheels[i].steeringAngle = Mathf.LerpAngle(steerableWheels[i].steeringAngle, wheelSteeringAngle, Time.deltaTime * wheelRotateSpeed);
            }
        }
    }

}
