using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPhysics : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody carRB;

    [SerializeField] Transform[] rayPoints;
    [SerializeField] LayerMask drivable;
    [SerializeField] Transform accelerationPoint;
    [SerializeField] GameObject[] tires = new GameObject[4];
    [SerializeField] GameObject[] frontTireParent = new GameObject[2];
    [SerializeField] TrailRenderer[] skidMarks = new TrailRenderer[2];
    [SerializeField] ParticleSystem[] skidSmokes = new ParticleSystem[2];
    [SerializeField] AudioSource engineSound, skidSound;

    [Header("Suspension Settings")]
    [SerializeField] float springStiffness;
    [SerializeField] float damperStiffness;
    [SerializeField] float restLength;
    [SerializeField] float springTravel;
    [SerializeField] float wheelRadius;

    int[] wheelIsGrounded = new int[4];
    bool isGrounded = false;

    [Header("Input")]
    float moveInput = 0;
    float steerInput = 0;
    float brakeInput = 0;

    [Header("Car Settings")]
    [SerializeField] float acceleration = 25f;
    [SerializeField] float deceleration = 10f;
    [SerializeField] float maxSpeed = 100f;
    [SerializeField] float steerStrength = 15f;
    [SerializeField] AnimationCurve turningCurve;
    [SerializeField] float dragCoefficient = 1f;
    [SerializeField] float brakingDeceleration = 100f;
    [SerializeField] float brakingDragCoefficient = 0.5f;


    Vector3 currentCarLocalVelocity = Vector3.zero;
    float carVelocityRatio = 0;

    [Header("Visuals")]
    [SerializeField] float tireRotSpeed = 3000f;
    [SerializeField] float maxSteeringAngle = 30f;
    [SerializeField] float minSideSkidVelocity = 10f;

    [Header("Audio")]
    [SerializeField] [Range(0,1)] float minPitch = 1f;
    [SerializeField] [Range(1, 5)] float maxPitch = 5f;


    void FixedUpdate()
    {
        Suspension();
        GroundCheck();
        CalculateCarVelocity();
        Movement();
        Visuals();
        EngineSound();
    }

#region Setting Functions


#endregion

#region Movement

    void Movement()
    {
        if (isGrounded)
        {
            Acceleration();
            Deceleration();
            Turn();
            SidewaysDrag();
        }
    }

    void Acceleration()
    {
        if (currentCarLocalVelocity.z < maxSpeed)
        {
            carRB.AddForceAtPosition(acceleration * moveInput * transform.forward, accelerationPoint.position, ForceMode.Acceleration);
        }
    }

    void Deceleration()
    {
        carRB.AddForceAtPosition((brakeInput > 0 ? brakingDeceleration : deceleration) * Mathf.Abs(carVelocityRatio) * -transform.forward, accelerationPoint.position, ForceMode.Acceleration);
    }

    void Turn()
    {
        carRB.AddTorque(steerStrength * steerInput * turningCurve.Evaluate(Mathf.Abs(carVelocityRatio)) * Mathf.Sign(carVelocityRatio) * transform.up, ForceMode.Acceleration);
    }

    void SidewaysDrag()
    {
        float currentSidewaysSpeed = currentCarLocalVelocity.x;

        float dragMagnitude = -currentSidewaysSpeed * (brakeInput > 0 ? brakingDragCoefficient : dragCoefficient);

        Vector3 dragForce = transform.right * dragMagnitude;

        carRB.AddForceAtPosition(dragForce, carRB.worldCenterOfMass, ForceMode.Acceleration);
    }

#endregion

