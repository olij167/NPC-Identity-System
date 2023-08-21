using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using Pathfinding;

public class NPCBrain : MonoBehaviour
{
    public NPCInfo npcInfo;

    public NPCEmotions npcEmotions;

    //public NPCNeeds npcNeeds;

    public NPCRelationships npcRelationships;

   // public NPCSchedule npcSchedule;

    private AIDestinationSetter destinationSetter;

    private void Awake()
    {
        npcEmotions.personality = npcInfo.personality;

        destinationSetter = GetComponent<AIDestinationSetter>();

        ////Follow someone of interest for now

        //if (npcRelationships.partner != null)
        //{
        //    destinationSetter.target = npcRelationships.partner.transform;
        //}
        //else if (npcRelationships.parent1 != null )
        //{
        //    destinationSetter.target = npcRelationships.parent1.transform;

        //}
        //else if (npcRelationships.children != null && npcRelationships.children.Count > 0 )
        //{
        //    destinationSetter.target = npcRelationships.children[0].transform;
        //}
        //else
        //{
        //    for (int i = 0; i < npcRelationships.relationships.Count; i++)
        //    {
        //        if (npcRelationships.relationships[i].affiliation == NPCRelationships.Affiliation.Friend)
        //        {
        //            destinationSetter.target = npcRelationships.relationships[i].otherNPC.transform;
        //            break;

        //        }
        //    }
        //}

    }

    private void Update()
    {
        npcEmotions.SetMood(npcEmotions.emotion, npcEmotions.personality);
    }


    public void SetRelationshipStats(NPCBrain otherNPC)
    {
        NPCRelationships.Relationship newRelo = new NPCRelationships.Relationship();

        newRelo.otherNPC = otherNPC;

        //Check beliefs & determine compatibility
        //Check Sexual preferences & determine attraction

        //Set authority based on age & occupation

        //Set respect based on authority and compatibility
        //Set trust based on reputation and comfort

        //Set affection based on other npc reputation, relationship type
    }

    //private void FixedUpdate()
    //{
    //    //TrackNeeds();
    //}

    //public void DecreaseNeedIncrimental(float need, float decreaseRate)
    //{
    //     need -= Time.deltaTime * decreaseRate;
    //}

    //public void IncreaseNeedIncrimental(float need, float increaseRate)
    //{
    //     need += Time.deltaTime * increaseRate;
    //}

    //public float DecreaseNeedInstant(float need, float decreaseAmount)
    //{
    //    return need -= decreaseAmount;
    //}

    //public float IncreaseNeedInstant(float need, float increaseAmount)
    //{
    //    return need += increaseAmount;
    //}

    //public void TrackNeeds()
    //{
    //    npcNeeds.hunger += Time.deltaTime * npcNeeds.hungerIncreaseRate;
    //    npcNeeds.thirst += Time.deltaTime * npcNeeds.thirstIncreaseRate;
    //    npcNeeds.toilet += Time.deltaTime * npcNeeds.toiletIncreaseRate;
    //    npcNeeds.social -= Time.deltaTime * npcNeeds.socialDecreaseRate;
    //    npcNeeds.boredom += Time.deltaTime * npcNeeds.boredomIncreaseRate;

    //    if (WeatherController.instance.temperature > npcNeeds.temperatureRange.x)
    //    {
    //        npcNeeds.warmth += Time.deltaTime * WeatherController.instance.temperature - npcNeeds.temperatureRange.x;


    //    }
    //    else if (WeatherController.instance.temperature < npcNeeds.temperatureRange.y)
    //    {
    //        npcNeeds.warmth -= Time.deltaTime * WeatherController.instance.temperature + npcNeeds.temperatureRange.y;

    //    }
    //}
}

//[CustomEditor(typeof(NPCBrain))]
//public class NPCEmotionEditor : Editor
//{
//    NPCInfo info;
//    NPCEmotions emotions;
//    NPCRelationships relationships;
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        NPCBrain brain = (NPCBrain)target;
//        if (brain == null) return;

//        info = brain.npcInfo;
//        emotions = brain.npcEmotions;
//        relationships = brain.npcRelationships;

//    }
//}
