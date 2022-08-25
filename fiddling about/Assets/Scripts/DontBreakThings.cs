using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontBreakThings : MonoBehaviour {

	GameObject Manager;
	void Start()
	{

	}
	void Awake()
	{
		Manager = GameObject.Find("GameManager");
		DontDestroyOnLoad(Manager);
	}
}
