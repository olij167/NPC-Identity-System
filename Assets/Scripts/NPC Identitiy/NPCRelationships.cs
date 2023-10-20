using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

[System.Serializable]
public class NPCRelationships
{
    //[System.Serializable]
    //public class Reputation // how society views the npc
    //{
    //    public float fame; //positive reputation
    //    public float infamy; //negative reputation


    //}

    //public Reputation reputation;

    public NPCBrain parent1, parent2, partner;
    public List<NPCBrain> siblings;
    public List<NPCBrain> children;

    [System.Serializable]
    public class Relationship
    {
        public NPCBrain otherNPC;

        public int otherNPCAge;

        public RelationshipVariables relationshipStats = new RelationshipVariables();

        public RelationType relationType;
        public Affiliation affiliation;
    }

    public List<Relationship> relationships;

    [System.Serializable]
    public enum Affiliation
    {
        None, Parent, StepParent, Sibling, HalfSibling, StepSibling, Child, StepChild, ParentsSibling, Cousin, SiblingsChild, Colleague, Friend, Neighbour, Partner, Rival, Boss, Employee, Mentor, Student
    }

    [System.Serializable]
    public enum RelationType //Why they know/ interact with the other npc
    {
        Stranger, Family, Lover, Friend, Occupational, Mentorship, Acquaintance
    }

    [System.Serializable]
    public enum Sentiment // how they feel towards the other npc
    {
        Indifferent, Friendly, Admires, Flirty, Attracted, Love, Dislike, Aggressive, Hate, LoveHate
    }

    //[System.Serializable]
    //public class RelationshipWithNPC
    //{
    //    public NPCInfo otherNPC;

    //    public RelationshipVariables relationshipVariables;

    //    public string feelingsAboutNPC;
    //}

    [System.Serializable]
    public class RelationshipVariables
    {
        //PRIMRARY VARIABLES - the NPCs identities
        [Range(0f, 1f)] public float attraction; //sexual attraction - Based on what features the npc is attracted to
        [Range(-1f, 1f)] public float compatibility; //based on belief values
        //[Range(0f, 1f)] public float familiarity; //based on relation & how long they have known each other
        //[Range(0f, 100f)] public float authority; // The power dynamic between the NPCs - based on relation, age, occupation, and...


        //SECONDARY VARIABLES - the relationship dynamic
        [Range(-1f, 1f)] public float hostility; // how agressive they are towards the other NPC - based on compatibility, attraction, extroversion, and familiarity
        [Range(-1f, 1f)] public float affection; // how much they care about the other NPC - based on compatibility, hostility


        //TERTIARY VARIABLES 
        [Range(0f, 1f)] public float romanticPotential; // based on attraction and affection;

        //[Range(-100f, 100f)] public float trust; //Will they rely on the other NPC? - based on familiarity, compatibility, and...
        //[Range(-100f, 100f)] public float respect; // How do they treat the other NPC? - based on compatibility, authority, hostility, and...
    }

    public Relationship SetRelationshipValues(NPCBrain thisNPC, NPCBrain otherNPC)
    {
        Relationship newRelo = new Relationship();

        newRelo.otherNPC = otherNPC;
        newRelo.otherNPCAge = otherNPC.npcInfo.age;

        newRelo.relationshipStats.compatibility = CheckBeliefCompatibility(thisNPC, otherNPC, newRelo);


        if (NPCCreator.CheckAge(otherNPC.npcInfo.age) && otherNPC.npcInfo.family != thisNPC.npcInfo.family)
        {
            newRelo.relationshipStats.attraction = Mathf.Clamp01(Mathf.Abs(thisNPC.npcInfo.sexualPreference + otherNPC.npcInfo.appearance));
        }
        else newRelo.relationshipStats.attraction = 0;

        thisNPC.npcRelationships.SetHostility(thisNPC, newRelo);

        newRelo.relationshipStats.affection = thisNPC.npcRelationships.SetAffection(newRelo);

        newRelo.relationshipStats.romanticPotential = CheckRomanticPotential(thisNPC, otherNPC, newRelo);

        SetRelationType(thisNPC, otherNPC, newRelo);
        SetAffiliation(thisNPC, otherNPC, newRelo);
        //SetRelation(thisNPC, otherNPC, newRelo);

        // create a "personality filter" to be applied after the base relationship values are set comparing both NPC's personality traits
        return newRelo;
    }

