using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

public class TimeController : MonoBehaviour
{
    public static TimeController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    [System.Serializable] public enum Day { Monday = 1, Tuesday = 2, Wednesday = 3, Thursday = 4, Friday = 5, Saturday = 6, Sunday = 7}

    [System.Serializable] public enum Month { January, Feburary, March, April, May, June, July, August, September, October, November, December}

    [System.Serializable] public enum Season { Summer, Autumn, Winter, Spring}

    [System.Serializable]
    public class TimeDay
    {
        [Header("Day")]
        public Day day = Day.Sunday;
        public int date = 1;
    }
    [System.Serializable]
    public class TimeMonth
    {
        [Header("Month")]
        public Month month = Month.January;
        public int endOfMonth = 31;

        public Season season = Season.Summer;
    }

    [Header("Time")]
    public float timeMultiplier = 1f;
    [Range(0, 24)] public float timeOfDay;

    public Date currentDate;
    //public TimeDay currentDay;
    //public TimeMonth currentMonth;
    //public int year;
    public TimeHourMinSec currentTime;

    [System.Serializable]
    public class TimeHourMinSec
    {
         public int timeHours;
         public float timeMinutes;
         public float timeSeconds;
    }

    [Header("UI")]
    public bool showSeconds = true;
    public bool twelveHourTime;
    [Tooltip("False = Southern, True = Northern")]
    public bool hemisphere = false;
    private bool isNewDay;

    public TextMeshProUGUI timeText, dayText;

    [SerializeField] private Light directionalLight;
    [SerializeField] private LightingPreset preset;

    private WeatherController weatherController;

    [System.Serializable]
    public class Date 
    {
        public TimeDay day;
        public TimeMonth month; 
        public int year; 
    }

    
    void Start()
    {

        //weatherController = FindObjectOfType<WeatherController>();
        weatherController = WeatherController.instance;

        dayText.text = currentDate.day.day.ToString() + ", " + currentDate.month.month.ToString() + " " + currentDate.day.date;

        SetMonthVariables(currentDate.month);

        WeatherController.instance.SetSeasonalConditions();

    }

    void Update()
    {
        if (preset == null)
        {
            return;
        }

        if (Application.isPlaying)
        {
            timeOfDay += Time.deltaTime * timeMultiplier;

            timeOfDay %= 24;
            UpdateLighting(timeOfDay / 24f);

            currentTime.timeHours = (int)timeOfDay;
            currentTime.timeMinutes = (timeOfDay - currentTime.timeHours) * 60;
            currentTime.timeSeconds = (currentTime.timeMinutes - (int)currentTime.timeMinutes) * 60;

            

            if (timeOfDay < 12)
            {
                if (showSeconds)
                    timeText.text = currentTime.timeHours.ToString("00") + ":" + currentTime.timeMinutes.ToString("00") + ":" + currentTime.timeSeconds.ToString("00");
                else
                    timeText.text = currentTime.timeHours.ToString("00") + ":" + currentTime.timeMinutes.ToString("00");

                if (twelveHourTime)
                {
                    timeText.text = timeText.text + " am";
                }
            }
            else
            {
                if (!twelveHourTime)
                {
                    if (showSeconds)
                        timeText.text = currentTime.timeHours.ToString("00") + ":" + currentTime.timeMinutes.ToString("00") + ":" + currentTime.timeSeconds.ToString("00");
                    else
                        timeText.text = currentTime.timeHours.ToString("00") + ":" + currentTime.timeMinutes.ToString("00");
                }
                else
                {
                    if (showSeconds)
                        timeText.text = (currentTime.timeHours - 12).ToString("00") + ":" + currentTime.timeMinutes.ToString("00") + ":" + currentTime.timeSeconds.ToString("00");
                    else
                        timeText.text = (currentTime.timeHours - 12).ToString("00") + ":" + currentTime.timeMinutes.ToString("00");

                    timeText.text = timeText.text + " pm";
                }

                if (timeOfDay > 23.99f)
                {
                    isNewDay = true;
                }
            }

            if (isNewDay && timeOfDay < 1f)
            {
                SetDayVariables(currentDate);
                SetNewDay();
            }
        }
    }

    public Date SetDayVariables(Date date)
    {
        if (date.day.day < Day.Sunday)
            date.day.day++;
        else
            date.day.day = Day.Monday;
        
        if (date.day.date < date.month.endOfMonth)
        {
            date.day.date += 1;
        }
        else
        {
            if (date.month.month < Month.December)
                date.month.month++;
            else
                date.month.month = Month.January;

            SetMonthVariables(date.month);

            date.day.date = 1;
        }
        return date;

    }

    public Date GetDateInFuture(Date startDate, int daysInFuture)
    {
        Date trackedDate = new Date();
        trackedDate.day = startDate.day;
        trackedDate.month = startDate.month;

        if (daysInFuture > 0)
        {
            for (int i = 1; i < daysInFuture; i++)
            {
                SetDayVariables(trackedDate);

            }

            //Debug.Log(trackedDate.day.day + ", " + trackedDate.month.month + " " + trackedDate.day.date);
        }
        Debug.Log(trackedDate.day.day + ", " + trackedDate.month.month + " " + trackedDate.day.date);

        return trackedDate;

    }


