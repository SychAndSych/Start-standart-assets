using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Other / Run Timer")]
    [DisallowMultipleComponent]
public class Run_Timer : MonoBehaviour
{

    [Tooltip("Показатель времени")]
    [SerializeField]
    TMP_Text Value_text = null;

    [Tooltip("Для дебага увеличить или уменьшить время")]
    [SerializeField]
    int Debug_value = 0;
    private void Update()
    {
        float value = Debug_value + Mathf.RoundToInt(Time.time);

        float hours = Mathf.FloorToInt(value / (60 * 60));

        float minutes = Mathf.FloorToInt((value - hours * 60 * 60) / 60);

        float seconds = value - (hours * 60 * 60 + minutes * 60);

        string hours_string = hours >= 10 ? hours.ToString() : "0" + hours.ToString();
        string minutes_string = minutes >= 10 ? minutes.ToString() : "0" + minutes.ToString();
        string seconds_string = seconds >= 10 ? seconds.ToString() : "0" + seconds.ToString();

        value = value >= 0 ? value : 0;

        Value_text.text = hours_string + ":" + minutes_string + ":" + seconds_string;
    }
}
}