    //public RelationshipVariables SetRelationshipVariables(NPCInfo thisNPC, Relationship relo)
    //{
    //    relo.relationshipStats.compatibility = SetCompatibility(relo.otherNPC.npcInfo);

    //    relo.relationshipStats.affection = SetAffection(relo.otherNPC.npcInfo);
    //    relo.relationshipStats.attraction = SetAttraction(thisNPC, relo.otherNPC.npcInfo);
    //    relo.relationshipStats.comfort = SetComfort(relo.otherNPC.npcInfo);
    //    relo.relationshipStats.trust = SetTrust(relo.otherNPC.npcInfo);
    //    relo.relationshipStats.respect = SetRespect(relo.otherNPC.npcInfo);
    //    relo.relationshipStats.authority = SetAuthority(relo.otherNPC.npcInfo);
    //    relo.relationshipStats.hostility = SetHostility(relo.otherNPC.npcInfo);


    //    return relo.relationshipStats;
    //}


    public float SetHostility(NPCBrain thisNPC, Relationship newRelo)
    {
        return newRelo.relationshipStats.hostility = Mathf.Clamp((-newRelo.relationshipStats.compatibility + -newRelo.relationshipStats.attraction + /*newRelo.relationshipStats.familiarity +*/ thisNPC.npcEmotions.disposition.extroversion) / 2, -1f, 1f);
    }

    public float SetAffection(Relationship newRelo)
    {
        return newRelo.relationshipStats.affection = Mathf.Clamp((newRelo.relationshipStats.compatibility + newRelo.relationshipStats.hostility /*+ newRelo.relationshipStats.familiarity*/) / 2, -1f, 1f);

    }

    public float CheckRomanticPotential(NPCBrain thisNPC, NPCBrain otherNPC, Relationship newRelo)
    {

        if (thisNPC.npcInfo.family != otherNPC.npcInfo.family)
        {
            for (int i = 0; i < otherNPC.npcRelationships.relationships.Count; i++)
            {
                if (thisNPC == otherNPC.npcRelationships.relationships[i].otherNPC)
                {
                    return Mathf.Clamp01((newRelo.relationshipStats.attraction + newRelo.relationshipStats.compatibility + otherNPC.npcRelationships.relationships[i].relationshipStats.attraction + otherNPC.npcRelationships.relationships[i].relationshipStats.compatibility) / 4);
                }
            }
        }

        return 0;
    }

    //public bool CheckAttractionPotential(NPCInfo thisNPC, NPCInfo otherNPC)
    //{
    //    for (int i = 0; i < relationships.Count; i++)
    //    {
    //        if (otherNPC == relationships[i].otherNPC.npcInfo)
    //        {
    //            if (thisNPC.family != otherNPC.family)
    //            {
    //                switch (relationships[i].relationType)
    //                {
    //                    case RelationType.Family:
    //                        return relationships[i].relationshipStats.attraction = 0;
    //                    case RelationType.Friend:
    //                        if (CheckAttractionPotential(thisNPC, otherNPC))
    //                        {
    //                            return relationships[i].relationshipStats.attraction = Mathf.Abs(thisNPC.sexualPreference + otherNPC.appearance);
    //                        }
    //                        else return Random.Range(0, 100);
    //                    case RelationType.Occupation:
    //                        if (CheckAttractionPotential(thisNPC, otherNPC))
    //                        {
    //                            return relationships[i].relationshipStats.attraction = Mathf.Abs(thisNPC.sexualPreference + otherNPC.appearance);
    //                        }
    //                        else return Random.Range(0, 100);
    //                    case RelationType.Acquaintance:
    //                        if (CheckAttractionPotential(thisNPC, otherNPC))
    //                        {
    //                            return relationships[i].relationshipStats.attraction = Mathf.Abs(thisNPC.sexualPreference + otherNPC.appearance);
    //                        }
    //                        else return Random.Range(0, 100);
    //                    case RelationType.Stranger:
    //                        if (CheckAttractionPotential(thisNPC, otherNPC))
    //                        {
    //                            return relationships[i].relationshipStats.attraction = Mathf.Abs(thisNPC.sexualPreference + otherNPC.appearance);
    //                        }
    //                        else return Random.Range(0, 100);
    //                }
    //            }
    //        }
    //    }

