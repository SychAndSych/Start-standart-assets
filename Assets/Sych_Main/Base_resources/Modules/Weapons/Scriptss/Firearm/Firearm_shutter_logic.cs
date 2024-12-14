using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace Sych_scripts
{
    [DisallowMultipleComponent]
    public class Firearm_shutter_logic : MonoBehaviour
    {
        #region Переменные
        [Space(10)]
        [Tooltip("Включить режим стрельбы с передёргиванием затвора (например для снайперской винтовки с затвором)")]
        [SerializeField]
        bool Use_shutter_bool = false;

        internal bool Shutter_bool { get; private set; } = true;//Передёрнул ли "затвор"

        [ShowIf(nameof(Use_shutter_bool))]
        [Tooltip("Дополнительное время на, то что бы оружие не сразу стреляло после передёргивания затвора (нужно, для того, что бы не словить баг с преждевременным выстрелом)")]
        [SerializeField]
        float Additional_time_stop_shutter_reload = 0.2f;

        [ShowIf(nameof(Use_shutter_bool))]
        [Tooltip("Событие относящиеся к началу передёргивания затвора (в основном для запуска анимации, которая в конце задействует End_shutter_reload)")]
        [SerializeField]
        UnityEvent Start_shutter_reload_event = new UnityEvent();

        #endregion


        #region Системные методы

        private void OnEnable()
        {
            Preparation_shutter();
        }


        #endregion


        #region Методы

        /// <summary>
        /// Проверить исполнительность (вдруг выключили по ходу работы)
        /// </summary>
        void Preparation_shutter()
        {
            if (Use_shutter_bool)
            {
                if (!Shutter_bool)
                {
                    Shutter_reload();
                }
            }
        }


        /// <summary>
        /// Дополнительное время на, то что бы оружие не сразу стреляло после передёргивания затвора (нужно, для того, что бы не словить баг с преждевременным выстрелом)
        /// </summary>
        void Additional_time_shutter_reload_method()
        {
            Shutter_bool = true;
        }

        #endregion


        #region Публичные методы

        /// <summary>
        /// Начало передёргивание затвора
        /// </summary>
        public void Shutter_reload()
        {
            if (Use_shutter_bool)
            {
                Shutter_bool = false;
                Start_shutter_reload_event.Invoke();
            }

        }

        /// <summary>
        /// Конец передёргивания "затвора"
        /// </summary>
        public void End_reload_shutter()
        {
            Invoke(nameof(Additional_time_shutter_reload_method), Additional_time_stop_shutter_reload);
        }

        /// <summary>
        /// Досрочный ресет
        /// </summary>
        public void Early_reset()
        {
            Shutter_bool = true;
        }
        #endregion

    }
}