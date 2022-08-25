using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownMover : MonoBehaviour
{
    GameObject cam;
    Rigidbody rb;
    Vector3 vel;
    float multiplier = 4f;
    [SerializeField]
    Transform playerInputSpace = default;
    float interactionRadius = 2.0f;

    
    // Start is called before the first frame update
    void Awake()
    {
        cam = Camera.main.gameObject;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 forw = Vector3.Cross(cam.transform.forward, transform.forward);
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        Vector3 righ = Vector3.Cross(cam.transform.right, transform.right);
        if (playerInputSpace) {
		    Vector3 forward = playerInputSpace.forward;
			forward.y = 0f;
			forward.Normalize();
			Vector3 right = playerInputSpace.right;
			right.y = 0f;
			right.Normalize();
			vel =
			(forward * v + right * h);
            Vector3 pos;
            if(playerInputSpace.position.y > 0f)
            {
                pos = playerInputSpace.position - new Vector3(0, playerInputSpace.position.y, 0);
            }
            else
            {
                pos = playerInputSpace.position + new Vector3(0, playerInputSpace.position.y, 0);
            }
            //transform.LookAt(pos);
		}

        if(v < 0f || h > 0f)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if(v > 0f || h < 0f)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        Vector3.Normalize(vel);
        rb.velocity = (vel * multiplier);
        if(Input.GetKeyUp(KeyCode.Z) && FindObjectOfType<Yarn.Unity.DialogueRunner>().isDialogueRunning == false)
        {
            CheckForNPCs();
        }
        if(Input.GetKeyUp(KeyCode.Z) && FindObjectOfType<Yarn.Unity.DialogueRunner>().isDialogueRunning)
        {
            FindObjectOfType<Yarn.Unity.DialogueUI>().MarkLineComplete();    
        }
    }
    void CheckForNPCs()
    {
            var allParticipants = new List<Yarn.Unity.Example.NPC> (FindObjectsOfType<Yarn.Unity.Example.NPC> ());
            var target = allParticipants.Find (delegate (Yarn.Unity.Example.NPC p) {
                return string.IsNullOrEmpty (p.talkToNode) == false && // has a conversation node?
                (p.transform.position - this.transform.position)// is in range?
                .magnitude <= interactionRadius;
            });
            if (target != null) {
                // Kick off the dialogue at this node.
                FindObjectOfType<Yarn.Unity.DialogueRunner> ().StartDialogue (target.talkToNode);
            }
    }
}