    //    return false;
    //}

    //public bool CheckSexualAllignment(NPCInfo thisNPC, NPCInfo otherNPC) // check whether thisNPC is attracted to the otherNPC's features
    //{
    //    switch (thisNPC.sexualPreference)
    //    {
    //        case float n when n > 0.75f && n <= 1f:
    //            if (otherNPC.appearance > 0.75f)
    //            {
    //                return true;
    //            }
    //            else return false;

    //        case float n when n > 0.25f && n <= 0.75f:
    //            if (otherNPC.appearance > 0.25f && otherNPC.appearance <= 0.75f)
    //            {
    //                return true;
    //            }
    //            else return false;
    //        case float n when n >= -0.25f && n <= 0.25f:
    //            if (otherNPC.appearance >= -0.25f && otherNPC.appearance <= 0.25f)
    //            {
    //                return true;
    //            }
    //            else return false;
    //        case float n when n <= -0.75f && n > -0.25f:
    //            if (otherNPC.appearance <= -0.75f && otherNPC.appearance > -0.25f)
    //            {
    //                return true;
    //            }
    //            else return false;
    //        case float n when n <= -1f && n > -0.75f:
    //            if (otherNPC.appearance > -0.75f)
    //            {
    //                return true;
    //            }
    //            else return false;
    //    }

    //    return false;
    //}
    

    public float CheckBeliefCompatibility(NPCBrain thisNPC, NPCBrain otherNPC, Relationship newRelo)
    {
        //check whether their beliefs are compatible or conflicting

        float indiCompatibility = thisNPC.npcInfo.beliefs.privacy + otherNPC.npcInfo.beliefs.privacy;
        float divCompatibility = thisNPC.npcInfo.beliefs.diversity + otherNPC.npcInfo.beliefs.diversity;
        float freeCompatibility = thisNPC.npcInfo.beliefs.freedom + otherNPC.npcInfo.beliefs.freedom;

        float average = (indiCompatibility + divCompatibility + freeCompatibility) / 3;
        //Debug.Log("This npc = " + thisNPC.name + ", other npc = " + otherNPC.name + ", compatibility = " + average);

        return newRelo.relationshipStats.compatibility = Mathf.Clamp(average, -1, 1);

    }

    public RelationType SetRelationType(NPCBrain thisNPC, NPCBrain otherNPC, Relationship relationship)
    {
        for (int i = 0; i < otherNPC.npcRelationships.relationships.Count; i++)
        {
            if (otherNPC.npcRelationships.relationships[i].otherNPC == thisNPC)
            {
                if (otherNPC.npcRelationships.relationships[i].relationType == RelationType.Lover && partner == null)
                {
                    partner = otherNPC;
                }

                return relationship.relationType = otherNPC.npcRelationships.relationships[i].relationType;
            }
        }

        if (thisNPC.npcInfo.family == otherNPC.npcInfo.family)
        {
            return relationship.relationType = RelationType.Family;
        }


        if (NPCCreator.CheckAge(thisNPC.npcInfo.age))
        {
            if (NPCCreator.CheckAge(otherNPC.npcInfo.age)) 
            {
                if (Chance.Percentage(relationship.relationshipStats.romanticPotential + 0.25f * 100) && partner == null && otherNPC.npcRelationships.partner == null)
                {
                    //Debug.Log("Lovers");
                    partner = otherNPC;

                    return relationship.relationType = RelationType.Lover;
                }
                
            }
        }

        if (Chance.Percentage(100 * relationship.relationshipStats.compatibility))
        {
            //Debug.Log("Friends");
            return relationship.relationType = RelationType.Friend;
        }
            
        if (Chance.CoinFlip())
        {
            //Debug.Log("Aquaintances");
            return relationship.relationType = RelationType.Acquaintance;
        }
        else return relationship.relationType = RelationType.Stranger;
    }

