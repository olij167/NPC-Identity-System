using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =("PersonalityTrait"))]
[System.Serializable]
public class PersonalityTrait : ScriptableObject
{
    public string traitName;
    [TextArea(3, 5)] public string traitDescription;

    public EmotionalEffect effectOnEmotions;
    //public NPCRelationships.RelationshipVariables effectOnRelationships; // needs a way to variate based on existing relationship variables?
    public NPCInfo.BeliefValues effectOnBeliefs;
}

[System.Serializable]
public class EmotionalEffect
{
    public Vector2 happinessThresholds, stressThresholds, shockThresholds; // how the trait affects the npc's emotional thresholds

}
