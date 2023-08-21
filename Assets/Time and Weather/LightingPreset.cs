using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu (fileName = "Lighting Preset", menuName ="Lighting Preset")]
public class LightingPreset : ScriptableObject
{
    public Gradient ambientColour;
    public Gradient directionalColour;
    public Gradient fogColour;
}
