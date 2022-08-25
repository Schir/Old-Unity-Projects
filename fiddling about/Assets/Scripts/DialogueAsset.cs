using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class DialogueAsest
{
    [MenuItem("Assets/Create/Dialogue")]
    public static void CreateAsset ()
    {
        Dialogue asset = ScriptableObject.CreateInstance<Dialogue>();
        
        AssetDatabase.CreateAsset(asset, "Assets/Resources/Dialogue/NewDialogue.asset");
        AssetDatabase.SaveAssets();
        
        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}