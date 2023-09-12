using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(NPCCreator))]
public class Community : MonoBehaviour
{
    NPCCreator creator;
    public MadLibNameList communityNames;
    

    public string communityName;

    public NPCInfo.BeliefValues communityValues;
    public List<BeliefConcept.BeliefStance> communityMorals;

    public int population;
    public Vector2 popRange;

    public List<Household> households;
    public List<GameObject> housePrefabs;

    //Available resources
    //Desired Resources

    // Industries
    // organisations

    public List<OrganisationController> potentialOrganisations;
    public List<OrganisationController> organisations;

    private void Start()
    {
        creator = GetComponent<NPCCreator>();
    }

    [System.Serializable]
    public class Household
    {
        public string householdName;
        public List<NPCBrain> householdMembers = new List<NPCBrain>();

        public NPCInfo.BeliefValues householdBeliefs = new NPCInfo.BeliefValues();

        public GameObject house;
    }

    //public class IndustryValues
    //{
    //    //public Organisation.Industry industry;

    //    public List<Organisation> organisations;
    //}

    public void DespawnCommunity()
    {
        if (creator.npcList.Count > 0)
            creator.DespawnAll();

        if (households.Count > 0)
        {
            foreach (Household household in households)
            {
                DestroyImmediate(household.house.gameObject);
                //households.Remove(household);
            }

            households = new List<Household>();
        }

        if (organisations.Count > 0)
        {
            foreach (OrganisationController org in organisations)
            {
                DestroyImmediate(org.gameObject);
                //households.Remove(household);
            }

            organisations = new List<OrganisationController>();
        }
    }

    public void SpawnRandomCommunity()
    {
#if UNITY_EDITOR
        if (creator == null)
            creator = GetComponent<NPCCreator>();
#endif

        DespawnCommunity();

        communityName = communityNames.GenerateName();

        // Set community belief values
        communityValues.privacy = Random.Range(-1f, 1f);
        communityValues.diversity = Random.Range(-1f, 1f);
        communityValues.freedom = Random.Range(-1f, 1f);

        // Set community conceptual beliefs based on values

        // Set community political structure based on values

        // Set population
        population = Random.Range((int)popRange.x, (int)popRange.y);

        creator.familyNum = Mathf.RoundToInt(population / 4);

        //Debug.Log("Family num = " + creator.familyNum);

        creator.CreateCommunityFamilies(this, creator.familyNum);

        // Set social heirarchy classes

        // Generate NPCs
        for (int i = 0; i < population; i++)
        {
            creator.SpawnRandomNPC();
        }
        // spawn houses, npcs in the same immediate family can share a house - children may move out when of age
        SetHouseholds();

        RemoveDuplicateHouseholds();
        
        foreach (NPCBrain brain in creator.npcList)
        {
            if (brain.npcInfo.household.householdName.Length <= 0)
            {
                brain.npcInfo.household.householdName = "Homeless";

                //Debug.Log(brain.npcInfo.fullName + " is homeless");
            }
        }

        SetHouseholdBeliefs();
        // Some npcs can be homeless

        // Set industries
        // Random for now but later based on local resources
        // Generate organisations

        SpawnOrganisations();

    }

    public Household CreateHousehold()
    {
        Household newHouse = new Household();
        newHouse.house = Instantiate(housePrefabs[Random.Range(0, housePrefabs.Count)], SpawnObjects.GenerateRandomWayPoint(), Quaternion.identity);
        Vector3 waypoint = new Vector3(newHouse.house.transform.position.x, newHouse.house.transform.localScale.y / 2, newHouse.house.transform.position.z);
        newHouse.house.transform.position = waypoint;

        Material sharedBodyMaterial = newHouse.house.GetComponent<MeshRenderer>().sharedMaterial;
        Material bodyMaterial = newHouse.house.GetComponent<MeshRenderer>().material = new Material(sharedBodyMaterial);

        bodyMaterial.color = creator.SetRandomColour();
        return newHouse;
    }

