using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    float reloadTime = 2.0f;
    bool isTransitioning = false;
    bool canCollide = true;
    Movement MovementScript;

    AudioSource colisAudio;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip success;

    
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem explodeParticles;


    void Start()
    {
        MovementScript = GetComponent<Movement>();
        colisAudio = GetComponent<AudioSource>();
        
    }

    void Update()
    {
        Cheats();
    }

    void Cheats()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {
            canCollide = !canCollide;
        }

        else if (Input.GetKey(KeyCode.L))
        {
            NextLevel();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (!isTransitioning)
        {
            switch (other.gameObject.tag)
            {
            case "Friendly":
                Debug.Log("On landing pad");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
            }
       }
    }

    void StartCrashSequence()
    {
        if (canCollide)
        {
            MovementScript.enabled = false;
            isTransitioning = true;
            explodeParticles.Play();
            colisAudio.Stop();
            colisAudio.PlayOneShot(deathSound);
            Invoke("ReloadLevel", reloadTime);
        }
    }

    void ReloadLevel()
    {
        MovementScript.enabled = false;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void StartSuccessSequence()
    {
        MovementScript.enabled = false;
        isTransitioning = true;
        successParticles.Play();
        colisAudio.Stop();
        colisAudio.PlayOneShot(success);
        Invoke("NextLevel", reloadTime);
    }

    void NextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextSceneIndex);
    }
}
