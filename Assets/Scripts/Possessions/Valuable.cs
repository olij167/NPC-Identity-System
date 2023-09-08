using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Possession/Valuable")]
public class Valuable : Possession
{
    //public override PossessionType PosType { set => possessionType = PossessionType.Valuable; }
    [field: ReadOnlyField] public PossessionType possessionType = PossessionType.Valuable;
    public override void Usage()
    {
        base.Usage();
    }
}

