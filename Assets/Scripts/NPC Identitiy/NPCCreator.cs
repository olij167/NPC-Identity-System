using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Assign each NPC a house (or as homeless)
public class NPCCreator : MonoBehaviour
{
    //public static NPCCreator instance;

    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //    }
    //    else if (instance != this)
    //    {
    //        Destroy(gameObject);
    //    }
    //}
    public TimeController timeController;
    public GameObject npcPrefab;
    public NameList maleNames, femaleNames, nonBinaryNames, lastNames;

    public Vector2 personalityThresholdRange; //good default: x = 2, y = 15

    //public int elderlyNum;
    //public int adultNum;
    //public int childNum;

    public List<NPCInfo.Family> possibleFamilies;
    public List<NPCInfo.Family> families;

    public List<NPCBrain> npcList;

    [HideInInspector] public int familyNum;
    [HideInInspector] public int npcNum;

    public GameObject CreateNPC(NPCInfo.LifeStage lifeStage, NPCInfo.Gender gender, float appearance, float sexuality, NPCInfo.Family family)
    {
        GameObject newNPC = Instantiate(npcPrefab, SpawnObjects.GenerateRandomWayPoint(), Quaternion.identity);

        Material sharedBodyMaterial = newNPC.transform.GetChild(1).GetComponent<MeshRenderer>().sharedMaterial;
        Material bodyMaterial = newNPC.transform.GetChild(1).GetComponent<MeshRenderer>().material = new Material(sharedBodyMaterial);

        bodyMaterial.color = SetRandomColour();

        NPCBrain newBrain = newNPC.GetComponent<NPCBrain>();
        NPCInfo newInfo = newBrain.npcInfo;

        
        newInfo.lifeStage = lifeStage;
        newInfo.age = SetRandomAge(lifeStage);

        if (newInfo.age <= 18f)
        {
            Vector3 minSize = new Vector3(0.2f, 0.2f, 0.2f);
            newNPC.transform.localScale = Vector3.Lerp(minSize, Vector3.one, newInfo.age / 18f);
        }

        newInfo.personality = SetPersonality(newBrain);
        newBrain.npcEmotions.personality = newInfo.personality;

        newInfo.gender = gender;
        newInfo.appearance = appearance;
        //newInfo.genderPreference = sexuality;
        //newInfo.sexualPreference = SetSexualPreference(sexuality);
        newInfo.sexualPreference = sexuality;

        newInfo.community = GetComponent<Community>();
        newInfo.family = family;
        SetFullName(newInfo);

        //family.familyMembers.Add(newBrain);

        newInfo.beliefs = SetBeliefValues(family);

        newNPC.name = newInfo.firstName + " " + newInfo.lastName;

        newBrain.transform.Find("Head").GetChild(0).GetComponent<NPCUIPanel>().SetName();
        //newInfo.ofAge = ofAge;

        npcList.Add(newBrain);

        SetRelationships();

        if (newBrain.npcRelationships.parent2 != null)
            newBrain.npcInfo.familyOtherSide = newBrain.npcRelationships.parent2.npcInfo.family;

        Material sharedSkinMaterial = newNPC.transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial;
        Material skinMaterial = newNPC.transform.GetChild(0).GetComponent<MeshRenderer>().material = new Material(sharedSkinMaterial);

        newInfo.npcSkills.SetNPCSkills(newBrain);

        skinMaterial.color = SetSkinColour(newBrain);

        newBrain.npcSchedule.SetCalendar(newBrain.npcSchedule.calendar);

        return newNPC;
    }

    public void SpawnRandomNPC() //for inspector button
    {
        CreateNPC(SetLifeStage(SetRandomAge()), SetRandomGender(), SetRandomAppearance(), SetRandomSexualPreference(), SetRandomFamily());
    }

    public Color SetRandomColour(float minHue = 0f, float maxHue = 1f, float minSat = 0.5f, float maxSat = 1f, float minVal = 0.5f, float maxVal = 1f)
    {
        return Random.ColorHSV(minHue, maxHue, minSat, maxSat, minVal, maxVal);
    }

