using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class TimeController : MonoBehaviour
{

    [SerializeField] private float timeMultiplier;

    [SerializeField, Range(0,24)] private float startHour;

    [SerializeField] private TextMeshProUGUI timeText;

    [SerializeField] private Light sunLight;

    [SerializeField] private float sunriseHour;
    [SerializeField] private float sunsetHour;

    [SerializeField] private Color dayAmbientLight;
    [SerializeField] private Color nightAmbientLight;

    [SerializeField] private AnimationCurve lightChangeCurve;

    [SerializeField] private float maxLightIntensity;
    [SerializeField] private Light moonLight;
    [SerializeField] private float maxMoonIntensity;


    private DateTime currentTime;

    private TimeSpan sunriseTime;
    private TimeSpan sunsetTime;


    // Start is called before the first frame update
    void Start()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);

        sunriseTime = TimeSpan.FromHours(sunriseHour);
        sunsetTime = TimeSpan.FromHours(sunsetHour);
        
    }

    // Update is called once per frame
    void Update()
    {

        if(Application.isPlaying)
        {
            UpdateTimeOfDay();
            RotateSun();
            UpdateLightSettings();
        }
        else{
            UpdateStartSettings(startHour);
        }
    }

    private void UpdateTimeOfDay()
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);

        if(timeText != null)
            timeText.text = currentTime.ToString("HH:mm");
    
    }

    private void RotateSun()
    {
        float sunLightRotation;

        if(currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime)
        {
            TimeSpan sunriseToSunsetDuration = CalcTimeDifference(sunriseTime, sunsetTime);
            TimeSpan timeSinceSunrise = CalcTimeDifference(sunriseTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(0, 180, (float)percentage);
        }

        else
        {
            TimeSpan sunsetToSunriseDuration = CalcTimeDifference(sunsetTime, sunriseTime);
            TimeSpan timeSinceSunset = CalcTimeDifference(sunsetTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);
        }

        sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
    }

    private void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down);
        sunLight.intensity = Mathf.Lerp(0, maxLightIntensity, lightChangeCurve.Evaluate(dotProduct));
        moonLight.intensity = Mathf.Lerp(maxMoonIntensity, 0, lightChangeCurve.Evaluate(dotProduct));
        RenderSettings.ambientLight = Color.Lerp(nightAmbientLight, dayAmbientLight, lightChangeCurve.Evaluate(dotProduct));
    }

    private void UpdateStartSettings(float timePercent)
    {
        // float dotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down);
        sunLight.intensity = Mathf.Lerp(0, maxLightIntensity, lightChangeCurve.Evaluate(timePercent));
        moonLight.intensity = Mathf.Lerp(maxMoonIntensity, 0, lightChangeCurve.Evaluate(timePercent));
        RenderSettings.ambientLight = Color.Lerp(nightAmbientLight, dayAmbientLight, lightChangeCurve.Evaluate(timePercent));
    }

    private TimeSpan CalcTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan difference = toTime - fromTime;

        if (difference.TotalSeconds < 0)
            difference += TimeSpan.FromHours(24);

        return difference;

    }
}
