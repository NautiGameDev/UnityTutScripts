using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] float xRotSpeed = 0.0f;
    [SerializeField] float yRotSpeed = 0.0f;
    [SerializeField] float zRotSpeed = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xRot = xRotSpeed * Time.deltaTime;
        float yRot = yRotSpeed * Time.deltaTime;
        float zRot = zRotSpeed * Time.deltaTime;

        transform.Rotate(xRot, yRot, zRot);
    }
}
