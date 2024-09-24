using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    
    [SerializeField] float mainThrust = 1000.0f;
    [SerializeField] float rotThrust = 25.0f;

    AudioSource thrustSound;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem thrusterParticles;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        thrustSound = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    private void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!thrustSound.isPlaying)
        {
            thrustSound.PlayOneShot(mainEngine);
            thrusterParticles.Play();
        }
    }

    private void StopThrusting()
    {
        thrustSound.Stop();
        thrusterParticles.Stop();
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            SetRotation(rotThrust);
        }

        else if (Input.GetKey(KeyCode.D))
        {
            SetRotation(-rotThrust);
        }
    }

    void SetRotation(float currentRot)
    {
        rb.freezeRotation = true; 
        transform.Rotate(Vector3.forward * currentRot * Time.deltaTime);
        rb.freezeRotation = false;
    }

}
