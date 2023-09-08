using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = ("NameList/Madlib"))]
public class MadLibNameList : ScriptableObject
{
    public List<string> nameList;

    public List<string> prefixList;
    public List<string> suffixList;

    public string GenerateName()
    {
        int p = Random.Range(0, prefixList.Count);
        int s = Random.Range(0, suffixList.Count);

        if (!System.Char.IsUpper(prefixList[p][0]))
            prefixList[p] = FirstLetterCapital(prefixList[p]);

        if (System.Char.IsUpper(suffixList[s][0]))
        {
            return prefixList[p] + " " + suffixList[s];
        }
        else
            return prefixList[p] + suffixList[s];
    }
    public void GenerateNamesList(int numOfNames)
    {

        for (int i = 0; i < numOfNames; i++)
        {
            int p = Random.Range(0, prefixList.Count);
            int s = Random.Range(0, suffixList.Count);

            if (!System.Char.IsUpper(prefixList[p][0]))
                prefixList[p] = FirstLetterCapital(prefixList[p]);

            if (System.Char.IsUpper(suffixList[s][0]))
            {
                nameList.Add(prefixList[p] + /*baseWordList[b] +*/ " " + suffixList[s]);
            }
            else
                nameList.Add(prefixList[p] + /*baseWordList[b] +*/ suffixList[s]);
        }
    }

    private string FirstLetterCapital(string str)
    {
        return System.Char.ToUpper(str[0]) + str.Remove(0, 1);
    }

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

        for (int i = 0; i < prefixList.Count; i++)
        {
            for (int x = 0; x < prefixList.Count; x++)
            {
                if (prefixList[x] == prefixList[i] && x != i)
                {
                    prefixList.RemoveAt(x);
                    RemoveDuplicateNames();
                }
            }
        }

        for (int i = 0; i < suffixList.Count; i++)
        {
            for (int x = 0; x < suffixList.Count; x++)
            {
                if (suffixList[x] == suffixList[i] && x != i)
                {
                    suffixList.RemoveAt(x);
                    RemoveDuplicateNames();
                }
            }
        }
    }
}


[CustomEditor(typeof(MadLibNameList))]
public class MadLibNameListEditor : Editor
{
    int numOfNames;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MadLibNameList nameList = (MadLibNameList)target;
        if (nameList == null) return;

        GUILayout.BeginHorizontal();

        numOfNames = EditorGUILayout.IntField("Num of Names:", numOfNames);

        if (GUILayout.Button("Generate Names")) nameList.GenerateNamesList(numOfNames);

        GUILayout.EndHorizontal();

        if (GUILayout.Button("Remove Duplicate Names")) nameList.RemoveDuplicateNames();
    }
}

