//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//public class NPCGenerator : MonoBehaviour
//{
//    public NameList maleNames, femaleNames, /*nonBinaryNames,*/ lastNames;

//    public GameObject npcPrefab;

//    public int numToSpawn = 10;

//    public Vector2 personalityThresholdRange;

//    public List<NPCInfo.Family> families;

//    public List<NPCBrain> npcsSpawned;

//    [HideInInspector] public int maxNumOfChildren = 6;
   
//    // SPAWN NPC METHODS

//    public NPCInfo.Family GenerateFamily()
//    {
//        //if (families == null)
//        //{
//        //    families = new List<NPCInfo.Family>();
//        //}

//        NPCInfo.Family family = new NPCInfo.Family();

//        string lastName = lastNames.nameList[Random.Range(0, lastNames.nameList.Count)];

//        for (int i = 0; i < families.Count; i++)
//        {
//            if (families[i].lastName == lastName)
//            {
//                lastName = lastNames.nameList[Random.Range(0, lastNames.nameList.Count)];
//                i = 0;
//            }
//        }

//        family.lastName = lastName;

//        family.familyValues.independence = Random.Range(-100, 100);
//        family.familyValues.diversity = Random.Range(-100, 100);
//        family.familyValues.freedom = Random.Range(-100, 100);

//        return family;
        
//    }
//    public GameObject CreateNewNPC(int age) // random whether a partner or kids will also be spawned
//    {
//        GameObject newNPC = Instantiate(npcPrefab, Vector3.zero, Quaternion.identity);

//        NPCBrain newBrain = newNPC.GetComponent<NPCBrain>();
//        NPCInfo newInfo = newBrain.npcInfo;

//        SetGender(newInfo);
//        SetSexuality(newInfo);

//        SetFamily(newInfo);

//        SetFullName(newInfo);
//        newNPC.name = newInfo.fullName;
//        SetAge(newInfo, age);
//        SetPersonality(newInfo, newBrain);

//        npcsSpawned.Add(newBrain);

//        CheckAge(newInfo);

//        //CreatePartnerAndChildren(newInfo, newBrain);

//        return newNPC;
//    } 
    
//    public GameObject CreateSingleNPC(int age) // will only spawn one NPC, can result in orphans
//    {
//        GameObject newNPC = CreateNewNPC(age);

//        NPCInfo newInfo = newNPC.GetComponent<NPCBrain>().npcInfo;
//        newInfo.hasChildren = false;
//        newInfo.hasPartner = false;

//        return newNPC;
//    }
//    public GameObject CreateNPCWithPartner(int age) // will spawn an NPC and their partner
//    {
//        GameObject newNPC = CreateNewNPC(age);

//        NPCBrain newBrain = newNPC.GetComponent<NPCBrain>();
//        NPCInfo newInfo = newBrain.npcInfo;

//        newInfo.hasChildren = false;
//        newInfo.hasPartner = true;

//        CreatePartnerAndChildren(newInfo, newBrain);

//        return newNPC;
//    }
//    public GameObject CreateNPCWithKids(int age, int numOfChildren) // spawn a single parent and a set num of children
//    {
//        GameObject newNPC = CreateNewNPC(age);

//        NPCBrain newBrain = newNPC.GetComponent<NPCBrain>();
//        NPCInfo newInfo = newBrain.npcInfo;

//        newInfo.hasChildren = true;
//        newInfo.hasPartner = false;

//        CreatePartnerAndChildren(newInfo, newBrain, numOfChildren);

//        return newNPC;
//    }
//    public GameObject CreateNPCWithPartnerAndKids(int age, int numOfChildren) // spawn two parents and a set number of kids
//    {
//        GameObject newNPC = CreateNewNPC(age);

//        NPCBrain newBrain = newNPC.GetComponent<NPCBrain>();
//        NPCInfo newInfo = newBrain.npcInfo;

//        newInfo.hasChildren = true;
//        newInfo.hasPartner = true;

//        CreatePartnerAndChildren(newInfo, newBrain, numOfChildren);

//        return newNPC;
//    }

