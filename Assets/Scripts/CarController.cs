using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentbreakForce;
    private bool isBreaking;
    public int gear = 1;
    private bool shiftUp;
    private bool shiftDown;
    public int nitro = 500;
    private bool useNitro;
    private int nitroBoost = 1;
    private float force = 1f;
    private float realForce;
    public int player = 1;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheeTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    [SerializeField] private Rigidbody vehicle;

    private void FixedUpdate()
    {
        GetInput();
        Motor();
        Steering();
        UpdateWheels();
        UpdateGear();
        UpdateNitro();
    }

    private void GetInput()
    {
        if (player == 2)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            isBreaking = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Joystick2Button5);
            shiftUp = Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick2Button3);
            shiftDown = Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Joystick2Button2);
            useNitro = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Joystick2Button4);
        }
    }

    private void UpdateGear()
    {
        if (shiftUp == true)
            if (gear < 6)
                gear++;
        if (shiftDown == true)
            if (gear > 1)
                gear--;
        if (gear == 1)
        {
            realForce = motorForce / 10;
        }
        if (gear == 2)
        {
            realForce = motorForce / 10 + 600;
        }
        if (gear == 3)
        {
            realForce = motorForce / 10 + 600 * 2;
        }
        if (gear == 4)
        {
            realForce = motorForce / 10 + 600 * 3;
        }
        if (gear == 5)
        {
            realForce = motorForce / 10 + 600 * 4;
        }
        if (gear == 6)
        {
            realForce = motorForce / 10 + 600 * 5;
        }
    }

    private void UpdateNitro()
    {
        if (useNitro == true)
        {
            if (nitro > 0)
            {
                nitroBoost = 3;
                nitro--;
            }
            else
            {
                nitroBoost = 1;
            }
        }
        else
        {
            nitroBoost = 1;
        }
    }

    private void Motor()
    {
        if (verticalInput < 0)
            gear = 1;

        if (verticalInput != 0) {
            if (rearLeftWheelCollider.motorTorque < realForce)
            {
                rearLeftWheelCollider.motorTorque = verticalInput * (nitroBoost*gear*150 * Mathf.Log10(force));
                rearRightWheelCollider.motorTorque = verticalInput * (nitroBoost*gear*150 * Mathf.Log10(force));
                force += 1;
            }
        }

        //print(rearLeftWheelCollider.rpm);
        //print(rearLeftWheelCollider.motorTorque);
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
        if (verticalInput == 0 && rearLeftWheelCollider.rpm > 10)
        {
            /*if (vehicle.velocity.magnitude > 3)
            {
                rearLeftWheelCollider.motorTorque = -2000f;
                rearRightWheelCollider.motorTorque = -2000f;
            }*/
            rearLeftWheelCollider.motorTorque = 0f;
            rearRightWheelCollider.motorTorque = 0f;
        }
        if (gear == 1)
        {
            if (rearLeftWheelCollider.rpm > 500)
            {
                rearLeftWheelCollider.motorTorque = rearLeftWheelCollider.motorTorque * (-1f);
                rearRightWheelCollider.motorTorque = rearRightWheelCollider.motorTorque * (-1f);
            }
        }
        else if (gear == 2)
        {
            if (rearLeftWheelCollider.rpm > 1400)
            {
                rearLeftWheelCollider.motorTorque = rearLeftWheelCollider.motorTorque * (-1f);
                rearRightWheelCollider.motorTorque = rearRightWheelCollider.motorTorque * (-1f);
            }
            else if (rearLeftWheelCollider.rpm < 200)
            {
                rearLeftWheelCollider.motorTorque = 0f;
                rearRightWheelCollider.motorTorque = 0f;
            }
            else if (rearLeftWheelCollider.rpm < 1100)
            {
                rearLeftWheelCollider.motorTorque = verticalInput * (nitroBoost * gear * 90 * Mathf.Log10(force));
                rearRightWheelCollider.motorTorque = verticalInput * (nitroBoost * gear * 90 * Mathf.Log10(force));
            }
        }
        else if(gear == 3)
        {
            if (rearLeftWheelCollider.rpm > 2300)
            {
                rearLeftWheelCollider.motorTorque = rearLeftWheelCollider.motorTorque * (-1f);
                rearRightWheelCollider.motorTorque = rearRightWheelCollider.motorTorque * (-1f);
            }
            else if (rearLeftWheelCollider.rpm < 300)
            {
                rearLeftWheelCollider.motorTorque = 0f;
                rearRightWheelCollider.motorTorque = 0f;
            }
            else if (rearLeftWheelCollider.rpm < 2000)
            {
                rearLeftWheelCollider.motorTorque = verticalInput * (nitroBoost * gear * 90 * Mathf.Log10(force));
                rearRightWheelCollider.motorTorque = verticalInput * (nitroBoost * gear * 90 * Mathf.Log10(force));
            }
        }
        else if (gear == 4)
        {
            if (rearLeftWheelCollider.rpm > 3200)
            {
                rearLeftWheelCollider.motorTorque = rearLeftWheelCollider.motorTorque * (-1f);
                rearRightWheelCollider.motorTorque = rearRightWheelCollider.motorTorque * (-1f);
            }
            else if (rearLeftWheelCollider.rpm < 400)
            {
                rearLeftWheelCollider.motorTorque = 0f;
                rearRightWheelCollider.motorTorque = 0f;
            }
            else if (rearLeftWheelCollider.rpm < 2900)
            {
                rearLeftWheelCollider.motorTorque = verticalInput * (nitroBoost * gear * 90 * Mathf.Log10(force));
                rearRightWheelCollider.motorTorque = verticalInput * (nitroBoost * gear * 90 * Mathf.Log10(force));
            }
        }
        else if(gear == 5)
        {
            if (rearLeftWheelCollider.rpm > 4100)
            {
                rearLeftWheelCollider.motorTorque = rearLeftWheelCollider.motorTorque * (-1f);
                rearRightWheelCollider.motorTorque = rearRightWheelCollider.motorTorque * (-1f);
            }
            else if (rearLeftWheelCollider.rpm < 500)
            {
                rearLeftWheelCollider.motorTorque = 0f;
                rearRightWheelCollider.motorTorque = 0f;
            }
            else if (rearLeftWheelCollider.rpm < 3800)
            {
                rearLeftWheelCollider.motorTorque = verticalInput * (nitroBoost * gear * 90 * Mathf.Log10(force));
                rearRightWheelCollider.motorTorque = verticalInput * (nitroBoost * gear * 90 * Mathf.Log10(force));
            }
        }
        else if (gear == 6)
        {
            if (rearLeftWheelCollider.rpm > 5000)
            {
                rearLeftWheelCollider.motorTorque = rearLeftWheelCollider.motorTorque * (-1f);
                rearRightWheelCollider.motorTorque = rearRightWheelCollider.motorTorque * (-1f);
            }
            else if (rearLeftWheelCollider.rpm < 600)
            {
                rearLeftWheelCollider.motorTorque = 0f;
                rearRightWheelCollider.motorTorque = 0f;
            }
            else if (rearLeftWheelCollider.rpm < 4700)
            {
                rearLeftWheelCollider.motorTorque = verticalInput * (nitroBoost * gear * 90 * Mathf.Log10(force));
                rearRightWheelCollider.motorTorque = verticalInput * (nitroBoost * gear * 90 * Mathf.Log10(force));
            }
        }
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void Steering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheeTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
