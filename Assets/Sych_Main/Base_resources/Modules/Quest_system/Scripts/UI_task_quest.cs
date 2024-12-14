using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Sych_scripts
{
    [DisallowMultipleComponent]
    public class UI_task_quest : MonoBehaviour
    {
        #region Переменные
        [Tooltip("Отображение имени подзадачи")]
        [SerializeField]
        public TMP_Text Name_task = null;

        [Tooltip("Картинка показывающая, что выполнено правильно")]
        [SerializeField]
        GameObject Win_image = null;

        [Tooltip("Картинка показывающая, что задача провалена")]
        [SerializeField]
        GameObject Lose_image = null;

        UI_quest Active_UI_task_quest = null;//Текущая задача квеста

        #endregion


        #region Публичные методы

        /// <summary>
        /// Очистить (привести в первоначальный вид)
        /// </summary>
        public void Clear()
        {
            Win_image.SetActive(false);
            Lose_image.SetActive(false);
        }


        /// <summary>
        /// Передать данные о квесте
        /// </summary>
        /// <param name="_quest_SO">Данные квеста</param>
        /// /// <param name="_id_task">Id подзадачи</param>
        public void Add_quest_SO(Quest_SO _quest_SO, int _id_task, UI_quest _UI_task_quest)
        {
            if (Name_task)
                Name_task.text = _quest_SO.Description_Tasks_array[_id_task];

            Active_UI_task_quest = _UI_task_quest;
        }

        public void Add_quest_SO(Quest_SO _quest_SO, int _id_task)
        {
            if (Name_task)
                Name_task.text = _quest_SO.Description_Tasks_array[_id_task];
        }

        /// <summary>
        /// Как завершить задачу?
        /// </summary>
        /// <param name="_win">Правильно ? (в противном случае будет числиться проваленой)</param>
        public void Completed(bool _win)
        {
            if (_win)
                Win_image.SetActive(true);
            else
                Lose_image.SetActive(true);

            Invoke(nameof(Delay_check), 0.1f);
        }
        #endregion

        #region Методы
        void Delay_check()
        {
            if(Active_UI_task_quest)
            Active_UI_task_quest.Completed_task();
        }

        #endregion
    }
}