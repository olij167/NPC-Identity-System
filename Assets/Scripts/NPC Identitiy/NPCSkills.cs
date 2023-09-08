using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCSkills
{
    [Range(0f, 1f)] public float strength;
    [Range(0f, 1f)] public float speed;
    [Range(0f, 1f)] public float charisma;
    [Range(0f, 1f)] public float coordination;
    [Range(0f, 1f)] public float intelligence;
    [Range(0f, 1f)] public float willpower;
    [Range(0f, 1f)] public float luck;

    //public bool SkillCheck(float skillValue, float requiredSkill, bool luckCheck = true)
    //{
    //    if (!luckCheck)
    //    {
    //        if (skillValue >= requiredSkill)
    //            return true;
    //    }
    //    else if (Chance.Percentage(Mathf.Clamp01(luck + skillValue / 2)))
    //        return true;

    //    return false;
    //} 
    
    public bool SkillCheck(NPCSkills skillValue, NPCInfo npc)
    {
        float skillCheck = 0;

        if (skillValue.strength <= npc.npcSkills.strength) skillCheck += 0.1f;
        else if (skillValue.strength / 2 >= npc.npcSkills.strength) skillCheck -= 0.1f;
        
        if (skillValue.speed <= npc.npcSkills.speed) skillCheck += 0.1f;
        else if (skillValue.speed / 2 >= npc.npcSkills.speed) skillCheck -= 0.1f;
         
        if (skillValue.charisma <= npc.npcSkills.charisma) skillCheck += 0.1f;
        else if (skillValue.coordination / 2 >= npc.npcSkills.coordination) skillCheck -= 0.1f;
         
        if (skillValue.coordination <= npc.npcSkills.coordination) skillCheck += 0.1f;
        else if (skillValue.coordination / 2 >= npc.npcSkills.coordination) skillCheck -= 0.1f;
         
        if (skillValue.intelligence <= npc.npcSkills.intelligence) skillCheck += 0.1f;
        else if (skillValue.intelligence /2 >= npc.npcSkills.intelligence) skillCheck -= 0.1f;
         
        if (skillValue.willpower <= npc.npcSkills.willpower) skillCheck += 0.1f;
        else if (skillValue.willpower /2 >= npc.npcSkills.willpower) skillCheck -= 0.1f;
         
        if (skillValue.luck <= npc.npcSkills.luck) skillCheck += 0.1f;
        else if (skillValue.luck /2 >= npc.npcSkills.luck) skillCheck -= 0.1f;

        // check if value is higher than x


        if (skillCheck >= 0.5)
        {
            return true;
        }
        else
        return false;
    }

    public void SetFamilySkills()
    {
        strength = Random.Range(0f, 1f);
        speed = Random.Range(0f, 1f);
        charisma = Random.Range(0f, 1f);
        coordination = Random.Range(0f, 1f);
        intelligence = Random.Range(0f, 1f);
        willpower = Random.Range(0f, 1f);
        luck = Random.Range(0f, 1f);
    }
    public void SetNPCSkills(NPCBrain brain)
    {
        if (brain.npcRelationships.parent1 != null)
        {
            if (brain.npcRelationships.parent2 != null)
            {
                for (int i = 0; i < 7; i++)
                {
                    float r = Random.Range(0.25f, 0.75f);

                    switch (i)
                    {
                        case 0:
                            strength = Mathf.Lerp(brain.npcRelationships.parent1.npcInfo.npcSkills.strength, brain.npcRelationships.parent2.npcInfo.npcSkills.strength, r);
                            break;

                        case 1:
                            speed = Mathf.Lerp(brain.npcRelationships.parent1.npcInfo.npcSkills.speed, brain.npcRelationships.parent2.npcInfo.npcSkills.speed, r);
                            break;

                        case 2:
                            charisma = Mathf.Lerp(brain.npcRelationships.parent1.npcInfo.npcSkills.charisma, brain.npcRelationships.parent2.npcInfo.npcSkills.charisma, r);
                            break;

                        case 3:
                            coordination = Mathf.Lerp(brain.npcRelationships.parent1.npcInfo.npcSkills.coordination, brain.npcRelationships.parent2.npcInfo.npcSkills.coordination, r);
                            break;

                        case 4:
                            intelligence = Mathf.Lerp(brain.npcRelationships.parent1.npcInfo.npcSkills.intelligence, brain.npcRelationships.parent2.npcInfo.npcSkills.intelligence, r);
                            break;

                        case 5:
                            willpower = Mathf.Lerp(brain.npcRelationships.parent1.npcInfo.npcSkills.willpower, brain.npcRelationships.parent2.npcInfo.npcSkills.willpower, r);
                            break;

                        case 6:
                            luck = Mathf.Lerp(brain.npcRelationships.parent1.npcInfo.npcSkills.luck, brain.npcRelationships.parent2.npcInfo.npcSkills.luck, r);
                            break;


                    }
                }
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    float r = Random.Range(0.25f, 0.75f);

                    switch (i)
                    {
                        case 0:
                            strength = Mathf.Lerp(brain.npcRelationships.parent1.npcInfo.npcSkills.strength, Random.Range(0.25f, 0.75f), r);
                            break;

                        case 1:
                            speed = Mathf.Lerp(brain.npcRelationships.parent1.npcInfo.npcSkills.speed, Random.Range(0.25f, 0.75f), r);
                            break;

                        case 2:
                            charisma = Mathf.Lerp(brain.npcRelationships.parent1.npcInfo.npcSkills.charisma, Random.Range(0.25f, 0.75f), r);
                            break;

                        case 3:
                            coordination = Mathf.Lerp(brain.npcRelationships.parent1.npcInfo.npcSkills.coordination, Random.Range(0.25f, 0.75f), r);
                            break;

                        case 4:
                            intelligence = Mathf.Lerp(brain.npcRelationships.parent1.npcInfo.npcSkills.intelligence, Random.Range(0.25f, 0.75f), r);
                            break;

                        case 5:
                            willpower = Mathf.Lerp(brain.npcRelationships.parent1.npcInfo.npcSkills.willpower, Random.Range(0.25f, 0.75f), r);
                            break;

                        case 6:
                            luck = Mathf.Lerp(brain.npcRelationships.parent1.npcInfo.npcSkills.luck, Random.Range(0.25f, 0.75f), r);
                            break;

                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < 7; i++)
            {
                float r = Random.Range(0.25f, 0.75f);

                switch (i)
                {
                    case 0:
                        strength = Mathf.Lerp(brain.npcInfo.family.skillGenetics.strength, Random.Range(0.25f, 0.75f), r);
                        break;

                    case 1:
                        speed = Mathf.Lerp(brain.npcInfo.family.skillGenetics.speed, Random.Range(0.25f, 0.75f), r);
                        break;

                    case 2:
                        charisma = Mathf.Lerp(brain.npcInfo.family.skillGenetics.charisma, Random.Range(0.25f, 0.75f), r);
                        break;

                    case 3:
                        coordination = Mathf.Lerp(brain.npcInfo.family.skillGenetics.coordination, Random.Range(0.25f, 0.75f), r);
                        break;

                    case 4:
                        intelligence = Mathf.Lerp(brain.npcInfo.family.skillGenetics.intelligence, Random.Range(0.25f, 0.75f), r);
                        break;

                    case 5:
                        willpower = Mathf.Lerp(brain.npcInfo.family.skillGenetics.willpower, Random.Range(0.25f, 0.75f), r);
                        break;

                    case 6:
                        luck = Mathf.Lerp(brain.npcInfo.family.skillGenetics.luck, Random.Range(0.25f, 0.75f), r);
                        break;

                }
            }


        }

        luck += Random.Range(-0.5f, 0.5f);

        if (luck < 0f) luck = 0.1f;
        if (luck > 1f) luck = 0.9f;
    }
}