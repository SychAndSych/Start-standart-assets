using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Lean.Pool;

namespace Sych_scripts
{
    [DisallowMultipleComponent]
    public class UI_task_administrator : MonoBehaviour
    {
        #region Переменные
        [Tooltip("Внутренее (отдельное меню) тело куда складировать задачи")]
        [SerializeField]
        Transform Content_transform = null;

        [Tooltip("Префаб задания (Для внутреннего меню)")]
        [SerializeField]
        UI_quest Quest_prefab = null;

        [field: Tooltip("Текст описания квеста")]
        [field: SerializeField]
        public TMP_Text Description_text { get; private set; } = null;

        List<Active_task_class> UI_task_list = new List<Active_task_class>();

        bool Init = false;//Инициализация при подписках
        #endregion

        #region Системные методы

        private void OnEnable()
        {
            if (Quest_system.Singleton_Instance && !Init)
            {
                Quest_system.Singleton_Instance.Add_quest_event.AddListener(Add_new_task);
                Quest_system.Singleton_Instance.Remove_quest_event.AddListener(Remove_task);
                Quest_system.Singleton_Instance.End_task_event.AddListener(End_subtask);
                Init = true;
            }
        }

        private void Start()
        {
            if (!Init)
            {
                Quest_system.Singleton_Instance.Add_quest_event.AddListener(Add_new_task);
                Quest_system.Singleton_Instance.Remove_quest_event.AddListener(Remove_task);
                Quest_system.Singleton_Instance.End_task_event.AddListener(End_subtask);
                Init = true;
            }
        }

        private void OnDisable()
        {
            if (Init)
            {
                Quest_system.Singleton_Instance.Add_quest_event.RemoveListener(Add_new_task);
                Quest_system.Singleton_Instance.Remove_quest_event.RemoveListener(Remove_task);
                Quest_system.Singleton_Instance.End_task_event.RemoveListener(End_subtask);
                Init = false;
            }
        }

        #endregion


        #region Методы
        /// <summary>
        /// Добавить задачу
        /// </summary>
        void Add_new_task(Quest_SO _quest_SO)
        {
            UI_task_list.Add(new Active_task_class());
            UI_task_list[UI_task_list.Count - 1].Quest_Data = _quest_SO;

            UI_task_list[UI_task_list.Count - 1].UI_quest_ = LeanPool.Spawn(Quest_prefab, Content_transform);
            UI_task_list[UI_task_list.Count - 1].UI_quest_.Add_quest_SO(_quest_SO);
        }

        /// <summary>
        /// Убрать задачу
        /// </summary>
        void Remove_task(Quest_SO _quest_SO)
        {
            foreach (Active_task_class task_class in UI_task_list)
            {
                if (_quest_SO == task_class.Quest_Data)
                {
                    task_class.UI_quest_.Clear();
                    LeanPool.Despawn(task_class.UI_quest_);
                    break;
                }
            }
            
        }

        /// <summary>
        /// Закончить подзадачи
        /// </summary>
        /// <param name="_quest_SO_data">Данные квеста и его подзадач</param>
        /// <param name="_id_task">ID подзадачи</param>
        /// <param name="_win">Успешно закончен?</param>
        void End_subtask(Quest_SO _quest_SO_data, int _id_task, bool _win)
        {
            foreach (Active_task_class task_class in UI_task_list)
            {
                if (_quest_SO_data == task_class.Quest_Data)
                {
                    if(_win)
                        task_class.UI_quest_.Task_win_completed(_id_task);
                    else
                        task_class.UI_quest_.Task_lose_completed(_id_task);

                    break;
                }
            }
        }

        #endregion


        [System.Serializable]
        public class Active_task_class
        {
            [Tooltip("Квест")]
            [SerializeField]
            public Quest_SO Quest_Data = null;

            [Tooltip("Задача")]
            [SerializeField]
            public UI_quest UI_quest_ = null;
        }
    }
}
