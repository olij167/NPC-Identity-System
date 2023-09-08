using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
[CreateAssetMenu(menuName = "Occupation/Organisation")]
public class Organisation : ScriptableObject
{
    public string orgName;
    [TextArea(2, 8)] public string orgDescription;

    //public Industry industry;
    public OrgLifeCycle lifeCycleStage;

    //public List<JobListing> potentialJobs;

    //public List<InventoryAsset> inventory;
    //public List<Facility> facilities;

    public List<Resource> goodsProvided;
    public List<Service> servicesProvided;

    //[System.Serializable]
    //public enum Industry 
    //{
    //    Farming, Mining, Healthcare, Education, Manufacturing, Retail, Entertainment, Crime, Law, Government, Construction, Maintainance, Finance, Arms, Art
    //}
    // TO DO: set different infrastructure and potential jobs depending on industry
    // TO DO: create sub categories of potential goods & services a business could provide per industry
    // TO DO: make an organisation generator

    //[System.Serializable]
    //public class ProductionInfo
    //{
    //    //produce
    //    public Resource produce;

    //    public List<Facility> requiredFacilities;//required facilities to produce
    //    public List<Resource> requiredResources; // required resources to produce
    //    public List<Job> requiredJobs; //required jobs to produce
    //    public float requiredTime; // how long it takes to produce
    //}

    [System.Serializable]
    public enum OrgLifeCycle
    {
        Establishment, Growth, Maturity, Decline
    }

    

    //[System.Serializable]
    //public class WeeklyRoster
    //{
    //    public List<DailyShifts> dailyShifts = new List<DailyShifts>();

    //}

    //public WeeklyRoster roster;

    //public WeeklyRoster SetBlankRoster()
    //{
    //    roster = new WeeklyRoster();

    //    for (int i = 0; i < 7; i++)
    //    {
    //        DailyShifts shifts = new DailyShifts();

    //        switch (i)
    //        {
    //            case 0: shifts.rosterDay = TimeController.Day.Monday; break;
    //            case 1: shifts.rosterDay = TimeController.Day.Tuesday; break;
    //            case 2: shifts.rosterDay = TimeController.Day.Wednesday; break;
    //            case 3: shifts.rosterDay = TimeController.Day.Thursday; break;
    //            case 4: shifts.rosterDay = TimeController.Day.Friday; break;
    //            case 5: shifts.rosterDay = TimeController.Day.Saturday; break;
    //            case 6: shifts.rosterDay = TimeController.Day.Sunday; break;
    //        }

    //        roster.dailyShifts.Add(shifts);

    //    }

    //    return roster;
    //}

    //[System.Serializable]
    //public class DailyShifts
    //{
    //    public TimeController.Day rosterDay;

    //    public List<ShiftSlot> shiftSlots = new List<ShiftSlot>();
    //}

    //[System.Serializable]
    //public class ShiftSlot
    //{
    //    public Job job;
    //    [Tooltip("x = start time, y = end time")]
    //    public Vector2 shiftDuration;
    //    public int numOfEmployees;
    //    // definition of shift location and activities
    //}

    //[System.Serializable]
    //public class Facility
    //{
    //    public string facilityName;
    //    public GameObject facilityPrefab;
    //    public List<Resource> facilityProduce;

    //    public List<JobListing> jobsAtFacility;
    //}

    //[System.Serializable]
    //public class Resource
    //{
    //    public string resourceName;
    //    public GameObject resource;
    //    public ResourceGrade resourceGrade;
    //    public float price;
    //    public ProductionInfo productionInfo;
    //    // requirements to produce good - materials & facilities
    //}

    //[System.Serializable]
    //public enum ResourceGrade
    //{
    //    Raw, Refined, Produced
    //}

    //[System.Serializable]
    //public enum ResourceQuality
    //{

    //}

    [System.Serializable]
    public class Service
    {
        public string serviceDescription;
        public float price;
        public ProductionInfo productionInfo;
        // requirements to provide service  - materials & facilities
    }


    //[System.Serializable]
    //public class InventoryAsset
    //{
    //    public Resource resource;
    //    public float quantity;
    //}

    //[CustomEditor(typeof(Organisation))]
    //public class OrgEditor : Editor
    //{
    //    public override void OnInspectorGUI()
    //    {
    //        base.OnInspectorGUI();

    //        Organisation org = (Organisation)target;
    //        if (org == null) return;

    //        if (GUILayout.Button("Reset Roster"))
    //        {
    //            org.roster = org.SetBlankRoster();
    //        }
    //    }
    //}
}
