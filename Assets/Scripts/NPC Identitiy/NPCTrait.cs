using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Trait")]
[System.Serializable]
public class NPCTrait : ScriptableObject
{
    public string traitName;
    public int traitID;

    [TextArea(2, 10)] public string traitDescription;

    public NPCSkills effectOnSkills;
    public NPCEmotions.EmotionalDisposition effectOnDisposition;
    public NPCInfo.BeliefValues effectOnBeliefValues;

    public List<NPCTrait> conflictingTraits; // Traits which cannot be added if the current trait is active

    // effect the NPCs interests (likes & dislikes)
    // effect the NPCs relationships
}



// Create a script to generate & effect an npc's identity values based on their traits