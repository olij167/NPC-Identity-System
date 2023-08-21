using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class NPCNeeds
{
    //public float hungerIncreaseRate;
    //[Range(0, 100f)] public float hunger;

    //public float thirstIncreaseRate;
    //[Range(0, 100f)] public float thirst;

    ////public float warmthDecreaseRate;
    //[Range(-100, 100f)] public float warmth;

    //public float toiletIncreaseRate;
    //[Range(0, 100f)] public float toilet;

    //public float boredomIncreaseRate;
    //[Range(0, 100f)] public float boredom;

    //public float socialDecreaseRate;
    //[Range(0, 100f)] public float social;

    public List<Need> needsList;

    public Vector2 temperatureRange; //y = cold min, x = hot max

    [System.Serializable]
    public class Need
    {
        public string needName;
        public float needValue;

        public float incrementRate;
        public bool isInstant;

        //public UnityEvent
    }

    //public float DecreaseNeedIncrimental(float need, float decreaseRate)
    //{
    //    return need -= Time.deltaTime * decreaseRate;
    //}

    //public float IncreaseNeedIncrimental(float need, float increaseRate)
    //{
    //    return need += Time.deltaTime * increaseRate;
    //}

    //public float DecreaseNeedInstant(float need, float decreaseAmount)
    //{
    //    return need -= decreaseAmount;
    //}

    //public float IncreaseNeedInstant(float need, float increaseAmount)
    //{
    //    return need += increaseAmount;
    //}

    public float ApplyIncrement(float needValue, float incrementRate, bool isInstant)
    {
        if (isInstant)
        {
            return needValue += incrementRate;
        }
        else
            return needValue += incrementRate * Time.deltaTime;
    }

    public void TrackNeeds()
    {
        //IncreaseNeedIncrimental(hunger, hungerIncreaseRate);
        //IncreaseNeedIncrimental(thirst, thirstIncreaseRate);
        //IncreaseNeedIncrimental(toilet, toiletIncreaseRate);
        //DecreaseNeedIncrimental(social, socialDecreaseRate);
        //IncreaseNeedIncrimental(boredom, boredomIncreaseRate);

        //if (WeatherController.instance.temperature > temperatureRange.x)
        //{
        //    IncreaseNeedIncrimental(warmth, WeatherController.instance.temperature - temperatureRange.x);
        //}
        //else if (WeatherController.instance.temperature < temperatureRange.y)
        //{
        //    DecreaseNeedIncrimental(warmth, WeatherController.instance.temperature + temperatureRange.y);
        //}

        foreach (Need need in needsList)
        {
            ApplyIncrement(need.needValue, need.incrementRate, need.isInstant);
        }
    }

    public Need CreateNewNeed(string needName)
    {
        float needValue = 0;

        Need newNeed = new Need();
        newNeed.needName = needName;
        newNeed.needValue = needValue;

        return newNeed;
    }

    public void AddNeedToList(Need newNeed)
    {
        needsList.Add(newNeed);

    }

    static int SortNeedsByValues(Need need1, Need need2)
    {
        return need2.needValue.CompareTo(need1.needValue);
    }

    public void CheckStatValues()
    {

        needsList.Sort(SortNeedsByValues);

    }

}

//[CustomEditor(typeof(NPCBrain))]
//public class NPCNeedsEditor : Editor
//{
//    NPCNeeds npcNeeds;
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        NPCBrain brain = (NPCBrain)target;
//        if (brain == null) return;

//        npcNeeds = brain.npcNeeds;

//        GUILayout.Space(5f);

//        GUILayout.Label("Create New Stats for the Player");

//        GUILayout.BeginHorizontal();

//        npcNeeds.newStatName = GUILayout.TextField(npcNeeds.newStatName, 25);

//        if (GUILayout.Button("Create New Stat"))
//        {
//            // Create a new emotion and add it to the list
//            StatContainer.Stat newStat = CreateNewStat(statContainer.newStatName);
//            AddStatToList(newStat);

//            CheckStatValues();

//            // reset text field
//            statContainer.newStatName = "New Stat";

//        }
//        GUILayout.EndHorizontal();

//        GUILayout.Space(5f);

//        GUILayout.Label("Increase & Decrease Stat Values");

//        for (int i = 0; i < statContainer.listOfStats.Count; i++)
//        {
//            //CheckStatValues();

//            GUILayout.BeginHorizontal();

//            if (GUILayout.Button("Increase value"))
//            {
//                statContainer.listOfStats[i].statValue++;

//                CheckStatValues();
//            }

//            GUILayout.Label(statContainer.listOfStats[i].statName);

//            if (GUILayout.Button("Decrease value"))
//            {
//                statContainer.listOfStats[i].statValue--;

//                CheckStatValues();
//            }
//            GUILayout.EndHorizontal();


//        }

//    }

   
//}
