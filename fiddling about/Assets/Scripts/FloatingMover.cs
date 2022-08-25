using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingMover : MonoBehaviour
{
    private bool direction = true;
    private Vector3 frameMove;

    void Start()
    {
        transform.position = transform.position + new Vector3(0f,-0.3f,0.0f);
        frameMove = Vector3.up;
    }
    void Update()
    {
        if (transform.position.y >= .75f)
        {
            direction = false;
        }
        else if(transform.position.y <= 0.2f)
        {
            direction = true;
        }
        if(direction)
        {
            transform.position = transform.position +  (frameMove * Time.deltaTime * 0.1f);
        }
        else
        {
            transform.position = transform.position - (frameMove * Time.deltaTime * 0.1f);
        }
    }
}