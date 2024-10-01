using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Car : MonoBehaviour
{
    
    [SerializeField] float speed = 0f;
    [SerializeField] float speedChange = 5f;
    [SerializeField] float turnSpeed = 100f;
    [SerializeField] float maxSpeed = 30f;
    [SerializeField] float minSpeed = 10f;

    int steerValue;
    public float Speed { get {return speed; } }


    void Update()
    {
        
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if (speed < maxSpeed && steerValue == 0)
        {
            speed += speedChange * Time.deltaTime;
            
            
        }
        else if (speed > minSpeed && steerValue != 0)
        {
            speed -= speedChange * Time.deltaTime;
            
        }
        
        if (transform.position.y <= -0.25)
        {
            ProcessCollision();
        }

        transform.Rotate(0f, steerValue * turnSpeed * Time.deltaTime, 0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            ProcessCollision();
        }
    }

    public void Steer(int value)
    {
        steerValue = value;
    }

    void ProcessCollision()
    {
        SceneManager.LoadScene(0);
    }
}
