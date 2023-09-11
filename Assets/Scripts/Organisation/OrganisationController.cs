using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OrganisationController : MonoBehaviour
{
    public NPCCreator creator;
    public TimeController timeController;

    public Organisation org;
    
    public List<JobListing> potentialJobs;
    public List<FilledJob> filledJobs;

    public Inventory inventory;

    public Facility.WeeklyRoster roster; // shifts available each day
    //public Facility.DailyRoster dailyRoster;
    public List<ComingRosterDay> filledRoster;

    [System.Serializable]
    public class FilledJob
    {
        public NPCBrain currentEmployee;
        public Job job;

        //public int shiftsPerWeek;
    }

    [System.Serializable]
    public class JobListing
    {
        public int numOfPositions;
        public Job job;
    }

    [System.Serializable]
    public class Inventory
    {
        public List<Possession> capital;
    }

    //TO DO:
    // - Create Methods to run the business
    //      - Generate a weekly roster
    //      - track work output per employee
    //      - day to day profits & losses
    //      - larger business decisions such as:
    //          ~ changing goods / services,
    //          ~ changing prices,
    //          ~ hiring, promoting & firing employees  
    //          ~ expanding: renovating or opening new locations
    //          ~ going out of business

    [System.Serializable]
    public class ComingRosterDay
    {
        public TimeController.Date rosterDate;
        public List<Shift> shifts = new List<Shift>();
    }

    [System.Serializable]
    public class Shift
    {
        public Job job;
        public NPCBrain employee;
        public Vector2 shiftDuration;

    }

    public void CheckOpenJobs()
    {
        List<Job> openJobLists = new List<Job>();

        foreach( Facility facility in inventory.capital)
        {
            foreach(JobListing job in facility.jobsAtFacility)
            {
                potentialJobs.Add(job);
            }
            roster = facility.roster;
        }

        for (int x = 0; x < potentialJobs.Count; x++)
        {
            int openJobs = potentialJobs[x].numOfPositions;

            for (int i = 0; i < filledJobs.Count; i++)
            {
                if (filledJobs[i].job == potentialJobs[x].job)
                {
                    openJobs -= 1;
                }
            }

            //Debug.Log(openJobs + " open " + potentialJobs[x].job.jobName + " position(s) at " + org.orgName);

            if (openJobs > 0)
                for (int i = 0; i < openJobs; i++)
                {
                    openJobLists.Add(potentialJobs[x].job);
                }
        }

        foreach (Job listing in openJobLists)
        {
            FillPosition(listing);
        }

        FillRoster(roster);
    }

    public void FillRoster(Facility.WeeklyRoster requiredShifts)
    {
        TimeController.Date dayOfWeek = new TimeController.Date();
        dayOfWeek.day = new TimeController.TimeDay();

        dayOfWeek.day.day = timeController.currentDate.day.day;
        dayOfWeek.day.date = timeController.currentDate.day.date;
        dayOfWeek.month = timeController.currentDate.month;

        for (int i = 0; i < requiredShifts.dailyShifts.Count; i++)
        {
            ComingRosterDay rosterDay = new ComingRosterDay();

            if (i != 0)
                timeController.SetDayVariables(dayOfWeek);

            rosterDay.rosterDate = new TimeController.Date();
            rosterDay.rosterDate.day = new TimeController.TimeDay();

            rosterDay.rosterDate.day.day = dayOfWeek.day.day;
            rosterDay.rosterDate.day.date = dayOfWeek.day.date;
            rosterDay.rosterDate.month = dayOfWeek.month;
            //rosterDay.shifts = new List<Shift>();

            Debug.Log(dayOfWeek.day.day + ", " + dayOfWeek.month.month + " " + dayOfWeek.day.date);

            filledRoster.Add(rosterDay);

        }


        for (int d = 0; d < filledRoster.Count; d++) // compare shift slots to current roster
        {
            for (int i = 0; i < roster.dailyShifts.Count; i++)
            {
                if (filledRoster[d].rosterDate.day.day == roster.dailyShifts[i].rosterDay) // if it's the same day
                {
                    for (int x = 0; x < roster.dailyShifts[i].shiftSlots.Count; x++) // for each shift slot
                    {
                        int requiredEmployees = roster.dailyShifts[i].shiftSlots[x].numOfEmployees;
                        for (int r = 0; r < requiredEmployees; r++) // for each identical shift
                        {
                            // Set the identical shift vatiables
                            Shift shift = new Shift();
                            shift.shiftDuration = roster.dailyShifts[i].shiftSlots[x].shiftDuration;
                            shift.job = roster.dailyShifts[i].shiftSlots[x].job;

                            List<NPCBrain> availableEmployees = new List<NPCBrain>();
                            List<NPCBrain> unavailableEmployees = new List<NPCBrain>();

                            for (int fJ = 0; fJ < filledJobs.Count; fJ++) // for each employee
                            {
                                if (filledJobs[fJ].job == shift.job) // if their job is the same as the shift requires
                                {
                                    availableEmployees.Add(filledJobs[fJ].currentEmployee); // add em to a list
                                }
                            }

                            if (availableEmployees.Count > 0)
                                foreach (NPCBrain employee in availableEmployees)
                                {
                                    for (int fR = 0; fR < filledRoster[d].shifts.Count; fR++) // check the currently filled shifts
                                    {
                                        if (employee != null && employee == filledRoster[d].shifts[fR].employee) // if they're already working remove them
                                        {
                                            unavailableEmployees.Add(employee);
                                        }
                                    }
                                }

                            for (int u = 0; u < unavailableEmployees.Count; u++)
                            {
                                for (int a = 0; a < availableEmployees.Count; a++)
                                {
                                    if (unavailableEmployees[u] == availableEmployees[a])
                                        availableEmployees.RemoveAt(a);
                                }
                            }


                            if (availableEmployees.Count > 0)
                            {
                                int rand = Random.Range(0, availableEmployees.Count);

                                shift.employee = availableEmployees[rand];

                                NPCSchedule.ScheduledActivity work = new NPCSchedule.ScheduledActivity();

                                work.activityType = NPCSchedule.ActivityType.Work;
                                work.duration = shift.shiftDuration;

                                shift.employee.npcSchedule.ScheduleActivity(filledRoster[d].rosterDate, work);

                                filledRoster[d].shifts.Add(shift);
                            }
                            else // shift is open
                            {
                                shift.employee = null;

                                filledRoster[d].shifts.Add(shift);
                            }
                        }

                    }
                }
            }
        }
    }

    public void FillPosition(Job job)
    {
        //Debug.Log("Attempting to fill open" + job.jobName + " positions");

        //if (filledPositions == null)
        //    filledPositions = new List<FilledJob>();

        if (creator == null) creator = GetComponent<NPCCreator>();

        for (int i = 0; i < creator.npcList.Count; i++)
        {
            //Debug.Log("Checking Qualifications for " + creator.npcList[i].name);
            if (creator.npcList[i].npcInfo.job == null && creator.npcList[i].npcInfo.age >= job.requiredAge && creator.npcList[i].npcInfo.npcSkills.SkillCheck(job.requiredSkills, creator.npcList[i].npcInfo))
            {
                //Debug.Log(creator.npcList[i].name + " passed the job interview");
                FilledJob newJob = new FilledJob();
                newJob.job = job;
                newJob.currentEmployee = creator.npcList[i];
                creator.npcList[i].npcInfo.job = newJob.job;
                filledJobs.Add(newJob);
                //Debug.Log(newJob.currentEmployee.name + " is now a " + newJob.job.jobName + " for " + org.orgName);
                return;

            }

        }

    }
}

[CustomEditor(typeof(OrganisationController))]
public class OrginisationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        OrganisationController org = (OrganisationController)target;
        if (org == null) return;

        if (GUILayout.Button("Fill Positions"))
        {
            org.CheckOpenJobs();
        } 
       
    }
}