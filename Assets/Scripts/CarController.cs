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
    private float force = 1f;
    private float realForce;
    public int player = 1;
    // 0.5f - 2f
    private float minPitch = 0.5f;
    private float maxPitch = 2f;
    private float slipLat;
    private float slipLong;

    AudioSource[] sounds;
    AudioSource audioSource;
    AudioSource audioSource2;
    public bool skidding = false;
    public float falseTime = 0;

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

    public Transform centerOfMass;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = centerOfMass.localPosition;

        sounds = GetComponents<AudioSource>();
        audioSource = sounds[0];
        audioSource2 = sounds[1];
    }

    private void FixedUpdate()
    {
        GetInput();
        Motor();
        Steering();
        UpdateWheels();
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
        }
    }

    private void UpdateSmoke()
    {
        rearLeftWheelCollider.GetGroundHit(out WheelHit rearLeftWheelData);
        if (rearLeftWheelData.sidewaysSlip > 0.65)
        {
            rearLeftSmoke.GetComponent<ParticleSystem>().Play();
            skidding = true;
        }
        else if (rearLeftWheelData.sidewaysSlip < -0.65)
        {
            rearLeftSmoke.GetComponent<ParticleSystem>().Play();
            skidding = true;
        }
        else if (rearLeftWheelData.forwardSlip > 0.65)
        {
            rearLeftSmoke.GetComponent<ParticleSystem>().Play();
            skidding = true;
        }
        else if (rearLeftWheelData.forwardSlip < -0.65)
        {
            rearLeftSmoke.GetComponent<ParticleSystem>().Play();
            skidding = true;
        }
        else
        {
            rearLeftSmoke.GetComponent<ParticleSystem>().Stop();
            skidding = false;
        }

        rearRightWheelCollider.GetGroundHit(out WheelHit rearRightWheelData);
        if (rearRightWheelData.sidewaysSlip > 0.65)
        {
            rearRightSmoke.GetComponent<ParticleSystem>().Play();
            skidding = true;
        }
        else if (rearRightWheelData.sidewaysSlip < -0.65)
        {
            rearRightSmoke.GetComponent<ParticleSystem>().Play();
            skidding = true;
        }
        else if (rearRightWheelData.forwardSlip > 0.65)
        {
            rearRightSmoke.GetComponent<ParticleSystem>().Play();
            skidding = true;
        }
        else if (rearRightWheelData.forwardSlip < -0.65)
        {
            rearRightSmoke.GetComponent<ParticleSystem>().Play();
            skidding = true;
        }
        else
        {
            rearRightSmoke.GetComponent<ParticleSystem>().Stop();
            skidding = false;
        }

        frontLeftWheelCollider.GetGroundHit(out WheelHit frontLeftWheelData);
        if (frontLeftWheelData.sidewaysSlip > 0.65)
        {
            frontLeftSmoke.GetComponent<ParticleSystem>().Play();
            skidding = true;
        }
        else if (frontLeftWheelData.sidewaysSlip < -0.65)
        {
            frontLeftSmoke.GetComponent<ParticleSystem>().Play();
            skidding = true;
        }
        else if (frontLeftWheelData.forwardSlip > 0.65)
        {
            frontLeftSmoke.GetComponent<ParticleSystem>().Play();
            skidding = true;
        }
        else if (frontLeftWheelData.forwardSlip < -0.65)
        {
            frontLeftSmoke.GetComponent<ParticleSystem>().Play();
            skidding = true;
        }
        else
        {
            frontLeftSmoke.GetComponent<ParticleSystem>().Stop();
            skidding = false;
        }

        frontRightWheelCollider.GetGroundHit(out WheelHit frontRightWheelData);
        if (frontRightWheelData.sidewaysSlip > 0.65)
        {
            frontRightSmoke.GetComponent<ParticleSystem>().Play();
            skidding = true;
        }
        else if (frontRightWheelData.sidewaysSlip < -0.65)
        {
            frontRightSmoke.GetComponent<ParticleSystem>().Play();
            skidding = true;
        }
        else if (frontRightWheelData.forwardSlip > 0.65)
        {
            frontRightSmoke.GetComponent<ParticleSystem>().Play();
            skidding = true;
        }
        else if (frontRightWheelData.forwardSlip < -0.65)
        {
            frontRightSmoke.GetComponent<ParticleSystem>().Play();
            skidding = true;
        }
        else
        {
            frontRightSmoke.GetComponent<ParticleSystem>().Stop();
            skidding = false;
        }
    }

    private void UpdateAudio()
    {
        //audioSource.pitch = minPitch + 1.5f * (rearLeftWheelCollider.rpm/5000);
        if (gear == 1)
        {
            audioSource.pitch = minPitch + 1.5f * (rearLeftWheelCollider.rpm / 250);
        }
        else if (rearLeftWheelCollider.rpm < 5)
        {
            audioSource.pitch = minPitch;
        }
        else if (gear == 2)
        {
            audioSource.pitch = minPitch + 1.5f * ((rearLeftWheelCollider.rpm - 100) / 1200);
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

        if (audioSource.pitch > maxPitch)
            audioSource.pitch = maxPitch;

        if (skidding)
        {
            if (!audioSource2.isPlaying)
                audioSource2.Play();
        }
        else
        {
            if (falseTime == 0)
                falseTime = Time.time;
            if ((Time.time - falseTime) >= 0.5f)
            {
                audioSource2.Stop();
                falseTime = 0;
                audioSource2.volume = 1;
            }
        }
    }

    private void Motor()
    {
        /*if (verticalInput < 0 && (rearLeftWheelCollider.rpm > 100 || rearRightWheelCollider.rpm > 100))
            rearLeftWheelCollider.motorTorque = -100;
            rearRightWheelCollider.motorTorque = -100;*/

        if (verticalInput > 0) {
            /*if (rearLeftWheelCollider.motorTorque < realForce)
            {
                rearLeftWheelCollider.motorTorque = verticalInput * (gear*150 * Mathf.Log10(force));
                rearRightWheelCollider.motorTorque = verticalInput * (gear*150 * Mathf.Log10(force));
                force += 1;
            }*/
            rearLeftWheelCollider.motorTorque = verticalInput * motorForce;
            rearRightWheelCollider.motorTorque = verticalInput * motorForce;
        }
        else if (verticalInput < 0)
        {
            rearLeftWheelCollider.motorTorque = verticalInput * motorForce;
            rearRightWheelCollider.motorTorque = verticalInput * motorForce;
            if (rearLeftWheelCollider.rpm < -500 || rearRightWheelCollider.rpm < -500)
            {
                rearLeftWheelCollider.motorTorque = rearLeftWheelCollider.motorTorque * (-1f);
                rearRightWheelCollider.motorTorque = rearRightWheelCollider.motorTorque * (-1f);
            }
        }
        else if (verticalInput < 0 && rearLeftWheelCollider.rpm > 100)
        {
            rearLeftWheelCollider.motorTorque = rearLeftWheelCollider.motorTorque * (-1f);
            rearRightWheelCollider.motorTorque = rearRightWheelCollider.motorTorque * (-1f);
        }
        else if (verticalInput == 0 && rearLeftWheelCollider.rpm > 10)
        {
            rearLeftWheelCollider.motorTorque = 0f;
            rearRightWheelCollider.motorTorque = 0f;
        }
        else if (verticalInput == 0 && rearLeftWheelCollider.rpm < 10)
        {
            isBreaking = true;
        }

        //print(rearLeftWheelCollider.rpm);
        //print(rearLeftWheelCollider.motorTorque);
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();

        // rpm 0-250
        if (gear == 1)
        {
            if (rearLeftWheelCollider.rpm > 250)
            {
                gear = 2;
                //rearLeftWheelCollider.motorTorque = rearLeftWheelCollider.motorTorque * (-1f);
                //rearRightWheelCollider.motorTorque = rearRightWheelCollider.motorTorque * (-1f);
            }
        }
        // rpm 100-1400
        else if (gear == 2)
        {
            if (rearLeftWheelCollider.rpm > 500)
            {
                gear = 3;
                //rearLeftWheelCollider.motorTorque = rearLeftWheelCollider.motorTorque * (-1f);
                //rearRightWheelCollider.motorTorque = rearRightWheelCollider.motorTorque * (-1f);
            }
            else if (rearLeftWheelCollider.rpm < 230)
            {
                gear = 1;
                //rearLeftWheelCollider.motorTorque = 0f;
                //rearRightWheelCollider.motorTorque = 0f;
            }
            else if (rearLeftWheelCollider.rpm < 1100)
            {
                //rearLeftWheelCollider.motorTorque = verticalInput * (gear * 90 * Mathf.Log10(force));
                //rearRightWheelCollider.motorTorque = verticalInput * (gear * 90 * Mathf.Log10(force));
            }
        }
        // rpm 300-2300
        else if (gear == 3)
        {
            if (rearLeftWheelCollider.rpm > 750)
            {
                gear = 4;
                //rearLeftWheelCollider.motorTorque = rearLeftWheelCollider.motorTorque * (-1f);
                //rearRightWheelCollider.motorTorque = rearRightWheelCollider.motorTorque * (-1f);
            }
            else if (rearLeftWheelCollider.rpm < 480)
            {
                gear = 2;
                //rearLeftWheelCollider.motorTorque = 0f;
                //rearRightWheelCollider.motorTorque = 0f;
            }
            else if (rearLeftWheelCollider.rpm < 2000)
            {
                //rearLeftWheelCollider.motorTorque = verticalInput * (gear * 90 * Mathf.Log10(force));
                //rearRightWheelCollider.motorTorque = verticalInput * (gear * 90 * Mathf.Log10(force));
            }
        }
        // rpm 400-3200
        else if (gear == 4)
        {
            if (rearLeftWheelCollider.rpm > 1000)
            {
                gear = 5;
                //rearLeftWheelCollider.motorTorque = rearLeftWheelCollider.motorTorque * (-1f);
                //rearRightWheelCollider.motorTorque = rearRightWheelCollider.motorTorque * (-1f);
            }
            else if (rearLeftWheelCollider.rpm < 730)
            {
                gear = 3;
                //rearLeftWheelCollider.motorTorque = 0f;
                //rearRightWheelCollider.motorTorque = 0f;
            }
            else if (rearLeftWheelCollider.rpm < 2900)
            {
                //rearLeftWheelCollider.motorTorque = verticalInput * (gear * 90 * Mathf.Log10(force));
                //rearRightWheelCollider.motorTorque = verticalInput * (gear * 90 * Mathf.Log10(force));
            }
        }
        // rpm 500-4100
        else if (gear == 5)
        {
            if (rearLeftWheelCollider.rpm > 1250)
            {
                gear = 6;
                //rearLeftWheelCollider.motorTorque = rearLeftWheelCollider.motorTorque * (-1f);
                //rearRightWheelCollider.motorTorque = rearRightWheelCollider.motorTorque * (-1f);
            }
            else if (rearLeftWheelCollider.rpm < 980)
            {
                gear = 4;
                //rearLeftWheelCollider.motorTorque = 0f;
                //rearRightWheelCollider.motorTorque = 0f;
            }
            else if (rearLeftWheelCollider.rpm < 3800)
            {
                //rearLeftWheelCollider.motorTorque = verticalInput * (gear * 90 * Mathf.Log10(force));
                //rearRightWheelCollider.motorTorque = verticalInput * (gear * 90 * Mathf.Log10(force));
            }
        }
        // rpm 600-5000
        else if (gear == 6)
        {
            if (rearLeftWheelCollider.rpm > 1500 || rearRightWheelCollider.rpm > 1500)
            {
                //rearLeftWheelCollider.motorTorque = rearLeftWheelCollider.motorTorque * (-1f);
                //rearRightWheelCollider.motorTorque = rearRightWheelCollider.motorTorque * (-1f);
                rearLeftWheelCollider.motorTorque = rearLeftWheelCollider.motorTorque * (-1f);
                rearRightWheelCollider.motorTorque = rearRightWheelCollider.motorTorque * (-1f);
            }
            else if (rearLeftWheelCollider.rpm < 1230)
            {
                gear = 5;
                //rearLeftWheelCollider.motorTorque = 0f;
                //rearRightWheelCollider.motorTorque = 0f;
            }
            else if (rearLeftWheelCollider.rpm < 4700)
            {
                //rearLeftWheelCollider.motorTorque = verticalInput * (gear * 90 * Mathf.Log10(force));
                //rearRightWheelCollider.motorTorque = verticalInput * (gear * 90 * Mathf.Log10(force));
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