//    public void CreatePartnerAndChildren(NPCInfo newInfo, NPCBrain newBrain) // Create a partner and/or a random number of kids
//    {
//        if (newInfo.hasPartner)
//        {
//            int minAge;
//            if (newInfo.age < 18)
//            {
//                newInfo.hasPartner = false;
//            }
//            else if (newInfo.age >= 18)
//            {
//                minAge = newInfo.age - 10;

//                if (minAge < 18 && newInfo.age > 18)
//                {
//                    minAge = newInfo.age - 5;

//                    if (minAge < 18)
//                    {
//                        minAge = newInfo.age - 2;
//                    }
//                }

//                CreatePartnerNPC(newBrain, minAge);
//            }
//        }
//        else if (newInfo.hasChildren) // single parent child, otherwise children are spawned when creating partner
//        {
//            if (newInfo.lifeStage == NPCInfo.LifeStage.Elderly)
//            {
//                newInfo.numOfChildren = Random.Range(1, maxNumOfChildren);
//            }
//            else if (newInfo.lifeStage == NPCInfo.LifeStage.Adult)
//            {
//                newInfo.numOfChildren = Random.Range(1, maxNumOfChildren);
//            }

//            for (int i = 0; i < newInfo.numOfChildren; i++)
//            {
//                CreateChildNPC(newBrain);
//            }
//        }

//        if (newInfo.hasChildren && newInfo.numOfChildren > 1) // set sibling relationships
//        {
//            newBrain.npcRelationships.SetSiblingRelationships(newBrain);
//        }
//    } 
//    public void CreatePartnerAndChildren(NPCInfo newInfo, NPCBrain newBrain, int numOfChildren)// Create a partner and/or a  set number of kids
//    {
//        if (newInfo.hasPartner)
//        {
//            int minAge;
//            if (newInfo.age < 18)
//            {
//                newInfo.hasPartner = false;
//            }
//            else if (newInfo.age >= 18)
//            {
//                minAge = newInfo.age - 10;

//                if (minAge < 18 && newInfo.age > 18)
//                {
//                    minAge = newInfo.age - 5;

//                    if (minAge < 18)
//                    {
//                        minAge = newInfo.age - 2;
//                    }
//                }

//                CreatePartnerNPC(newBrain, minAge, numOfChildren);
//            }
//        }
//        else if (newInfo.hasChildren) // single parent child, otherwise children are spawned when creating partner
//        {
//            if (newInfo.lifeStage == NPCInfo.LifeStage.Elderly)
//            {
//                newInfo.numOfChildren = numOfChildren;
//            }
//            else if (newInfo.lifeStage == NPCInfo.LifeStage.Adult)
//            {
//                newInfo.numOfChildren = numOfChildren;
//            }

//            for (int i = 0; i < newInfo.numOfChildren; i++)
//            {
//                CreateChildNPC(newBrain);
//            }
//        }

//        if (newInfo.hasChildren && newInfo.numOfChildren > 1) // set sibling relationships
//        {
//            newBrain.npcRelationships.SetSiblingRelationships(newBrain);
//        }
//    }

//    public void CreatePartnerNPC(NPCBrain partner, int minAge) // Create a partner for the current NPC and a random number of kids
//    {
//        GameObject newNPC = Instantiate(npcPrefab, Vector3.zero, Quaternion.identity);

//        NPCBrain newBrain = newNPC.GetComponent<NPCBrain>();
//        NPCInfo newInfo = newBrain.npcInfo;

//        SetGender(newInfo, partner.npcInfo);
//        //SetSexuality(partner.npcInfo, newInfo);

//        SetFamily(newInfo);

//        if (Chance.OddsOn(2)) SetFullName(newInfo); //whether they keep maiden name or not
//        else SetFullName(newInfo, partner.npcInfo.family);

//        newNPC.name = newInfo.fullName;
//        SetAge(newInfo, minAge);
//        SetPersonality(newInfo, newBrain);



//        npcsSpawned.Add(newBrain);

//        newBrain.npcRelationships.SetPartnerRelationships(newBrain, partner);

//        newInfo.hasPartner = true;