    public Affiliation SetAffiliation(NPCBrain thisNPC, NPCBrain otherNPC, Relationship relationship)
    {
        if (thisNPC != otherNPC)
        {
            if (relationship.relationType == RelationType.Lover && thisNPC.npcRelationships.partner == null && otherNPC.npcRelationships.partner == null)
            {
                partner = otherNPC;
                otherNPC.npcRelationships.partner = thisNPC;

                return relationship.affiliation = Affiliation.Partner;
            }

            if (otherNPC == partner)
            {
                return relationship.affiliation = Affiliation.Partner;
            }

            if (thisNPC.npcInfo.family == otherNPC.npcInfo.family)
            {
                if (otherNPC.npcInfo.age - 18 >= thisNPC.npcInfo.age) // if the other npc is at least 18 years older
                {
                    if (parent1 == null) // check if this npc needs a parent
                    {
                        parent1 = otherNPC;
                        if (!otherNPC.npcRelationships.children.Contains(otherNPC))
                            otherNPC.npcRelationships.children.Add(thisNPC);

                        //if (otherNPC.npcRelationships.partner != null) // if the parent has a partner they get two parents
                        //{
                        //    parent2 = otherNPC.npcRelationships.partner;
                        //    if (!otherNPC.npcRelationships.partner.npcRelationships.children.Contains(otherNPC))
                        //        otherNPC.npcRelationships.partner.npcRelationships.children.Add(thisNPC);

                        //}

                        return relationship.affiliation = Affiliation.Child; // set child affiliation for first parent
                    }
                }
                else if (thisNPC.npcInfo.age - 18 >= otherNPC.npcInfo.age) // if this npc is at least 18 years older, do the above but reversed
                {
                    if (otherNPC.npcRelationships.parent1 == null)
                    {
                        otherNPC.npcRelationships.parent1 = thisNPC;
                        if (!children.Contains(otherNPC))
                            children.Add(otherNPC);

                        return relationship.affiliation = Affiliation.Parent;
                    }
                }
            }

            if (parent1 != null && parent1.npcRelationships.partner != null && parent2 == null)
            {

                parent2 = parent1.npcRelationships.partner;
                if (!parent2.npcRelationships.children.Contains(thisNPC))
                    parent2.npcRelationships.children.Add(thisNPC);

                return relationship.affiliation = Affiliation.Child;
            }

            if (children.Contains(otherNPC) || partner != null && partner.npcRelationships.children.Contains(otherNPC))
            {
                if (thisNPC != otherNPC.npcRelationships.parent1)
                {
                    otherNPC.npcRelationships.parent2 = thisNPC;

                    if (!children.Contains(otherNPC))
                        children.Add(otherNPC);

                    return relationship.affiliation = Affiliation.Parent;
                }
            }

            if (parent1 != null)
            {
                if (parent1.npcRelationships.children != null)
                {
                    foreach (NPCBrain child in parent1.npcRelationships.children)
                    {
                        if (child != thisNPC)
                        {
                            if (!siblings.Contains(child))
                                siblings.Add(child);
                        }
                    }
                }

                if (siblings.Contains(otherNPC))
                    return relationship.affiliation = Affiliation.Sibling;

                if (parent1.npcRelationships.siblings != null && parent1.npcRelationships.siblings.Contains(otherNPC))
                {
                    return relationship.affiliation = Affiliation.ParentsSibling;
                }

                if (parent1.npcRelationships.siblings != null)
                    foreach (NPCBrain sibling in parent1.npcRelationships.siblings)
                        if (sibling.npcRelationships.children.Contains(otherNPC))
                        {
                            return relationship.affiliation = Affiliation.SiblingsChild;
                        }

                for (int i = 0; i < parent1.npcRelationships.siblings.Count; i++)
                {
                    if (parent1.npcRelationships.siblings != null && parent1.npcRelationships.siblings[i].npcRelationships.children.Contains(otherNPC))
                    {
                        return relationship.affiliation = Affiliation.Cousin;
                    }
                }

                if (parent2 != null)
                {
                    if (parent2.npcRelationships.children != null)
                    {
                        foreach (NPCBrain child in parent2.npcRelationships.children)
                        {
                            if (child != thisNPC)
                            {
                                if (!siblings.Contains(child))
                                    siblings.Add(child);
                            }
                        }
                    }


                    if (otherNPC.npcRelationships.siblings.Contains(thisNPC) || siblings.Contains(otherNPC))
                    {
                        if (!siblings.Contains(otherNPC))
                            siblings.Add(otherNPC);

                        return relationship.affiliation = Affiliation.Sibling;

                    }

                    if (parent2.npcRelationships.siblings != null && parent2.npcRelationships.siblings.Contains(otherNPC))
                    {
                        return relationship.affiliation = Affiliation.ParentsSibling;
                    }

                    if (parent2.npcRelationships.siblings != null)
                        foreach (NPCBrain sibling in parent2.npcRelationships.siblings)
                            if (sibling.npcRelationships.children.Contains(otherNPC))
                            {
                                return relationship.affiliation = Affiliation.SiblingsChild;
                            }

                    for (int i = 0; i < parent2.npcRelationships.siblings.Count; i++)
                    {
                        if (parent2.npcRelationships.siblings != null && parent2.npcRelationships.siblings[i].npcRelationships.children.Contains(otherNPC))
                        {
                            return relationship.affiliation = Affiliation.Cousin;
                        }
                    }
                }


            }

            

            if (relationship.relationType == RelationType.Friend)
            {
                return relationship.affiliation = Affiliation.Friend;
            }
        }

        return relationship.affiliation = Affiliation.None;

    }


