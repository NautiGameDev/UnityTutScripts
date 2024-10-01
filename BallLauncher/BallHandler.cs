using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] GameObject ballPrefab;
    [SerializeField] Rigidbody2D pivotPoint;
    [SerializeField] float respawnDelay = 2f;
    [SerializeField] float detachDelay = 0.2f;
    Rigidbody2D currentBallRigidbody;
    SpringJoint2D currentBallSpringJoint;
    bool isDragging;

    Camera mainCamera;
    

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        isDragging = false;
        SpawnNewBall();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBallRigidbody == null)
        {
            return;
        }

        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (isDragging)
            {
                LaunchBall();
            }
            isDragging = false;
            return;
        }

        isDragging = true;
        currentBallRigidbody.isKinematic = true;
        Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(touchPos);
        currentBallRigidbody.position = worldPos;

        
    }

    void LaunchBall()
    {
        currentBallRigidbody.isKinematic = false;
        currentBallRigidbody = null;

        Invoke("DetachBall", detachDelay);    
    }

    void DetachBall()
    {
        currentBallSpringJoint.enabled = false;
        currentBallSpringJoint = null;

        Invoke("SpawnNewBall", respawnDelay);
    }

    void SpawnNewBall()
    {
        
        GameObject ballInstance = Instantiate(ballPrefab, pivotPoint.position, Quaternion.identity);
        currentBallRigidbody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();
        currentBallSpringJoint.connectedBody = pivotPoint;
    }
}
