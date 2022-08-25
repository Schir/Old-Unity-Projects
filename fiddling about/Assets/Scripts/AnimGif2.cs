
using UnityEngine;
using System.IO;

public class AnimGif2 : MonoBehaviour {

    [SerializeField] private Texture[] frames;
    [SerializeField] private  float fps = 10.0f;
	private Texture texxxture;
    private Material mat;

	private int dummy=0;
	private int dummy1 = 0;
	private int timer = 0;
	private bool direction = true;
    void Start () {
        mat = GetComponent<Renderer> ().material;
		frames = new Texture2D[25];
		for(int i = 0; i < 25; i++)
			{
				dummy1++;
				string filepathstring = "Textures/fractal_2_"+ dummy1.ToString();

				texxxture = Resources.Load(filepathstring) as Texture;
				frames[i] =  texxxture;
				//Debug.Log(frames[i]);
			}
		}

    void FixedUpdate () {
		if(timer == 3)
		{
			if(dummy >= 24)
			{
				direction = false;
			}
			else if(dummy <= 0)
			{
				direction = true;
			}
			switch(direction)
			{
				case false:
					dummy--;
					break;
				case true:
					dummy++;
					break;
			}
     	   mat.mainTexture = frames[dummy]; 
			timer = 0;
		}
		else
		{
			timer++;
		}
    }
}