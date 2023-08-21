using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BeliefConcept")]
[System.Serializable]
public class BeliefConcept : ScriptableObject
{
    public string conceptName;
    [TextArea(2, 10)] public string conceptDescription;

    //public BeliefStance npcStance;
    public List<BeliefStance> potentialNPCStancesOnConcept; // check against npc beliefs and assign their beliefType based on how closely it alligns with their beliefs

    [System.Serializable]
    public class BeliefStance
    {
        [Range(-1f, 1f)] public float levelOfAgreeance; // -1 = disagrees, 0 = impartial, 1 = agrees
        public NPCInfo.BeliefValues beliefAllignment; // the belief values that correspond to that level of agreeance with a concept


    }
}

