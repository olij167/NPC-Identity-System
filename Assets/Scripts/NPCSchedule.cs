using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

[System.Serializable]
public class NPCSchedule
{
    // commitments & free time
    // commitments = things the npc must do like go to work

    //calendar - what the npc must do and when

    [System.Serializable]
    public class TimePeriod
    {
        public TimeController.Month month;
        [Range(1,31)]public int day;

        public float startTimeHours, startTimeMinutes;
        public float endTimeHours, endTimeMinutes;
    }

    [System.Serializable]
    public class Schedule
    {
        public string commitment;
        public TimePeriod timePeriod;
    }

    public List<TimePeriod> calendar;
    //public TimePeriod SortByTime(TimePeriod time1, TimePeriod time2)
    //{
    //    if (time1.month < time2.month)
    //    {
    //        return time1;
    //    }
    //    else if (time1.month == time2.month)
    //    {
    //        if (time1.day < time2.day)
    //        {
    //            return time1;
    //        }
    //        else if (time1.day == time2.day)
    //        {
    //            if (time1.startTimeHours < time2.startTimeHours)
    //            {
    //                return time1;
    //            }
    //        }
    //    }

    //    return time2;
    //}

    public void SortByTime()
    {
        List<TimePeriod> comparisonCalendar = calendar;
        List<TimePeriod> sortedCalendar = new List<TimePeriod>();

        for (int i = 0; i < calendar.Count; i++)
        {
            for (int x = 0; x < comparisonCalendar.Count; x++)
            {
                if (i != x)
                if (calendar[i].month < calendar[x].month)
                {

                }

            }
        }
    }

    //public List<TimePeriod> SortCalendar()
    //{
    //    List<TimePeriod> orderedCalendar = new List<TimePeriod>();

    //    for (int i = 0; i < calendar.Count; i++)
    //    {
    //        if (i + 1 > calendar.Count)
    //        {
    //            TimePeriod temp = SortByTime(calendar[i], calendar[i + 1]);

    //            switch(temp)
    //            {
    //                case TimePeriod n when n == calendar[i]:

    //                    break;
    //                case TimePeriod n when n == calendar[i + 1]:
    //                    calendar[i + 1] = calendar[i];
    //                    calendar[i] = temp;
    //                    break;
    //            }
    //            calendar.
    //        }
    //        else
    //        {

    //            TimePeriod temp = SortByTime(calendar[i], calendar[i - 1]);

    //            switch (temp)
    //            {
    //                case TimePeriod n when n == calendar[i - 1]:

    //                    break;
    //                case TimePeriod n when n == calendar[i]:
    //                    calendar[i] = calendar[i - 1];
    //                    calendar[i - 1] = temp;
    //                    break;
    //            }
    //        }
    //    }
    //}

    //public TimePeriod SortByTime(TimePeriod t1, TimePeriod t2)
    //{
    //    int m1 = (int)t1.month;
    //    int m2 = (int)t2.month;

    //    if (m1 != m2)
    //    {
    //        if (m1 < m2) return t1;
    //        else return t2;
    //    }
    //    else if (t1.day != t2.day)
    //    {
    //        if (t1.day < t2.day) return t1;
    //        else return t2;
    //    }
    //    else if (t1.startTimeHours != t2.startTimeHours)
    //    {
    //        if (t1.startTimeHours < t2.startTimeHours) return t1;
    //        else return t2;
    //    }
    //    else
    //    {
    //        if (t1.startTimeMinutes < t2.startTimeMinutes) return t1;
    //        else return t2;
    //    }
    //}

    public void SortCalendar()
    {
        for (int i = 0; i < calendar.Count; i++)
        {
            for (int x = 0; x < calendar.Count; x++)
            {
                if (i != x)
                {
                    //calendar.Sort(SortByTime(calendar[i], calendar[x]));
                    
                }
            }
        }
        
    }
}

//[CustomEditor(typeof(NPCBrain))]
//public class NPCSCheduleEditor : Editor
//{
//    NPCSchedule schedule;

//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        NPCBrain brain = (NPCBrain)target;
//        if (brain == null) return;

//        schedule = brain.npcSchedule;

//        if (GUILayout.Button("Sort Calendar"))
//        {
//            schedule.SortCalendar();
//        }
//    }
//}