//        if (partner.npcInfo.hasChildren) //spawn children for 2 parent couple
//        {
//            newInfo.hasChildren = true;
//            //later change this to incorporate step parents

//            newInfo.numOfChildren = Random.Range(1, maxNumOfChildren);
//            partner.npcInfo.numOfChildren = newInfo.numOfChildren;

//            for (int i = 0; i < newInfo.numOfChildren; i++)
//            {
//                CreateChildNPC(partner, newBrain);
//            }
//        }
//        else newInfo.hasChildren = false;

//        if (newInfo.hasChildren && newInfo.numOfChildren > 1) // set sibling relationships
//        {
//            newBrain.npcRelationships.SetSiblingRelationships(newBrain);
//        }
//    }
//    public void CreatePartnerNPC(NPCBrain partner, int minAge, int numOfChildren) // Create a partner for the current NPC and a set number of kids
//    {
//        GameObject newNPC = Instantiate(npcPrefab, Vector3.zero, Quaternion.identity);

//        NPCBrain newBrain = newNPC.GetComponent<NPCBrain>();
//        NPCInfo newInfo = newBrain.npcInfo;

//        SetGender(newInfo, partner.npcInfo);
//        //SetSexuality(partner.npcInfo, newInfo);

//        SetFamily(newInfo);

//        if (Chance.OddsOn(2)) SetFullName(newInfo); //whether they keep maiden name or not
//        else SetFullName(newInfo, partner.npcInfo.family);

//        newNPC.name = newInfo.fullName;
//        SetAge(newInfo, minAge);
//        SetPersonality(newInfo, newBrain);

//        npcsSpawned.Add(newBrain);

//        newBrain.npcRelationships.SetPartnerRelationships(newBrain, partner);

//        newInfo.hasPartner = true;

//        if (partner.npcInfo.hasChildren) //spawn children for 2 parent couple
//        {
//            newInfo.hasChildren = true;
//            //later change this to incorporate step parents

//            newInfo.numOfChildren = numOfChildren;
//            partner.npcInfo.numOfChildren = newInfo.numOfChildren;

//            for (int i = 0; i < newInfo.numOfChildren; i++)
//            {
//                CreateChildNPC(partner, newBrain);
//            }
//        }
//        else newInfo.hasChildren = false;

//        if (newInfo.hasChildren && newInfo.numOfChildren > 1) // set sibling relationships
//        {
//            newBrain.npcRelationships.SetSiblingRelationships(newBrain);
//        }
//    }

//    public GameObject CreateChildNPC(NPCBrain parent)
//    {
//        GameObject newNPC = Instantiate(npcPrefab, Vector3.zero, Quaternion.identity);

//        NPCBrain newBrain = newNPC.GetComponent<NPCBrain>();
//        NPCInfo newInfo = newBrain.npcInfo;

//        SetFamily(newInfo, parent.npcInfo.family);

//        SetGender(newInfo);
//        SetSexuality(newInfo);
//        SetFullName(newInfo, parent.npcInfo.family);
//        newNPC.name = newInfo.fullName;

//        newInfo.family = parent.npcInfo.family;
//        SetChildAge(newInfo, parent.npcInfo.age - 18);
//        SetPersonality(newInfo, newBrain);

//        npcsSpawned.Add(newBrain);

//        CheckAge(newInfo); //see whether they are old enough to have a partner and/or kids

//        if (newInfo.hasPartner)
//        {
//            //CreatePartnerNPC(newBrain);
//        }
//        else if (newInfo.hasChildren)
//        {
//            newInfo.numOfChildren = Random.Range(1, 6);

//            for (int i = 0; i < newInfo.numOfChildren; i++)
//            {
//                CreateChildNPC(newBrain);
//            }
//        }

//        if (newInfo.hasChildren && newInfo.numOfChildren > 1) // set sibling relationships
//        {
//            newBrain.npcRelationships.SetSiblingRelationships(newBrain);
//        }