    public Color SetSkinColour(NPCBrain brain)
    {
        float r = Chance.NumberPicker(1f);

        if (brain.npcRelationships.parent1 != null && brain.npcRelationships.parent2 != null)
        {

            //Debug.Log(brain.name + " skin colour set based on parents. Colour mix = " + r);
            //brain.npcInfo.skinColour1 = brain.npcRelationships.parent1.npcInfo.skinColour;
            //brain.npcInfo.skinColour2 = brain.npcRelationships.parent2.npcInfo.skinColour;
            return brain.npcInfo.skinColour = Color.Lerp(brain.npcRelationships.parent1.npcInfo.skinColour, brain.npcRelationships.parent2.npcInfo.skinColour, r);
        }
        else
        {
            //Debug.Log(brain.name + " skin colour set based on family & chance. Colour mix = " + r);
            //brain.npcInfo.skinColour1 = brain.npcInfo.family.skinColour;
            //brain.npcInfo.skinColour2 = SetRandomColour(0f, 1f, 0.25f, 0.75f, 0.25f, 0.75f);

            if (brain.npcInfo.familyOtherSide != null)
            {
                return brain.npcInfo.skinColour = Color.Lerp(brain.npcInfo.family.skinColour, brain.npcInfo.familyOtherSide.skinColour, r);
            }
            else
                return brain.npcInfo.skinColour = Color.Lerp(brain.npcInfo.family.skinColour, SetRandomColour(0f, 1f, 0.25f, 0.75f, 0.25f, 0.75f), r);
        }
    }

    public void SetRelationships()
    {
        //Debug.Log("Setting Relationships...");

        foreach(NPCBrain npc in npcList)
        {
            List<NPCBrain> currentRelationships = new List<NPCBrain>();

            for (int i = 0; i < npc.npcRelationships.relationships.Count; i++)
            {
                currentRelationships.Add(npc.npcRelationships.relationships[i].otherNPC);
                //Debug.Log("Relationship between " + npc.name + " & " + npcList[i].name + " already exists.");
            }

            for (int i = 0; i < npcList.Count; i++)
            {
                if (npcList[i] != npc)
                {
                    if (!currentRelationships.Contains(npcList[i]))
                    {
                        NPCRelationships.Relationship newRelo = npc.npcRelationships.SetRelationshipValues(npc, npcList[i]);
                        npc.npcRelationships.relationships.Add(newRelo);
                        currentRelationships.Add(npcList[i]);
                        //Debug.Log("Relationship between " + npc.name + " & " + npcList[i].name + " has been set.");
                    }
                }
                else
                {
                    for (int x = 0; x < npc.npcRelationships.relationships.Count; x++)
                    {
                        if (npc.npcRelationships.relationships[x].otherNPC == npc)
                        {
                            npc.npcRelationships.relationships[x] = npc.npcRelationships.SetRelationshipValues(npc, npcList[i]);
                        }
                    }
                }
            }
        }
        //for (int y = 0; y < npcList.Count; y++)
        //{
        //    for (int i = 0; i < npcList.Count; i++)
        //    {
        //        if (npcList[i] != npcList[y])
        //        {
        //            for (int x = 0; x < npcList[y].npcRelationships.relationships.Count; x++)
        //            {
        //                if (npcList[y].npcRelationships.relationships[x].otherNPC = npcList[i])
        //                {
        //                    Debug.Log("Relationship between " + npcList[y].name + " & " + npcList[i].name + " already exists.");

        //                }
        //                else if (npcList[y].npcRelationships.relationships[x + 1] == null)
        //                {
        //                    NPCRelationships.Relationship newRelo = npcList[y].npcRelationships.SetRelationshipValues(npcList[y], npcList[i]);
        //                    npcList[y].npcRelationships.relationships.Add(newRelo);
        //                    Debug.Log("Relationship between " + npcList[y].name + " & " + npcList[i].name + " has been set.");
        //                }
        //            }
        //        }
        //    }
        //}
    }

