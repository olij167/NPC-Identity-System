using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Pathfinding;
using UnityEngine.AI;

[RequireComponent(typeof(NPCObjectDetection))]
public class NPCBrain : MonoBehaviour
{
    public NPCInfo npcInfo;

    public NPCEmotions npcEmotions;

    //public NPCNeeds npcNeeds;

    public NPCRelationships npcRelationships;

    public NPCSchedule npcSchedule;

    public NPCSchedule.ScheduledActivity currentActivity;

    private AIDestinationSetter destinationSetter;
    public GameObject waypointPrefab;
    GameObject waypoint;
    float timer = 0f;

    public NPCObjectDetection npcEyes;
    //TimeController.Date currentDate;
    //TimeController.TimeHourMinSec currentTime;

    private void Awake()
    {
        npcEmotions.personality = npcInfo.personality;

        destinationSetter = GetComponent<AIDestinationSetter>();

        timer = npcInfo.personality.attentionSpan;

        //currentDate = TimeController.instance.currentDate;
        //currentTime = TimeController.instance.currentTime;
        
        //currentActivity = null;
       // SetCurrentActivity();
    }

    private void Update()
    {
        npcEmotions.SetMood(npcEmotions.emotion, npcEmotions.personality);

        //if (TimeController.instance.currentTime.timeHours >= currentActivity.duration.y)
        //    SetCurrentActivity();

        GoToActivity();

        if (TimeController.instance.currentTime.timeHours >= currentActivity.duration.y) 
        {
            SetCurrentActivity();
        }
        // ^^ change to set at midnight each day,
        // - first set planned activities then generate freetime activites
        // - freetime activites should check what the npc's partner, friends, and family are doing and whether they would wanna hangout
    }

    public void SetCurrentActivity()
    {
        //get current date and time
        //Debug.Log("setting activity");
        //Debug.Log(TimeController.instance.currentDate.month.month.ToString());

        // find respective date in calendar
        for (int m = 0; m < npcSchedule.calendar.calendarMonths.Count; m++)
        {
            if (npcSchedule.calendar.calendarMonths[m].month.month == TimeController.instance.currentDate.month.month)
            {
                Debug.Log("Correct month found");
                for (int d = 0; d < npcSchedule.calendar.calendarMonths[m].calendarDays.Count; d++)
                {
                    if (npcSchedule.calendar.calendarMonths[m].calendarDays[d].day.day.date == TimeController.instance.currentDate.day.date)
                    {
                        Debug.Log("Correct day found");
                        if (npcSchedule.calendar.calendarMonths[m].calendarDays[d].scheduledActivities != null && npcSchedule.calendar.calendarMonths[m].calendarDays[d].scheduledActivities.Count > 0)
                        {
                            Debug.Log("activities found during day");
                            for (int a = 0; a < npcSchedule.calendar.calendarMonths[m].calendarDays[d].scheduledActivities.Count; a++)
                            {
                                //if the current time is within the activities duration
                                if (npcSchedule.calendar.calendarMonths[m].calendarDays[d].scheduledActivities[a].duration.x <= TimeController.instance.currentTime.timeHours &&
                                    npcSchedule.calendar.calendarMonths[m].calendarDays[d].scheduledActivities[a].duration.y > TimeController.instance.currentTime.timeHours)
                                {
                                    //go to location
                                    Debug.Log("activity found for right now");

                                    currentActivity.activityType = npcSchedule.calendar.calendarMonths[m].calendarDays[d].scheduledActivities[a].activityType;
                                    currentActivity.duration = npcSchedule.calendar.calendarMonths[m].calendarDays[d].scheduledActivities[a].duration;
                                    currentActivity.location = npcSchedule.calendar.calendarMonths[m].calendarDays[d].scheduledActivities[a].location;
                                }
                            }
                        }
                        else if (Chance.CoinFlip() && npcInfo.household.house != null) //For now either walk around or go home if not at work
                        {
                            NPCSchedule.ScheduledActivity sleep = new NPCSchedule.ScheduledActivity();

                            sleep.activityType = NPCSchedule.ActivityType.Sleep;
                            sleep.location = npcInfo.household.house.transform;
                            sleep.duration.x = TimeController.instance.currentTime.timeHours;
                            sleep.duration.y = TimeController.instance.currentTime.timeHours + Random.Range(1f, 8f);

                            //Debug.Log(name + ":" + " Time for bed");

                            currentActivity = sleep;

                        }
                        else
                        {
                            NPCSchedule.ScheduledActivity activity = new NPCSchedule.ScheduledActivity();

                            activity.activityType = NPCSchedule.ActivityType.FreeTime;

                            currentActivity = activity;
                        }

                        // check current mood and needs
                        //    // create a list of freetime activities with associated moods and need levels
                        //    // check list and find most suitable activity
                        // eg. sleep when tired, socialise when lonely, perform hobby when needs are filled
                    }
                }
            }
        }

        //if (currentActivity.duration.)

        //Debug.Log(name + ":" + " Time for " + currentActivity.activityType.ToString());
    }

    public void GoToActivity()
    {
        if (currentActivity.location != null) // if you have somewhere to be then go there
            destinationSetter.target = currentActivity.location;
        else // otherwise wander aimlessly
        {
            timer += Time.deltaTime;
            float wanderRange = Random.Range(5f, 10f);

            if (timer >= npcInfo.personality.attentionSpan)
            {
                //waypoint.transform.position = RandomNavSphere(transform.position, wanderRange, -1);
                //destinationSetter.target = waypoint.transform;
                if (waypoint != null) Destroy(waypoint.gameObject);

                waypoint = Instantiate(waypointPrefab, RandomNavSphere(transform.position, wanderRange, -1), Quaternion.identity);

                destinationSetter.target = waypoint.transform;

                timer = 0;
            }
        }
    }

    public void GenerateFreetimeActivites()
    {
        // Check NPC Needs -> perform selfcare if below certain thresholds, overrides other freetime activites (should differ based on traits) 
        // Check NPC Hobbies -> what they enjoy doing (should differ based on traits) 
        // Check Relationships -> what the npc's partner, friends, and family are doing and whether they would wanna hangout
            // check other npcs schedule -> if also free -> check if they have any common hobbies -> decide from mutual hobbies -> change relationship values during activity
            // should also depend on both npcs emotions, needs, and whether they are already doing a freetime activity
    }

    // from: https://forum.unity.com/threads/solved-random-wander-ai-using-navmesh.327950/
    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    //public void SetRelationshipStats(NPCBrain otherNPC)
    //{
    //    NPCRelationships.Relationship newRelo = new NPCRelationships.Relationship();

    //    newRelo.otherNPC = otherNPC;

    //    //Check beliefs & determine compatibility
    //    //Check Sexual preferences & determine attraction

    //    //Set authority based on age & occupation

    //    //Set respect based on authority and compatibility
    //    //Set trust based on reputation and comfort

    //    //Set affection based on other npc reputation, relationship type
    //}

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

[CustomEditor(typeof(NPCBrain))]
public class NPCEmotionEditor : Editor
{
    NPCInfo info;
    NPCEmotions emotions;
    NPCRelationships relationships;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        NPCBrain brain = (NPCBrain)target;
        if (brain == null) return;

        info = brain.npcInfo;
        emotions = brain.npcEmotions;
        relationships = brain.npcRelationships;

    }
}