//        return newNPC;
//    } // spawn single parent child
//    public void CreateChildNPC(NPCBrain parent1, NPCBrain parent2)
//    {
//        GameObject newNPC = Instantiate(npcPrefab, Vector3.zero, Quaternion.identity);

//        NPCBrain newBrain = newNPC.GetComponent<NPCBrain>();
//        NPCInfo newInfo = newBrain.npcInfo;

//        SetFamily(newInfo, parent1.npcInfo.family);


//        SetGender(newInfo);
//        SetSexuality(newInfo);

//        if (parent1.npcInfo.lastName == parent2.npcInfo.lastName)
//        {
//            SetFullName(newInfo, parent1.npcInfo.family);
//        }
//        else
//        {
//            int r = Random.Range(0, 2);
//            if (r == 0)
//            {
//                SetFullName(newInfo, parent1.npcInfo.family);

//            }
//            else if (r == 1)
//            {
//                SetFullName(newInfo, parent2.npcInfo.family);

//            }
//            else
//            {
//                SetFullName(newInfo, parent1.npcInfo.family);

//            }
//        }

//        newNPC.name = newInfo.fullName;

//        if (parent1.npcInfo.age > parent2.npcInfo.age) SetChildAge(newInfo, parent2.npcInfo.age - 18);
//        else SetChildAge(newInfo, parent1.npcInfo.age - 18);

//        SetPersonality(newInfo, newBrain);

//        npcsSpawned.Add(newBrain);

//        newBrain.npcRelationships.SetParentRelationships(newBrain, parent1);
//        newBrain.npcRelationships.SetParentRelationships(newBrain, parent2);

//        CheckAge(newInfo);

//        if (newInfo.hasPartner)
//        {
//            CreatePartnerAndChildren(newInfo, newBrain);
//        }
//        else if (newInfo.hasChildren)
//        {
//            newInfo.numOfChildren = Random.Range(1, maxNumOfChildren);

//            for (int i = 0; i < newInfo.numOfChildren; i++)
//            {
//                CreateChildNPC(newBrain);
//            }
//        }

//        if (newInfo.hasChildren && newInfo.numOfChildren > 1) // set sibling relationships
//        {
//            newBrain.npcRelationships.SetSiblingRelationships(newBrain);
//        }
//    } // spawn 2 parent child

//    // SET GENDER NAME & PERSONALITY VARIABLES

//    public NPCInfo.Family SetFamily(NPCInfo npc)
//    {
//        NPCInfo.Family newFam = GenerateFamily();
//        families.Add(newFam);
//        return npc.family = newFam;
//    } 
    
//    public NPCInfo.Family SetFamily(NPCInfo npc, NPCInfo.Family family)
//    {
//        return npc.family = family;
//    }
//    public NPCInfo.Sexuality SetSexuality(NPCInfo npc)
//    {
//        int r = Random.Range(0, 4);

//        if (r == 0 || r == 1) return npc.sexuality = NPCInfo.Sexuality.Straight;
//        else if (r == 2) return npc.sexuality = NPCInfo.Sexuality.Gay;
//        else return npc.sexuality = NPCInfo.Sexuality.Bi;
//    }

//    public NPCInfo.Gender SetGender(NPCInfo npc)
//    {
//        int r = Random.Range(0, 2);

//        switch (r)
//        {
//            case 0:
//                return npc.gender = NPCInfo.Gender.Male;

//            case 1:
//                return npc.gender = NPCInfo.Gender.Female;

//                //case 2:
//                //    return npc.gender = NPCInfo.Gender.NonBinary;
//        }

//        return npc.gender = NPCInfo.Gender.NonBinary;
//    }

//    public NPCInfo.Gender SetGender(NPCInfo npc, NPCInfo partner)
//    {
//        switch (partner.sexuality)
//        {
//            case NPCInfo.Sexuality.Straight:
//                if (partner.gender == NPCInfo.Gender.Male)
//                {
//                    if (Chance.CoinFlip())
//                    {
//                        npc.sexuality = NPCInfo.Sexuality.Straight;
//                    }
//                    else npc.sexuality = NPCInfo.Sexuality.Bi;

//                    return npc.gender = NPCInfo.Gender.Female;