    public void SetHouseholds()
    {
        foreach (NPCBrain brain in creator.npcList)
        {
            if (brain.npcInfo.household.householdName.Length <= 0)
                if (brain.npcRelationships.partner != null)
                {
                    Household newHouse = CreateHousehold();

                    newHouse.householdMembers.Add(brain);

                    brain.npcInfo.household = newHouse;
                    brain.npcInfo.household.house = newHouse.house;

                    newHouse.householdMembers.Add(brain.npcRelationships.partner);
                    brain.npcRelationships.partner.npcInfo.household = newHouse;
                    brain.npcRelationships.partner.npcInfo.household.house = newHouse.house;

                    if (brain.npcInfo.lastName != brain.npcRelationships.partner.npcInfo.lastName && !brain.npcInfo.lastName.Contains(brain.npcRelationships.partner.npcInfo.lastName) && !brain.npcRelationships.partner.npcInfo.lastName.Contains(brain.npcInfo.lastName))
                    {
                        newHouse.householdName = brain.npcInfo.lastName + "-" + brain.npcRelationships.partner.npcInfo.lastName;
                    }
                    else newHouse.householdName = brain.npcInfo.lastName;

                    newHouse.house.name = newHouse.householdName + " Home";

                    if (brain.npcRelationships.children.Count > 0)
                    {
                        foreach (NPCBrain child in brain.npcRelationships.children)
                        {
                            if ((child.npcInfo.age > 18f && Chance.OddsOn(3)) || (child.npcInfo.age > 24f && !Chance.OddsOn(5)) || child.npcInfo.age > 30f && !Chance.OddsOn(10) || newHouse.householdMembers.Contains(child))
                            {
                                //don't add them to the household
                            }
                            else
                            {
                                newHouse.householdMembers.Add(child);
                                child.npcInfo.household = newHouse;
                                child.npcInfo.household.house = newHouse.house;
                            }
                        }
                    }
                    if (brain.npcRelationships.partner.npcRelationships.children.Count > 0)
                    {
                        foreach (NPCBrain child in brain.npcRelationships.partner.npcRelationships.children)
                        {
                            if ((child.npcInfo.age > 18f && Chance.OddsOn(3)) || (child.npcInfo.age > 24f && !Chance.OddsOn(5)) || child.npcInfo.age > 30f && !Chance.OddsOn(10) || newHouse.householdMembers.Contains(child))
                            {
                                //don't add them to the household
                            }
                            else
                            {
                                newHouse.householdMembers.Add(child);
                                child.npcInfo.household = newHouse;
                                child.npcInfo.household.house = newHouse.house;
                            }
                        }
                    }

                    households.Add(newHouse);
                }
                else if (brain.npcRelationships.children.Count > 0)
                {
                    Household newHouse = CreateHousehold();


                    newHouse.householdMembers.Add(brain);
                    newHouse.householdName = brain.npcInfo.lastName;

                    brain.npcInfo.household = newHouse;
                    newHouse.house.name = newHouse.householdName + " Home";

                    foreach (NPCBrain child in brain.npcRelationships.children)
                    {
                        if ((child.npcInfo.age > 18f && Chance.OddsOn(3)) || (child.npcInfo.age > 24f && !Chance.OddsOn(5)) || child.npcInfo.age > 30f && !Chance.OddsOn(10) || newHouse.householdMembers.Contains(child))
                        {
                            //don't add them to the household
                        }
                        else
                        {
                            newHouse.householdMembers.Add(child);
                            child.npcInfo.household = newHouse;
                            child.npcInfo.household.house = newHouse.house;
                        }
                    }

                    households.Add(newHouse);
                }
                else if (brain.npcRelationships.parent1 != null)
                {
                    if (brain.npcRelationships.parent1.npcInfo.household != null)
                        if (!brain.npcRelationships.parent1.npcInfo.household.householdMembers.Contains(brain))
                        {
                            brain.npcRelationships.parent1.npcInfo.household.householdMembers.Add(brain);
                            brain.npcInfo.household = brain.npcRelationships.parent1.npcInfo.household;
                            brain.npcInfo.household.house = brain.npcRelationships.parent1.npcInfo.household.house;
                        }
                        else
                        {
                            Household newHouse = CreateHousehold();

                            newHouse.householdMembers.Add(brain);

                            newHouse.householdName = brain.npcRelationships.parent1.npcInfo.lastName;
                            newHouse.house.name = newHouse.householdName + " Home";

                            brain.npcInfo.household = newHouse;
                            brain.npcInfo.household.house = newHouse.house;

                            newHouse.householdMembers.Add(brain.npcRelationships.parent1);
                            brain.npcRelationships.parent1.npcInfo.household = newHouse;
                            brain.npcRelationships.parent1.npcInfo.household.house = newHouse.house;

                            households.Add(newHouse);

                        }
                }
        }

    }

    public void SetHouseholdBeliefs()
    {
        foreach (Household household in households)
        {
            int numOfMembers = household.householdMembers.Count;

            foreach (NPCBrain brain in household.householdMembers)
            {
                household.householdBeliefs.diversity += brain.npcInfo.beliefs.diversity;
                household.householdBeliefs.freedom += brain.npcInfo.beliefs.freedom;
                household.householdBeliefs.privacy += brain.npcInfo.beliefs.privacy;
            }

            household.householdBeliefs.diversity = household.householdBeliefs.diversity / numOfMembers;
            household.householdBeliefs.freedom = household.householdBeliefs.freedom / numOfMembers;
            household.householdBeliefs.privacy = household.householdBeliefs.privacy / numOfMembers;
        }
    }
    public void RemoveDuplicateHouseholds()
    {
        int houseCount = households.Count;
        for (int i = 0; i < households.Count; i++)
        {
            for (int x = 0; x < households.Count; x++)
            {
                if (i != x)
                {
                    for (int h1 = 0; h1 < households[i].householdMembers.Count; h1++)
                    {
                        for (int h2 = 0; h2 < households[x].householdMembers.Count; h2++)
                        {
                            if (households[i].householdMembers[h1] == households[x].householdMembers[h2])
                            {
                                DestroyImmediate(households[x].house.gameObject);
                                households.RemoveAt(x);
                                break;
                            }
                        }

                        if (households.Count != houseCount)
                        {
                            break;
                        }
                    }
                }

                if (households.Count != houseCount)
                {
                    RemoveDuplicateHouseholds();
                }
            }
        }
    }

    public void SpawnOrganisations()
    {
        foreach(OrganisationController org in potentialOrganisations)
        {
            GameObject newOrg = Instantiate(org.gameObject, SpawnObjects.GenerateRandomWayPoint(), Quaternion.identity);
            OrganisationController controller = newOrg.GetComponent<OrganisationController>();
            organisations.Add(controller);
            controller.creator = creator;
            controller.timeController = FindObjectOfType<TimeController>();
            controller.CheckOpenJobs();
        }
    }

}

[CustomEditor(typeof(Community))]
public class CommunityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Community community = (Community)target;
        if (community == null) return;

        if (GUILayout.Button("Spawn Community"))
        {
            community.SpawnRandomCommunity();
        } 
        
        if (GUILayout.Button("Despawn Community"))
        {
            community.DespawnCommunity();
        }
        
    }
}
