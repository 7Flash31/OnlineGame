using UnityEngine;

public class CarController : MonoBehaviour
{
    public Transform frontLeft;
    public Transform frontRight;
    public Transform backLeft;
    public Transform backRight;

    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider backLeftWheel;
    public WheelCollider backRightWheel;

    public float maxMotorTorque;
    public float maxSteeringAngle;


    public void FixedUpdate()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        frontLeftWheel.steerAngle = steering;
        frontRightWheel.steerAngle = steering;
        frontLeft.rotation = new Quaternion(frontLeft.rotation.x, steering, frontLeft.rotation.z, frontLeft.rotation.w);
        frontRight.rotation = new Quaternion(frontRight.rotation.x, steering, frontRight.rotation.z, frontRight.rotation.w);

        backLeftWheel.motorTorque = motor;
        backRightWheel.motorTorque = motor;
    }
}