//                }
//                else
//                {
//                    if (Chance.CoinFlip())
//                    {
//                        npc.sexuality = NPCInfo.Sexuality.Straight;
//                    }
//                    else npc.sexuality = NPCInfo.Sexuality.Bi;

//                    return npc.gender = NPCInfo.Gender.Male;
//                }
//            case NPCInfo.Sexuality.Gay:
//                if (partner.gender == NPCInfo.Gender.Male)
//                {
//                    if (Chance.CoinFlip())
//                    {
//                        npc.sexuality = NPCInfo.Sexuality.Gay;
//                    }
//                    else npc.sexuality = NPCInfo.Sexuality.Bi;

//                    return npc.gender = NPCInfo.Gender.Male;
//                }
//                else
//                {
//                    if (Chance.CoinFlip())
//                    {
//                        npc.sexuality = NPCInfo.Sexuality.Gay;
//                    }
//                    else npc.sexuality = NPCInfo.Sexuality.Bi;

//                    return npc.gender = NPCInfo.Gender.Female;
//                }
//            case NPCInfo.Sexuality.Bi:
//                SetGender(npc);

//                switch (npc.gender)
//                {
//                    case NPCInfo.Gender.Male:
//                        if (partner.gender == NPCInfo.Gender.Male)
//                        {
//                            if (Chance.CoinFlip())
//                            {
//                                npc.sexuality = NPCInfo.Sexuality.Gay;
//                            }
//                            else npc.sexuality = NPCInfo.Sexuality.Bi;

//                        }
//                        else
//                        {
//                            if (Chance.CoinFlip())
//                            {
//                                npc.sexuality = NPCInfo.Sexuality.Straight;
//                            }
//                            else npc.sexuality = NPCInfo.Sexuality.Bi;
//                        }
//                        break;
//                    case NPCInfo.Gender.Female:
//                        if (partner.gender == NPCInfo.Gender.Male)
//                        {
//                            if (Chance.CoinFlip())
//                            {
//                                npc.sexuality = NPCInfo.Sexuality.Straight;
//                            }
//                            else npc.sexuality = NPCInfo.Sexuality.Bi;

//                        }
//                        else
//                        {
//                            if (Chance.CoinFlip())
//                            {
//                                npc.sexuality = NPCInfo.Sexuality.Gay;
//                            }
//                            else npc.sexuality = NPCInfo.Sexuality.Bi;
//                        }

//                        break;
//                }

//                return npc.gender;

//        }

//        return NPCInfo.Gender.NonBinary;

//    }

//    public string SetFullName(NPCInfo npc)
//    {
//        bool hasMiddleName = Chance.OddsOn(2);
//        switch (npc.gender)
//        {
//            case NPCInfo.Gender.Male:
//                {
//                    npc.firstName = maleNames.nameList[Random.Range(0, maleNames.nameList.Count)];

//                    if (hasMiddleName)
//                    {
//                        string middleName = maleNames.nameList[Random.Range(0, maleNames.nameList.Count)];

//                        if (middleName != npc.firstName)
//                        {
//                            npc.firstName += " " + middleName;
//                        }
//                        else break;
//                    }

//                    break;

//                }

//            case NPCInfo.Gender.Female:
//                {
//                    npc.firstName = femaleNames.nameList[Random.Range(0, femaleNames.nameList.Count)];

//                    if (hasMiddleName)
//                    {
//                        string middleName = femaleNames.nameList[Random.Range(0, femaleNames.nameList.Count)];

//                        if (middleName != npc.firstName)
//                        {
//                            npc.firstName += " " + middleName;
//                        }
//                        else break;
//                    }
//                    break;
//                }

//                //case NPCInfo.Gender.NonBinary:
//                //    {

//                //        npc.firstName = nonBinaryNames.nameList[Random.Range(0, nonBinaryNames.nameList.Count)];

//                //        if (hasMiddleName)
//                //        {
//                //            string middleName = nonBinaryNames.nameList[Random.Range(0, nonBinaryNames.nameList.Count)];