    public string SetFullName(NPCInfo npc)
    {
        bool hasMiddleName = Chance.Percentage(33f);
        //bool hasTwoMiddleNames = Chance.Percentage(25f);
        switch (npc.gender)
        {
            case NPCInfo.Gender.Male:
                {
                    npc.firstName = maleNames.nameList[Random.Range(0, maleNames.nameList.Count)];

                    if (hasMiddleName)
                    {
                        string middleName = maleNames.nameList[Random.Range(0, maleNames.nameList.Count)];

                        if (middleName != npc.firstName)
                        {
                            npc.firstName += " " + middleName;
                        }

                        //if (hasTwoMiddleNames)
                        //{
                        //    string secondMiddleName = maleNames.nameList[Random.Range(0, maleNames.nameList.Count)];

                        //    if (secondMiddleName != npc.firstName && secondMiddleName != middleName)
                        //    {
                        //        npc.firstName += " " + middleName;
                        //    }
                        //}
                    }

                    break;

                }

            case NPCInfo.Gender.Female:
                {
                    npc.firstName = femaleNames.nameList[Random.Range(0, femaleNames.nameList.Count)];

                    if (hasMiddleName)
                    {
                        string middleName = femaleNames.nameList[Random.Range(0, femaleNames.nameList.Count)];

                        if (middleName != npc.firstName)
                        {
                            npc.firstName += " " + middleName;
                        }

                        //if (hasTwoMiddleNames)
                        //{
                        //    string secondMiddleName = femaleNames.nameList[Random.Range(0, femaleNames.nameList.Count)];

                        //    if (secondMiddleName != npc.firstName && secondMiddleName != middleName)
                        //    {
                        //        npc.firstName += " " + middleName;
                        //    }
                        //}
                    }
                    break;
                }

            case NPCInfo.Gender.NonBinary:
                {

                    npc.firstName = nonBinaryNames.nameList[Random.Range(0, nonBinaryNames.nameList.Count)];

                    if (hasMiddleName)
                    {
                        string middleName = nonBinaryNames.nameList[Random.Range(0, nonBinaryNames.nameList.Count)];

                        if (middleName != npc.firstName)
                        {
                            npc.firstName += " " + middleName;
                        }

                        //if (hasTwoMiddleNames)
                        //{
                        //    string secondMiddleName = nonBinaryNames.nameList[Random.Range(0, nonBinaryNames.nameList.Count)];

                        //    if (secondMiddleName != npc.firstName && secondMiddleName != middleName)
                        //    {
                        //        npc.firstName += " " + middleName;
                        //    }
                        //}
                    }

                    break;
                }

        }

        if (Chance.OddsOn(4))
            npc.lastName = npc.family.lastName + "-" + npc.familyOtherSide.lastName;
        else if (Chance.OddsOn(2))
            npc.lastName = npc.familyOtherSide.lastName;
        else
            npc.lastName = npc.family.lastName;

        return npc.fullName = npc.firstName + " " + npc.lastName;
    }

    public NPCEmotions.Personality SetPersonality(NPCBrain brain)
    {

        NPCEmotions.Personality personality = new NPCEmotions.Personality();
        personality.happinessThresholds.x = Random.Range(personalityThresholdRange.x, personalityThresholdRange.y); //happiness threshold
        personality.happinessThresholds.y = Random.Range(-personalityThresholdRange.y, -personalityThresholdRange.x); //sadness threshold

        personality.stressThresholds.x = Random.Range(personalityThresholdRange.x, personalityThresholdRange.y); //anger threshold
        personality.stressThresholds.y = Random.Range(-personalityThresholdRange.y, -personalityThresholdRange.x); //nervous threshold

        personality.shockThresholds.x = Random.Range(personalityThresholdRange.x, personalityThresholdRange.y); // surprise threshold
        personality.shockThresholds.y = Random.Range(-personalityThresholdRange.y, -personalityThresholdRange.x); // fear threshold

        personality.emotionalDisposition = brain.npcEmotions.SetEmotionalDisposition(personality.happinessThresholds, personality.stressThresholds, personality.shockThresholds);

        personality.extroversion = Random.Range(-1f, 1f);
        personality.attentionSpan = Random.Range(5f, 25f);

        brain.npcEmotions.personality = personality;
        //brain.npcEmotions.personality.emotionalDisposition = personality.emotionalDisposition;
        return personality;
    }

    public void CreateNewRandomFamilies(int numOfFamilies = 10)
    {
        possibleFamilies = new List<NPCInfo.Family>();

        for (int i = 0; i < numOfFamilies; i++)
        {
           possibleFamilies.Add(CreateRandomFamily()); 
        }
    }

    public void CreateCommunityFamilies(Community community, int numOfFamilies = 10)
    {
        possibleFamilies = new List<NPCInfo.Family>();

        for (int i = 0; i < numOfFamilies; i++)
        {
            possibleFamilies.Add(CreateFamily(community));
        }
    }

