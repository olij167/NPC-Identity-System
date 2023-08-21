using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [System.Serializable] public enum Month { January = 1, Feburary = 2, March = 3, April = 4, May = 5, June = 6, July = 7, August = 8, September = 9, October = 10, November = 11 , December = 12}

    [System.Serializable] public enum Season { Summer, Autumn, Winter, Spring}

    [Header("Day")]
    public Day currentDay = Day.Sunday;
    public int dayOfMonth = 1;

    [Header("Month")]
    public Month currentMonth = Month.January;
    public int endOfMonth = 30;

    public Season currentSeason = Season.Summer;

    [Header("Time")]
    public float timeMultiplier = 1f;
    [Range(0, 24)] public float timeOfDay;
    [HideInInspector] public int timeHours;
    [HideInInspector] public float timeMinutes;
    [HideInInspector] public float timeSeconds;

    [Header("UI")]
    public bool showSeconds = true;
    public bool twelveHourTime;
    private bool isNewDay;

    public TextMeshProUGUI timeText, dayText;

    [SerializeField] private Light directionalLight;
    [SerializeField] private LightingPreset preset;

    private WeatherController weatherController;


    void Start()
    {

        //weatherController = FindObjectOfType<WeatherController>();
        weatherController = WeatherController.instance;

        dayText.text = currentDay.ToString() + ", " + currentMonth.ToString() + " " + dayOfMonth;

        ChangeMonth();

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

            timeHours = (int)timeOfDay;
            timeMinutes = (timeOfDay - timeHours) * 60;
            timeSeconds = (timeMinutes - (int)timeMinutes) * 60;

            

            if (timeOfDay < 12)
            {
                if (showSeconds)
                    timeText.text = timeHours.ToString("00") + ":" + timeMinutes.ToString("00") + ":" + timeSeconds.ToString("00");
                else
                    timeText.text = timeHours.ToString("00") + ":" + timeMinutes.ToString("00");

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
                        timeText.text = timeHours.ToString("00") + ":" + timeMinutes.ToString("00") + ":" + timeSeconds.ToString("00");
                    else
                        timeText.text = timeHours.ToString("00") + ":" + timeMinutes.ToString("00");
                }
                else
                {
                    if (showSeconds)
                        timeText.text = (timeHours - 12).ToString("00") + ":" + timeMinutes.ToString("00") + ":" + timeSeconds.ToString("00");
                    else
                        timeText.text = (timeHours - 12).ToString("00") + ":" + timeMinutes.ToString("00");

                    timeText.text = timeText.text + " pm";
                }

                if (timeOfDay > 23.99f)
                {
                    isNewDay = true;
                }
            }

            if (isNewDay && timeOfDay < 1f)
            {
                ProgressDays();
            }
        }
    }

    void ProgressDays()
    {
        if (currentDay > Day.Sunday)
            currentDay++;
        else
            currentDay = Day.Monday;
        
        if (dayOfMonth < endOfMonth)
        {
            dayOfMonth++;
        }
        else
        {
            if (currentMonth < Month.December)
                currentMonth++;
            else
                currentMonth = Month.January;

            ChangeMonth();
        }

        dayText.text = currentDay.ToString() + ", " + currentMonth.ToString() + " " + dayOfMonth;

        weatherController.SetDailyConditions();

        isNewDay = false;
    }

    public void ChangeMonth()
    {
        switch(currentMonth)
        {
            case Month.January:
                endOfMonth = 31;
                break;

            case Month.Feburary:
                endOfMonth = 28;
                break;

            case Month.March:
                endOfMonth = 31;
                currentSeason = Season.Autumn;
                WeatherController.instance.SetSeasonalConditions();
                break;

            case Month.April:
                endOfMonth = 30;
                break;

            case Month.May:
                endOfMonth = 31;
                break;

            case Month.June:
                endOfMonth = 30;
                currentSeason = Season.Winter;
                WeatherController.instance.SetSeasonalConditions();
                break;

            case Month.July:
                endOfMonth = 31;
                break;

            case Month.August:
                endOfMonth = 31;
                break;

            case Month.September:
                endOfMonth = 30;
                currentSeason = Season.Spring;
                WeatherController.instance.SetSeasonalConditions();
                break;

            case Month.October:
                endOfMonth = 31;
                break;

            case Month.November:
                endOfMonth = 30;
                break;

            case Month.December:
                endOfMonth = 31;
                currentSeason = Season.Summer;
                WeatherController.instance.SetSeasonalConditions();
                break;
        }

        dayOfMonth = 1;
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
}


