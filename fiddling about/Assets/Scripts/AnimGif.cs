
using UnityEngine;
using System.IO;

public class AnimGif : MonoBehaviour {

    [SerializeField] private Texture[] frames;
	private Texture texxxture;
    private Material mat;

	private int dummy=0;
	private int dummy1 = 0;
	private float dummytimer = 0f;
	private bool direction = true;
    void Start () {
        mat = GetComponent<Renderer> ().material;
		frames = new Texture2D[150];
		for(int i = 0; i < 150; i++)
			{
				dummy1++;
				string filepathstring = "Textures/fractal"+ dummy1.ToString();

				texxxture = Resources.Load(filepathstring) as Texture;
				frames[i] =  texxxture;
				//Debug.Log(frames[i]);
			}
		}

    void FixedUpdate () {
        //int index = (int)(Time.time * fps);
        //index = index % frames.Length;
		if(dummytimer >= 0.005f)
		{
			if(dummy >= 149)
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
			dummytimer = 0f;
    	}
		else
		{
			dummytimer += Time.fixedDeltaTime;
		}
	}
}