    public NPCInfo.Family CreateFamily(Community community)
    {
        NPCInfo.Family family = new NPCInfo.Family();

        string lastName = lastNames.nameList[Random.Range(0, lastNames.nameList.Count)];

        for (int i = 0; i < possibleFamilies.Count; i++)
        {
            if (possibleFamilies[i].lastName == lastName)
            {
                lastName = lastNames.nameList[Random.Range(0, lastNames.nameList.Count)];
                i = 0;
            }
        }

        family.lastName = lastName;

        family.familyValues.privacy = Random.Range(community.communityValues.privacy - 0.25f, community.communityValues.privacy + 0.25f);
        family.familyValues.diversity = Random.Range(community.communityValues.diversity - 0.25f, community.communityValues.diversity + 0.25f);
        family.familyValues.freedom = Random.Range(community.communityValues.freedom - 0.25f, community.communityValues.freedom + 0.25f);

        family.skillGenetics.SetFamilySkills();

        family.skinColour = SetRandomColour(0f, 1f, 0.25f, 0.75f, 0.25f, 0.75f);

        return family;
    }

    public NPCInfo.Family CreateRandomFamily()
    {
        NPCInfo.Family family = new NPCInfo.Family();

        string lastName = lastNames.nameList[Random.Range(0, lastNames.nameList.Count)];

        for (int i = 0; i < possibleFamilies.Count; i++)
        {
            if (possibleFamilies[i].lastName == lastName)
            {
                lastName = lastNames.nameList[Random.Range(0, lastNames.nameList.Count)];
                i = 0;
            }
        }

        family.lastName = lastName;

        family.familyValues.privacy = Random.Range(-1f, 1f);
        family.familyValues.diversity = Random.Range(-1f, 1f);
        family.familyValues.freedom = Random.Range(-1f, 1f);

        family.skillGenetics.SetFamilySkills();

        family.skinColour = SetRandomColour(0f, 1f, 0.25f, 0.75f, 0.25f, 0.75f);

        return family;
    }

    public void SetRandomIdentityVariables(NPCInfo npc)
    {
        npc.age = SetRandomAge();
        npc.lifeStage = SetLifeStage(npc.age);
        npc.gender = SetRandomGender();
        npc.sexualPreference = SetRandomSexualPreference();
       // npc.genderPreference = SetRandomSexuality(npc.sexualPreference);
        npc.family = SetRandomFamily();

        //npc.ofAge = CheckAge(npc.age);
        //npc.hasPartner = GetAge(npc.age, 2);
        //npc.hasChildren = GetAge(npc.age, 2);

    }

    public int SetRandomAge() { return Random.Range(0, 102); }
    
    public NPCInfo.LifeStage SetLifeStage(int age)
    {
        switch (age)
        {
            case int n when n < 2:
                return NPCInfo.LifeStage.Baby;
            case int n when n >= 2 && n < 4:
                return  NPCInfo.LifeStage.Toddler;
            case int n when n >= 4 && n < 13:
                return NPCInfo.LifeStage.Child;
            case int n when n >= 13 && n < 18:
                return NPCInfo.LifeStage.Teen;
            case int n when n >= 18 && n < 30:
                return NPCInfo.LifeStage.YoungAdult;
            case int n when n >= 30 && n < 65:
                return NPCInfo.LifeStage.Adult;
            case int n when n >= 65 && n < 90:
                return NPCInfo.LifeStage.Elderly;
            case int n when n >= 90:
                return NPCInfo.LifeStage.VeryElderly;
        }

        return NPCInfo.LifeStage.Error;
    }

    public int SetRandomAge(NPCInfo.LifeStage lifeStage)
    {
        switch (lifeStage)
        {
            case NPCInfo.LifeStage.Baby:
                return Random.Range(0, 2);
            case NPCInfo.LifeStage.Toddler:
                return Random.Range(2, 4);
            case NPCInfo.LifeStage.Child:
                return Random.Range(4, 13);
            case NPCInfo.LifeStage.Teen:
                return Random.Range(13, 18);
            case NPCInfo.LifeStage.YoungAdult:
                return Random.Range(18, 30);
            case NPCInfo.LifeStage.Adult:
                return Random.Range(30, 65);
            case NPCInfo.LifeStage.Elderly:
                return Random.Range(65, 90);
            case NPCInfo.LifeStage.VeryElderly:
                return Random.Range(90, 102);
        }

        return 666;
    }

    public float SetRandomAppearance() { return Random.Range(-1f, 1f); }

    public float SetRandomSexualPreference() { return Random.Range(-1f, 1f); }

    public NPCInfo.Gender SetRandomGender()
    {
        float r = Chance.NumberPicker(3);

        switch (r)
        {
            case 0:
                return NPCInfo.Gender.Male;
            case 1:
                return NPCInfo.Gender.Female;
            case 2:
                return NPCInfo.Gender.NonBinary;

        }
        return NPCInfo.Gender.NonBinary;
    }

