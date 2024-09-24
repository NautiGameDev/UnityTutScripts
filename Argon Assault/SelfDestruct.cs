using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{

    [SerializeField] bool sdActive = false;
    [SerializeField] float timeToDestroy = 3f;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource explosionSFX = GetComponent<AudioSource>();

        if (sdActive)
        {
            if (explosionSFX != null)
            {
                explosionSFX.Play();
            }
            Destroy(gameObject, timeToDestroy);
            
        }
    }

    
}
