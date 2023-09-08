using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Possession/Resource")]
public class Resource : Possession
{
    public ResourceGrade resourceGrade;
    public ProductionInfo productionInfo;
    //public override PossessionType PosType { set => possessionType = PossessionType.Resource; }
    [field: ReadOnlyField] public PossessionType possessionType = PossessionType.Resource;

    public override void Usage()
    {
        base.Usage();
    }

    [System.Serializable]
    public enum ResourceGrade
    {
        Raw, Refined, Produced
    }
}

