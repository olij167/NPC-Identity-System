using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(menuName = "Job")]
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

    //work schedule

    public Job nextPromotion;
}