#region Visuals

    void Visuals()
    {
        TireVisuals();
        VFX();
    }

    void TireVisuals()
    {

        float steeringAngle = maxSteeringAngle * steerInput;

        for (int i = 0; i < tires.Length; i++)
        {
            if (i < 2)
            {
                tires[i].transform.Rotate(Vector3.right, tireRotSpeed * carVelocityRatio * Time.deltaTime, Space.Self);
            
                frontTireParent[i].transform.localEulerAngles = new Vector3(frontTireParent[i].transform.localEulerAngles.x, steeringAngle, frontTireParent[i].transform.localEulerAngles.z);

            }
            else
            {
                tires[i].transform.Rotate(Vector3.right, tireRotSpeed * moveInput * Time.deltaTime, Space.Self);
            }

        }
    }

    void SetTirePosition(GameObject tire, Vector3 targetPosition)
    {
        tire.transform.position = targetPosition;
    }

    void VFX()
    {
        if (isGrounded && currentCarLocalVelocity.x > minSideSkidVelocity && carVelocityRatio > 0)
        {
            ToggleSkidMarks(true);
            ToggleSkidSmokes(true);
            ToggleSkidSound(true);
        }
        else
        {
            ToggleSkidMarks(false);
            ToggleSkidSmokes(false);
            ToggleSkidSound(false);
        }
    }

    void ToggleSkidMarks(bool toggle)
    {
        foreach (var skidMark in skidMarks)
        {
            skidMark.emitting = toggle;
        }
    }

    void ToggleSkidSmokes(bool toggle)
    {
        foreach (var smoke in skidSmokes)
        {
            if (toggle)
            {
                smoke.Play();
            }
            else
            {
                smoke.Stop();
            }
        }
    }

#endregion

#region Car Status Check
    void GroundCheck()
    {
        int tempGroundedWheels = 0;

        for (int i=0; i < wheelIsGrounded.Length; i++)
        {
            tempGroundedWheels += wheelIsGrounded[i];
        }

        if(tempGroundedWheels > 1)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void CalculateCarVelocity()
    {
        currentCarLocalVelocity = transform.InverseTransformDirection(carRB.velocity);
        carVelocityRatio = currentCarLocalVelocity.z / maxSpeed;
    }
#endregion

#region Input Handling

    public void SetAccelerationInput(float value)
    {
        moveInput = value;
    }

    public void SetTurnInput(float value)
    {
        steerInput = value;
    }

    public void SetBrakeInput(float value)
    {
        brakeInput = value;
    }

#endregion

#region Suspension Functions
    void Suspension()
    {
        for (int i=0; i < rayPoints.Length; i++)
        {
            RaycastHit hit;
            float maxLength = restLength + springTravel;

            if (Physics.Raycast(rayPoints[i].position, -rayPoints[i].up, out hit, maxLength + wheelRadius, drivable))
            {

                wheelIsGrounded[i] = 1;

                float currentSpringLength = hit.distance - wheelRadius;
                float springCompression = (restLength - currentSpringLength) / springTravel;

                float springVelocity = Vector3.Dot(carRB.GetPointVelocity(rayPoints[i].position), rayPoints[i].up);
                float dampForce = damperStiffness * springVelocity;

                float springForce = springStiffness * springCompression;

                float netForce = springForce - dampForce;

                carRB.AddForceAtPosition(netForce * rayPoints[i].up, rayPoints[i].position);

                //Visuals

                SetTirePosition(tires[i], hit.point + rayPoints[i].up * wheelRadius);

                Debug.DrawLine(rayPoints[i].position, hit.point, Color.red);
            }
            else
            {
                wheelIsGrounded[i] = 0;

                //Visuals

                SetTirePosition(tires[i], rayPoints[i].position - rayPoints[i].up * maxLength);

                Debug.DrawLine(rayPoints[i].position, rayPoints[i].position + (wheelRadius + maxLength) * rayPoints[i].up, Color.green);
            }
        }

    }
#endregion

#region Audio

        //FIXME: Move Audio and Visual FX to new carfx script

    void EngineSound()
    {
        engineSound.pitch = Mathf.Lerp(minPitch, maxPitch, Mathf.Abs(carVelocityRatio));

    }

    void ToggleSkidSound(bool toggle)
    {
        skidSound.mute = !toggle;
    }

#endregion

}
