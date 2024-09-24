using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] GameObject explosionFX;
    [SerializeField] GameObject hitVFX;
    GameObject parentGameObject;
    [SerializeField] int points = 100;
    [SerializeField] int hitpoints = 50;

    ScoreBoard SB;

    void Start()
    {
        SB = FindObjectOfType<ScoreBoard>(); //Find the first object of type scoreboard
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;

        parentGameObject = GameObject.FindWithTag("ParticleParent");
        

    }

    void OnParticleCollision(GameObject other)
    {
        hitpoints -= 20;
        ProcessParticles(hitVFX);

        if (hitpoints <= 0)
        {
            KillEnemy();
        }

        ProcessScore();
    }

    void KillEnemy()
    {
        ProcessParticles(explosionFX);
        Destroy(gameObject);
        
    }

    void ProcessParticles(GameObject particleType)
    {
        GameObject vfx = Instantiate(particleType, transform.position, Quaternion.identity);
        vfx.transform.parent = parentGameObject.transform;    
    }

    void ProcessScore()
    {
        SB.IncreaseScore(points);
    }
}
