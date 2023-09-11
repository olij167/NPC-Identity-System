using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class NPCSchedule
{

    public Calendar calendar;
    //public List<TimeController.Day> daysInYear;
    public CalendarWeek currentWeek;


    [System.Serializable]
    public class Calendar
    {
        public List<CalendarMonth> calendarMonths = new List<CalendarMonth>();
    }

    [System.Serializable]
    public class CalendarMonth
    {
        public TimeController.TimeMonth month = new TimeController.TimeMonth();
        public List<CalendarDay> calendarDays = new List<CalendarDay>();
    }

    [System.Serializable]
    public class CalendarDay
    {
        public TimeController.TimeDay day = new TimeController.TimeDay();
        public List<ScheduledActivity> scheduledActivities = new List<ScheduledActivity>();
    }


    [System.Serializable]
    public class CalendarWeek
    {
        public List<WeekDay> weekDays = new List<WeekDay>();
       
    }

    [System.Serializable]
    public class WeekDay
    {
        public TimeController.Day weekDay;
        public List<ScheduledActivity> scheduledActivities = new List<ScheduledActivity>();
    }
    public static CalendarWeek SetWeekDays()
    {
        CalendarWeek week = new CalendarWeek();

        for (int i = 0; i < 7; i++)
        {
            TimeController.Day day = new TimeController.Day();
            switch (i)
            {
                case 0: day = TimeController.Day.Sunday; break;
                case 1: day = TimeController.Day.Monday; break;
                case 2: day = TimeController.Day.Tuesday; break;
                case 3: day = TimeController.Day.Wednesday; break;
                case 4: day = TimeController.Day.Thursday; break;
                case 5: day = TimeController.Day.Friday; break;
                case 6: day = TimeController.Day.Saturday; break;
            }
            WeekDay weekDay = new WeekDay();

            weekDay.weekDay = day;

            week.weekDays.Add(weekDay);
        }

        return week;
    }

    [System.Serializable]
    public class ScheduledActivity
    {
        [Tooltip ("x = start time, y = end time")]
        public Vector2 duration;
        public ActivityType activityType;

        // info about the scheduled activity for the npc
            // - where to go
            // - how it affects their mood

    }

    [System.Serializable]
    public enum ActivityType
    {
        Social, Hobby, Sleep, Work, SelfCare
    }

    public void SetCalendar(Calendar calendar)
    {
        for (int m = 0; m < 12; m++)
        {
            calendar.calendarMonths.Add(new CalendarMonth());
        }

        for (int m = 0; m < calendar.calendarMonths.Count; m++)
        {
            switch (m)
            {
                case 0: calendar.calendarMonths[m].month.month = TimeController.Month.January; break;
                case 1: calendar.calendarMonths[m].month.month = TimeController.Month.Feburary; break;
                case 2: calendar.calendarMonths[m].month.month = TimeController.Month.March; break;
                case 3: calendar.calendarMonths[m].month.month = TimeController.Month.April; break;
                case 4: calendar.calendarMonths[m].month.month = TimeController.Month.May; break;
                case 5: calendar.calendarMonths[m].month.month = TimeController.Month.June; break;
                case 6: calendar.calendarMonths[m].month.month = TimeController.Month.July; break;
                case 7: calendar.calendarMonths[m].month.month = TimeController.Month.August; break;
                case 8: calendar.calendarMonths[m].month.month = TimeController.Month.September; break;
                case 9: calendar.calendarMonths[m].month.month = TimeController.Month.October; break;
                case 10: calendar.calendarMonths[m].month.month = TimeController.Month.November; break;
                case 11: calendar.calendarMonths[m].month.month = TimeController.Month.December; break;
            }

            SetMonthVariables(calendar.calendarMonths[m].month);
        }

        List<TimeController.Day> daysInYear = new List<TimeController.Day>();

        for (int i = 0; i < 365; i++)
        {
            if (i == 0)
            {
                daysInYear.Add(TimeController.Day.Monday);
            }
            else daysInYear.Add(SetDayOfWeek(daysInYear[i - 1]));
        }

        int currentDayCount = 0;

        for (int i = 0; i < calendar.calendarMonths.Count; i++)
        {
            
            for (int d = 0; d < calendar.calendarMonths[i].month.endOfMonth; d++)
            {
                calendar.calendarMonths[i].calendarDays.Add(new CalendarDay());
            }
            
            for (int d = 0; d < calendar.calendarMonths[i].calendarDays.Count; d++)
            {
                calendar.calendarMonths[i].calendarDays[d].day.day = daysInYear[currentDayCount];
                currentDayCount++;
                calendar.calendarMonths[i].calendarDays[d].day.date = d + 1;

                //for (int h = 0; h < 24; h++)
                //{
                //    ScheduledActivity hour = new ScheduledActivity();
                //    hour.hourOfDay = h;
                //    calendar.calendarMonths[i].calendarDays[d].calendarHours.Add(hour);
                //}
            }
        }
    }
    //for (int d = 0; d < calendar.calendarMonths[m].month.endOfMonth; d++)
    //{
    //    calendar.calendarMonths[m].calendarDays.Add(new CalendarDay());
    //}

    //for (int d = 0; d < calendar.calendarMonths[m].calendarDays.Count; d++)
    //{
    //    //switch (d)
    //    //{
    //    //    case int n when n == 0 || n == 7 || n == 14 || n == 21 || n == 28: calendar.calendarMonths[m].calendarDays[d].day.day = TimeController.Day.Monday; break;
    //    //    case int n when n == 1 || n == 8 || n == 15 || n == 22 || n == 29: calendar.calendarMonths[m].calendarDays[d].day.day = TimeController.Day.Tuesday; break;
    //    //    case int n when n == 2 || n == 9 || n == 16 || n == 23 || n == 30: calendar.calendarMonths[m].calendarDays[d].day.day = TimeController.Day.Wednesday; break;
    //    //    case int n when n == 3 || n == 10 || n == 17 || n == 24 || n == 31: calendar.calendarMonths[m].calendarDays[d].day.day = TimeController.Day.Thursday; break;
    //    //    case int n when n == 4 || n == 11 || n == 18 || n == 25: calendar.calendarMonths[m].calendarDays[d].day.day = TimeController.Day.Friday; break;
    //    //    case int n when n == 5 || n == 12 || n == 19 || n == 26: calendar.calendarMonths[m].calendarDays[d].day.day = TimeController.Day.Saturday; break;
    //    //    case int n when n == 6 || n == 13 || n == 20 || n == 27: calendar.calendarMonths[m].calendarDays[d].day.day = TimeController.Day.Sunday; break;
    //    //}

    //    calendar.calendarMonths[m].calendarDays[d].day.date = d + 1;


    //    for (int hr = 0; hr < 24; hr++)
    //    {
    //        calendar.calendarMonths[m].calendarDays[d].calendarHours.Add(new CalendarHour());
    //    }
    //}
    public TimeController.Day SetDayOfWeek(TimeController.Day yesterday)
    {
        switch (yesterday)
        {
            case TimeController.Day.Sunday: return TimeController.Day.Monday;
            case TimeController.Day.Monday: return TimeController.Day.Tuesday;
            case TimeController.Day.Tuesday: return TimeController.Day.Wednesday;
            case TimeController.Day.Wednesday: return TimeController.Day.Thursday;
            case TimeController.Day.Thursday: return TimeController.Day.Friday;
            case TimeController.Day.Friday: return TimeController.Day.Saturday;
            case TimeController.Day.Saturday: return TimeController.Day.Sunday;
            default: return TimeController.Day.Monday;
        }
    }

    public void SetMonthVariables(TimeController.TimeMonth month)
    {
        switch ((int)month.month)
        {
            case 0:
                month.month = TimeController.Month.January;
                month.season = TimeController.Season.Summer;
                month.endOfMonth = 31;
                break;
            case 1:
                month.month = TimeController.Month.Feburary;
                month.season = TimeController.Season.Summer;
                month.endOfMonth = 28;
                break;
            case 2:
                month.month = TimeController.Month.March;
                month.season = TimeController.Season.Autumn;
                month.endOfMonth = 31;
                break;
            case 3:
                month.month = TimeController.Month.April;
                month.season = TimeController.Season.Autumn;
                month.endOfMonth = 30;
                break;
            case 4:
                month.month = TimeController.Month.May;
                month.season = TimeController.Season.Autumn;
                month.endOfMonth = 31;
                break;
            case 5:
                month.month = TimeController.Month.June;
                month.season = TimeController.Season.Winter;
                month.endOfMonth = 30;
                break;
            case 6:
                month.month = TimeController.Month.July;
                month.season = TimeController.Season.Winter;
                month.endOfMonth = 31;
                break;
            case 7:
                month.month = TimeController.Month.August;
                month.season = TimeController.Season.Winter;
                month.endOfMonth = 31;
                break;
            case 8:
                month.month = TimeController.Month.September;
                month.season = TimeController.Season.Spring;
                month.endOfMonth = 30;
                break;
            case 9:
                month.month = TimeController.Month.October;
                month.season = TimeController.Season.Spring;
                month.endOfMonth = 31;
                break;
            case 10:
                month.month = TimeController.Month.November;
                month.season = TimeController.Season.Spring;
                month.endOfMonth = 30;
                break;
            case 11:
                month.month = TimeController.Month.December;
                month.season = TimeController.Season.Summer;
                month.endOfMonth = 31;
                break;
        }
    }

    public void ScheduleActivity(TimeController.Date date, ScheduledActivity activity)
    {
        // find date in calendar
        
        for (int m = 0; m < calendar.calendarMonths.Count; m++)
        {
            if (calendar.calendarMonths[m].month.month == date.month.month)
            {
                for (int d = 0; d < calendar.calendarMonths[m].calendarDays.Count; d++)
                {
                    if (calendar.calendarMonths[m].calendarDays[d].day.date == date.day.date)
                    {
                        calendar.calendarMonths[m].calendarDays[d].scheduledActivities.Add(activity); // schedule activity

                        // TO DO:
                        // Sort calendar based on activity start times
                        // Create a system to check the npcs calendar to determine what they should be doing

                    }
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