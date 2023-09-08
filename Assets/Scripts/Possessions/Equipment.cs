using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Possession/Equipment")]
public class Equipment : Possession
{
    //public override PossessionType PosType { set => possessionType = PossessionType.Equipment; }
    [field: ReadOnlyField] public PossessionType possessionType = PossessionType.Equipment;

    public override void Usage()
    {
        base.Usage();
    }
}
