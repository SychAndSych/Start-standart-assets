using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace Sych_scripts
{
    [DisallowMultipleComponent]
    public class Charged : MonoBehaviour
    {
        [field: Space(10)]
        [field: Tooltip("Включить зарядку")]
        [field: SerializeField]
        public bool Active_charged_bool { get; private set; } = false;

        [ShowIf(nameof(Active_charged_bool))]
        [Tooltip("Время за которое выстрел зарядится до полной мощности")]
        [SerializeField]
        float Charged_shot_time = 2f;

        [ShowIf(nameof(Active_charged_bool))]
        [Tooltip("На сколько этапов делится зарядка выстрела")]
        [SerializeField]
        int Charged_shot_step = 3;

        internal int Charged_shot_step_active { get; private set; } = 0;//Параметр показывающий текущий этап заряда

        [Tooltip("Эвент зарядки в float")]
        [SerializeField]
        UnityEvent<float> Charged_value_event = new UnityEvent<float>();

        [Tooltip("Эвент зарядки в этапах (int)")]
        [SerializeField]
        UnityEvent<int> Charged_step_event = new UnityEvent<int>();

        internal float Charged_shot_value_active { get; private set; } = 0;//Параметр который нужен для подсчёта заряженности


        /// <summary>
        /// Сбросить значение по умолчанию
        /// </summary>
        public void Reset_method()
        {
            Charged_shot_step_active = 0;
            Charged_shot_value_active = 0;
        }


        /// <summary>
        /// Заряжается
        /// </summary>
        public void Charger()
        {
            if (Charged_shot_time >= 1)
                Charged_shot_value_active += Time.deltaTime / Charged_shot_time;
            else
                Charged_shot_value_active += (1 - Charged_shot_value_active);

            Charged_shot_value_active = Mathf.Clamp(Charged_shot_value_active, 0f, 1f);

            Charged_shot_step_active = Mathf.RoundToInt((float)Charged_shot_step * Charged_shot_value_active);

            Charged_value_event.Invoke(Charged_shot_value_active);
            Charged_step_event.Invoke(Charged_shot_step_active);
        }

    }
}