    void SetNewDay()
    {
        dayText.text = currentDate.day.day.ToString() + ", " + currentDate.month.month.ToString() + " " + currentDate.day.date;

        weatherController.SetDailyConditions();

        isNewDay = false;
    }

    public void SetNewMonth(TimeMonth month)
    {
        if (month.month < Month.December)
            month.month++;
        else
            month.month = Month.January;

        SetMonthVariables(month);

    }

    public void SetMonthVariables(TimeMonth month)
    {
        switch (hemisphere)
        {
            case false:
                switch (month.month)
                {
                    case Month.January:
                        month.endOfMonth = 31;
                        month.season = Season.Summer;
                        break;
                    case Month.Feburary:
                        month.endOfMonth = 28;
                        month.season = Season.Summer;
                        break;
                    case Month.March:
                        month.endOfMonth = 31;
                        month.season = Season.Autumn;
                        break;
                    case Month.April:
                        month.endOfMonth = 30;
                        month.season = Season.Autumn;
                        break;
                    case Month.May:
                        month.endOfMonth = 31;
                        month.season = Season.Autumn;
                        break;
                    case Month.June:
                        month.endOfMonth = 30;
                        month.season = Season.Winter;
                        break;
                    case Month.July:
                        month.endOfMonth = 31;
                        month.season = Season.Winter;
                        break;
                    case Month.August:
                        month.endOfMonth = 31;
                        month.season = Season.Winter;
                        break;
                    case Month.September:
                        month.endOfMonth = 30;
                        month.season = Season.Spring;
                        break;
                    case Month.October:
                        month.endOfMonth = 31;
                        month.season = Season.Spring;
                        break;
                    case Month.November:
                        month.endOfMonth = 30;
                        month.season = Season.Spring;
                        break;
                    case Month.December:
                        month.endOfMonth = 31;
                        month.season = Season.Summer;
                        break;
                }
                break;
            case true:
                switch (month.month)
                {
                    case Month.January:
                        month.endOfMonth = 31;
                        month.season = Season.Winter;
                        break;
                    case Month.Feburary:
                        month.endOfMonth = 28;
                        month.season = Season.Winter;
                        break;
                    case Month.March:
                        month.endOfMonth = 31;
                        month.season = Season.Spring;
                        break;
                    case Month.April:
                        month.endOfMonth = 30;
                        month.season = Season.Spring;
                        break;
                    case Month.May:
                        month.endOfMonth = 31;
                        month.season = Season.Spring;
                        break;
                    case Month.June:
                        month.endOfMonth = 30;
                        month.season = Season.Summer;
                        break;
                    case Month.July:
                        month.endOfMonth = 31;
                        month.season = Season.Summer;
                        break;
                    case Month.August:
                        month.endOfMonth = 31;
                        month.season = Season.Summer;
                        break;
                    case Month.September:
                        month.endOfMonth = 30;
                        month.season = Season.Autumn;
                        break;
                    case Month.October:
                        month.endOfMonth = 31;
                        month.season = Season.Autumn;
                        break;
                    case Month.November:
                        month.endOfMonth = 30;
                        month.season = Season.Autumn;
                        break;
                    case Month.December:
                        month.endOfMonth = 31;
                        month.season = Season.Winter;
                        break;
                }
                break;
        }
    } 
    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = preset.ambientColour.Evaluate(timePercent);
        RenderSettings.fogColor = preset.fogColour.Evaluate(timePercent);