//                //            if (middleName != npc.firstName)
//                //            {
//                //                npc.firstName += " " + middleName;
//                //            }
//                //            else break;
//                //        }

//                //        break;
//                //    }

//        }
//        npc.lastName = npc.family.lastName;

//        return npc.fullName = npc.firstName + " " + npc.lastName;
//    }

//    public string SetFullName(NPCInfo npc, NPCInfo.Family family)
//    {
//        bool hasMiddleName = Chance.OddsOn(2);
//        switch (npc.gender)
//        {
//            case NPCInfo.Gender.Male:
//                {
//                    npc.firstName = maleNames.nameList[Random.Range(0, maleNames.nameList.Count)];

//                    if (hasMiddleName)
//                    {
//                        string middleName = maleNames.nameList[Random.Range(0, maleNames.nameList.Count)];

//                        if (middleName != npc.firstName)
//                        {
//                            npc.firstName += " " + middleName;
//                        }
//                        else break;
//                    }

//                    break;

//                }

//            case NPCInfo.Gender.Female:
//                {
//                    npc.firstName = femaleNames.nameList[Random.Range(0, femaleNames.nameList.Count)];

//                    if (hasMiddleName)
//                    {
//                        string middleName = femaleNames.nameList[Random.Range(0, femaleNames.nameList.Count)];

//                        if (middleName != npc.firstName)
//                        {
//                            npc.firstName += " " + middleName;
//                        }
//                        else break;
//                    }
//                    break;
//                }

//                //case NPCInfo.Gender.NonBinary:
//                //    {

//                //        npc.firstName = nonBinaryNames.nameList[Random.Range(0, nonBinaryNames.nameList.Count)];

//                //        if (hasMiddleName)
//                //        {
//                //            string middleName = nonBinaryNames.nameList[Random.Range(0, nonBinaryNames.nameList.Count)];

//                //            if (middleName != npc.firstName)
//                //            {
//                //                npc.firstName += " " + middleName;
//                //            }
//                //            else break;
//                //        }

//                //        break;
//                //    }

//        }
//        npc.lastName = family.lastName;

//        return npc.fullName = npc.firstName + " " + family.lastName;
//    }

//    public void SetPersonality(NPCInfo npc, NPCBrain brain)
//    {
//        npc.personality.happinessThresholds.x = Random.Range(personalityThresholdRange.x, personalityThresholdRange.y); //happiness threshold
//        npc.personality.happinessThresholds.y = Random.Range(-personalityThresholdRange.y, -personalityThresholdRange.x); //sadness threshold

//        npc.personality.stressThresholds.x = Random.Range(personalityThresholdRange.x, personalityThresholdRange.y); //anger threshold
//        npc.personality.stressThresholds.y = Random.Range(-personalityThresholdRange.y, -personalityThresholdRange.x); //nervous threshold

//        npc.personality.shockThresholds.x = Random.Range(personalityThresholdRange.x, personalityThresholdRange.y); // surprise threshold
//        npc.personality.shockThresholds.y = Random.Range(-personalityThresholdRange.y, -personalityThresholdRange.x); // fear threshold

//        npc.personality.emotionalDisposition = brain.npcEmotions.SetEmotionalDisposition();
//    }

//    public void SetBeliefs(NPCInfo npc, NPCBrain brain)
//    {
//        //npc.beliefs.independence = 
//        //npc.beliefs.diversity = 
//        //npc.beliefs.freedom = 
//    }


//    // SET AGE VARIABLES
//    public void CheckAge(NPCInfo npc)
//    {
//        if (npc.age >= 18f)
//        {
//            npc.hasPartner = Chance.OddsOn(2);

//            if (npc.hasPartner)
//            {
//                npc.hasChildren = Chance.OddsOn(3);
//            }
//            else
//            {
//                npc.hasChildren = Chance.OddsOn(4);
//            }
//        }
//        else
//        {
//            npc.hasPartner = false;
//            npc.hasChildren = false;
//        }
//    }

//    public int SetAge(NPCInfo npc, int minAge)
//    {
//        npc.age = Random.Range(minAge, 75);

//        SetLifeStage(npc);

