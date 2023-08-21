using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatContainer
{
    public List<Stat> listOfStats;

    //public Stat highestStat;

    [System.Serializable]
    public class Stat
    {
        public string statName = "New Stat";
        [Range(-1f, 1f)] public float statValue;
    }

    [HideInInspector] public string newStatName = "New Stat";

    public Stat CreateNewStat(string statName)
    {
        float statValue = 0;

        Stat newStat = new Stat();
        newStat.statName = statName;
        newStat.statValue = statValue;

        return newStat;
    }

    public void AddStatToList(Stat newStat)
    {
        listOfStats.Add(newStat);

    }

    static int SortStatByValues(Stat s1, Stat s2)
    {
        return s2.statValue.CompareTo(s1.statValue);
    }

    public void CheckStatValues()
    {

        listOfStats.Sort(SortStatByValues);

        //statContainer.highestStat = statContainer.listOfStats[0];

    }
}
