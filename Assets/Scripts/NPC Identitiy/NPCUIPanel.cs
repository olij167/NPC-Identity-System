using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using TMPro;

public class NPCUIPanel : MonoBehaviour
{
    private NPCBrain brain;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI emotionalDispositionText;
    public TextMeshProUGUI beliefText;

    public void SetName()
    {
        brain = transform.parent.parent.GetComponent<NPCBrain>();
        nameText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        nameText.text = brain.name;

    }
}

//public class NPCUIPanelEditor : Editor
//{

//}
