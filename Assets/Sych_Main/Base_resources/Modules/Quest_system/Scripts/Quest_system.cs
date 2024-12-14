using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sych_scripts
{
    [AddComponentMenu("Sych scripts / Game / Quest / Quest system")]
    [DisallowMultipleComponent]
    public class Quest_system : Singleton<Quest_system>
    {
        #region Переменные
        [Tooltip("Массив квестов (все в общем)")]
        [SerializeField]
        Quest_SO[] Quest_Data_array = new Quest_SO[0];

        internal UnityEvent<Quest_SO> Add_quest_event = new UnityEvent<Quest_SO>();//Ивент на добавление задачи

        internal UnityEvent<Quest_SO> Remove_quest_event = new UnityEvent<Quest_SO>();//Ивент на убирание задачи

        internal UnityEvent<Quest_SO, int, bool> End_task_event = new UnityEvent<Quest_SO, int, bool>();//Ивент на окончание пунктов задачи

        internal UnityEvent<int, int> Start_task_event = new UnityEvent<int, int>();//Старт части квеста (активирует агент который соответствует части квеста)

        [Tooltip("Активные квесты")]
        List<Quest_SO> Active_quest_list = new List<Quest_SO>();

        #endregion


        #region Проверяющие методы
        /// <summary>
        /// Проверить есть ли квест уже в активном списке
        /// </summary>
        bool Check_quest_in_active_list(int _id_quest)
        {
                bool result = false;

                foreach (Quest_SO quest in Active_quest_list)
                {
                    if (Quest_Data_array[_id_quest] == quest)
                    {
                        result = true;
                        break;
                    }
                }

                return result;
        }

        /// <summary>
        /// Закончить квест
        /// </summary>
        /// <param name="_id_quest">Id номер квеста</param>
        /// <param name="_win">Успешно выполнен?</param>
        void End_quest(int _id_quest, bool _win)
        {
            Remove_quest(_id_quest);

            if (Quest_Data_array[_id_quest].Name_id_quest != "")
                Save_PlayerPrefs.Save_parameter(Type_parameter_bool.Quest_completed_bool, Quest_Data_array[_id_quest].Name_id_quest, true);

                if (_id_quest > -1)
                    Save_PlayerPrefs.Save_parameter(Type_parameter_bool.Quest_completed_bool, _id_quest, true);
                else
                    Debug.LogError("Не указан ID искомого квеста!");
        }
        #endregion


        #region Публичные методы

        /// <summary>
        /// Добавить квест в список
        /// </summary>
        /// <param name="_id_quest">Id номер квеста</param>
        public void Add_quest(int _id_quest)
        {
            if (!Check_quest_in_active_list(_id_quest) && Quest_Data_array[_id_quest].Check_requirements_true)
            {
                Active_quest_list.Add(new Quest_SO());
                Active_quest_list[Active_quest_list.Count - 1] = Quest_Data_array[_id_quest];
                Add_quest_event.Invoke(Quest_Data_array[_id_quest]);

                Start_task_quest(_id_quest, 0);
                //Active_quest_list[Active_quest_list.Count - 1].Quest_UI = UI_quest_system.Singleton_Instance.Add_external_new_task();
                //Active_quest_list[Active_quest_list.Count - 1].Quest_UI.Add_quest_SO(Quest_Data_array[_id_quest]);
            }
            
        }

        /// <summary>
        /// Убрать квест из списка
        /// </summary>
        /// <param name="_id_quest">Id номер квеста</param>
        public void Remove_quest(int _id_quest)
        {
            if (Check_quest_in_active_list(_id_quest))
            {
                foreach (Quest_SO quest in Active_quest_list)
                {
                    if (Quest_Data_array[_id_quest] == quest)
                    {
                        Remove_quest_event.Invoke(quest);
                        //UI_quest_system.Singleton_Instance.Remove_new_task(quest.Quest_UI);
                        Active_quest_list.Remove(quest);
                        break;
                    }
                }
            }
                
        }

        /// <summary>
        /// Начать подзадачу
        /// </summary>
        /// <param name="_id_quest">Id номер квеста</param>
        /// <param name="_id_task_quest">Id номер подзадачи</param>
        public void Start_task_quest(int _id_quest, int _id_task_quest)
        {
            Start_task_event.Invoke(_id_quest, _id_task_quest);

        }

        /// <summary>
        /// Закончить подзадачу
        /// </summary>
        /// <param name="_id_quest">Id номер квеста</param>
        /// <param name="_id_task_quest">Id номер подзадачи</param>
        /// <param name="_win">Успешно завершили подзадачу?</param>
        public void End_task_quest(int _id_quest, int _id_task_quest, bool _win)
        {
                    End_task_event.Invoke(Quest_Data_array[_id_quest], _id_task_quest, _win);

                    Start_task_quest(_id_quest, _id_task_quest + 1);
                    /*
                    if (_win)
                        Active_quest_list[x].Quest_UI.Subtask_win_completed(_id_subtask_quest);
                    else
                        Active_quest_list[x].Quest_UI.Subtask_lose_completed(_id_subtask_quest);
                    */
        }



        /// <summary>
        /// Закончить квест как правильно выполненный
        /// </summary>
        /// <param name="_id_quest">Id номер квеста</param>
        public void End_win_quest(int _id_quest)
        {
            End_quest(_id_quest, true);
        }

        /// <summary>
        /// Закончить квест как правильно выполненный
        /// </summary>
        /// <param name="_id_quest">Данные квеста</param>
        public void End_win_quest(Quest_SO _quest_SO)
        {
            for (int x = 0; x < Quest_Data_array.Length; x++)
            {
                if (Quest_Data_array[x] == _quest_SO)
                {
                    End_win_quest(x);
                    break;
                }
            }
        }

        /// <summary>
        /// Закончить квест как проваленый
        /// </summary>
        /// <param name="_id_quest">Id номер квеста</param>
        public void End_lose_quest(int _id_quest)
        {
            End_quest(_id_quest, false);
        }
        #endregion
    }
}