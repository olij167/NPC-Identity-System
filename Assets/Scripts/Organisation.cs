using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Organisation : MonoBehaviour
{
    public string orgName;

    [TextArea(3, 10)] public string orgDescription;

    public List<JobListing> potentialJobs;

    public List<FilledJob> filledPositions;

    public NPCCreator creator;


    // list of possible positions in organisation
    //     - list of bosses
    //     - list of employees
    //     - required skills / traits for position
    //     - beneficial skills / traits for position
    //     - detremental skills / traits to oc
    // list of filled positions in workplace

    private void Start()
    {
        creator = FindObjectOfType<NPCCreator>();

        //FillPositions();

    }

    [System.Serializable]
    public class JobListing
    {
        public int numOfPositions;
        public Job job;
    }

    [System.Serializable]
    public class FilledJob
    {
        public NPCBrain currentEmployee;
        public Job job;
    }

    public void CheckOpenJobs()
    {
        List<Job> openJobsList = new List<Job>();

        for (int x = 0; x < potentialJobs.Count; x++)
        {
            int openJobs = potentialJobs[x].numOfPositions;

            for (int i = 0; i < filledPositions.Count; i++)
            {
                if (filledPositions[i].job == potentialJobs[x].job)
                {
                    openJobs -= 1;
                }
            }

            Debug.Log(openJobs + " open " + potentialJobs[x].job.jobName + " position(s) at " + orgName);

            if (openJobs > 0)
                for (int i = 0; i < openJobs; i++)
                {
                    openJobsList.Add(potentialJobs[x].job);
                }
        }

        foreach (Job job in openJobsList)
        {
            FillPosition(job);
        }
    }

    public void FillPosition(Job job)
    {
        Debug.Log("Attempting to fill open" + job.jobName + " positions");

        //if (filledPositions == null)
        //    filledPositions = new List<FilledJob>();

        for (int i = 0; i < creator.npcList.Count; i++)
        {
            Debug.Log("Checking Qualifications for " + creator.npcList[i].name);
            if (creator.npcList[i].npcInfo.job == null && creator.npcList[i].npcInfo.age >= job.requiredAge && creator.npcList[i].npcInfo.npcSkills.SkillCheck(job.requiredSkills, creator.npcList[i].npcInfo))
            {
                Debug.Log(creator.npcList[i].name + " passed the job interview");
                FilledJob newJob = new FilledJob();
                newJob.job = job;
                newJob.currentEmployee = creator.npcList[i];
                creator.npcList[i].npcInfo.job = newJob.job;
                filledPositions.Add(newJob);
                Debug.Log(newJob.currentEmployee.name + " is now a " + newJob.job.jobName + " for " + orgName);
                return;

            }

        }
    }
}

[CustomEditor(typeof(Organisation))]
public class OrginisationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Organisation org = (Organisation)target;
        if (org == null) return;

        if (GUILayout.Button("Fill Positions"))
        {
            org.CheckOpenJobs();
        }
    }
}