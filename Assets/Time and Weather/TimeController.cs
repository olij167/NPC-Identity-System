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


    public TimeDay currentDay;
    public TimeMonth currentMonth;
    public int year;
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
    private bool isNewDay;

    public TextMeshProUGUI timeText, dayText;

    [SerializeField] private Light directionalLight;
    [SerializeField] private LightingPreset preset;

    private WeatherController weatherController;

    [System.Serializable]
    public class Date 
    {
        public TimeDay date;
        public TimeMonth month; 
        public int year; 
    }

    public Date GetCurrentDate()
    {
        Date currentDate = new Date();
        currentDate.date = currentDay;
        currentDate.month = currentMonth;
        currentDate.year = year;
        return currentDate;
    } 
    public static List<Date> GetDateInFuture(Date startDate, int daysInFuture)
    {
        List<Date> daysBetween = new List<Date>();
        Date date;

        for (int i = 0; i < daysInFuture; i++)
        {
            date = new Date();
            daysBetween.Add(date);

            if (i == 0)
            {
                date.date = startDate.date;
                date.month = startDate.month;

                //daysBetween[i] = date;
            }
            else if (daysBetween[i - 1].date.date < daysBetween[i - 1].month.endOfMonth)
            {
                date.date = new TimeDay(); 
                date.date.date += 1;
            }
            else
            {
                switch (date.month.month)
                {
                    case Month.January: date.month.month = Month.Feburary; date.month.endOfMonth = 28; break;
                    case Month.Feburary: date.month.month = Month.March; date.month.endOfMonth = 31; break;
                    case Month.March: date.month.month = Month.April; date.month.endOfMonth = 30; break;
                    case Month.April: date.month.month = Month.May; date.month.endOfMonth = 31; break;
                    case Month.May: date.month.month = Month.June; date.month.endOfMonth = 30; break;
                    case Month.June: date.month.month = Month.July; date.month.endOfMonth = 31; break;
                    case Month.July: date.month.month = Month.August; date.month.endOfMonth = 31; break;
                    case Month.August: date.month.month = Month.September; date.month.endOfMonth = 30; break;
                    case Month.September: date.month.month = Month.October; date.month.endOfMonth = 31; break;
                    case Month.October: date.month.month = Month.November; date.month.endOfMonth = 30; break;
                    case Month.November: date.month.month = Month.December; date.month.endOfMonth = 31; break;
                    case Month.December: date.month.month = Month.January; date.month.endOfMonth = 31; break;
                }

                date.date = new TimeDay(); 
                date.date.date = 1;
            }

            if (i != 0)
                switch (daysBetween[i - 1].date.day)
                {
                    case Day.Sunday: date.date.day = Day.Monday; break;
                    case Day.Monday: date.date.day = Day.Tuesday; break;
                    case Day.Tuesday: date.date.day = Day.Wednesday; break;
                    case Day.Wednesday: date.date.day = Day.Thursday; break;
                    case Day.Thursday: date.date.day = Day.Friday; break;
                    case Day.Friday: date.date.day = Day.Saturday; break;
                    case Day.Saturday: date.date.day = Day.Sunday; break;
                }
        }

        return daysBetween;
    }

    void Start()
    {

        //weatherController = FindObjectOfType<WeatherController>();
        weatherController = WeatherController.instance;

        dayText.text = currentDay.day.ToString() + ", " + currentMonth.month.ToString() + " " + currentDay.date;

        SetMonthVariables(currentMonth);

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
                SetDay(currentDay,currentMonth);
            }
        }
    }

    void SetDay(TimeDay day, TimeMonth month)
    {
        if (day.day > Day.Sunday)
            day.day++;
        else
            day.day = Day.Monday;
        
        if (day.date < month.endOfMonth)
        {
            day.date++;
        }
        else
        {
            if (month.month < Month.December)
                month.month++;
            else
                month.month = Month.January;

            SetMonthVariables(month);
        }

        dayText.text = day.day.ToString() + ", " + month.month.ToString() + " " + day.date;

        weatherController.SetDailyConditions();

        isNewDay = false;
    }

    public void SetMonthVariables(TimeMonth month)
    {
        switch(month.month)
        {
            case Month.January:
                month.endOfMonth = 31;
                break;

            case Month.Feburary:
                month.endOfMonth = 28;
                break;

            case Month.March:
                month.endOfMonth = 31;
                month.season = Season.Autumn;
                WeatherController.instance.SetSeasonalConditions();
                break;

            case Month.April:
                month.endOfMonth = 30;
                break;

            case Month.May:
                month.endOfMonth = 31;
                break;

            case Month.June:
                month.endOfMonth = 30;
                month.season = Season.Winter;
                WeatherController.instance.SetSeasonalConditions();
                break;

            case Month.July:
                month.endOfMonth = 31;
                break;

            case Month.August:
                month.endOfMonth = 31;
                break;

            case Month.September:
                month.endOfMonth = 30;
                month.season = Season.Spring;
                WeatherController.instance.SetSeasonalConditions();
                break;

            case Month.October:
                month.endOfMonth = 31;
                break;

            case Month.November:
                month.endOfMonth = 30;
                break;

            case Month.December:
                month.endOfMonth = 31;
                month.season = Season.Summer;
                WeatherController.instance.SetSeasonalConditions();
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

//[CustomEditor(typeof(TimeController))]
//public class TimeControllerEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        TimeController timeController = (TimeController)target;
//        if (timeController == null) return;

//        if (GUILayout.Button("Set Calendar"))
//        {
//            timeController.calendar = timeController.SetCalendar();
//        }
//    }
//}


