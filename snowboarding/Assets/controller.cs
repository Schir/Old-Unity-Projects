using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(transform.forward * 5f);
        if(Input.GetAxis("Horizontal") != 0)
        {
            rb.AddForce(transform.right * Input.GetAxis("Horizontal") * 6f);
        }
        if(Input.GetAxis("Vertical") != 0)
        {
            rb.AddForce(transform.forward * Input.GetAxis("Vertical") * 9f);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "goal" || collision.gameObject.tag == "Goal")
        {
            Debug.Log("You win!");
        }
        Debug.Log("There was definitely a collision.");
    }
    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Goal" || collider.tag == "goal")
        {
            Debug.Log("You win!");
        }
    }
}
