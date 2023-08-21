using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Create and edit npc emotions in the inspector
#if UNITY_EDITOR
[CustomEditor(typeof(PlayerInfoController))]

public class StatsInspectorEditor : Editor
{
    StatContainer statContainer;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PlayerInfoController playerInfo = (PlayerInfoController)target;
        if (playerInfo == null) return;

        statContainer = playerInfo.playerStats;

        GUILayout.Space(5f);

        GUILayout.Label("Create New Stats for the Player");

        GUILayout.BeginHorizontal();

        statContainer.newStatName = GUILayout.TextField(statContainer.newStatName, 25);

        if (GUILayout.Button("Create New Stat"))
        {
            // Create a new emotion and add it to the list
            StatContainer.Stat newStat = statContainer.CreateNewStat(statContainer.newStatName);
            statContainer.AddStatToList(newStat);

            statContainer.CheckStatValues();

            // reset text field
            statContainer.newStatName = "New Stat";

        }
        GUILayout.EndHorizontal();

        GUILayout.Space(5f);

        GUILayout.Label("Increase & Decrease Stat Values");

        for (int i = 0; i < statContainer.listOfStats.Count; i++)
        {
            //CheckStatValues();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Increase value"))
            {
                statContainer.listOfStats[i].statValue++;

                statContainer.CheckStatValues();
            }

            GUILayout.Label(statContainer.listOfStats[i].statName);

            if (GUILayout.Button("Decrease value"))
            {
                statContainer.listOfStats[i].statValue--;

                statContainer.CheckStatValues();
            }
            GUILayout.EndHorizontal();


        }
    }

  
}
#endif
