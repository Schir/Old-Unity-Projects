using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorAnimator : MonoBehaviour {

	public int timer;
	public int animationFrame;
	private int timerMax = 8;
	private int animationFrameMax = 3;
	private float frame0z;
	private float frame1z;
	private float frame2z;
	private float frame3z;
	private Vector3 frame0;
	private Vector3 frame1;
	private Vector3 frame2;
	private Vector3 frame3;
	public bool animate = true;
	public bool animationChosen = false;

	// Use this for initialization
	void Start () {
		timer = 0;
		animationFrame = 0;
		frame0z = -37.1f/-2.5f;
		frame1z = -50.2f/-5.5f;
		frame2z = 48.5f/-3.5f;
		frame3z = 32.0f/-2.5f;
		frame0 = new Vector3(0.0f, 0.0f, frame0z);
		frame1 = new Vector3(0.0f, 0.0f, frame1z);
		frame2 = new Vector3(0.0f, 0.0f, frame2z);
		frame3 = new Vector3(0.0f, 0.0f, frame3z);
		

	}
	
	public void CheckFrame(int frameNumber)
	{
		switch(frameNumber)
		{
			case 0:
				transform.rotation = Quaternion.Euler(frame0);
				break;
			case 1:
				transform.rotation = Quaternion.Euler(frame1);
				break;

			case 2:
				transform.rotation = Quaternion.Euler(frame2);
				break;
			case 3:
				transform.rotation = Quaternion.Euler(frame3);
				break;
			default:
				break;
		}
	}
	// Update is called once per frame
	void Update () {
		if(animate)
		{
			if(timer == timerMax)
			{
				if(animationFrame == animationFrameMax)
				{
					animationFrame = 0;
				} 
				else
				{
					animationFrame++;
				}
				timer = 0;
				CheckFrame(animationFrame);
			}
			else
			{
				timer++;
			}
		}
		else
		{
			if(!animationChosen)
			{
				System.Random rnd = new System.Random();
				CheckFrame(rnd.Next(0, 3));
				animationChosen = true;
			}
		}		
	}
	public void setAnimation(bool doIt)
	{
		animate = doIt;
	}
}
