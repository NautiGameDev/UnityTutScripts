using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Enemy))] //Requires a script to be present on object in editor
public class EnemyHealth : MonoBehaviour
{

    [SerializeField] int maxHitPoints = 5;
    
    [Tooltip("Adds amount to max hitpoints when enemy dies.")]
    [SerializeField] int difficultyRamp = 1;
    int currentHitPoints = 5;

    Enemy enemy;

    // Start is called before the first frame update
    void OnEnable()
    {
        currentHitPoints = maxHitPoints;
    }

    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnParticleCollision(GameObject other)
    {
        ProcessHit();
    }

    void ProcessHit()
    {
        currentHitPoints --;

        if (currentHitPoints <= 0)
        {
            enemy.RewardGold();
            gameObject.SetActive(false);
            maxHitPoints+= difficultyRamp;
            
        }
    }
}