//        return npc.age;
//    }

//    public int SetChildAge(NPCInfo npc, int maxAge)
//    {
//        npc.age = Random.Range(0, maxAge);

//        SetLifeStage(npc);

//        return npc.age;
//    }

//    public NPCInfo.LifeStage SetLifeStage(NPCInfo npc)
//    {
//        switch (npc.age)
//        {
//            case int n when (n >= 90):
//               return npc.lifeStage = NPCInfo.LifeStage.VeryElderly;
//            case int n when (n >= 60):
//                return npc.lifeStage = NPCInfo.LifeStage.Elderly;
//            case int n when (n >= 30 && n <= 59):
//                return npc.lifeStage = NPCInfo.LifeStage.Adult;
//            case int n when (n >= 20 && n <= 29):
//                return npc.lifeStage = NPCInfo.LifeStage.YoungAdult;
//            case int n when (n >= 13 && n <= 19):
//                return npc.lifeStage = NPCInfo.LifeStage.Teen;
//            case int n when (n >= 5 && n <= 12):
//                return npc.lifeStage = NPCInfo.LifeStage.Child;
//            case int n when (n >= 2 && n <= 4):
//                return npc.lifeStage = NPCInfo.LifeStage.Toddler;
//            case int n when (n >= 0 && n <= 3):
//                return npc.lifeStage = NPCInfo.LifeStage.Toddler;
//        }

//        return npc.lifeStage = NPCInfo.LifeStage.Error;
//    }

//#if UNITY_EDITOR
//    public void DespawnAll()
//    {
//        for (int i = 0; i < npcsSpawned.Count; i++)
//        {
//           DestroyImmediate(npcsSpawned[i].gameObject);
//        }

//        families = new List<NPCInfo.Family>();
//        npcsSpawned = new List<NPCBrain>();
//    }
//#endif
//}

//#if UNITY_EDITOR
//[CustomEditor(typeof(NPCGenerator))]
//public class NPCGeneratorEditor : Editor
//{
//    int numOfKids;
//    bool oneParent;

//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        NPCGenerator generator = (NPCGenerator)target;
//        if (generator == null) return;

//        GUILayout.Space(20);

//        GUILayout.BeginHorizontal();

//        if (GUILayout.Button("Elderly NPC"))
//        {
//            int r = Random.Range(60, 102);

//            generator.CreateSingleNPC(r);

//        }

//        if (GUILayout.Button("Adult NPC"))
//        {
//            int r = Random.Range(20, 59);

//            generator.CreateSingleNPC(r);
//        }

//        if (GUILayout.Button("Child NPC"))
//        {
//            int r = Random.Range(2, 18);

//            generator.CreateSingleNPC(r);
//        }

//        if (GUILayout.Button("NPC Couple"))
//        {

//            generator.CreateNPCWithPartner(20);
//        }
//        GUILayout.EndHorizontal();

//        GUILayout.Space(20);


//        //GUILayout.BeginVertical();
//        //EditorGUILayout.LabelField("Num of kids:");
//        numOfKids = EditorGUILayout.IntField("Num of kids:", numOfKids);
//        //GUILayout.EndVertical();
//        GUILayout.Space(5);

//        GUILayout.BeginHorizontal();


//        //GUILayout.BeginVertical();
//        oneParent = EditorGUILayout.Toggle("Single parent?:", oneParent);

//        GUILayout.Space(-100);

//        if (GUILayout.Button("Spawn Family"))
//        {

//            if (numOfKids == 0)
//            {
//                numOfKids = Random.Range(0, generator.maxNumOfChildren);
//            }

//            if (!oneParent)
//            {
//                generator.CreateNPCWithPartnerAndKids(20, numOfKids);
//            }
//            else
//            {
//                generator.CreateNPCWithKids(20, numOfKids);
//            }
//        }

//        //GUILayout.EndVertical();
//        GUILayout.EndHorizontal();

//        GUILayout.Space(20);

//        if (GUILayout.Button("Despawn All NPCs"))
//        {
//            generator.DespawnAll();
//        }
//    }
//}
//#endif
