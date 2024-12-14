//Спавнит чего либо
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using NaughtyAttributes;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Other / Spawner")]
    [DisallowMultipleComponent]
    public class Spawner : MonoBehaviour
    {
        #region Переменные
        [Tooltip("Префаб")]
        [SerializeField]
        GameObject Prefab = null;

        [Tooltip("Точка спавна")]
        [SerializeField]
        Transform Point_spawn = null;

        [Tooltip("Включить при старте")]
        [SerializeField]
        bool Start_bool = false;

        [Tooltip("Не поворачивать заспавненый объект как точка спавна")]
        [SerializeField]
        bool No_rotation_spawn_object_bool = false;

        [Tooltip("Таймер спавна")]
        [SerializeField]
        bool Timer_bool = false;

        [ShowIf(nameof(Timer_bool))]
        [Tooltip("Время через которое спавнит")]
        [SerializeField]
        float Time_timer = 1f;

        [ShowIf(nameof(Timer_bool))]
        [Tooltip("Зациклить спавн")]
        [SerializeField]
        bool Loop_bool = false;

        [ShowIf(nameof(Timer_bool))]
        [Tooltip("Заспавнить определённое количество")]
        [SerializeField]
        bool Count_spawn_bool = false;

        [ShowIfNew(ActionOnConditionFail.DONT_DRAW, ConditionOperator.AND, nameof(Count_spawn_bool), nameof(Timer_bool))]
        [Tooltip("Сколько раз заспавнить")]
        [SerializeField]
        int Count = 10;

        Coroutine Spawn_coroutine = null;

        int Count_active = 0;
        #endregion


        #region Системные методы
        private void Start()
        {
            if (Start_bool)
                Activation();
        }
        #endregion


        #region Методы
        IEnumerator Coroutine_spawn()
        {
            while (Loop_bool || Count > Count_active)
            {
                yield return new WaitForSeconds(Time_timer);

                Spawn();

                if (Count_spawn_bool)
                {
                    Count_active++;

                    if (Count <= Count_active)
                        Stop_loop_spawn();
                }
            }

            Spawn_coroutine = null;

        }

        /// <summary>
        /// Спавнить
        /// </summary>
        void Spawn()
        {
            Vector3 position = Point_spawn ? Point_spawn.position: transform.position;
            Quaternion rotation = Point_spawn ? Point_spawn.rotation : transform.rotation;

            if (No_rotation_spawn_object_bool)
                rotation = Quaternion.identity;

            LeanPool.Spawn(Prefab, position, rotation);
        }
        #endregion


        #region Публичные методы
        /// <summary>
        /// Активировать
        /// </summary>
        [ContextMenu("Активировать")]
        public void Activation()
        {
            if (Timer_bool)
            {
                if (Spawn_coroutine == null)
                {
                    Spawn_coroutine = StartCoroutine(Coroutine_spawn());
                    Count_active = 0;
                }

            }
            else
            {
                Spawn();
            }
        }

        /// <summary>
        /// Остановить зацикленый спавн
        /// </summary>
        public void Stop_loop_spawn()
        {
            Loop_bool = false;
        }
        #endregion

    }



}