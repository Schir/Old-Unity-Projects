using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class SenatorAsest
{
    [MenuItem("Assets/Create/Parliament")]
    public static void CreateAsset ()
    {
        MembersOfParliament asset = ScriptableObject.CreateInstance<MembersOfParliament>();
        List<Senator> tempList = new List<Senator>();
        System.Random rand = new System.Random();
        for(int i = 0; i < 15; i++)
        {
            tempList.Add(Senator.CreateSenator(rand));
        }
        asset.ListOfSenators = tempList;
        AssetDatabase.CreateAsset(asset, "Assets/Resources/Senator/NewParliament.asset");

        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}