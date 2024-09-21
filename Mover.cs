using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{

    [SerializeField] float moveSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        Instructions();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }


    void Instructions()
    {
        Debug.Log("This is just a simple beginner tutorial for noobz of noobz.");
    }

    void Move()
    {
        float xValue = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float zValue = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.Translate(xValue, 0.0f, zValue);
    }
}
