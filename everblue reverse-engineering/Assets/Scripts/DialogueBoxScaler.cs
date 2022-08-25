using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBoxScaler : MonoBehaviour
{
    public TextMesh mesh;
    public SpriteRenderer render;
    int lineCount;
    int width;
    bool updateNeeded = true;
    int length;
    public UnityEngine.UI.Text texter;
    // Start is called before the first frame update
    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        //lineCount = CountLines();
        length = mesh.text.Length;
        //RedrawBox();
    }

    // Update is called once per frame
    void Update()
    {
        mesh.text = texter.text;
        if(mesh.text.Length != length)
        {
            updateNeeded = true;
        }
    }

    void LateUpdate()
    {
        if(updateNeeded)
        {
            RedrawBox();
        }
    }

    int CountLines()
    {
        int i = 0;
        int previousloc = 0;
        int currentloc = 0;
        int w = 0;
        for(int j = 0; j < mesh.text.Length; j++)
        {
            if(mesh.text[j] == '\n')
            {
                i++;
                previousloc = currentloc;
                currentloc = j;
                if(currentloc-previousloc > w)
                {
                    w = currentloc - previousloc;
                }
            }
        }
        if(w == 0)
        {
            w = mesh.text.Length - currentloc;
        }
        else if (w < mesh.text.Length - currentloc)
        {
            w = mesh.text.Length - currentloc;
        }
        width = w;
        if(i == 0)
        {
            return 1;
        }
        else
        {
            return i+1;
        }
    }
    void RedrawBox()
    {
        lineCount = CountLines();
        render.size = new Vector2((width * mesh.fontSize * mesh.characterSize/200f)+ .14f, 
                                  (lineCount * mesh.fontSize * mesh.characterSize /100f * 1.5f )+ 0.14f);
                                  updateNeeded = false;
    }
}
