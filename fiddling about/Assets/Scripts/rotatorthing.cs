using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatorthing : MonoBehaviour {

	public int timer;
	private bool direction;
	private int timerMax;
	private int i;
	private int delay;

	// ----------------------------------------
	void Start () {
		timer = 0;
		direction = true;
		i = 0;
		timerMax = 23;
		delay = 4;
	}
	

	//----------------------------------------
	void Update () {
	if(i == delay)
	{
		if(direction)
		{
			if(timer < timerMax)
			{	
				transform.Rotate(new Vector3(0.05f * timer, 0.0f, 0.0f));
			}
			if(timer >= timerMax)
			{
				direction = false;
				timer = 0;
			}
		}
		if(!direction)
		{
			if(timer < timerMax)
			{
				transform.Rotate(new Vector3(-0.05f * timer , 0.0f, 0.0f));
			}
			if(timer >= timerMax)
			{
				direction = true;
				timer = 0;
			}
		}
		i = 0;
		timer += 1;
	}
	else
	{
		i += 1;
	}
	}
}
