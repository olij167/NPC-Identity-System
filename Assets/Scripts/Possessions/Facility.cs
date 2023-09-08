using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
[CreateAssetMenu(menuName = "Possession/Facility")]
public class Facility : Possession
{
    public List<Resource> facilityProduce;

    public List<OrganisationController.JobListing> jobsAtFacility;

    [System.Serializable]
    public class WeeklyRoster
    {
        //public TimeController.CurrentDate currentDate;
        public List<DailyShifts> dailyShifts = new List<DailyShifts>();

    }

    public WeeklyRoster roster;

    public WeeklyRoster SetBlankRoster()
    {
        roster = new WeeklyRoster();

        for (int i = 0; i < 7; i++)
        {
            DailyShifts shifts = new DailyShifts();

            if (i == 0)
                shifts.rosterDay = TimeController.Day.Sunday;
            else if (i == 1)
                shifts.rosterDay = TimeController.Day.Monday;
            else shifts.rosterDay = roster.dailyShifts[i - 1].rosterDay++;

            roster.dailyShifts.Add(shifts);
        }

        return roster;
    }

    [System.Serializable]
    public class DailyShifts
    {
        public TimeController.Day rosterDay;

        public List<ShiftSlot> shiftSlots = new List<ShiftSlot>();
    }

    [System.Serializable]
    public class ShiftSlot
    {
        public Job job;
        [Tooltip("x = start time, y = end time")]
        public Vector2 shiftDuration;
        public int numOfEmployees;
        // definition of shift location and activities
    }

   
    //public override PossessionType PosType { set { possessionType = PossessionType.Facility; } }
    [field: ReadOnlyField] public PossessionType possessionType = PossessionType.Facility;

    public override void Usage()
    {
        base.Usage();
    }
}

//[CustomEditor(typeof(Facility))]
//public class FacilityEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        Facility facility = (Facility)target;
//        if (facility == null) return;

//        if (GUILayout.Button("Reset Roster"))
//        {
//            facility.roster = facility.SetBlankRoster(TimeController.GetCurrentDateStatic());
//        }
//    }
//}
