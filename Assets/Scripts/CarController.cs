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
    // 0.5f - 2f
    private float minPitch = 0.5f;
    private float maxPitch = 2f;
    private float slipLat;
    private float slipLong;

    AudioSource audioSource;

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

    [SerializeField] private GameObject frontLeftSmoke;
    [SerializeField] private GameObject frontRightSmoke;
    [SerializeField] private GameObject rearLeftSmoke;
    [SerializeField] private GameObject rearRightSmoke;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        GetInput();
        Motor();
        Steering();
        UpdateWheels();
        UpdateGear();
        UpdateNitro();
        audioSource.pitch = minPitch;
        UpdateAudio();
        UpdateSmoke();
        //rearLeftWheelCollider.GetGroundHit(out WheelHit wheelData);
        //slipLat = wheelData.sidewaysSlip;
        //slipLong = wheelData.forwardSlip;
        //print(slipLat);
        //print(slipLong);
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

    private void UpdateSmoke()
    {
        rearLeftWheelCollider.GetGroundHit(out WheelHit rearLeftWheelData);
        if (rearLeftWheelData.sidewaysSlip > 0.5)
        {
            rearLeftSmoke.GetComponent<ParticleSystem>().Play();
        }
        else if (rearLeftWheelData.sidewaysSlip < -0.5)
        {
            rearLeftSmoke.GetComponent<ParticleSystem>().Play();
        }
        else if (rearLeftWheelData.forwardSlip > 0.5)
        {
            rearLeftSmoke.GetComponent<ParticleSystem>().Play();
        }
        else if (rearLeftWheelData.forwardSlip < -0.5)
        {
            rearLeftSmoke.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            rearLeftSmoke.GetComponent<ParticleSystem>().Stop();
        }

        rearRightWheelCollider.GetGroundHit(out WheelHit rearRightWheelData);
        if (rearRightWheelData.sidewaysSlip > 0.5)
        {
            rearRightSmoke.GetComponent<ParticleSystem>().Play();
        }
        else if (rearRightWheelData.sidewaysSlip < -0.5)
        {
            rearRightSmoke.GetComponent<ParticleSystem>().Play();
        }
        else if (rearRightWheelData.forwardSlip > 0.5)
        {
            rearRightSmoke.GetComponent<ParticleSystem>().Play();
        }
        else if (rearRightWheelData.forwardSlip < -0.5)
        {
            rearRightSmoke.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            rearRightSmoke.GetComponent<ParticleSystem>().Stop();
        }

        frontLeftWheelCollider.GetGroundHit(out WheelHit frontLeftWheelData);
        if (frontLeftWheelData.sidewaysSlip > 0.5)
        {
            frontLeftSmoke.GetComponent<ParticleSystem>().Play();
        }
        else if (frontLeftWheelData.sidewaysSlip < -0.5)
        {
            frontLeftSmoke.GetComponent<ParticleSystem>().Play();
        }
        else if (frontLeftWheelData.forwardSlip > 0.5)
        {
            frontLeftSmoke.GetComponent<ParticleSystem>().Play();
        }
        else if (frontLeftWheelData.forwardSlip < -0.5)
        {
            frontLeftSmoke.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            frontLeftSmoke.GetComponent<ParticleSystem>().Stop();
        }

        frontRightWheelCollider.GetGroundHit(out WheelHit frontRightWheelData);
        if (frontRightWheelData.sidewaysSlip > 0.5)
        {
            frontRightSmoke.GetComponent<ParticleSystem>().Play();
        }
        else if (frontRightWheelData.sidewaysSlip < -0.5)
        {
            frontRightSmoke.GetComponent<ParticleSystem>().Play();
        }
        else if (frontRightWheelData.forwardSlip > 0.5)
        {
            frontRightSmoke.GetComponent<ParticleSystem>().Play();
        }
        else if (frontRightWheelData.forwardSlip < -0.5)
        {
            frontRightSmoke.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            frontRightSmoke.GetComponent<ParticleSystem>().Stop();
        }
    }

    private void UpdateAudio()
    {
        //audioSource.pitch = minPitch + 1.5f * (rearLeftWheelCollider.rpm/5000);
        if (gear == 1)
        {
            audioSource.pitch = minPitch + 1.5f * (rearLeftWheelCollider.rpm / 500);
        }
        else if (rearLeftWheelCollider.rpm < 5)
        {
            audioSource.pitch = minPitch;
        }
        else if (gear == 2)
        {
            audioSource.pitch = minPitch + 1.5f * ((rearLeftWheelCollider.rpm - 200) / 1200);
        }
        else if (gear == 3)
        {
            audioSource.pitch = minPitch + 1.5f * ((rearLeftWheelCollider.rpm - 300) / 2000);
        }
        else if (gear == 4)
        {
            audioSource.pitch = minPitch + 1.5f * ((rearLeftWheelCollider.rpm - 400) / 2800);
        }
        else if (gear == 5)
        {
            audioSource.pitch = minPitch + 1.5f * ((rearLeftWheelCollider.rpm - 500) / 3600);
        }
        else if (gear == 6)
        {
            audioSource.pitch = minPitch + 1.5f * ((rearLeftWheelCollider.rpm - 600) / 4400);
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
            audioSource.pitch = minPitch + 1.5f * (rearLeftWheelCollider.rpm / 500);
        }
        if (gear == 2)
        {
            realForce = motorForce / 10 + 600;
            audioSource.pitch = minPitch + 1.5f * ((rearLeftWheelCollider.rpm - 200) / 1200);
        }
        if (gear == 3)
        {
            realForce = motorForce / 10 + 600 * 2;
            audioSource.pitch = minPitch + 1.5f * ((rearLeftWheelCollider.rpm - 300) / 2000);
        }
        if (gear == 4)
        {
            realForce = motorForce / 10 + 600 * 3;
            audioSource.pitch = minPitch + 1.5f * ((rearLeftWheelCollider.rpm - 400) / 2800);
        }
        if (gear == 5)
        {
            realForce = motorForce / 10 + 600 * 4;
            audioSource.pitch = minPitch + 1.5f * ((rearLeftWheelCollider.rpm - 500) / 3600);
        }
        if (gear == 6)
        {
            realForce = motorForce / 10 + 600 * 5;
            audioSource.pitch = minPitch + 1.5f * ((rearLeftWheelCollider.rpm - 600) / 4400);
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
        // rpm 0-500
        if (gear == 1)
        {
            if (rearLeftWheelCollider.rpm > 500)
            {
                rearLeftWheelCollider.motorTorque = rearLeftWheelCollider.motorTorque * (-1f);
                rearRightWheelCollider.motorTorque = rearRightWheelCollider.motorTorque * (-1f);
            }
        }
        // rpm 200-1400
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
        // rpm 300-2300
        else if (gear == 3)
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
        // rpm 400-3200
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
        // rpm 500-4100
        else if (gear == 5)
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
        // rpm 600-5000
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