    //public NPCInfo.GenderPreference SetRandomSexuality(float sexualPreference)
    //{

    //    switch (sexualPreference)
    //    {
    //        case float n when n <= -0.5f: // feminine preference
    //          return  NPCInfo.GenderPreference.FeminineAttraction; 
    //        case float n when n >= -0.5f && n <= 0.5f: // no preference
    //          return  NPCInfo.GenderPreference.BiAttraction; 
    //        case float n when n >= -0.5f && n <= 0.5f: // masculine preference
    //          return  NPCInfo.GenderPreference.MasculineAttraction;
    //    }

    //    return NPCInfo.GenderPreference.BiAttraction;
    //} 
    
    public float SetRandomSexuality()
    {       
        return Random.Range(-90f, 90f);
    }

    public NPCInfo.Family SetRandomFamily()
    {
        NPCInfo.Family fam = possibleFamilies[Random.Range(0, possibleFamilies.Count)];

        if (!families.Contains(fam))
        {
            families.Add(fam);
        }
        return fam;
    }

    //public float SetSexualPreference(NPCInfo.GenderPreference sexuality)
    //{
    //    switch (sexuality)
    //    {
    //        case NPCInfo.GenderPreference.FeminineAttraction:
    //            return Random.Range(-1f, -0.5f);
    //        case NPCInfo.GenderPreference.BiAttraction:
    //            return Random.Range(-0.5f, 0.5f);
    //        case NPCInfo.GenderPreference.MasculineAttraction:
    //            return Random.Range(0.5f, 1f);
            
    //    }
    //    return Random.Range(-0.5f, 0.5f);
    //}

    public static bool CheckAge(int age)
    {
        if (age >= 18f)
        {
            return true;

        }
        else return false;
    }

    //public int CheckBeliefCompatibility(NPCBrain thisNPC, NPCBrain otherNPC)
    //{
    //    //check whether their beliefs are compatible or conflicting

    //    int indiCompatibility = Mathf.Abs(thisNPC.npcInfo.beliefs.independence + otherNPC.npcInfo.beliefs.independence);
    //    int divCompatibility = Mathf.Abs(thisNPC.npcInfo.beliefs.diversity + otherNPC.npcInfo.beliefs.diversity);
    //    int freeCompatibility = Mathf.Abs(thisNPC.npcInfo.beliefs.freedom + otherNPC.npcInfo.beliefs.freedom);

    //    int average = (indiCompatibility + divCompatibility + freeCompatibility) /3;
    //    Debug.Log("This npc = " + thisNPC.name + ", other npc = " + otherNPC.name + ", compatibility = " + average);

    //    return average;

    //}

    public NPCInfo.BeliefValues SetBeliefValues(NPCInfo.Family family)
    {
        NPCInfo.BeliefValues beliefValues = new NPCInfo.BeliefValues();

        beliefValues = family.familyValues;

        if (Chance.CoinFlip())
        {
            beliefValues.privacy = -beliefValues.privacy;
            beliefValues.diversity = -beliefValues.diversity;
            beliefValues.freedom = -beliefValues.freedom;
        }

        return beliefValues;
    }

    //public void SetRelationshipValues(NPCBrain thisNPC)
    //{
    //    NPCRelationships.Relationship newRelo = new NPCRelationships.Relationship();


    //    for (int i = 0; i < npcList.Count; i++)
    //    {
    //        if (CheckAge(npcList[i].npcInfo.age))
    //        {
    //            newRelo.relationshipStats.attraction = Mathf.Abs(thisNPC.npcInfo.sexualPreference + npcList[i].npcInfo.appearance);
    //        }
    //        else newRelo.relationshipStats.attraction = 0;

    //        newRelo.relationshipStats.compatibility = CheckBeliefCompatibility(thisNPC, npcList[i]);
    //        newRelo.relationshipStats.affection = thisNPC.npcRelationships.SetAffection(npcList[i].npcInfo);
    //        newRelo.relationshipStats.authority = 0;
    //    }



    //}

#if UNITY_EDITOR
    public void DespawnAll()
    {
        for (int i = 0; i < npcList.Count; i++)
        {
            DestroyImmediate(npcList[i].gameObject);
        }

        families = new List<NPCInfo.Family>();
        npcList = new List<NPCBrain>();
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(NPCCreator))]
public class NPCGeneratorEditor : Editor
{
    int familyNum;
    int npcNum;
    bool justOpened = true;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        NPCCreator creator = (NPCCreator)target;
        if (creator == null) return;

