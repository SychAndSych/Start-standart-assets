//Скрипт который нужен для объектов и NPC которые выдают квесты
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Quest / Agent task Quest")]
    [DisallowMultipleComponent]
    public class Agent_task_Quest : MonoBehaviour
    {
        #region Переменные

        [field: Tooltip("Id номер квеста")]
        [field: SerializeField]
        public int Id_quest { get; private set; } = -1;

        [field: Tooltip("Id номер задачи квеста")]
        [field: SerializeField]
        public int Id_task { get; private set; } = -1;


        [Space(20)]
        [Tooltip("Ивент при включение игры (скрипта)")]
        [SerializeField]
        UnityEvent Initialization_event = new UnityEvent();


        [Space(20)]
        [Tooltip("Ивент при активации этой части квеста")]
        [SerializeField]
        public UnityEvent Start_event = new UnityEvent();

        [Tooltip("Ивент при окончание этой части квеста")]
        [SerializeField]
        public UnityEvent End_event = new UnityEvent();

        bool Init = false;//Инициализация при подписках
        #endregion


        #region Системные методы
        private void OnEnable()
        {
            if (Quest_system.Singleton_Instance && !Init)
            {
                Quest_system.Singleton_Instance.Start_task_event.AddListener(Start_scenario);
                Init = true;
            }
        }

        private void Start()
        {
            if (!Init)
            {
                Quest_system.Singleton_Instance.Start_task_event.AddListener(Start_scenario);
                Init = true;
            }

            Initialization_event.Invoke();
        }

        private void OnDisable()
        {
            if (Init)
            {
                Quest_system.Singleton_Instance.Start_task_event.RemoveListener(Start_scenario);
                Init = false;
            }
        }
        #endregion


        #region Методы
        /// <summary>
        /// Ивент при активации этой части квеста
        /// </summary>
        void Start_scenario(int _id_quest, int _id_task)
        {
            if(_id_quest == Id_quest && _id_task == Id_task)
           Start_event.Invoke();

        }

        #endregion



        #region Публичные методы
        /// <summary>
        /// Начать квест
        /// </summary>
        /// <param name="_id_quest">Id номер квеста</param>
        public void Start_quest(int _id_quest)
        {
            Quest_system.Singleton_Instance.Add_quest(_id_quest);
        }

        /// <summary>
        /// Закончить подзадачу
        /// </summary>
        /// <param name="_win">Правильно закончили?</param>
        public void End_task(bool _win)
        {
            End_event.Invoke();
            Quest_system.Singleton_Instance.End_task_quest(Id_quest, Id_task, _win);
        }
        #endregion

    }
}