using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / General / Test start activation")]
    [DisallowMultipleComponent]
    public class Test_start_activation : MonoBehaviour
    {
        [Tooltip("Задержка при срабатывание")]
        [SerializeField]
        float Time_delay = 0.1f;

        [Tooltip("Стартовый ивент")]
        [SerializeField]
        UnityEvent Start_event = new UnityEvent();


        void Start()
        {
            Invoke(nameof(Delay_start), Time_delay);
        }

        void Delay_start()
        {
            Start_event.Invoke();
        }
    }
}