using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{

    [SerializeField] float loadDelay = 1f;
    [SerializeField] ParticleSystem explosionFX;

    void Start()
    {
        explosionFX.Stop();
    }

    void OnTriggerEnter(Collider other)
   {
        StartCrashSequence();
   }

   void StartCrashSequence()
   {

        GetComponent<PlayerControls>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;

        explosionFX.Play();
        AudioSource explosionSFX = explosionFX.GetComponent<AudioSource>();
        explosionSFX.Play();
        
        Invoke("ReloadLevel", loadDelay);
   }

   void ReloadLevel()
   {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex);
   }
}
