using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Possession/Property")]
public class Property : Possession
{

    //public override PossessionType PosType { set => possessionType = PossessionType.Property; }

    [field: ReadOnlyField] public PossessionType possessionType = PossessionType.Property;

    public override void Usage()
    {
        base.Usage();
    }
}
