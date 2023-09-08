using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Possession : ScriptableObject
{
    public string assetName;

    public GameObject prefab;
    public float value;


    //public abstract PossessionType PosType { set; }
    //[ReadOnlyField] public PossessionType possessionType;

    public virtual void Usage()
    {
        // research how to define different usages per possessiontype & possession
    }
}

[System.Serializable]
public enum PossessionType
{
    Property, Equipment, Facility, Resource, Valuable
}


[System.Serializable]
public class ProductionInfo
{
    public List<Facility> requiredFacilities;//required facilities to produce
    public List<Resource> requiredResources; // required resources to produce
    public List<Job> requiredJobs; //required jobs to produce
    public float requiredTime; // how long it takes to produce
}



