using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownCameraMover : MonoBehaviour
{
    Vector3 vell = Vector3.zero;
    GameObject player;
    public float smooth = 0.04f;
    public float maxSpeed = 360f;
    float camOffset = 10f;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    private void LateUpdate() {
            if(Vector3.Magnitude(transform.position - player.transform.position) > camOffset)
            {
                Vector3 target = (Vector3.Normalize(transform.position - player.transform.position + Vector3.up * 2) * camOffset) ;
                transform.position = Vector3.SmoothDamp(transform.position, target, ref vell, smooth, maxSpeed);
            }
            transform.LookAt(player.transform.position);

        
    }
}
