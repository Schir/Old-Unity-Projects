using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAI1 : MonoBehaviour
{
    public Transform Target;
    public float timer = 0;
    public float timerMax = 2.5f;
    public bool directionforward = false;
    public bool directionsideways = true;
    public Rigidbody rb;
    int thrusts = 0;
    //public float timerMax = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer >= timerMax)
        {
            timer = 0f;
            if(directionforward)
            {
                rb.AddForce(transform.right * 40f);
            }
            else
            {
                rb.AddForce(transform.right * -40f);
            }
            thrusts++;
        }
        else
        {
            timer += Time.deltaTime;
        }
        if(thrusts > 3)
        {
            directionforward = !directionforward;
            thrusts = 0;
        }
        if(!directionforward)
        {
            //if(transform.rotation.x < 180)
            transform.Rotate(Vector3.up * .3f);
        }
        else
        {
            transform.Rotate(Vector3.up *  -.3f);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        directionforward = !directionforward;
        if(directionforward)
        {
            transform.Rotate(Vector3.up * -90f);
        }
        else
        {
            transform.Rotate(Vector3.up * 90f);
        }
    }
}