    //public void SetRelation(NPCBrain thisNPC, NPCBrain otherNPC, Relationship relationship)
    //{
    //    for (int i = 0; i < otherNPC.npcRelationships.relationships.Count; i++)
    //    {
    //        if (otherNPC.npcRelationships.relationships[i].otherNPC == thisNPC)
    //        {
    //            //return otherNPC.npcRelationships.relationships[i].relationType;
    //            switch (otherNPC.npcRelationships.relationships[i].affiliation)
    //            {
    //                case Affiliation.Parent:
    //                    if (thisNPC.npcRelationships.parent1 == null)
    //                    {
    //                        thisNPC.npcRelationships.parent1 = otherNPC;
    //                    }
    //                    else if (thisNPC.npcRelationships.parent2 == null && otherNPC.npcRelationships.partner == thisNPC.npcRelationships.parent1)
    //                    {
    //                        thisNPC.npcRelationships.parent2 = otherNPC;
    //                    }
    //                    relationship.affiliation = Affiliation.Child;
    //                    return;
    //                case Affiliation.Sibling:
    //                    thisNPC.npcRelationships.siblings.Add(otherNPC);
    //                    relationship.affiliation = Affiliation.Sibling;
    //                    return;
    //                case Affiliation.HalfSibling:
    //                    thisNPC.npcRelationships.siblings.Add(otherNPC);
    //                    relationship.affiliation = Affiliation.HalfSibling;
    //                    return;
    //                case Affiliation.Child:
    //                    relationship.affiliation = Affiliation.Parent;
    //                    thisNPC.npcRelationships.children.Add(otherNPC);
    //                    return;
    //                case Affiliation.ParentsSibling:
    //                    relationship.affiliation = Affiliation.SiblingsChild;
    //                    return;
    //                case Affiliation.SiblingsChild:
    //                    relationship.affiliation = Affiliation.ParentsSibling;
    //                    return;
    //                case Affiliation.Cousin:
    //                    relationship.affiliation = Affiliation.Cousin;
    //                    return;
    //                case Affiliation.Partner:
    //                    relationship.affiliation = Affiliation.Partner;
    //                    return;
    //                //case Affiliation.Mentor:
    //                //    relationship.affiliation = Affiliation.Student;
    //                //    return;
    //                //case Affiliation.Student:
    //                //    relationship.affiliation = Affiliation.Mentor;
    //                //    return;
    //                //case Affiliation.Boss:
    //                //    relationship.affiliation = Affiliation.Employee;
    //                //    return;
    //                //case Affiliation.Employee:
    //                //    relationship.affiliation = Affiliation.Boss;
    //                //    return;
    //                //case Affiliation.Colleague:
    //                //    relationship.affiliation = Affiliation.Colleague;
    //                //    return;
    //            }
    //        }
    //    }


    //    switch (relationship.relationType)
    //    {
    //        case RelationType.Family:
    //            {

