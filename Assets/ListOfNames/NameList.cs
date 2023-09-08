using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu (menuName =("NameList/Single"))]
public class NameList : ScriptableObject
{
    public List<string> nameList;

    public void RemoveDuplicateNames()
    {
        for (int i = 0; i < nameList.Count; i++)
        {
            for (int x = 0; x < nameList.Count; x++)
            {
                if (nameList[x] == nameList[i] && x != i)
                {
                    nameList.RemoveAt(x);
                    RemoveDuplicateNames();
                }
            }
        }
    }
}


[CustomEditor(typeof(NameList))]
public class NameListEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        NameList nameList = (NameList)target;
        if (nameList == null) return;

        if (GUILayout.Button("Remove Duplicate Names")) nameList.RemoveDuplicateNames();
    }
}