        GUILayout.Space(20);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Elderly NPC"))
        {
            if (Chance.CoinFlip()) creator.CreateNPC(NPCInfo.LifeStage.Elderly, creator.SetRandomGender(), creator.SetRandomAppearance(), creator.SetRandomSexualPreference(),creator.SetRandomFamily());
            else creator.CreateNPC(NPCInfo.LifeStage.VeryElderly, creator.SetRandomGender(), creator.SetRandomAppearance(), creator.SetRandomSexualPreference(), creator.SetRandomFamily());
        }

        if (GUILayout.Button("Adult NPC"))
        {
            if (Chance.CoinFlip()) creator.CreateNPC(NPCInfo.LifeStage.Adult, creator.SetRandomGender(), creator.SetRandomAppearance(), creator.SetRandomSexualPreference(), creator.SetRandomFamily());
            else creator.CreateNPC(NPCInfo.LifeStage.YoungAdult, creator.SetRandomGender(), creator.SetRandomAppearance(), creator.SetRandomSexualPreference(), creator.SetRandomFamily());
        }

        if (GUILayout.Button("Child NPC"))
        {
            switch (Random.Range(0,4))
            {
                case 0: 
                    creator.CreateNPC(NPCInfo.LifeStage.Teen, creator.SetRandomGender(), creator.SetRandomAppearance(), creator.SetRandomSexualPreference(), creator.SetRandomFamily());
                    break;
                case 1:
                    creator.CreateNPC(NPCInfo.LifeStage.Child, creator.SetRandomGender(), creator.SetRandomAppearance(), creator.SetRandomSexualPreference(), creator.SetRandomFamily());
                    break;
                case 2:
                    creator.CreateNPC(NPCInfo.LifeStage.Toddler, creator.SetRandomGender(), creator.SetRandomAppearance(), creator.SetRandomSexualPreference(), creator.SetRandomFamily());
                    break;
                case 3:
                    creator.CreateNPC(NPCInfo.LifeStage.Baby, creator.SetRandomGender(), creator.SetRandomAppearance(), creator.SetRandomSexualPreference(), creator.SetRandomFamily());
                    break;
            }
        }

        GUILayout.EndHorizontal();

        GUILayout.Space(20);

        //GUILayout.Space(20);

        if (GUILayout.Button("Generate Random NPC"))
        {
            creator.SpawnRandomNPC();
            //creator.SetRelationships();
        }

        GUILayout.BeginHorizontal();

        if (justOpened)
        {
            if (npcNum == 0 && creator.npcNum > 0)
            {
                npcNum = creator.npcNum;
            }
            //justOpened = false;
        }
        else if (npcNum != creator.npcNum)
        {
            creator.npcNum = npcNum;
        }

        npcNum = EditorGUILayout.IntField("Num of NPCs:", npcNum);

        GUILayout.Space(5);



        if (GUILayout.Button("Generate Multiple NPCs"))
        {
          
            if (npcNum > 0)
            {
                for (int i = 0; i < npcNum; i++)
                {
                    creator.SpawnRandomNPC();
                    creator.familyNum = familyNum;
                }

               
            }
            else creator.CreateNewRandomFamilies();
        }

        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        GUILayout.BeginHorizontal();

        if (justOpened)
        {
            if (familyNum == 0 && creator.familyNum > 0)
            {
                familyNum = creator.familyNum;
            }
            justOpened = false;
        }
        else if (familyNum != creator.familyNum)
        {
            creator.familyNum = familyNum;
        }


        familyNum = EditorGUILayout.IntField("Num of families:", familyNum);
        GUILayout.Space(5);



        if (GUILayout.Button("Generate Families"))
        {
            if (familyNum > 0)
            {
                creator.CreateNewRandomFamilies(familyNum);
                creator.familyNum = familyNum;
            }
            else creator.CreateNewRandomFamilies();
        }


        GUILayout.EndHorizontal();

        GUILayout.Space(20);

        if (GUILayout.Button("Despawn All NPCs"))
        {
            creator.DespawnAll();
        } 
        GUILayout.Space(20);

        //if (GUILayout.Button("Flip a coin"))
        //{
        //    Debug.Log(Chance.CoinFlip());
        //}
    }
}
#endif
