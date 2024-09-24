using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{

    [Header("General Setup Settings")]
    float moveSpeed = 20f;
    [Tooltip("Clamped ship movement along Z-axis")] [SerializeField] float zRange = 30f;
    [Tooltip("Clamped ship movement along Y-axis")] [SerializeField] float yRange = 15f;

    [Header("Pitch, Yaw, Roll tuning")]
    [Tooltip("Ship pitch by position")] [SerializeField] float positionPitchFactor = 3f;
    [Tooltip("Ship pitch by key input")] [SerializeField] float controlPitchFactor = -10f;
    [Tooltip("Ship yaw by position")] [SerializeField] float positionYawFactor = -2f;
    [Tooltip("Ship roll by key input")] [SerializeField] float controlRollFactor = -12f;

    float horzThrow;
    float vertThrow;

    [Header("Weapon Systems")]
    [SerializeField] GameObject[] lazers;

    AudioSource atkSFX;

    // Start is called before the first frame update
    void Start()
    {
        atkSFX = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Rotation();   
        Firing();
    }

    void Movement()
    {
        horzThrow = Input.GetAxis("Horizontal");
        vertThrow = Input.GetAxis("Vertical");

        float zOffset = moveSpeed * horzThrow * Time.deltaTime;
        float rawZPos = transform.localPosition.z - zOffset;
        float clampedZPos = Mathf.Clamp(rawZPos, -zRange, zRange);

        float yOffset = moveSpeed * vertThrow * Time.deltaTime;
        float rawYPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3 (transform.localPosition.x, clampedYPos, clampedZPos);
    }

    void Rotation()
    {
        float pitchDueToPosition = transform.localPosition.y * -positionPitchFactor;
        float pitchDueToControlThrow = vertThrow * controlPitchFactor;

        float pitch = pitchDueToPosition + pitchDueToControlThrow;
        float yaw = transform.localPosition.z * positionYawFactor;
        float roll = horzThrow * controlRollFactor;

        transform.localRotation = Quaternion.Euler(pitch, yaw + 90, roll);
    }

    void Firing()
    {
        
        if (Input.GetButton("Fire1"))
        {
            SetLazers(true);            
        }
        else
        {
            SetLazers(false);
        }
        
    }

    private void SetLazers(bool lazerState)
    {
        foreach (GameObject lazer in lazers)
        {            
            var emissionModule = lazer.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = lazerState;
        }
    }
}