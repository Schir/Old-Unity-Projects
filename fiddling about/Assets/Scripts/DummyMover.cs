using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMover : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Camera.main)
		{
			transform.position = Camera.main.transform.position;
		}
	}
}
