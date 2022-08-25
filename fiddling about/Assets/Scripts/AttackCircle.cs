using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class AttackCircle : MonoBehaviour {
	private int timer;
	private int hitCount;
	private bool hasFailedChallenge;
	private bool circleActive;
	private float zone1Start;
	private float zone1End;
	private float zone2Start;
	private float zone2End;
	private float zone3Start;
	private float zone3End;
	private int segments;
	
	void Start()
	{
		hasFailedChallenge = false;
		timer = 0;
		circleActive = false;
		zone1Start = 0.2f;
		zone1End = zone1Start + 0.15f;
		zone2Start = 0.5f;
		zone2End = zone2Start + 0.15f;
		zone3Start = 0.7f;
		zone3End = zone3Start + 0.2f;
		segments = 100;
	}

	void Update () 
	{
		if(circleActive)
		{
			//do something
		}
	}

	void DrawCircle(int segments)
	{

	}
	void drawLineSweep()
	{
		
	}
}