    //                if (thisNPC.npcRelationships.parent1 == null && otherNPC.npcInfo.age - 18 >= thisNPC.npcInfo.age) // set parents from child npc
    //                {
    //                    thisNPC.npcRelationships.parent1 = otherNPC;
    //                    otherNPC.npcRelationships.children.Add(thisNPC);
    //                    relationship.affiliation = Affiliation.Child;
    //                    return;
    //                }

    //                if (thisNPC.npcInfo.age - 18 >= otherNPC.npcInfo.age && otherNPC.npcRelationships.parent1 == null)
    //                {
    //                    otherNPC.npcRelationships.parent1 = thisNPC;
    //                    thisNPC.npcRelationships.children.Add(otherNPC);
    //                    relationship.affiliation = Affiliation.Parent;
    //                }

    //                if (parent1 != null && parent2 == null && parent1.npcRelationships.partner != null)
    //                {
    //                    parent2 = parent1.npcRelationships.partner;
    //                }

    //                if (otherNPC == parent2)
    //                {
    //                    otherNPC.npcRelationships.children.Add(thisNPC);
    //                    relationship.affiliation = Affiliation.Child;
    //                    return;
    //                }

    //                if (otherNPC == (thisNPC.npcRelationships.parent1 || thisNPC.npcRelationships.parent2))
    //                {
    //                    relationship.affiliation = Affiliation.Parent;

    //                    if (!otherNPC.npcRelationships.children.Contains(thisNPC))
    //                    {
    //                        otherNPC.npcRelationships.children.Add(thisNPC);
    //                    }
    //                }

    //                if (thisNPC.npcRelationships.children.Contains(otherNPC)) // set children from parent npcs
    //                {
    //                    if (otherNPC.npcRelationships.parent1 == null)
    //                    {
    //                        otherNPC.npcRelationships.parent1 = thisNPC;
    //                        relationship.affiliation = Affiliation.Child;
    //                    }
    //                    else if (otherNPC.npcRelationships.parent2 == null)
    //                    {
    //                        otherNPC.npcRelationships.parent2 = thisNPC;
    //                        relationship.affiliation = Affiliation.Child;
    //                    }
    //                    else thisNPC.npcRelationships.children.Remove(otherNPC);
    //                }

    //                if ((otherNPC.npcRelationships.parent1 || otherNPC.npcRelationships.parent2) == (thisNPC.npcRelationships.parent1 || thisNPC.npcRelationships.parent2) && thisNPC.npcRelationships.parent1 != null)
    //                {
    //                    thisNPC.npcRelationships.siblings.Add(otherNPC);
    //                    if ((otherNPC.npcRelationships.parent1 && otherNPC.npcRelationships.parent2) == (thisNPC.npcRelationships.parent1 && thisNPC.npcRelationships.parent2))
    //                    {
    //                        relationship.affiliation = Affiliation.Sibling;
    //                    }
    //                    else relationship.affiliation = Affiliation.HalfSibling;

    //                    return;
    //                }

    //                if (otherNPC.npcRelationships.siblings.Contains(parent1) || otherNPC.npcRelationships.siblings.Contains(parent2))
    //                {
    //                    relationship.affiliation = Affiliation.ParentsSibling;
    //                    return;
    //                }
    //                if (thisNPC.npcRelationships.siblings.Contains(otherNPC.npcRelationships.parent1) || thisNPC.npcRelationships.siblings.Contains(otherNPC.npcRelationships.parent2))
    //                {
    //                    relationship.affiliation = Affiliation.SiblingsChild;
    //                    return;
    //                }

    //                if (thisNPC.npcRelationships.parent1.npcRelationships.siblings.Contains(otherNPC.npcRelationships.parent1) || thisNPC.npcRelationships.parent1.npcRelationships.siblings.Contains(otherNPC.npcRelationships.parent2) ||
    //                    thisNPC.npcRelationships.parent2.npcRelationships.siblings.Contains(otherNPC.npcRelationships.parent1) || thisNPC.npcRelationships.parent2.npcRelationships.siblings.Contains(otherNPC.npcRelationships.parent2))
    //                {
    //                    relationship.affiliation = Affiliation.Cousin;
    //                    return;
    //                }
    //                return;
    //            }
    //        case RelationType.Lover:
                
