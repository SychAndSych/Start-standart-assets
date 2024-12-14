using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Other / Altitude indicator")]
    [DisallowMultipleComponent]
public class Altitude_indicator : MonoBehaviour
{
    [Tooltip("Цель за которой следим")]
    [SerializeField]
    Transform Target = null;

    [Tooltip("Самая низкая точка от которой будет идти отсчёт")]
    [SerializeField]
    Vector3 Start_position = Vector3.zero;

    [Tooltip("Показатель высоты")]
    [SerializeField]
    TMP_Text Value_text = null;

    [Tooltip("Умножает действительность (техническое приувеличение того, что есть)")]
    [SerializeField]
    float Multiplication_of_reality = 1f;
    private void Start()
    {
        Target = Game_administrator.Singleton_Instance.Player_administrator.Player_sc.transform;
    }

    private void Update()
    {
        if (Target)
        {
            float value = Mathf.RoundToInt((Target.position.y - Start_position.y) * Multiplication_of_reality);
            value = value >= 0 ? value : 0;

            Value_text.text = value.ToString();
        }
    }
}
}