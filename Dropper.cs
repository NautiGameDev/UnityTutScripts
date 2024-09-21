using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour
{
    MeshRenderer objRend;
    Rigidbody myRigidBody;
    [SerializeField] float fallTime = 100.0f;
    bool boxActive = false;

    // Start is called before the first frame update
    void Start()
    {
        objRend = GetComponent<MeshRenderer>();
        myRigidBody = GetComponent<Rigidbody>();

        objRend.enabled = false;
        myRigidBody.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > fallTime && boxActive == false)
        {
            TriggerFall();
        }
    }

    void TriggerFall()
    {
        boxActive = true;
        myRigidBody.useGravity = true;
        objRend.enabled = true;
    }
}