    //                thisNPC.npcRelationships.partner = otherNPC;
    //                relationship.affiliation = Affiliation.Partner;
    //            return;
                
    //        case RelationType.Friend:
    //            relationship.affiliation = Affiliation.Friend;
    //            return;

    //            //aquaintances - neighbours, mutuals, 
    //            //occupational
    //            //mentorship
    //    }

    //    relationship.affiliation = Affiliation.None;
    //}


    //public void CheckCurrentFamilyRelationships(NPCBrain thisNPC, NPCBrain otherNPC)
    //{
    //    int parentNum = 0;
    //    bool hasPartner;

    //    if (thisNPC.npcInfo.family.familyMembers.Contains(otherNPC))
    //        {

    //        for (int i = 0; i < thisNPC.npcRelationships.relationships.Count; i++)
    //        {
    //            if (relationships[i].relation == Relation.Parent)
    //            {
    //                parentNum += 1;
    //            }
    //            if (relationships[i].relation == Relation.Partner)
    //            {
    //                hasPartner = true;
    //            }

    //            if (i + 1 > thisNPC.npcRelationships.relationships.Count)
    //            {
    //                if (otherNPC.npcInfo.age - 18 >= thisNPC.npcInfo.age )
    //                {

    //                }
    //            }
    //        }
    //    }
    //}

    //public void SetFamilyRelationShips(NPCBrain thisNPC)
    //{
    //    List<NPCBrain> eligibleParents = new List<NPCBrain>();
    //    List<NPCBrain> siblings = new List<NPCBrain>();

    //    foreach (NPCBrain relative in thisNPC.npcInfo.family.familyMembers)
    //    { 
    //        if (relative != thisNPC)
    //        {
    //            if (relative.npcInfo.age - 18 > thisNPC.npcInfo.age)
    //            {
    //                if (!eligibleParents.Contains(relative))
    //                {
    //                    eligibleParents.Add(relative);
    //                }
    //            }
    //        }
    //    }

    //    for (int i = 0; i < eligibleParents.Count; i++)
    //    {
    //        NPCBrain parentNPC = eligibleParents[Random.Range(0, eligibleParents.Count)];
    //        SetParentRelationships(thisNPC, parentNPC);

    //        foreach(Relationship relo in parentNPC.npcRelationships.relationships)
    //        {
    //            if (relo.relation == Relation.Partner)
    //            {
    //                SetParentRelationships(thisNPC, relo.otherNPC);
    //            }

    //            if (relo.relation == Relation.Child)
    //            {
    //                //SetSi
    //                siblings.Add(relo.otherNPC);
    //            }

    //            //set parents siblings/ siblings child  relationships
    //            //set cousin relationships
    //            //if ()
    //        }
    //    }
    //}

    //public void SetPartnerRelationships(NPCBrain newBrain, NPCBrain partner)
    //{
    //    Relationship newRelationship = new Relationship();
    //    newRelationship.otherNPC = partner;
    //    newRelationship.otherNPCAge = partner.npcInfo.age;

    //    newRelationship.relationType = RelationType.Family;
    //    newRelationship.relation = Relation.Partner;


    //    newBrain.npcRelationships.relationships.Add(newRelationship);

    //    newRelationship.relationshipStats = SetRelationshipVariables(newBrain.npcInfo, newRelationship);


    //    newRelationship = new Relationship();
    //    newRelationship.otherNPC = newBrain;
    //    newRelationship.otherNPCAge = newBrain.npcInfo.age;

    //    newRelationship.relationType = RelationType.Family;
    //    newRelationship.relation = Relation.Partner;
    //    newRelationship.relationshipStats = SetRelationshipVariables(partner.npcInfo, newRelationship);


    //    partner.npcRelationships.relationships.Add(newRelationship);



    //}
    //public void SetParentRelationships(NPCBrain newBrain, NPCBrain parent)
    //{
    //   Relationship newRelationship = new Relationship();

    //    newRelationship.otherNPC = parent;
    //    newRelationship.otherNPCAge = parent.npcInfo.age;

