using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeatherController : MonoBehaviour
{
    public static WeatherController instance;
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

    [System.Serializable] public enum WeatherCondition { Clear, Cloudy, Overcast, Sprinkling, Raining, Storming }

    public WeatherCondition weatherCondition;

    public float temperature;
    public float chanceOfRain;
    //public float sunLightLevels;

    public SeasonConditions currentSeasonConditions;

    public List<SeasonConditions> seasonConditions;

    private TimeController timeController;

    public List<HourlyConditions> hourlyConditions;

    public TextMeshProUGUI tempText;
    public TextMeshProUGUI chanceOfRainText;
    public TextMeshProUGUI weatherConditionText;

    private void Start()
    {


        //timeController = FindObjectOfType<TimeController>();
        timeController = TimeController.instance;

        SetSeasonalConditions();
        SetDailyConditions();
    }



    public void SetSeasonalConditions()
    {
        for (int i = 0; i < seasonConditions.Count; i++)
        {
            Debug.Log("It is now " + timeController.currentDate.month.season);

            if (timeController.currentDate.month.season == seasonConditions[i].season)
            {
                currentSeasonConditions = seasonConditions[i];
                return;
            }
        }
    }

    public void SetDailyConditions()
    {
        float highTemp = Random.Range(currentSeasonConditions.tempRange.x + (currentSeasonConditions.tempRange.x / 2), currentSeasonConditions.tempRange.y);
        float lowTemp = Random.Range(currentSeasonConditions.tempRange.x, currentSeasonConditions.tempRange.y - (currentSeasonConditions.tempRange.y / 2));

        int hottestTime = Random.Range((int)currentSeasonConditions.hottestTimeRange.x, (int)currentSeasonConditions.hottestTimeRange.y);
        int coldestMorningTime = Random.Range((int)currentSeasonConditions.coldestMorningTimeRange.x, (int)currentSeasonConditions.coldestMorningTimeRange.y);
        int coldestNightTime = Random.Range((int)currentSeasonConditions.coldestNightTimeRange.x, (int)currentSeasonConditions.coldestNightTimeRange.y);

        float heighestRainChance = Random.Range((int)currentSeasonConditions.chanceOfRainRange.x, (int)currentSeasonConditions.chanceOfRainRange.y);
        int heighestRainTime = Random.Range(0, 23);

        Debug.Log("Heighest rain chance = " + heighestRainChance + " at " + heighestRainTime);

        //Debug.Log("high temp = " + highTemp + "low temp = " + lowTemp);
        //Debug.Log("high humidity = " + highHumidity + "low humidity = " + lowHumidity);

        //Debug.Log("coldest morning time = " + coldestMorningTime);
        //Debug.Log("Hottest time = " + hottestTime);
        //Debug.Log("coldest night time = " + coldestNightTime);

        hourlyConditions = new List<HourlyConditions>(24);

        for (int i = 0; i < 24; i++)
        {
            HourlyConditions hourly = new HourlyConditions();

            hourly.forcastTime = i;

            bool isRaining = false;
            float rainRandomiser = Random.Range(0f, 100f);

            // Set chance of rain for each hour
            if (i == heighestRainTime) //Set highest chance of rain
            {
                hourly.chanceOfRain = heighestRainChance;

            }
            else if (i < heighestRainTime) // lerp the rest based on the highest time
            {
                if (i > 0)
                {
                    hourly.chanceOfRain = Mathf.Lerp(hourlyConditions[i - 1].chanceOfRain, heighestRainChance, 1f / (heighestRainTime - hourly.forcastTime));
                }
                else
                {
                    hourly.chanceOfRain = Mathf.Lerp(chanceOfRain, heighestRainChance, 1f / (heighestRainTime - hourly.forcastTime));
                }
            }
            else if (i > heighestRainTime)
            {
                //if (i < 23)
                //{
                    hourly.chanceOfRain = Mathf.Lerp(chanceOfRain, Random.Range(0, hourlyConditions[i - 1].chanceOfRain), 1f / (hourly.forcastTime - heighestRainTime));
                //}
            }

            //Set Weather Conditions
            if (rainRandomiser <= hourly.chanceOfRain) // check whether it is raining
            {
                isRaining = true;
            }

            
            if (isRaining) //Set Conditions based on chance of rain & whether it is raining
            {
                switch(hourly.chanceOfRain)
                {
                    case float rc when (rc <= 33f):
                        hourly.weatherCondition = WeatherCondition.Sprinkling;
                        break;

                    case float rc when (rc > 33f && rc <= 66f):
                        hourly.weatherCondition = WeatherCondition.Raining;
                        break;
                        
                    case float rc when (rc > 66f):
                        hourly.weatherCondition = WeatherCondition.Storming;
                        break;

                }
            }
            else
            {
                switch (hourly.chanceOfRain)
                {
                    case float rc when (rc <= 33f):
                        hourly.weatherCondition = WeatherCondition.Clear;
                        //sunLightLevels = Mathf.Lerp(0f, 100f, timeController.timeOfDay);
                        break;

                    case float rc when (rc > 33f && rc <= 66f):
                        hourly.weatherCondition = WeatherCondition.Cloudy;
                        //sunLightLevels = Mathf.Lerp(0f, 50f, timeController.timeOfDay);

                        break;

                    case float rc when (rc > 66f):
                        hourly.weatherCondition = WeatherCondition.Overcast;
                        //sunLightLevels = Mathf.Lerp(0f, 25f, timeController.timeOfDay);
                        break;

                }
            }

            if (i == coldestMorningTime) //set the temp for the coldest time in the morning
            {
                hourly.temp = lowTemp;
            }
            else if (i == hottestTime) //set the temp for the hottest time of the day
            {
                hourly.temp = highTemp;
            }
            else if (i == coldestNightTime) //set the temp for the coldest time in the night
            {
                hourly.temp = lowTemp;
            }
            else // lerp unspecifed temps between the highs and lows
            {
                if (i <= coldestMorningTime)
                {
                    hourly.temp = coldestMorningTime;
                }
                else if (i > coldestMorningTime && i <= hottestTime)
                {
                    hourly.temp = Mathf.Lerp(hourlyConditions[i - 1].temp, highTemp, 1f / (hottestTime - hourly.forcastTime));
                }
                else if (i > hottestTime && i < coldestNightTime)
                {
                    hourly.temp = Mathf.Lerp(hourlyConditions[i - 1].temp, lowTemp, 1f / (coldestNightTime - hourly.forcastTime));
                }
                else if (i >= coldestNightTime)
                {
                    hourly.temp = coldestNightTime;
                }
            }

            hourlyConditions.Add(hourly);
        }

        temperature = hourlyConditions[timeController.currentTime.timeHours].temp;

        chanceOfRain = hourlyConditions[timeController.currentTime.timeHours].chanceOfRain;

        weatherCondition = hourlyConditions[timeController.currentTime.timeHours].weatherCondition;

        //Debug.Log("New Weather Conditions Set \n Temperature = " + temperature); // + ", Humidity = " + humidity);
    }


    private void Update()
    {
        if (hourlyConditions != null)
        {
            if (timeController.currentTime.timeHours < 23)
            {
                temperature = Mathf.Lerp(hourlyConditions[timeController.currentTime.timeHours].temp, hourlyConditions[timeController.currentTime.timeHours + 1].temp, 1f / timeController.timeOfDay);
                tempText.text = temperature.ToString("00") + "°C";

                chanceOfRain = Mathf.Lerp(hourlyConditions[timeController.currentTime.timeHours].chanceOfRain, hourlyConditions[timeController.currentTime.timeHours + 1].chanceOfRain, 1f / timeController.timeOfDay);
                chanceOfRainText.text = chanceOfRain.ToString("00") + "% Chance of Rain";
            }

            weatherConditionText.text = "Weather: " +hourlyConditions[timeController.currentTime.timeHours].weatherCondition.ToString();
        }
    }



}

[System.Serializable]
public class SeasonConditions
{
    public TimeController.Season season;
    public Vector2 tempRange;
    public Vector2 chanceOfRainRange;
    public Vector2 hottestTimeRange;
    public Vector2 coldestMorningTimeRange;
    public Vector2 coldestNightTimeRange;
}

[System.Serializable]
public class HourlyConditions
{
    public float temp;
    public float chanceOfRain;
    public WeatherController.WeatherCondition weatherCondition;
    public int forcastTime;
}
