using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class NPCInfo
{
    public string fullName;
    public string firstName;
    public string lastName;

    public Family family;
    public Color skinColour;
    //public Color skinColour1;
    //public Color skinColour2;
    public enum Gender { Male, Female, NonBinary}
    public Gender gender;

    [Range(-1,1)]public float appearance; //-1 = feminine, 0 = androgynous, 1 = masculine
    [Range(-1,1)]public float sexualPreference; //-1 = feminine, 0 = androgynous, 1 = masculine
    //public Vector2 sexuality;

    //public enum GenderPreference {MasculineAttraction, FeminineAttraction, BiAttraction}
    //public GenderPreference genderPreference;

    //public float genderPreference; // -1 = female attraction, 0 = bi, 1 = male attraction

    public int age;
    public enum LifeStage { Baby, Toddler, Child, Teen, YoungAdult, Adult, Elderly, VeryElderly, Deceased, Error};
    public LifeStage lifeStage;

    public List<NPCTrait> npcTraits;
    public NPCSkills npcSkills;

    //public bool ofAge;
    //public bool hasChildren;
    //public int numOfChildren;
    //public bool hasPartner;

    [HideInInspector] public NPCEmotions.Personality personality;

    public BeliefValues beliefs;
    public List<NPCConceptualBeliefs> conceptualBeliefs;

    public List<BackstoryElement> backstory;

    public Job job;

[System.Serializable]
    public class BeliefValues
    {
        [Range(-1f, 1f)] public float independence; // -1 = communism, 1 = capitalism
        [Range(-1f, 1f)] public float diversity; // -1 = conservatism, 1 = liberalism
        [Range(-1f, 1f)] public float freedom; // -1 = totalitarianism, 1 = progressivism

    }

    [System.Serializable]
    public class NPCConceptualBeliefs
    {
        public BeliefConcept concept;
        [Range(-1f, 1f)] public float passionForConcept;
        public BeliefConcept.BeliefStance stanceOnConcept;
    }

    [System.Serializable]
    public class Family
    {
        public string lastName;
        public BeliefValues familyValues = new BeliefValues();
        public List<BeliefConcept> familyConceptualBeliefs = new List<BeliefConcept>();

        public NPCSkills skillGenetics = new NPCSkills();

        public Color skinColour;

        //public List<NPCBrain> familyMembers;
    }


//[System.Serializable]
//    public class Backstory
//    {
//        public List<BackstoryElement> familyHistory;
//        public List<BackstoryElement> personalHistory;
//    }

[System.Serializable]
    public class BackstoryElement
    {
        public string significantEvent;

        public int ageDuringEvent;

        public BeliefValues effectOnBeliefs;
        public NPCRelationships.Relationship effectOnRelationship;
        //public NPCRelationships.Reputation effectOnReputation;
    }

}

//#if UNITY_EDITOR
//[CustomEditor(typeof(NPCBrain))]
//public class NPCInfoEditor : Editor
//{
//    NPCGenerator generator;
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        NPCBrain npc = (NPCBrain)target;
//        if (npc.npcInfo == null) return;

//        if (generator == null)
//        {
//            generator = FindObjectOfType<NPCGenerator>();
//        }
//        else
//        {
//            generator.SetLifeStage(npc.npcInfo);
//        }
//    }
//}
//#endif