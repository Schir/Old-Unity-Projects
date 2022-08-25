using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInfo : MonoBehaviour
{
    public enum ControllerStuff
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        STRAFELEFT,
        STRAFERIGHT,
        CONFIRM,
        CANCEL,
        MENU
    }
    public static Dictionary<KeyCode, ControllerStuff> ControllerGeneric = new Dictionary<KeyCode, ControllerStuff>{
    {KeyCode.UpArrow, ControllerStuff.UP},
    {KeyCode.W, ControllerStuff.UP},
    {KeyCode.A, ControllerStuff.LEFT},
    {KeyCode.LeftArrow, ControllerStuff.LEFT},
    {KeyCode.RightArrow, ControllerStuff.RIGHT},
    {KeyCode.RightArrow, ControllerStuff.RIGHT},
    {KeyCode.DownArrow, ControllerStuff.DOWN},
    {KeyCode.S, ControllerStuff.DOWN},
    {KeyCode.Return, ControllerStuff.CONFIRM},
    {KeyCode.Z, ControllerStuff.CONFIRM},
    {KeyCode.Space, ControllerStuff.CONFIRM},
    {KeyCode.X, ControllerStuff.CANCEL},
    {KeyCode.Q, ControllerStuff.STRAFELEFT},
    {KeyCode.W, ControllerStuff.STRAFERIGHT},
    {KeyCode.Escape, ControllerStuff.MENU}
    };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