        if (directionalLight != null)
        {
            directionalLight.color = preset.directionalColour.Evaluate(timePercent);
            directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360) - 90f, 170f, 0));
        }
    }

    private void OnValidate()
    {
        if (directionalLight != null)
            return;

        if (RenderSettings.sun != null)
        {
            directionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();

            foreach (Light light in lights)
            {
                directionalLight = light;
                return;
            }
        }
    }

    //public Calendar SetCalendar()
    //{
    //    Calendar calendar = new Calendar();

    //    for (int m = 0; m < 12; m++)
    //    {
    //        calendar.calendarMonths.Add(new CalendarMonth());
    //    }

    //    for (int m = 0; m < calendar.calendarMonths.Count; m++)
    //    {
    //        switch (m)
    //        {
    //            case 0: 
    //                calendar.calendarMonths[m].month.month = Month.January;
    //                calendar.calendarMonths[m].month.season = Season.Summer;
    //                calendar.calendarMonths[m].month.endOfMonth = 31;
    //                break;
    //            case 1: 
    //                calendar.calendarMonths[m].month.month = Month.Feburary; 
    //                calendar.calendarMonths[m].month.season = Season.Summer;
    //                calendar.calendarMonths[m].month.endOfMonth = 28;
    //                break;
    //            case 2: 
    //                calendar.calendarMonths[m].month.month = Month.March;
    //                calendar.calendarMonths[m].month.season = Season.Autumn;
    //                calendar.calendarMonths[m].month.endOfMonth = 31;
    //                break;
    //            case 3: 
    //                calendar.calendarMonths[m].month.month = Month.April; 
    //                calendar.calendarMonths[m].month.season = Season.Autumn;
    //                calendar.calendarMonths[m].month.endOfMonth = 30;
    //                break;
    //            case 4: 
    //                calendar.calendarMonths[m].month.month = Month.May; 
    //                calendar.calendarMonths[m].month.season = Season.Autumn;
    //                calendar.calendarMonths[m].month.endOfMonth = 31;
    //                break;
    //            case 5: 
    //                calendar.calendarMonths[m].month.month = Month.June; 
    //                calendar.calendarMonths[m].month.season = Season.Winter;
    //                calendar.calendarMonths[m].month.endOfMonth = 30;
    //                break;
    //            case 6: 
    //                calendar.calendarMonths[m].month.month = Month.July; 
    //                calendar.calendarMonths[m].month.season = Season.Winter;
    //                calendar.calendarMonths[m].month.endOfMonth = 31;
    //                break;
    //            case 7: 
    //                calendar.calendarMonths[m].month.month = Month.August; 
    //                calendar.calendarMonths[m].month.season = Season.Winter;
    //                calendar.calendarMonths[m].month.endOfMonth = 31;
    //                break;
    //            case 8: 
    //                calendar.calendarMonths[m].month.month = Month.September; 
    //                calendar.calendarMonths[m].month.season = Season.Spring;
    //                calendar.calendarMonths[m].month.endOfMonth = 30;
    //                break;
    //            case 9: 
    //                calendar.calendarMonths[m].month.month = Month.October; 
    //                calendar.calendarMonths[m].month.season = Season.Spring;
    //                calendar.calendarMonths[m].month.endOfMonth = 31;
    //                break;
    //            case 10: 
    //                calendar.calendarMonths[m].month.month = Month.November; 
    //                calendar.calendarMonths[m].month.season = Season.Spring;
    //                calendar.calendarMonths[m].month.endOfMonth = 30;
    //                break;
    //            case 11: 
    //                calendar.calendarMonths[m].month.month = Month.December; 
    //                calendar.calendarMonths[m].month.season = Season.Summer;
    //                calendar.calendarMonths[m].month.endOfMonth = 31;
    //                break;
    //        }
    //    }

    //    List<Day> daysInYear = new List<Day>();

    //    for (int i = 0; i < 365; i++)
    //    {
    //        if (i == 0)
    //        {
    //            daysInYear.Add(Day.Monday);
    //        }
    //        else daysInYear.Add(SetDayOfWeek(daysInYear[i - 1]));
    //    }

    //    int currentDayCount = 0;

    //    for (int i = 0; i < calendar.calendarMonths.Count; i++)
    //    {

    //        for (int d = 0; d < calendar.calendarMonths[i].month.endOfMonth; d++)
    //        {
    //            calendar.calendarMonths[i].calendarDays.Add(new TimeDay());
    //        }

    //        for (int d = 0; d < calendar.calendarMonths[i].calendarDays.Count; d++)
    //        {
    //            calendar.calendarMonths[i].calendarDays[d].day = daysInYear[currentDayCount];
    //            currentDayCount++;
    //            calendar.calendarMonths[i].calendarDays[d].date = d + 1;

    //            //for (int h = 0; h < 24; h++)
    //            //{
    //            //    CalendarHour hour = new CalendarHour();
    //            //    hour.hourOfDay = h;
    //            //    calendar.calendarMonths[i].calendarDays[d].calendarHours.Add(new CalendarHour());
    //            //}
    //        }
    //    }

    //    return calendar;
    //}

    //public Day SetDayOfWeek(Day yesterday)
    //{
    //    switch (yesterday)
    //    {
    //        case Day.Sunday: return Day.Monday;
    //        case Day.Monday: return Day.Tuesday;
    //        case Day.Tuesday: return Day.Wednesday;
    //        case Day.Wednesday: return Day.Thursday;
    //        case Day.Thursday: return Day.Friday;
    //        case Day.Friday: return Day.Saturday;
    //        case Day.Saturday: return Day.Sunday;
    //        default: return Day.Monday;
    //    }
    //}

    //public class CalendarDate
    //{
    //    [Tooltip("x = day, y = month")]
    //    public Vector2 date;
    //}
}

[CustomEditor(typeof(TimeController))]
public class TimeControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TimeController timeController = (TimeController)target;
        if (timeController == null) return;

        GUILayout.Space(20);


        if (GUILayout.Button("Progress Day"))
        {
            timeController.SetDayVariables(timeController.currentDate);
        } 
        
        if (GUILayout.Button("Progress Month"))
        {
            timeController.SetNewMonth(timeController.currentDate.month);
        }
    }
}