    //    newRelationship.relationType = RelationType.Family;
    //    newRelationship.relation = Relation.Parent;
    //    newRelationship.relationshipStats = SetRelationshipVariables(newBrain.npcInfo, newRelationship);



    //    newBrain.npcRelationships.relationships.Add(newRelationship);



    //    newRelationship = new Relationship();
    //    newRelationship.otherNPC = newBrain;
    //    newRelationship.otherNPCAge = newBrain.npcInfo.age;

    //    newRelationship.relationType = RelationType.Family;
    //    newRelationship.relation = Relation.Child;
    //    newRelationship.relationshipStats = SetRelationshipVariables(parent.npcInfo, newRelationship);



    //    parent.npcRelationships.relationships.Add(newRelationship);



    //}
    //public void SetSiblingRelationships(NPCBrain newBrain)
    //{
    //    List<NPCBrain> listOfChildren = new List<NPCBrain>();
    //    foreach (Relationship relo in newBrain.npcRelationships.relationships)
    //    {
    //        if (relo.relation == Relation.Child)
    //        {
    //            listOfChildren.Add(relo.otherNPC);
    //        }
    //    }

    //    foreach (NPCBrain brain in listOfChildren)
    //    {
    //        for (int i = 0; i < listOfChildren.Count; i++)
    //        {
    //            if (listOfChildren[i] != brain)
    //            {
    //                Relationship newRelationship = new Relationship();

    //                newRelationship.otherNPC = listOfChildren[i];
    //                newRelationship.otherNPCAge = listOfChildren[i].npcInfo.age;
    //                newRelationship.relationType = RelationType.Family;
    //                newRelationship.relation = Relation.Sibling;
    //                newRelationship.relationshipStats = SetRelationshipVariables(newBrain.npcInfo, newRelationship);

    //                brain.npcRelationships.relationships.Add(newRelationship);



    //            }
    //        }
    //    }
    //} 
    //public void SetParentsSiblingRelationships(NPCBrain newBrain)
    //{
    //    List<NPCBrain> listOfChildren = new List<NPCBrain>();
    //    foreach (Relationship relo in newBrain.npcRelationships.relationships)
    //    {
    //        if (relo.relation == Relation.Child)
    //        {
    //            listOfChildren.Add(relo.otherNPC);
    //        }
    //    }

    //    foreach (NPCBrain brain in listOfChildren)
    //    {
    //        for (int i = 0; i < listOfChildren.Count; i++)
    //        {
    //            if (listOfChildren[i] != brain)
    //            {
    //                Relationship newRelationship = new Relationship();

    //                newRelationship.otherNPC = listOfChildren[i];
    //                newRelationship.otherNPCAge = listOfChildren[i].npcInfo.age;
    //                newRelationship.relationType = RelationType.Family;
    //                newRelationship.relation = Relation.Sibling;
    //                newRelationship.relationshipStats = SetRelationshipVariables(newBrain.npcInfo, newRelationship);

    //                brain.npcRelationships.relationships.Add(newRelationship);



    //            }
    //        }
    //    }
    //}

}

//[CustomEditor(typeof(NPCBrain))]
//public class RelationshipsCustomEditor : Editor
//{
//    NPCRelationships.Relationship relationship;
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        NPCBrain brain = (NPCBrain)target;
//        if (brain == null) return;

//        for (int i = 0; i < brain.npcRelationships.relationships.Count; i++)
//        {
//            relationship = brain.npcRelationships.relationships[i];

//            if (brain.npcRelationships.relationships[i - 1] != null && brain.npcRelationships.relationships[i - 1].relationshipStats.listOfStats.Count > relationship.relationshipStats.listOfStats.Count)
//            {

//                    for (int r = 0; r < brain.npcRelationships.relationships[i - 1].relationshipStats.listOfStats.Count; r++)
//                    {
//                        if (!relationship.relationshipStats.listOfStats.Contains (brain.npcRelationships.relationships[i - 1].relationshipStats.listOfStats[i]))
//                    {
//                        StatContainer.Stat newStat = brain.npcRelationships.relationships[i - 1].relationshipStats.listOfStats[i];
//                        newStat.statValue = 0f;
//                        relationship.relationshipStats.listOfStats.Add(newStat);
//                    }
//                    } 

//            }
//        }


//    }
//}
