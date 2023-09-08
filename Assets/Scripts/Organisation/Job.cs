using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[System.Serializable]
[CreateAssetMenu(menuName = "Occupation/Job")]
public class Job : ScriptableObject
{
    public string jobName;
    [TextArea(3, 10)] public string jobDescription;

    //public NPCBrain currentEmployee;

    //list of job requirements (skills & traits)
    public NPCSkills requiredSkills;
    public float requiredAge;

    public List<Job> bosses;
    public List<Job> employees;

    public float payRate;

    [Range(-1f, 1f)] public float performance;
    [Range(-1f, 1f)] public float jobSatisfaction;

    //work schedule
    //public NPCSchedule.CalendarWeek weeklySchedule = NPCSchedule.SetWeekDays();

    [System.Serializable]
    public class JobShift
    {
        //public TimeController.Day dayOfShift;
        [Tooltip("x = start time, y = end time")]
        public Vector2 shiftDuration;

        // description of activities during shift
    }

    public Job nextPromotion;

    // script to perform the job
}

[CustomEditor(typeof(Job))]
public class JobEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Job job = (Job)target;
        if (job == null) return;

        if (GUILayout.Button("Reset Week Roster"))
        {
            //job. = NPCSchedule.SetWeekDays();
        }
    }
}
